using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.Initializers;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using ReLogic.Graphics;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using excels.UI;

namespace excels.Worldgen
{
	public class DeepDarkPass : GenPass
	{
		public DeepDarkPass(string name, float loadWeight) : base(name, loadWeight)
		{
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Generating Hyperion Caves";

			GenerateDeepDark((int)(Main.maxTilesX * (Main.rand.NextBool()?0.7f:0.3f)), (int)(Main.maxTilesY * 0.67f));
			//GenerateDeepDark(Main.maxTilesX / 2, (int)(Main.maxTilesY * 0.15f));
		}

		ushort stoneType = (ushort)TileType<Tiles.Blocks.DarkslateTile>();
		ushort wallType = (ushort)WallType<Tiles.Walls.DarkslateWall>();
		ushort crystal = (ushort)TileType<Tiles.OresBars.HyperionTile>();
		ushort ore = (ushort)TileType<Tiles.OresBars.DarksteelOreTile>();

		public bool GenerateDeepDark(int i, int j)
		{
			Vector2 orig = new Vector2(i, j);

			int circle_amount = 16;

			var circles = new List<Vector2>();
			var circles2 = new List<Vector2>();

			var circles_size = new List<int>();
			var circles2_size = new List<int>();
			var generate_cluster = new List<bool>();

			#region Generate the caves
			// Generates the center cave
			for (var cc = 0; cc < circle_amount; cc++)
			{
				int size = Main.rand.Next(24, 32);
				circles.Add(new Vector2(i, j));
				circles_size.Add(size);
				if (size <= 20)
					generate_cluster.Add(true);
				else
					generate_cluster.Add(false);

				/* Generates Circles */
				Point point = new Point(i, j);
				WorldUtils.Gen(point, new Shapes.Circle(circles_size[cc]), Actions.Chain(new GenAction[]
				{
					new Actions.SetTile(stoneType),
					new Actions.PlaceWall(wallType),
				}));
				WorldUtils.Gen(point, new Shapes.Circle((int)(circles_size[cc] * 1.2f)), Actions.Chain(new GenAction[]
				{
					new Modifiers.Dither(.33f),
					new Actions.SetTile(stoneType),
					new Actions.PlaceWall(wallType),
				}));
				WorldUtils.Gen(point, new Shapes.Circle((int)(circles_size[cc] * 1.4f)), Actions.Chain(new GenAction[]
				{
					new Modifiers.Dither(.66f),
					new Actions.SetTile(stoneType),
					new Actions.PlaceWall(wallType),
				}));


				if (i > Main.maxTilesX / 2)
					i += (int)(size * 0.8f);
				else
					i -= (int)(size * 0.8f);

				j = (int)orig.Y + Main.rand.Next(-17, 18);
			}


			i = (int)circles[2].X;
			j = (int)(circles[2].Y - 35);
			int orig2 = j;
			// Generate a second smaller cave above
			for (var cc = 0; cc < circle_amount - 3; cc++)
			{
				int size = Main.rand.Next(22, 28);
				circles2.Add(new Vector2(i, j));
				circles2_size.Add(size);
				if (size <= 20)
					generate_cluster.Add(true);
				else
					generate_cluster.Add(false);

				/* Generates Circles */
				Point point = new Point(i, j);
				WorldUtils.Gen(point, new Shapes.Circle(circles2_size[cc]), Actions.Chain(new GenAction[]
				{
					new Actions.SetTile(stoneType),
					new Actions.PlaceWall(wallType),
				}));
				WorldUtils.Gen(point, new Shapes.Circle((int)(circles2_size[cc] * 1.2f)), Actions.Chain(new GenAction[]
				{
					new Modifiers.Dither(.33f),
					new Actions.SetTile(stoneType),
					new Actions.PlaceWall(wallType),
				}));
				WorldUtils.Gen(point, new Shapes.Circle((int)(circles2_size[cc] * 1.4f)), Actions.Chain(new GenAction[]
				{
					new Modifiers.Dither(.66f),
					new Actions.SetTile(stoneType),
					new Actions.PlaceWall(wallType),
				}));


				if (i > Main.maxTilesX / 2)
					i += (int)(size * 0.8f);
				else
					i -= (int)(size * 0.8f);

				j = (int)orig2 + Main.rand.Next(-11, 12);
			}

			#endregion

			// Generates smaller clusters around the circles
			for (var mc = 0; mc < circle_amount; mc++)
			{
				if (generate_cluster[mc])
				{
					var origin = Main.rand.Next(circle_amount);

					var center = circles[origin];
					var dist = circles_size[origin];

					int height = (int)(dist * Main.rand.NextFloat(0.9f, 1.15f));
					if (Main.rand.NextBool())
						height = -height;

					var size = Main.rand.Next(7, 16);

					Point point = new Point((int)(center.X + Main.rand.Next(-10, 11)), (int)(center.Y + height));
					WorldUtils.Gen(point, new Shapes.Circle(size), Actions.Chain(new GenAction[]
					{
						new Actions.SetTile(stoneType),
						new Actions.PlaceWall(wallType),
					}));
					WorldUtils.Gen(point, new Shapes.Circle((int)(size * 1.2f)), Actions.Chain(new GenAction[]
					{
						new Modifiers.Dither(.33f),
						new Actions.SetTile(stoneType),
						new Actions.PlaceWall(wallType),
					}));
					WorldUtils.Gen(point, new Shapes.Circle((int)(size * 1.4f)), Actions.Chain(new GenAction[]
					{
						new Modifiers.Dither(.66f),
						new Actions.SetTile(stoneType),
						new Actions.PlaceWall(wallType),
					}));
				}
			}

			#region Generates the paths

			// Generates paths between the middle path
			for (var cc1 = 0; cc1 < circle_amount - 1; cc1++)
			{
				var pos1 = circles[cc1];
				var pos2 = circles[cc1 + 1] + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));

