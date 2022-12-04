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

namespace excels
{
	public class excelWorld : ModSystem
	{

		private UserInterface _radianceInterface;
		internal RadianceBar RadianceBar;

		public static bool downedNiflheim = false;
		public static bool downedChasm = false;
		public static bool downedStarship = false;

		public static bool downedGoblinSummoner = false;
		public static bool transformedNymph = false;

		public override void OnWorldLoad()
		{
			downedNiflheim = false;
			downedChasm = false;
			downedStarship = false;

			downedGoblinSummoner = false;
			transformedNymph = false;
		}

		public override void OnWorldUnload()
		{
			downedNiflheim = false;
			downedChasm = false;
			downedStarship = false;

			downedGoblinSummoner = false;
			transformedNymph = false;
		}

		public override void OnModLoad()
		{
			if (!Main.dedServ)
			{
				RadianceBar = new RadianceBar();
				RadianceBar.Activate();
				_radianceInterface = new UserInterface();
				_radianceInterface.SetState(RadianceBar);
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			_radianceInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1)
			{
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"Excelsior: Radiance Bar",
					delegate
					{
						_radianceInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

        public override void PostSetupContent()
        {
			//RecipeBroswerIntegration();

			BossChecklistIntegration();
		}

		private void BossChecklistIntegration()
		{
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
				return;

			if (bossChecklistMod.Version < new Version(1, 3, 1))
				return;

			bossChecklistMod.Call(
				"AddBoss",
				Mod,
				"Niflheim",
				ModContent.NPCType<NPCs.Glacial.GlacialQueen>(),
				3.7f,
				() => excelWorld.downedNiflheim,
				true,
				new List<int>() { ItemType<Items.Placeable.Decorations.Relics.NiflheimRelic>() },
				ItemType<Items.Misc.Summons.ReflectiveIceShard>(),
				$"Use a [i:{ItemType<Items.Misc.Summons.ReflectiveIceShard>()}] in a snowy landscape",
				null,
				(SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = ModContent.Request<Texture2D>("excels/NPCs/Glacial/Niflheim").Value;
					Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				}
			);

			bossChecklistMod.Call(
				"AddBoss",
				Mod,
				"Stellar Starship",
				ModContent.NPCType<NPCs.Stellar.StellarStarship>(),
				5.1f,
				() => excelWorld.downedStarship,
				true,
				new List<int>() { ItemType<Items.Placeable.Decorations.Relics.StellarStarshipRelic>(), ItemType<Items.Placeable.Decorations.Trophies.StellarTrophy>(), ItemType<Items.Vanity.StellarStarshipMask>() },
				ItemType<Items.Misc.Summons.PlanetaryTrackingDevice>(),
				$"Use a [i:{ItemType<Items.Misc.Summons.PlanetaryTrackingDevice>()}]",
				null,
				(SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = ModContent.Request<Texture2D>("excels/NPCs/Stellar/StellarStarship").Value;
					Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				}
			);

			bossChecklistMod.Call(
				"AddBoss",
				Mod,
				"Chasm",
				ModContent.NPCType<NPCs.Chasm.ChasmHead>(),
				12.8f,
				() => excelWorld.downedChasm,
				true,
				new List<int>() { ItemType<Items.Placeable.Decorations.Relics.ChasmRelic>(), ItemType<Items.Vanity.ChasmMask>() },
				ItemType<Items.Misc.Summons.InfectionRadar>(),
				$"Use a [i:{ItemType<Items.Misc.Summons.InfectionRadar>()}] in a mushroom biome",
				null,
				(SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = ModContent.Request<Texture2D>("excels/Textures/BossChecklistTextures/ChasmBossCheck").Value;
					Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				}
			);
		}

		private void RecipeBroswerIntegration()
		{
			if (!ModLoader.TryGetMod("RecipeBrowser", out Mod recipeBrowser))
				return;
			if (!Main.dedServ)
			{
				Texture2D cTexture = ModContent.Request<Texture2D>($"excels/Textures/RecipeBrowser/ClericIcon", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				recipeBrowser.Call("AddItemCategory", "Cleric", "Weapons", cTexture, (Predicate<Item>)((Item item) => item.DamageType == ModContent.GetInstance<ClericClass>()));
			}
		}


		public override void SaveWorldData(TagCompound tag)
		{
			if (downedNiflheim)
				tag["downedNiflheim"] = true;
			if (downedChasm)
				tag["downedChasm"] = true;
			if (downedStarship)
				tag["downedStarship"] = true;

			if (downedGoblinSummoner)
				tag["downedSummoner"] = true;
			if (transformedNymph)
				tag["savedNymph"] = true;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			downedNiflheim = tag.ContainsKey("downedNiflheim");
			downedChasm = tag.ContainsKey("downedChasm");
			downedStarship = tag.ContainsKey("downedStarship");

			downedGoblinSummoner = tag.ContainsKey("downedSummoner");
			transformedNymph = tag.ContainsKey("savedNymph");
		}

        public override void NetSend(BinaryWriter writer)
		{
			// Order of operations is important and has to match that of NetReceive
			var flags = new BitsByte();
			flags[0] = downedNiflheim;
			flags[1] = downedChasm;
			flags[2] = downedStarship;
			writer.Write(flags);

			var flagsV = new BitsByte();
			flagsV[0] = downedGoblinSummoner;
			flagsV[1] = transformedNymph;
			writer.Write(flagsV);
		}

		public override void NetReceive(BinaryReader reader)
		{
			// Order of operations is important and has to match that of NetSend
			BitsByte flags = reader.ReadByte();
			downedNiflheim = flags[0];
			downedChasm = flags[1];
			downedStarship = flags[2];

			BitsByte flagsV = reader.ReadByte();
			downedGoblinSummoner = flagsV[0];
			transformedNymph = flagsV[1];
		}

		public override void PostWorldGen()
		{
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Ice Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding. 
				// this looks for the 0 frame, which would be a normal wooden chest
				if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 0 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == ItemID.None)
						{
							if (Main.rand.NextBool(3))
							{
								switch (Main.rand.Next(2))
								{
									default: // 0
										chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Accessories.Banner.RegenBanner>());
										break;

									case 1:
										chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Accessories.Cleric.Healing.ApothSatchel>());
										break;
								}
							}
							break;
						}
					}
				}

