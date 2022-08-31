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

namespace excels.Items.WeaponHeal.Holyiest
{
    #region Grand Cross
    internal class GrandCross : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Rapidly conjures radiant healing bolts");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = 7;
			Item.useAnimation = 14;
			Item.reuseDelay = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Generic.HealingBolt>();
			Item.shootSpeed = 6.3f;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 80);

			Item.mana = 10;
			healAmount = 3;
			healRate = 1;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Generic.WoodenCross>())
				.AddIngredient(ModContent.ItemType<Materials.MysticCrystal>(), 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	#endregion

	#region Prophecy
	internal class Prophecy : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Generates a wall of healing energy \nHealing allies boosts their life regeneration");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 30;
			Item.reuseDelay = 9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ProphecyBolt>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.sellPrice(0, 1, 35);

			Item.mana = 25;
			healAmount = 1;
			healRate = 0.5f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (var i = 0; i < 5; i++)
            {
				Vector2 pos = position + new Vector2(8, (i * 16) - 32).RotatedBy(((Main.MouseWorld - position).SafeNormalize(Vector2.Zero).ToRotation()));
				CreateHealProjectile(player, source, pos, velocity, type, damage, knockback);
			}
			return false;
		}
	}

	public class ProphecyBolt : clericHealProj
    {
		public override string Texture => "excels/Items/WeaponHeal/Generic/HealingBolt";
        public override void SafeSetDefaults()
        {
			Projectile.width = Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 100;
			Projectile.alpha = 255;

			clericEvil = false;
			healPenetrate = 1;
			healUsesBuffs = true;

			canHealOwner = false;
        }

        public override void BuffEffects(Player target, Player healer)
        {
			target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.EnergyUnleash>(), GetBuffTime(healer, 2));
        }

        public override void AI()
        {
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 10, false);

			for (var i = 0; i < 2; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 220);
				d.velocity = -Projectile.velocity * 0.5f;
				d.scale = 0.9f;
				d.noGravity = true;
				if (i == 1)
                {
					d.scale = 1.2f;
                }
			}
        }

        public override void Kill(int timeLeft)
        {
			for (var i = 0; i < 20; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 220);
				d.scale = Main.rand.NextFloat(0.9f, 1.15f);
				if (Main.rand.NextBool(3))
				{
					d.noGravity = true;
					d.scale *= 1.1f; 
				}
				if (!Main.rand.NextBool(4))
				{
					d.velocity += (Projectile.velocity * Main.rand.NextFloat(0.3f, 0.8f));
				}
			}
        }
    }
	#endregion
}
