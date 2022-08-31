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

namespace excels.Items.Weapons.Radiant1
{
    internal class StaffofPearls : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Staff of Pearls");
			Tooltip.SetDefault("'Luck not included'");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 13;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 25;
			Item.reuseDelay = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 0;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SmallPearl>();
			Item.shootSpeed = 7.25f;
			Item.noMelee = true;

			clericRadianceCost = 7;
			Item.knockBack = 2.5f;
			Item.sellPrice(0, 0, 1, 20);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.WhitePearl)
				.AddIngredient(ItemID.FossilOre, 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class SmallPearl : clericProj
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
        {
			Projectile.width = Projectile.height = 18;
			Projectile.timeLeft = 200;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				if (k % 2 == 0) {
					Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.EntitySpriteDraw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
				} 
			} 

			return true;
		}

		public override void AI()
        {
			if (Collision.SolidTiles(Projectile.position + new Vector2(4, -2), Projectile.width - 8, Projectile.height + 4))
			{
				if (Projectile.velocity.Y > 0)
                {
					Projectile.position.Y -= 0.15f;
                }
				else
                {
					Projectile.position.Y += 0.1f;
                }
				Projectile.velocity.Y = -Projectile.velocity.Y * 0.8f;
			}

			if (Collision.SolidTiles(Projectile.position + new Vector2(-4, 4), Projectile.width + 8, Projectile.height - 8))
			{
				Projectile.velocity.X = -Projectile.velocity.X;
			}

			if (++Projectile.ai[0] > 11)
            {
				Projectile.velocity.Y += 0.145f;
            }
			Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 2);
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 15; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 236);
				d.scale = 0.9f;
				d.velocity *= 0.75f;
				if (!Main.rand.NextBool(3))
                {
					d.noGravity = true;
					d.scale += Main.rand.NextFloat(0.3f, 0.55f);
					d.velocity += Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * 0.5f;
                }
            }
        }
    }
}
