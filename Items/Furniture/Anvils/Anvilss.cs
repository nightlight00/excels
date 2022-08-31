using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Furniture.Anvils
{
    internal class StarlightAnvil : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to forge powerful weapons");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaults()
		{
			Item.createTile = ModContent.TileType<Tiles.Misc.StarlightAnvilTile>(); // This sets the id of the tile that this item should place when used.

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
}
