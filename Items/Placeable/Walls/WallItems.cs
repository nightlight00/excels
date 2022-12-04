using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using excels.Items.Placeable.Platforms;
using excels.Items.Placeable.Tiles;
using excels.Tiles.Walls;

namespace excels.Items.Placeable.Walls
{
    #region Skyline Brick Wall
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
			Item.createWall = ModContent.WallType<SkylineBrickWall>();
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

	#region Checker Brick Wall
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
			Item.createWall = ModContent.WallType<CheckerWall>();
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

	#region Stellar Wall
	internal class StellarWallItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Wall");
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
			Item.createWall = ModContent.WallType<StellarWall>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ModContent.RarityType<StellarRarity>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<StellarBrick>())
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	internal class StellarAdornedWallItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adorned Stellar Wall");
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
			Item.createWall = ModContent.WallType<StellarWallAdorned>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ModContent.RarityType<StellarRarity>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<StellarBrick>())
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	#endregion
}
