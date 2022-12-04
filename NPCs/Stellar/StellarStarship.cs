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

namespace excels.NPCs.Stellar
{

    [AutoloadBossHead]
    internal class StellarStarship : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[NPC.type] = 18; // 12;
            NPCID.Sets.TrailingMode[NPC.type] = 1;

            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Confused,
                    BuffID.Poisoned,
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.Ichor,
                    BuffID.CursedInferno,
                    BuffID.Frostburn,
                    BuffID.Frostburn2,
                    BuffID.Slow,
                    BuffID.Bleeding
                }
            });
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("The commander of a interstellar fleet, this starship is decorated with numerous weapons of galactic warfare")
            });
        }

        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.npcSlots = 50;

            NPC.lifeMax = 7400;
            NPC.defense = 14;
            NPC.damage = 40;
            NPC.knockBackResist = 0;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 146;
            NPC.height = 80;
            NPC.value = 70000;

            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath6;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            if (Main.expertMode)
            {
                SpeedModifier = 1.2f;
            }
            if (Main.masterMode)
            {
                SpeedModifier = 1.3f;
            }


        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 0)
            {
                for (var i = 0; i < 50; i++)
                {
                    Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 31);
                    d.noGravity = true;
                    d.velocity *= Main.rand.NextFloat(0.8f, 1.4f);
                    d.scale *= 1.4f;
                }

                if (Main.netMode == NetmodeID.Server)
                {
                    // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                    return;
                }
                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Top, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f) + new Vector2(0, -2), Mod.Find<ModGore>("StellarGoreGlass1").Type);
                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Top, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f) + new Vector2(0, -2), Mod.Find<ModGore>("StellarGoreGlass2").Type);

                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Left, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGoreLights").Type);
                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Bottom, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGoreLights").Type);
                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Right, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGoreLights").Type);

                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.TopLeft, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.4f, 1f), Mod.Find<ModGore>("StellarGoreCannon").Type);
                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.TopRight, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.4f, 1f), Mod.Find<ModGore>("StellarGoreCannon").Type);

                for (var a = 0; a < 4; a++)
                {
                    Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGorePlating").Type);
                }
                for (var i = 0; i < 5; i++)
                {
                    Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.5f, 0.9f), Mod.Find<ModGore>("StellarGoreRim").Type);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {           
            Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Stellar/StellarStarship").Value;
            var offset = new Vector2(NPC.width / 2f, NPC.height / 2f);
            Vector2 origin = NPC.frame.Size() / 2f;
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                // only every second one is drawn
                if (k % 2 == 0)
                {
                    //NPC.oldPos[k]
                    Vector2 drawPos = NPC.oldPos[k] - new Vector2(0, 14) - Main.screenPosition;
                    var trailColor = NPC.GetAlpha(new Color(120, 120, 120, 30) * (1f - (float)NPC.alpha / 255f)) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length); //drawColor * 0.1f;
                    Main.spriteBatch.Draw(texture, drawPos + offset, null, trailColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
                }
            }

            return true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Decorations.Trophies.StellarTrophy>(), 10));

            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Bags.BagStarship>()));
            // rule that checks if not expert mode
            LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
            leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.StellarStarshipMask>(), 7));
            leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.StellarPlating>(), 1, 45, 56));
            npcLoot.Add(leadingConditionRule);

            // rule that checks if in master mode
            LeadingConditionRule leadingConditionRule2 = new LeadingConditionRule(new Conditions.IsMasterMode());
            leadingConditionRule2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Decorations.Relics.StellarStarshipRelic>(), 1));
            npcLoot.Add(leadingConditionRule2);
        }

        public override void OnKill()
        {
            if (!excelWorld.downedStarship)
            {
                excelWorld.downedStarship = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }
            NPC.SetEventFlagCleared(ref excelWorld.downedStarship, -1);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // draw 'glowmask'
            Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Stellar/StellarStarship_Glow").Value;
            var offset = new Vector2(NPC.width / 2f, NPC.height / 2f);
            Vector2 origin = NPC.frame.Size() / 2f;
            Main.spriteBatch.Draw(texture, (NPC.position - new Vector2(0, 14) - Main.screenPosition) + offset, null, NPC.GetAlpha(new Color(155, 155, 155, 155)), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
        }

        int CurrentAttack = -1;
        int StartupLag = 0;
        int AttackTimer = 0;
        int CooldownTimer = 10;

        int AttackOrder = 0;
        float SpeedModifier = 1;

        bool FinishedAttack = false;
        Vector2 TargetPos = Vector2.Zero;

        bool summonedMinions = false;

        public override void AI()
        {
            NPC.TargetClosest(true);

            if (Main.player[NPC.target].dead)
            {
                NPC.velocity.Y -= 0.08f;
                return;
            }

            #region Attack Choice
            if (AttackTimer < 0)
            {
                if (CooldownTimer < 0)
                {
                    TargetPos = Vector2.Zero;
                    FinishedAttack = false;

                    switch (AttackOrder)
                    {
                        case 0:
                        case 5:
                            CurrentAttack = 0;
                            break;
                        case 1:
                            CurrentAttack = 1;
                            break;
                        case 2:
                        case 7:
                            CurrentAttack = 2;
                            break;
                        case 6:
                            CurrentAttack = 3;
                            break;
                        case 3:
                            CurrentAttack = 4;
                            break;
                        case 4:
                            CurrentAttack = 5;
                            break;
                    }
                    switch (CurrentAttack)
                    {
                        case 0:
                            AttackTimer = 10;
                            CooldownTimer = 15;
                            if (Main.expertMode)
                            {
                                CooldownTimer = 10;
                            }
                            break;

                        case 1:
                            AttackTimer = 40;
                            CooldownTimer = 40;
                            if (Main.masterMode)
                            {
                                CooldownTimer = 30;
                            }
                            break;

                        case 2:
                            AttackTimer = 100;
                            CooldownTimer = 44;
                            break;

                        case 3:
                            AttackTimer = 120;
                            CooldownTimer = 48;
                            if (Main.masterMode)
                            {
                                CooldownTimer = 38;
                            }
                            break;

                        case 4:
                            AttackTimer = 20;
                            CooldownTimer = 17;
                            if (Main.expertMode)
                            {
                                CooldownTimer = 13;
                            }
                            break;

                        case 5:
                            AttackTimer = 20;
                            CooldownTimer = 45;
                            if (Main.masterMode)
                            {
                                CooldownTimer = 35;
                            }
                            NPC.ai[1] = 0;
                            break;
                    }

                    AttackOrder++;
                    if (AttackOrder == 8)
                    {
                        AttackOrder = 0;
                    }
                }

                CooldownTimer--;
            }
            #endregion

            #region Attacks
            if (AttackTimer > 0)
            {
                switch (CurrentAttack)
                {
                    case 0: // flies to a poinr above a player
                        if (!FinishedAttack)
                        {
                            NPC.velocity = ((Main.player[NPC.target].Center - new Vector2(0, 200)) - NPC.Center).SafeNormalize(Vector2.Zero) * (11 * SpeedModifier);
                            //FinishedAttack = true;
                        }

                        AttackTimer++;
                        if (Vector2.Distance(Main.player[NPC.target].Center - new Vector2(0, 200), NPC.Center) < 8)
                        {
                            NPC.Center = Main.player[NPC.target].Center - new Vector2(0, 200);
                            NPC.velocity *= 0;
                            AttackTimer = 0;
                        }
                        break;

                    case 1: // fires lasers below it that spread out more as attack progresses
                        if (AttackTimer == 40)
                        {
                            TargetPos = NPC.Center + new Vector2(0, 200);
                        }
                        if (AttackTimer % 4 == 2)
                        {
                            // left cannon
                            Vector2 vel = (TargetPos - (NPC.Center + new Vector2(-42, 18))).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(40 - AttackTimer) * 1.5f) * 7;
                            Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-42, 10), vel, ProjectileID.GreenLaser, 20, 1);
                            p.friendly = false;
                            p.hostile = true;
                            p.netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item157, NPC.Center + new Vector2(-42, 10));

                            // left cannon
                            vel = (TargetPos - (NPC.Center + new Vector2(42, 18))).SafeNormalize(Vector2.Zero).RotatedBy(-MathHelper.ToRadians(40 - AttackTimer) * 1.5f) * 7;
                            Projectile p2 = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(42, 10), vel, ProjectileID.GreenLaser, 20, 1);
                            p2.friendly = false;
                            p2.hostile = true;
                            p2.netUpdate = true;
                            NPC.netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item157, NPC.Center + new Vector2(42, 10));
                        }
                        break;

                    case 2: // !FinishedAttack : Flies to a point near the player
                            // FinishedAttack : Flies horizontaly, firing missiles that home in on players
                        if (!FinishedAttack)
                        {
                            AttackTimer++;
                            if (TargetPos == Vector2.Zero)
                            {
                                TargetPos = Main.player[NPC.target].Center + new Vector2(400, -130);
                                if (Main.rand.NextBool())
                                {
                                    TargetPos = Main.player[NPC.target].Center + new Vector2(-400, -130);
                                }
                            }
                            NPC.velocity = (TargetPos - NPC.Center).SafeNormalize(Vector2.Zero) * (13 * SpeedModifier);

                            if (Vector2.Distance(TargetPos, NPC.Center) < (8 * SpeedModifier))
                            {
                                NPC.Center = TargetPos;
                                FinishedAttack = true;
                                TargetPos = Vector2.Zero;
                                NPC.velocity = Vector2.Zero;
                            }
                        }
                        else
                        {
                            if (TargetPos == Vector2.Zero)
                            {
                                if (Main.player[NPC.target].Center.X > NPC.Center.X)
                                {
                                    TargetPos.X = 1;
                                }
                                else
                                {
                                    TargetPos.X = -1;
                                }
                            }

                            NPC.velocity.X = TargetPos.X * (7.5f * SpeedModifier);

                            if (AttackTimer % 10 == 0)
                            {
                                if (NPC.ai[2] == 0)
                                {
                                    Vector2 vel = new Vector2(4, -4);
                                    Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(70, -40), vel, ModContent.ProjectileType<StellarRocket>(), 20, 3);
                                    p.netUpdate = true;                     
                                    NPC.ai[2]++;
                                    SoundEngine.PlaySound(SoundID.Item11, NPC.Center + new Vector2(70, -40));
                                }
                                else
                                {
                                    Vector2 vel = new Vector2(-4, -4);
                                    Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-70, -40), vel, ModContent.ProjectileType<StellarRocket>(), 20, 3);
                                    p.netUpdate = true;
                                    NPC.ai[2]--;
                                    SoundEngine.PlaySound(SoundID.Item11, NPC.Center + new Vector2(-70, -40));
                                }
                                NPC.netUpdate = true;
                            }
                        }
                        
                        if (AttackTimer == 1)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        break;

                    case 3: // fires green plasma around in circles, which explode into 3 green lasers after a bit
                            // looks pretty cool in game
                        int Amount = 7;
                        if (Main.masterMode || Main.getGoodWorld)
                        {
                            Amount = 6;
                        }
                        if (AttackTimer % Amount == 1)
                        {
                            float spd = 2.5f;
                            if (Main.expertMode || Main.getGoodWorld || Main.masterMode)
                            {
                                spd = 3.2f;
                            }

                            Vector2 vel = new Vector2(spd, spd).RotatedBy(MathHelper.ToRadians(30 + (120 - AttackTimer)) * 3);
                            Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-42, 0), vel, ModContent.ProjectileType<GreenPlasma>(), 10, 3);
                            p.friendly = false;
                            p.hostile = true;
                            p.netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item158, NPC.Center + new Vector2(-42, 0));

                            vel = new Vector2(spd, spd).RotatedBy(MathHelper.ToRadians(120 - (120 - AttackTimer)) * 3);
                            Projectile p2 = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(42, 0), vel, ModContent.ProjectileType<GreenPlasma>(), 10, 3);
                            p2.friendly = false;
                            p2.hostile = true;
                            p2.netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item158, NPC.Center + new Vector2(42, 0));
                            NPC.netUpdate = true;
                        }
                        break;

                    case 4: // shoots flare up, which will summon rockets above the player
                        if (AttackTimer == 15)
                        {
                            Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-46, -34), new Vector2(Main.rand.NextFloat(-2, 2), -3), ModContent.ProjectileType<StellarFlare>(), 20, 0);
                            p.netUpdate = true;
                            NPC.netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item11, NPC.Center + new Vector2(-46, -34));
                        }
                        if (AttackTimer == 5 && Main.masterMode)
                        {
                            Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-46, -34), new Vector2(Main.rand.NextFloat(-2, 2), -3), ModContent.ProjectileType<StellarFlare>(), 20, 0);
                            p.netUpdate = true;
                            NPC.netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item11, NPC.Center + new Vector2(-46, -34));
                        }
                        break;

                    case 5: // flies around player, shooting lasers
                        int dist = 250, distLong = 380;
                        switch (NPC.ai[1])
                        {
                            default: // top right
                                TargetPos = Main.player[NPC.target].Center + new Vector2(dist, -dist);
                                break;

                            case 1: // right
                                TargetPos = Main.player[NPC.target].Center + new Vector2(distLong, 0);
                                break;

                            case 2: // bottom right
                                TargetPos = Main.player[NPC.target].Center + new Vector2(dist, dist);
                                break;

                            case 3: // bottom
                                TargetPos = Main.player[NPC.target].Center + new Vector2(0, distLong);
                                break;

                            case 4: // bottom left
                                TargetPos = Main.player[NPC.target].Center + new Vector2(-dist, dist);
                                break;

                            case 5: // left
                                TargetPos = Main.player[NPC.target].Center + new Vector2(-distLong, 0);
                                break;

                            case 6: // top left
                                TargetPos = Main.player[NPC.target].Center + new Vector2(-dist, -dist);
                                break;
                        }

                        AttackTimer++;
                        NPC.velocity = (TargetPos - NPC.Center).SafeNormalize(Vector2.Zero) * (14 * SpeedModifier);

                        if (Vector2.Distance(TargetPos, NPC.Center) < 16)
                        {
                            NPC.ai[1]++;
                            if (Main.expertMode || (NPC.ai[1] == 0 || NPC.ai[1] == 2 || NPC.ai[1] == 4 || NPC.ai[1] == 6))
                            {
                                Vector2 vel = (Main.player[NPC.target].Center - (NPC.Center + new Vector2(-42, 5))).SafeNormalize(Vector2.Zero) * 7;
                                Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-42, 5), vel, ProjectileID.GreenLaser, 20, 1);
                                p.friendly = false;
                                p.hostile = true;
                                p.tileCollide = false;
                                p.timeLeft = 200;
                                p.netUpdate = true;
                                SoundEngine.PlaySound(SoundID.Item157, NPC.Center + new Vector2(-42, 5));

                                vel = (Main.player[NPC.target].Center - (NPC.Center + new Vector2(42, 5))).SafeNormalize(Vector2.Zero) * 7;
                                Projectile p2 = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + new Vector2(42, 5), vel, ProjectileID.GreenLaser, 20, 1);
                                p2.friendly = false;
                                p2.hostile = true;
                                p2.tileCollide = false;
                                p2.timeLeft = 200;
                                p2.netUpdate = true;
                                SoundEngine.PlaySound(SoundID.Item157, NPC.Center + new Vector2(42, 5));
                                NPC.netUpdate = true;
                            }

                            if (NPC.ai[1] == 7)
                            {
                                NPC.velocity = Vector2.Zero;
                                AttackTimer = 0;
                            }
                        }
                        break;
                }
            }
            #endregion

            #region Lighting
            Vector3 GlassLights = new Vector3(.255f * 2, .190f * 2, .059f * 2);
            Lighting.AddLight(NPC.Center + new Vector2(78, 0), GlassLights);
            Lighting.AddLight(NPC.Center - new Vector2(78, 0), GlassLights);
            Lighting.AddLight(NPC.Center + new Vector2(0, 20), GlassLights);
            Lighting.AddLight(NPC.Center + new Vector2(0, 60), GlassLights);
            #endregion

            NPC.spriteDirection = -1;
            AttackTimer--;

            #region Summon Minions

            if (!summonedMinions && NPC.life < NPC.lifeMax * 0.35f)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    return;
                }

                int index = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<StellarShip1>(), NPC.whoAmI);
                NPC minionNPC = Main.npc[index];

                if (minionNPC.ModNPC is StellarShip1 minion)
                {
                    // This checks if our spawned NPC is indeed the minion, and casts it so we can access its variables
                    minion.ParentIndex = NPC.whoAmI; // Let the minion know who the "parent" is
                }

                if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
                

                int index2 = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<StellarShip2>(), NPC.whoAmI);
                NPC minionNPC2 = Main.npc[index2];

                if (minionNPC2.ModNPC is StellarShip2 minion2)
                {
                    // This checks if our spawned NPC is indeed the minion, and casts it so we can access its variables
                    minion2.ParentIndex = NPC.whoAmI; // Let the minion know who the "parent" is
                }

                if (Main.netMode == NetmodeID.Server && index2 < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: index2);
                }

                summonedMinions = true;
            }

            #endregion

        }

        /*
        private Player TargetStrongest()
        {
            int target = -1;

            for (var i = 0; i < Main.maxPlayers; i++)
            {
                if (target == -1)
                {
                    target = Main.player[i];
                }
            }
            return NPC.TargetClosest(false);
        }
        */
    }
}
