using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.ThrowPotions
{

    #region Toxic Spell
    internal class ToxicSpellPot : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Spell Potion");
			Tooltip.SetDefault("Creates a poisonous spell aura on destruction \nIgnores defense completely and makes enemies more susceptible to other cleric attacks");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 22;
			Item.useTime = Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<ToxicSpellThrow>();
			Item.shootSpeed = 8;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			clericEvil = true;
			clericBloodCost = 5;
			Item.mana = 15;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Bottle)
				.AddIngredient(ItemID.Stinger, 1)
				.AddIngredient(ItemID.JungleSpores, 5)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}

	public class ToxicSpellThrow : SpellThrowBase
	{
        public override void SaferSafeSetDefaults()
        {
			SpellAura = ModContent.ProjectileType<ToxicSpell>();
		}
    }

	public class ToxicSpell : SpellAuraBase
    {
	//	public override string GlowTexture => "TEST1/Items/SpellPotions/ToxicSpell";

        public override void SaferSafeSetDefaults()
        {
			BuffType = BuffID.Poisoned;
			BuffTime = 480;
			DeathDustType = 39;
        }
    }
	#endregion

	#region Fungal Spell
	internal class FungalSpellPot : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fungal Spell Potion");
			Tooltip.SetDefault("Creates a infectious spell aura on destruction \nIgnores defense completely and makes enemies more susceptible to other cleric attacks");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 4;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 22;
			Item.useTime = Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<FungalSpellThrow>();
			Item.shootSpeed = 8;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			clericEvil = true;
			clericBloodCost = 5;
			Item.mana = 15;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Bottle)
				.AddIngredient(ItemID.GlowingMushroom, 20)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}

	public class FungalSpellThrow : SpellThrowBase
	{
		public override void SaferSafeSetDefaults()
		{
			SpellAura = ModContent.ProjectileType<FungalSpell>();
		}
	}

	public class FungalSpell : SpellAuraBase
	{
		//	public override string GlowTexture => "TEST1/Items/SpellPotions/ToxicSpell";

		public override void SaferSafeSetDefaults()
		{
			BuffType = ModContent.BuffType<Buffs.Debuffs.Mycosis>();
			BuffTime = 480;
			DeathDustType = 176;
		}
	}
	#endregion

	#region Fiery Spell
	internal class FierySpellPot : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Spell Potion");
			Tooltip.SetDefault("Creates a flaming spell aura on destruction \nIgnores defense completely and makes enemies more susceptible to other cleric attacks");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 22;
			Item.useTime = Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<FierySpellThrow>();
			Item.shootSpeed = 8;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			clericEvil = true;
			clericBloodCost = 5;
			Item.mana = 15;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Bottle)
				.AddIngredient(ItemID.Obsidian, 8)
				.AddIngredient(ItemID.Hellstone, 5)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}

	public class FierySpellThrow : SpellThrowBase
	{
		public override void SaferSafeSetDefaults()
		{
			SpellAura = ModContent.ProjectileType<FierySpell>();
		}
	}

	public class FierySpell : SpellAuraBase
	{
		//	public override string GlowTexture => "TEST1/Items/SpellPotions/ToxicSpell";

		public override void SaferSafeSetDefaults()
		{
			BuffType = BuffID.OnFire;
			BuffTime = 540;
			DeathDustType = 6;
		}
	}
	#endregion
}


