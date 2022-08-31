using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.NPCs.Chasm
{
    public class InfectionCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.timeLeft = 120;
            Projectile.alpha = 60;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 360);
        }


        public override void AI()
        {
            Projectile.velocity *= 0.91f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 6;
            }
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Main.rand.Next(7) <= 2)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 176);
                d.noGravity = true;
                d.velocity *= 0.3f;
                d.scale = Main.rand.NextFloat(0.9f, 1.4f);
            }
        }
    }

    public class InfectionBreath : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Mushroom}";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.timeLeft = 120;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            Projectile.ignoreWater = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 500);
        }

        public override void AI()
        {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 176, Projectile.velocity.X * 2, Projectile.velocity.Y * 2);
            d.noGravity = true;
            //d.velocity *= 0.3f;
            d.scale = Main.rand.NextFloat(1.9f, 2.5f);
            
        }
    }


    public class InfectionWave : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 26;
            Projectile.hostile = true;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 100;
            Projectile.penetrate = 1;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 500);
        }

        public override void AI()
        {
            for (var i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 176);
                d.noGravity = true;
                d.velocity *= 0.3f;
                d.scale = Main.rand.NextFloat(0.9f, 1.4f);
            }

            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 20 && Projectile.ai[0] <= 90)
            {
                Vector2 move = Vector2.Zero;
                float distance = 800f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.player[k].active)
                    {
                        Vector2 newMove = Main.player[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                        }
                    }
                }
                if (target)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (20 * Projectile.velocity + move); // / 5f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 11f / magnitude;
            }
        }
    }

    public class InfectionMissile : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SaucerMissile}";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }


        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.hostile = true;
            Projectile.timeLeft = 150;
            Projectile.tileCollide = false;
        }

        // creates 2 layers of infectious clouds of different distance

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                d.velocity *= Main.rand.NextFloat(0.9f, 1.4f);
                d.scale = Main.rand.NextFloat(0.8f, 1.6f);
            }
            for (var i = 0; i < 6; i++)
            {
                Vector2 shootvel = Projectile.velocity.RotatedBy(MathHelper.ToRadians((360f / 6f) * (i + 1))) * 0.25f;
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootvel, ModContent.ProjectileType<InfectionCloud>(), 18, 1);
                p.timeLeft = 240;
            }
            for (var i = 0; i < 6; i++)
            {
                Vector2 shootvel = Projectile.velocity.RotatedBy(MathHelper.ToRadians(((360f / 6f) * (i + 1))) + 60) * 0.5f;
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootvel, ModContent.ProjectileType<InfectionCloud>(), 18, 1);
                p.timeLeft = 240;
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            for (var i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 6, Projectile.velocity.X * -2, Projectile.velocity.Y * -2);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1, 1.3f);
            }
            Dust d2 = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, Projectile.velocity.X * -1.4f, Projectile.velocity.Y * -1.4f);
            d2.noGravity = true;
            d2.velocity *= 0.3f;
            d2.scale = 0.8f + Main.rand.NextFloat();
            Dust d3 = Dust.NewDustDirect(Projectile.Center, 0, 0, 176, Projectile.velocity.X * -1f, Projectile.velocity.Y * -1f);
            d3.noGravity = true;
            d3.velocity *= 0.5f;
            d3.scale = 0.6f + Main.rand.NextFloat();

            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Vector2 move = Vector2.Zero;
            float distance = 800f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.player[k].active)
                {
                    Vector2 newMove = Main.player[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (20 * Projectile.velocity + move); // / 5f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 9f / magnitude;
            }
        }
    }
}
