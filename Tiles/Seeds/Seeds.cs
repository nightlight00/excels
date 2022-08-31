using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace excels.Tiles.Seeds
{
	public enum PlantStage : byte
	{
		Planted,
		Growing1,
		Growing2,
		Growing3,
		Grown
	}


	internal class SugarBeetTile : ModTile
    {
		private const int FrameWidth = 18; // A constant for readability and to kick out those magic numbers

		public override void SetStaticDefaults()
		{
			Main.tileObsidianKill[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.ReplaceTileBreakUp[Type] = true;
			TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]); // Make this tile interact with golf balls in the same way other plants do

			// We do not use this because our tile should only be spelunkable when it's fully grown. That's why we use the IsTileSpelunkable hook instead
			//Main.tileSpelunker[Type] = true;

			// Do NOT use this, it causes many unintended side effects
			//Main.tileAlch[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sugar Beet");
			AddMapEntry(new Color(56, 160, 59), name);

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.AnchorValidTiles = new int[] {
				TileID.Dirt
			};
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.Width = 2;
			TileObjectData.addTile(Type);

			AnimationFrameHeight = 40;

			HitSound = SoundID.Grass;
			DustType = DustID.Grass;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Seeds.SugarBeetSeed>());
		}
		

		private readonly int animationFrameWidth = 18*2;

		public override bool CanPlace(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j); // Safe way of getting a tile instance

			if (tile.HasTile)
			{
				int tileType = tile.TileType;
				if (tileType == Type)
				{
					PlantStage stage = GetStage(i, j); // The current stage of the herb

					// Can only place on the same herb again if it's grown already
					return stage == PlantStage.Grown;
				}
				else
				{
					// Support for vanilla herbs/grasses:
					if (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType] || tileType == TileID.WaterDrip || tileType == TileID.LavaDrip || tileType == TileID.HoneyDrip || tileType == TileID.SandDrip)
					{
						bool foliageGrass = tileType == TileID.Plants || tileType == TileID.Plants2;
						bool moddedFoliage = tileType >= TileID.Count && (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType]);
						bool harvestableVanillaHerb = Main.tileAlch[tileType] && WorldGen.IsHarvestableHerbWithSeed(tileType, tile.TileFrameX / 18);

						if (foliageGrass || moddedFoliage || harvestableVanillaHerb)
						{
							WorldGen.KillTile(i, j);
							if (!tile.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
							{
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
							}

							return true;
						}
					}

					return false;
				}
			}

			return true;
		}

		public override bool IsTileSpelunkable(int i, int j)
		{
			PlantStage stage = GetStage(i, j);

			// Only glow if the herb is grown
			return stage == PlantStage.Grown;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 0; // This is -1 for tiles using StyleAlch, but vanilla sets to -2 for herbs, which causes a slight visual offset between the placement preview and the placed tile. 
		}

		Vector2 til = Vector2.Zero;

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
			if (!Main.tile[(int)til.X, (int)til.Y].HasTile)
				return;

			if (frame != 5)
            {
				frameCounter++;
            }

			if (frameCounter >= 120)
			{
				frameCounter = 0 - Main.rand.Next(20);
				if (++frame >= 5)
				{
					frame = 4;
				}
			}
		}
		
		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			frameYOffset = Main.tileFrame[Type] * AnimationFrameHeight;
			til = new Vector2(i, j);
		}
		
		// A helper method to quickly get the current stage of the herb (assuming the tile at the coordinates is our herb)
		private static PlantStage GetStage(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			return (PlantStage)(tile.TileFrameX / 40);
		}
	}
}
