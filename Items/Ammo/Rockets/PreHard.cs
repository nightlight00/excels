using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;

namespace excels.Items.Ammo.Rockets
{
	/*
    public class FlareRocket : ModItem
    {
		public override string Texture => "excels/Items/Weapons/Launcher1/FlareRocket";

        public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Combine the explosiveness of grenades and range of flares!");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			Item.damage = 23; 
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 12;
			Item.maxStack = 999;
			Item.consumable = true; // This marks the item as consumable, making it automatically be consumed when it's used as ammunition, or something else, if possible.
			Item.knockBack = 1.2f;
			Item.value = 10;
			Item.shoot = ModContent.ProjectileType<Weapons.Launcher1.FlareRocket>(); // The projectile that weapons fire when using this item as ammunition.
			Item.shootSpeed = 4f; // The speed of the projectile.
			Item.ammo = AmmoID.Rocket; // The ammo class this ammo belongs to.
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			Recipe recipe = Mod.CreateRecipe(ModContent.ItemType<FlareRocket>(), 75);
			recipe.AddIngredient(ItemID.Flare, 75);
			recipe.AddIngredient(ItemID.Grenade);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
	*/
}
