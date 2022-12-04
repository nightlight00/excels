using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using System.Collections.Generic;

namespace excels.Items.Misc
{
    internal class MushroomWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Places mushrooms");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.width = Item.height = 36;
            Item.rare = 1;
            Item.tileWand = ItemID.Mushroom;
            Item.createTile = ModContent.TileType<Tiles.Blocks.MushroomTile>();
        }
    }
}
