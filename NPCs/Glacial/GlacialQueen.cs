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

namespace excels.NPCs.Glacial
{
    [AutoloadBossHead]
    public class GlacialQueen : ModNPC
    {
        public override string Texture => "excels/NPCs/Glacial/Niflheim";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Niflheim");
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Confused,
                    BuffID.Frostburn,
                    BuffID.Slow,
                    BuffID.Bleeding
				}
            });
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, de);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Position = new Vector2(0f, 50f),
                PortraitScale = 1.05f
                //PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
           // Main.npcFrameCount[Type] = 8;

            NPCID.Sets.TrailCacheLength[NPC.type] = 9; // The length of old position to be recorded
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.MPAllowedEnemies[Type] = true; 
        }

        public static int secondStageHeadSlot = -1;
        /*
        public override void Load()
        {
            // We want to give it a second boss head icon, so we register one
            string texture = BossHeadTexture + "_SecondStage"; // Our texture is called "ClassName_Head_Boss_SecondStage"
            secondStageHeadSlot = Mod.AddBossHeadTexture(texture, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise
        }

        public override void BossHeadSlot(ref int index)
        {
            int slot = secondStageHeadSlot;
            if (phase2 && slot != -1)
            {
                // If the boss is in its second stage, display the other head icon instead
                index = slot;
            }
        }
        */
        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.npcSlots = 50;

            NPC.lifeMax = 4200;
            NPC.defense = 8;
            NPC.damage = 10;
            NPC.knockBackResist = 0;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 52;
            NPC.height = 172;
            NPC.coldDamage = true;
            NPC.value = 60000;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath6;

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Audio/Music/Boss/niflheim");
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (var du = 0; du < 40; du++)
                {
                    Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 92);
                    d.velocity = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-4, -1));
                    d.scale = Main.rand.NextFloat(1.2f, 1.5f);
                    d.fadeIn = d.scale * 1.4f;
                    d.noLight = true;
                }

                if (Main.netMode == NetmodeID.Server)
                {
                    // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                    return;
                }
                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, new Vector2(Main.rand.NextFloat(-2, 2), -5), Mod.Find<ModGore>("NiflheimGoreHead").Type);
                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, new Vector2(Main.rand.NextFloat(-2, 2), -5), Mod.Find<ModGore>("NiflheimGoreIce2").Type);
                for (var a = 0; a < 4; a++)
                {
                    Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-5, -2)), Mod.Find<ModGore>("NiflheimGoreArm").Type);
                }
                for (var i = 0; i < 3; i++)
                {
                    Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-5, -2)), Mod.Find<ModGore>("NiflheimGoreIce1").Type);
                }

            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Decorations.Trophies.NiflheimTrophy>(), 10));
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Bags.BagNiflheim>()));

            // rule that checks if not expert mode
            LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
            leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.GlacialOre>(), 1, 44, 66));
          //  npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Materials.GlacialOre>(), 1, 44, 66));
            int[] WeapDrops = {
                ModContent.ItemType<Items.Weapons.Glacial.Boss.DarkEye>(),
                ModContent.ItemType<Items.Weapons.Glacial.Boss.Nastrond>(),
                ModContent.ItemType<Items.Weapons.Glacial.Boss.FrozenChainblade>(),
                ModContent.ItemType<Items.Weapons.ThrowPotions.SnowSpellPot>()
            };
            leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Random.SnowflakeAmulet>(), 2));
            leadingConditionRule.OnSuccess(ItemDropRule.OneFromOptions(1, WeapDrops));
            leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Misc.SnowFlower>()));
            npcLoot.Add(leadingConditionRule);

            // rule that checks if in master mode
            LeadingConditionRule leadingConditionRule2 = new LeadingConditionRule(new Conditions.IsMasterMode());
            leadingConditionRule2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Decorations.Relics.NiflheimRelic>(), 1));
            npcLoot.Add(leadingConditionRule2);
        }

        public override void OnKill()
        {
            if (!excelWorld.downedNiflheim) 
            {
                for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 0.00056f); k++)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);

                    Tile tile = Framing.GetTileSafely(x, y);
                    if (!tile.IsActuated && (tile.TileType == TileID.SnowBlock || tile.TileType == TileID.IceBlock || tile.TileType == TileID.CorruptIce || tile.TileType == TileID.FleshIce || tile.TileType == TileID.HallowedIce))
                    {
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 9), WorldGen.genRand.Next(3, 4), ModContent.TileType<Tiles.OresBars.GlacialOreTile>());
                    }

                }

                string TXT = "The ice and snow is filled with cryogenic powers";

                if (Main.netMode != NetmodeID.Server)
                {
                    string text = Language.GetTextValue("The ice and snow are filled with cryogenic powers", Lang.GetNPCNameValue(NPC.type), TXT);
                    Main.NewText(text, 0, 155, 252);
                }
                else
                {
                    NetworkText text = NetworkText.FromKey("The ice and snow are filled with cryogenic powers", Lang.GetNPCNameValue(NPC.type), TXT);
                    //Chat.ChatHelper.BroadcastChatMessage(text, new Color(0, 155, 252));
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(text, new Color(0, 155, 252));
                }

                excelWorld.downedNiflheim = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }
            NPC.SetEventFlagCleared(ref excelWorld.downedNiflheim, -1);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Born from the prayers of her followers, Niflheim watches over her domain with her forever present gaze")
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;

            if (phase2)
            {
                Main.instance.LoadNPC(NPC.type);
                Texture2D texture = TextureAssets.Npc[NPC.type].Value;
                texture.Frame(1, 8, 0, NPC.frame.Y);

                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                }
            }

            return true;
        }

        // variables //

        // movement
        float Xspeed;
        float Yspeed;
        float maxSpeed = 6.8f;
        float speedInc = 0.063f;

        // attacks
        int attackType = -1;
        int attackTimer = 0;
        int waitTimer = 40; // to give breathing room during the fight

        // transformation
        bool phase2 = false;
        bool transforming = false;
        int iceArmor = 0;

        public override void AI()
        {
            NPC.spriteDirection = 0;

            // get a player to target
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            // despawn if players are dead
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }

            #region Transformation
            if (NPC.life < NPC.lifeMax * 0.33f && !phase2)
            {
                // so cant kill boss before phase 2
                NPC.immortal = true;
                if (transforming)
                {
                    NPC.alpha -= 2;
                    if (NPC.alpha <= 0)
                    {
                        NPC.immortal = false;
                        phase2 = true;
                        NPC.damage = 24;
                    }
                    else
                    {
                        if (iceArmor > 4)
                        {
                            Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GlacialArmor>(), 14, 0);
                            p.ai[1] = 1;
                            Projectile p2 = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GlacialArmor>(), 14, 0);
                            p2.ai[1] = -1;
                            iceArmor = 0;
                        }
                        iceArmor++;

                        Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 185);
                        Vector2 shootVel = d.position - NPC.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= -4;
                        d.velocity = shootVel;
                        d.alpha = NPC.alpha;
                        d.noGravity = true;
                        return;
                    }
                }
                else
                {
                    NPC.alpha += 5;
                }
                if (NPC.alpha > 250)
                {
                    // this starts phase2 and sets new stats
                    NPC.defense += 6;
                    NPC.life = (int)(NPC.lifeMax * 0.33f) - 1; // reset to this, -1 so its not an infinite loop
                    NPC.velocity = Vector2.Zero;
                    NPC.rotation = NPC.velocity.ToRotation();
                    NPC.alpha = 250;
                    NPC.Center = player.Center - new Vector2(0, 200);
                    NPC.damage = -1;
                    attackTimer = 0;
                    attackType = -1;
                    waitTimer = 20;
                    Xspeed = 0;
                    Yspeed = 0;
                    maxSpeed += 1.3f;
                    speedInc += 0.007f;
                    transforming = true;
                    SoundEngine.PlaySound(SoundID.Item28, NPC.Center);
                }
                if (NPC.alpha > 180)
                {
                    // prevent rest of code from running
                    return;
                }
            }


            #endregion

            #region Movement
            if (player.Center.X < (NPC.Center.X - 40))
            {
                Xspeed -= speedInc;
                if (Xspeed < -maxSpeed) 
                {
                    Xspeed = -maxSpeed;
                }
            }
            else if (player.Center.X > (NPC.Center.X + 40))
            {
                Xspeed += speedInc;
                if (Xspeed > maxSpeed)
                {
                    Xspeed = maxSpeed;
                }
            }
            if (player.Center.Y < (NPC.Center.Y - 40))
            {
                Yspeed -= speedInc * 0.8f;
                if (Yspeed < -maxSpeed)
                {
                    Yspeed = -maxSpeed;
                }
            }
            else if (player.Center.Y > (NPC.Center.Y + 40))
            {
                Yspeed += speedInc * 0.8f;
                if (Yspeed > maxSpeed)
                {
                    Yspeed = maxSpeed;
                }
            }
            else
            {
                Yspeed *= 0.95f;
            }

            NPC.velocity = new Vector2(Xspeed, Yspeed);
            NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 2);
            #endregion

            #region Attacks
            if (waitTimer < 0)
            {
                // choose attack
                attackType = Main.rand.Next(4);
                if (phase2) { attackType += 1; }
                switch (attackType)
                {
                    default: 
                    case 0:
                    case 1:
                        // snowball attack
                        attackTimer = 90;
                        waitTimer = 60;
                        if (Main.masterMode) attackTimer += 20;
                        break;
                    case 2:
                        // homing frost fire
                        attackTimer = 60; // 1 attack every 20 ticks - 3
                        if (Main.expertMode && phase2) { attackTimer = 90; } // 1 attack every 15 ticks - 6
                        else if (phase2) { attackTimer = 75; } // 1 attack every 15 ticks - 5
                        else if (Main.expertMode) { attackTimer = 80; } // 1 attack every 20 ticks - 4
                        // expert mode just increase projectile count by 1 and lowers ticks needed
                        if (Main.masterMode) { attackTimer += 15; }
                        // master mode makes 1 more attack happen
                        waitTimer = 50;
                        break;
                    case 3:
                        // spawn aura on player that spawns minions
                        attackTimer = 30;
                        waitTimer = 35;
                        int amt = 4;
                        if (Main.expertMode) amt = 6;
                        if (NPC.CountNPCS(ModContent.NPCType<GlacialAngel>()) + NPC.CountNPCS(ModContent.NPCType<GlacialBeholder>()) > amt)
                        {
                            // redo attack randomizer
                            attackTimer = -1;
                            waitTimer = -1;
                        }
                        break;
                    case 4:
                        // frost auras near player
                        attackTimer = 40;
                        waitTimer = 50;
                        break;
                }
                if (Main.masterMode) waitTimer = (int)(waitTimer * 0.9f);
                if (phase2) waitTimer = (int)(waitTimer * 0.8f);
            }
            // perform attacks
            if (attackTimer > 0)
            {
                switch (attackType)
                {
                    default:
                        // snowballs
                        int vel = 0;
                        if (phase2) vel = 1;
                        if (attackTimer % (20 - (vel * 2)) == 0)
                        {
                            Vector2 shootVel = player.Center - NPC.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= 8 + (vel * 4);
                            shootVel = new Vector2(shootVel.X / 2, -5);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-60, 0), shootVel, ModContent.ProjectileType<SnowBalls>(), 10, 0.2f);
                            SoundEngine.PlaySound(SoundID.Item51, NPC.Center + new Vector2(-60, 0));
                        }
                        else if (attackTimer % (20 - (vel * 2)) == 10 - vel)
                        {
                            Vector2 shootVel = player.Center - NPC.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= 8 + (vel * 4);
                            shootVel = new Vector2(shootVel.X / 2, -5);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + new Vector2(60, 0), shootVel, ModContent.ProjectileType<SnowBalls>(), 8, 0.2f);
                            SoundEngine.PlaySound(SoundID.Item51, NPC.Center + new Vector2(60, 0));
                        }
                        break;
                    case 2:
                        // homing frost bolts
                        int time = 0;
                        if (phase2) { time = 5; }
                        if (attackTimer % (20 - time) == 0)
                        {
                            Vector2 shootVel = player.Center - NPC.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= 6;
                            shootVel = new Vector2(shootVel.X / 3, -5);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Top, shootVel, ModContent.ProjectileType<FrostFire>(), 17, 0);
                            SoundEngine.PlaySound(SoundID.Item28, NPC.Top);
                        }
                        break;
                    case 3:
                        // minion spawn
                        if (attackTimer == 30)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<FrostAura>(), 20, 0);
                            SoundEngine.PlaySound(SoundID.Item120, player.Center);
                        }
                        break;
                    case 4:
                        // frost auras
                        int Dist = 150 + Main.rand.Next(15);
                        int amt = 20;
                        if (Main.expertMode) amt -= 5;
                        if (attackTimer >= amt && attackTimer % 5 == 0)
                        {
                            int Xoff = Main.rand.Next(-Dist, Dist);
                            int Yoff = Main.rand.Next(-(Dist - Math.Abs(Xoff)), Dist - Math.Abs(Xoff));
                            Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), player.Center + new Vector2(Xoff, Yoff), Vector2.Zero, ModContent.ProjectileType<FrostAura>(), 20, 0);
                            p.ai[1] = 1;
                        }
                        SoundEngine.PlaySound(SoundID.Item120, player.Center);
                        break;
                }
                attackTimer--;
            }
            else
            {
                // decrease wait timer when attack done
                waitTimer--;
            }

            #endregion

            NPC.netUpdate = true;

            // dust in phase 2
            if (phase2)
            {
                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 185);
                Vector2 shootVel = d.position - NPC.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= -4;
                d.velocity = shootVel;
                d.alpha = 30;
                d.noGravity = true;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            return;

            // the number its multiplied is the frame wanted
            // ex 1 = second frame
            int phaseDraw = 0;
            if (phase2 || transforming) phaseDraw = 1;
            // order these in importance
            if (NPC.immortal) // 1 - transform
            {
                NPC.frame.Y = frameHeight * (2 + phaseDraw);
            }
            else if (attackTimer > 0) // 2 - attacks
            {
                switch (attackType)
                {
                    default: NPC.frame.Y = frameHeight * (2 + phaseDraw); break;
                    case 2: case 4: NPC.frame.Y = frameHeight * (4 + phaseDraw); break;
                }
            }
            else // 3 - idle/waiting
            {
                // 0 = first frame
                NPC.frame.Y = frameHeight * (0 + phaseDraw);
            }
        }
    }
}
