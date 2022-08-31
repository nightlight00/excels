using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;

namespace excels.NPCs.Glacial
{
    public class GlacialBeholder : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Gazer");
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 10; // cursed skull ai
            AIType = NPCID.CursedSkull; // so faster movement

            NPC.height = NPC.width = 30;
            NPC.damage = 18;
            NPC.lifeMax = 68;
            NPC.defense = 8;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath3;
            NPC.value = 20;
            NPC.knockBackResist = 0.3f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.stepSpeed = 3;
            AnimationType = NPCID.FlyingFish;
            NPC.coldDamage = true;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 76);
                d.velocity *= 0.4f;
                d.scale = 1f;
                d.noGravity = true;
                d.alpha = 70;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,

				// not using this lore since no longer called a beholder
 				//new FlavorTextBestiaryInfoElement("Accross the lands beholders are seen as a menace to all that live, but this one seems to be more of an annoyance than danger")
                new FlavorTextBestiaryInfoElement("A distant cousin to beholders, these little eyes don't serve Cuthulu but Niflheim herself")
            }); 
        }


        public override void HitEffect(int hitDirection, double damage)
        {
            for (var i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 76);
                d.scale = 1.2f;
                d.velocity *= 0.6f;
                d.alpha = 50;
                d.noGravity = true;
            }
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Vector2.One, GoreID.Smoke1);
            }
        }
    }

    public class GlacialAngel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Angel");
        }

        public override void SetDefaults()
        {
            //NPC.aiStyle = 56; // dungeon spirit ai
            // current ai is exactly the same but without the dusts made from ds ai

            NPC.height = NPC.width = 30;
            NPC.damage = 40;
            NPC.lifeMax = 56;
            NPC.defense = 10;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath3;
            NPC.value = 20;
            NPC.knockBackResist = 0.6f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.coldDamage = true;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 76);
                d.velocity *= 0.4f;
                d.scale = 1f;
                d.noGravity = true;
                d.alpha = 70;
            }
            //NPC.velocity *= 1.5f;
            NPC.TargetClosest(true);
            Vector2 vector102 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num859 = Main.player[NPC.target].Center.X - vector102.X;
            float num860 = Main.player[NPC.target].Center.Y - vector102.Y;
            float num861 = (float)Math.Sqrt((double)(num859 * num859 + num860 * num860));
            float num862 = 12f;
            num861 = num862 / num861;
            num859 *= num861;
            num860 *= num861;
            NPC.velocity.X = (NPC.velocity.X * 100f + num859) / 96f;
            NPC.velocity.Y = (NPC.velocity.Y * 100f + num860) / 101f;
            NPC.rotation += MathHelper.ToRadians(10);
            return;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,

                new FlavorTextBestiaryInfoElement("Within this sentient snowflake lies the wish to just make a snow angel")
            });
        }


        public override void HitEffect(int hitDirection, double damage)
        {
            for (var i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 76);
                d.scale = 1.2f;
                d.velocity *= 0.6f;
                d.alpha = 50;
                d.noGravity = true;
            }
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Vector2.One, GoreID.Smoke1);
            }
        }
    }
}
