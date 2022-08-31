using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace excels.Items.Food
{
    internal class Carrot : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Minor improvements to all stats\n'A fashionable vegetable to wear during winter'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Apple);
            Item.maxStack = 30;
            Item.rare = 1;
            Item.consumable = true;
            Item.buffType = BuffID.WellFed;
            Item.buffTime = 46800; // 600 
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.holdStyle = ItemHoldStyleID.None;
        }
    }
}
