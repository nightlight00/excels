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

namespace excels.Items.Armor.Fossil
{
	[AutoloadEquip(EquipType.Head)]
	internal class FossilMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.FossilHeadPiece"));
			Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RangedCritChance", 12));

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = 6;
			Item.defense = 9;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<FossilChest>() && legs.type == ModContent.ItemType<FossilGreaves>();
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Ranged) += 12;
		}

		public override void ArmorSetShadows(Player player)
		{
			if (player.GetModPlayer<excelPlayer>().FossilSet)
			{
				player.armorEffectDrawOutlinesForbidden = true;
			}
		}


		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.excels.ItemDescriptions.ArmorSetBonus.PrehistoricSet");
			player.GetDamage(DamageClass.Ranged) += 0.08f;
			player.GetModPlayer<excelPlayer>().FossilSet = true;

			if (player.velocity != Vector2.Zero)
			{
				if (Main.rand.NextBool())
				{
					Dust d = Dust.NewDustDirect(player.position, player.width, player.height, ModContent.DustType<Dusts.FossilBoneDust>());
					d.noGravity = true;
					d.scale = Main.rand.NextFloat(1.45f, 1.75f);
					d.velocity = -player.velocity / 6;
				}
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.Materials.AncientFossil>(), 16)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	internal class FossilChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.ArmorNames.FossilChestPiece"));
			Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RangedDamage", 10));

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = 6;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Ranged) += 0.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.Materials.AncientFossil>(), 18)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	internal class FossilGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.ArmorNames.FossilLegPiece"));
			Tooltip.SetDefault($"{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RangedCritChance", 12)}\n{Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.MovementSpeed", 10)}");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = 6;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Ranged) += 12;
			player.moveSpeed += 0.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.Materials.AncientFossil>(), 14)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
