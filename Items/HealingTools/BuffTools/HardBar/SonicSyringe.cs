using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.HealingTools.BuffTools.HardBar
{
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
