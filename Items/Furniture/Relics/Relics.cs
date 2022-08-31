using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace excels.Items.Furniture.Relics
{
	public class StellarStarshipRelic : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Minion Boss Relic");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
			// The place style (here by default 0) is important if you decide to have more than one relic share the same tile type (more on that in the tiles' code)
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Relics.ExcelsiorRelics>(), 0);

			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Master;
			Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
			Item.value = Item.buyPrice(0, 5);
		}
	}

	public class NiflheimRelic : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Minion Boss Relic");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
			// The place style (here by default 0) is important if you decide to have more than one relic share the same tile type (more on that in the tiles' code)
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Relics.ExcelsiorRelics>(), 1);

			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Master;
			Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
			Item.value = Item.buyPrice(0, 5);
		}
	}

	public class ChasmRelic : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Minion Boss Relic");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
			// The place style (here by default 0) is important if you decide to have more than one relic share the same tile type (more on that in the tiles' code)
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Relics.ExcelsiorRelics>(), 2);

			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Master;
			Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
			Item.value = Item.buyPrice(0, 5);
		}
	}
}
