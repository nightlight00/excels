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

namespace excels.NPCs.Meteorite
{
    internal class FlamingSludge : ModNPC
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
                    BuffID.Confused,
                    ModContent.BuffType<Buffs.Debuffs.FragileBones>(),
                }
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.LavaSlime);
            AIType = NPCID.LavaSlime;
            AnimationType = NPCID.LavaSlime;

            NPC.damage = 60;
            NPC.defense = 10;
            NPC.lifeMax = 63;
            NPC.knockBackResist = 0.3f;
            NPC.value = 40;

            NPC.npcSlots = 0.5f;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerMeteorSlime>();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.OnFire, 360);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Meteorite, 50));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor, 
                new FlavorTextBestiaryInfoElement("Pieces of meteorite managed to find itself stuck withen a slime")
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num785 = 0;
                while ((double)num785 < damage / (double)NPC.lifeMax * 100.0)
                {
                    int num786 = 25;
                    if (Main.rand.Next(2) == 0)
                    {
                        num786 = 6;
                    }
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, num786, (float)hitDirection, -1f, 0, default(Color), 1f);
                    int num787 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 2f);
                    Main.dust[num787].noGravity = true;
                   // num = num785;
                    num785++;
                }
                return;
            }
            for (int num788 = 0; num788 < 50; num788++)
            {
                int num789 = 25;
                if (Main.rand.Next(2) == 0)
                {
                    num789 = 6;
                }
                Dust.NewDust(NPC.position, NPC.width, NPC.height, num789, (float)(2 * hitDirection), -2f, 0, default(Color), 1f);
                //num = num788;
            }
            for (int num790 = 0; num790 < 50; num790++)
            {
                int num791 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                Dust dust = Main.dust[num791];
                dust.velocity *= 6f;
                Main.dust[num791].noGravity = true;
                //num = num790;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneMeteor)
            {
                return 0.3f;
            }
            return 0;
        }
    }

    public class MeteorSpirit : ModNPC
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
                    BuffID.Confused,
                    ModContent.BuffType<Buffs.Debuffs.FragileBones>(),
                }
            });
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = 2;
            //  AIType = NPCID.LavaSlime;
            AnimationType = NPCID.MeteorHead;

            NPC.width = NPC.height = 24;
            NPC.damage = 18;
            NPC.defense = 12;
            NPC.lifeMax = 38;
            NPC.knockBackResist = 0.7f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 90;

            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;


            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerMeteorSpirit>();
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.OnFire, 360);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Meteorite, 50));
        }

        public override void AI()
        {
            NPC.rotation = NPC.velocity.X * 0.11f;
            int num3 = Dust.NewDust(new Vector2(NPC.position.X - NPC.velocity.X, NPC.position.Y - NPC.velocity.Y), NPC.width, NPC.height, 6, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 2f);
            Main.dust[num3].noGravity = true;
            Dust expr_E9D7_cp_0_cp_0 = Main.dust[num3];
            expr_E9D7_cp_0_cp_0.velocity.X = expr_E9D7_cp_0_cp_0.velocity.X * 0.3f;
            Dust expr_E9F2_cp_0_cp_0 = Main.dust[num3];
            expr_E9F2_cp_0_cp_0.velocity.Y = expr_E9F2_cp_0_cp_0.velocity.Y * 0.3f;

            // done like this so intial shot comes out sooner
            NPC.ai[3]++;
            if (NPC.ai[3] >= 90)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.BurningSphere);
                NPC.ai[3] = -200; // desired result is 290
                NPC.netUpdate = true;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,
                new FlavorTextBestiaryInfoElement("When meteorites strike the surface, their fragments lay scattered for the taking.  Some pieces are alive, and will attack")
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num785 = 0;
                while ((double)num785 < damage / (double)NPC.lifeMax * 100.0)
                {
                    int num786 = 25;
                    if (Main.rand.Next(2) == 0)
                    {
                        num786 = 6;
                    }
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, num786, (float)hitDirection, -1f, 0, default(Color), 1f);
                    int num787 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 2f);
                    Main.dust[num787].noGravity = true;
                    // num = num785;
                    num785++;
                }
                return;
            }
            for (int num788 = 0; num788 < 50; num788++)
            {
                int num789 = 25;
                if (Main.rand.Next(2) == 0)
                {
                    num789 = 6;
                }
                Dust.NewDust(NPC.position, NPC.width, NPC.height, num789, (float)(2 * hitDirection), -2f, 0, default(Color), 1f);
                //num = num788;
            }
            for (int num790 = 0; num790 < 50; num790++)
            {
                int num791 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                Dust dust = Main.dust[num791];
                dust.velocity *= 6f;
                Main.dust[num791].noGravity = true;
                //num = num790;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneMeteor)
            {
                return 0.08f;
            }
            return 0;
        }
    }

    public class MeteorGolem : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.Poisoned,
                    BuffID.Confused,
                    ModContent.BuffType<Buffs.Debuffs.FragileBones>(),
                }
            });
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = 3;
            NPC.CloneDefaults(NPCID.GraniteGolem);
            AIType = NPCID.GoblinScout;
            //  AIType = NPCID.LavaSlime;
            AnimationType = NPCID.GraniteGolem;
            NPC.damage = 42;
            NPC.defense = 16;
            NPC.lifeMax = 102;
            NPC.knockBackResist = 0.45f;
            NPC.value = 190;

            NPC.npcSlots = 1;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerMeteorGolem>();
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 360);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Meteorite, 25));
        }

        public override void AI()
        {
            int num3 = Dust.NewDust(new Vector2(NPC.position.X - NPC.velocity.X, NPC.position.Y - NPC.velocity.Y), NPC.width, NPC.height, 6, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 2f);
            Main.dust[num3].noGravity = true;
            Dust expr_E9D7_cp_0_cp_0 = Main.dust[num3];
            expr_E9D7_cp_0_cp_0.velocity.X = expr_E9D7_cp_0_cp_0.velocity.X * 0.3f;
            Dust expr_E9F2_cp_0_cp_0 = Main.dust[num3];
            expr_E9F2_cp_0_cp_0.velocity.Y = expr_E9F2_cp_0_cp_0.velocity.Y * 0.3f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,
                new FlavorTextBestiaryInfoElement("When meteorites strike the surface, their fragments lay scattered for the taking.  Some pieces are alive, and will attack")
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num785 = 0;
                while ((double)num785 < damage / (double)NPC.lifeMax * 100.0)
                {
                    int num786 = 25;
                    if (Main.rand.Next(2) == 0)
                    {
                        num786 = 6;
                    }
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, num786, (float)hitDirection, -1f, 0, default(Color), 1f);
                    int num787 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 2f);
                    Main.dust[num787].noGravity = true;
                    // num = num785;
                    num785++;
                }
                return;
            }
            for (int num788 = 0; num788 < 50; num788++)
            {
                int num789 = 25;
                if (Main.rand.Next(2) == 0)
                {
                    num789 = 6;
                }
                Dust.NewDust(NPC.position, NPC.width, NPC.height, num789, (float)(2 * hitDirection), -2f, 0, default(Color), 1f);
                //num = num788;
            }
            for (int num790 = 0; num790 < 50; num790++)
            {
                int num791 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                Dust dust = Main.dust[num791];
                dust.velocity *= 6f;
                Main.dust[num791].noGravity = true;
                //num = num790;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneMeteor)
            {
                return 0.17f;
            }
            return 0;
        }
    }
}
