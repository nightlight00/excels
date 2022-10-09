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
using ReLogic.Graphics;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using excels.UI;

using System.Windows.Forms;

namespace excels
{
	public class excels : Mod
	{
        public override void Load()
        {
            // https://www.youtube.com/watch?v=mgtvLaPYL-I
            // interesting idea, to have a popup appear
            // MessageBox.Show("", "");
            // Enviroment.UserName
        }

        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("DialogueTweak", out Mod dialogueTweak))
            {
                //   dialogueTweak.Call("ReplacePortrait", NPCType<NPCs.Town.Priestess>(), "excels/Textures/excelsIconTest");
            }

        }
        public override uint ExtraPlayerBuffSlots => 44;
    }
}