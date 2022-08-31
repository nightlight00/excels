using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace excels.Items
{
    public class StellarRarity : ModRarity
    {
        public override Color RarityColor => new Color((byte)(96), (byte)(58), (byte)(185));

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            // normally would want offset > 0, but this makes this rarity appear more often and i kinda want to flex custom rare lol
            if (offset > 1)
            {
                return ItemRarityID.Pink;
            }
            if (offset < -1)
            {
                return ItemRarityID.LightRed;
            }

            return Type;
        }
    }

    public class ClericBuffRarity : ModRarity
    {
        public override Color RarityColor => new Color((byte)(213), (byte)(119), (byte)(242));

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            return Type;
        }
    }
}
