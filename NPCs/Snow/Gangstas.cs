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
namespace excels.NPCs.Snow
{
    internal class StabbyJr : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Frostburn,
                    BuffID.Frostburn2,
                    BuffID.Poisoned
                }
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Crimslime);
            AIType = NPCID.Crimslime;
            AnimationType = NPCID.Crimslime;

            NPC.damage = 48;
            NPC.defense = 18;
            NPC.lifeMax = 178;
            NPC.knockBackResist = 0.6f;
            NPC.value = 280;
            NPC.alpha = 0;
            NPC.coldDamage = true;
            NPC.HitSound = SoundID.NPCHit11;
            NPC.DeathSound = SoundID.NPCDeath15;

           // Banner = NPC.type;
          //  BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerMeteorSlime>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion,
                //new FlavorTextBestiaryInfoElement("Pieces of meteorite managed to find itself stuck withen a slime")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.FrostLegion.Chance * 0.15f;
        }

        public override void AI()
        {
            if (NPC.velocity.Y != 0)
            {
                if (NPC.velocity.X < 0)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
            }
            else
            {
                if (Main.player[NPC.target].Center.X < NPC.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SnowBlock, 1, 2, 6));
            npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Items.Food.Carrot>(), 75));
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num380 = 0;
                while ((double)num380 < damage / (double)NPC.lifeMax * 100.0)
                {
                    int num381 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 76, (float)hitDirection, -1f, 0, default(Color), 1f);
                    Main.dust[num381].noGravity = true;
                    num380++;
                }
            }
            else
            {
                for (int num382 = 0; num382 < 50; num382++)
                {
                    int num383 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 76, (float)hitDirection, -1f, 0, default(Color), 1f);
                    Main.dust[num383].noGravity = true;
                    Dust dust = Main.dust[num383];
                    dust.scale *= 1.2f;
                }
            }
        }
    }

    internal class SnowCasta : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Frostburn,
                    BuffID.Frostburn2,
                    BuffID.Poisoned
                }
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.SnowmanGangsta);
            //AIType = NPCID.SnowmanGangsta;
            AnimationType = NPCID.SnowmanGangsta;

            NPC.damage = 30;
            NPC.defense = 20;
            NPC.lifeMax = 190;
            NPC.knockBackResist = 0.6f;
            NPC.value = 400;
            NPC.coldDamage = true;
            NPC.HitSound = SoundID.NPCHit11;
            NPC.DeathSound = SoundID.NPCDeath15;

            // Banner = NPC.type;
            //  BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerMeteorSlime>();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SnowBlock, 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.MafiosoHat>(), 50));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Staff1.FrozenScarf>(), 40));
            npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Items.Food.Carrot>(), 75));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion,
                //new FlavorTextBestiaryInfoElement("Pieces of meteorite managed to find itself stuck withen a slime")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.FrostLegion.Chance * 0.3f;
        }

        public override void AI()
        {
            NPC.ai[2]++;
            if (NPC.ai[2] > 140)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Icicle>());
                NPC.netUpdate = true;
                NPC.ai[2] = 0;
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num380 = 0;
                while ((double)num380 < damage / (double)NPC.lifeMax * 100.0)
                {
                    int num381 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 76, (float)hitDirection, -1f, 0, default(Color), 1f);
                    Main.dust[num381].noGravity = true;
                    num380++;
                }
            }
            else
            {
                for (int num382 = 0; num382 < 50; num382++)
                {
                    int num383 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 76, (float)hitDirection, -1f, 0, default(Color), 1f);
                    Main.dust[num383].noGravity = true;
                    Dust dust = Main.dust[num383];
                    dust.scale *= 1.2f;
                }
            }
        }
    }

    internal class SnowGolem : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Frostburn,
                    BuffID.Frostburn2,
                    BuffID.Poisoned
                }
            });
        }



        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.SnowmanGangsta);
            //AIType = NPCID.SnowmanGangsta;
            AnimationType = NPCID.SnowmanGangsta;

            NPC.damage = 70;
            NPC.defense = 30;
            NPC.lifeMax = 2500;
            NPC.knockBackResist = 0.2f;
            NPC.value = 400;

            NPC.coldDamage = true;
            NPC.HitSound = SoundID.NPCHit11;
            NPC.DeathSound = SoundID.NPCDeath15;

            // Banner = NPC.type;
            //  BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerMeteorSlime>();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SnowBlock, 1, 10, 25));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Guns1.SnowballCannonEX>(), 6));
            npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Items.Food.Carrot>(), 75));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion,
                //new FlavorTextBestiaryInfoElement("Pieces of meteorite managed to find itself stuck withen a slime")
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num380 = 0;
                while ((double)num380 < damage / (double)NPC.lifeMax * 100.0)
                {
                    int num381 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 76, (float)hitDirection, -1f, 0, default(Color), 1f);
                    Main.dust[num381].noGravity = true;
                    num380++;
                }
            }
            else
            {
                for (int num382 = 0; num382 < 50; num382++)
                {
                    int num383 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 76, (float)hitDirection, -1f, 0, default(Color), 1f);
                    Main.dust[num383].noGravity = true;
                    Dust dust = Main.dust[num383];
                    dust.scale *= 1.2f;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.CountNPCS(ModContent.NPCType<SnowGolem>()) == 0)
            {
                return SpawnCondition.FrostLegion.Chance * 0.1f;
            }
            return 0f;
        }

        bool RapidFire = false;
        int RapidCount = 0;
        public override void AI()
        {
            NPC.ai[2]++;
            if (!RapidFire)
            {
                if (NPC.ai[2] > 300)
                {
                    RapidFire = true;
                }
            }
            else if (NPC.ai[2] > 8)
            {
                NPC.TargetClosest(true);
                Vector2 vel = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.Zero).RotatedByRandom(MathHelper.ToRadians(18)) * Main.rand.NextFloat(15, 18);
                Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, vel, ProjectileID.SnowBallFriendly, 30, 2);
                p.friendly = false;
                p.hostile = true;
                p.netUpdate = true;
                NPC.netUpdate = true;

                RapidCount++;
                if (RapidCount > 30)
                {
                    RapidCount = 0;
                    RapidFire = false;
                }
                NPC.ai[2] = 0;
            }
        }
    }

    internal class Icicle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.ChaosBall);
            NPC.coldDamage = true;
            NPC.aiStyle = -1;
            NPC.damage = 40;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 240);
        }

        public override void AI()
        {
            if (NPC.target == 255)
            {
                NPC.TargetClosest(true);
                float num127 = 6f;
                if (NPC.type == 25)
                {
                    num127 = 5f;
                }
                if (NPC.type == 112 || NPC.type == 666)
                {
                    num127 = 7f;
                }
                Vector2 vector16 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num128 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector16.X;
                float num129 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector16.Y;
                float num130 = (float)Math.Sqrt((double)(num128 * num128 + num129 * num129));
                num130 = num127 / num130;
                NPC.velocity.X = num128 * num130;
                NPC.velocity.Y = num129 * num130;
            }

            Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 92);
            d.velocity *= -0.1f;
            d.noGravity = true;
            d.scale *= 1.15f;

            NPC.EncourageDespawn(100);

            NPC.rotation = NPC.velocity.ToRotation();

            /*
             * homing effect also takng from speel script
            float num145 = 15f;
            float num146 = 0.0833333358f;
            Vector2 center = NPC.Center;
            Vector2 center2 = Main.player[NPC.target].Center;
            Vector2 vec = center2 - center;
            vec.Normalize();
            if (vec.HasNaNs())
            {
                vec = new Vector2((float)NPC.direction, 0f);
            }
            NPC.velocity = (NPC.velocity * (num145 - 1f) + vec * (NPC.velocity.Length() + num146)) / num145;
            if (NPC.velocity.Length() < 6f)
            {
                NPC.velocity *= 1.05f;
                return;
            }
            */
        }
    }
}
