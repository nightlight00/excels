using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;


namespace excels.Buffs.Debuffs
{
    public class Mycosis : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mycosis"); // Buff display name
			Description.SetDefault("An infectious fungus is invading your skin"); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}

        public override void Update(Player player, ref int buffIndex)
        {
			player.GetModPlayer<excelPlayer>().DebuffMycosis = true;
			int type = 176;
			if (Main.rand.NextBool(4)) { type = 113; }
			Dust d = Dust.NewDustDirect(player.position, player.width, player.height, type);
			d.noGravity = true;
			d.velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-3, -4.8f));
			d.scale = Main.rand.NextFloat(0.9f, 1.4f);
		}

        public override void Update(NPC npc, ref int buffIndex)
        {
			npc.GetGlobalNPC<excelNPC>().DebuffMycosis = true;
			int type = 176;
			if (Main.rand.NextBool(4)) { type = 113; }
			Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, type);
			d.noGravity = true;
			d.velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-3, -4.8f));
			d.scale = Main.rand.NextFloat(0.9f, 1.4f);
		}
    }

	public class FragileBones : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prehistoric Curse"); // Buff display name
			Description.SetDefault("You feel like you're being crushed"); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}

        public override void Update(NPC npc, ref int buffIndex)
        {
			npc.defense -= 10;
			for (var i = 0; i < (1 + Main.rand.Next(1)); i++)
			{
				if (Main.rand.NextBool(4))
				{
					Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.FossilBoneDust>());
					d.noGravity = true;
				}
			}
        }

        public override void Update(Player player, ref int buffIndex)
        {
			player.statDefense -= 10;
			for (var i = 0; i < (1 + Main.rand.Next(1)); i++)
			{
				if (Main.rand.NextBool(4))
				{
					Dust d = Dust.NewDustDirect(player.position, player.width, player.height, ModContent.DustType<Dusts.FossilBoneDust>());
				}
			}
		}
    }

	public class DeepWound : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deep Wound"); // Buff display name
			Description.SetDefault("Getting hit spills blood"); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<excelNPC>().DebuffWound = true;
		}
	}
}
