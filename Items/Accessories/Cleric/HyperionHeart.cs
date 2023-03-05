using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using excels.Items.Materials.Ores;

namespace excels.Items.Accessories.Cleric
{
    internal class HyperionHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyperion Heart");
            Tooltip.SetDefault("Increases max radiance by 15\nWhile under the effects of Anguished Soul, increases radiant damage by 10%\n'Strengthens in response to psychological trauma'");
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
            player.GetModPlayer<ClericClassPlayer>().radianceStatMax2 += 15;
            player.GetModPlayer<excelPlayer>().hyperionHeart = true;
            if (player.HasBuff(ModContent.BuffType<Buffs.ClericCld.AnguishedSoul>())) {
                player.GetModPlayer<ClericClassPlayer>().clericRadiantMult += 0.1f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HyperionCrystal>(), 18)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