				// gold chest
				if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == ItemID.None)
						{
							if (Main.rand.NextBool(6))
							{
								chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Flamethrower.OverchargedFlashlight>());
								chest.item[inventoryIndex+1].SetDefaults(ModContent.ItemType<Items.Ammo.Other.LampOil>());
								chest.item[inventoryIndex+1].stack = Main.rand.Next(40, 81);
							}
							break;
						}
					}
				}

				// locked gold chest
				if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 2 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == ItemID.None)
						{ 
							if (Main.rand.NextBool(4))
								chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Necrotic1.Dungeon.TideClamp>());
							break;
						}
					}
				}

				// locked shadow chest
				if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 4 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == ItemID.None)
						{
							if (Main.rand.NextBool(7))
							{
								chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Flamethrower.Roaster>());
								chest.item[inventoryIndex + 1].SetDefaults(ModContent.ItemType<Items.Ammo.Other.LighterFluid>());
								chest.item[inventoryIndex + 1].stack = Main.rand.Next(74, 102);
							}
							break;
						}
					}
				}
			}
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Random Gems"));

			if (ShiniesIndex != -1)
			{
				tasks.Insert(ShiniesIndex + 1, new SkylineOrePass("Skyline Ores", 237.4298f));
				tasks.Insert(ShiniesIndex + 2, new GladiolusPass("Gladiolus", 237.429999f));
				tasks.Insert(ShiniesIndex + 3, new HyperionPass("Hyperion", 280));
			}

		}

		public override void PostAddRecipes()
		{
			for (var i = 0; i < Recipe.maxRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];
				if (recipe.TryGetResult(ItemID.NightsEdge, out Item _))
				{
					recipe.RemoveTile(TileID.DemonAltar);
					recipe.AddTile(ModContent.TileType<Tiles.Stations.StarlightAnvilTile>());
				}
			}
		}

		int GladiolusTimer = 900; // 15 seconds
		public override void PostUpdateEverything()
		{
			GladiolusTimer--;

			// random herb time
			if (GladiolusTimer <= 0)
			{
				bool GladiolusPlaced = false;

				// attempts to spawn gladiolus 50 times before giving up
				for (var i = 0; i < 300; i++)
				{
					if (!GladiolusPlaced)
					{
						int x = WorldGen.genRand.Next(0, Main.maxTilesX);
						int y = WorldGen.genRand.Next(0, (int)(Main.maxTilesY * 0.25f));
						if (Main.tile[x, y].TileType == TileID.Grass || Main.tile[x, y].TileType == TileID.GolfGrass)
						{
							if (!Main.tile[x, y - 1].HasTile)
							{
								WorldGen.PlaceTile(x, y - 1, ModContent.TileType<Tiles.Herbs.GladiolusTile>());
								GladiolusPlaced = true;
							}
						}
					}
				}

				GladiolusTimer = Main.rand.Next(900, 1500) + 900;
			}
		}
	}

	public class HyperionPass : GenPass
    {
		/*
		 *  Dark Caves Gen
		 *  1 - Generate several large circles of stone and keep their centers in a list
		 *    -> The frequency disaptes as it gets farther from the center
		 *  2 - Generate tunnels by making wide lines through the centers
		 *  3 - Generate Hyperion Crystals attached to the tiles
		*/

		public HyperionPass(string name, float loadWeight) : base(name, loadWeight)
		{
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Hyperion Crystals are forming down below";

			// "6E-05" is "scientific notation". It simply means 0.00006 but in some ways is easier to read.
			for (int t = 0; t < (int)((Main.maxTilesX * Main.maxTilesY) * 3E-05); t++)
			{
				int x = WorldGen.genRand.Next((int)(Main.maxTilesX * 0f), (int)(Main.maxTilesX * 0.4f));
				if (Main.rand.NextBool())
					x = WorldGen.genRand.Next((int)(Main.maxTilesX * 0.6f), (int)(Main.maxTilesX * 1f));

				int y = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.65f), (int)(Main.maxTilesY - 250));

				if (Main.tile[x,y].HasTile && Main.tile[x, y].TileType == TileID.Stone)
					GenerateCrystal(x, y);
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
			{0,1,1,1,0},
			{0,0,1,0,0},
		};

		private readonly int[,] _crystal2shape = {
			{0,0,1,0,0},
			{0,1,1,1,0},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{1,1,1,1,1},
			{0,1,1,1,0},
			{0,0,1,0,0},
		};

		private readonly int[,] _crystal3shape = {
			{0,0,1,0,0},
			{0,1,1,1,0},
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
			{0,1,0},
		};

		private readonly int[,] _crystal5shape = {
			{0,1,0},
			{1,1,1},
			{1,1,1},
			{1,1,1},
			{1,1,1},
			{1,1,1},
			{1,1,1},
			{0,1,0},
		};


		public bool GenerateCrystal(int i, int j)
		{
			int[,] shape = _crystal1shape;
			switch (Main.rand.Next(5))
            {
				case 0: shape = _crystal1shape; break;
				case 1: shape = _crystal2shape; break;
				case 2: shape = _crystal3shape; break;
				case 3: shape = _crystal4shape; break;
				case 4: shape = _crystal5shape; break;

			}

			for (int y = 0; y < shape.GetLength(0); y++)
			{
				for (int x = 0; x < shape.GetLength(1); x++)
				{
					int k = i - (shape.GetLength(1) / 2) + x;
					int l = j - (shape.GetLength(0) / 2) + y;

					if (shape[y,x] == 1)
                    {
						//int t = Main.tile[k, l].TileType;
						//if (t == TileID.Stone || t == TileID.Dirt || t == TileID.Silt || t == TileID.Mud || t == TileID.ClayBlock)
							Main.tile[k, l].ClearTile();

						WorldGen.PlaceTile(k, l, ModContent.TileType<Tiles.OresBars.HyperionTile>());
						Tile tile = Framing.GetTileSafely(k, l);
					}

				}
			}
			return true;
		}
	}

	public class GladiolusPass : GenPass
    {
		public GladiolusPass(string name, float loadWeight) : base(name, loadWeight)
		{
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
			// has 400 tries to create 50 herbs, growth stage is randomized
			int GladCount = 0;
			for (var i = 0; i < 400; i++)
			{
				if (GladCount < 50)
				{
					int x = WorldGen.genRand.Next(0, Main.maxTilesX);
					int y = WorldGen.genRand.Next(0, (int)(Main.maxTilesY * 0.25f));
					if (Main.tile[x, y].TileType == TileID.Grass || Main.tile[x, y].TileType == TileID.GolfGrass)
					{
						if (!Main.tile[x, y - 1].HasTile)
						{
							WorldGen.PlaceTile(x, y - 1, ModContent.TileType<Tiles.Herbs.GladiolusTile>(), false, false, -1, Main.rand.Next(3));
							GladCount++;
						}
					}
				}
			}
		}
    }

	public class SkylineOrePass : GenPass
    {
		public SkylineOrePass(string name, float loadWeight) : base(name, loadWeight)
		{
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Skyline Stars are filling the outer skies";

			// "6E-05" is "scientific notation". It simply means 0.00006 but in some ways is easier to read.
			for (int t = 0; t < (int)((Main.maxTilesX * Main.maxTilesY) * 2E-05); t++)
			{

				int x = WorldGen.genRand.Next((int)(Main.maxTilesX * 0.015f), (int)(Main.maxTilesX * 0.985f));
				if (x < Main.maxTilesX * 0.4f || x > Main.maxTilesX * 0.6f)
				{
					int y = WorldGen.genRand.Next(20, (int)(Main.maxTilesY * 0.1f));
					PlaceSkyline(x, y);
					//WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), ModContent.TileType<Tiles.OresBars.GlacialOreTile>()w);
				}
			}
		}

		private readonly int[,] _skylineoreshape = {
			{0,0,0,0,1,0,0,0,0},
			{0,1,2,2,1,2,2,1,0},
			{0,2,1,2,1,2,1,2,0},
			{0,2,2,1,1,1,2,2,0},
			{1,1,1,1,2,1,1,1,1},
			{0,2,2,1,1,1,2,2,0},
			{0,2,1,2,1,2,1,2,0},
			{0,1,2,2,1,2,2,1,0},
			{0,0,0,0,1,0,0,0,0},
		};
		private readonly int[,] _skylineoreslopeshape = {
			{0,0,0,0,0,0,0,0,0},
			{0,0,1,2,0,1,2,0,0},
			{0,4,0,0,0,0,0,3,0},
			{0,2,0,0,0,0,0,1,0},
			{0,0,0,0,0,0,0,0,0},
			{0,4,0,0,0,0,0,3,0},
			{0,2,0,0,0,0,0,1,0},
			{0,0,3,4,0,3,4,0,0},
			{0,0,0,0,0,0,0,0,0},
		};
		private readonly int[,] _skylineorewallshape = {
			{0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0},
			{0,0,1,1,1,1,1,0,0},
			{0,0,1,1,1,1,1,0,0},
			{0,0,1,1,1,1,1,0,0},
			{0,0,1,1,1,1,1,0,0},
			{0,0,1,1,1,1,1,0,0},
			{0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0},
		};

		public bool PlaceSkyline(int i, int j)
		{
			for (int y = 0; y < _skylineoreshape.GetLength(0); y++)
			{
				for (int x = 0; x < _skylineoreshape.GetLength(1); x++)
				{
					int k = i - 4 + x;
					int l = j - 4 + y;

					int tileType = 0;
					SlopeType slopeType = 0;
					switch (_skylineoreshape[y, x])
					{
						case 1:
							tileType = ModContent.TileType<Tiles.OresBars.SkylineOreTile>();
							break;
						case 3:
							tileType = TileID.Diamond;
							break;
						case 2:
							tileType = TileID.Cloud;
							break;
					}
					switch (_skylineoreslopeshape[y, x])
					{
						case 1:
							slopeType = SlopeType.SlopeDownLeft; 
							break;
						case 2:
							slopeType = SlopeType.SlopeDownRight;
							break;
						case 3:
							slopeType = SlopeType.SlopeUpLeft;
							break;
						case 4:
							slopeType = SlopeType.SlopeUpRight;
							break;
					}
					switch (_skylineorewallshape[y, x])
					{
						case 1:
							WorldGen.PlaceWall(k, l, WallID.Cloud);
							break;
					}
					if (tileType != 0)
					{
						WorldGen.PlaceTile(k, l, tileType);
						Tile tile = Framing.GetTileSafely(k, l);
						tile.Slope = slopeType;
					}
				}
			}
			return true;
		}
	}
}
