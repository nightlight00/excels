using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Misc.Herbs
{
	internal class Gladiolus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gladiolus");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.maxStack = 999;
			Item.placeStyle = 0;
			Item.width = 12;
			Item.height = 14;
			Item.value = 240;
		}
	}

	internal class GladiolusSeeds : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gladiolus Seeds");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.placeStyle = 0;
			Item.width = 12;
			Item.height = 14;
			Item.value = 80;
			Item.createTile = ModContent.TileType<Tiles.Herbs.GladiolusTile>();
		}
	}
}
