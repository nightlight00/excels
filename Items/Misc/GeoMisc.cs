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
using Terraria.GameContent.ItemDropRules;

namespace excels.Items.Misc
{
    internal class GeoSac : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Geologist Reward Sac");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}\n'I wonder whats inside?");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.rare = 1;
            Item.consumable = true;
            Item.maxStack = 999;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Crates;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int[] miningArmor = new int[] {
                ItemID.MiningHelmet,
                ItemID.MiningShirt,
                ItemID.MiningPants
            };
            itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, miningArmor));

            IItemDropRule[] potions = new IItemDropRule[]
            {
                ItemDropRule.Common(ItemID.SpelunkerPotion, 1, 1, 3),
                ItemDropRule.Common(ItemID.MiningPotion, 1, 2, 4),
                ItemDropRule.Common(ItemID.BuilderPotion, 1, 2, 3),
                ItemDropRule.Common(ItemID.ShinePotion, 1, 1, 4),
                ItemDropRule.Common(ItemID.TrapsightPotion, 1, 3, 5)
            };
            itemLoot.Add(new OneFromRulesRule(3, potions));

            IItemDropRule[] others = new IItemDropRule[]
            {
                ItemDropRule.Common(ItemID.BonePickaxe, 1),
                ItemDropRule.Common(ItemID.Bomb, 1, 3, 7),
                ItemDropRule.Common(ItemID.SpelunkerGlowstick, 1, 20, 35),
                ItemDropRule.Common(ItemID.Rope, 1, 50, 100),
                ItemDropRule.Common(ItemID.Torch, 1, 9, 21)
            };
            itemLoot.Add(new FewFromRulesRule(2, 2, others));

            IItemDropRule[] chestLoot = new IItemDropRule[]
            {
                ItemDropRule.Common(ItemID.MagicMirror, 1),
                ItemDropRule.Common(ItemID.CordageGuide, 1),
                ItemDropRule.Common(ItemID.ShoeSpikes, 1),
                ItemDropRule.Common(ItemID.ClimbingClaws, 1),
                ItemDropRule.Common(ItemID.Radar, 1),
                ItemDropRule.Common(ItemID.WoodenBoomerang, 1),
                ItemDropRule.Common(ItemID.AngelStatue, 1),
                ItemDropRule.Common(ItemID.BandofRegeneration, 1),
                ItemDropRule.Common(ItemID.AncientChisel, 2)
            };
            itemLoot.Add(new FewFromRulesRule(2, 1, chestLoot));

            itemLoot.Add(ItemDropRule.Coins(3500, true));
        }
    }
}
