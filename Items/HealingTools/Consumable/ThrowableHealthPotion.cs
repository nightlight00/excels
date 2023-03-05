using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.HealingTools.Consumable
{
	public class ThrowableHealthPotion : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Throws a health potion that shatters on collision");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<ThrownHealthPotion>();
			Item.shootSpeed = 9;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.buyPrice(0, 0, 80);

			Item.mana = 5;
			healAmount = 30;
			healRate = 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}
	}

	public class ThrownHealthPotion : clericHealProj
	{
		public override string Texture => "excels/Items/HealingTools/Consumable/ThrowableHealthPotion";

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 22;
			Projectile.timeLeft = 999;
			Projectile.friendly = true;

			healPenetrate = 999;
			clericEvil = false;
			canDealDamage = false;
			canHealOwner = false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Break();
			return false;
		}

		public virtual void Break()
		{
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 32);
			for (var i = 0; i < 15; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 266);

			}
			for (var e = 0; e < 20; e++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 13);
				d.velocity = new Vector2(0, Main.rand.NextFloat(0.8f, 3)).RotatedByRandom(MathHelper.ToRadians(360));
			}
			SoundEngine.PlaySound(SoundID.Item107, Projectile.Center);
			Projectile.Kill();
		}

		public override void BuffEffects(Player target, Player healer)
		{
			Break();
		}

		public override void AI()
		{
			BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 32);
			Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
			Projectile.ai[0]++;

			if (Projectile.ai[0] > 16)
			{
				Projectile.velocity.Y += 0.24f;
				Projectile.velocity.X *= 0.97f;

				if (Projectile.velocity.X > 0)
				{
					Projectile.rotation += MathHelper.ToRadians((Projectile.ai[0] - 10) * 0.25f);
				}
				else
				{
					Projectile.rotation -= MathHelper.ToRadians((Projectile.ai[0] - 10) * 0.25f);
				}
			}
		}
	}
}
