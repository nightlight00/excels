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

namespace excels.Items.WeaponHeal.Holyiest
{
    internal class PhoenixScepter : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Calling of the Phoenix");
			Tooltip.SetDefault("Conjures a majestic phoenix to aid allies");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PhoenixBlast>();
			Item.shootSpeed = 3.7f;
			Item.noMelee = true;

			Item.mana = 10;
			healAmount = 10;
			healRate = 1.5f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}
	}

	public class PhoenixBlast : clericHealProj
    {
		public override void SetStaticDefaults()
		{
			//	Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 33;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 60;
			Projectile.timeLeft = 300;
			Projectile.alpha = 70;
			Projectile.extraUpdates = 3;
			Projectile.tileCollide = false;

			healPenetrate = -1;
		}

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

			if (Projectile.ai[0] >= 1)
            {
				Projectile.alpha += 20;
				if (Projectile.alpha > 255)
                {
					Projectile.Kill();
                }
            }

            for (var i = 0; i < 3; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(32), 64, 64, 6);
				d.velocity = -Projectile.velocity * Main.rand.NextFloat(0.6f, 0.9f);
				d.noGravity = true;
            }

			Dust wing1 = Dust.NewDustPerfect(Projectile.Center + new Vector2(28, 0).RotatedBy(Projectile.rotation + MathHelper.ToRadians(45)), 6);
			wing1.noGravity = true;
			wing1.scale = 1.8f;
			wing1.velocity = -Projectile.velocity * 0.8f;

			Dust wing2 = Dust.NewDustPerfect(Projectile.Center + new Vector2(28, 0).RotatedBy(Projectile.rotation + MathHelper.ToRadians(225)), 6);
			wing2.noGravity = true;
			wing2.scale = 1.8f;
			wing2.velocity = -Projectile.velocity * 0.8f;

			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 32);

			// 64 is the sprite size (here both width and height equal)
			const int HalfSpriteWidth = 60 / 2;
			const int HalfSpriteHeight = 60 / 2;

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
				if (k % 3 == 0)
				{
					Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
					Vector2 drawPos = (Projectile.oldPos[k] - new Vector2(DrawOffsetX, DrawOriginOffsetY) - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, 1, SpriteEffects.None, 0);
				}
			}

            return true;
		}
	}
}
