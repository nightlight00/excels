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



namespace excels.Tiles.Trophie
{
    #region Dev Trophy
	internal class NightLightLampy : ModTile
	{ 

		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileWaterDeath[Type] = true;
			Main.tileLavaDeath[Type] = true;
			// Main.tileFlame[Type] = true; // This breaks it.

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
			TileObjectData.addTile(Type);

			// Etc
			AddMapEntry(new Color(51, 16, 180));
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 64, ModContent.ItemType<Items.Furniture.Random.NightLightLamp>());
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int topY = j - tile.TileFrameY / 18 % 3;
			short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);

			Main.tile[i, topY].TileFrameX += frameAdjustment;
			Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
			Main.tile[i, topY + 2].TileFrameX += frameAdjustment;

			Wiring.SkipWire(i, topY);
			Wiring.SkipWire(i, topY + 1);
			Wiring.SkipWire(i, topY + 2);

			// Avoid trying to send packets in singleplayer.
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
			}
		}

		Color Max = new Color(.085f * 0.05f, .049f * 0.05f, .255f * 0.05f);
		Color Min = new Color(.237f * 0.05f, .212f * 0.05f, 0f * 0.05f);
		float timer = 0;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 0)
			{
				/*
				r = Main.DiscoColor.R;
				b = Main.DiscoColor.B;
				g = Main.DiscoColor.G;
				*/
				timer += 0.015f;
				// We can support different light colors for different styles here: switch (tile.frameY / 54)
				r = MathHelper.Lerp(Max.R, Min.R, timer);
				g = MathHelper.Lerp(Max.G, Min.G, timer);
				b = MathHelper.Lerp(Max.B, Min.B, timer);

				if (timer > 1)
                {
					Color temp = Max;
					Max = Min;
					Min = temp;
					timer = 0;
                }
				
			}
		}
	}
    #endregion

    internal class StellarTrophyTile : ModTile
    {
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
			DustType = 7;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Furniture.Trophy.StellarTrophy>());
		}
	}
}
