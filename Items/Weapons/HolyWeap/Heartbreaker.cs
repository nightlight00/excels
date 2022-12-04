using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.HolyWeap
{
    internal class Heartbreaker : ClericHolyWeap
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/8a29d9:-Holy Weapon-]\nRight click to cast [c/d3103e:Heartache] \n[c/d3103e:Heartache] greatly increases ally's damage but at a health cost");
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
			Item.shoot = ModContent.ProjectileType<HeartachingAura>();
			Item.shootSpeed = 0.1f;
			Item.noMelee = false;
			Item.knockBack = 4.5f;
			Item.damage = 28;
			Item.rare = 2;

			clericEvil = true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (target.GetGlobalNPC<excelNPC>().BlessedSpell < 300)
				target.GetGlobalNPC<excelNPC>().BlessedSpell = 300;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (player.HasBuff(ModContent.BuffType<Buffs.ClericCld.BlessingCooldown>())) { return false; }
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.UseSound = SoundID.Item20;
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
				player.AddBuff(ModContent.BuffType<Buffs.ClericCld.AnguishedSoul>(), 450);
				return true;
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.ShatteredHeartbeat>(), 6)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	public class HeartachingAura : clericHealProj
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 4;
			Projectile.timeLeft = 2;
			Projectile.alpha = 255;

			canHealOwner = true;
			healRate = -1;
			healPenetrate = -1;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			HealDistance(Main.LocalPlayer, player, 300, false);
			for (var i = 0; i < 40; i++)
			{
				Dust d = Dust.NewDustDirect(player.Center, 0, 0, 90);
				d.velocity = new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)).RotatedByRandom(MathHelper.ToRadians(360));
				d.scale = Main.rand.NextFloat(1.4f, 1.7f);
				d.noGravity = true;
			}
		}

        public override void PostHealEffects(Player target, Player healer)
        {
			target.statLife -= 25;
			CombatText.NewText(target.getRect(), CombatText.DamagedFriendly, 25);

			if (target.statLife < 0)
            {
				target.KillMe(PlayerDeathReason.ByPlayer(healer.whoAmI), 10, 0);
				return;
            }
			target.AddBuff(ModContent.BuffType<Heartache>(), GetBuffTime(healer, 10));
        }
    }

	public class Heartache : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heartache");
			Description.SetDefault("Damage increased by 18%, but for a price");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // Add this so the nurse doesn't remove the buff when healing
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage(DamageClass.Generic) += 0.18f;
		}
	}
}
