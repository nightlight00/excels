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
    internal class FlamingWarhammer : ClericHolyWeap
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/F6AC04:-Holy Weapon-]\nRight click to cast [c/FC6619:Dancing Flames] \n[c/FC6619:Dancing Flames] riles up allies, increasing damage and critical strike chance");
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
			Item.shoot = ModContent.ProjectileType<FireAura>();
			Item.shootSpeed = 0.1f;
			Item.noMelee = false;
			Item.knockBack = 4.5f;
			Item.damage = 32;
			Item.rare = 3;

			clericEvil = false;
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
				player.AddBuff(ModContent.BuffType<Buffs.ClericCld.BlessingCooldown>(), 900);
				return true;
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 8)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class FireAura : clericHealProj
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 4;
			Projectile.timeLeft = 2;
			Projectile.alpha = 255;

			canHealOwner = true;
		}

        public override void Kill(int timeLeft)
        {
			Player player = Main.player[Projectile.owner];
			BuffDistance(Main.LocalPlayer, player, 300);
			for (var i = 0; i < 40; i++)
            {
				Dust d = Dust.NewDustDirect(player.Center, 0, 0, 6);
				d.velocity = new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)).RotatedByRandom(MathHelper.ToRadians(360));
				d.scale = Main.rand.NextFloat(1.4f, 1.7f);
				d.noGravity = true;
				d.noLight = true;
			}
        }

        public override void BuffEffects(Player target, Player healer)
        {
			target.AddBuff(ModContent.BuffType<FiredUp>(), GetBuffTime(healer, 9));
        }
    }

	public class FiredUp : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fired Up!");
			Description.SetDefault("7% increased damage and critical strike chance");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // Add this so the nurse doesn't remove the buff when healing
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCritChance(DamageClass.Generic) += 7;
			player.GetDamage(DamageClass.Generic) += 0.07f;
			player.buffImmune[BuffID.OnFire] = true;

			Dust d = Dust.NewDustDirect(player.Center + new Vector2(0, -10), 0, 0, 6);
			d.velocity = new Vector2(Main.rand.NextFloat(-1.7f, 1.7f), Main.rand.NextFloat(-1, -3));
			d.noGravity = true;
			d.scale = Main.rand.NextFloat(1.3f, 1.4f);
			d.fadeIn = d.scale * 1.2f;
		}
	}
}
