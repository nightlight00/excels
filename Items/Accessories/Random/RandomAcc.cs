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

namespace excels.Items.Accessories.Random
{
    public class MimicToothNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Taking damage causes the necklace to bite back \nSeen as a good luck charm in some parts");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 3;
            Item.width = Item.height = 28;
            Item.accessory = true;
            Item.value = 500;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().MimicNecklace = true;
            player.luck += 0.2f;
         //   player.star
        }
    }

    public class SnowflakeAmulet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Getting hit surrounds you in a frigid aura");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 1;
            Item.width = Item.height = 28;
            Item.accessory = true;
            Item.value = 500;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().SnowflakeAura = true;
        }
    }

    public class YetiToothNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Getting hit surrounds you in a blizzard");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 3;
            Item.width = Item.height = 28;
            Item.accessory = true;
            Item.value = 500;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().BlizzardAura = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SnowflakeAmulet>())
                .AddIngredient(ModContent.ItemType<MimicToothNecklace>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class FireBadge : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("15% increased flamethrower damage\nFlamethrowers drench enemies in oil but halves it's penetration capabilities");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 4;
            Item.width = Item.height = 28;
            Item.accessory = true;
            Item.value = 500;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AvengerEmblem)
                .AddIngredient(ItemID.MagmaStone)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().FireBadge = true;
        }
    }


    /*
    [AutoloadEquip(EquipType.Face)]
    public class FrozenSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Every 60 health consumed using necrotic weapons refunds 10 health and surrounds you with a frigid aura");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 3;
            Item.width = Item.height = 28;
            Item.accessory = true;
            Item.value = 500;
        }

        public override void UpdateEquip(Player player)
        {
           // player.GetModPlayer<excelPlayer>().SnowflakeAura = true;
            player.GetModPlayer<excelPlayer>().skullPendant = true;
            player.GetModPlayer<excelPlayer>().skullPendantFrost = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Cleric.Necrotic.SkullPendant>())
                .AddIngredient(ModContent.ItemType<SnowflakeAmulet>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    */
}
