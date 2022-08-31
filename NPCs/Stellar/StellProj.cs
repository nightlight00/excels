using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;
using System.IO;

namespace excels.NPCs.Stellar
{
    internal class GreenPlasma : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.hostile = true;
            Projectile.timeLeft = 75;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // TODO : add lighting
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90); 
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item114, Projectile.Center);
            for (var i = 0; i < 3; i++)
            {
                Vector2 vel = Projectile.velocity.RotatedBy(MathHelper.ToRadians(i * 120)) * 0.9f;
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vel, ProjectileID.GreenLaser, Projectile.damage * 2, Projectile.knockBack * 3);
                p.friendly = false;
                p.hostile = true;
                p.netUpdate = true;
            }
        }
    }

    internal class StellarRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.timeLeft = 200;
        }

        public override void AI()
        {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
            d.velocity = -Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(7)) * Main.rand.NextFloat(0.23f, 0.35f);
            d.scale = Main.rand.NextFloat(1.24f, 1.34f);
            d.fadeIn = d.scale * Main.rand.NextFloat(1, 1.17f);
            d.noGravity = true;

            if (Main.rand.NextBool())
            {
                Dust d2 = Dust.NewDustPerfect(Projectile.Center + new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 6);
                d2.scale = 1.34f + Main.rand.NextFloat(0.5f);
                d2.noGravity = true;
                d2.velocity = -Projectile.velocity * 0.4f;
            }

            Projectile.ai[0]++;
            if (Projectile.ai[0] > 24 && Projectile.ai[0] <= 80 && Projectile.ai[1] == 0)
            {
                Vector2 targetPos = Vector2.Zero;
                float targetDist = 900;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    Player player = Main.player[k];
                    float distance = Vector2.Distance(player.Center, Projectile.Center);
                    if (distance < targetDist)
                    {
                        targetDist = distance;
                        targetPos = player.Center;
                        target = true;
                    }
                }
                if (target)
                {
                    float num145 = 6f;
                    float num146 = 0.0833333358f;
                    Vector2 vec = targetPos - Projectile.Center;
                    vec.Normalize();
                    if (vec.HasNaNs())
                    {
                        vec = new Vector2((float)Projectile.direction, 0f);
                    }
                    Projectile.velocity = (Projectile.velocity * (num145 - 1f) + vec * (Projectile.velocity.Length() + num146)) / num145;
                }
            }

            if (Projectile.velocity.Length() < 6)
            {
                Projectile.velocity *= 1.1f;
            }
            
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
    }

    internal class StellarFlare : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.timeLeft = 110;
            Projectile.hostile = true;
        }

        int missiles = 0;
        public override void AI()
        {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
            d.velocity = -Projectile.velocity * 0.3f;
            d.scale = 1.5f;
            d.noGravity = true;

            if (Main.rand.NextBool())
            {
                Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                d2.scale = 1.1f;
                d2.noGravity = true;
                d2.velocity = -Projectile.velocity * 0.4f;
            }

            Projectile.velocity.X *= 0.94f;
            Projectile.velocity.Y *= 1.07f;
            if (Projectile.velocity.Y < -6)
            {
                Projectile.velocity.Y = -6;
            }

            Vector2 targetPos = Vector2.Zero;
            float targetDist = 900;
            int target = 0;
            bool Targeted = false;
            for (int k = 0; k < 200; k++)
            {
                Player player = Main.player[k];
                float distance = Vector2.Distance(player.Center, Projectile.Center);
                if (distance < targetDist)
                {
                    targetDist = distance;
                    targetPos = player.Center;
                    target = player.whoAmI;
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                if (Projectile.Center.Y < (Main.player[target].position.Y - (Main.screenHeight / 2) - 30))
                {
                    if (Main.rand.Next(5) == 1)
                    {
                        Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Main.player[target].position + new Vector2(0, -Main.screenHeight / 2), new Vector2(0, 11).RotatedByRandom(MathHelper.ToRadians(9)), ModContent.ProjectileType<StellarRocket>(), 10, 2);
                        p.ai[1] = 1;
                        p.netUpdate = true;
                        Projectile.netUpdate = true;

                        missiles++;
                        if (missiles >= 4)
                        {
                            Projectile.Kill();
                        }
                    }
                }
            }
        }
    }
}
