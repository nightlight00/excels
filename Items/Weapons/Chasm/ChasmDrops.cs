using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;


namespace excels.Items.Weapons.Chasm
{
    #region Vindicator
    public class V90 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vindicator"); // vindicate
			Tooltip.SetDefault("50% chance to not consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 37;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 5;
            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.height = 26;
            Item.width = 62;
            Item.knockBack = 1.5f;
            Item.rare = 7;
            Item.value = 5000;
            Item.shoot = 10;
            Item.shootSpeed = 11;
            Item.UseSound = SoundID.Item11;
            Item.noMelee = true;
            Item.autoReuse = true;
			Item.sellPrice(0, 5);
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 newSpeed = velocity * Main.rand.NextFloat(0.95f, 1.06f);
            newSpeed = newSpeed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(6)));
            Projectile.NewProjectile(source, position, newSpeed, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-14f, 0f);
        }

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= 0.50f;
		}
	}
    #endregion



    #region Skewer
    public class Skewer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skewer"); // vindicate
			Tooltip.SetDefault("Striking foes creates infectious clouds");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.height = Item.width = 40;
            Item.knockBack = 1.5f;
            Item.rare = 7;
            Item.value = 5000;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SkewerSlice>(); // The projectile is what makes a shortsword work
			Item.shootSpeed = 12;
			Item.noUseGraphic = true;
			Item.sellPrice(0, 4, 80);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 45f;
			position += muzzleOffset;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(20)));
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
	}

	// taken from github : shortsword projectile
	public class SkewerSlice : ModProjectile
	{
		public const int FadeInDuration = 2;
		public const int FadeOutDuration = 2;

		public const int TotalDuration = 9;

		// The "width" of the blade
		public float CollisionWidth => 10f * Projectile.scale;

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skewer");
		}

		public override void SetDefaults()
		{
			Projectile.alpha = 100;
			Projectile.Size = new Vector2(6); // This sets width and height to the same value (important when projectiles can rotate)
			Projectile.aiStyle = -1; // Use our own AI to customize how it behaves, if you don't want that, keep this at ProjAIStyleID.ShortSword. You would still need to use the code in SetVisualOffsets() though
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true; // Prevents hits through tiles. Most melee weapons that use projectiles have this
			Projectile.extraUpdates = 1; // Update 1+extraUpdates times per tick
			Projectile.timeLeft = 360; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
			Projectile.hide = true; // Important when used alongside player.heldProj. "Hidden" projectiles have special draw conditions
			
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Vector2 sped = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90 + Main.rand.Next(-15, 16))) * 0.5f;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, sped, ModContent.ProjectileType<MushroomSkewer>(), Projectile.damage / 2, 0, Main.player[Projectile.owner].whoAmI);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, -sped, ModContent.ProjectileType<MushroomSkewer>(), Projectile.damage / 2, 0, Main.player[Projectile.owner].whoAmI);
		}

        public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Timer += 1;
			if (Timer >= TotalDuration)
			{
				// Kill the projectile if it reaches it's intented lifetime
				Projectile.Kill();
				return;
			}
			else
			{
				// Important so that the sprite draws "in" the player's hand and not fully infront or behind the player
				player.heldProj = Projectile.whoAmI;
			}

			// Fade in and out
			// GetLerpValue returns a value between 0f and 1f - if clamped is true - representing how far Timer got along the "distance" defined by the first two parameters
			// The first call handles the fade in, the second one the fade out.
			// Notice the second call's parameters are swapped, this means the result will be reverted
			Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

			// Keep locked onto the player, but extend further based on the given velocity (Requires ShouldUpdatePosition returning false to work)
			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
			Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

			// Set spriteDirection based on moving left or right. Left -1, right 1
			Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();

			// Point towards where it is moving, applied offset for top right of the sprite respecting spriteDirection
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;

			// The code in this method is important to align the sprite with the hitbox how we want it to
			SetVisualOffsets();
		}

		private void SetVisualOffsets()
		{
			// 48 is the sprite size (here both width and height equal)
			const int HalfSpriteWidth = 48 / 2;
			const int HalfSpriteHeight = 48 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

			// Vanilla configuration for "hitbox towards the end"
			//if (Projectile.spriteDirection == 1) {
			//	DrawOriginOffsetX = -(HalfProjWidth - HalfSpriteWidth);
			//	DrawOffsetX = (int)-DrawOriginOffsetX * 2;
			//	DrawOriginOffsetY = 0;
			//}
			//else {
			//	DrawOriginOffsetX = (HalfProjWidth - HalfSpriteWidth);
			//	DrawOffsetX = 0;
			//	DrawOriginOffsetY = 0;
			//}
		}

		public override bool ShouldUpdatePosition()
		{
			// Update Projectile.Center manually
			return false;
		}

		public override void CutTiles()
		{
			// "cutting tiles" refers to breaking pots, grass, queen bee larva, etc.
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
			Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity * 6f;
			float collisionPoint = 0f; // Don't need that variable, but required as parameter
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
		}
	}

	public class MushroomSkewer : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.timeLeft = 80;
			Projectile.alpha = 60;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 360);
		}


		public override void AI()
		{
			Projectile.velocity *= 0.91f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.timeLeft < 30)
			{
				Projectile.alpha += 6;
			}
			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}

			if (Main.rand.Next(7) <= 2)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 176);
				d.noGravity = true;
				d.velocity *= 0.3f;
				d.scale = Main.rand.NextFloat(0.9f, 1.4f);
			}
		}
	}
	#endregion
}
