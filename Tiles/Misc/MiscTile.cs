using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace excels.Tiles.Misc
{
    #region Skyline Brick
    internal class SkylineBrickTile : ModTile
    {
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(197, 246, 245));

			DustType = ModContent.DustType<Dusts.SkylineDust>();
			ItemDrop = ModContent.ItemType<Items.TileItems.SkylineBrick>();
			HitSound = SoundID.Tink;
			//sound = 1;
		}
	}

	internal class SkylineBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Marble;
			ItemDrop = ModContent.ItemType<Items.TileItems.SkylineBrickWallItem>();

			AddMapEntry(new Color(197, 246, 245));
		}
	}
	#endregion

	#region Glacial Brick
	internal class GlacialBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(48, 184, 246));

			DustType = ModContent.DustType<Dusts.GlacialDust>();
			ItemDrop = ModContent.ItemType<Items.TileItems.GlacialBrick>();
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

    #region Checker
    internal class CheckerTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(223, 241, 252));

			DustType = DustID.Marble;
			ItemDrop = ModContent.ItemType<Items.TileItems.CheckerItem>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;
		}
	}

	internal class CheckerWall : ModWall
    {
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Marble;
			ItemDrop = ModContent.ItemType<Items.TileItems.CheckerWallItem>();

			AddMapEntry(new Color(143, 158, 168));
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

			AddMapEntry(new Color(124, 255, 234));

			DustType = DustID.Marble;
			ItemDrop = ModContent.ItemType<Items.TileItems.HyperionLampBlock>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;

			if (!Main.dedServ)
			{
				flameTexture = ModContent.Request<Texture2D>("excels/Tiles/Misc/HyperionLampTile_Flame"); // We could also reuse Main.FlameTexture[] textures, but using our own texture is nice.
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

	#region Skyline Brick
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
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<Items.Furniture.Anvils.StarlightAnvil>(), 1);
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

			//ItemDrop = ModContent.ItemType<Items.Furniture.Anvils.StarlightAnvil>();
			HitSound = SoundID.Tink;
			//SoundStyle = 1;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Oil Kit");
			AddMapEntry(new Color(83, 75, 91), name);

		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Furniture.Random.OilJit>(), 1);
		}
    }

	public class CorruptStarlightFuserTile : ModTile
    {
		public override void SetStaticDefaults()
		{
			// If a tile is a light source
			Main.tileLighted[Type] = true;
			// This changes a Framed tile to a FrameImportant tile
			// For modders, just remember to set this to true when you make a tile that uses a TileObjectData
			// Or basically all tiles that aren't like dirt, ores, or other basic building tiles
			Main.tileFrameImportant[Type] = true;
			// Set to True if you'd like your tile to die if hit by lava
			Main.tileLavaDeath[Type] = true;
			// Use this to utilize an existing template
			// The names of styles are self explanatory usually (you can see all existing templates at the link mentioned earlier)
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
			// This last call adds a new tile
			// Before that, you can make some changes to newTile like height, origin and etc.
			TileObjectData.addTile(Type);

			// AddMapEntry is for setting the color and optional text associated with the Tile when viewed on the map
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("CorruptStarlightFuser");
			AddMapEntry(new Color(238, 145, 105), name);

			
			// Can't use this since texture is vertical
			AnimationFrameHeight = 36;
		}


		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frame = Main.tileFrame[TileID.ImbuingStation];
		}
	}
}
