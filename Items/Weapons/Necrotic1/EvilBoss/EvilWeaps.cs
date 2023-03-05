using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace excels.Items.Weapons.Necrotic1.EvilBoss
{
    #region Bloodletting Eye
    internal class BloodlettingEye : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Conjures a life-stealing blood bolt\nEvery 3 successful hits conjures an additional 2 blood bolts");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 27;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 44;
			Item.useTime = Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<BloodBolt>();
			Item.shootSpeed = 11f;
			Item.noMelee = true;

			clericEvil = true;
			clericBloodCost = 8;
			Item.sellPrice(0, 0, 75);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CrimtaneBar, 8)
				.AddIngredient(ItemID.TissueSample, 12)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.GetModPlayer<BloodPlayer>().bloodlettingEyePoints >= 3)
            {
				for (var i = 0; i < 3; i++)
                {
					Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(14)) * Main.rand.NextFloat(0.8f, 1), type, (int)(damage * 0.8f), knockback, player.whoAmI);
                }
				player.GetModPlayer<BloodPlayer>().bloodlettingEyePoints -= 3;
				return false;
            }
			Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(2)), type, damage, knockback, player.whoAmI);
			return false;
        }
    }

	public class BloodBolt : clericHealProj
    {
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 16;
			Projectile.timeLeft = 76;
			Projectile.alpha = 80;

			canDealDamage = true;
			healPenetrate = 1;
			//buffConsumesPenetrate = true;
			healRate = -1;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Main.player[Projectile.owner].GetModPlayer<BloodPlayer>().bloodlettingEyePoints++;

			if (!target.friendly && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
			{
				Player player = Main.player[Projectile.owner];
				int healAmount = Math.Clamp(damage / 7, 1, 6);
				player.statLife += healAmount;
				if (player.statLife > player.statLifeMax2)
					player.statLife = player.statLifeMax2;
				player.HealEffect(healAmount);
			}
		}

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation();

			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
			d.velocity = Projectile.velocity * 0.5f;
			d.scale = 0.9f + Main.rand.NextFloat() * 0.3f;
			d.noGravity = true;
			if (Main.rand.Next(8) <= 5)
            {
				Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
				d2.velocity += Projectile.velocity * 0.1f + new Vector2(0, 1);
				d2.scale *= Main.rand.NextFloat(0.7f, 0.88f);
			}

			Lighting.AddLight(Projectile.Center, Color.MediumVioletRed.ToVector3() * 0.7f);

			// Center sprite on hitbox
			const int HalfSpriteWidth = 38 / 2;
			const int HalfSpriteHeight = 18 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height  * 0.5f);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = (Color.Red * 0.33f) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, texture.Frame(1, 2, 0, 1), color, Projectile.rotation, drawOrigin, 1f -(k*0.03f), SpriteEffects.None, 0);
			}
			// new Vector2(Projectile.width * 0.5f + DrawOffsetX, Projectile.height * 0.5f + DrawOriginOffsetY)
			return true;
		}

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 15; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
				d.velocity = Main.rand.NextVector2Circular(0.2f, 0.2f) * Main.rand.NextFloat(2, 3.5f);
				d.noGravity = true;
				d.scale = Main.rand.NextFloat(1.2f, 1.5f);

				Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 183);
				d2.velocity = Projectile.velocity * Main.rand.NextFloat(0.5f, 0.8f);
				d2.noGravity = true;
				d2.scale = Main.rand.NextFloat(1.4f, 1.7f);
			}
        }
    }

	public class BloodPlayer : ModPlayer
    {
		public int bloodlettingEyePoints = 0;
    }

    #endregion

    #region Devcouring Rod
    internal class DevouringRod : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Vile spit steals mana from struck foes\nIgnores 5 points of enemy defense");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 44;
			Item.useTime = Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<VileSpit>();
			Item.shootSpeed = 9.6f;
			Item.noMelee = true;
			Item.ArmorPenetration = 5;

			clericEvil = true;
			clericBloodCost = 5;
			Item.sellPrice(0, 0, 75);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DemoniteBar, 8)
				.AddIngredient(ItemID.ShadowScale, 12)
				.AddTile(TileID.Anvils)
				.Register();
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (var i = 0; i < 3+Main.rand.Next(3); i++)
            {
				Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(7 + 4 * i)) * Main.rand.NextFloat(0.9f, 1.1f), type, damage, knockback, player.whoAmI);
            }
			return false;
        }
    }

	public class VileSpit : clericHealProj
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 8;
			Projectile.timeLeft = 76;
			Projectile.alpha = 255;

			canHealOwner = true;
			canDealDamage = true;
			healPenetrate = 1;
			//buffConsumesPenetrate = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 180);

			if (!target.friendly && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
			{
				Player player = Main.player[Projectile.owner];
				int manaAmount = Math.Clamp(damage / 3, 1, 9);
				player.statMana += manaAmount;
				if (player.statMana > player.statManaMax2)
					player.statMana = player.statManaMax2;
				player.ManaEffect(manaAmount);
			}
		}

		public override void AI()
		{
			for (var i = 0; i < 1 + Main.rand.Next(1); i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
				d.velocity = Projectile.velocity * 0.3f;
				d.scale = Main.rand.NextFloat(1.3f, 1.5f);
				d.noGravity = true;
				d.alpha = 60;
			}
			
			if (Main.rand.Next(8) <= 3)
			{
				Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
				d2.velocity += Projectile.velocity * 0.1f + new Vector2(0, 1);
				d2.scale *= Main.rand.NextFloat(0.6f, 0.75f);
				d2.alpha = 60;
			}
		}

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
				d.velocity = Main.rand.NextVector2Circular(0.25f, 0.25f) * Main.rand.NextFloat(7f, 10.5f);
				d.scale = Main.rand.NextFloat(1.6f, 1.9f);
				d.noGravity = true;
				d.alpha = 40;
			}
        }
    }
    #endregion
}
