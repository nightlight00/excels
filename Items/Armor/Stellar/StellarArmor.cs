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

namespace excels.Items.Armor.Stellar
{
    [AutoloadEquip(EquipType.Head)]
    internal class StellarHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Helmet");
			Tooltip.SetDefault("Increases damage of stellar weapons by 10%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = ModContent.RarityType<StellarRarity>();
			Item.defense = 6;
		}

        public override void UpdateEquip(Player player)
        {
			player.GetModPlayer<excelPlayer>().StellarDamageBonus += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<StellarChest>() && legs.type == ModContent.ItemType<StellarBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			// taking damage temporarily surrounds player with ice shards
			player.setBonus = "Stellar weapons gain new bonus effects";
			player.GetModPlayer<excelPlayer>().StellarSet = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 16)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	internal class StellarChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Spacesuit");
			Tooltip.SetDefault("Increases critical strike chance of stellar weapons by 8%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = ModContent.RarityType<StellarRarity>();
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<excelPlayer>().StellarCritBonus += 8;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 20)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	internal class StellarBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Boots");
			Tooltip.SetDefault("Increases attack speed of stellar weapons by 15%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = ModContent.RarityType<StellarRarity>();
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<excelPlayer>().StellarUseSpeed += 0.15f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 14)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
