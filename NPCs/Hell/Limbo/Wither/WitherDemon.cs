using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace excels.NPCs.Hell.Limbo.Wither
{
    internal class WitheringDemon : ModNPC
    {
        public override string Texture => $"Terraria/Images/NPC_{NPCID.Demon}";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Demon);
            AIType = NPCID.CaveBat;
            AnimationType = NPCID.Demon;

            NPC.damage = 60;
            NPC.defense = 10;
            NPC.lifeMax = 63;
            NPC.knockBackResist = 0.3f;
            NPC.value = 40;
            NPC.lavaImmune = true;

            //Banner = NPC.type;
            //BannerItem = ModContent.ItemType<Tiles.Decorations.Banners.BItems.BannerMeteorSlime>();
        }

        public override void AI()
        {
            int witherAmount = 0, witherDemonAmount = 0;
            for (var i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active)
                {
                    if (Main.npc[i].type == ModContent.NPCType<Wither>())
                        witherAmount++;
                    if (Main.npc[i].type == ModContent.NPCType<WitheringDemon>())
                        witherDemonAmount++;
                }
            }

            NPC.ai[0]++;
            if (witherAmount >= witherDemonAmount * 2)
            {
                if (NPC.ai[0] > 320 && NPC.ai[0] % 8 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int target = -1;
                        for (var i = 0; i < Main.maxPlayers; i++)
                        {
                            if (Main.player[i].active)
                            {
                                target = i;
                            }
                        }

                        if (target != -1)
                        {
                            if (Vector2.Distance(NPC.Center, Main.player[target].Center) < 2000)
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, (Main.player[target].Center - NPC.Center).SafeNormalize(Vector2.Zero).RotatedByRandom(MathHelper.ToRadians(20)) * 3.3f, ModContent.ProjectileType<WitherBrimstone>(), 50, 3);
                        }
                    }

                    if (NPC.ai[0] >= 360)
                        NPC.ai[0] = 0;
                }
            }
            else
            {
                if (NPC.ai[0] == 550)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Wither>());
                    }
                    NPC.ai[0] = 0;
                }
            }
        }
    }
}
