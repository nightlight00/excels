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
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Enums;

namespace excels.Tiles.Decorations.Treasure
{
    internal class TestHeart : ModTile
    {
        public override void SetStaticDefaults()
        {
			// Properties
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			// Main.tileFlame[Type] = true; // This breaks it.

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
		}
    }
}
