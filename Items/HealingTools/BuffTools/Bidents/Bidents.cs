using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.HealingTools.BuffTools.Bidents
{
    #region Gold
    internal class GoldBident : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Conjures a ruby bolt that grants allies increased defense");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item13;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<RubyBolt>();
			Item.shootSpeed = 7.2f;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 70);

			Item.mana = 10;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.GoldBar, 12)
				.AddIngredient(ItemID.Ruby, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class RubyBolt : clericHealProj
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 4;
			Projectile.timeLeft = 90;
			Projectile.alpha = 255;

			buffConsumesPenetrate = true;
		}

		public override void AI()
		{
			BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 20);

			for (var i = 0; i < 2; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 60);
				d.velocity = Vector2.Zero;
				d.noGravity = true;
				d.scale *= 1.5f;
			}
			if (++Projectile.ai[0] > 24)
			{
				Projectile.velocity.Y += 0.12f;
			}
		}

		public override void BuffEffects(Player target, Player healer)
		{
			target.AddBuff(ModContent.BuffType<RubyDefender>(), GetBuffTime(healer, 5));
		}
	}

	public class RubyDefender : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Defender");
			Description.SetDefault("Grants an additional 5 defense");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // Add this so the nurse doesn't remove the buff when healing
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 5; // Grant a +4 defense boost to the player while the buff is active.
		}
	}
	#endregion

	#region Platinum
	internal class PlatinumBident : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Conjures a diamond bolt that grants allies increased critical strike chance");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item13;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<DiamondBolt>();
			Item.shootSpeed = 7.2f;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 70);

			Item.mana = 10;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PlatinumBar, 12)
				.AddIngredient(ItemID.Diamond, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class DiamondBolt : clericHealProj
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 4;
			Projectile.timeLeft = 90;
			Projectile.alpha = 255;

			buffConsumesPenetrate = true;
		}

		public override void AI()
		{
			BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 20);

			for (var i = 0; i < 2; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 63);
				d.velocity = Vector2.Zero;
				d.noGravity = true;
				d.scale *= 1.5f;
			}
			if (++Projectile.ai[0] > 24)
			{
				Projectile.velocity.Y += 0.12f;
			}
		}

		public override void BuffEffects(Player target, Player healer)
		{
			target.AddBuff(ModContent.BuffType<DiamondCutter>(), GetBuffTime(healer, 5));
		}
	}

	public class DiamondCutter : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Cutter");
			Description.SetDefault("Grants an additional 5% chance for a critical strike");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // Add this so the nurse doesn't remove the buff when healing
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCritChance(DamageClass.Generic) += 5; // Grant a +4 defense boost to the player while the buff is active.
		}
	}
	#endregion
}
