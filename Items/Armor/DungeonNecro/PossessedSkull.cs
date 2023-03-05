using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace excels.Items.Armor.DungeonNecro
{
    [AutoloadEquip(EquipType.Head)]
    internal class PossessedSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.NecroticDamage", 16)+"\n"+ Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.BloodCostReduce", 12));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 32;
            Item.rare = 8;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.16f;
            player.GetModPlayer<excelPlayer>().bloodCostMult -= 0.12f;
        }
    }
}
