using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.Blood
{
    internal class BloodyZombieHand : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 30;
			Item.useTime = Item.useAnimation = 17;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2.3f;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.NPCDeath21;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<BloodDrop>();
			Item.shootSpeed = 8;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 80);

			clericEvil = true;
			clericBloodCost = 5;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (var i = 0; i < 3; i++)
			{
				Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(18));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale;
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}

	public class BloodDrop : clericProj
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.WaterBolt;

		public override void SafeSetDefaults()
		{
			Projectile.height = Projectile.width = 12;
			Projectile.friendly = true;
			Projectile.timeLeft = 140;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (var i = 0; i < 15; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5, ((-Projectile.velocity.X * 1.2f) / 15) * i, ((-Projectile.velocity.Y * 1.2f) / 15) * i);
				d.scale = Main.rand.NextFloat(1.25f, 1.8f);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (var i = 0; i < 15; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5, ((-Projectile.velocity.X * 1.2f) / 15) * i, ((-Projectile.velocity.Y * 1.2f) / 15) * i);
				d.scale = Main.rand.NextFloat(1.25f, 1.8f);
			}
			return true;
		}

		public override void AI()
		{
			if (Projectile.timeLeft < 120)
			{
				Projectile.velocity.Y += 0.18f;
			}
			for (var i = 0; i < 2; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5, -Projectile.velocity.X * 0.75f, -Projectile.velocity.Y * 0.75f);
				d.scale = Main.rand.NextFloat(0.9f, 1.2f);
				d.velocity *= 0.3f;
			}
		}
	}
}
