using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace excels.NPCs.Hell.Limbo.Wither
{
    internal class Wither : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.CursedSkull);

            NPC.width = NPC.height = 26;
            NPC.lifeMax = 400;
            NPC.damage = 20;
            NPC.defense = 20;
            NPC.alpha = 60;
            NPC.lavaImmune = true;
        }

        public override void AI()
        {
            for (var i = 0; i < 2; i++)
            {
                int type = ModContent.DustType<Dusts.WitherSmoke>();
                if (Main.rand.NextBool(7) || (NPC.ai[2] > 270 && Main.rand.NextBool(3)))
                    type = ModContent.DustType<Dusts.BrimstoneDust>();

                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, type);
                d.noGravity = true;
                d.velocity = NPC.velocity / 3;
            }

            if (NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = NPC.direction = -1;
            }
            else if (NPC.velocity.X > 0f)
            {
                NPC.spriteDirection = NPC.direction = 1;
            }

            if (Main.rand.NextBool(5))
                NPC.ai[2]++;

            if (++NPC.ai[2] > 330)
            {
                int target = -1;
                for (var i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active)
                    {
                        target = i;
                    }
                }

                if (target != -1)
                {
                    Vector2 velocity = (Main.player[target].Center - NPC.Center).SafeNormalize(Vector2.Zero) * 4;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<WitherBrimstone>(), 40, 3);
                    NPC.velocity -= velocity*1.2f;
                    NPC.ai[2] = 0;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[2] > 320)
                NPC.frame.Y = frameHeight;
            else
                NPC.frame.Y = 0;
        }
    }

    internal class WitherBrimstone : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 2;
            //Projectile.
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 800;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.BrimstoneHellscape>(), Main.rand.NextBool(4) ? 240 : 360);
        }

        public override void AI()
        {
            if (++Projectile.ai[0] == 80)
                Projectile.tileCollide = true;

            if (Projectile.wet)
                Projectile.Kill();

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.BrimstoneDust>());
            d.noGravity = true;
            d.velocity = Projectile.velocity / 2;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.BrimstoneDust>());
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(0.3f, 0.3f) * 18;
                d.scale *= 1.2f;
            }
        }
    }
}
