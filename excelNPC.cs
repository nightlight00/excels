using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;
using System.IO;

namespace excels
{
    public class excelNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;


        public bool DebuffMycosis = false;
        public bool DebuffWound = false;
        public int MarkedTimer = 0;

        public int SpellCurse = 0;

        public override void SetDefaults(NPC npc)
        {
            /* slimes,
            if (npc.aiStyle == 1 || npc.type == NPCID.MeteorHead || npc.type == NPCID.Snatcher || npc.type == NPCID.AngryTrapper || npc.type == NPCID.GraniteGolem || npc.type == NPCID.GraniteFlyer || npc.type == NPCID.Dandelion || npc.type == NPCID.MartianDrone || npc.type == NPCID.MartianProbe || npc.type == NPCID.MartianSaucer || npc.type == NPCID.MartianWalker || npc.type == NPCID.Mimic || npc.type == NPCID.BigMimicCorruption || npc.type == NPCID.BigMimicCrimson || npc.type == NPCID.BigMimicHallow || npc.type == NPCID.BigMimicJungle || npc.type == NPCID.IceMimic || npc.type == NPCID.PresentMimic || npc.type == NPCID.Golem || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight || npc.type == NPCID.GolemHead || npc.type == NPCID.RockGolem || npc.type == NPCID.IceGolem || npc.type == NPCID.Plantera || npc.type == NPCID.KingSlime || npc.type == NPCID.QueenSlimeBoss || npc.type == NPCID.CrimsonAxe || npc.type == NPCID.CursedHammer || npc.type == NPCID.EnchantedSword || npc.type == NPCID.DeadlySphere || npc.type == NPCID.DungeonSpirit || npc.type == NPCID.Everscream || npc.type == NPCID.MourningWood || npc.type == NPCID.PirateShipCannon || npc.type == NPCID.IceElemental || npc.type == NPCID.ManEater || npc.type == NPCID.Pixie || npc.type == NPCID.Pumpking || npc.type == NPCID.SantaNK1 || npc.type == NPCID.MartianTurret || npc.type == NPCID.Reaper || npc.type == NPCID.Wraith || npc.type == NPCID.PossessedArmor)
            { 
                 NPCID.Sets.DebuffImmunitySets.Add(npc.type, new NPCDebuffImmunityData
                    {
                    SpecificallyImmuneTo = new int[] {
                        ModContent.BuffType<Buffs.Debuffs.FragileBones>(),
                    }
                });
            }
*/
        }

        public override void ResetEffects(NPC npc)
        {
            DebuffMycosis = false;
        }

