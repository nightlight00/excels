using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Accessories.Cleric.Necrotic
{
    [AutoloadEquip(EquipType.Neck)]
    internal class SkullPendant : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Every 60 health consumed using necrotic weapons refunds 10 health");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 28;
            Item.rare = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().skullPendant = true;
        }
    }

    [AutoloadEquip(EquipType.Neck)]
    internal class DeadMansPendant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dead Man's Pendant");
            Tooltip.SetDefault("Every 60 health consumed using necrotic weapons refunds 10 health and grants temporary invincibility");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 28;
            Item.rare = 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SkullPendant>())
                .AddIngredient(ItemID.CrossNecklace)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().skullPendant = true;
            player.GetModPlayer<excelPlayer>().skullPendant2 = true;
        }
    }
}
