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
            Item.damage = 8;
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

    #region Shroomy Bullet
    internal class ShroomyBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomy Bullet");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 999;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<ShroomBullet>();
            Item.damage = 5;
            Item.shootSpeed = 2.8f;
            Item.knockBack = 1.7f;
            Item.consumable = true;
            Item.sellPrice(0, 0, 0, 3);
        }

        public override void AddRecipes()
        {
            CreateRecipe(30)
                .AddIngredient(ItemID.MusketBall, 30)
                .AddIngredient(ItemID.GlowingMushroom)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class ShroomBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            AIType = ProjectileID.NanoBullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 900);
            Projectile.damage = (int)(Projectile.damage * 0.66f);
            Projectile.velocity *= 0.66f;
        }
    }
    #endregion
}
