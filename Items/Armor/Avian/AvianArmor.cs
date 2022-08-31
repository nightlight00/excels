using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;

namespace excels.Items.Armor.Avian
{
    [AutoloadEquip(EquipType.Head)]
    internal class AvianHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.ArmorNames.AvianHeadPiece"));
			Tooltip.SetDefault("Increases your max number of minions by 1");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(0, 0, 20);
			Item.rare = 1;
			Item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
			player.headcovered = false;
			player.maxMinions++;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AvianChest>() && legs.type == ModContent.ItemType<AvianSkirt>();
		}

		public override void UpdateArmorSet(Player player)
		{
			// taking damage temporarily surrounds player with ice shards
			player.setBonus = "Minions have a chance to summon feathers from the sky on attack \nIncreases minion knockback by 30%";
			player.GetModPlayer<excelPlayer>().AvianSet = true;
			player.GetKnockback(DamageClass.Summon) *= 1.3f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Feather, 6)
				.AddIngredient(ItemID.Silk, 16)
				.AddTile(TileID.Loom)
				.Register();
		}
	}


	[AutoloadEquip(EquipType.Body)]
	internal class AvianChest: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.ArmorNames.AvianChestPiece"));
			Tooltip.SetDefault("Increases minion damage by 7%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(0, 0, 25);
			Item.rare = 1;
			Item.defense = 2;
		}

        public override void UpdateEquip(Player player)
        {
			player.GetDamage(DamageClass.Summon) += 0.07f;
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Feather, 8)
				.AddIngredient(ItemID.Silk, 24)
				.AddTile(TileID.Loom)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	internal class AvianSkirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.ArmorNames.AvianLegPiece"));
			Tooltip.SetDefault("Increases minion damage by 4%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(0, 0, 15);
			Item.rare = 1;
			Item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.04f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Feather, 6)
				.AddIngredient(ItemID.Silk, 20)
				.AddTile(TileID.Loom)
				.Register();
		}
	}

	public class AvianSkyFeather : ModProjectile
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.HarpyFeather}";

        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.HarpyFeather);
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 999;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			AIType = -1;
        }

		Vector2 spawnPos;
		bool Spawn = true;

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if (Spawn)
            {
				for (var i = 0; i < 10; i++)
				{
					Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 116);
					d.velocity = (Projectile.velocity / 2).RotatedByRandom(MathHelper.ToRadians(15));
					d.noGravity = true;
				}
				spawnPos = Main.player[Projectile.owner].Center;
				Spawn = false;
			}
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 10; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 116);
				d.velocity = (Projectile.velocity / 2).RotatedByRandom(MathHelper.ToRadians(15));
				d.noGravity = true;
            }
        }
    }
}
