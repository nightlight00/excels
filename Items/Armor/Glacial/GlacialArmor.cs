using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;
using excels.Items.Materials.Ores;

namespace excels.Items.Armor.Glacial
{
    [AutoloadEquip(EquipType.Head)]
    public class GlacialHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.GlacialHeadPiece"));
			Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RangedCritChance", 6));

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
			player.GetCritChance(DamageClass.Ranged) += 6;
        }

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<GlacialBody>() && legs.type == ModContent.ItemType<GlacialLegs>();
		}


		public override void UpdateArmorSet(Player player)
        {
			// taking damage temporarily surrounds player with ice shards
			player.setBonus = Language.GetTextValue("Mods.excels.ItemDescriptions.ArmorSetBonus.GlacialSet");
			player.GetCritChance(DamageClass.Ranged) += 6;
			player.GetModPlayer<excelPlayer>().GlacialSet = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<GlacialBar>(), 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class GlacialBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.GlacialChestPiece"));
			Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RangedDamage", 8));

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
			player.GetDamage(DamageClass.Ranged) += 0.06f;
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<GlacialBar>(), 16)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class GlacialLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.GlacialLegPiece"));
			Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.RangedCritChance", 7));

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 18;
			Item.sellPrice(gold: 1);
			Item.rare = 1;
			Item.defense = 3;
		}

        public override void UpdateEquip(Player player)
        {
			player.GetCritChance(DamageClass.Ranged) += 7;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<GlacialBar>(), 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class GlacialBlast : ModProjectile
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SnowBallFriendly}";
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 50;
			Projectile.timeLeft = 7;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 120);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 120);
		}

		public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
				for (var i = 0; i < 35; i++)
				{
					Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92);
					d.noGravity = true;
					d.velocity *= Main.rand.NextFloat(1.3f, 1.6f);
					d.scale *= 1.2f;
					if (i > 20)
                    {
						d.velocity *= 1.3f;
						d.scale *= 1.4f;
                    }
				}
                Projectile.ai[0]++;
            }
        }
    }


	public class GlacialShard : ModProjectile
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SnowBallFriendly}";
        public override void SetDefaults()
        {
			Projectile.width = Projectile.height = 14;
			Projectile.timeLeft = 220;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.penetrate = -1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		int Dist = 10;
		int DistInc = 3;
		int MaxDist = 100;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.Frostburn, 120);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
			target.AddBuff(BuffID.Frostburn, 120);
		}

        public override bool? CanDamage()
        {
			if (Dist < 50) {
				return false;
			}
			return true;
        }

        public override void AI()
        {
			Player player = Main.player[Projectile.owner];
			MaxDist = (int)Projectile.ai[1];
			DistInc = (int)Math.Floor(MaxDist / 33f);

			if (Projectile.timeLeft < 40) // 220 - 180{
			{
				Dist -= DistInc;
				if (Dist < 5)
                {
					Projectile.Kill();
                }
			}
            else if (Dist < MaxDist)
            {
				Dist += DistInc;
            }
			Projectile.ai[0] += 0.07f;
			Projectile.position.X = player.Center.X - (Projectile.width / 2) + (int)(Math.Cos(Projectile.ai[0]) * Dist);
			Projectile.position.Y = player.Center.Y - (Projectile.height / 2) + (int)(Math.Sin(Projectile.ai[0]) * Dist);

			int dstType = 76;
			if (Main.rand.NextBool(3)) dstType = 92;
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dstType);
			d.noGravity = true;
			d.velocity *= 0.1f;
			d.scale = Main.rand.NextFloat(0.9f, 1.3f);
		}
    }
}
