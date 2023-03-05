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
using excels.Items.Materials;

namespace excels.Tiles.Decorations.Furniture.Tabletop
{
    internal class BottleOfSunlightPlaced : ModTile
    {
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.addTile(Type);

			ItemDrop = ModContent.ItemType<BottleOfSunlight>();
			HitSound = SoundID.Shatter;
			DustType = DustID.Glass;

			AddMapEntry(new Color(251, 216, 91));

		}

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			r = 2.44f * 0.4f;
			g = 2.55f * 0.4f;
			b = 1.32f * 0.4f;
		}

        public override bool RightClick(int i, int j)
        {
			bool y = false;
			KillTile(i, j, ref y, ref y, ref y);
            return base.RightClick(i, j);
        }
    }
}
