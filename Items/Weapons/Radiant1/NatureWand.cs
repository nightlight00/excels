using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System;

namespace excels.Items.Weapons.Radiant1
{
    internal class NatureBombWand : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
          //  DisplayName.SetDefault("Staff of Pearls");
            Tooltip.SetDefault("Conjures a nature bomb\nThe nature bomb deals increased damage if left to rest for a while");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.width = Item.height = 40;
            Item.useTime = Item.useAnimation = 29;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 10000;
            Item.rare = 0;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<NatureBomb>();
            Item.shootSpeed = 9.44f;
            Item.noMelee = true;
            Item.knockBack = 4.3f;

            Item.damage = 15;
            clericRadianceCost = 4;
            Item.sellPrice(0, 0, 0, 15);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 30)
                .AddIngredient(ItemID.Acorn, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class NatureBomb : clericHealProj
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.Acorn}";

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 2000;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            clericEvil = false;
            canDealDamage = true;
        }

        int explosionTimer = 1400;
        int damageMax = 3;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] < explosionTimer)
            {
                Projectile.ai[0] = explosionTimer;
                Boom();
            }
        }

        private void Boom()
        {

            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 3);
                d.noGravity = true;
                d.velocity *= Main.rand.NextFloat(0.8f, 1.3f);
                d.scale *= Main.rand.NextFloat(1.2f, 1.4f);
            }

            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 60;
            Projectile.Center = Projectile.position;

            for (var i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 3);
                d.noGravity = true;
                d.velocity *= Main.rand.NextFloat(0.8f, 1.3f);
                d.scale *= Main.rand.NextFloat(0.8f, 1f);
            }


            Projectile.alpha = 255;

            Projectile.timeLeft = 12;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 6);
            if (Projectile.ai[0] == 0)
            {
                damageMax = Projectile.damage * 4;
            }

            if (Projectile.ai[0] < explosionTimer)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + new Vector2(8).RotatedBy(Projectile.rotation - MathHelper.ToRadians(90)), 3);
                d.noGravity = true;
                d.velocity = Vector2.Zero;
            }

            if (++Projectile.ai[0] == explosionTimer)
            {
                Boom();
            }

            if (Math.Abs(Projectile.velocity.Length()) < 0.3f)
            {
                if (Projectile.alpha < 150)
                    Projectile.alpha += 4;
                if (Projectile.damage < damageMax)
                    Projectile.damage += 1;
            }

            Projectile.velocity.Y += 0.2f;
            Projectile.velocity *= 0.98f;

            if (Collision.SolidTiles(Projectile.position + new Vector2(4, -2), Projectile.width - 8, Projectile.height + 4))
            {
                if (Projectile.velocity.Y > 0)
                {
                    Projectile.position.Y -= 0.15f;
                }
                else
                {
                    Projectile.position.Y += 0.1f;
                }
                Projectile.velocity.Y = -Projectile.velocity.Y * 0.8f;
            }

            if (Collision.SolidTiles(Projectile.position + new Vector2(-4, 4), Projectile.width + 8, Projectile.height - 8))
            {
                Projectile.velocity.X = -Projectile.velocity.X;
            }
        }
    }
}
