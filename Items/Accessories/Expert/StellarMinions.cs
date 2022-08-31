using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;


namespace excels.Items.Accessories.Expert
{
    internal class StellarMinionA : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.netImportant = true;
        }

        public override bool? CanHitNPC(NPC target) => false;

        public override bool? CanCutTiles() => false;


        float speedScale = 1;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.GetModPlayer<excelPlayer>().StellarAcc || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            Projectile.velocity = ((player.Center - new Vector2(46 * -player.direction, 20)) - Projectile.Center).SafeNormalize(Vector2.Zero) * (5 * speedScale);
            if (Vector2.Distance(Projectile.Center, player.Center - new Vector2(46 * -player.direction, 20)) < 6)
            {
                // Projectile.Center = player.Center - new Vector2(46 * -player.direction, 20);
                speedScale *= 0.45f;
                if (speedScale < 0)
                {
                    speedScale = 0;
                }
            }
            else
            {
                if (Vector2.Distance(Projectile.Center, player.Center - new Vector2(46 * -player.direction, 20)) > 180)
                {
                    speedScale += 0.08f;
                    if (speedScale > 2.4f)
                    {
                        speedScale = 2.4f;
                    }
                }
                else
                {
                    if ((speedScale + 0.12f) < 1)
                    {
                        speedScale += 0.12f;
                    }
                    else
                    {
                        speedScale += 1 - speedScale;
                        if (speedScale > 1)
                        {
                            speedScale -= 0.09f;
                        }
                    }
                }
            }
            Projectile.rotation = Projectile.velocity.X * 0.07f;

            Projectile.ai[0]++;
            if (Main.rand.NextBool(3))
            {
                Projectile.ai[0]++;
            }

            if (Projectile.ai[0] > 30)
            {
                Vector2 pos = Vector2.Zero;
                bool target = false;
                float distance = 600;

                for (var i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.lifeMax > 5 && Vector2.Distance(Projectile.Center, npc.Center) < distance && npc.active && !npc.friendly && npc.type != NPCID.TargetDummy && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                    {
                        pos = npc.Center;
                        distance = Vector2.Distance(Projectile.Center, npc.Center);
                        target = true;
                    }
                }

                if (target)
                {
                    Vector2 vel = (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * 9.7f;
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vel, ProjectileID.EyeLaser, (int)(40 * player.GetModPlayer<excelPlayer>().StellarDamageBonus), Projectile.knockBack * 3, player.whoAmI);
                    p.friendly = true;
                    p.hostile = false;
                    p.scale = 0.75f;
                    p.netUpdate = true;
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;
                }
            }
        }
    }
}