        public override bool PreAI(NPC npc)
        {
            MarkedTimer--;
            SpellCurse--;
            if (MarkedTimer > 0)
            {
                for (var i = 0; i < 3; i++)
                {
                    for (var d = 0; d < 4; d++)
                    {
                        Vector2 vel = Vector2.One.RotatedBy(MathHelper.ToRadians(90 * d));
                        Dust dst = Dust.NewDustDirect(npc.Center + vel * (i * 4), 0, 0, 27);
                        dst.noGravity = true;
                        dst.scale = 1.5f - (i * 0.35f);
                        dst.velocity = vel * 8;
                    }
                }
            }
            
            if (npc.HasBuff(BuffID.Electrified))
            {
                Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height/2, 226);
                d.velocity = new Vector2(0, -Main.rand.NextFloat(3, 4)).RotatedByRandom(MathHelper.ToRadians(32));
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1, 1.2f);
                d.noLight = true;
            }

            return base.PreAI(npc);
        }

        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.GoblinSummoner)
            {
                if (!excelWorld.downedGoblinSummoner)
                {
                    excelWorld.downedGoblinSummoner = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                    }
                }
                NPC.SetEventFlagCleared(ref excelWorld.downedGoblinSummoner, -1);
            }
        }


        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.AngryNimbus:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.MageGun.StormCaller>(), 17));
                    break;

                case NPCID.GoblinSummoner:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Banner.ShadowflameBanner>(), 3));
                    break;

                case NPCID.GraniteGolem:
                case NPCID.GraniteFlyer:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.EnergizedGranite>(), 5, 2, 3));
                    break;

                case NPCID.GoblinShark:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Blood.ScarletScythe>(), 4));
                    break;

                case NPCID.SandShark:
                case NPCID.SandsharkCorrupt:
                case NPCID.SandsharkCrimson:
                case NPCID.SandsharkHallow:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Materials.AncientFossil>(), 6, 4));
                    break;

                case NPCID.SnowBalla:
                case NPCID.SnowmanGangsta:
                case NPCID.MisterStabby:
                    npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Items.Food.Carrot>(), 75));
                    if (npc.type != NPCID.MisterStabby)
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.MafiosoHat>(), 50));
                    if (npc.type == NPCID.SnowmanGangsta)
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Guns1.TommyGun>(), 40));
                    break;

                case NPCID.Drippler:
                    npcLoot.Add(ItemDropRule.OneFromOptionsWithNumerator(90, 3, ModContent.ItemType<Items.Weapons.Blood.DripplingStick>()));
                    break;

                case NPCID.WyvernHead:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.WyvernScale>(), 1, 2, 4));
                    break;

                case NPCID.Nurse:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.WeaponHeal.Generic.Syringe>(), 1, 40, 60));
                    break;

                case NPCID.BloodZombie:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Blood.BloodyZombieHand>(), 50));
                    break;

                case NPCID.IceMimic:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Flamethrower.Hypothermia>(), 4));
                    break;

                case NPCID.FireImp:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Flamethrower.BoltTorch>(), 50));
                    break;

                case NPCID.LunarTowerNebula:
                case NPCID.LunarTowerSolar:
                case NPCID.LunarTowerStardust:
                case NPCID.LunarTowerVortex:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BlackholeFragment>(), 1, 5, 15));
                    break;

                case NPCID.MourningWood:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.PumpkinMoon.WitchsCauldron>(), 12));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Flamethrower.Trailblazer>(), 12));
                    break;

                case NPCID.SwampThing:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Eclipse.Harbringer>(), 60));
                    break;



                case NPCID.QueenBee:
                    LeadingConditionRule queenBeeRule = new LeadingConditionRule(new Conditions.NotExpert());
                    queenBeeRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Cleric.Healing.NectarBottle>(), 3));
                    npcLoot.Add(queenBeeRule);
                    break;

                case NPCID.WallofFlesh:
                    LeadingConditionRule wallOfFleshRule = new LeadingConditionRule(new Conditions.NotExpert());
                    wallOfFleshRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.WeaponHeal.Holyiest.PhoenixScepter>(), 3));
                    wallOfFleshRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Cleric.Damage.ClericEmblem>(), 3));
                    npcLoot.Add(wallOfFleshRule);
                    break;

                case NPCID.Golem:
                    LeadingConditionRule golemRule = new LeadingConditionRule(new Conditions.NotExpert());
                    golemRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Flamethrower.SolarEngine>(), 7));
                    npcLoot.Add(golemRule);
                    break;

                case NPCID.BloodNautilus:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Blood.Hemorrhage>(), 3));
                    break;
            }        
        }

        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {    
            if (npc.life <= 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (npc.active && npc.lifeMax > 5 && !npc.friendly && npc.type != NPCID.TargetDummy)
                    {
                        if (npc.boss)
                            for (var i = 0; i < 5; i++)
                            {
                                Item.NewItem(npc.GetSource_FromThis(), npc.getRect(), ModContent.ItemType<Items.Misc.MinorBlessing>());
                            }
                        else if (Main.rand.NextBool(7))
                            Item.NewItem(npc.GetSource_FromThis(), npc.getRect(), ModContent.ItemType<Items.Misc.MinorBlessing>());
                    }
                }

                switch (npc.type)
                {
                    case NPCID.MisterStabby:
                        if (Main.rand.NextBool(6) || (Main.expertMode && Main.rand.NextBool(3)))
                        {
                            SplitSlime(npc, ModContent.NPCType<NPCs.Snow.StabbyJr>(), 1, 0.4f);
                        }
                        break;

                    case NPCID.EyeofCthulhu:
                        if (Main.moonPhase == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Item.NewItem(npc.GetSource_FromThis(), npc.getRect(), ModContent.ItemType<Items.Weapons.Midnight.LunarReflection>());
                        }
                        break;

                    #region Slimes splitting in Master Mode
                    case NPCID.BlueSlime:
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            return;
                        if (!Main.masterMode)
                            return;

                        if (npc.ai[1] == 166)
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.velocity / 4, ProjectileID.Bomb, 100, 4);

                        // Green Slime 
                        switch (npc.netID) {
                            case NPCID.GreenSlime:
                                SplitSlime(npc, NPCID.YellowSlime, 0.66f);
                                SplitSlime(npc, NPCID.BlueSlime, 0.66f);
                                break;
                            case NPCID.PurpleSlime:
                                SplitSlime(npc, NPCID.RedSlime);
                                SplitSlime(npc, NPCID.BlueSlime);
                                break;
                        }
                        break;
                    case NPCID.RainbowSlime:
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            return;
                        if (!Main.masterMode)
                            return;
                        SplitSlime(npc, NPCID.Crimslime, 1, 2);
                        SplitSlime(npc, NPCID.LavaSlime, 1, 2);
                        SplitSlime(npc, NPCID.YellowSlime, 1, 2);
                        SplitSlime(npc, NPCID.GreenSlime, 2.5f, 2);
                        SplitSlime(npc, NPCID.CorruptSlime, 1, 2);
                        SplitSlime(npc, NPCID.PurpleSlime, 1, 2);
                        SplitSlime(npc, NPCID.IlluminantSlime, 1, 2);
                        break;
                    case NPCID.ToxicSludge:
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            return;
                        if (!Main.masterMode)
                            return;
                        for (var i = 0; i < 3; i++)
                            SplitSlime(npc, NPCID.GreenSlime);  
                        break;
                    case NPCID.KingSlime:
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            return;
                        if (!Main.masterMode)
                            return;
                        for (var i = 0; i < 8; i++)
                            SplitSlime(npc, NPCID.BlueSlime, 1, 3f);
                        for (var i = 0; i < 4; i++)
                            SplitSlime(npc, NPCID.SlimeSpiked, 1, 3f);
                        break;
                    case NPCID.QueenSlimeBoss:
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            return;
                        if (!Main.masterMode)
                            return;
                        for (var i = 0; i < 4; i++)
                        {
                            SplitSlime(npc, NPCID.QueenSlimeMinionBlue, 1, 2f);
                            SplitSlime(npc, NPCID.QueenSlimeMinionPink, 1, 2f);
                            SplitSlime(npc, NPCID.QueenSlimeMinionPurple, 1, 2f);
                        }
                        break;
                    case NPCID.SlimeSpiked:
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            return;
                        if (!Main.masterMode)
                            return;
                        SplitSlime(npc, NPCID.BlueSlime, 0.8f, 0.2f);
                        break;
                    case NPCID.SpikedJungleSlime:
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            return;
                        if (!Main.masterMode)
                            return;
                        SplitSlime(npc, NPCID.JungleSlime, 0.8f, 0.2f);
                        break;
                    case NPCID.SpikedIceSlime:
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            return;
                        if (!Main.masterMode)
                            return;
                        SplitSlime(npc, NPCID.IceSlime, 0.8f, 0.2f);
                        break;
                        #endregion
                }
            }
            

        }

        private void SplitSlime(NPC npc, int type, float healMult = 1f, float speedMult = 1f)
        {
            int index = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)npc.position.X, (int)npc.position.Y, type, Target: npc.target);

            // option to make spawned slimes weaker, used for green slime
            if (healMult != 0 && !Main.hardMode)
            {
                Main.npc[index].lifeMax = (int)(Main.npc[index].lifeMax * healMult);
                Main.npc[index].life = Main.npc[index].lifeMax+1;
            }

            Main.npc[index].value *= 0.33f;
            Main.npc[index].velocity = (npc.velocity / 3) + (new Vector2(Main.rand.NextFloat(-3, 3), -4).RotatedByRandom(MathHelper.ToRadians(20)) * speedMult);
            Main.npc[index].GetImmuneTime(Main.LocalPlayer.whoAmI, 20);
            if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: index);
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (DebuffMycosis)
            {
                int dot = NPC.downedPlantBoss ? 28 : Main.hardMode ? 16 : 8;
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= dot;
                if (damage < (dot / 4))
                {
                    damage = (dot / 4);
                }
                //npc.color = new Color(1.13f, 1.46f, 2.45f);
            }
            if (DebuffWound)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 8;
            }

            if (npc.HasBuff(BuffID.Electrified))
            {
                int dot = (MathF.Abs(npc.velocity.X) > 1) ? 24 : 10;

                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= dot;
                if (damage < 1)
                {
                    damage = 1;
                }
            }
            if (npc.HasBuff(BuffID.Bleeding))
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 11;
                if (damage < 3)
                {
                    damage = 3;
                }
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (DebuffWound)
            {
                int dmg = (int)(damage * 0.8f);
                npc.life -= dmg;
                CombatText.NewText(npc.getRect(), Color.DarkRed, dmg);
            }
        }

        public override void OnHitNPC(NPC npc, NPC target, int damage, float knockback, bool crit)
        {
            if (DebuffWound)
            {
                int dmg = (int)(damage * 0.8f);
                npc.life -= dmg;
                CombatText.NewText(npc.getRect(), Color.DarkRed, dmg);
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            if (npc.type == NPCID.SporeBat || npc.type == NPCID.SporeSkeleton || npc.type == NPCID.FungiSpore)
            {
                // 1/3 chance to inflict mycosis for 6-9 seconds
                if (Main.rand.NextBool(3))
                {
                    target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), (6 + Main.rand.Next(4)) * 60);
                }
            }
            if (npc.type == NPCID.FungiBulb || npc.type == NPCID.MushiLadybug|| npc.type == NPCID.AnomuraFungus)
            {
                // guaranteed chance to inflict mycosis for 7-11 seconds
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), (7 + Main.rand.Next(5)) * 60);
            }
            if (npc.type == NPCID.FungoFish || npc.type == NPCID.GiantFungiBulb)
            {
                // guaranteed chance to inflict mycosis for 14-20 seconds
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), (14 + Main.rand.Next(7)) * 60);
            }
            if (target.ZoneGlowshroom && Main.rand.NextBool(13))
            {
                int scale = 1;
                if (Main.hardMode) { 
                    scale++; 
                    if (NPC.downedPlantBoss) { scale++; }
                }
                int dot = (int)(6 + (npc.damage / (5 - scale)) * (60 * (0.8f + (scale * 0.1f))));
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), dot);
            }
          
        }
        
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        { 
            if (NPC.downedPlantBoss && type == NPCID.WitchDoctor)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Accessories.Banner.VenomBanner>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 30);
                nextSlot++;
            }
            if (type == NPCID.Demolitionist)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Furniture.Random.OilJit>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 15);
                nextSlot++;
                if (NPC.downedBoss1)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Ammo.Other.LampOil>());
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 4, 75);
                    nextSlot++;
                }
                if (NPC.downedMechBoss3 && NPC.downedMechBoss2 && NPC.downedMechBoss1 && !Main.dayTime) 
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Ammo.Other.LighterFluid>());
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 14);
                    nextSlot++;
                }
            }
            if (Main.hardMode)
            {
                /*if (NPC.downedGolemBoss && type == NPCID.ArmsDealer)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Flamethrower.FlameBubbler>());
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 75);
                    nextSlot++;
                }*/
                if (type == NPCID.GoblinTinkerer && excelWorld.downedGoblinSummoner && !Main.dayTime)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Ammo.Other.ShadowfireFluid>());
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 12, 50);
                    nextSlot++;    
                }
            }
        }

        
        public override void GetChat(NPC npc, ref string chat)
        {
            //if (NPC.downedGolemBoss && Main.hardMode && npc.type == NPCID.Steampunker && Main.rand.NextBool(8))
            var merchant = NPC.FindFirstNPC(NPCID.Merchant);
            if (npc.type == NPCID.Demolitionist && merchant >= 1 && NPC.downedBoss1 && Main.rand.NextBool(6))
                chat = $"Ya think {Main.npc[merchant].GivenName} might need some oil for that fancy helmet he has?";
            if (npc.type == NPCID.Merchant && Main.rand.NextBool(8))
                chat = "Lamp oil? Ropes? Bombs? You want it? Well too bad, I only sell rope.";
        }
    }
}
