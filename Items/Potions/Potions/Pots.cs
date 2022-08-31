using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace excels.Items.Potions.Potions
{
    internal class SweetPotion : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assuaging Potion");
			Tooltip.SetDefault("Increases healing potency"); // by 1");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

			// Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
			ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
				new Color(255, 231, 51),
				new Color(255, 143, 0),
				new Color(255, 102, 0)
			};
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = 30;
			Item.consumable = true;
			Item.rare = 1;
			Item.value = Item.buyPrice(silver: 10);
			Item.buffType = ModContent.BuffType<Buffs.Potions.SweetBuff>(); // Specify an existing buff to be applied when used.
			Item.buffTime = 10 * 60 * 60; // The amount of time the buff declared in Item.buffType will last in ticks. Set to 3 minutes, as 60 ticks = 1 second.
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Bottle)
				.AddIngredient(ItemID.Honeyfin)
				.AddIngredient(ModContent.ItemType<Misc.Herbs.Gladiolus>())
				.AddIngredient(ItemID.Daybloom)
				.AddIngredient(ItemID.Moonglow)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}
}
