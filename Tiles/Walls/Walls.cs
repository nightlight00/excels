﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;
using excels.Items.Placeable.Walls;

namespace excels.Tiles.Walls
{
	internal class SkylineBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Marble;
			ItemDrop = ModContent.ItemType<SkylineBrickWallItem>();

			AddMapEntry(new Color(197, 246, 245));
		}
	}

	internal class DarkslateWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Granite;

			AddMapEntry(new Color(19, 11, 26));
		}

		public override bool CanExplode(int i, int j) => false;
    }


	internal class CheckerWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Marble;
			ItemDrop = ModContent.ItemType<CheckerWallItem>();

			AddMapEntry(new Color(143, 158, 168));
		}
	}

	internal class HyperionBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = ModContent.DustType<Dusts.HyperionMetalDust>();
			ItemDrop = ModContent.ItemType<ReinforcedDarkslateWallItem>();

			AddMapEntry(new Color(44, 46, 82));
		}
	}

	internal class StellarWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = ModContent.DustType<Dusts.StellarDust>();
			ItemDrop = ModContent.ItemType<StellarWallItem>();

			AddMapEntry(new Color(55, 34, 138));
		}
	}

	internal class StellarWallAdorned : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = ModContent.DustType<Dusts.StellarDust>();
			ItemDrop = ModContent.ItemType<StellarAdornedWallItem>();

			AddMapEntry(new Color(55, 34, 138));
		}
	}
}
