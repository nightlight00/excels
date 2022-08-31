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

namespace excels.Items.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    internal class DevCoat : ModItem 
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("nightlight's Coat");
            Tooltip.SetDefault("'Great for impersonating devs!'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.vanity = true;
            Item.rare = 9;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    internal class AcesGoldFoxMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ace's Gold Fox Mask");
            Tooltip.SetDefault("'Great for impersonating devs!'");
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.vanity = true;
            Item.rare = 9;
        }
    }


    [AutoloadEquip(EquipType.Head)]
    internal class MafiosoHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Welcome to the family, Bobby'");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.vanity = true;
            Item.rare = 1;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    internal class RobotMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hungering Robot Mask");
            Tooltip.SetDefault("[74 61 73 74 79]");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.vanity = true;
            Item.rare = 1;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    internal class StellarStarshipMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.vanity = true;
            Item.rare = 1;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    internal class ChasmMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.vanity = true;
            Item.rare = 1;
        }
    }
}
