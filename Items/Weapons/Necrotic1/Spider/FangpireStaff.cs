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

namespace excels.Items.Weapons.Necrotic1.Spider
{
    internal class FangpireStaff : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Struck foe's life is stolen as they succumb to deadly toxins");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 43;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 44;
			Item.useTime = Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FangpireSpit>();
			Item.shootSpeed = 4;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 90);

			clericEvil = true;
			clericBloodCost = 8;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SpiderFang, 18)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class FangpireSpit : clericHealProj
    {
		//	public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SalamanderSpit}";
		public override void SetStaticDefaults()
		{
			//	Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
        {
			Projectile.width = Projectile.height = 16;
			Projectile.timeLeft = 240;
			Projectile.friendly = true;
			Projectile.extraUpdates = 3;

			clericEvil = true;
			canDealDamage = true;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 25; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
				d.noGravity = true;
				d.scale = Main.rand.NextFloat(1.4f, 1.6f);
				d.alpha = 150;
				d.velocity *= 1.4f;

				if (Main.rand.NextBool(4))
                {
					d.noGravity = false;
					d.scale *= 0.9f;
                }
			}
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.Poisoned, 300);
			target.AddBuff(BuffID.Venom, 180);
			if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy)
            {
				float rot = Main.rand.Next(360);
				for (var i = 0; i < 3; i++)
                {
					Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), target.Center, new Vector2(5).RotatedBy(MathHelper.ToRadians(90 + 120 * i)), ProjectileID.VampireHeal, 0, 0, Main.player[Projectile.owner].whoAmI);
					p.ai[0] = Main.player[Projectile.owner].whoAmI;
					p.ai[1] = 4;
				}
            }
			CreateWeb();
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			CreateWeb();
			return true;
        }

		private void CreateWeb()
        {
			Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FangWeb>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack * 2, Main.player[Projectile.owner].whoAmI);
			p.rotation = Main.rand.NextFloat(360);
		}

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (++Projectile.ai[0] < 10)
				return;

			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
			d.velocity = -Projectile.velocity * 0.3f;
			d.noGravity = true;
			d.scale = Main.rand.NextFloat(1.2f, 1.4f);
			d.alpha = 180;

			if (Main.rand.NextBool(3))
            {
				d.noGravity = false;
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
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, 1, SpriteEffects.None, 0);
			}

			return true;
		}
	}

	public class FangWeb : clericHealProj
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 50;
			Projectile.timeLeft = 60;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			Projectile.alpha = 80;

			clericEvil = true;
			canDealDamage = true;
		}

        public override bool? CanHitNPC(NPC target)
        {
			if (target.friendly || Projectile.ai[0] < 15 || Projectile.timeLeft < 10)
				return false;
			return true;
        }

        public override void AI()
        {
			Projectile.ai[0]++;
            Projectile.position = Projectile.Center;
			Projectile.scale = 1 + MathF.Sin(Projectile.ai[0] * 0.5f) / 15;
			Projectile.Center = Projectile.position;

			if (Projectile.timeLeft < 10)
				Projectile.alpha += 18;

		//	Projectile.rotation += MathHelper.ToRadians(9);
		}
	}
}
