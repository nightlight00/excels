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


namespace excels.Items.Accessories.Ranged
{
    #region Virtual Shades
    public class VirtualShades : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().VirtualShades = true;
        }
    }
    #endregion


    /*
    internal class ArtemisQuiver : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows autofire for all ranged weapons\nShooting generates additonal projectiles depending on the weapon type\nBows generate an additonal arrows\nGuns generate 3 additional bullets\nLaunchers generate supporting fire");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 4;
            Item.width = Item.height = 28;
            Item.accessory = true;
            Item.value = 500;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().ArtemisSigil = true;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ArtemisRift>()] == 0)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<ArtemisRift>(), 0, 0, player.whoAmI);
            }
        }
    }

    public class ArtemisRift : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.VortexVortexLightning}";

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.netImportant = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.width = Projectile.height = 8;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.GetModPlayer<excelPlayer>().ArtemisSigil)
                Projectile.Kill();

            Projectile.timeLeft = 2;
            Projectile.Center = player.Center - new Vector2(0, 40);

            if (player.HeldItem.DamageType == DamageClass.Ranged)
            {
                if (--Projectile.ai[0] <= 0 && Main.myPlayer == Main.player[Projectile.owner].whoAmI && Main.mouseLeft && !Main.LocalPlayer.mouseInterface && !player.mapFullScreen)
                {
                    // switch dont work, so sad
                    // will default to bullets
                    int type = ModContent.ProjectileType<ArtemisArrow>();
                    int dur = 20;
                    int dmg = 55;
                    float vel = 5;
                    if (player.HeldItem.useAmmo == AmmoID.Bullet)
                    {
                        type = ProjectileID.MoonlordBullet;
                        dur = player.HeldItem.useAnimation / 3;
                        dmg = 32;
                        vel = 8;
                    }
                    else if (player.HeldItem.useAmmo == AmmoID.Arrow || player.HeldItem.useAmmo == AmmoID.Stake)
                    {
                        type = ModContent.ProjectileType<ArtemisArrow>();
                        dur = player.HeldItem.useAnimation;
                        dmg = 55;
                        vel = 5;
                    }
                    else if (player.HeldItem.useAmmo == AmmoID.Rocket || player.HeldItem.useAmmo == AmmoID.StyngerBolt || player.HeldItem.useAmmo == AmmoID.JackOLantern || player.HeldItem.useAmmo == AmmoID.NailFriendly)
                    {
                        //type = 2;
                    }
                    else if (player.HeldItem.useAmmo == AmmoID.Gel)
                    {
                        //type = 3;
                    }
                    else if (player.HeldItem.useAmmo == AmmoID.Dart)
                    {
                        dur = 20;
                    }

                    Vector2 pos = player.Center + new Vector2(64).RotatedByRandom(MathHelper.ToRadians(360));

                    for (var i = 0; i < 11; i++)
                    {
                        Dust d = Dust.NewDustPerfect(pos, 229);
                        d.noGravity = true;
                        d.velocity = (pos - player.Center).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians((360/11)*i)) * 2;
                        d.scale = 1.3f;
                    }

                    Vector2 velo = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * vel;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, velo, type, dmg, 4, player.whoAmI);
                    Projectile.ai[0] = dur;
                }
            }
        }
    }

    public class ArtemisArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            AIType = ProjectileID.Bullet;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.damage = 55;
            Projectile.extraUpdates = 4;
            Projectile.penetrate = 8;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (++Projectile.ai[1] > 40)
                Projectile.tileCollide = true;

            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 229);
                d.scale *= Main.rand.NextFloat(1.2f, 1.3f);
                d.fadeIn = d.scale * 1.2f;
                d.alpha = 180;
                d.noGravity = true;
                d.velocity = Projectile.velocity * 0.4f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 18; i++)
            {
                Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 229);
                d2.scale *= Main.rand.NextFloat(1.2f, 1.3f);
                d2.fadeIn = d2.scale * 1.2f;
                d2.alpha = 180;
                d2.noGravity = true;
                d2.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(360)) * 0.67f;
            }
        }
    }
*/
}

