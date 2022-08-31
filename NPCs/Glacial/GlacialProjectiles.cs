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

namespace excels.NPCs.Glacial
{
    public class SnowBalls : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Chunk");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.coldDamage = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.frame = Main.rand.Next(5);
                Projectile.ai[0]++;
            }

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 76);
                d.velocity *= 0.4f;
                d.scale = 0.7f;
                d.noGravity = true;
            }
            Projectile.velocity.Y += 0.09f;
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 3);
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 8; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 76);
                d.velocity *= 0.6f;
                d.scale = 0.9f;
                d.noGravity = true;
            }
        }
    }

    public class FrostFire : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FrostBoltStaff}";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.alpha = 255;
            Projectile.coldDamage = true;
        }

        float speed = 1;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.expertMode || Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
            Projectile.Kill();
        }

        public override void AI()
        {
            Projectile.ai[0] += 0.5f;
            for (var i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92);
                d.noGravity = true;
                d.scale = 1.1f;
                d.velocity *= 0.1f;
            }
            if (Projectile.ai[0] < 5)
            {
                Projectile.velocity *= 0.97f;
            }
            if (Projectile.ai[0] > 5 && Projectile.ai[0] < 40)
            {
                // taken from github once again, thats why variable names arent accurate
                Player closestNPC = FindClosestNPC(2000);
                if (closestNPC == null)
                    return;

                // If found, change the velocity of the projectile and turn it in the direction of the target
                // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
                Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
                Projectile.rotation = Projectile.velocity.ToRotation();
                speed += 0.07f;
                if (Main.expertMode || Main.masterMode)
                {
                    Projectile.ai[0] -= 0.1f;
                    if (speed > 8) { speed = 8; }
                }
                else
                {
                    if (speed > 5) { speed = 5; }
                }
            }
        }

        // taken from github, modified to trail players
        public Player FindClosestNPC(float maxDetectDistance)
        {
            Player closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player target = Main.player[k];
                // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                // Check if it is within the radius
                if (sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    sqrMaxDetectDistance = sqrDistanceToTarget;
                    closestNPC = target;
                }
                
            }

            return closestNPC;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 8; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92);
                d.velocity *= 0.6f;
                d.scale = 0.9f;
                d.noGravity = true;
            }
        }
    }

    public class FrostAura : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FrostBoltStaff}";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 64;
            Projectile.hostile = true;
            Projectile.timeLeft = 999; // custom death so dont want it to die prematurely
            Projectile.alpha = 255;
            Projectile.coldDamage = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 360);
            if (Main.rand.NextBool(3) && Main.expertMode)
            {
                target.AddBuff(BuffID.Frozen, 120);
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            if (Projectile.ai[0] < 48 || Projectile.ai[1] == 0)
            {
                return false;
            }
            return true;
        }

        public override void AI()
        {
            Projectile.ai[0] += 2 - (Projectile.ai[1] * 0.25f); // slower if damaging type
            if (Projectile.ai[0] > 54)
            {
                Projectile.Kill();
            }
            int Dist = (int)Projectile.ai[0] - 16;
            if (Dist < 0) Dist = 0;
            for (int i = 0; i < 5; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(Projectile.Center + speed * Dist, 92, Vector2.Zero, Scale: 1.5f);
                d.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[1] == 0)
            {
                // this is where the queen summons her minions
                int minionCount = 4;
                if (Main.expertMode) { minionCount = 6; }
                int angels = NPC.CountNPCS(ModContent.NPCType<GlacialAngel>());
                int gaze = NPC.CountNPCS(ModContent.NPCType<GlacialBeholder>());

                if (angels < minionCount / 2 && gaze >= angels)
                {
                    NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<GlacialAngel>());
                }
                if (gaze < minionCount / 2)
                {
                    NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<GlacialBeholder>());
                }
            }
        }
    }

    public class GlacialArmor : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        float distance = 180;
        float cur_rot = 0;

        public override void AI()
        {
            if (NPC.CountNPCS(ModContent.NPCType<GlacialQueen>()) > 0)
            {
                float curDistance = 99999;
                Vector2 position = Vector2.Zero;
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];

                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < curDistance && npc.type == ModContent.NPCType<GlacialQueen>())
                    {
                        curDistance = distance;
                        position = npc.Center;
                        Projectile.alpha = npc.alpha;
                        Projectile.rotation = npc.rotation;
                    }

                }
                Projectile.Center = position;

                cur_rot += 0.05f;
                float changeX = 0, changeY = 0;
                if (Projectile.ai[1] == 1)
                {
                    changeX = (MathF.Cos(cur_rot * Projectile.ai[1]) * distance);
                }
                else
                {
                    changeX = -(MathF.Cos(cur_rot * Projectile.ai[1]) * distance); // * Projectile.ai[1];
                }
                changeY = MathF.Cos(cur_rot) * (distance / 5);
                Projectile.position += new Vector2(changeX, changeY).RotatedBy(Projectile.rotation);
                //Projectile.velocity.RotatedBy(MathHelper.ToRadians(45 * Projectile.ai[1]));
                Projectile.timeLeft = 2;
            }
            else
            {
                Projectile.Kill();
            }
        }
    }
}
