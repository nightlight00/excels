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

namespace excels.Tiles.OresBars
{
    public class GlacialOreTile : ModTile
    {
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 350; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
		//	Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Glacial Ore");
			AddMapEntry(new Color(48, 184, 246), name);

			DustType = ModContent.DustType<Dusts.GlacialDust>();
			ItemDrop = ModContent.ItemType<Items.Materials.GlacialOre>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
			// mineResist = 4f;
			// minPick = 200;
		}
	}

	class SkylineOreTile : ModTile
	{

		public override void SetStaticDefaults()
		{

			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 290; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
										//	Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Skyline Pebble");
			AddMapEntry(new Color(197, 246, 245), name);

			DustType = ModContent.DustType<Dusts.SkylineDust>(); 
			ItemDrop = ModContent.ItemType<Items.Materials.SkylineOre>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
			MinPick = 50;
		}
	}

	class PurityOre : ModTile
	{

		public override void SetStaticDefaults()
		{

			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 290; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
										//	Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Purified Stone");
			AddMapEntry(new Color(197, 246, 245), name);

		//	DustType = ModContent.DustType<Dusts.SkylineDust>();
			ItemDrop = ModContent.ItemType<Items.Materials.PurifiedStone>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
			//MinPick = 50;
		}
	}

	public class HyperionTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 670; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 600; // How often tiny dust appear off this tile. Larger is less frequently
										//	Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hyperion Crystal");
			AddMapEntry(new Color(124, 255, 234), name);

			DustType = ModContent.DustType<Dusts.HyperionEnergyDust>();
			ItemDrop = ModContent.ItemType<Items.Materials.HyperionCrystal>();
			HitSound = SoundID.Tink;
			MineResist = 4f;
			MinPick = 170;
		}

        public override bool CanExplode(int i, int j)
        {
			return false;
        }
		
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			r = 255 / 124;
			g = 255 / 255;
			b = 255 / 234;
        }
    }

	public class ExcelBarTiles : ModTile
    {
		public override void SetStaticDefaults()
		{
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.MetalBar")); // localized text for "Metal Bar"

			DustType = 84;
		}

        public override bool CreateDust(int i, int j, ref int type)
        {
			Tile t = Main.tile[i, j];
			int style = t.TileFrameX / 18;
			if (style == 0)
            {
				type = ModContent.DustType<Dusts.SkylineDust>();
			}
			else if (style == 1)
			{
				type = ModContent.DustType<Dusts.GlacialDust>();
			}
			else if (style == 2) {

            }
			return true;
        }

        public override bool Drop(int i, int j)
		{
			Tile t = Main.tile[i, j];
			int style = t.TileFrameX / 18;

			// It can be useful to share a single tile with multiple styles. This code will let you drop the appropriate bar if you had multiple.
			if (style == 0)
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Materials.SkylineBar>());
			}
			else if (style == 1)
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Materials.GlacialBar>());
			}
			else if (style == 2)
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Materials.MysticCrystal>());
			}

			return base.Drop(i, j);
		}
	}
}
