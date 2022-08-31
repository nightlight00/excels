using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace excels.Items.Accessories.Summon
{
    internal class FriendshipBracelet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Friendship Necklace");
            Tooltip.SetDefault("'Shows the bond between you and your summons!' \nSlightly increases life regeneration for each active summon");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().FriendshipRegen = true;
        //    player.GetModPlayer<excelPlayer>().SpiritAttackSpeed += 0.12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ShatteredHeartbeat>(), 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
