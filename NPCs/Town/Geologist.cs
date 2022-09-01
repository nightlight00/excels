using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.ModLoader.IO;
using excels.Items.Materials;

namespace excels.NPCs.Town
{
    // [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
    [AutoloadHead]
    public class Geologist : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
            // DisplayName.SetDefault("Example Person");
            Main.npcFrameCount[Type] = 21;
            //NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 3;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 1; // ProjectileID.CrystalPulse;
            NPCID.Sets.AttackTime[Type] = 30;
            NPCID.Sets.AttackAverageChance[Type] = 80;
            NPCID.Sets.HatOffsetY[Type] = 4;

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            // Set Geologist's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang
            NPC.Happiness
                //Biomes
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Love)
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
                //NPCs
                .SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Like);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;

            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 10;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Nurse;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("Mods.excels.Bestiary.Geologist")
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return excelWorld.transformedNymph;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int num = NPC.life > 0 ? 1 : 5;
            for (int k = 0; k < num; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
            }
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() { "Jewel", "Nymm", "Nephele", "Corycia", "Melania", "Kleodora", "Daphnis" };
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new();
            chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Geologist.Standard1"), 1);
            chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Geologist.Standard2"), 1);
            chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Geologist.Standard3"), 1);
            chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Geologist.Standard4"), 1);
            if (Main.hardMode)
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Geologist.Hardmode1"), 1);
            if (NPC.downedQueenSlime)
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Geologist.PostQueenSlime"), 0.8f);
            if (currentFetchQuest > 15)
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Geologist.LanternOnly"), 0.8f);
            if (currentFetchQuest > 18)
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Geologist.NoMeteor"), 0.6f);
            int taxMan = NPC.FindFirstNPC(NPCID.TaxCollector);
            if (taxMan >= 0)
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Geologist.TaxMan"), 0.8f);
            return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = "Donate";
        }

        public static int currentFetchQuest = 0;

        public override void LoadData(TagCompound tag)
        {
            currentFetchQuest = tag.GetInt("Quest");
        }

        public override void SaveData(TagCompound tag)
        {
            tag["Quest"] = currentFetchQuest;
        }


        private bool HasProperItemCount(int ItemType, int ItemAmount)
        {
            for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
            {
                Item item = Main.LocalPlayer.inventory[i]; // Main.item[i];
                if (item.type == ItemType && item.stack >= ItemAmount)
                {
                    item.stack -= ItemAmount;
                    return true;
                }
            }
            return false;
        }

        private void DeliveredItem()
        {
            SoundEngine.PlaySound(SoundID.Item37);
            Main.npcChatText = "Thank you!  Come again for your next task!";
            Main.npcChatCornerItem = ItemID.None;
            currentFetchQuest++;
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
            else
            {
                // set the current chat, which will be changed later if there still is a quest
                Main.npcChatText = Language.GetTextValue("Mods.excels.Dialogue.Geologist.DonationCompletePreHard");
                if (Main.hardMode)
                    Main.npcChatText = Language.GetTextValue("Mods.excels.Dialogue.Geologist.DonationCompleteHard");

                switch (currentFetchQuest)
                {
                    case 0:
                        if (WorldGen.SavedOreTiers.Copper == TileID.Copper)
                        {
                            if (HasProperItemCount(ItemID.CopperOre, 5))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 5 chunks of Copper Ore, please?";
                            Main.npcChatCornerItem = ItemID.CopperOre;
                        }
                        else
                        {
                            if (HasProperItemCount(ItemID.TinOre, 5))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 5 chunks of Tin Ore, please?";
                            Main.npcChatCornerItem = ItemID.TinOre;
                        }
                        break;

                    case 1:
                        if (HasProperItemCount(ItemID.Amethyst, 2))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 2 Amethysts, please?";
                        Main.npcChatCornerItem = ItemID.Amethyst;
                        break;

                    case 2:
                        if (HasProperItemCount(ItemID.Topaz, 2))
                            DeliveredItem(); 
                        else
                            Main.npcChatText = "Would you mind fetching me 2 Topazs, please?";
                        Main.npcChatCornerItem = ItemID.Topaz;
                        break;

                    case 3:
                        if (WorldGen.SavedOreTiers.Iron == TileID.Iron)
                        {
                            if (HasProperItemCount(ItemID.IronOre, 8))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 8 chunks of Iron Ore, please?";
                            Main.npcChatCornerItem = ItemID.IronOre;
                        }
                        else
                        {
                            if (HasProperItemCount(ItemID.LeadOre, 8))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 8 chunks of Lead Ore, please?";
                            Main.npcChatCornerItem = ItemID.LeadOre;
                        }
                        break;

                    case 4:
                        if (HasProperItemCount(ItemID.Marble, 25))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 25 pieces of Marble, please?";
                        Main.npcChatCornerItem = ItemID.Marble;
                        break;

                    case 5:
                        if (HasProperItemCount(ItemID.Granite, 8))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 25 pieces of Granite, please?";
                        Main.npcChatCornerItem = ItemID.Granite;
                        break;

                    case 6:
                        if (WorldGen.SavedOreTiers.Silver == TileID.Silver)
                        {
                            if (HasProperItemCount(ItemID.SilverOre, 11))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 11 chunks of Silver Ore, please?";
                            Main.npcChatCornerItem = ItemID.SilverOre;
                        }
                        else
                        {
                            if (HasProperItemCount(ItemID.TungstenOre, 11))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 11 chunks of Tungsten Ore, please?";
                            Main.npcChatCornerItem = ItemID.TungstenOre;
                        }
                        break;

                    case 7:
                        if (HasProperItemCount(ItemID.Sapphire, 4))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 4 Sapphires, please?";
                        Main.npcChatCornerItem = ItemID.Sapphire;
                        break;

                    case 8:
                        if (HasProperItemCount(ItemID.Emerald, 4))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 4 Emeralds, please?";
                        Main.npcChatCornerItem = ItemID.Emerald;
                        break;

                    case 9:
                        if (WorldGen.SavedOreTiers.Gold == TileID.Gold)
                        {
                            if (HasProperItemCount(ItemID.GoldOre, 15))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 15 chunks of Gold Ore, please?";
                            Main.npcChatCornerItem = ItemID.GoldOre;
                        }
                        else
                        {
                            if (HasProperItemCount(ItemID.PlatinumOre, 15))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 15 chunks of Platinum Ore, please?";
                            Main.npcChatCornerItem = ItemID.PlatinumOre;
                        }
                        break;

                    case 10:
                        if (HasProperItemCount(ItemID.Ruby, 5))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 5 Rubies, please?";
                        Main.npcChatCornerItem = ItemID.Ruby;
                        break;

                    case 11:
                        if (HasProperItemCount(ItemID.Diamond, 5))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 5 Diamonds, please?";
                        Main.npcChatCornerItem = ItemID.Diamond;
                        break;

                    case 12:
                        if (HasProperItemCount(ModContent.ItemType<SkylineOre>(), 15))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 15 chunks of Skyline Ore, please?";
                        Main.npcChatCornerItem = ModContent.ItemType<SkylineOre>();
                        break;

                    case 13:
                        if (HasProperItemCount(ItemID.FossilOre, 20))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 20 Sturdy Fossils, please?";
                        Main.npcChatCornerItem = ItemID.FossilOre;
                        break;

                    case 14:
                        if (HasProperItemCount(ItemID.Amber, 5))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 5 Ambers, please?";
                        Main.npcChatCornerItem = ItemID.Amber;
                        break;

                    case 15:
                        if (HasProperItemCount(ItemID.LifeCrystal, 2))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 2 Life Crystals, please?";
                        Main.npcChatCornerItem = ItemID.LifeCrystal;
                        break;

                    case 16:
                        if (WorldGen.crimson)
                        {
                            if (HasProperItemCount(ItemID.CrimtaneOre, 20))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 20 chunks of Crimtane Ore, please?";
                            Main.npcChatCornerItem = ItemID.CrimtaneOre;
                        }
                        else
                        {
                            if (HasProperItemCount(ItemID.DemoniteOre, 20))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 20 chunks of Demonite Ore, please?";
                            Main.npcChatCornerItem = ItemID.DemoniteOre;
                        }
                        break;

                    case 17:
                        if (HasProperItemCount(ItemID.Obsidian, 15))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 15 pieces of Obsidian, please?";
                        Main.npcChatCornerItem = ItemID.Obsidian;
                        break;

                    case 18:
                        if (HasProperItemCount(ItemID.Hellstone, 25))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 25 pieces of Hellstone, please?";
                        Main.npcChatCornerItem = ItemID.Hellstone;
                        break;

                    case 19:
                        if (HasProperItemCount(ModContent.ItemType<GlacialOre>(), 25))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 25 chunks of Glacial Ore, please?";
                        Main.npcChatCornerItem = ModContent.ItemType<GlacialOre>();
                        break;

                    case 20:
                        if (!Main.hardMode)
                            break;

                        if (WorldGen.SavedOreTiers.Cobalt == TileID.Cobalt)
                        {
                            if (HasProperItemCount(ItemID.CobaltOre, 30))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 30 chunks of Cobalt Ore, please?";
                            Main.npcChatCornerItem = ItemID.CobaltOre;
                        }
                        else
                        {
                            if (HasProperItemCount(ItemID.PalladiumOre, 30))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 30 chunks of Palladium Ore, please?";
                            Main.npcChatCornerItem = ItemID.PalladiumOre;
                        }
                        break;

                    case 21:
                        if (!Main.hardMode)
                            break;
                        if (HasProperItemCount(ItemID.CrystalShard, 15))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 15 Crystal Shards, please?";
                        Main.npcChatCornerItem = ItemID.CrystalShard;
                        break;

                    case 22:
                        if (!Main.hardMode)
                            break;

                        if (WorldGen.SavedOreTiers.Mythril == TileID.Mythril)
                        {
                            if (HasProperItemCount(ItemID.MythrilOre, 36))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 36 chunks of Mythril Ore, please?";
                            Main.npcChatCornerItem = ItemID.MythrilOre;
                        }
                        else
                        {
                            if (HasProperItemCount(ItemID.OrichalcumOre, 36))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 36 chunks of Orichalcum Ore, please?";
                            Main.npcChatCornerItem = ItemID.OrichalcumOre;
                        }
                        break;

                    case 23:
                        if (!Main.hardMode)
                            break;

                        if (WorldGen.SavedOreTiers.Adamantite == TileID.Adamantite)
                        {
                            if (HasProperItemCount(ItemID.AdamantiteOre, 42))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 42 chunks of Adamantite Ore, please?";
                            Main.npcChatCornerItem = ItemID.AdamantiteOre;
                        }
                        else
                        {
                            if (HasProperItemCount(ItemID.TitaniumOre, 42))
                                DeliveredItem();
                            else
                                Main.npcChatText = "Would you mind fetching me 42 chunks of Titanium Ore, please?";
                            Main.npcChatCornerItem = ItemID.TitaniumOre;
                        }
                        break;

                    case 24:
                        if (HasProperItemCount(ModContent.ItemType<HyperionCrystal>(), 20))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 20 Hyperion Crystals, please?";
                        Main.npcChatCornerItem = ModContent.ItemType<HyperionCrystal>();
                        break;

                    case 25:
                        if (!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3)
                        if (HasProperItemCount(ItemID.ChlorophyteOre, 50))
                            DeliveredItem();
                        else
                            Main.npcChatText = "Would you mind fetching me 50 chunks of Chlorophyte Ore, please?";
                        Main.npcChatCornerItem = ItemID.ChlorophyteOre;
                        break;
                }
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemID.MetalDetector);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 5);
            nextSlot++;

            if (NPC.downedBoss3 && NPC.downedBoss2 && currentFetchQuest > 13)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Tools.Pickaxes.DinoPick>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 20);
                nextSlot++;
            } // Fossil Battler Pickaxe

            if (currentFetchQuest > 15)
            {
                shop.item[nextSlot].SetDefaults(ItemID.HeartLantern);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50);
                nextSlot++;
            } // Heart Lantern
            if (currentFetchQuest > 4)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Marble);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 15);
                nextSlot++;
            } // Marble
            if (currentFetchQuest > 5)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Granite);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 15);
                nextSlot++;
            } // Granite
            if (currentFetchQuest > 13)
            {
                shop.item[nextSlot].SetDefaults(ItemID.DesertFossil);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 3);
                nextSlot++;
            } // Desert Fossil
            if (currentFetchQuest > 17)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Obsidian);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 1, 20);
                nextSlot++;
            } // Obsidian

            if (currentFetchQuest > 0)
            {
                if (WorldGen.SavedOreTiers.Copper == TileID.Copper) {
                    shop.item[nextSlot].SetDefaults(ItemID.CopperOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 70);
                    nextSlot++;
                }
                else {
                    shop.item[nextSlot].SetDefaults(ItemID.TinOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 85);
                    nextSlot++;
                }
            } // Copper ? Tin
            if (currentFetchQuest > 3)
            {
                if (WorldGen.SavedOreTiers.Iron == TileID.Iron) {
                    shop.item[nextSlot].SetDefaults(ItemID.IronOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 1, 20);
                    nextSlot++;
                }
                else {
                    shop.item[nextSlot].SetDefaults(ItemID.LeadOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 1, 50);
                    nextSlot++;
                }
            } // Iron ? Lead
            if (currentFetchQuest > 6)
            {
                if (WorldGen.SavedOreTiers.Silver == TileID.Silver) {
                    shop.item[nextSlot].SetDefaults(ItemID.SilverOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 2, 30);
                    nextSlot++;
                }
                else {
                    shop.item[nextSlot].SetDefaults(ItemID.TungstenOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 2, 65);
                    nextSlot++;
                }
            } // Silver ? Tungsten
            if (currentFetchQuest > 9)
            {
                if (WorldGen.SavedOreTiers.Gold == TileID.Gold) {
                    shop.item[nextSlot].SetDefaults(ItemID.GoldOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 3, 10);
                    nextSlot++;
                }
                else {
                    shop.item[nextSlot].SetDefaults(ItemID.PlatinumOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 3, 35);
                    nextSlot++;
                }
            } // Gold ? Platinum
            if (currentFetchQuest > 12)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<SkylineOre>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 3);
                nextSlot++;
            } // Skyline
            if (currentFetchQuest > 19)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<GlacialOre>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 4, 66);
                nextSlot++;
            } // Glacial
            if (currentFetchQuest > 13)
            {
                shop.item[nextSlot].SetDefaults(ItemID.FossilOre);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 3, 15);
                nextSlot++;
            } // Sturdy Fossil
            if (currentFetchQuest > 16)
            {
                if (WorldGen.crimson)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.CrimtaneOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 30);
                    nextSlot++;
                }
                else
                {
                    shop.item[nextSlot].SetDefaults(ItemID.DemoniteOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 15);
                    nextSlot++;
                }
            } // Demonite ? Crimtane
            if (currentFetchQuest > 18)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Hellstone);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 6, 40);
                nextSlot++;
            } // Hellstone
            if (Main.hardMode) {
                if (currentFetchQuest > 20)
                {
                    if (WorldGen.SavedOreTiers.Cobalt == TileID.Cobalt) { 
                        shop.item[nextSlot].SetDefaults(ItemID.CobaltOre);
                        shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 9, 50);
                        nextSlot++;
                    }
                    else { 
                        shop.item[nextSlot].SetDefaults(ItemID.PalladiumOre);
                        shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 9, 90);
                        nextSlot++;
                    }
                } // Cobalt ? Palladium
                if (currentFetchQuest > 22)
                {
                    if (WorldGen.SavedOreTiers.Mythril == TileID.Mythril) {
                        shop.item[nextSlot].SetDefaults(ItemID.MythrilOre);
                        shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 11);
                        nextSlot++;
                    }
                    else {
                        shop.item[nextSlot].SetDefaults(ItemID.OrichalcumOre);
                        shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 11, 70);
                        nextSlot++;
                    }
                } // Mythtil ? Orichalcum
                if (currentFetchQuest > 23)
                {
                    if (WorldGen.SavedOreTiers.Adamantite == TileID.Adamantite) {
                        shop.item[nextSlot].SetDefaults(ItemID.AdamantiteOre);
                        shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 12, 20);
                        nextSlot++;
                    }
                    else {
                        shop.item[nextSlot].SetDefaults(ItemID.TitaniumOre);
                        shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 13, 10);
                        nextSlot++;
                    }
                } // Adamantite ? Titanium
                if (currentFetchQuest > 25)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.ChlorophyteOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 15);
                    nextSlot++;
                } // Chlorophyte

                shop.item[nextSlot].SetDefaults(ItemID.Geode);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 85);
                nextSlot++;
            }

            if (currentFetchQuest > 1)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Amethyst);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 75);
                nextSlot++;
            } // Amythest
            if (currentFetchQuest > 2)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Topaz);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 6, 30);
                nextSlot++;
            } // Topaz
            if (currentFetchQuest > 7)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Sapphire);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 7, 35);
                nextSlot++;
            } // Sapphire
            if (currentFetchQuest > 8)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Emerald);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 7, 70);
                nextSlot++;
            } // Emerald
            if (currentFetchQuest > 10)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Ruby);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 10, 15);
                nextSlot++;
            } // Ruby
            if (currentFetchQuest > 11)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Diamond);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 11);
                nextSlot++;
            } // Diamond
            if (currentFetchQuest > 14)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Amber);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 10);
                nextSlot++;
            } // Amber
            if (Main.hardMode) {
                if (currentFetchQuest > 21)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.CrystalShard);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 9, 50);
                    nextSlot++;
                } // Crystal Shard
                if (currentFetchQuest > 24)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<HyperionCrystal>());
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 13, 50);
                    nextSlot++;
                } // Hyperion Crystal
            }
        }

        // Make this Town NPC teleport to the King and/or Queen statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return true;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = NPC.downedMoonlord ? 110 : NPC.downedPlantBoss ? 80 : Main.hardMode ? 50 : 30;
            knockback = NPC.downedMoonlord ? 5.4f : NPC.downedPlantBoss ? 4.3f : Main.hardMode ? 3.8f : 2.1f;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = NPC.downedMoonlord ? ProjectileID.CrystalPulse : NPC.downedPlantBoss ? ProjectileID.CrystalPulse : Main.hardMode ? ProjectileID.CrystalPulse : ProjectileID.DiamondBolt;
            attackDelay = NPC.downedMoonlord ? 20 : NPC.downedPlantBoss ? 24 : Main.hardMode ? 27 : 30;
        }

        public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
        {
            item = NPC.downedMoonlord ? ItemID.CrystalSerpent : NPC.downedPlantBoss ? ItemID.CrystalSerpent : Main.hardMode ? ItemID.CrystalSerpent : ItemID.DiamondStaff;
            closeness = 8;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 1f;
        }
    }
}