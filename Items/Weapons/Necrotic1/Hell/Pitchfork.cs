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

namespace excels.Items.Weapons.Necrotic1.Hell
{
    class DevilPitchfork : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Devilish Pitchfork");
			Tooltip.SetDefault("Conjures twin fireballs \nFireballs create fiery auras on collision that restore your health"); // Conjures a lava geyser at the cursor \nTODO : Better attack = fires two firballs (like betsy wrath) that each create fiery auras on hit \n TODO (cont) : auras steal life from enemies");
			Item.staff[Item.type] = true;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 35;
			Item.width = Item.height = 66;
			Item.useTime = Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.8f;
			Item.value = 10000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item88;
			Item.shootSpeed = 8.6f;
			Item.shoot = ModContent.ProjectileType<PitchforkFire>();
			Item.noMelee = true;
			Item.sellPrice(0, 1, 10);

			clericEvil = true;
			clericBloodCost = 8;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity * new Vector2(1.15f, 1), type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity * new Vector2(0.85f, 1), type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 16)
				//.AddIngredient(ItemID.FallenStar)
				.AddTile(TileID.Anvils)
				.Register();
		}

	}
	public class PitchforkFire : clericHealProj
	{
		public override void SetStaticDefaults()
		{
			//	Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 10;
			Projectile.timeLeft = 300;
			Projectile.alpha = 0; // 255;
			Projectile.friendly = true;
			Projectile.penetrate = -1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 22;

			healPenetrate = 1;
			canDealDamage = true;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Aura();
			return Projectile.tileCollide;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.OnFire, 360);
			if (Projectile.ai[1] == 1)
            {
				Projectile p = Projectile.NewProjectileDirect(target.GetSource_FromAI(), Projectile.Center, new Vector2(6, 6).RotatedByRandom(MathHelper.ToRadians(360)), ProjectileID.VampireHeal, 0, 0, Main.player[Projectile.owner].whoAmI);
				p.ai[0] = Main.player[Projectile.owner].whoAmI;
				p.ai[1] = 1 + Main.player[Projectile.owner].GetModPlayer<excelPlayer>().healBonus;
			}
			Aura();
        }

		public virtual void Aura()
        {
			if (Projectile.ai[1] == 1)
				return;

			for (var i = 0; i < 20; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
				d.scale = Main.rand.NextFloat(1.25f, 1.5f);
				d.noGravity = true;
				d.velocity *= Main.rand.NextFloat(1.7f, 2.15f);
            }

			Projectile.alpha = 255;
			Projectile.position = Projectile.Center;
			Projectile.width = Projectile.height = 40;
			Projectile.Center = Projectile.position;
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 60;
			Projectile.damage = (int)(Projectile.damage * 0.75f);
			SoundEngine.PlaySound(SoundID.Item34, Projectile.Center);

			Projectile.tileCollide = false;
			Projectile.ai[1] = 1;
        }

        public override void AI()
		{
			if (Projectile.ai[1] == 1)
            {
				for (var i = 0; i < 4; i++)
                {
					Dust d2 = Dust.NewDustPerfect(Projectile.Center + new Vector2(20).RotatedByRandom(MathHelper.ToRadians(360)), 6);
					d2.noGravity = true;
                }
				return;
            }

			Projectile.rotation = Projectile.velocity.ToRotation();

			Dust d = Dust.NewDustPerfect(Projectile.Center, 6);
			d.velocity = Projectile.velocity * -0.2f;
			d.scale *= 1.25f;
			d.noGravity = true;
			
			if (++Projectile.ai[0] > 10)
			{
				Projectile.velocity.Y += 0.17f;
				Projectile.velocity.X *= 0.97f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			float scale = 1;
			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor); // * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
				scale -= 0.06f;
			}

			if (Projectile.ai[1] == 1)
            {
				return false;
            }

			return true;
		}

	}

	class FirePillar : clericProj
    {
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 50;
			Projectile.penetrate = -1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 90);
		}

		public override void AI()
		{
			Projectile.height += 3;
			Projectile.position.Y -= 3;

			for (var i = 0; i < 2; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, -3);
				d.noGravity = true;
				d.scale = Main.rand.NextFloat() + 1.7f;
				d.velocity.X *= 0.3f;
			}
		}
	}

	class DevilPitchfork2 : clericProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Devilish Pitchfork");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SafeSetDefaults()
		{
			Projectile.height = 84;
			Projectile.width = 30;
			Projectile.timeLeft = 50;
			Projectile.penetrate = 6;
			Projectile.tileCollide = false;
			Projectile.friendly = true;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.localNPCHitCooldown = 20;
		}
		/*
        public override bool PreDraw(ref Color lightColor)
        {
		  //Redraw the Projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(Main.ProjectileTexture[Projectile.type].Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Main.ProjectileTexture[Projectile.type], drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		*/
		public override void AI()
		{
			if (Projectile.ai[0] == 1)
            {
				Projectile.frame = 1;
				Projectile.timeLeft -= 1;
				Projectile.alpha = 100;
				Lighting.AddLight(Projectile.Center, new Vector3(1.23f, .26f, 2.20f));
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height / 2, 27);
            }
			else {
				Projectile.frame = 0;
				Projectile.alpha += 5;
				Lighting.AddLight(Projectile.Center, new Vector3(2.55f, 1.23f, .15f));
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height / 2, 6);
				d.scale += 0.25f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.ai[0] == 1)
			{
				target.AddBuff(BuffID.ShadowFlame, 120);
				for (int i = 0; i < 15; i++)
				{
					Dust dst = Dust.NewDustDirect(target.position, target.width, target.height, 27, Main.rand.NextFloat(-4, 4), -5);
					dst.scale = Main.rand.NextFloat(0.9f, 1.3f);
				}
				return;
			}
			SoundEngine.PlaySound(SoundID.Item100, Projectile.Center);
			for (int i = 0; i < 15; i++)
			{
				Dust dst = Dust.NewDustDirect(target.position, target.width, target.height, 6, Main.rand.NextFloat(-4, 4), -5);
				dst.scale = Main.rand.NextFloat(1.1f, 1.6f);
			}
			var player = Main.player[Projectile.owner];
			for (var i = 0; i < 2; i++)
			{
				Vector2 pos = new Vector2(target.position.X + Main.rand.NextFloat(-Projectile.width * 2, Projectile.width * 2), target.position.Y + (target.height * 1.5f));
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pos, new Vector2(0, -15), ModContent.ProjectileType<DevilPitchfork2>(), (int)(Projectile.damage * 0.66f), Projectile.knockBack, player.whoAmI, 1);
				for (int d = 0; d < 15; d++)
				{
					Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 0.5f);
					Dust dst = Dust.NewDustDirect(pos + (speed * 3), target.width, target.height, 27, 0, 0);
					dst.scale = Main.rand.NextFloat(1.1f, 1.6f);
					dst.noGravity = true;
				}
			}
		}
	}
}
