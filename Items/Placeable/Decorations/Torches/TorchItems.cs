﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using excels.Tiles.Decorations.Torches;

namespace excels.Items.Placeable.Decorations.Torches
{
	#region Granite
	internal class GranTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Energy Torch");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.flame = true;
			Item.noWet = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.holdStyle = ItemHoldStyleID.HoldFront;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<GranTorchT>();
			Item.width = 10;
			Item.height = 12;
			Item.value = 50;
		}

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
		{ // Overrides the default sorting method of this Item.
			itemGroup = ContentSamples.CreativeHelper.ItemGroup.Torches; // Vanilla usually matches sorting methods with the right type of item, but sometimes, like with torches, it doesn't. Make sure to set whichever items manually if need be.
		}

		public override void HoldItem(Player player)
		{
			// Randomly spawn sparkles when the torch is held. Twice bigger chance to spawn them when swinging the torch.
			if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0)
			{
				Dust d = Dust.NewDustDirect(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<Dusts.EnergyDust>());
				d.noGravity = true;
			}

			// Create a white (1.0, 1.0, 1.0) light at the torch's approximate position, when the item is held.
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);

			Lighting.AddLight(position, 1.14f * 0.5f, 2.36f * 0.5f, 2.55f * 0.5f);
		}

		public override void PostUpdate()
		{
			// Create a white (1.0, 1.0, 1.0) light when the item is in world, and isn't underwater.
			if (!Item.wet)
			{
				Lighting.AddLight(Item.Center, 1.14f * 0.4f, 2.36f * 0.4f, 2.55f * 0.4f);
			}
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			dryTorch = true; // This makes our item eligible for being selected with smart select at a short distance when not underwater.
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe(5)
				.AddIngredient(ItemID.Torch, 5)
				.AddIngredient(ModContent.ItemType<Materials.EnergizedGranite>())
				.Register();
		}
	}
	#endregion

	#region Stellar
	internal class StellarTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Bulb");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.flame = true;
			Item.noWet = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.holdStyle = ItemHoldStyleID.HoldFront;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<StellarBulb>();
			Item.width = 18;
			Item.height = 18;
			Item.value = 50;
			Item.rare = ModContent.RarityType<StellarRarity>();
		}

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
		{ // Overrides the default sorting method of this Item.
			itemGroup = ContentSamples.CreativeHelper.ItemGroup.Torches; // Vanilla usually matches sorting methods with the right type of item, but sometimes, like with torches, it doesn't. Make sure to set whichever items manually if need be.
		}

		public override void HoldItem(Player player)
		{
			// Create a white (1.0, 1.0, 1.0) light at the torch's approximate position, when the item is held.
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);

			Lighting.AddLight(position, 2.55f * 0.5f, 1.58f * 0.5f, 0.46f * 0.5f);
		}

		public override void PostUpdate()
		{
			// Create a white (1.0, 1.0, 1.0) light when the item is in world, and isn't underwater.
			if (!Item.wet)
			{
				Lighting.AddLight(Item.Center, 2.55f * 0.5f, 1.58f * 0.5f, 0.46f * 0.5f);
			}
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			dryTorch = true; // This makes our item eligible for being selected with smart select at a short distance when not underwater.
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.Torch, 20)
				.AddIngredient(ModContent.ItemType<Materials.StellarPlating>())
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.Torch)
				.AddIngredient(ModContent.ItemType<Tiles.StellarBrick>())
				.Register();
		}
	}
	#endregion
}
