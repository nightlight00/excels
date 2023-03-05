using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace excels.Buffs.HealOverTime
{
    internal class RadiantSoul : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radiant Soul");
            Main.buffNoTimeDisplay[Type] = true;
        }

        Player owner = null;

        public override void Update(Player player, ref int buffIndex)
        {
            if (owner == null)
                owner = player;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (owner == null)
                return;

            var modPlayer = owner.GetModPlayer<HealOverTime>();

            if (modPlayer.healsPerSecond.Count == 0)
                return;

            tip = $"Radiant energy soothes your soul\nTotal Healing ([c/76d550:{modPlayer.totalHealsPerSecond}HP/s])";

            for (var i = 0; i < modPlayer.healsPerSecond.Count; i++)
            {
                tip += $"\n{modPlayer.healSource[i]} ([c/76d550:{modPlayer.healsPerSecond[i]}HP/s]) : {TimeToString(modPlayer.healDuration[i])}";
            }
        }

        private string TimeToString(decimal time)
        {
            int seconds = (int)Math.Floor(time / 60);

            string returnValue = $"{seconds}s";
            if (seconds >= 60)
            {
                int minutes = 0;
                while (seconds >= 60)
                {
                    minutes++;
                    seconds -= 60;
                }
                returnValue = $"{minutes}m " + returnValue;
            }

            return returnValue;
        }
    }

    internal class HealOverTime : ModPlayer
    {
        public List<string> healSource = new List<string>();
        public List<int> healsPerSecond = new List<int>();
        public List<int> healDuration = new List<int>();
        public List<int> healFrequency = new List<int>();

        public int totalHealsPerSecond = 0;

        int healToShow = 0;
        int timeUntilHeal = 0;

        public virtual void AddHeal(Player healer, string healingSource, int healingPerSecond, int durationInSeconds)
        {
            healingPerSecond += healer.GetModPlayer<excelPlayer>().healBonus;
            durationInSeconds += healer.GetModPlayer<excelPlayer>().buffBonus;
            durationInSeconds *= 60;

            if (healsPerSecond.Count > 0) {

                for (var i = 0; i < healsPerSecond.Count; i++)
                {
                    if (healSource[i] == healingSource)
                    {
                        if (healDuration[i] < durationInSeconds)
                        {
                            healDuration[i] = durationInSeconds;
                            return;
                        }
                    }
                }
            }

            int frequency = 1;
            if (healingPerSecond % 5 == 0)
                frequency = 5; // 60 / 5 = 12
            else if (healingPerSecond % 3 == 0)
                frequency = 3; // 60 / 3 = 20
            else if (healingPerSecond % 2 == 0)
                frequency = 2; // 60 / 3 = 30
            else
                frequency = 1;

            healSource.Add(healingSource);
            healsPerSecond.Add(healingPerSecond);
            healDuration.Add(durationInSeconds);
            healFrequency.Add(frequency);
        }

        public override void UpdateLifeRegen()
        {
            // Prevent code from running if no healing can be done
            if (healsPerSecond.Count <= 0 || Player.dead || !Player.active)
                return;

            // Add the buff if not present
            if (!Player.HasBuff(ModContent.BuffType<RadiantSoul>()))
                Player.AddBuff(ModContent.BuffType<RadiantSoul>(), 2);

            // Make the buff stay
            Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<RadiantSoul>())] = 2;

            // Add the heal amount to variable
            totalHealsPerSecond = 0;
            for (var i = 0; i < healsPerSecond.Count; i++)
            {
                totalHealsPerSecond += healsPerSecond[i];
                healDuration[i]--;
                if (healDuration[i] % (60 / healFrequency[i]) == 0)
                {
                    //CombatText.NewText(Player.getRect(), Color.White, $"{healsPerSecond[i]} | {healFrequency[i]} | {healsPerSecond[i] / healFrequency[i]}");
                    healToShow += (int)(healsPerSecond[i] / healFrequency[i]);
                    if (healDuration[i] <= 0)
                    {
                        healSource.RemoveAt(i);
                        healsPerSecond.RemoveAt(i);
                        healDuration.RemoveAt(i);
                        healFrequency.RemoveAt(i);
                    }
                }
            }

            // Perform actual healing
            int frequency2 = 1;
            if (totalHealsPerSecond % 5 == 0)
                frequency2 = 5;
            else if (totalHealsPerSecond % 3 == 0)
                frequency2 = 3;
            else if (totalHealsPerSecond % 2 == 0)
                frequency2 = 2;

            timeUntilHeal++;
            if (timeUntilHeal % (60 / frequency2) == 0)
            {
                // We don't want to show nothing
                if (healToShow > 0)
                {
                    Player.statLife += healToShow;
                    if (Player.statLife > Player.statLifeMax2)
                        Player.statLife = Player.statLifeMax2;
                    Player.HealEffect(healToShow);

                    healToShow = 0;
                }
            }

            // Remove buff if no more healing being done
            if (healsPerSecond.Count == 0)
            {
                Player.ClearBuff(ModContent.BuffType<RadiantSoul>());
            }
        }
    }
}
