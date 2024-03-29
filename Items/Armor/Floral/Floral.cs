﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;

namespace excels.Items.Armor.Floral
{
    [AutoloadEquip(EquipType.Head)]
    internal class FloralStethoscope : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.FloralHeadPiece"));
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.HealingPower", 1));
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 32;
            Item.rare = 0;
            Item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().healBonus += 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FloralChest>() && legs.type == ModContent.ItemType<FloralBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            player.setBonus = "Healing allies increases your's and their life regeneration \nEnemies are less likely to target you";

            // player.GetModPlayer<excelPlayer>().healBonus += 1;
            player.GetModPlayer<excelPlayer>().FloralSet = true;
            player.aggro -= 200;   
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Misc.Herbs.Gladiolus>(), 3)
                .AddIngredient(ItemID.Vine, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class FloralChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.FloralChestPiece"));
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RadiantDamage", 3));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 22;
            Item.width = 24;
            Item.rare = 0;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ClericClassPlayer>().clericRadiantMult += 0.03f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Misc.Herbs.Gladiolus>(), 5)
                .AddIngredient(ItemID.Vine, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class FloralBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.FloralLegPiece"));
            Tooltip.SetDefault($"{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.HealingPower", 1)}\n{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.MovementSpeed", 10)}");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 22;
            Item.width = 24;
            Item.rare = 0;
            Item.defense = 0;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericRadiantMult += 0.03f;
            player.moveSpeed += 0.1f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Misc.Herbs.Gladiolus>(), 3)
                .AddIngredient(ItemID.Vine, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
