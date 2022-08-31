using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace excels.Buffs.Potions
{
    internal class SweetBuff : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assuaging Heart");
			Description.SetDefault("Increases healing potency by 1");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			// Use a ModPlayer to keep track of the buff being active
			player.GetModPlayer<excelPlayer>().healBonus++;
		}
	}
}
