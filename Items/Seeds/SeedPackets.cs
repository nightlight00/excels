using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace excels.Items.Seeds
{
    internal class SugarBeetSeed : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sugar Beet Seed Packet");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Seeds.SugarBeetTile>();
			Item.width = 16;
			Item.height = 16;
		}
	}
}
