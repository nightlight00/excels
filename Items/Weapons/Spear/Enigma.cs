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

namespace excels.Items.Weapons.Spear
{
    internal class Enigma : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Enigma");
			Tooltip.SetDefault("Charge forward while thrusting the spear\nRight-clicking instead fires an explosive energy ball\n'Eyes up, Guardian!'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 46;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 29;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.damage = 63;
            Item.knockBack = 4.7f;
            Item.rare = 5;
            Item.UseSound = SoundID.Item71;
            Item.shoot = ModContent.ProjectileType<EnigmaProjectile>();
            Item.shootSpeed = 3.4f;
            Item.sellPrice(0, 0, 50);
			Item.autoReuse = true;

        }

		public override bool AltFunctionUse(Player player) => true;

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
				if (player.altFunctionUse == 2)
					SoundEngine.PlaySound(SoundID.Item36, player.Center); // shotgun sound
				else
					SoundEngine.PlaySound(SoundID.Item71, player.Center);
            }

            return null;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.altFunctionUse == 2)
            {
				Projectile.NewProjectile(source, position, velocity * 2, ModContent.ProjectileType<EnigmaBlast>(), (int)(damage * 2.4f), 2, player.whoAmI);
				Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
				p.ai[0] = 1;
				player.reuseDelay = 15;
				return false;
			}
			return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AncientBattleArmorMaterial, 2)
                .AddIngredient(ItemID.AdamantiteBar, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.AncientBattleArmorMaterial, 2)
                .AddIngredient(ItemID.TitaniumBar, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

	public class EnigmaProjectile : ModProjectile
	{
		// Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 40f;
		protected virtual float HoldoutRangeMax => 160f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enigma");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.
		}

        public override bool? CanHitNPC(NPC target)
        {
			if (Projectile.ai[0] == 0 && !target.friendly)
				return base.CanHitNPC(target);
			return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
			if (Projectile.ai[0] == 0)
			{
				player.immune = true;
				player.immuneNoBlink = true;
				int iTime = 14;
				if (Main.expertMode)
					iTime = 12;
				if (Main.masterMode)
					iTime = 8;
				if (player.immuneTime < iTime)
					player.immuneTime = iTime;
			}
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
				if (Projectile.ai[0] == 0)
				{
					player.velocity = new Vector2(17, 0).RotatedBy(Projectile.velocity.ToRotation());
					player.immune = true;
					player.immuneNoBlink = true;
					if (player.immuneTime < 2)
						player.immuneTime = 2;
				}
			}

			if (Projectile.ai[0] == 0)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center, 134);
				d.noGravity = true;
				d.velocity = Projectile.velocity / 5;
				d.scale = 1.4f;
			}

			Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() * 0.85f);

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

			return false; // Don't execute vanilla AI.
		}
	}

	public class EnigmaBlast : ModProjectile
    {
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = Projectile.height = 14;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] == 0)
            {
				Explosion();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.ai[0] == 0)
			{
				Explosion();
			}
			return false;
        }

        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() / 3);

			if (Projectile.ai[0] != 0)
				return;

			Dust d = Dust.NewDustPerfect(Projectile.Center, 134);
			d.velocity = Projectile.velocity / 5;
			d.noGravity = true;
			d.scale = 1.4f;

			for (var i = 0; i < 2; i++)
            {
				Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 134);
				d2.noGravity = true;
				d2.scale = 0.8f;
				d2.velocity = Projectile.velocity / 2;
			}
        }

		private void Explosion()
        {
			Projectile.ai[0]++;

			SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 6;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.position = Projectile.Center;
			Projectile.width = Projectile.height = 120;
			Projectile.Center = Projectile.position;
			for (var i = 0; i < 50; i++)
            {
				Dust d = Dust.NewDustPerfect(Projectile.Center, 134);
				d.velocity = Main.rand.NextVector2Circular(20, 20) / 3;
				d.noGravity = false;
				d.scale = 1.6f;

				if (i % 3 == 0)
                {
					Dust d2 = Dust.NewDustPerfect(Projectile.Center, 31);
					d2.color = new Color(Color.Purple.ToVector3() / 3);
					d2.noGravity = true;
					d2.velocity = Main.rand.NextVector2Circular(20, 20) / 2.7f;
					d2.scale = Main.rand.NextFloat(1.5f, 1.8f);
				}
            }
        }
    }
}
