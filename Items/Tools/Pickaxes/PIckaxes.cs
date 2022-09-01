using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace excels.Items.Tools.Pickaxes
{
    internal class DinoPick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fossil Battler Pickaxe");
            Tooltip.SetDefault("'If only you could revive the fossils, then it'd be a proper fight!'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldPickaxe);

            Item.DamageType = DamageClass.Melee;
            Item.damage = 17;
            Item.useTime = Item.useAnimation = 8;
            Item.pick = 100;
            Item.width = Item.height = 42;
            Item.rare = 2;
            Item.value = 30000;
            Item.knockBack = 3.4f;
        }
    }
}
