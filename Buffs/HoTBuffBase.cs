using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace excels.Buffs
{
    public abstract class HoTBuffBase : ModBuff
    {
        int healingTime = 0;
        public int healAmount = 0;
        public int healFrames = 30;

        public override void Update(Player player, ref int buffIndex)
        {
            if (++healingTime >= healFrames && healAmount > 0)
            {
                player.statLife += healAmount;
                player.HealEffect(healAmount);
                if (player.statLife > player.statLifeMax2)
                    player.statLife = player.statLifeMax2;

                healingTime -= healFrames;
            }

            SafeUpdate(player, ref buffIndex);
        }

        public virtual void SafeUpdate(Player player, ref int buffIndex)
        {

        }
    }
}
