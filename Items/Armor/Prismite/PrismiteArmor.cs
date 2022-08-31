using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Armor.Prismite
{
    [AutoloadEquip(EquipType.Head)]
    internal class PrismiteMask : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystalline Mask");
			Tooltip.SetDefault("5% increased radiant damage\nHealing gives an extra 1 health");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 20;
			Item.sellPrice(gold: 1);
			Item.rare = 5;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			var modPlayer = ClericClassPlayer.ModPlayer(player);
			modPlayer.clericRadiantMult += 0.05f;
			player.GetModPlayer<excelPlayer>().healBonus++;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<PrismiteBody>() && legs.type == ModContent.ItemType<PrismiteBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			var modPlayer = ClericClassPlayer.ModPlayer(player);
			player.setBonus = "Healing allies has a chance to critically heal them\nCritical heals restore 50% more health and temporarily increase ally's maximum health"; // "Double tap down to activate / deactivate 'Heartbreak'";
			player.GetModPlayer<excelPlayer>().PrismiteSet = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PearlwoodGreaves)
				.AddIngredient(ItemID.CrystalShard, 8)
				.AddIngredient(ItemID.SoulofLight, 7)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}


	[AutoloadEquip(EquipType.Body)]
	internal class PrismiteBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystalline Chestplate");
			Tooltip.SetDefault("6% increased cleric critical strike chance\nHealing gives an extra 2 health");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 20;
			Item.sellPrice(gold: 1);
			Item.rare = 5;
			Item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			var modPlayer = ClericClassPlayer.ModPlayer(player);
			modPlayer.clericCrit += 6;
			player.GetModPlayer<excelPlayer>().healBonus+= 2;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PearlwoodBreastplate)
				.AddIngredient(ItemID.CrystalShard, 10)
				.AddIngredient(ItemID.SoulofLight, 8)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}


	[AutoloadEquip(EquipType.Legs)]
	internal class PrismiteBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystalline Boots");
			Tooltip.SetDefault("5% increased radiant damage");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 20;
			Item.sellPrice(gold: 1);
			Item.rare = 5;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			var modPlayer = ClericClassPlayer.ModPlayer(player);
			modPlayer.clericRadiantMult += 0.05f;
			//player.GetModPlayer<excelPlayer>().healBonus++;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PearlwoodGreaves)
				.AddIngredient(ItemID.CrystalShard, 7)
				.AddIngredient(ItemID.SoulofLight, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
