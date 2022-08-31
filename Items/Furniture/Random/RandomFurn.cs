using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace excels.Items.Furniture.Random
{
    internal class NightLightLamp : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightlight");
			Tooltip.SetDefault("'This is what peak performance looks like'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = 9;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Trophie.NightLightLampy>();
			Item.width = 16;
			Item.height = 16;
		}
	}

	internal class OilJit : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oil Kit");
			Tooltip.SetDefault("Allows the conversion of fossils into lamp oil");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Misc.OilKit>();
			Item.width = 16;
			Item.height = 16;
		}
	}
}
