using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace excels.Items.Furniture.Paintings
{
    internal class ReflectivePrayer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Attire");
			Tooltip.SetDefault("'O. Night'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.createTile = ModContent.TileType<Tiles.Paintings.ReflectivePrayerPainting>(); // This sets the id of the tile that this item should place when used.

			Item.width = 40; // The item texture's width
			Item.height = 20; // The item texture's height

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.maxStack = 99;
			Item.consumable = true;
			Item.value = 150;
		}
	}

	internal class SelfPortrait : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portrait of Self");
			Tooltip.SetDefault("'O. Night'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.createTile = ModContent.TileType<Tiles.Paintings.SelfPortraitPainting>(); // This sets the id of the tile that this item should place when used.

			Item.width = 40; // The item texture's width
			Item.height = 20; // The item texture's height

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.maxStack = 99;
			Item.consumable = true;
			Item.value = 150;
		}
	}
}
