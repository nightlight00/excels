using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Enums;
using excels.Items.Placeable.Decorations.Trophies;


namespace excels.Tiles.Decorations.Trophies
{
	#region Boss Trophy Tile
	internal class BossTrophies : ModTile
	{
		public const int FrameWidth = 18 * 3;
		public const int FrameHeight = 18 * 3;
		public const int HorizontalFrames = 3;

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = false;

			TileObjectData.newTile.StyleWrapLimitVisualOverride = 3;
			TileObjectData.newTile.StyleMultiplier = 3;
			TileObjectData.newTile.StyleWrapLimit = 3;
			TileObjectData.newTile.styleLineSkipVisualOverride = 0;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
			DustType = 7;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			switch (frameX / FrameWidth) {
				case 0:
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<StellarTrophy>());
					break;
				case 1:
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<ChasmTrophy>());
					break;
				case 2:
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<NiflheimTrophy>());
					break;
			}
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			// Only required If you decide to make your tile utilize different styles through Item.placeStyle

			// This preserves its original frameX/Y which is required for determining the correct texture floating on the pedestal, but makes it draw properly
			tileFrameX %= FrameWidth * 3; // Clamps the frameX (two horizontally aligned place styles, hence * 2)
			tileFrameY %= FrameHeight; // Clamps the frameY 
		}
	}
	#endregion
}
