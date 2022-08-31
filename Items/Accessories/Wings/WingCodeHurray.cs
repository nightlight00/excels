using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;


namespace excels.Items.Accessories.Wings
{

	[AutoloadEquip(EquipType.Wings)]
	internal class FossilisedWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows flight and slow fall");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// These wings use the same values as the solar wings
			// Fly time: 180 ticks = 3 seconds
			// Fly speed: 9
			// Acceleration multiplier: 2.5
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(120, 4f, 1.75f);
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.7f; // Falling glide speed
			ascentWhenRising = 0.1f; // Rising speed
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2f;
			constantAscend = 0.1f;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.Materials.AncientFossil>(), 10)
				.AddIngredient(ItemID.SoulofFlight, 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
