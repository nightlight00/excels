using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.Necrotic1.EvilBoss
{
    internal class BloodlettingEye : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Striking foes leaves crimson hearts that restores a percentage of max health");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 26;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 44;
			Item.useTime = Item.useAnimation = 37;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CrimsonHeart>();
			Item.shootSpeed = 11f;
			Item.noMelee = true;

			clericEvil = true;
			clericBloodCost = 6;
			Item.sellPrice(0, 0, 75);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CrimtaneBar, 8)
				.AddIngredient(ItemID.TissueSample, 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class CrimsonHeart : clericHealProj
    {
		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 14;
			Projectile.timeLeft = 76;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

			canHealOwner = true;
			canDealDamage = true;
			healPenetrate = 1;
			//buffConsumesPenetrate = true;
			healRate = -1;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.ai[0]++;
			Projectile.velocity = -Projectile.velocity / 2;
			Projectile.alpha = 0;
			Projectile.timeLeft = 320;
			canDealDamage = false;
		}

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
				d.velocity = Projectile.velocity * 0.5f;
				d.noGravity = true;
				if (Main.rand.Next(8) <= 5)
                {
					Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
					d2.velocity += Projectile.velocity * 0.1f + new Vector2(0, 1);
					d2.scale *= Main.rand.NextFloat(0.6f, 0.75f);
				}
				Projectile.velocity.Y += 0.12f;
            }
            else
            {
				Projectile.GetGlobalProjectile<excelProjectile>().healStrength = (int)(Main.player[Projectile.owner].statLifeMax2 * 0.025f);
				Projectile.GetGlobalProjectile<excelProjectile>().healRate = 0;
				HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 32);
				Projectile.velocity *= 0.95f;
            }
        }
    }


	internal class DevouringRod : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Striking foes leaves shadow orbs that restores health and mana");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 18;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 44;
			Item.useTime = Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ShadowOrb>();
			Item.shootSpeed = 9.6f;
			Item.noMelee = true;

			clericEvil = true;
			clericBloodCost = 3;
			Item.sellPrice(0, 0, 75);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DemoniteBar, 8)
				.AddIngredient(ItemID.ShadowScale, 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class ShadowOrb : clericHealProj
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 14;
			Projectile.timeLeft = 76;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

			canHealOwner = true;
			canDealDamage = true;
			healPenetrate = 1;
			//buffConsumesPenetrate = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 180);

			Projectile.ai[0]++;
			Projectile.velocity = -Projectile.velocity / 2;
			Projectile.alpha = 0;
			Projectile.timeLeft = 320;
			canDealDamage = false;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
				d.velocity = Projectile.velocity * 0.3f;
				d.noGravity = true;
				d.alpha = 40;
				if (Main.rand.Next(8) <= 5)
				{
					Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
					d2.velocity += Projectile.velocity * 0.1f + new Vector2(0, 1);
					d2.scale *= Main.rand.NextFloat(0.6f, 0.75f);
					d2.alpha = 40;
				}
			}
			else
			{
				Projectile.GetGlobalProjectile<excelProjectile>().healStrength = 6;
				Projectile.GetGlobalProjectile<excelProjectile>().healRate = 0;
				HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 32);
				Projectile.velocity *= 0.95f;
			}
		}

		public override void PostHealEffects(Player target, Player healer)
		{
			int manaAmount = 8 + (healer.GetModPlayer<excelPlayer>().healBonus * 2);
			target.ManaEffect(manaAmount);
			target.statMana += manaAmount;

			if (target.statMana > target.statManaMax2)
			{
				target.statMana = target.statManaMax2;
			}
		}
	}
}
