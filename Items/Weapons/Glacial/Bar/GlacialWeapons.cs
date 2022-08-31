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

namespace excels.Items.Weapons.Glacial.Bar
{
    #region Froslance
    public class Froslance : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

        public override void SetDefaults()
        {
            Item.width = Item.height = 46;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.damage = 26;
            Item.knockBack = 4.7f;
            Item.rare = 1;
            Item.UseSound = SoundID.Item71;
			Item.shoot = ModContent.ProjectileType<FroslanceProj>();
			Item.shootSpeed = 2.4f;
			Item.sellPrice(0, 0, 50);

		}
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool? UseItem(Player player)
        {
            // Because we're skipping sound playback on use animation start, we have to play it ourselves whenever the item is actually used.
            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(SoundID.Item71, player.Center);
            }

            return null;
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.Materials.GlacialBar>(), 8)
				.AddTile(TileID.Anvils)
				.Register();
		}

	}

	public class FroslanceProj : ModProjectile
    {
		// Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 40f;
		protected virtual float HoldoutRangeMax => 120f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Froslance");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.Frostburn, 240);
        }

        public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
			int duration = player.itemAnimationMax; // Define the duration the projectile will exist in frames

			player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

			// Reset projectile time left if necessary
			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}

			Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

			float halfDuration = duration * 0.5f;
			float progress;

			// Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation.
			if (Projectile.timeLeft < halfDuration)
			{
				progress = Projectile.timeLeft / halfDuration;
			}
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			// Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

			// Apply proper rotation to the sprite.
			if (Projectile.spriteDirection == -1)
			{
				// If sprite is facing left, rotate 45 degrees
				Projectile.rotation += MathHelper.ToRadians(45f);
			}
			else
			{
				// If sprite is facing right, rotate 135 degrees
				Projectile.rotation += MathHelper.ToRadians(135f);
			}

			// Avoid spawning dusts on dedicated servers
			if (!Main.dedServ)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
				d.scale = Main.rand.NextFloat(0.8f, 1f);
				d.noGravity = true;
				if (Main.rand.NextBool(3))
                {
					Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92, Projectile.velocity.X * 1.7f, Projectile.velocity.Y * 1.7f);
					d2.scale = Main.rand.NextFloat(1.2f, 1.4f);
					d2.noGravity = true;
				}
			}

			return false; // Don't execute vanilla AI.
		}
    }
    #endregion

    #region Snowdance
	public class Snowdance : ModItem
    {
        public override void SetStaticDefaults()
        {
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

        public override void SetDefaults()
        {
			Item.width = Item.height = 46;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 7;
			Item.useAnimation = 28;
			Item.mana = 6;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<SnowdanceProj>();
			Item.shootSpeed = 7.4f;
			Item.damage = 14;
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.noMelee = true;
			Item.knockBack = 4.2f;
			Item.sellPrice(0, 0, 45);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(12));
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.Materials.GlacialBar>(), 8)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class SnowdanceProj : ModProjectile
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SnowBallFriendly}";

        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Snowdance");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void SetDefaults()
        {
			Projectile.height = Projectile.width = 14;
			Projectile.scale = 0.6f;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 100;
			Projectile.friendly = true;
			Projectile.alpha = 255;
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

		public override void AI()
        {
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-3, 4)));
			Projectile.alpha -= 11;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 10; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.width, 76);
				d.velocity *= 0.9f;
				d.noGravity = true;
				d.scale = 1.2f;
            }
        }
    }
    #endregion

    #region Gun
	public class GlacialGun : ModItem
    {
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Coats regular bullets with frostfire");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
        public override void SetDefaults()
        {
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 21;
			Item.useTime = Item.useAnimation = 14;
			Item.useAmmo = AmmoID.Bullet;
			Item.shoot = 10;
			Item.shootSpeed = 6;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.rare = 1;
			Item.knockBack = 2.9f;
			Item.UseSound = SoundID.Item11;
			Item.sellPrice(0, 0, 45);
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4f, 0f);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.Materials.GlacialBar>(), 8)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet)
			{ 
				type = ModContent.ProjectileType<GlacialBullet>();
			}
		}
	}

	public class GlacialBullet : ModProjectile
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NanoBullet}";

        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.Bullet);
			AIType = ProjectileID.NanoBullet;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.Frostburn, 160);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

			return base.OnTileCollide(oldVelocity);
        }

        public override void AI()
        {
            if (Main.rand.NextBool(4))
            {
				Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 92);
				d.noGravity = true;
				d.scale = Main.rand.NextFloat(0.56f, 0.9f);
				d.velocity = Projectile.velocity * 0.3f;
            }
        }
    }
	#endregion

	#region Healing Staff

	internal class GlacialGuardianStaff : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Conjures a frosty healing bolt \nHealing allies freezes them, increasing their defense but lowering their movement speed");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 8;
			Item.useTime = Item.useAnimation = 29;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<GlacialBolt>();
			Item.shootSpeed = 6.8f;
			Item.noMelee = true;
			Item.sellPrice(0, 0, 35);

			Item.mana = 10;
			healAmount = 7;
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
				.AddIngredient(ModContent.ItemType<Materials.GlacialBar>(), 8)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class GlacialBolt : clericHealProj
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 8;
			Projectile.timeLeft = 85;
			Projectile.alpha = 0; // 255;

			healUsesBuffs = true;
			healPenetrate = 1;
		}

        public override void BuffEffects(Player target, Player healer)
        {
			target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.GlacialGuardBuff>(), GetBuffTime(healer, 15));
        }

        public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 10);
			Dust d = Dust.NewDustPerfect(Projectile.Center, 92); // 96 kinda cool accident
			//Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 92);
			d.noGravity = true;
			d.velocity = Projectile.velocity * -0.2f;
			d.scale = 1.1f;
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 20; i++)
			{
				Vector2 vel = new Vector2(Main.rand.NextFloat(0.25f, 2.25f)).RotatedByRandom(MathHelper.ToRadians(180));
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92);
				d.scale = Main.rand.NextFloat(1.05f, 1.2f);
				d.noGravity = true;
				d.fadeIn = d.scale * 1.15f;
				d.velocity = vel;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			float scale = 1.1f;
			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor); // * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
				scale -= 0.04f + (0.01f * Projectile.ai[0]);
			}

			return true;
		}

	}

	#endregion
}
