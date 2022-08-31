using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.ThrowPotions
{
	// TODO : Alchemy Bag accessory
	// Increases lifetime of potioned spells, increases debuff duration from spells, increases damage bonus from spells, and increases duraction of damage bonus

    public abstract class SpellThrowBase : clericProj
    {
		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 22;
			Projectile.timeLeft = 999;
			Projectile.friendly = true;

			clericEvil = true;
			SaferSafeSetDefaults();
		}

		public virtual void SaferSafeSetDefaults()
        {

        }

		public int SpellAura;

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage = Projectile.damage * 2;
			crit = false;
		}

		public override void AI()
		{
			Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
			Projectile.ai[0]++;

			if (Projectile.ai[0] > 20)
			{
				Projectile.velocity.Y += 0.3f;
				Projectile.velocity.X *= 0.97f;

				if (Projectile.velocity.X > 0)
				{
					Projectile.rotation += MathHelper.ToRadians((Projectile.ai[0] - 10) * 0.5f);
				}
				else
				{
					Projectile.rotation -= MathHelper.ToRadians((Projectile.ai[0] - 10) * 0.5f);
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Death();
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Death();
			return true;
		}

		void Death()
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, SpellAura, Projectile.damage, 0, Main.player[Projectile.owner].whoAmI);

			if (Main.netMode == NetmodeID.Server)
			{
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			for (var i = 0; i < 5; i++)
			{
				Gore g = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, new Vector2(2, 2).RotatedBy(MathHelper.ToRadians(90 * i)), GoreID.Smoke1);
				g.velocity = new Vector2(0, Main.rand.NextFloat(1.1f, 2.6f)).RotatedBy(MathHelper.ToRadians((360/5) * i + Main.rand.Next(-30, 31)));
				for (var e = 0; e < 4; e++)
				{
					Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 13);
					d.velocity = new Vector2(0, Main.rand.NextFloat(0.8f, 3)).RotatedByRandom(MathHelper.ToRadians(360));
				}
			}
			SoundEngine.PlaySound(SoundID.Item107, Projectile.Center);
		}
	}

	public abstract class SpellAuraBase : clericProj
    {
		public override void SafeSetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = Projectile.height = 96;
			Projectile.timeLeft = 120;
			Projectile.friendly = true;
			//Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 7;

			clericEvil = true;
			clericPotion = true;
			SaferSafeSetDefaults();
		}

		public int BuffType;
		public int BuffTime;

		public int DeathDustType;

		public virtual void SaferSafeSetDefaults()
		{

		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.ai[0] % 15 == 0)
			{
				return true;
			}
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage = Projectile.damage;
			target.AddBuff(BuffType, BuffTime);
			crit = false;

			target.GetGlobalNPC<excelNPC>().SpellCurse = 300;
		}

		public override void AI()
		{
			Projectile.ai[0]++;

			Projectile.position = Projectile.Center;
			/*
			if (Projectile.ai[0] % 15 == 0)
			{
				Projectile.scale = 1.1f;
			}
			Projectile.scale -= 0.01f;
			*/
			Projectile.scale = 1 + MathF.Sin(Projectile.ai[0] * 0.5f) / 15;
			Projectile.Center = Projectile.position;
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 40; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DeathDustType);
				d.alpha = 80;
			}
		}
	}
}
