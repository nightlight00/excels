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

namespace excels.NPCs.Chasm.Quake
{
    /*
    internal class QuakeBossMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quake");
        }

        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.npcSlots = 10;

            NPC.lifeMax = 9999;
            NPC.defense = 20;
            NPC.damage = 50;
            NPC.knockBackResist = 0;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 80;
            NPC.height = 80;
            NPC.coldDamage = true;
            NPC.value = 60000;

            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath6;

            NPC.immortal = true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile) => false;

        public override bool? CanBeHitByItem(Player player, Item item) => false;

        public override void AI()
        {
            NPC.TargetClosest(true);

            NPC.ai[0]++;
            if (NPC.ai[0] > 40)
            {
                NPC.Center = Main.player[NPC.target].Center + (Main.rand.NextVector2Unit() * 200);
                NPC.ai[0] = 0;
            }

            NPC.spriteDirection = 0;
            NPC.rotation = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.Zero).ToRotation();
            if (NPC.rotation < 90 || NPC.rotation > 270)
            {
                NPC.spriteDirection = -1;
            }
        }
    }
    */
}
