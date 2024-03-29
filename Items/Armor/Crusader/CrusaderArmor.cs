﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace excels.Items.Armor.Crusader
{
    [AutoloadEquip(EquipType.Head)]
    internal class CrusaderHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.CrusaderHeadPiece"));
            Tooltip.SetDefault($"{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RadiantDamage", 7)}\n{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.ClericCritChance", 4)}");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 32;
            Item.rare = 5;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericRadiantMult += 0.07f;
            modPlayer.clericCrit += 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CrusaderGarments>() && legs.type == ModContent.ItemType<CrusaderGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            player.setBonus = Language.GetTextValue("Mods.excels.ItemDescriptions.ArmorSetBonus.CrusaderSet"); // "Prevents death once\nThis effect has a 3 minute cooldown and temporarily decreases all damage for 1 minute\nIncreases max radiance by 40";

            // player.GetModPlayer<excelPlayer>().healBonus += 1;
            player.GetModPlayer<excelPlayer>().CrusaderSet = true;
            modPlayer.radianceStatMax2 += 40;
        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.GetModPlayer<excelPlayer>().HolyKnightSet)
            {
                player.armorEffectDrawShadow = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ModContent.ItemType<Materials.BottleOfSunlight>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }


    [AutoloadEquip(EquipType.Body)]
    internal class CrusaderGarments : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.CrusaderChestPiece"));
            Tooltip.SetDefault($"{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RadiantDamage", 8)}\n{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.HealingPower", 1)}\n{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RadianceRegenSlight")}");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 32;
            Item.rare = 5;
            Item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericRadiantMult += 0.08f;
            modPlayer.radianceRegenRate += 1;
            player.GetModPlayer<excelPlayer>().healBonus += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 16)
                .AddIngredient(ModContent.ItemType<Materials.BottleOfSunlight>(), 24)
                .AddIngredient(ItemID.Silk, 32)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }


    [AutoloadEquip(EquipType.Legs)]
    internal class CrusaderGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.CrusaderLegPiece"));
            Tooltip.SetDefault($"{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RadiantDamage", 6)}\n{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.ClericCritChance", 5)}");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 32;
            Item.rare = 5;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericRadiantMult += 0.06f;
            modPlayer.clericCrit += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 8)
                .AddIngredient(ModContent.ItemType<Materials.BottleOfSunlight>(), 18)
                .AddIngredient(ItemID.Silk, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
