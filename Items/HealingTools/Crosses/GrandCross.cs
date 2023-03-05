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
using excels.Buffs.HealOverTime;
using excels.Items.Materials.Ores;

namespace excels.Items.HealingTools.Crosses
{
	internal class GrandCross : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Conjures a radiant bolt that generates a Holy Cross\nThe Holy Cross lingers and can heal allies five times\nOnly one Holy Cross can exist at a time");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.shoot = ModContent.ProjectileType<PristineCross>();
			Item.shootSpeed = 4.2f;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 80);

			Item.mana = 10;
			healAmount = 6;
			healRate = 1;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile p = CreateHealProjectile(player, source, position, velocity, ModContent.ProjectileType<HealingBolt>(), damage, knockback, ai1: Item.shoot);
			p.timeLeft = 90;
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<WoodenCross>())
				.AddIngredient(ModContent.ItemType<MysticCrystal>(), 8)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class PristineCross : clericHealProj
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 58;
			Projectile.timeLeft = 1500;
			Projectile.alpha = 50; // 255;
			Projectile.tileCollide = false;

			healPenetrate = 5;
			canHealOwner = true;
			timeBetweenHeal = 100;
		}

		public override void AI()
		{
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 65);
			Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.65f);

			for (var i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2CircularEdge(65, 65), 204);
				d.noLight = true;
				d.noGravity = true;
				d.velocity = Vector2.Zero;
			}
			Projectile.ai[0] += 2.5f * (6 - healPenetrate);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, 1, SpriteEffects.None, 0);

			for (var i = 0; i < healPenetrate; i++)
			{
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(14).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] + (360 / healPenetrate * i))), null, Projectile.GetAlpha(lightColor) * 0.33f, Projectile.rotation, drawOrigin, 1, SpriteEffects.None, 0);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 20; i++)
			{
				Vector2 vel = new Vector2(Main.rand.NextFloat(0.25f, 2.25f)).RotatedByRandom(MathHelper.ToRadians(180));
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 204);
				d.scale = Main.rand.NextFloat(1.15f, 1.35f);
				d.fadeIn = d.scale * 1.15f;
				d.velocity = vel;
			}
		}
	}
}
