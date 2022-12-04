using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Buffs.ClericBonus
{
    public abstract class ClericBonusBuff : HoTBuffBase
    {
        public string BuffName;
        public string BuffDesc;

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Names();
            DisplayName.SetDefault(BuffName);
            Description.SetDefault(BuffDesc);
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = false;
        }

        public virtual void Names()
        {

        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ModContent.RarityType<Items.ClericBuffRarity>();
        }
    }

    internal class GlacialGuardBuff : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Glacial Guard";
            BuffDesc = "Increases defense by 15 but decreases movement abilities";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 15;

            player.GetModPlayer<excelPlayer>().GlacialGuard = true;
        }
    }

    internal class HolyGuardBuff : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Holy Guard";
            BuffDesc = "You're being protected by the holy light";
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 10;
        }
    }

    internal class FloralBeauty : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Floral Beauty";
            BuffDesc = "Increased life regeneration";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 5;
        }
    }

    internal class EnergyFlowBuff : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Energy Flow";
            BuffDesc = "Increased damage, critical strike chance and movement speed";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.GetCritChance(DamageClass.Generic) += 5;
            player.moveSpeed += 0.2f;
        }
    }

    internal class PrismiteHeart : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Prismite Heart";
            BuffDesc = "Temporarily increases max health";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 += 40;
        }
    }

    internal class Supplied : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Supplied";
            BuffDesc = "Offensive abilities boosted\n10% increased melee and whip speed, 10% decreased ammo usage, and increased mana and radiance regeneration";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
            player.manaRegen += 2;
            player.GetModPlayer<ClericClassPlayer>().radianceRegenRate += 2;
            // ammo reduction is done in mod player
        }
    }

    internal class SoothingSoul : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Soothing Soul : [c/76d550:6HP/s]";
            BuffDesc = "Cured from minor damaging status effects and healing over time";

            healAmount = 3;
            healFrames = 30;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            player.buffTime[buffIndex] += time / 2;
            return false;
        }
    }

    internal class NaturesHeart : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Nature's Heart : [c/76d550:2HP/s]";
            BuffDesc = "Slowly transforming nature's energy into health";

            healAmount = 1;
            healFrames = 30;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            player.buffTime[buffIndex] += time / 2;
            if (player.buffTime[buffIndex] > 480)
                player.buffTime[buffIndex] = 480;
            return false;
        }

        public override void SafeUpdate(Player player, ref int buffIndex)
        {
            player.statDefense += 2;
        }
    }

    internal class PropheticWisdom : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Prophetic Wisdom : [c/76d550:3HP/s]";
            BuffDesc = "Blessings purify your soul";

            healAmount = 1;
            healFrames = 20;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            player.buffTime[buffIndex] += time / 2;
            return false;
        }

        public override void SafeUpdate(Player player, ref int buffIndex)
        {
            player.lifeRegen++;
        }
    }

    // Heretic Breaker Buff
    internal class HolySavagry : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Holy Savagry";
            BuffDesc = "10% increased damage and 6% increased critical strike chance\nCritical strikes heal yourself";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.GetCritChance(DamageClass.Generic) += 6;
            player.GetModPlayer<excelPlayer>().HereticBreakerBuff = true;
        }
    }

    // Sonic Syringe Buff
    internal class EuphoricGuard : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Absolute Defense";
            BuffDesc = "Defense increased by 10 and 6% increased damage reduction\nYou have a 10% chance to block attacks";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 10;
            player.endurance += 0.06f;
            player.GetModPlayer<excelPlayer>().SonicSyringeBuff = true;
        }
    }


    internal class GuardianPhoenix : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Guardian Pheonix";
            BuffDesc = "Decreases damage taken by 10% and immunity to On Fire!";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.1f;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true;
        }
    }

    internal class PriestessBlessingRadiance : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Priestess' Blessing : Radiance";
            BuffDesc = "Increases healing potency by 1";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<excelPlayer>().healBonus += 1;
        }
    }

    internal class PriestessBlessingNecrotic : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Priestess' Blessing : Necrotic";
            BuffDesc = "Necrotic blood cost reduced by 15%";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<excelPlayer>().bloodCostMult -= 0.15f;
        }
    }
}
