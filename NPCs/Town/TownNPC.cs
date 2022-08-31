using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.DataStructures;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.ModLoader.IO;

namespace excels.NPCs.Town
{

    [AutoloadHead]
    internal class Priestess : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 26;

            NPCID.Sets.ExtraFramesCount[Type] = 9; 
            NPCID.Sets.AttackFrameCount[Type] = 4;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f, 
                Direction = -1  
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Love) // chilly room 
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
                .SetBiomeAffection<MushroomBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate)

                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Love) 
                .SetNPCAffection(NPCID.Truffle, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Painter, AffectionLevel.Like)
                .SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate)
                .SetNPCAffection(NPCID.Nurse, AffectionLevel.Hate)
            ;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
			    new FlavorTextBestiaryInfoElement("Mods.excels.Bestiary.Priestess")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCID.Guide;
            NPC.CloneDefaults(NPCID.Guide);

            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.friendlyRegen = 6;

            NPC.lifeMax = 250;
            AnimationType = NPCID.Guide;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Alice",
                "Ophilia",
                "Lianna",
                "Laurenne",
                "Ro",
                "Sophia",
                "Olivia",
                "Macy",
                "Astrid",
                "Stephanie",
                "Luna",
                "Pomera",
                "Chtolly"
            };
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return excelWorld.downedNiflheim;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
            button2 = "Bless";
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            if (NPC.FindFirstNPC(NPCID.Painter) >= 0)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Furniture.Paintings.ReflectivePrayer>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 75);
                nextSlot++;
            }

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.WeaponHeal.Holyiest.Prophecy>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 35);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Accessories.Cleric.Healing.MiniatureCross>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 20);
            nextSlot++;

            // Priest armor
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Priest.PriestHelmet>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 15);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Priest.PriestChest>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 15);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Priest.PriestBoots>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 15);
            nextSlot++;


            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.WeaponHeal.Generic.ThrowableHealthPotion>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 70);
            nextSlot++;
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            // Chat that doesn't have any special requirements
            chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Standard1"), 1);
            chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Standard2"), 1);
            chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Standard3"), 0.8f);

            // Dialogue about the priest's armor set : If the player is wearing it or not
            if (Main.LocalPlayer.GetModPlayer<excelPlayer>().PriestSet)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.PriestArmorEquipped"), 1);
            }
            else
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Standard4"), 1);
            }

            // Hardmode only
            if (Main.hardMode)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Hardmode1"), 1);
            }

            // NPC specific dialogue
            int demoman = NPC.FindFirstNPC(NPCID.Demolitionist);
            if (demoman >= 0)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Demolitionist", Main.npc[demoman].GivenName), 0.8f);
            }

            int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
            if (armsDealer >= 0)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.ArmsDealer", Main.npc[armsDealer].GivenName), 0.8f);
            }

            int nurse = NPC.FindFirstNPC(NPCID.Nurse);
            if (nurse >= 0)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Nurse", Main.npc[nurse].GivenName), 0.8f);
            }

            int painter = NPC.FindFirstNPC(NPCID.Painter);
            if (painter >= 0)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Painter", Main.npc[painter].GivenName), 0.8f);
            }

            // Weather specific dialogue
            if (Main.raining)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Raining"), 1);
            }
            if (!Main.raining && !Main.IsItStorming)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.NoBadWeather"), 1);
            }
            if (Main.IsItAHappyWindyDay)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.Windy"), 1);
            }

            // Boss specific dialogue
            if (NPC.downedAncientCultist)
            {
                chat.Add(Language.GetTextValue("Mods.excels.Dialogue.Priestess.DownedCultist"), 1);
            }

            return chat;
        }

        public override bool UsesPartyHat() => false;

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
            else
            {
                // if player has either of them
                if (Main.LocalPlayer.HasBuff(ModContent.BuffType<Buffs.ClericBonus.PriestessBlessingRadiance>()) || Main.LocalPlayer.HasBuff(ModContent.BuffType<Buffs.ClericBonus.PriestessBlessingNecrotic>()))
                {
                    Main.npcChatText = Language.GetTextValue("Mods.excels.Dialogue.Priestess.BlessingAlreadyRecieved");
                }
                else
                {
                    // play sound when getting blessed
                    SoundEngine.PlaySound(SoundID.Item4, NPC.Center);

                    Player mp = Main.LocalPlayer;
                    float necrostrength = (1 + mp.GetModPlayer<ClericClassPlayer>().clericNecroticAdd) * mp.GetModPlayer<ClericClassPlayer>().clericNecroticMult;
                    float radiantstrength = (1 + mp.GetModPlayer<ClericClassPlayer>().clericRadiantAdd + (mp.GetModPlayer<excelPlayer>().healBonus * 0.05f)) * mp.GetModPlayer<ClericClassPlayer>().clericRadiantMult;
                    // healing bonuses give .05

                    // if necrotic is stronger
                    if (necrostrength > radiantstrength)
                    {
                        Main.npcChatText = Language.GetTextValue("Mods.excels.Dialogue.Priestess.BlessingNecrotic");
                        Main.LocalPlayer.AddBuff(ModContent.BuffType<Buffs.ClericBonus.PriestessBlessingNecrotic>(), 36000);
                    }
                    else
                    {
                        // will normally default to this, radiant is equal or stronger
                        Main.npcChatText = Language.GetTextValue("Mods.excels.Dialogue.Priestess.BlessingRadiant");
                        Main.LocalPlayer.AddBuff(ModContent.BuffType<Buffs.ClericBonus.PriestessBlessingRadiance>(), 36000);
                    }
                }
                // give player healer buff
            }
        }
    }

}
