using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;

namespace excels.Items.Ammo.Arrows
{
    internal class PoisonTippedArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poisonous Arrow");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.rare = 0;
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<Weapons.Bows.PoisonArrow>();
            Item.damage = 9;
            Item.shootSpeed = 4;
            Item.knockBack = 2.5f;
            Item.consumable = true;
            Item.DamageType = DamageClass.Ranged;
            Item.sellPrice(0, 0, 0, 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient(ItemID.WoodenArrow, 50)
                .AddIngredient(ItemID.JungleSpores)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
