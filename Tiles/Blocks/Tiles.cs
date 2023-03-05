using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace excels.Tiles.Blocks
{
    #region Skyline Brick
    internal class SkylineBrickTile : ModTile
    {
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(197, 246, 245));

			DustType = ModContent.DustType<Dusts.SkylineDust>();
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.SkylineBrick>();
			HitSound = SoundID.Tink;
			//sound = 1;
		}
	}
	#endregion

	#region Glacial Brick
	internal class GlacialBrickNewTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(48, 184, 246));

			DustType = ModContent.DustType<Dusts.GlacialDust>();
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.GlacialBrickNew>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
		}

		public override bool HasWalkDust() => true;

		public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
		{
			dustType = DustID.Snow;
		}
	}
	#endregion

	#region Ancient Glacial Brick
	internal class GlacialBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(48, 184, 246));

			DustType = ModContent.DustType<Dusts.GlacialDust>();
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.GlacialBrick>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
		}

		public override bool HasWalkDust() => true;

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
			dustType = DustID.Snow;
        }
    }
	#endregion

	#region Deepslate 
	internal class DarkslateTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][TileID.Stone] = true;
			Main.tileMerge[TileID.Stone][Type] = true;
			Main.tileMerge[Type][ModContent.TileType<OresBars.HyperionTile>()] = true;
			Main.tileMerge[ModContent.TileType<OresBars.HyperionTile>()][Type] = true;

			AddMapEntry(new Color(38, 29, 48));

			DustType = DustID.Granite;
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.DarkslateItem>();
			HitSound = SoundID.Tink;
			MinPick = 120;
			//SoundStyle = 1;
		}

		public override bool CanExplode(int i, int j) => false;
	}
	#endregion

	#region Checker Bricks
	internal class CheckerTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(223, 241, 252));

			DustType = DustID.Marble;
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.CheckerItem>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
		}
	}
	#endregion
	
	#region Sunlight Block
    internal class SunlightBlockPlaced : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(251, 216, 91));

			DustType = 204;
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.SunlightBlockItem>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 2.44f * 0.75f;
			g = 2.55f * 0.75f;
			b = 1.32f * 0.75f;
		}
	}
	#endregion

	#region Hyperion Lamp
	internal class HyperionLampTile : ModTile
	{
		private Asset<Texture2D> flameTexture;

		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<HyperionBrickTile>()] = true;
			Main.tileMerge[ModContent.TileType<HyperionBrickTile>()][Type] = true;

			AddMapEntry(new Color(124, 255, 234));

			DustType = DustID.Marble;
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.HyperionLampBlock>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;

			if (!Main.dedServ)
			{
				flameTexture = ModContent.Request<Texture2D>("excels/Tiles/Blocks/HyperionLampTile_Flame"); // We could also reuse Main.FlameTexture[] textures, but using our own texture is nice.
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 255 / 124;
			g = 255 / 255;
			b = 255 / 234;
		}

		public override bool CreateDust(int i, int j, ref int type)
        {
			type = ModContent.DustType<Dusts.HyperionMetalDust>();
			if (Main.rand.NextBool(3))
				type = ModContent.DustType<Dusts.HyperionEnergyDust>();
            return true;
        }

        public override bool CanExplode(int i, int j) => false;

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (i % 2 == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			Tile tile = Main.tile[i, j];
			int width = 16;
			int offsetY = 0;
			int height = 16;
			short frameX = tile.TileFrameX;
			short frameY = tile.TileFrameY;

			TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);

			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i); // Don't remove any casts.

			// We can support different flames for different styles here: int style = Main.tile[j, i].frameY / 54;
			for (int c = 0; c < 7; c++)
			{
				float shakeX = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
				float shakeY = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

				spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(frameX, frameY, width, height), new Color(100, 100, 100, 0), 0f, default, 1f, effects, 0f);
			}
		}
	}
	#endregion

	#region Reinforced Darkslate
	internal class HyperionBrickTile : ModTile
	{

		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;

			AddMapEntry(new Color(67, 78, 121));

			DustType = ModContent.DustType<Dusts.HyperionMetalDust>();
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.ReinforcedDarkslateItem>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
		}

		public override bool CanExplode(int i, int j) => false;
	}
	#endregion

	#region Mushroom Tile
	internal class MushroomTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;

			AddMapEntry(new Color(190, 90, 25));

			//DustType = ;
			ItemDrop = ItemID.Mushroom;
			//HitSound = SoundID.;
			//sound = 1;
		}
	}
	#endregion

	#region Decorated Purity Bricks
	internal class DecoratedPurityBricksTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;

			AddMapEntry(new Color(221, 227, 233));

			DustType = DustID.Marble;
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.DecoratedPurityBrick>();
			HitSound = SoundID.Tink;
			//sound = 1;
		}
	}
	#endregion

	#region Stellar Bricks
	internal class StellarBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(55, 34, 138));

			DustType = ModContent.DustType<Dusts.StellarDust>();
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.StellarBrick>();
			HitSound = SoundID.Tink;
			//sound = 1;
		}
	}


	#endregion

	#region Blackhole Brick
	internal class BlackholeBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(255, 169, 29));

			//DustType = ModContent.DustType<Dusts.b>();
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.BlackholeBrick>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
			// mineResist = 4f;
			// minPick = 200;
		}
	}
	#endregion
}
