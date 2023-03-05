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
using System.Collections.Generic;

namespace excels.Items.HealingTools.Generic.PixieWand
{
	internal class PixieWand : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Creates a pixie replica that seeks out injured allies");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 5;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PixieEnergy>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.sellPrice(0, 1, 50);

			Item.mana = 10;
			healAmount = 11;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PixieDust, 15)
				.AddIngredient(ItemID.UnicornHorn)
				.AddIngredient(ItemID.Pearlwood, 20)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class PixieEnergy : clericHealProj
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 14;
			Projectile.timeLeft = 200;
			Projectile.alpha = 30;

			healPenetrate = 3;
		}

		float maxSpeed = 8;

		public override void AI()
		{
			Vector2 targetPos = Vector2.Zero;
			float targetHealth = 1000;
			bool target = false;
			for (int k = 0; k < 200; k++)
			{
				Player player = Main.player[k];
				float health = player.statLife;
				if (health < targetHealth && health < player.statLifeMax2 && player != Main.player[Projectile.owner])
				{
					targetHealth = health;
					targetPos = player.Center;
					target = true;
				}
			}
			if (target)
			{
				AdjustVelocity(targetPos);
			}

			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 55);
			d.noGravity = true;
			d.scale *= 0.7f;
			d.velocity = -Projectile.velocity * 0.4f;


			Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 2);
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 35);
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 20; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 55);
				d.noGravity = true;
				d.velocity *= Main.rand.NextFloat(0.8f, 1.8f);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, 0.8f, SpriteEffects.None, 0);
			}

			return true;
		}

		private void AdjustVelocity(Vector2 pos, float mult = 1f)
		{
			if (pos.X > Projectile.Center.X)
			{
				Projectile.velocity.X += 0.2f * mult;
				if (Projectile.velocity.X > maxSpeed) { Projectile.velocity.X = maxSpeed; }
			}
			else
			{
				Projectile.velocity.X -= 0.2f * mult;
				if (Projectile.velocity.X < -maxSpeed) { Projectile.velocity.X = -maxSpeed; }
			}
			if (pos.Y > Projectile.Center.Y - 40)
			{
				Projectile.velocity.Y += 0.1f * mult;
				if (Projectile.velocity.Y > maxSpeed) { Projectile.velocity.Y = maxSpeed; }
			}
			else
			{
				Projectile.velocity.Y -= 0.2f * mult;
				if (Projectile.velocity.Y < -maxSpeed) { Projectile.velocity.Y = -maxSpeed; }
			}
		}
	}
}
