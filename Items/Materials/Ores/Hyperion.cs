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
    public class HyperionCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.rare = 4;
            Item.maxStack = 9999;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<Tiles.OresBars.HyperionTile>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.placeStyle = 0;
            Item.sellPrice(0, 0, 11);
        }
    }
}
