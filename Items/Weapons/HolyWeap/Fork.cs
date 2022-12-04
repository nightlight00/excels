using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace excels.Items.Weapons.HolyWeap
{
    #region Tuning Fork
    internal class TuningFork : ClericHolyWeap
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/F6AC04:-Holy Weapon-]\nRight click to cast [c/F6DD04:Holy Beam] \n[c/F6DD04:Holy Beam] restores 6 health");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 10000;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HolyBeam>();
			Item.shootSpeed = 4f;
			Item.noMelee = false;
			Item.knockBack = 5.3f;
			Item.damage = 9;

			clericEvil = false;
		}
		/*
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			Player player = Main.player[Item.whoAmI];
			int heal = 4 + player.GetModPlayer<excelPlayer>().healBonus;
			Tooltip.SetDefault($"[c/F6AC04:-Holy Weapon-]\nRight click to cast [c/F6DD04:Holy Beam] \n[c/F6DD04:Holy Beam] restores {heal} health");
		}
		*/
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
				if (player.HasBuff(ModContent.BuffType<Buffs.ClericCld.BlessingCooldown>())) { return false; }
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.UseSound = SoundID.Item35;
			}
            else
            {
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noMelee = false;
				Item.UseSound = SoundID.Item1;
			}
			return true;
        }

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (target.GetGlobalNPC<excelNPC>().BlessedSpell < 180)
				target.GetGlobalNPC<excelNPC>().BlessedSpell = 180;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.altFunctionUse == 2)
            {
				player.AddBuff(ModContent.BuffType<Buffs.ClericCld.BlessingCooldown>(), 900);
				return true;
            }
			return false;
        }

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.GoldBar, 8)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.PlatinumBar, 8)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	public class HolyBeam : clericHealProj
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 4;
			Projectile.timeLeft = 100;
			Projectile.extraUpdates = 110;
			Projectile.alpha = 255;

			healPenetrate = -1;
			healRate = 0;
			healPower = 6;
		}


		public override void AI()
		{
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 10);
			Dust d = Dust.NewDustPerfect(Projectile.Center, 204);
			d.velocity = Vector2.Zero;
			d.scale *= 1.15f;
		}
	}
	#endregion

	#region Templar's Mace 
	internal class TemplarsMace : ClericHolyWeap
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Templar's Mace");
			Tooltip.SetDefault("[c/F6AC04:-Holy Weapon-]\nRight click to cast [c/F6DD04:Holy Shield] \n[c/F6DD04:Holy Shield] generates a radiant bubble, which grant's all allies inside it gain an extra 10 defense");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 10000;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HolyBubble>();
			Item.shootSpeed = 4f;
			Item.noMelee = false;
			Item.knockBack = 6.5f;
			Item.damage = 15;
			Item.crit = 4;
			Item.rare = 1;

			clericEvil = false;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (target.GetGlobalNPC<excelNPC>().BlessedSpell < 420)
				target.GetGlobalNPC<excelNPC>().BlessedSpell = 420;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (player.HasBuff(ModContent.BuffType<Buffs.ClericCld.BlessingCooldown>())) { return false; }
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.UseSound = SoundID.Item35;
			}
			else
			{
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noMelee = false;
				Item.UseSound = SoundID.Item1;
			}
			return true;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				player.AddBuff(ModContent.BuffType<Buffs.ClericCld.BlessingCooldown>(), 1500);
				Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
				return false;
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.MysticCrystal>(), 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class HolyBubble : clericHealProj
    {
        public override void SafeSetDefaults()
        {
			Projectile.width = Projectile.height = 128;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 240;
			Projectile.alpha = 100;

			canHealOwner = true;
        }

		float dScale = 1;

		public override void AI()
		{
			BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 64);
			for (var i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center + new Vector2(0, 64).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] + (90 * i))), 204);
				d.scale = 1.6f * dScale;
				d.noGravity = true;
				d.velocity = Vector2.Zero;

				Dust d2 = Dust.NewDustPerfect(Projectile.Center + new Vector2(0, 44).RotatedBy(MathHelper.ToRadians(-Projectile.ai[0] + (90 * i))), 204);
				d2.scale = 1.1f * dScale;
				d2.noGravity = true;
				d2.velocity = Vector2.Zero;
			}
			Projectile.ai[0] += 2.2f;
			if (Projectile.timeLeft < 20)
            {
				Projectile.alpha += 8;
				dScale -= 0.05f;
            }
        }

        public override void BuffEffects(Player target, Player healer)
        {
			target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.HolyGuardBuff>(), 2);
        }
    }
	#endregion
}
