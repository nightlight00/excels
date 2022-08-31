using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;


namespace excels.Items.Misc.Summons
{
	internal class InfectionRadar : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Used to track down sources of the fungal infection");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 20;
			Item.value = 100;
			Item.rare = ItemRarityID.Yellow;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			// If you decide to use the below UseItem code, you have to include !NPC.AnyNPCs(id), as this is also the check the server does when receiving MessageID.SpawnBoss.
			// If you want more constraints for the summon item, combine them as boolean expressions:
			//    return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<MinionBossBody>()); would mean "not daytime and no MinionBossBody currently alive"
			return player.ZoneGlowshroom && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Chasm.ChasmHead>());
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				// If the player using the item is the client
				// (explicitely excluded serverside here)
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = ModContent.NPCType<NPCs.Chasm.ChasmHead>();

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
					NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.ShroomiteBar, 12)
				.AddIngredient(ItemID.SoulofMight, 4)
				.AddTile(TileID.Autohammer)
				.Register();
		}
	}

	internal class PlanetaryTrackingDevice : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Broadcasts your planet's coordinates to the galaxy");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 20;
			Item.value = 100;
			Item.rare = ModContent.RarityType<StellarRarity>();
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			// If you decide to use the below UseItem code, you have to include !NPC.AnyNPCs(id), as this is also the check the server does when receiving MessageID.SpawnBoss.
			// If you want more constraints for the summon item, combine them as boolean expressions:
			//    return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<MinionBossBody>()); would mean "not daytime and no MinionBossBody currently alive"
			return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Stellar.StellarStarship>());
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				// If the player using the item is the client
				// (explicitely excluded serverside here)
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = ModContent.NPCType<NPCs.Stellar.StellarStarship>();

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
					NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.MeteoriteBar, 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	internal class ReflectiveIceShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("It feels like it's watching you");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 20;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			// If you decide to use the below UseItem code, you have to include !NPC.AnyNPCs(id), as this is also the check the server does when receiving MessageID.SpawnBoss.
			// If you want more constraints for the summon item, combine them as boolean expressions:
			//    return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<MinionBossBody>()); would mean "not daytime and no MinionBossBody currently alive"
			return player.ZoneSnow && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Glacial.GlacialQueen>());
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				// If the player using the item is the client
				// (explicitely excluded serverside here)
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = ModContent.NPCType<NPCs.Glacial.GlacialQueen>();

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
					NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.IceBlock, 40)
				.AddIngredient(ItemID.TissueSample, 10)
				.AddTile(TileID.DemonAltar)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.IceBlock, 40)
				.AddIngredient(ItemID.ShadowScale, 10)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}
