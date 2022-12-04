using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

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
			Item.useTime = Item.useAnimation = 10;
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
        public override void SaferSafeSetDefaults()
        {
			Projectile.width = Projectile.height = 82;
			BuffType = BuffID.Poisoned;
			BuffTime = 480;
        }

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 50; i++)
			{
				int dType = 89;
				if (Main.rand.NextBool(3))
					dType = 39;
				Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24, 24), dType);
				d.scale = Main.rand.NextFloat(1.2f, 1.6f);
				d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f) * 17;
				d.noGravity = true;
			}
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
			Item.useTime = Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<FungalSpellThrow>();
			Item.shootSpeed = 7.5f;
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
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 50; i++)
			{
				int type = 176;
				if (Main.rand.NextBool(3)) 
					type = 113;
				Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24, 24), type);
				d.scale = Main.rand.NextFloat(1.2f, 1.6f);
				d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f) * 20;
				d.noGravity = true;
			}
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
			Item.useTime = Item.useAnimation = 10;
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
			Projectile.timeLeft = 150;
			BuffType = BuffID.OnFire;
			BuffTime = 540;
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 50; i++)
			{
				int dType = 6;
				if (Main.rand.NextBool(3))
					dType = 174;
				Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24, 24), dType);
				d.scale = Main.rand.NextFloat(1.2f, 1.6f);
				d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f) * 20;
				d.noGravity = true;
			}
		}
	}
	#endregion

	#region Ice Spell
	internal class SnowSpellPot : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snow Spell Potion");
			Tooltip.SetDefault("Creates a frozen spell aura on destruction \nIgnores defense completely and makes enemies more susceptible to other cleric attacks");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 9;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 22;
			Item.useTime = Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<SnowSpellThrow>();
			Item.shootSpeed = 8.25f;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			//clericEvil = true;
			clericBloodCost = 5;
			Item.mana = 15;
		}
	}

	public class SnowSpellThrow : SpellThrowBase
	{
		public override void SaferSafeSetDefaults()
		{
			SpellAura = ModContent.ProjectileType<SnowSpell>();
			SpeedFallOff = 0.98f;
			clericEvil = false;
		}
	}

	public class SnowSpell : SpellAuraBase
	{
		//	public override string GlowTexture => "TEST1/Items/SpellPotions/ToxicSpell";

		public override void SaferSafeSetDefaults()
		{
			BuffType = BuffID.Frostburn;
			BuffTime = 540;
			clericEvil = false;
		}  

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 50; i++)
            {
				int dType = 76;
				if (Main.rand.NextBool())
					dType = 92;
				Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24, 24), dType);
				d.scale = Main.rand.NextFloat(1.2f, 1.6f);
				d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f) * 20;
				d.noGravity = true;
            }
        }
    }
	#endregion

	#region Infernal Spell
	internal class InfernalSpellPot : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Fire Spell Potion");
			Tooltip.SetDefault("Creates a cursed spell aura on destruction \nIgnores defense completely and makes enemies more susceptible to other cleric attacks");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 24;
			Item.useTime = Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<InfernalSpellThrow>();
			Item.shootSpeed = 9.5f;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			clericEvil = true;
			clericBloodCost = 8;
			Item.mana = 20;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Bottle)
				.AddIngredient(ItemID.CursedFlame, 8)
				.AddIngredient(ItemID.SoulofNight, 5)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}

	public class InfernalSpellThrow : SpellThrowBase
	{
		public override void SaferSafeSetDefaults()
		{
			SpellAura = ModContent.ProjectileType<InfernalSpell>();
			SpeedFallOff = 0.987f;
		}
	}

	public class InfernalSpell : SpellAuraBase
	{
		public override void SaferSafeSetDefaults()
		{
			Projectile.width = Projectile.height = 126;
			Projectile.timeLeft = 240;
			BuffType = BuffID.CursedInferno;
			BuffTime = 540;
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 75; i++)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24, 24), 75);
				d.scale = Main.rand.NextFloat(1.2f, 1.6f)*2;
				d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f) * 28;
				d.noGravity = true;
			}
		}
	}
	#endregion

	#region God Blood Spell
	internal class GodBloodSpellPot : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("God Blood Spell Potion");
			Tooltip.SetDefault("Creates an ichor spell aura on destruction \nIgnores defense completely and makes enemies more susceptible to other cleric attacks");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 24;
			Item.useTime = Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<GodBloodSpellThrow>();
			Item.shootSpeed = 9.25f;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			clericEvil = true;
			clericBloodCost = 8;
			Item.mana = 20;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Bottle)
				.AddIngredient(ItemID.Ichor, 8)
				.AddIngredient(ItemID.SoulofNight, 5)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}

	public class GodBloodSpellThrow : SpellThrowBase
	{
		public override void SaferSafeSetDefaults()
		{
			SpellAura = ModContent.ProjectileType<GodBloodSpell>();
			SpeedFallOff = 0.989f;
		}
	}

	public class GodBloodSpell : SpellAuraBase
	{
		public override void SaferSafeSetDefaults()
		{
			Projectile.width = Projectile.height = 126;
			Projectile.timeLeft = 240;
			BuffType = BuffID.Ichor;
			BuffTime = 540;
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 75; i++)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24, 24), 170);
				d.scale = Main.rand.NextFloat(1.2f, 1.6f) * 1.4f;
				d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f) * 28;
				d.noGravity = true;
			}
		}
	}
	#endregion

	#region Blessed Spell
	internal class BlessedSpellPot : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blessed Spell Potion");
			Tooltip.SetDefault("Creates a blessed spell aura on destruction \nIgnores defense completely and makes enemies more susceptible to other cleric attacks\nStruck foes generate additional Minor Blessings upon death");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 29;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 24;
			Item.useTime = Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<BlessedSpellThrow>();
			Item.shootSpeed = 9f;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			//clericEvil = true;
			clericBloodCost = 8;
			Item.mana = 20;
		}
	}

	public class BlessedSpellThrow : SpellThrowBase
	{
		public override void SaferSafeSetDefaults()
		{
			SpellAura = ModContent.ProjectileType<BlessedSpell>();
			SpeedFallOff = 0.975f;
			clericEvil = false;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.GetGlobalNPC<excelNPC>().BlessedSpell = 300;
		}
    }

	public class BlessedSpell : SpellAuraBase
	{
		public override void SaferSafeSetDefaults()
		{
			clericEvil = false;
			Projectile.width = Projectile.height = 102;
			Projectile.timeLeft = 210;
			BuffType = -1;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.GetGlobalNPC<excelNPC>().BlessedSpell = 900;
        }

        public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 65; i++)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24, 24), 204);
				d.scale = Main.rand.NextFloat(1.2f, 1.6f) * 1.2f;
				d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f) * 28;
				d.noGravity = true;
			}
		}
	}
	#endregion
}


