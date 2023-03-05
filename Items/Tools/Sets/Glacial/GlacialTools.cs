using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using excels.Items.Materials.Ores;

namespace excels.Items.Tools.Sets.Glacial
{
    internal class GlacialPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadExtraRange[Item.type] = 1;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.width = Item.height = 40;
            Item.rare = 1;
            Item.autoReuse = true;
            Item.knockBack = 2.5f;
            Item.useTurn = true;

            Item.pick = 90;   
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GlacialBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }

    internal class GlacialHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadExtraRange[Item.type] = 1;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.width = 46;
            Item.height = 48;
            Item.rare = 1;
            Item.autoReuse = true;
            Item.knockBack = 2.5f;
            Item.useTurn = true;

            Item.axe = 20;
            Item.hammer = 80;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GlacialBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
}
