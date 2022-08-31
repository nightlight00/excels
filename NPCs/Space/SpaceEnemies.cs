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

namespace excels.NPCs.Space
{
    internal class SkylineHarpy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skyline Sentinal");
            Main.npcFrameCount[NPC.type] = 6;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Position = new Vector2(0f, 10f),
                PortraitPositionYOverride = -10
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Harpy);
            AIType = NPCID.Harpy;
            AnimationType = NPCID.Harpy;

            NPC.damage = 25;
            NPC.defense = 13;
            NPC.lifeMax = 150;
            NPC.knockBackResist = 0.7f;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerSkylineSentinal>();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SkylineOre>(), 3, 2, 4));
            npcLoot.Add(ItemDropRule.Common(ItemID.Feather, 3));
            npcLoot.Add(ItemDropRule.Food(ItemID.ChickenNugget, 75));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int num824 = 0; num824 < 50; num824++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, (float)(2 * hitDirection), -2f, 0, default(Color), 1f);
            }
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.NextFloat(-3, 3), 0), 80);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.NextFloat(-3, 3), 0), 81);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

				// not using this lore since no longer called a beholder
 				//new FlavorTextBestiaryInfoElement("Accross the lands beholders are seen as a menace to all that live, but this one seems to be more of an annoyance than danger")
                new FlavorTextBestiaryInfoElement("An elite troop of harpies who wear a special armor that concentrates their energy into beams")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Sky)
            {
                return 0.3f;
            }
            return 0;
        }

        private int attackTimer = 0;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackTimer = reader.ReadInt32();
        }

        public override void AI()
        {
            NPC.ai[0] = 1;

            attackTimer++;
            if (attackTimer == 40 || attackTimer == 80)
            {
                
                Vector2 shootVel = Main.player[NPC.target].Center - NPC.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= 3.1f;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, shootVel, ModContent.ProjectileType<SpaceLaser>(), 18, 2);
            }
            else if (attackTimer == 360)
            {
                attackTimer = 0;
            }
        }
    }

    public class SpaceLaser : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.HarpyFeather}";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 6;
            Projectile.extraUpdates = 7;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 200;
        }

        public override void AI()
        {
            for (var i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15, newColor: new Color(0, 180, 230));
                d.noGravity = true;
                d.velocity *= 0.3f;
                d.alpha = 150;
                d.scale = 1.2f;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15, newColor: new Color(0, 180, 230));
                d.noGravity = true;
                d.velocity *= 1.4f;
                d.alpha = 80;
                d.scale = 1.6f;
            }
        }
    }
}
