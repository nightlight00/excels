using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.Necrotic1.Dungeon
{
	internal class SpinalCrucifix : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Conjures crossbones that construct a crucifix on kills" +
							 "\nThe crucifix damages enemies and supports allies" +
                             "\n'Rattle 'em bones!'"); // \nStanding near the cross increases damage");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 42;
			Item.useTime = Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CrucifixCrossbone>();
			Item.shootSpeed = 11f;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 90);

			clericEvil = true;
			clericBloodCost = 10;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Bone, 55)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class CrucifixCrossbone : clericProj
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 18;
			Projectile.timeLeft = 80;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.penetrate = 3;

			clericEvil = true;
		}

        public override void AI()
        {
			if (Projectile.velocity.X > 0)
            {
				Projectile.rotation -= 6;
            }
			else
            {
				Projectile.rotation += 6;
			}
			if (Main.rand.Next(4) <= 2)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 26);
				d.noGravity = true;
				d.velocity = Projectile.velocity * 0.66f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.damage = (int)(Projectile.damage * 0.85f);
			if (target.life <= 0)
            {
				Projectile.NewProjectile(Projectile.GetSource_None(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BoneCrucifix>(), 0, 0, Main.player[Projectile.owner].whoAmI);
            }
        }
    }

	public class SkeletonCurse : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Calcium Curse");
			Description.SetDefault("Increased knockback and damage, plus recieve less damage when hit");

			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
		}

        public override void Update(Player player, ref int buffIndex)
        {
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetKnockback(DamageClass.Generic) += 0.5f;
			player.endurance += 0.05f;
        }
    }

	public class BoneCrucifix : clericHealProj
    {
		public override void SafeSetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 46;
			Projectile.timeLeft = 320;
			Projectile.friendly = true;
			Projectile.penetrate = -1;

			canDealDamage = false;
			canHealOwner = true;
			clericEvil = true;
		}

		int stolenAmount = 0;

		public override void AI()
        {
			BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 160);
			if (Projectile.timeLeft % 80 == 39)
			{
				for (var i = 0; i < 15; i++)
				{
					Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
					Dust d = Dust.NewDustPerfect(Projectile.Center - new Vector2(0, 4), 26, speed * 2.4f); //204
																					   //d.velocity *= 0;
					d.fadeIn = 1.16f;
					d.noGravity = true;
				}
				for (var i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (Vector2.Distance(npc.Center, Projectile.Center) < 160 && npc.active && npc.lifeMax > 5 && !npc.friendly)
					{
						if (!heallist.Contains(npc.whoAmI) && stolenAmount < 16)
						{
							Projectile.NewProjectile(Projectile.GetSource_None(), npc.Center, new Vector2(4, 4).RotatedByRandom(300), ProjectileID.VampireHeal, 0, 0, Main.player[Projectile.owner].whoAmI, Main.player[Projectile.owner].whoAmI, 2);
							heallist.Add(npc.whoAmI);
							stolenAmount += 2;
						}

						npc.life -= 30;
						npc.HitEffect(0, 30);
						Main.player[Projectile.owner].addDPS(30);
						CombatText.NewText(npc.getRect(), CombatText.DamagedHostile, 30);

						npc.checkDead();
						if (npc.life > 0)
                        {
							SoundEngine.PlaySound(npc.HitSound.Value, npc.Center);
                        }
					}
				}
			}

			for (var i = 0; i < 4; i++)
			{
				Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
				Dust d = Dust.NewDustPerfect(Projectile.Center + speed * 160, 26, speed * 0); //204
				d.noGravity = true;
			}
		}

        public override void BuffEffects(Player target, Player healer)
        {
			target.AddBuff(ModContent.BuffType<SkeletonCurse>(), 2);
        }

        public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 15; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 26);
				d.velocity = new Vector2(Main.rand.NextFloat(-2, 2), -2 - Main.rand.NextFloat());
				d.scale *= Main.rand.NextFloat(1.05f, 1.15f);
			}
		}
    }



	internal class TideClamp : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Fires tidal waves that create minature tide hearts on hit \nTide hearts can heal but burst if left alone for too long");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 27;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 42;
			Item.useTime = Item.useAnimation = 23;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TidePulse>();
			Item.shootSpeed = 9.6f;
			Item.noMelee = true;
			Item.sellPrice(0, 1, 30);

			clericRadianceCost = 6;
		}
	}

	public class TidePulse : clericProj
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 3;
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 24;
			Projectile.timeLeft = 320;
			Projectile.friendly = true;
			Projectile.alpha = 80;
		}

		public override void AI()
		{
			for (var i = 0; i < 2; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 33);
				d.noGravity = true;
				d.scale *= 1.23f;
				d.velocity *= -0.8f;
			}
			if (++Projectile.ai[0] > 14)
			{
				Projectile.velocity.Y += 0.26f;
				Projectile.velocity.X *= 0.977f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (++Projectile.frameCounter > 3)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame > 2)
				{
					Projectile.frame = 0;
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (var i = 0; i < 3; i++)
			{
				Projectile.NewProjectile(Projectile.GetSource_None(), Projectile.Center, new Vector2(3, 3).RotatedBy(MathHelper.ToRadians(120 * i - 15)), ModContent.ProjectileType<TidalHeart>(), Projectile.damage * 2, Projectile.knockBack / 3, Main.player[Projectile.owner].whoAmI);
			}
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 15; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 33);
				d.noGravity = true;
				d.velocity = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
				d.scale *= 1.25f;
			}
		}
	}

	public class TidalHeart : clericHealProj
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 14;
			Projectile.timeLeft = 160;
			Projectile.friendly = true;
			Projectile.alpha = 80;
			Projectile.penetrate = -1;

			canDealDamage = false;
			clericEvil = true;
			canHealOwner = true;
			healPenetrate = 1;

			healRate = 0.5f;
			healPower = 3;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

		public override void AI()
		{
			Projectile.GetGlobalProjectile<excelProjectile>().healStrength = 3;
			Projectile.GetGlobalProjectile<excelProjectile>().healRate = 1;

			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 30);
			Projectile.velocity *= 0.93f;

			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 33);
			d.noGravity = true;
			d.velocity *= -0.8f;

			if (Projectile.timeLeft == 10)
			{
				Projectile.alpha = 255;
				Projectile.velocity *= 0;
				Projectile.knockBack *= 1.45f;
				Projectile.tileCollide = false;
				Projectile.position = Projectile.Center;
				Projectile.width = Projectile.height = 14 * 4;
				Projectile.Center = Projectile.position;
				canDealDamage = true;

				for (var i = 0; i < 15; i++)
				{
					Dust d1 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 33);
					d1.noGravity = true;
					d1.velocity = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
					d1.scale *= 1.25f;
				}
			}
		}
	}
}
