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

    #region Skyline Arrow
    internal class SkylineArrowItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skyline Arrow");
            Tooltip.SetDefault("Ignores gravity");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.rare = 1;
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<SkylineArrow>();
            Item.damage = 6;
            Item.shootSpeed = 4;
            Item.knockBack = 2.5f;
            Item.consumable = true;
            Item.DamageType = DamageClass.Ranged;
            Item.sellPrice(0, 0, 0, 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe(75)
                .AddIngredient(ItemID.WoodenArrow, 75)
                .AddIngredient(ModContent.ItemType<SkylineBar>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class SkylineArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            AIType = ProjectileID.Bullet;
            Projectile.penetrate = 2;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates++;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (int)(Projectile.damage * 0.8f);
        }

        public override void AI()
        {
            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 16);
                d.scale *= Main.rand.NextFloat(1.2f, 1.3f);
                d.fadeIn = d.scale * 1.2f;
                d.alpha = 110;
                d.noGravity = true;
                d.velocity = Projectile.velocity * 0.34f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 8; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 7);

                Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 16);
                d2.scale *= Main.rand.NextFloat(1.2f, 1.3f);
                d2.fadeIn = d.scale * 1.2f;
                d2.alpha = 110;
                d2.noGravity = true;
                d2.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(360)) * 0.45f;
            }
        }
    }
    #endregion

    #region Light Arrow
    internal class LightArrowItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunlight Arrow");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.rare = 1;
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<LightArrow>();
            Item.damage = 16;
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
                .AddIngredient(ModContent.ItemType<Materials.BottleOfSunlight>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class LightArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates++;
            Projectile.ignoreWater = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) { 
            Explode();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(2.44f, 2.55f, 1.32f)*0.85f);

            if (Projectile.ai[1] == 0)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(2, 2), 4, 4, 204);
                d.velocity = Projectile.velocity * Main.rand.NextFloat(0.2f, 0.5f);
                d.scale = 0.3f + Main.rand.NextFloat() * 0.2f;
            }
        }

        private void Explode()
        {
            if (Projectile.ai[1] != 0)
                return;

            Projectile.velocity = Vector2.Zero;
            Projectile.ai[1] = 1;
            Projectile.tileCollide = false;
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 80;
            Projectile.Center = Projectile.position;
            Projectile.alpha = 255;
            Projectile.timeLeft = 8;

            for (var i = 0; i < 20; i++)
            {
                var add = new Vector2(20).RotatedBy(MathHelper.ToRadians(360 / 20 * i));
                Dust d1 = Dust.NewDustPerfect(Projectile.Center + add, 204);
                d1.velocity = add/20 * 2.33f;
                d1.scale = 2;

                Dust d2 = Dust.NewDustPerfect(Projectile.Center + add, 204);
                d2.velocity = -add/20 * 1.2f;
                d2.scale = 1.2f;
            } 
        }
    }
    #endregion
}
