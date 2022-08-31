using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Accessories.Cleric.Damage
{
    internal class ClericEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("8% increased radiant and necrotic damage and healing gives an additional 1 health");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 28;
            Item.rare = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ClericClassPlayer>().clericNecroticMult += 0.08f;
            player.GetModPlayer<ClericClassPlayer>().clericRadiantMult += 0.08f;
            player.GetModPlayer<excelPlayer>().healBonus += 1;
        }
    }
}
