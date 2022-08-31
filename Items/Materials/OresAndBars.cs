using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Materials
{
    #region Skyline
    public class SkylineBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An incredibly light metal");
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
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<Tiles.OresBars.ExcelBarTiles>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.placeStyle = 0;
            Item.sellPrice(0, 0, 10);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SkylineOre>(), 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }

    public class SkylineOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skyline Pebble");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
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
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<Tiles.OresBars.SkylineOreTile>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 3, 20);
        }
    }
    #endregion

    #region Glacial
    public class GlacialBar : ModItem
    {
        public override void SetStaticDefaults()
        {
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
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<Tiles.OresBars.ExcelBarTiles>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.placeStyle = 1;
            Item.sellPrice(0, 0, 12);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GlacialOre>(), 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }

    public class GlacialOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
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
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<Tiles.OresBars.GlacialOreTile>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 3, 80);
        }
    }
    #endregion

    #region Granite
    public class EnergizedGranite : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A chunk of granite that emits a strange energy");
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

    #region Purity 
    public class MysticCrystal : ModItem
    {
        public override string Texture => "excels/Items/Materials/PurityBar";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purity Bar");
            Tooltip.SetDefault("A refined chunk of purity");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 1;
            Item.maxStack = 999;

            Item.createTile = ModContent.TileType<Tiles.OresBars.ExcelBarTiles>();
            Item.placeStyle = 2;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 0, 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PurifiedStone>(), 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }

    public class PurifiedStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A stone cleansed of all evil");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<Tiles.OresBars.PurityOre>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 0, 10);
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

    #region Hyperion
    public class HyperionCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 4;
            Item.maxStack = 999;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.createTile = ModContent.TileType<Tiles.OresBars.HyperionTile>();
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.placeStyle = 0;
            Item.sellPrice(0, 0, 11);
        }
    }
    #endregion
}
