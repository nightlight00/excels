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
using excels.Items.Placeable.Stations;

namespace excels.Tiles.Stations
{

	public class StarlightAnvilTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;

			AdjTiles = new int[] { TileID.Anvils };

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.addTile(Type);

			//ItemDrop = ModContent.ItemType<Items.Furniture.Anvils.StarlightAnvil>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Starlight Anvil");
			AddMapEntry(new Color(85, 53, 163), name);

		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<StarlightAnvil>(), 1);
		}
	}

	public class OilKit : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.addTile(Type);

			ItemDrop = ModContent.ItemType<OilJit>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Oil Kit");
			AddMapEntry(new Color(83, 75, 91), name);

		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<OilJit>(), 1);
		}
	}
}
