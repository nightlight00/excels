using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using excels.Tiles.Decorations.Misc;

namespace excels.Items.Placeable.Decorations.Misc
{
	#region The Funny
	internal class NightLightLamp : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightlight");
			Tooltip.SetDefault("'This is what peak performance looks like'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = 9;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<NightLightLampy>();
			Item.width = 16;
			Item.height = 16;
		}
	}
	#endregion
}
