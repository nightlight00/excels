using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using System.Collections.Generic;

namespace excels.Items.Misc
{
    public class MinorBlessing : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.IsAPickup[Item.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.CloneDefaults(ItemID.Heart);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightGoldenrodYellow.ToVector3() * 0.45f);
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange = 300;
        }

        public override bool OnPickup(Player player)
        {
            player.GetModPlayer<ClericClassPlayer>().radianceStatCurrent += 10;
            CombatText.NewText(player.getRect(), Color.LightGoldenrodYellow, 10);
            return false;
        }
    }



    internal class ClericsHandbook : ModItem
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.Book}";

        int page = 0;
        int maxPage = 4;

        string title = "A short guide to help your cleric journey\nRight Click to flip the page";
        string seperator = "----------";

        string tableOfContents = "Page 1: Radiant Weapons & Radiance Bar\nPage 2: Support Tools\nPage 3: Necrotic Weapons\nPage 4: Synergies"; 
        string radiant = "Radiant Weapons & Radiance Bar\nRadiant weapons are your basic tools.  \nThey require 'radiance' to function, which can be seen while holding any cleric weapon.  \n'Radiance' regenerates slowly over time, or can be filled faster by healing allies.";
        string support = "Support Tools\nSupport tools can be used to either heal or buff allies, or even both!\nHealing allies not only helps them out, but will also benefit you (Check synergies for more info).";
        string necrotic = "Necrotic Weapons\nNecrotic weapons are your special weapons.\nThey require health to use and can usually steal enemy life as well.\nWhen used, you'll recieve 'Anguished Soul', which halves positive regeneration.";
        string synergy = "Synergies\nRadiant weapons gain 25% of Nectrotic weapon bonuses, and vice versa.\nHealing allies fills up your 'radiance' bar and decreases the duration of 'Anguished Soul'\n'Anguished Soul' increases radiance regenertaion rate";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cleric's Handbook");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 1;
            
        }

        public override bool ConsumeItem(Player player) => false;

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            if (++page > maxPage)
                page = 0;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Title", title));
            tooltips.Add(new TooltipLine(Mod, "Sep", seperator));
            switch (page)
            {
                case 0:
                    tooltips.Add(new TooltipLine(Mod, "Tip", tableOfContents));
                    break;

                case 1:
                    tooltips.Add(new TooltipLine(Mod, "Tip", radiant));
                    break;

                case 2:
                    tooltips.Add(new TooltipLine(Mod, "Tip", support));
                    break;

                case 3:
                    tooltips.Add(new TooltipLine(Mod, "Tip", necrotic));
                    break;

                case 4:
                    tooltips.Add(new TooltipLine(Mod, "Tip", synergy));
                    break;
            }
        }
    }
}
