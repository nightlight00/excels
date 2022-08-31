using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Buffs.ClericCld
{
    internal class BlessingCooldown : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prayer Exhaut");
			Description.SetDefault("Holy weapon prayers need to replenish their energy");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = false;
			Main.debuff[Type] = true; // Add this so the nurse doesn't remove the buff when healing
		}
	}

	internal class RevivalRecover : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Revival Recover");
			Description.SetDefault("Recovering from revival");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true; // Add this so the nurse doesn't remove the buff when healing
		}

		Player owner;
		int index;
		float damage = 0;

        public override void Update(Player player, ref int buffIndex)
        {
			damage = ((player.buffTime[buffIndex] - (10800 * 0.66f)) / (10800 / 3));
			if (damage < 0)
				damage = 0;

			player.GetDamage(DamageClass.Generic) -= 0.2f * damage;
			owner = player;
			index = buffIndex;
		}

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
			if (damage > 0)
				tip = $"Recovering from revival\nAll damage decreased by {Math.Round(20 * damage)}%";
			else
				tip = $"Recovering from revival";
		}
    }

	internal class AnguishedSoul : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anguished Soul");
			Description.SetDefault("Cost of performing the dark arts \nAll positive life regeneration halved");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = false;
			Main.debuff[Type] = true; // Add this so the nurse doesn't remove the buff when healing
		}

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
			rare = ItemRarityID.Red;
        }

        public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<excelPlayer>().AnguishSoul = true;
		}
	}
}
