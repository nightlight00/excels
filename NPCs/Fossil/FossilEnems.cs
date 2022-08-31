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
using System.IO;
using Terraria.Audio;
using Terraria.GameContent;

namespace excels.NPCs.Fossil
{
    internal class RexxieSkull : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.Poisoned,
                    BuffID.Frostburn,
                    BuffID.Frostburn2
                }
            });
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm,

				new FlavorTextBestiaryInfoElement("The fossil of an acient king, detached from it's body long ago"),
    		});
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.AncientFossil>(), 3, 1, 2));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneSandstorm && Main.hardMode)
            {
                return 0.1f;
            }
            return 0;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Crimslime);
            AIType = NPCID.Crimslime;
            AnimationType = NPCID.Crimslime;

            NPC.lifeMax = 320;
            NPC.defense = 30;
            NPC.damage = 65;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.alpha = 0;
            NPC.knockBackResist = 0.7f;
            NPC.value = 500;
            NPC.npcSlots = 1;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerRexxie>();
        }

        public override void AI()
        {
            if (--NPC.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.NPCHit2, NPC.Center);
                NPC.soundDelay = 270 + Main.rand.Next(100);
            }

            if (NPC.velocity.Y != 0)
            {
                if (NPC.velocity.X < 0)
                {
                    NPC.spriteDirection = 1;
                }
                else
                {
                    NPC.spriteDirection = -1;
                }
            }
            else
            {
                if (Main.player[NPC.target].Center.X < NPC.Center.X)
                {
                    NPC.spriteDirection = 1;
                }
                else
                {
                    NPC.spriteDirection = -1;
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num785 = 0;
                while ((double)num785 < damage / (double)NPC.lifeMax * 100.0)
                {
                    int num786 = ModContent.DustType<Dusts.FossilBoneDust>();
                    if (Main.rand.Next(4) == 0)
                    {
                        num786 = DustID.Sand;
                    }
                    Dust d = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, num786, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 0, default(Color), 2f);
                    num785++;
                    if (num786 == DustID.Sand)
                    {
                        d.scale = Main.rand.NextFloat(0.7f, 0.9f);
                    }
                }
                return;
            }
            for (int num788 = 0; num788 < 40; num788++)
            {
                int num786 = ModContent.DustType<Dusts.FossilBoneDust>();
                if (Main.rand.Next(4) == 0)
                {
                    num786 = DustID.Sand;
                }
                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, num786, (float)(2 * hitDirection), -2f, 0, default(Color), 1f);
                //num = num788;
                if (num786 == DustID.Sand)
                {
                    d.scale = Main.rand.NextFloat(0.7f, 0.9f);
                }
            }
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, (NPC.velocity * -0.3f).RotatedByRandom(MathHelper.ToRadians(20)), Mod.Find<ModGore>("RexxieGoreHead").Type);
            Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, (NPC.velocity * -0.3f).RotatedByRandom(MathHelper.ToRadians(20)), Mod.Find<ModGore>("RexxieGoreJaw").Type);
        }
    }

    internal class Fossilraptor : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fossiliraptor");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.Poisoned,
                    BuffID.Frostburn,
                    BuffID.Frostburn2
                }
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode && spawnInfo.Player.ZoneSandstorm)
            {
                return 0.12f;
            }
            return 0;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm,

                new FlavorTextBestiaryInfoElement("The fossil of a quick raptor, its a miracle that it's body is still mostly intact"),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.AncientFossil>(), 3, 1, 2));
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.GiantWalkingAntlion);
            AIType = NPCID.GiantWalkingAntlion;
            AnimationType = NPCID.GiantWalkingAntlion;

            NPC.lifeMax = 470;
            NPC.defense = 28;
            NPC.damage = 80;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.knockBackResist = 0.4f;
            NPC.value = 675;
            NPC.width = 70;
            NPC.height = 40;
            NPC.npcSlots = 1;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerFossiliraptor>();
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num785 = 0;
                while ((double)num785 < damage / (double)NPC.lifeMax * 100.0)
                {
                    int num786 = ModContent.DustType<Dusts.FossilBoneDust>();
                    if (Main.rand.Next(4) == 0)
                    {
                        num786 = DustID.Sand;
                    }
                    Dust d = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, num786, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 0, default(Color), 2f);
                    num785++;
                    if (num786 == DustID.Sand)
                    {
                        d.scale = Main.rand.NextFloat(0.7f, 0.9f);
                    }
                }
                return;
            }
            for (int num788 = 0; num788 < 40; num788++)
            {
                int num786 = ModContent.DustType<Dusts.FossilBoneDust>();
                if (Main.rand.Next(4) == 0)
                {
                    num786 = DustID.Sand;
                }
                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, num786, (float)(2 * hitDirection), -2f, 0, default(Color), 1f);
                //num = num788;
                if (num786 == DustID.Sand)
                {
                    d.scale = Main.rand.NextFloat(0.7f, 0.9f);
                }
            }
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, (NPC.velocity * -0.3f).RotatedByRandom(MathHelper.ToRadians(20)), Mod.Find<ModGore>("RaptorGoreHead").Type);
            for (var g = 0; g < 2; g++)
            {
                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, (NPC.velocity * -0.4f).RotatedByRandom(MathHelper.ToRadians(20)), Mod.Find<ModGore>("RaptorGoreLeg").Type);
                Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, (NPC.velocity * -0.45f).RotatedByRandom(MathHelper.ToRadians(20)), Mod.Find<ModGore>("RaptorGoreRibs").Type);
            }
        }
    }
}
