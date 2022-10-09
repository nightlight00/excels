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

namespace excels.NPCs.Joke
{
    /*
    internal class NecoArc : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("little shit");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 9999999;
            NPC.defense = 9999999;
            NPC.knockBackResist = 0;
            NPC.immortal = true;
            NPC.damage = 99999999;
            NPC.reflectsProjectiles = true;
        }

        private void SpawnShit()
        {
            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + Main.rand.Next(-300, 300), (int)NPC.position.Y + Main.rand.Next(-300, 300), ModContent.NPCType<NecoArc>());
            SoundEngine.PlaySound(new SoundStyle("excels/Audio/BuruNyuu"));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            NPC.position += new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-300, 300));
            SoundEngine.PlaySound(new SoundStyle("excels/Audio/BuruNyuu"));
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            SpawnShit();
        }

        public override bool CheckDead()
        {
            SpawnShit();
            SpawnShit();
            return base.CheckDead();
        }

        public override void AI()
        {
            NPC.TargetClosest();
            NPC.velocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.Zero) * 3;
        }
    }
    */
}
