using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;


namespace excels.Items.HealingTools.Consumable
{
	public class Syringe : ClericDamageItem
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NurseSyringeHeal}";

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Yowch!'");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<SyringeThrown>();
			Item.shootSpeed = 9;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.sellPrice(0, 0, 0, 10);

			healAmount = 3;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(50)
				.AddIngredient(ItemID.SilverBar, 2)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(50)
				.AddIngredient(ItemID.TungstenBar, 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class SyringeThrown : clericHealProj
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NurseSyringeHeal}";

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 8;
			Projectile.timeLeft = 999;
			Projectile.friendly = true;

			healPenetrate = 999;
			clericEvil = false;
			canDealDamage = false;
			canHealOwner = false;
		}

		private void SetVisualOffsets()
		{
			// 32 is the sprite size (here both width and height equal)
			const int HalfSpriteWidth = 10 / 2;
			const int HalfSpriteHeight = 32 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (var i = 0; i < 14; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 11);
				d.scale = 0.7f;
				d.velocity += Projectile.velocity * 0.2f;
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			Break();
			return false;
		}

		public virtual void Break()
		{
			Projectile.Kill();
		}

		public override void BuffEffects(Player target, Player healer)
		{
			Break();
		}

		public override void AI()
		{
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 12, false);
			Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
			Projectile.ai[0]++;

			if (Projectile.ai[0] > 16)
			{
				Projectile.velocity.Y += 0.24f;
				Projectile.velocity.X *= 0.97f;

				if (Projectile.velocity.X > 0)
				{
					Projectile.rotation += MathHelper.ToRadians((Projectile.ai[0] - 10) * 0.25f);
				}
				else
				{
					Projectile.rotation -= MathHelper.ToRadians((Projectile.ai[0] - 10) * 0.25f);
				}
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			}
			SetVisualOffsets();
		}
	}
}
