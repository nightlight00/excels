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

namespace excels.NPCs.Dungeon
{
    public class DungeonMimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // manually copied from mimic immunties
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.Confused,
                    BuffID.Poisoned
                }
            });
            Main.npcFrameCount[Type] = 18;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of NPC NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement("Mimics are very dangerous, as many dungeon goers foolishly drop their guard around treasure chests.  Some experts argue that the Mimic is the true master of any dungeon, as no monster is more at home than the Mimic.")
                //new FlavorTextBestiaryInfoElement("Talking to a chest doesn't cause craziness, but if the chest answers back, it may cause death! It still contains rare treasure, regardless!")
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Mimic);
            NPC.lifeMax = 250;
            NPC.defense = 15;
            NPC.damage = 40;
            AnimationType = NPCID.Mimic;
            NPC.value = 10000;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Tiles.Decorations.Banners.BItems.BannerDungeonMimic>();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            int[] MimicDrops = {
                ModContent.ItemType<Items.Weapons.MageGun.ThunderLord>(),
                ModContent.ItemType<Items.Weapons.Boomerang.Glaive>(),
                ModContent.ItemType<Items.Accessories.Random.MimicToothNecklace>(),
                ModContent.ItemType<Items.Accessories.Cleric.Necrotic.SkullPendant>()
            };
            npcLoot.Add(ItemDropRule.OneFromOptions(1, MimicDrops));
            int[] SpecialLoot = {
                ItemID.GoldenKey,
                ItemID.LockBox
            };
            npcLoot.Add(ItemDropRule.OneFromOptions(6, SpecialLoot));
            //npcLoot.Add(ItemDropRule.Common(ItemID.GoldenKey, 6));
            
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneDungeon && !NPC.AnyNPCs(ModContent.NPCType<DungeonMimic>()) && NPC.downedBoss3)
            {
                return 0.03f;
            }
            return 0;
        }

        int num = 0;

        public override void HitEffect(int hitDirection, double damage)
        {
            int num672 = 7;
            if (NPC.ai[3] == 2f)
            {
                num672 = 10;
            }
            if (NPC.ai[3] == 3f)
            {
                num672 = 37;
            }
            if (NPC.life > 0)
            {
                int num673 = 0;
                while ((double)num673 < damage / (double)NPC.lifeMax * 50.0)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, num672, 0f, 0f, 0, default(Color), 1f);
                    num = num673;
                    num673 = num + 1;
                }
                return;
            }
            for (int num674 = 0; num674 < 20; num674 = num + 1)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, num672, 0f, 0f, 0, default(Color), 1f);
                num = num674;
            }
            int num675 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), 61, NPC.scale);
            Gore gore2 = Main.gore[num675];
            gore2.velocity *= 0.3f;
            num675 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2((float)hitDirection, 0f), 62, NPC.scale);
            gore2 = Main.gore[num675];
            gore2.velocity *= 0.3f;
            num675 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 10f), new Vector2((float)hitDirection, 0f), 63, NPC.scale);
            gore2 = Main.gore[num675];
            gore2.velocity *= 0.3f;
        }
    }
}
