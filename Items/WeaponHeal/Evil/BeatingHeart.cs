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

namespace excels.Items.WeaponHeal.Evil
{

    internal class BeatingHeart : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beating Heart");
			Tooltip.SetDefault("Rapidly pulses seeking hearts");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 13;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SeekingHeart>();
			Item.shootSpeed = 7.8f;
			Item.noMelee = true;
			Item.sellPrice(0, 1);

			Item.mana = 8;
			healAmount = 4;
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(11));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
        }
        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.ShatteredHeartbeat>(), 6)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class SeekingHeart : clericHealProj
    {
		public override string Texture => $"Terraria/Images/Item_{ItemID.Heart}";

        public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 12;
			Projectile.timeLeft = 70;
			Projectile.alpha = 100; // 255;

			canDealDamage = false;
			healPenetrate = 1;
			clericEvil = true;
		}

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
				Projectile.velocity = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * 7.8f;
			}

			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 20);

			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90);

			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 183);
			d.noGravity = true;
			d.velocity = -Projectile.velocity * 0.4f;
		}

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 15; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 183);
				d.noGravity = true;
				d.scale = 1.3f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				if (k % 2 == 0)
				{
					Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
					Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, 1, SpriteEffects.None, 0);
				}
			}

			return true;
		}
	}
}
