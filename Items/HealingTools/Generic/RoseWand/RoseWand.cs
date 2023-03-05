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

namespace excels.Items.HealingTools.Generic.RoseWand
{
    internal class RoseWand : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Conjures a short-ranged healing bolt that can also cure poison");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<RoseWandBolt>();
			Item.shootSpeed = 6.4f;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 85);

			Item.mana = 15;
			healAmount = 4;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
        }

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.JungleRose)
				.AddIngredient(ItemID.RichMahogany, 8)
				.AddIngredient(ItemID.JungleSpores, 4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class RoseWandBolt : clericHealProj
    {
		public override void SetStaticDefaults()
		{
		//	Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
        {
			Projectile.width = Projectile.height = 8;
			Projectile.timeLeft = 50;
			Projectile.alpha = 0; // 255;

			healPenetrate = 2;
			canHealOwner = false;
			healUsesBuffs = true;
        }

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation();
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 10);
			Dust d = Dust.NewDustPerfect(Projectile.Center, 298);
			d.velocity = Projectile.velocity * -0.2f;
			d.noGravity = true;
			d.scale = 1.1f;
        }

        public override void BuffEffects(Player target, Player healer)
        {
			target.ClearBuff(BuffID.Poisoned);
        }

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 20; i++)
			{
				Vector2 vel = new Vector2(Main.rand.NextFloat(0.25f, 2.25f)).RotatedByRandom(MathHelper.ToRadians(360));
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 298);
				d.scale = Main.rand.NextFloat(1.2f, 1.4f);
				d.fadeIn = d.scale * 1.15f;
				d.noGravity = true;
				d.velocity = vel;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			float scale = 0.9f;
			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor); // * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
				scale -= 0.05f;
			}

			return true;
		}
	}
}
