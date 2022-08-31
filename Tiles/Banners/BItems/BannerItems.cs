using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;

namespace excels.Tiles.Banners.BItems
{
    internal class BannerRexxie : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rexxie Skull Banner");
			Tooltip.SetDefault("Nearby players get a bonus against: Rexxie Skull");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<ExcelBanners>();
			Item.placeStyle = 0;       
		}
	}

	internal class BannerFossiliraptor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fossiliraptor Banner");
			Tooltip.SetDefault("Nearby players get a bonus against: Fossiliraptor");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<ExcelBanners>();
			Item.placeStyle = 1;
		}
	}

	internal class BannerMeteorGolem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Golem Banner");
			Tooltip.SetDefault("Nearby players get a bonus against: Meteor Golem");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<ExcelBanners>();
			Item.placeStyle = 2;
		}
	}

	internal class BannerMeteorSpirit: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Spirit Banner");
			Tooltip.SetDefault("Nearby players get a bonus against: Meteor Spirit");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<ExcelBanners>();
			Item.placeStyle = 3;
		}
	}

	internal class BannerMeteorSlime : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flaming Sludge Banner");
			Tooltip.SetDefault("Nearby players get a bonus against: Flaming Sludge");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<ExcelBanners>();
			Item.placeStyle = 4;
		}
	}

	internal class BannerSkylineSentinal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyline Sentinal Banner");
			Tooltip.SetDefault("Nearby players get a bonus against: Skyline Sentinal");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<ExcelBanners>();
			Item.placeStyle = 5;
		}
	}
	internal class BannerDungeonMimic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Mimic Banner");
			Tooltip.SetDefault("Nearby players get a bonus against: Dungeon Mimic");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<ExcelBanners>();
			Item.placeStyle = 6;
		}
	}
}
