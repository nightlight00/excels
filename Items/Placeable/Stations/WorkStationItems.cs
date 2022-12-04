using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using excels.Tiles.Stations;

namespace excels.Items.Placeable.Stations
{
	#region Starlight Anvil
	internal class StarlightAnvil : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Used to forge powerful weapons");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.createTile = ModContent.TileType<StarlightAnvilTile>(); // This sets the id of the tile that this item should place when used.

			Item.width = 40; // The item texture's width
			Item.height = 20; // The item texture's height

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.rare = ModContent.RarityType<StellarRarity>();

			Item.maxStack = 99;
			Item.consumable = true;
			Item.value = 150;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 8)
				.AddIngredient(ModContent.ItemType<Materials.GlacialBar>(), 8)
				.AddIngredient(ItemID.IronAnvil)
				.AddTile(TileID.Hellforge)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 8)
				.AddIngredient(ModContent.ItemType<Materials.GlacialBar>(), 8)
				.AddIngredient(ItemID.LeadAnvil)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
	#endregion

	#region Oil Kit
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
			Item.createTile = ModContent.TileType<OilKit>();
			Item.width = 16;
			Item.height = 16;
		}
	}
	#endregion
}
