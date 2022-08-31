using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace excels.Tiles.Banners
{
    internal class ExcelBanners : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };	
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			
			DustType = -1;
			

			TileID.Sets.DisableSmartCursor[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}


		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int style = frameX / 18;
			int item;
			switch (style)
			{
				case 0:
					item = ModContent.ItemType<BItems.BannerRexxie>();
					break;
				case 1:
					item = ModContent.ItemType<BItems.BannerFossiliraptor>();
					break;
				case 2:
					item = ModContent.ItemType<BItems.BannerMeteorGolem>();
					break;
				case 3:
					item = ModContent.ItemType<BItems.BannerMeteorSpirit>();
					break;
				case 4:
					item = ModContent.ItemType<BItems.BannerMeteorSlime>();
					break;
				case 5:
					item = ModContent.ItemType<BItems.BannerSkylineSentinal>();
					break;
				case 6:
					item = ModContent.ItemType<BItems.BannerDungeonMimic>();
					break;
				default:
					return;
			}
			Item.NewItem(Main.LocalPlayer.GetSource_DropAsItem(), new Vector2(i * 16, j * 16), item);
		}


		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].TileFrameX / 18;
				int type;
				switch (style)
				{
					case 0:
						type = ModContent.NPCType<NPCs.Fossil.RexxieSkull>();
						break;
					case 1:
						type = ModContent.NPCType<NPCs.Fossil.Fossilraptor>();
						break;
					case 2:
						type = ModContent.NPCType<NPCs.Meteorite.MeteorGolem>();
						break;
					case 3:
						type = ModContent.NPCType<NPCs.Meteorite.MeteorSpirit>();
						break;
					case 4:
						type = ModContent.NPCType<NPCs.Meteorite.FlamingSludge>();
						break;
					case 5:
						type = ModContent.NPCType<NPCs.Space.SkylineHarpy>();
						break;
					case 6:
						type = ModContent.NPCType<NPCs.Dungeon.DungeonMimic>();
						break;
					default:
						return;
				}

				//player.AddBuff(BuffID.MonsterBanner, 60, false, false);
				player.HasNPCBannerBuff(type);
				
			}
		}
	}
}
