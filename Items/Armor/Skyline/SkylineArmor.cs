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
using Terraria.Localization;
using excels.Items.Materials.Ores;

namespace excels.Items.Armor.Skyline
{
    [AutoloadEquip(EquipType.Head)]
    public class SkylineHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyline Circlet");
			Tooltip.SetDefault("10% increased movement speed");

			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22; 
			Item.sellPrice(gold: 1); 
			Item.rare = 1;
			Item.defense = 3; 
		}

        public override void UpdateEquip(Player player)
        {
			//player.GetHairSettings(true, true, true, true, false);
			player.headcovered = false;
			player.moveSpeed += 0.1f;
        }

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<SkylineBody>() && legs.type == ModContent.ItemType<SkylineLegs>();
		}

		public override void UpdateArmorSet(Player player)
        {
			// taking damage temporarily surrounds player with ice shards
			player.setBonus = "Increased jump speed and max run speed \nCompletely negates fall damage \n'I believe I can fly!'";

			player.noFallDmg = true;
			player.maxRunSpeed *= 1.2f;
			player.jumpSpeedBoost = 2.5f;

			if (( ((player.velocity.X > 1) || player.velocity.X < -1) || ((player.velocity.Y > 1) || (player.velocity.Y < -1))) && (Main.rand.Next(7) <= 5))
            {
				Dust d = Dust.NewDustDirect(player.position, player.width, player.height, 16, -player.velocity.X / 12, -player.velocity.Y / 12);
				d.velocity *= -player.velocity / 6;
				d.scale = Main.rand.NextFloat(1.05f, 1.12f);
				d.noGravity = true;
            }
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SkylineBar>(), 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class SkylineBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyline Platemail");
			Tooltip.SetDefault("Increased running acceleration"); // "Increased max run speed");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26; 
			Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = 1;
			Item.defense = 4;
		}

        public override void UpdateEquip(Player player)
        {
			player.runAcceleration *= 1.5f;
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SkylineBar>(), 14)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class SkylineLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyline Boots");
			Tooltip.SetDefault("20% increased movement speed");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 18;
			Item.rare = 1;
			Item.defense = 2;
		}

        public override void UpdateEquip(Player player)
        {
			player.moveSpeed += 0.2f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SkylineBar>(), 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

}
