using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace excels.Items.Potions.Flasks
{
    internal class FrostfireFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flask of Frostfire");
            Tooltip.SetDefault("Melee and Whip attacks burns foes with frostfire");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FlaskofFire);
            Item.buffType = ModContent.BuffType<Buffs.Flasks.GlacialImbueBuff>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ModContent.ItemType<Materials.GlacialOre>(), 3)
                .AddTile(TileID.ImbuingStation)
                .Register();
        }
    }
}
