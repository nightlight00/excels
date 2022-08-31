using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.Shroomite
{
    internal class ShroomiteShredder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires in five round bursts \nOnly the first shot consumes ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 4;
            Item.useAnimation = 20;
            Item.reuseDelay = 8;
            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.height = 26;
            Item.width = 62;
            Item.knockBack = 2.2f;
            Item.rare = 8;
            Item.value = 5000;
            Item.shoot = 10;
            Item.shootSpeed = 9.7f;
            //Item.UseSound = SoundID.Item11;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.sellPrice(0, 3, 80);
        }

        int shotStyle = 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item11, player.Center);
            if (shotStyle == 0 || shotStyle == 4)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            else if (shotStyle == 1 || shotStyle == 3)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(4)), type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-4)), type, damage, knockback, player.whoAmI);
            }
            else if (shotStyle == 2)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(8)), type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-8)), type, damage, knockback, player.whoAmI);
            }
            shotStyle++;
            if (shotStyle == 5)
            {
                shotStyle = 0;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16f, 0f);
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < 18)
            {
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShroomiteBar, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class ShroomiteBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires high velocity arrows");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 67;
            Item.useTime = Item.useAnimation = 8;
            Item.reuseDelay = 14;
            Item.autoReuse = true;
            Item.knockBack = 3.4f;
            Item.noMelee = true;
            Item.height = 36;
            Item.width = 16;
            Item.rare = 8;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = 10;
            Item.shootSpeed = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5;
            Item.sellPrice(0, 4);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(6)), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-6)), type, damage, knockback, player.whoAmI);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShroomiteBar, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class ShroomiteLobber : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Launches grenades");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GrenadeLauncher);
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 85;
            Item.useTime = 7;
            Item.useAnimation = 21;
            Item.reuseDelay = 14;
            Item.reuseDelay = 14;
            Item.autoReuse = true;
            Item.knockBack = 7.3f;
            Item.noMelee = true;
            Item.height = 36;
            Item.width = 16;
            Item.rare = 8;
            Item.useAmmo = AmmoID.Rocket;
            Item.shoot = ProjectileID.GrenadeI;

            Item.shootSpeed = 13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.crit = 6;
            Item.sellPrice(0, 3, 90);
            //   Item.UseSound = SoundID.Item61;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-26f, 0f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            SoundEngine.PlaySound(SoundID.Item61, player.Center);
            return true;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < 19)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShroomiteBar, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class ShroomiteFlamethrower : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("20% chance to not consume ammo\nSplits flames into three");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 43;
            Item.knockBack = 0.3f;
            Item.noMelee = true;
            Item.useTime = 8;
            Item.useAnimation = 24;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.rare = 8;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 6.25f;
            Item.useAmmo = ItemID.Gel;
            Item.shoot = ProjectileID.Flames;

            Item.useTurn = false;
            Item.autoReuse = true;
            Item.sellPrice(0, 3, 70);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (var i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(20 * (i - 1))), type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShroomiteBar, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < player.itemAnimationMax - 2)
            {
                return false;
            }
            return Main.rand.Next() > .2f;
        }
    }
}

