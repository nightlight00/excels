using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Armor.HolyKnight
{
    [AutoloadEquip(EquipType.Head)]
    internal class HolyKnightHelmet : ModItem
    {
        // set bonus :
        // increases healing by 1
        // for every 35 points of health you restore, restores 3 health of your own
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flamesilk Hat");
            Tooltip.SetDefault("6% increased radiant damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 32;
            Item.rare = 1;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericRadiantMult += 0.07f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HolyKnightChest>() && legs.type == ModContent.ItemType<HolyKnightBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            player.setBonus = "Every 35 health you heal restores 5 of your own health";

           // player.GetModPlayer<excelPlayer>().healBonus += 1;
            player.GetModPlayer<excelPlayer>().HolyKnightSet = true;
        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.GetModPlayer<excelPlayer>().HolyKnightSet)
            {
                player.armorEffectDrawShadowLokis = true;
                player.armorEffectDrawShadowSubtle = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.MysticCrystal>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class HolyKnightChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Holy Knight Chestplate");
            Tooltip.SetDefault("Increases max radiance by 20\n3% increased radiant damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 22;
            Item.width = 24;
            Item.rare = 1;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ClericClassPlayer>().radianceStatMax2 += 20;
            player.GetModPlayer<ClericClassPlayer>().clericRadiantMult += 0.03f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.MysticCrystal>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class HolyKnightBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Priest's Boots");
            Tooltip.SetDefault("4% increased cleric critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 22;
            Item.width = 24;
            Item.rare = 1;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericCrit += 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.MysticCrystal>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}