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

namespace excels.Items.Misc.Fans
{
    #region Harpy Fan
    internal class HarpyFan : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Manipulates the wind currents to push it's wielder back! \nWind Strength : 13");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.useTime = Item.useAnimation = 20;
            Item.reuseDelay = 25;
            Item.width = Item.height = 30;
            Item.rare = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 13;
            Item.noMelee = true;

            Item.knockBack = 4;
            Item.shoot = 10;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.velocity.X = -velocity.X * 0.8f;
            if (player.velocity.Y != 0)
            {
                // player x speed gains extra boost when in air
                player.velocity.X = -velocity.X * 1.5f;
            }
            player.velocity.Y = -velocity.Y;

            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(player.position, player.width, player.width, 16);
                d.scale = Main.rand.NextFloat(1.3f, 1.5f);
                d.velocity = velocity.RotatedByRandom(MathHelper.ToRadians(40)) * Main.rand.NextFloat(0.3f, 0.5f);
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkylineBar>(), 5)
                .AddIngredient(ItemID.Feather, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    #endregion

    #region Wyvern's Wing
    internal class WyvernsWing : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wyvern's Wing");
            Tooltip.SetDefault("Manipulates the wind currents to send it's wielder flying!  \n'It is said that in distant worlds wyverns actually have wings, what stupid fairy tales'  \nWind Strength : 19");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.useTime = Item.useAnimation = 20;
            Item.reuseDelay = 25;
            Item.width = Item.height = 30;
            Item.rare = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 19;
            Item.noMelee = true;

            Item.knockBack = 5.4f;
            Item.shoot = 10;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.velocity.X = -velocity.X * 0.8f;
            if (player.velocity.Y != 0)
            {
                // player x speed gains extra boost when in air
                player.velocity.X = -velocity.X * 1.5f;
            }
            player.velocity.Y = -velocity.Y;

            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(player.position, player.width, player.width, 16);
                d.scale = Main.rand.NextFloat(1.3f, 1.5f);
                d.velocity = velocity.RotatedByRandom(MathHelper.ToRadians(50)) * Main.rand.NextFloat(0.1f, 0.4f);

                Dust d2 = Dust.NewDustDirect(player.position, player.width, player.width, 16);
                d2.scale = Main.rand.NextFloat(1.3f, 1.5f);
                d2.velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5)) * Main.rand.NextFloat(0.3f, 0.6f);
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HarpyFan>())
                .AddIngredient(ModContent.ItemType<Materials.WyvernScale>(), 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    #endregion

    #region Sakura Fan
    internal class SakuraFan : ModItem
    {
        public override void SetStaticDefaults()
        {
           // DisplayName.SetDefault("Wyvern's Wing");
            Tooltip.SetDefault("Manipulates wind currents and petals to propel it's wielder through the air!  \nWind Strength : 16.5");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.useTime = Item.useAnimation = 20;
            Item.reuseDelay = 25;
            Item.width = Item.height = 30;
            Item.rare = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 16.5f;
            Item.noMelee = true;

            Item.shoot = 10;
            Item.damage = 34;
            Item.knockBack = 7;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.velocity.X = -velocity.X * 0.8f;
            if (player.velocity.Y != 0)
            {
                // player x speed gains extra boost when in air
                player.velocity.X = -velocity.X * 1.5f;
            }
            player.velocity.Y = -velocity.Y;

            for (var i = 0; i < 35; i++)
            {
                Dust d = Dust.NewDustDirect(player.position, player.width, player.width, 71);
                d.scale = Main.rand.NextFloat(1.1f, 1.4f);
                d.velocity = velocity.RotatedByRandom(MathHelper.ToRadians(50)) * Main.rand.NextFloat(0.1f, 0.4f);
                // d.color = new Color(1.99f, .36f, 1.49f);
            }
          
            for (var i = 0; i < 20; i++)
                {
                    Dust d2 = Dust.NewDustDirect(player.position, player.width, player.width, 16);
                d2.scale = Main.rand.NextFloat(1.3f, 1.5f);
                d2.velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5)) * Main.rand.NextFloat(0.2f, 0.6f);
               // d2.color = new Color(199, 36, 149);
            }

            for (var i = 0; i < 7; i++)
            {
                Projectile p = Projectile.NewProjectileDirect(source, position, velocity.RotatedBy(MathHelper.ToRadians(-21 + (i * 7))) / 2, ProjectileID.FlowerPetal, damage, knockback/2, player.whoAmI);
                p.scale = 0.7f;
                p.DamageType = DamageClass.Generic;
                p.localNPCHitCooldown = 10;
                p.usesLocalNPCImmunity = true;
                p.timeLeft = 60;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DynastyWood, 30)
                .AddIngredient(ItemID.SoulofLight, 6)
                .AddIngredient(ItemID.SoulofNight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    #endregion


}
