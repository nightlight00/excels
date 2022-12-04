using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using excels.Items.Placeable.Platforms;
using excels.Items.Placeable.Walls;
using excels.Tiles.Blocks;

namespace excels.Items.Placeable.Tiles
{
    #region Skyline Brick
    internal class SkylineBrick : ModItem
    {
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Granite Energy Torch");
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
			Item.createTile = ModContent.TileType<SkylineBrickTile>();
			Item.width = 16;
			Item.height = 16;
		}

		public override void AddRecipes()
		{
			CreateRecipe(5)
				.AddIngredient(ItemID.StoneBlock, 5)
				.AddIngredient(ModContent.ItemType<Materials.SkylineOre>())
				.AddTile(TileID.Furnaces)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SkylineBrickWallItem>(), 4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	#endregion

	#region Glacial Brick
	internal class GlacialBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Granite Energy Torch");
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
			Item.createTile = ModContent.TileType<GlacialBrickTile>();
			Item.width = 16;
			Item.height = 16;
		}

		public override void AddRecipes()
		{
			CreateRecipe(5)
				.AddIngredient(ItemID.StoneBlock, 5)
				.AddIngredient(ModContent.ItemType<Materials.GlacialOre>())
				.AddTile(TileID.Furnaces)
				.Register();
		}
	}
	#endregion

	#region Checker Tile
	internal class CheckerItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Checkered Brick");
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
			Item.createTile = ModContent.TileType<CheckerTile>();
			Item.width = 16;
			Item.height = 16;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2)
				.AddIngredient(ItemID.Marble)
				.AddIngredient(ItemID.Granite)
				.AddTile(TileID.Furnaces)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<CheckerWallItem>(), 4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	#endregion

	#region Hyperion Lamp
	internal class HyperionLampBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Granite Energy Torch");
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
			Item.createTile = ModContent.TileType<HyperionLampTile>();
			Item.width = 16;
			Item.height = 16;
		}

		public override void AddRecipes()
		{
			CreateRecipe(5)
				.AddIngredient(ItemID.StoneBlock, 5)
				.AddIngredient(ModContent.ItemType<Materials.HyperionCrystal>())
				.AddTile(TileID.Furnaces)
				.Register();
		}
	}
	#endregion

	#region Decorated Purity Bricks
	internal class DecoratedPurityBrick : ModItem
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
			Item.createTile = ModContent.TileType<DecoratedPurityBricksTile>();
			Item.width = 16;
			Item.height = 16;
		}

		public override void AddRecipes()
		{
			CreateRecipe(5)
				.AddIngredient(ItemID.StoneBlock, 5)
				.AddIngredient(ModContent.ItemType<Materials.PurifiedStone>())
				.AddTile(TileID.Furnaces)
				.Register();
		}
	}
	#endregion

	#region Stellar Bricks
	internal class StellarBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Block");
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
			Item.createTile = ModContent.TileType<StellarBrickTile>();
			Item.width = 16;
			Item.height = 16;
			Item.rare = ModContent.RarityType<StellarRarity>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.StoneBlock, 20)
				.AddIngredient(ModContent.ItemType<Materials.StellarPlating>())
				.AddTile(TileID.Furnaces)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<StellarPlatform>(), 2)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<StellarWallItem>(), 4)
				.AddTile(TileID.WorkBenches)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<StellarAdornedWallItem>(), 4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	#endregion

	#region Blackhole Bricks
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
			Item.createTile = ModContent.TileType<BlackholeBrickTile>();
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
	#endregion
}
