﻿using Terraria;
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
    internal class DarksteelOre : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 4;
            Item.maxStack = 9999;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<DarksteelOreTile>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 3, 90);
        }
    }

    internal class DarksteelBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 4;
            Item.maxStack = 9999;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<Tiles.OresBars.ExcelBarTiles>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.placeStyle = 3;
            Item.sellPrice(0, 0, 12);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DarksteelOre>(), 3)
                .AddTile(TileID.AdamantiteForge)
                .Register();
        }
    }
}
