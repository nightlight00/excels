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
/*
namespace excels.NPCs.Space.Solar
{
    internal class SolarSpirit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Spirit");
            Main.npcFrameCount[NPC.type] = 2;

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

            NPC.damage = 60;
            NPC.defense = 15;
            NPC.lifeMax = 340;
            NPC.knockBackResist = 0.3f;

            //Banner = NPC.type;
            //BannerItem = ModContent.ItemType<Tiles.Banners.BItems.BannerSkylineSentinal>();
        }

        public override void AI()
        {
            NPC.ai[0]++;
            NPC.TargetClosest(true);

            if (NPC.ai[0] == 300)
            {
                NPC.velocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.Zero) * 18;
                NPC.rotation = NPC.velocity.X * 0.4f;
            }
            else if (NPC.ai[0] < 300)
            {
                NPC.velocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.Zero) * 3;
                NPC.rotation = NPC.velocity.ToRotation();
            }
            else if (NPC.ai[0] > 320)
            {
                NPC.ai[0] = 0;
            }

            NPC.spriteDirection = 1;
            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = -1;
            }

            if (++NPC.frameCounter % 6 == 0)
            {
             //   NPC.frame.Y += 26;
            }
        }
    }
}


*/