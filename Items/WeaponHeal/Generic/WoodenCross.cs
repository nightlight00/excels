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

namespace excels.Items.WeaponHeal.Generic
{
    #region Wooden Cross
    internal class WoodenCross : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Conjures a radiant bolt that generates a Light Cross\nThe Light Cross lingers and can heal allies three times\nOnly one Light Cross can exist at a time");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			//Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.shoot = ModContent.ProjectileType<GenericCross>();
			Item.shootSpeed = 3.5f;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 0, 50);

			Item.mana = 10;
			healAmount = 4;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile p = CreateHealProjectile(player, source, position, velocity, ModContent.ProjectileType<HealingBolt>(), damage, knockback, ai1: Item.shoot);
			p.timeLeft = 60;
			return false;
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Wood, 18)
				//.AddIngredient(ItemID.FallenStar)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class HealingBolt : clericHealProj
    {
        public override void SetStaticDefaults()
        {
		//	Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void SafeSetDefaults()
        {
			Projectile.width = Projectile.height = 12;
			Projectile.timeLeft = 100;
			Projectile.alpha = 0; // 255;

			healPenetrate = 1;
        }

		bool Start = true;
		Vector2 mousePos = Vector2.Zero;

        public override void AI()
        {
			if (Start)
            {
				if (Main.myPlayer == Main.player[Projectile.owner].whoAmI)
					mousePos = Main.MouseWorld;
				Start = false;
            }

			if (Vector2.Distance(Projectile.Center, mousePos) < 10)
				Projectile.Kill();
			Projectile.rotation = Projectile.velocity.ToRotation();
			Dust d = Dust.NewDustPerfect(Projectile.Center, 204);
			d.velocity = Projectile.velocity * -0.2f;
			d.scale *= 1.25f;
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
			for (var p = 0; p < Main.maxProjectiles; p++)
            {
				Projectile proj = Main.projectile[p];
				if (proj.active)
                {
					if (proj.type == Projectile.ai[1] && proj.owner == Main.player[Projectile.owner].whoAmI)
						proj.Kill();
                }
            }
			Projectile proj2 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, (int)Projectile.ai[1], 0, 0, Main.player[Projectile.owner].whoAmI);
			proj2.GetGlobalProjectile<excelProjectile>().healStrength = Projectile.GetGlobalProjectile<excelProjectile>().healStrength;
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			float scale = 0.9f + (0.25f * Projectile.ai[0]);
			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor); // * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
				scale -= 0.04f + (0.01f * Projectile.ai[0]);
			}

			return true;
		}
		
	}

	public class GenericCross : clericHealProj
    {
        public override void SafeSetDefaults()
        {
			Projectile.width = 34;
			Projectile.height = 44;
			Projectile.timeLeft = 1000;
			Projectile.alpha = 50; // 255;
			Projectile.tileCollide = false;

			healPenetrate = 3;
			canHealOwner = true;
			timeBetweenHeal = 120;
		}

        public override void AI()
        {
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 50);
			Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.4f);

			for (var i = 0; i < 3; i++)
            {
				Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2CircularEdge(50, 50), 204);
				d.noLight = true;
				d.noGravity = true;
				d.velocity = Vector2.Zero;
            }
			Projectile.ai[0] += 1.35f * (4 - healPenetrate);
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
				Main.EntitySpriteDraw(texture, drawPos+new Vector2(10).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]+(360/healPenetrate*i))), null, Projectile.GetAlpha(lightColor) * 0.33f, Projectile.rotation, drawOrigin, 1, SpriteEffects.None, 0);
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
    
	#endregion

	#region Thrown Health Pot
	public class ThrowableHealthPotion : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Throws a health potion that shatters on collision");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<ThrownHealthPotion>();
			Item.shootSpeed = 9;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.buyPrice(0, 0, 80);

			Item.mana = 5;
			healAmount = 30;
			healRate = 0;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
        }
    }

	public class ThrownHealthPotion : clericHealProj
    {
		public override string Texture => "excels/Items/WeaponHeal/Generic/ThrowableHealthPotion";

        public override void SafeSetDefaults()
        {
			Projectile.width = Projectile.height = 22;
			Projectile.timeLeft = 999;
			Projectile.friendly = true;

			healPenetrate = 999;
			clericEvil = false;
			canDealDamage = false;
			canHealOwner = false;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Break();
			return false;
        }

        public virtual void Break()
        {
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 32);
			for (var i = 0; i < 15; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 266);
				
            }
			for (var e = 0; e < 20; e++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 13);
				d.velocity = new Vector2(0, Main.rand.NextFloat(0.8f, 3)).RotatedByRandom(MathHelper.ToRadians(360));
			}
			SoundEngine.PlaySound(SoundID.Item107, Projectile.Center);
			Projectile.Kill();
		}

        public override void BuffEffects(Player target, Player healer)
        {
			Break();
        }

        public override void AI()
        {
			BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 32);
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
		}
    }
	#endregion

	#region Syringe
	public class Syringe : ClericDamageItem
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NurseSyringeHeal}";

        public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("e");
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

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
				{
					line2.Text = $"Restores {3 + Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().healBonus} health \n'Yowch!'";
				}
			}
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
	#endregion

}
