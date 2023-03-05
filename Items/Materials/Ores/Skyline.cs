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
    public class SkylineBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.MaterialNames.SkylineBar"));
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.MaterialDescriptions.SkylineBar"));
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
            Item.placeStyle = 0;
            Item.sellPrice(0, 0, 10);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SkylineOre>(), 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }

    public class SkylineOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.MaterialNames.SkylineOre"));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
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
            Item.createTile = ModContent.TileType<Tiles.OresBars.SkylineOreTile>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 3, 20);
        }
    }
}
