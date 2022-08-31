using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace excels.Items.TileItems
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
			Item.createTile = ModContent.TileType<Tiles.Misc.SkylineBrickTile>();
			Item.width = 16;
			Item.height = 16;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2)
				.AddIngredient(ItemID.StoneBlock, 2)
				.AddIngredient(ModContent.ItemType<Materials.SkylineOre>())
				.AddTile(TileID.Furnaces)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SkylineBrickWallItem>(), 4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	internal class SkylineBrickWallItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyline Brick Wall");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
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
			Item.createWall = ModContent.WallType<Tiles.Misc.SkylineBrickWall>();
			Item.width = 24;
			Item.height = 24;
		}

		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<SkylineBrick>())
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
			Item.createTile = ModContent.TileType<Tiles.Misc.GlacialBrickTile>();
			Item.width = 16;
			Item.height = 16;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2)
				.AddIngredient(ItemID.StoneBlock, 2)
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
			Item.createTile = ModContent.TileType<Tiles.Misc.CheckerTile>();
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

	internal class CheckerWallItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Checkered Wall");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
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
			Item.createWall = ModContent.WallType<Tiles.Misc.CheckerWall>();
			Item.width = 24;
			Item.height = 24;
		}

		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<CheckerItem>())
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
			Item.createTile = ModContent.TileType<Tiles.Misc.HyperionLampTile>();
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
}