				for (var ccc = 0; ccc < 5; ccc++)
				{
					Point point = new Point((int)(pos1.X + ((pos2.X - pos1.X) / 5 * ccc)), (int)(pos1.Y + ((pos2.Y - pos1.Y) / 5 * ccc)));
					float mult = Main.rand.NextFloat(0.2f, 0.45f);

					WorldUtils.Gen(point, new Shapes.Circle((int)(circles_size[cc1] * mult)), Actions.Chain(new GenAction[]
					{
						new Actions.ClearTile(),
					}));
				}
			}

			// Generates paths between the upper path
			for (var cc1 = 0; cc1 < circle_amount - 6; cc1++)
			{
				var pos1 = circles2[cc1];
				var pos2 = circles2[cc1 + 1] + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));

				for (var ccc = 0; ccc < 5; ccc++)
				{
					Point point = new Point((int)(pos1.X + ((pos2.X - pos1.X) / 5 * ccc)), (int)(pos1.Y + ((pos2.Y - pos1.Y) / 5 * ccc)));
					float mult = Main.rand.NextFloat(0.3f, 0.44f);

					WorldUtils.Gen(point, new Shapes.Circle((int)(circles2_size[cc1] * mult)), Actions.Chain(new GenAction[]
					{
						new Actions.ClearTile(),
					}));
				}
			}

			// Generates paths from top to bottom
			for (var cc3 = 0; cc3 < 2; cc3++)
			{
				var pos1 = circles[2 + (9 * cc3) + Main.rand.Next(-1, 2)];
				var pos2 = circles2[2 + (8 * cc3) + Main.rand.Next(-1, 2)] + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));

				for (var ccc = 0; ccc < 7; ccc++)
				{
					Point point = new Point((int)(pos1.X + ((pos2.X - pos1.X) / 7 * ccc)), (int)(pos1.Y + ((pos2.Y - pos1.Y) / 7 * ccc)));
					float mult = Main.rand.NextFloat(0.3f, 0.44f);

					WorldUtils.Gen(point, new Shapes.Circle((int)(circles_size[cc3] * mult)), Actions.Chain(new GenAction[]
					{
						new Actions.ClearTile(),
					}));
				}
			}
			#endregion

			// Generates ore between the layers
			int veinsGenerated = 0;
			while (veinsGenerated < 40)
			{
				for (var k = 0; k < circle_amount; k++)
				{
					for (var l = 0; l < Main.rand.Next(3); l++)
					{
						Vector2 origin = circles[k];
						origin += Main.rand.NextVector2Circular(18, 18);

						OreVeins(origin.ToPoint(), Main.rand.Next(2, 5));
						veinsGenerated++;
					}
				}

				for (var k = 0; k < circles2.Count; k++)
				{
					for (var l = 0; l < Main.rand.Next(2); l++)
					{
						Vector2 origin = circles2[k];
						origin += Main.rand.NextVector2Circular(15, 15);

						OreVeins(origin.ToPoint(), Main.rand.Next(2, 4));
						veinsGenerated++;
					}
				}
			}

			#region Generates openings
			for (var o1 = 0; o1 < 3; o1++)
			{
				Point point = new Point((int)(circles[0].X + (25 * o1)), (int)(circles[0].Y + Main.rand.Next(-8, 9)));
				WorldUtils.Gen(point, new Shapes.Circle((int)(circles_size[0] * 0.4f)), Actions.Chain(new GenAction[]
				{
					new Actions.ClearTile(),
				}));

				point = new Point((int)(circles[circle_amount - 1].X - (25 * o1)), (int)(circles[circle_amount - 1].Y + Main.rand.Next(-8, 9)));
				WorldUtils.Gen(point, new Shapes.Circle((int)(circles_size[circle_amount - 1] * 0.4f)), Actions.Chain(new GenAction[]
				{
					new Actions.ClearTile(),
				}));
			}
			#endregion


			// Generates Crystals
			for (var cc2 = 0; cc2 < circle_amount - 1; cc2++)
			{
				int size = (int)(circles_size[cc2] * 0.67);
				for (var wid = -size / 2 + 1; wid < size - 1; wid++)
				{
					for (var hei = -size / 2 + 1; hei < size - 1; hei++)
					{
						int x = (int)(circles[cc2].X + wid);
						int y = (int)(circles[cc2].Y + hei);

						bool canGenerate = true;

						// Check for any nearby crystals
						for (var ii = -12; ii < 12; ii++)
						{
							for (var jj = -12; jj < 12; jj++)
							{
								if (Main.tile[x + ii, y + jj].TileType == crystal)
									canGenerate = false;
							}
						}

						if (canGenerate)
						{
							Tile t = Main.tile[x, y];

							if (t.TileType == stoneType && t.TileType == stoneType && t.HasTile && Main.rand.NextBool(10))
								GenerateCrystal(x, y);
						}
					}
				}
			}

			for (var cc2 = 0; cc2 < circle_amount - 6; cc2++)
			{
				int size = (int)(circles2_size[cc2] * 0.67);
				for (var wid = -size / 2 + 1; wid < size - 1; wid++)
				{
					for (var hei = -size / 2 + 1; hei < size - 1; hei++)
					{
						int x = (int)(circles2[cc2].X + wid);
						int y = (int)(circles2[cc2].Y + hei);

						bool canGenerate = true;

						// Check for any nearby crystals
						for (var ii = -12; ii < 12; ii++)
						{
							for (var jj = -12; jj < 12; jj++)
							{
								if (Main.tile[x + ii, y + jj].TileType == crystal)
								{
									canGenerate = false;
								}
							}
						}

						if (canGenerate)
						{
							Tile t = Main.tile[x, y];

							if (t.TileType == stoneType && t.TileType == stoneType && t.HasTile && Main.rand.NextBool(10))
								GenerateCrystal(x, y);
						}
					}
				}
			}

			return true;
		}

		public void OreVeins(Point origin, int strength)
		{
			for (var i = 0; i < 3; i++)
			{
				WorldUtils.Gen(origin, new Shapes.Circle(strength), Actions.Chain(new GenAction[]
				{
					new Modifiers.OnlyTiles(stoneType),
					new Modifiers.Dither(0.25f),
					new Actions.SetTileKeepWall(ore),
				}));

				origin += Main.rand.NextVector2Circular(strength * 2, strength * 2).ToPoint();
			}
		}


		private readonly int[,] _crystal1shape = {
			{0,0,1,0,0},
			{0,1,1,1,0},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{0,1,1,1,0},
			{0,0,1,0,0},
		};

		private readonly int[,] _crystal2shape = {
			{0,0,1,1,0,0},
			{0,1,1,1,1,0},
			{1,1,1,1,1,1},
			{1,1,1,1,1,1},
			{1,1,1,1,1,1},
			{1,1,1,1,1,1},
			{1,1,1,1,1,1},
			{0,1,1,1,1,0},
			{0,0,1,1,0,0},
		};

		private readonly int[,] _crystal3shape = {
			{0,0,1,0,0},
			{0,1,1,1,0},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{0,1,1,1,0},
			{0,0,1,0,0},
		};

		private readonly int[,] _crystal4shape = {
			{0,1,0},
			{1,1,1},
			{1,1,1},
			{1,1,1},
			{1,1,1},
			{1,1,1},
			{1,1,1},
			{1,1,1},
			{0,1,0},
		};

		private readonly int[,] _crystal5shape = {
			{0,0,0,0,1,1,1},
			{0,0,0,1,1,1,1},
			{0,0,1,1,1,1,1},
			{0,1,1,1,1,1,0},
			{1,1,1,1,1,0,0},
			{1,1,1,1,0,0,0},
			{1,1,1,0,0,0,0},
		};

		private readonly int[,] _crystal6shape = {
			{1,1,1,0,0,0,0},
			{1,1,1,1,0,0,0},
			{1,1,1,1,1,0,0},
			{0,1,1,1,1,1,0},
			{0,0,1,1,1,1,1},
			{0,0,0,1,1,1,1},
			{0,0,0,0,1,1,1},
		};


		public bool GenerateCrystal(int i, int j)
		{
			int[,] shape = _crystal1shape;
			switch (Main.rand.Next(6))
			{
				case 0: shape = _crystal1shape; break;
				case 1: shape = _crystal2shape; break;
				case 2: shape = _crystal3shape; break;
				case 3: shape = _crystal4shape; break;
				case 4: shape = _crystal5shape; break;
				case 5: shape = _crystal6shape; break;
			}

			for (int y = 0; y < shape.GetLength(0); y++)
			{
				for (int x = 0; x < shape.GetLength(1); x++)
				{
					int k = i - (shape.GetLength(1) / 2) + x;
					int l = j - (shape.GetLength(0) / 2) + y;

					if (shape[y, x] == 1)
					{
						Main.tile[k, l].ClearTile();
						//int t = Main.tile[k, l].TileType;
						//if (t == TileID.Stone || t == TileID.Dirt || t == TileID.Silt || t == TileID.Mud || t == TileID.ClayBlock)
						if (!Main.tile[k, l].HasTile)
							WorldGen.PlaceTile(k, l, crystal);
					}
				}
			}
			return true;
		}
	}
}
