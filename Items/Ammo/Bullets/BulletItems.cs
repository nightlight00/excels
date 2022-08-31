using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;

namespace excels.Items.Ammo.Bullets
{
    internal class GlacialBul : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Bullet");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 999;
            Item.rare = 1;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<Weapons.Glacial.Bar.GlacialBullet>();
            Item.damage = 6;
            Item.shootSpeed = 4;
            Item.knockBack = 3.5f;
            Item.consumable = true;
            Item.sellPrice(0, 0, 0, 3);
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
                .AddIngredient(ItemID.MusketBall, 100)
                .AddIngredient(ModContent.ItemType<Items.Materials.GlacialBar>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
