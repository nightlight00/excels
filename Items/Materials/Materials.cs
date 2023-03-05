using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Materials
{

    #region Bottle of Sunlight
    public class BottleOfSunlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottle of Sunlight");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 1;
            Item.maxStack = 999;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.useTime = 10;
            Item.createTile = ModContent.TileType<Tiles.Decorations.Furniture.Tabletop.BottleOfSunlightPlaced>();
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.placeStyle = 0;
            Item.buyPrice(0, 0, 5);
        }
    }
    #endregion

    #region Granite
    public class EnergizedGranite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.MaterialNames.GraniteEnergy"));
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.MaterialDescriptions.GraniteEnergy"));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.sellPrice(0, 0, 1);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.position, 1.14f * 0.3f, 2.36f * 0.3f, 2.55f * 0.3f);
        }
    }
    #endregion

    #region Fossil
    public class AncientFossil : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Fossil Rock");
            Tooltip.SetDefault("Prehistoric bones from underneath the grainy sands");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 6;
            Item.maxStack = 999;
            Item.sellPrice(0, 0, 8);
        }

    }
    #endregion

    #region Stellar Plating
    public class StellarPlating : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Taken from an interstellar machine");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.maxStack = 999;
            Item.sellPrice(0, 0, 5, 20);
        }
    }
    #endregion

    #region Wyvern Scale
    public class WyvernScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The scales of a mystical flying creature");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 5;
            Item.maxStack = 999;
            Item.sellPrice(0, 0, 7, 25);
        }

    }
    #endregion

    #region Shattered Heartbeat
        public class ShatteredHeartbeat : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It's still beating");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 2;
            Item.maxStack = 999;
            Item.sellPrice(0, 0, 8);
        }
        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ItemID.LifeCrystal)
                .AddIngredient(ItemID.Bone, 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    #endregion

    #region Blackhole Fragement
    class BlackholeFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blackhole Fragment");
            Tooltip.SetDefault("'The depth of the universe is being crushed by this fragment'");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.value = 20000;
            Item.width = 26;
            Item.height = 22;
            Item.rare = 9;
            Item.maxStack = 999;
            Item.sellPrice(0, 0, 20);
        }
    }
    #endregion
}
