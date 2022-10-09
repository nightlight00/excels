using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;
using System.IO;
using Terraria.Audio;

namespace excels.NPCs.GreatOak
{
    internal class GreatOak : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.npcSlots = 50;

            NPC.lifeMax = 13000;
            NPC.defense = 14;
            NPC.damage = 40;
            NPC.knockBackResist = 0;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 54;
            NPC.height = 64;
            NPC.value = 70000;
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath6;
        }

        // NPC.ai[0] -> Current enemy state
        // NPC.ai[1] -> Attack cooldown
        // NPC.ai[2] -> Attack variable (flexible) (DO NOT MIX, ALWAYS RESET)

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            switch (NPC.ai[0])
            {
                // Boss intro scene
                case 0:
                    Intro();
                    break;
                // Deciding next boss attack / inbetween attack actions
                case 1:
                    DecideNextAttack(target);
                    break;
                // Reposition near target
                case 2:
                    GenericMovement(target);
                    break;
                
                /* ATTACKS */
                // Seeking Pellets
                case 3:
                    PelletAttack();
                    break;
            }

            NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 3.74f);
        }

        private void Intro()
        {
            NPC.ai[0] = 1;
            // TODO : Add a boss intro
        }

        private void DecideNextAttack(Player target)
        {
            NPC.ai[1]--;

            if (Vector2.Distance(target.Center, NPC.Center) > 600)
            {
                NPC.ai[0] = 2;
                return;
            }

            if (NPC.ai[1] <= 0)
            {
                NPC.ai[0] = 3;
        
            }
        }

        float maxHorizontalSpeed = 9.5f;
        float maxVerticalSpeed = 5.4f;
        Vector2 targetOffset = new Vector2(0, -120);

        private void GenericMovement(Player target)
        {
            Vector2 trueTargetCenter = target.Center + targetOffset;

            if (trueTargetCenter.X > NPC.Center.X)
                if (NPC.velocity.X > maxHorizontalSpeed)
                    NPC.velocity.X = maxHorizontalSpeed;
                else
                    NPC.velocity.X += 0.7f;
            if (trueTargetCenter.X < NPC.Center.X)
                if (NPC.velocity.X < -maxHorizontalSpeed)
                    NPC.velocity.X = -maxHorizontalSpeed;
                else
                    NPC.velocity.X -= 0.7f;
            if (trueTargetCenter.Y > NPC.Center.Y)
                if (NPC.velocity.Y > maxVerticalSpeed)
                    NPC.velocity.Y = maxVerticalSpeed;
                else
                    NPC.velocity.Y += 0.5f;
            if (trueTargetCenter.Y < NPC.Center.Y)
                if (NPC.velocity.Y < -maxVerticalSpeed)
                    NPC.velocity.Y = -maxVerticalSpeed;
                else
                    NPC.velocity.Y -= 0.5f;

            if (Vector2.Distance(target.Center, NPC.Center) < 140)
                NPC.ai[0] = 1;
        }

        private void ResetAttacks()
        {
            NPC.ai[0] = 1;
            NPC.ai[2] = 0;
        }

        private void PelletAttack()
        {
            if (NPC.ai[1] <= 0)
                NPC.ai[1] = 90;

            NPC.velocity *= 0.96f;

            NPC.ai[1]--;
            if (NPC.ai[1] < 60 && NPC.ai[1] > 15)
            {
                if (NPC.ai[1] % 3 == 0)
                {
                    int fireAmount = 1;

                    if (Main.expertMode || NPC.life < (NPC.lifeMax / 2))
                        NPC.ai[2]++;
                    if (NPC.ai[2] > 3 || Main.masterMode)
                    {
                        NPC.ai[2] -= 3;
                        fireAmount = 2;
                    }

                    for (var i = 0; i < fireAmount; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-10, 0), new Vector2(Main.rand.NextFloat(-4, -2.7f), Main.rand.NextFloat(-1.8f, 1.8f)), ModContent.ProjectileType<OakBolt>(), 45, 3);
                            p.ai[1] = (1 -(NPC.life / NPC.lifeMax)) * 3.5f;
                            Projectile p2 = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(10, 0), new Vector2(Main.rand.NextFloat(4, 2.7f), Main.rand.NextFloat(-1.8f, 1.8f)), ModContent.ProjectileType<OakBolt>(), 45, 3);
                            p2.ai[1] = (1 - (NPC.life / NPC.lifeMax)) * 3.5f;
                        }
                    }
                }
            }

            if (NPC.ai[1] == 1)
                ResetAttacks();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 3) {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y > 3 * frameHeight)
                    NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            
            Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/GreatOak/GreatOakWings").Value;
            int frameHeight = texture.Height / Main.npcFrameCount[NPC.type];
            var frame = texture.Frame(1, Main.npcFrameCount[NPC.type], 0, NPC.frame.Y/frameHeight);
            Main.EntitySpriteDraw(texture,
                NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY),
                frame, new Color(200, 225, 200, 225), NPC.rotation, frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Main.EntitySpriteDraw((Texture2D)ModContent.Request<Texture2D>(Texture),
                NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY),
                frame, NPC.GetAlpha(drawColor), NPC.rotation, frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }

    public class OakBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 400;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] <= 80)
            {
                Projectile.velocity *= 0.96f;
                if (Projectile.ai[0] == 80)
                {
                    bool target = false;
                    Vector2 targetPos = Projectile.Center;
                    float distance = 9000;
                    for (var i = 0; i < Main.maxPlayers; i++)
                    {
                        Player p = Main.player[i];
                        if (Vector2.Distance(p.Center, Projectile.Center) < distance)
                        {
                            target = true;
                            distance = Vector2.Distance(p.Center, Projectile.Center);
                            targetPos = p.Center;
                        }
                    }
                    if (target)
                    {
                        int rotateAmount = Main.masterMode ? 17 : Main.expertMode ? 10 : 6;
                        Projectile.velocity = ((targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * (4.7f + Projectile.ai[1])).RotatedByRandom(MathHelper.ToRadians(rotateAmount));
                    }
                }
            }
            if (Projectile.ai[0] == 180)
                Projectile.tileCollide = true;

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override bool PreDraw(ref Color lightColor) // thumbs up!!!!
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                var offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                var frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + offset;
                float sizec = Projectile.scale * (Projectile.oldPos.Length - k) / (Projectile.oldPos.Length * 0.8f);
                Color color = new Color(220, 126, 255, 255) * (1f - Projectile.alpha) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, frame, color, Projectile.oldRot[k], frame.Size() / 2, sizec, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            return true;
        }
    }

    public class TreeFellerNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tree Feller");
            Main.npcFrameCount[Type] = 2;
            NPCID.Sets.TrailCacheLength[NPC.type] = 36;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 999;
            NPC.defense = 999;
            NPC.damage = 40;

            NPC.width = NPC.height = 68;
            NPC.immortal = true;
            NPC.knockBackResist = 0;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void AI()
        {
            NPC.ai[0]++;

            NPC.velocity.X = MathF.Sin(NPC.ai[0] / 10) * 7;
            NPC.velocity.Y = MathF.Cos(NPC.ai[0] / 10) * -7;
            NPC.rotation += MathHelper.ToRadians(14);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                var offset = new Vector2(NPC.width / 2f, NPC.height / 2f);
                var frame = texture.Frame(1, Main.npcFrameCount[NPC.type], 0, 1);
                Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + offset;
                Color color = new Color(220, 126, 255, 255) * (1f - NPC.alpha) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, frame, color, NPC.oldRot[k], frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            
            return true;
        }
    }
}
