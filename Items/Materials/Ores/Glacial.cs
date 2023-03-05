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
    public class GlacialBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.MaterialNames.GlacialBar"));
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.MaterialDescriptions.GlacialBar"));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 1;
            Item.maxStack = 9999;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<Tiles.OresBars.ExcelBarTiles>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.placeStyle = 1;
            Item.sellPrice(0, 0, 12);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GlacialOre>(), 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }

    public class GlacialOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.MaterialNames.GlacialOre"));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 1;
            Item.maxStack = 9999;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<Tiles.OresBars.GlacialOreTile>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 3, 80);
        }
    }
}
