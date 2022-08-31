using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.Radiant1
{
    internal class CoralScepter : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Sprays a plentitude of colourful bubbles");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 40;
			Item.useTime = 11;
			Item.useAnimation = 33;
			Item.reuseDelay = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 0;
			//Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CoralBubble>();
			Item.shootSpeed = 6.8f;
			Item.noMelee = true;

			clericRadianceCost = 3;
			healAmount = 1;
			Item.sellPrice(0, 0, 0, 80);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(13));
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			SoundEngine.PlaySound(SoundID.Item111, player.Center);
			return false;
        }

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Coral, 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class CoralBubble : clericHealProj
    {
        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
		}

        public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 18;
			Projectile.timeLeft = 90;
			Projectile.alpha = 255;

			canDealDamage = true;
			healPenetrate = 1;
			//buffConsumesPenetrate = true;
		}

        public override void AI()
        {
			if (Projectile.ai[0] == 0)
			{
				Projectile.frame = Main.rand.Next(4);
			}
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 20);
            if (++Projectile.ai[0] > 20)
			{
				Projectile.velocity.Y -= 0.13f;
			}
			Projectile.rotation = Projectile.velocity.X * 0.15f;
			Projectile.velocity.X *= 0.99f;
        }

        public override void Kill(int timeLeft)
        {
			Color c = new Color(118, 251, 255);
			switch (Projectile.frame)
            {
				case 1: c = new Color(255, 184, 220); break;
				case 2: c = new Color(112, 255, 167);  break;
				case 3: c = new Color(245, 171, 132);  break;
            }
			for (var i = 0; i < 10; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 303, newColor: c);
				d.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15)) * 0.5f;
				d.noGravity = true;
				d.alpha = 160;
			}
        }
    }
}
