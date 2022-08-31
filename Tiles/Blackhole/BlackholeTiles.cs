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

namespace excels.Tiles.Blackhole
{
    internal class BlackholeBrickTile : ModTile
    {
		public override void SetStaticDefaults()
		{ 
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(255, 169, 29));

			DustType = ModContent.DustType<Dusts.GlacialDust>();
			ItemDrop = ModContent.ItemType<Items.TileItems.Blackhole.BlackholeBrick>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
			// mineResist = 4f;
			// minPick = 200;
		}
	}

	internal class BlackholePlatformTile : ModTile
    {
		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.Platforms[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			AddMapEntry(new Color(200, 200, 200));

			ItemDrop = ModContent.ItemType<Items.TileItems.Blackhole.BlackholePlatform>();
			AdjTiles = new int[] { TileID.Platforms };

			// Placement
			TileObjectData.newTile.CoordinateHeights = new[] { 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleMultiplier = 27;
			TileObjectData.newTile.StyleWrapLimit = 27;
			TileObjectData.newTile.UsesCustomCanPlace = false;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(Type);
		}
	}
}
