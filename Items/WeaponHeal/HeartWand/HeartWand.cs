using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace excels.Items.WeaponHeal.HeartWand
{
    internal class HeartWand : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HeartWandProjectile>();
			Item.shootSpeed = 3.2f;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 85);

			Item.mana = 7;
			healAmount = 2;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.LifeCrystal)
				.AddIngredient(ItemID.GoldBar, 10)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.LifeCrystal)
				.AddIngredient(ItemID.PlatinumBar, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }

    internal class HeartWandProjectile : clericHealProj {

		public override void SetStaticDefaults()
		{
			//	Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 20;
			Projectile.timeLeft = 120;
			Projectile.alpha = 40;
			Projectile.extraUpdates = 2;

			healPenetrate = 2;
			canHealOwner = false;
			healUsesBuffs = true;
		}

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
			HealCollision(Main.LocalPlayer, Main.player[Projectile.owner]);

			if (Main.rand.NextBool())
            {
				int dustType = 183;
				if (Main.rand.NextBool(3))
					dustType = 204;

				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType);
				d.velocity = Projectile.velocity * 0.3f;
				d.noGravity = true;
				d.scale = Main.rand.NextFloat(1.2f, 1.5f);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 24; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 183);
				d.velocity = Main.rand.NextVector2Circular(1.3f, 1.3f) + Projectile.velocity * Main.rand.NextFloat(0.1f, 0.3f);
				d.noGravity = true;
				d.scale = Main.rand.NextFloat(1.7f, 1.9f);
			}
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Color.White * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, 1f - (k * 0.09f), SpriteEffects.None, 0);
			}
			return true;
		}
	}
}
