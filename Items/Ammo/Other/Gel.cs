using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using excels.Items.Materials.Ores;

namespace excels.Items.Ammo.Other
{
    internal class GelCanistar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Endless Gel Canister");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.rare = 2;
            Item.ammo = AmmoID.Gel;
            //Item.shoot = 10;
            Item.knockBack = 0.5f;
            Item.DamageType = DamageClass.Ranged;
            Item.sellPrice(0, 1);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Gel, 3996)
                .AddTile(TileID.CrystalBall)
                .Register();
        }
    }

    internal class LampOil : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% chance to not be consumed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.rare = 1;
            Item.ammo = AmmoID.Gel;
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.sellPrice(0, 0, 0, 75);
        }

        public override bool CanBeConsumedAsAmmo(Item weapon, Player player)
        {
            return Main.rand.NextFloat() >= 0.1f;
        }
    }

    internal class LighterFluid : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% chance to not be consumed\nBrighter flames travel slower");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.rare = 1;
            Item.ammo = AmmoID.Gel;
            Item.shoot = ProjectileID.MolotovFire; // this will be modified in global items
            Item.damage = 5;
            Item.knockBack = 1.2f;
            Item.DamageType = DamageClass.Ranged;
            Item.sellPrice(0, 0, 1, 20);
        }

        public override bool CanBeConsumedAsAmmo(Item weapon, Player player)
        {
            return Main.rand.NextFloat() >= 0.1f;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe(40)
                .AddIngredient(ModContent.ItemType<LampOil>(), 40)
                .AddIngredient(ItemID.Hellstone)
                .AddTile(TileID.AdamantiteForge)
                .Register();
        }
    }

    internal class FrostOil : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frostfire Oil");
            Tooltip.SetDefault("10% chance to not be consumed\nFrost fire travels slower");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.rare = 1;
            Item.ammo = AmmoID.Gel;
            Item.shoot = ProjectileID.MolotovFire3; // this will be modified in global items
            Item.damage = 2;
            Item.knockBack = 0.3f;
            Item.DamageType = DamageClass.Ranged;
            Item.sellPrice(0, 0, 1, 20);
        }

        public override bool CanBeConsumedAsAmmo(Item weapon, Player player)
        {
            return Main.rand.NextFloat() >= 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(40)
                .AddIngredient(ModContent.ItemType<LampOil>(), 40)
                .AddIngredient(ModContent.ItemType<GlacialOre>())
                .AddTile(TileID.AdamantiteForge)
                .Register();
        }
    }

    internal class ShadowfireFluid : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowfire Oil");
            Tooltip.SetDefault("10% chance to not be consumed\nShadow flames travel much slower");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.rare = 1;
            Item.ammo = AmmoID.Gel;
            Item.shoot = ProjectileID.MolotovFire2; // this will be modified in global items
            Item.damage = 2;
            Item.knockBack = 1.2f;
            Item.DamageType = DamageClass.Ranged;
            Item.sellPrice(0, 0, 1, 50);
        }

        public override bool CanBeConsumedAsAmmo(Item weapon, Player player)
        {
            return Main.rand.NextFloat() >= 0.1f;
        }
    }
}
