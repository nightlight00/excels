using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.WeaponHeal.HardBar
{
    internal class HereticBreaker : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Boosts ally's offensive abilities are improved and allow critical strikes to heal themselves");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 42;
			Item.useTime = Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HereticBeam>();
			Item.shootSpeed = 2f;
			Item.noMelee = true;
			Item.mana = 10;
			Item.sellPrice(0, 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.TitaniumBar, 14)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class HereticBeam : clericHealProj
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 16;
			Projectile.timeLeft = 350;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 6;
			Projectile.penetrate = 3;
			//Projectile.usesIDStaticNPCImmunity
			buffConsumesPenetrate = true;
		}

        public override void AI()
        {
			if (++Projectile.ai[0] < 6*Projectile.extraUpdates)
				return;

			Dust d = Dust.NewDustPerfect(Projectile.Center, 156);
			d.noGravity = true;
			d.velocity = Projectile.velocity * 0.1f;
			d.scale = 1.125f;

			Dust ds1 = Dust.NewDustPerfect(Projectile.Center, 156);
			ds1.noGravity = true;
			ds1.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90)) * 0.55f;
			ds1.scale = 0.9f;

			Dust ds2 = Dust.NewDustPerfect(Projectile.Center, 156);
			ds2.noGravity = true;
			ds2.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-90)) * 0.55f;
			ds2.scale = 0.9f;
		}

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
				Dust d = Dust.NewDustPerfect(Projectile.Center, 156);
				d.noGravity = true;
				d.scale = 1;
				d.velocity = Main.rand.NextVector2Circular(3, 3);
            }
        }

        public override void BuffEffects(Player target, Player healer)
        {
			target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.HolySavagry>(), GetBuffTime(healer, 15));
        }
    }

	internal class SonicSyringe : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Boosts ally's defensive abilities are improved and grant a chance to block attacks");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 42;
			Item.useTime = Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.noUseGraphic = true;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SonicSyringeProj>();
			Item.shootSpeed = 2f;
			Item.noMelee = true;
			Item.mana = 10;
			Item.sellPrice(0, 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.AdamantiteBar, 14)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class SonicSyringeProj : clericHealProj
    {
		//public override string Texture => "excels/Items/WeaponHeal/HardBar/SonicSyringe";

		public override void SafeSetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear);
			Projectile.width = Projectile.height = 16;
			Projectile.timeLeft = 350;
			Projectile.penetrate = 3;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			//Projectile.usesIDStaticNPCImmunity
			buffConsumesPenetrate = true;
		}

		protected virtual float HoldoutRangeMin => 60f;
		protected virtual float HoldoutRangeMax => 130f;

        public override bool PreAI()
        {
			Player player = Main.player[Projectile.owner];
			int duration = player.itemAnimationMax; // Define the duration the projectile will exist in frames

			player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

			// Reset projectile time left if necessary
			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}

			Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

			float halfDuration = duration * 0.5f;
			float progress;

			// Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation.
			if (Projectile.timeLeft < halfDuration)
			{
				progress = Projectile.timeLeft / halfDuration;
			}
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}
			 
			//Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

			// Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
			
			if (Projectile.spriteDirection == -1)
			
			{
				// If sprite is facing left, rotate 45 degrees
				Projectile.rotation += MathHelper.ToRadians(45f);
			}
			else
			{
				// If sprite is facing right, rotate 135 degrees
				Projectile.rotation += MathHelper.ToRadians(135f);
			}

			return false;
			/*
			const int HalfSpriteWidth = 40 / 2;
			const int HalfSpriteHeight = 40 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
			*/
		}

		public override void BuffEffects(Player target, Player healer)
		{
			target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.HolySavagry>(), GetBuffTime(healer, 15));
		}
	}
}
