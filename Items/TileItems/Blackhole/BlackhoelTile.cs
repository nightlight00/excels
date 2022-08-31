using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace excels.Items.TileItems.Blackhole
{
    internal class BlackholeBrick : ModItem
    {
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Blackhole.BlackholeBrickTile>();
			Item.width = 16;
			Item.height = 16;
		}

		public override void AddRecipes()
		{
			CreateRecipe(10)
				.AddIngredient(ItemID.StoneBlock, 10)
				.AddIngredient(ModContent.ItemType<Materials.BlackholeFragment>())
				.AddTile(TileID.LunarCraftingStation)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<BlackholePlatform>(), 2)
				.Register();
		}
	}

	internal class BlackholePlatform : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Blackhole.BlackholePlatformTile>();
			Item.width = 16;
			Item.height = 16;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2)
				.AddIngredient(ModContent.ItemType<BlackholeBrick>())
				.Register();
		}
	}
}
