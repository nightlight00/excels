﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.Collections.Generic;

namespace excels.Items.HealingTools.Generic.HallowBar
{
	#region Asclepius
	public class Asclepius : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Asclepius's Rod");
			Tooltip.SetDefault("Summons a rejuvinating snake head");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 5;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<AsclepiusHead>();
			Item.shootSpeed = 3f;
			Item.noMelee = true;
			Item.sellPrice(0, 3);

			Item.mana = 15;
			healAmount = 16;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HallowedBar, 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class AsclepiusHead : clericHealProj
    {
		public override string Texture => "excels/Items/HealingTools/Generic/HallowBar/CaduceusHeads";

        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 2;
		}

        public override void SafeSetDefaults()
        {
			Projectile.extraUpdates = 3;
			Projectile.ignoreWater = true;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.alpha = 255;
			Projectile.timeLeft = 200;
			healPenetrate = 2;
        }

        public override void AI()
        {
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 25, false);

			Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

			Projectile.rotation = Projectile.velocity.ToRotation();
			// Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation += MathHelper.Pi;
				// For vertical sprites use MathHelper.PiOver2
			}

			Projectile.frame = 1;

			Projectile.alpha -= 8;
			if (Projectile.alpha < 50)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center - (Projectile.velocity * 5), 180);
				d.noGravity = true;
				d.velocity = new Vector2(1.2f).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
				d.scale = 2;
			}
			Projectile.ai[0] += 12;

			const int HalfSpriteWidth = 24 / 2;
			const int HalfSpriteHeight = 24 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
		}

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 30; i++)
            {
				Dust d = Dust.NewDustPerfect(Projectile.Center, 180);
				d.noGravity = true;
				d.velocity = new Vector2(1 + (0.04f * i)).RotatedBy(MathHelper.ToRadians((360 / 15) * i));
				d.scale = 1.4f + (0.04f * i);
			}
        }
    }
	#endregion

	#region Caduceus
	public class Caduceus : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons twin snake heads");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 8;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CaduceusHeads>();
			Item.shootSpeed = 3.3f;
			Item.noMelee = true;
			Item.sellPrice(0, 5);

			Item.mana = 18;
			healAmount = 13;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback, -1);
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback, 1);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Asclepius>())
				.AddIngredient(ItemID.ChlorophyteBar, 18)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class CaduceusHeads : clericHealProj
	{

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SafeSetDefaults()
		{
			Projectile.extraUpdates = 3;
			Projectile.ignoreWater = true;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.alpha = 255;
			Projectile.timeLeft = 260;
			healPenetrate = 3;
		}

		bool Start = false;
		int DustType = 178;
		float StartRot = 0;
		Vector2 Movement;

		public override void AI()
		{
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 30, false);

			Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

			Projectile.rotation = Projectile.velocity.ToRotation();
			// Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation += MathHelper.Pi;
				// For vertical sprites use MathHelper.PiOver2
			}

			if (!Start)
            {
				Movement = Projectile.Center;
				StartRot = Projectile.rotation;
				if (Projectile.ai[0] == 1)
				{
					Projectile.frame = 1;
					DustType = 180;
				}
				Start = false;
			}

			Projectile.alpha -= 7;
			if (Projectile.alpha < 50)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center - (Projectile.velocity * 5), DustType);
				d.noGravity = true;
				d.velocity = -Projectile.velocity;
				d.scale = 2;
			}

			Projectile.Center = Movement + new Vector2(0.5f * Projectile.ai[0]).RotatedBy(MathHelper.ToRadians(StartRot - Projectile.ai[1]));

			Projectile.ai[1] += 7;
			Movement += Projectile.velocity * 0.4f;

			const int HalfSpriteWidth = 24 / 2;
			const int HalfSpriteHeight = 24 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 30; i++)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center, DustType);
				d.noGravity = true;
				d.velocity = new Vector2(1 + (0.04f * i)).RotatedBy(MathHelper.ToRadians((360 / 15) * i));
				d.scale = 1.4f + (0.04f * i);
			}
		}
	}
	#endregion
}
