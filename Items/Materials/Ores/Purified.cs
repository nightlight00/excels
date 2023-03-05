using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using excels.Tiles.OresBars;

namespace excels.Items.Materials.Ores
{
    public class MysticCrystal : ModItem
    {
        public override string Texture => "excels/Items/Materials/Ores/PurityBar";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purity Bar");
            Tooltip.SetDefault("A refined chunk of purity");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 1;
            Item.maxStack = 999;

            Item.createTile = ModContent.TileType<ExcelBarTiles>();
            Item.placeStyle = 2;
            Item.consumable = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 0, 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PurifiedStone>(), 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }

    public class PurifiedStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A stone cleansed of all evil");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<PurityOre>();
            Item.consumable = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 0, 10);
        }
    }
}
