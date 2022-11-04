using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Creative;

namespace excels.Items.Accessories.Shield
{
    [AutoloadEquip(EquipType.Shield)]
    internal class BeetleShield : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beetle Husk Shield");
            Tooltip.SetDefault("Grants immunity to knockback \nGrants immunity to fire blocks " +
                "\nReflects damage back onto enemies \nEnemies are more likely to target you\nDecreases damage taken by 10%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.defense = 7;
            Item.rare = 8;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TurtleShield>())
                .AddIngredient(ItemID.BeetleHusk, 6)
                .AddIngredient(ItemID.LunarTabletFragment, 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.aggro += 400;
            player.noKnockback = true;
            player.fireWalk = true;
            player.endurance += 0.1f;
            player.GetModPlayer<excelPlayer>().ShieldReflect = true;

            player.hasRaisableShield = true;

            player.GetModPlayer<excelPlayer>().BeetleShield = true;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<AccessoryDrawHelper>()] == 0)
            {
                Projectile p = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, new Vector2(0, -3),
                    ModContent.ProjectileType<AccessoryDrawHelper>(), 0, 0, player.whoAmI);
                p.ai[0] = 1;
            }
        }
    }

    [AutoloadEquip(EquipType.Shield)]
    internal class TurtleShield : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Turtle Shell Shield");
            Tooltip.SetDefault("Grants immunity to knockback \nGrants immunity to fire blocks " +
                "\nReflects damage back onto enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.defense = 4;
            Item.rare = 7;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ObsidianShield)
                .AddIngredient(ItemID.ChlorophyteBar, 6)
                .AddIngredient(ItemID.TurtleShell)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.noKnockback = true;
            player.fireWalk = true;
            player.GetModPlayer<excelPlayer>().ShieldReflect = true;

            player.hasRaisableShield = true;
        }
    }

    [AutoloadEquip(EquipType.Shield)]
    internal class SnowShield : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Shield");
            Tooltip.SetDefault("Grants immunity to knockback \nGetting hit surrounds you in a frigid aura");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.defense = 3;
            Item.rare = 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltShield)
                .AddIngredient(ModContent.ItemType<Random.SnowflakeAmulet>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.noKnockback = true;
            player.GetModPlayer<excelPlayer>().SnowflakeAura = true;
            player.GetModPlayer<excelPlayer>().ShieldReflect = true;

            player.hasRaisableShield = true;
        }
    }
}
