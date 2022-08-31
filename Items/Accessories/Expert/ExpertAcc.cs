using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace excels.Items.Accessories.Expert
{
    
    internal class NiflheimExpertAcc : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voluspa");
            Tooltip.SetDefault("All attacks are imbued with frost fire \nGrants immunity to all cold related debuffs");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.expert = true;
            Item.accessory = true;

        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().NiflheimAcc = true;

            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Chilled] = true;
        }
    }
    
    internal class ChasmExpertAcc : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Medicine");
            Tooltip.SetDefault("While under the effects of damaging debuffs, gain increased defense, damage, and critical strike chance" +
                             "\nDecreases all damage taken by 10%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.expert = true;
            Item.accessory = true;
            
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += 0.1f;

            int effects = 0;
            for (var i = 0; i < player.CountBuffs(); i++)
            {
                int b = player.buffType[i];
                if (b == BuffID.OnFire || b == BuffID.CursedInferno || b == BuffID.Ichor || b == BuffID.Frostburn || b == BuffID.ShadowFlame || b == BuffID.Poisoned || b == BuffID.Venom  || b == BuffID.Bleeding || b == BuffID.Rabies || b == ModContent.BuffType<Buffs.Debuffs.Mycosis>())
                {
                    effects++;
                }
            }
            if (effects > 0)
            {
                player.GetModPlayer<excelPlayer>().ChasmAcc = true;
                player.statDefense += 8 + ((effects - 1) * 2);
                player.GetDamage(DamageClass.Generic) += 0.1f + ((effects - 1) * 0.02f);
                player.GetCritChance(DamageClass.Generic) += 5 + (effects - 1);
            }
        }
    }

    internal class StellarExpertAcc : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("High-Tech Battle Plans");
            Tooltip.SetDefault("Summons a memeber of the stellar army to fight with you");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.expert = true;
            Item.accessory = true;

        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().StellarAcc = true;

            if (player.ownedProjectileCounts[ModContent.ProjectileType<StellarMinionA>()] == 0)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, new Vector2(0, -3), ModContent.ProjectileType<StellarMinionA>(), 0, 0, player.whoAmI);
            }
        }
    }
}
