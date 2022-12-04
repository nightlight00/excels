
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace excels 
{
    internal class excelConfig : ModConfig
	{   // ConfigScope.ClientSide should be used for client side, usually visual or audio tweaks.
		// ConfigScope.ServerSide should be used for basically everything else, including disabling items or changing NPC behaviours
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Label("$Mods.excels.Config.ScreenShake.Label")]
		[Tooltip("$Mods.excels.Config.ScreenShake.Tip")]
		[DefaultValue(1f)]
		public float ScreenShakeIntensity;

		// Cleric Advanced Tooltips
		[Header("$Mods.excels.Config.ClericTooltip.Header")] 
		
		[Label("$Mods.excels.Config.ClericTooltip.Label")] 
		[Tooltip("$Mods.excels.Config.ClericTooltip.Tip")]
		[DefaultValue(true)]
		public bool ClericAdvancedTooltip; 

		[SeparatePage]
		[Label("$Mods.excels.Config.ClericTooltip.HealLabel")]
		[Tooltip("$Mods.excels.Config.ClericTooltip.HealTip")] 
		[DefaultValue(true)] 
		public bool ClericHealTooltip;
		
	}
}
