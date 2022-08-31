using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.Launcher1
{
    /*
    public class FlareCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flare Cannon");
            Tooltip.SetDefault("Combines the firepower of flares and explosions!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 44;
            Item.useAmmo = AmmoID.Flare;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.height = 22;
            Item.width = 46;
            Item.knockBack = 3.2f;
            Item.rare = 2;
            Item.value = 5000;
            Item.shoot = ModContent.ProjectileType<FlareRocket>();
            Item.shootSpeed = 5;
            Item.UseSound = SoundID.Item36;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-24f, -4f);
        }

        public override void AddRecipes()
        {
            ModContent.GetInstance<FlareCannon>().CreateRecipe()
                .AddIngredient(ItemID.FlareGun)
                .AddIngredient(ModContent.ItemType<Guns1.GrenadePistol>())
                .AddIngredient(ItemID.IllegalGunParts)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, new Vector2(player.Center.X, player.Center.Y - 8), velocity, ModContent.ProjectileType<FlareRocket>(), damage, knockback, player.whoAmI, 0);
            return false;
        }
    }
    */

    public class FlareRocket : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flare}";

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.width = Projectile.height = 6;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] < 2)
            {
                Explosion();
                target.AddBuff(BuffID.OnFire, 360);
            }
            else
            {
                target.AddBuff(BuffID.OnFire, 240);
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Projectile.ai[0] < 2)
            {
                Explosion();
                target.AddBuff(BuffID.OnFire, 360);
            }
            else
            {
                target.AddBuff(BuffID.OnFire, 240);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] < 2)
            {
                Explosion();
            }
            return false;
        }

        private void Explosion()
        {
            Projectile.alpha = 255;
            Projectile.velocity *= 0;
            Projectile.timeLeft = 15;
            Projectile.knockBack *= 1.45f;
            Projectile.tileCollide = false;
            Projectile.ai[0] = 2;
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 65;
            Projectile.Center = Projectile.position;
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center); // grenade explosion
            for (var i = 0; i < 18; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                d.scale = Main.rand.NextFloat(1) / 4 + 0.9f;
                d.velocity = new Vector2(Main.rand.NextFloat(-2.4f, 2.4f), Main.rand.NextFloat(-0.25f, -3));

                Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                d2.scale = Main.rand.NextFloat(1) / 3 + 1f;
                d2.velocity = new Vector2(Main.rand.NextFloat(-2.4f, 2.4f), Main.rand.NextFloat(-0.25f, -3));
                d2.alpha = 200;
            }
            for (var g = 0; g < 4; g++)
            {
                int Type = Main.rand.Next(3);
                switch (Type)
                {
                    case 1: Type = GoreID.Smoke1; break;
                    case 2: Type = GoreID.Smoke2; break;
                    default: Type = GoreID.Smoke3; break;
                }
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(-2 + (1 * g) + Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.2f, -0.05f)), Type);
            }
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 2)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
                Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 6);
                d.velocity *= 0;
                d.scale = 0.9f;
                d.noGravity = true;
                if (Main.rand.NextBool(4))
                {
                    Dust d2 = Dust.NewDustDirect(Projectile.Center, 0, 0, 31);
                    d2.velocity *= 0.2f;
                    d2.scale = Main.rand.NextFloat(0.9f, 1.2f);
                    d2.noGravity = true;
                }


                if (Projectile.ai[0] < 1)
                {
                    Projectile.ai[0] += 0.032f;
                }
                else
                {
                    Projectile.velocity.Y += 0.08f;
                }
            }
        }
    }
}
