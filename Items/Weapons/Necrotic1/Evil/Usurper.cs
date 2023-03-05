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
using excels.Items.HealingTools.Crosses;

namespace excels.Items.Weapons.Necrotic1.Evil
{
    internal class UsurperCross : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbra Cross");
			Tooltip.SetDefault("Conjures twin shadow healing bolts \nHealing allies also heals you");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 34;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<UsurperBolt>();
			Item.shootSpeed = 7f;
			Item.noMelee = true;
			Item.damage = 17;
			Item.knockBack = 5;
			Item.sellPrice(0, 1, 15);

			clericBloodCost = 3;
			clericEvil = true;

			healAmount = 2;
			healRate = 0.5f;
		//	heal
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CreateHealProjectile(player, source, position, velocity.RotatedBy(MathHelper.ToRadians(7)), type, damage, knockback);
			CreateHealProjectile(player, source, position, velocity.RotatedBy(MathHelper.ToRadians(-7)), type, damage, knockback);
			return false;
        }

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<WoodenCross>())
				.AddIngredient(ItemID.DemoniteBar, 7)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<WoodenCross>())
				.AddIngredient(ItemID.CrimtaneBar, 7)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class UsurperBolt : clericHealProj
	{
		public override void SetStaticDefaults()
		{
	//		Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 19;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 12;
			Projectile.timeLeft = 80;
			Projectile.alpha = 0; // 255;
			Projectile.CanBeReflected();

			canDealDamage = true;
			healPenetrate = 1;
			
			clericEvil = true;
		}
		
        public override void PostHealEffects(Player target, Player healer)
        {
			int healAmount = (int)(2 + (healer.GetModPlayer<excelPlayer>().healBonus * healRate)) / 2;
			healer.HealEffect(healAmount);
			healer.statLife += healAmount;
			if (healer.statLife > healer.statLifeMax2)
            {
				healer.statLife = healer.statLifeMax2;
            }
        }

        public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 10);
			Dust d = Dust.NewDustPerfect(Projectile.Center, 27);
			d.velocity = Projectile.velocity * -0.2f;
			d.scale = 0.94f;
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 20; i++)
			{
				Vector2 vel = new Vector2(Main.rand.NextFloat(0.25f, 2.25f)).RotatedByRandom(MathHelper.ToRadians(180));
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 27);
				d.scale = Main.rand.NextFloat(1.05f, 1.15f);
				d.fadeIn = d.scale * 1.15f;
				d.velocity = vel;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			float scale = 1.15f;
			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor); // * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
				scale -= 0.04f;
			}

			return true;
		}

	}
}
