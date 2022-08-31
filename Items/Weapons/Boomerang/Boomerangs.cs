using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;

namespace excels.Items.Weapons.Boomerang
{
	#region Glaive
	public class Glaive : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Weapon so simple even a monkey could use it'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 34;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 32;
			Item.knockBack = 2.3f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useTime = Item.useAnimation = 20;
			Item.shoot = ModContent.ProjectileType<GlaiveProj>();
			Item.shootSpeed = 8.4f;
			Item.rare = 3;
			Item.sellPrice(0, 0, 90);
		}

		public override bool CanUseItem(Player player)
		{
			// doesnt really affect gameplay, just makes it feel more responsive
			return player.ownedProjectileCounts[Item.shoot] < 2;
		}
	}

	public class GlaiveProj : ModProjectile
	{
		public override string Texture => "excels/Items/Weapons/Boomerang/Glaive";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glaive");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BouncingShield);
			//	AIType = ProjectileID.BouncingShield;
			Projectile.aiStyle = -1;
			Projectile.width = Projectile.height = 38;
			//Projectile.DamageType = DamageClass.Melee;
			//Projectile.timeLeft = 9999;
			//Projectile.friendly = true;
			Projectile.penetrate = -1;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.ai[0] = 1f;
			Projectile.netUpdate = true;

			bool shouldMakeSound = false;

			if (oldVelocity.X != Projectile.velocity.X)
			{
				if (Math.Abs(oldVelocity.X) > 4f)
				{
					shouldMakeSound = true;
				}

				Projectile.position.X += Projectile.velocity.X;
				Projectile.velocity.X = -oldVelocity.X;
			}

			if (oldVelocity.Y != Projectile.velocity.Y)
			{
				if (Math.Abs(oldVelocity.Y) > 4f)
				{
					shouldMakeSound = true;
				}

				Projectile.position.Y += Projectile.velocity.Y;
				Projectile.velocity.Y = -oldVelocity.Y;
			}

			if (shouldMakeSound)
			{
				// if we should play the sound..
				Projectile.netUpdate = true;
				Collision.HitTiles(Projectile.position, Projectile.velocity*2, Projectile.width, Projectile.height);
				// Play the sound
				SoundEngine.PlaySound(SoundID.Dig, new Vector2((int)Projectile.position.X, (int)Projectile.position.Y));
			}

			return false;
		}

        float safeRot = 0;
		public override void AI()
		{
			// since the shield changes rotation to match velocity, it doesnt save the old one
			//safeRot += MathHelper.ToRadians(4 * Projectile.velocity.X);
			//Projectile.rotation = safeRot;

			if (Projectile.soundDelay == 0 && Projectile.type != 383)
			{
				Projectile.soundDelay = 8;
				SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
			}

			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[1] += 1f;
				if (Projectile.ai[1] >= 30f)
				{
					Projectile.ai[0] = 1f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
			}
			else
			{
				Projectile.tileCollide = false;
				float num42 = 16f;
				float num43 = 1.2f;

				Vector2 vector2 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num44 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector2.X;
				float num45 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector2.Y;
				float num46 = (float)Math.Sqrt((double)(num44 * num44 + num45 * num45));
				if (num46 > 3000f)
				{
					Projectile.Kill();
				}
				num46 = num42 / num46;
				num44 *= num46;
				num45 *= num46;

				if (Projectile.velocity.X < num44)
				{
					Projectile.velocity.X = Projectile.velocity.X + num43;
					if (Projectile.velocity.X < 0f && num44 > 0f)
					{
						Projectile.velocity.X = Projectile.velocity.X + num43;
					}
				}
				else
				{
					if (Projectile.velocity.X > num44)
					{
						Projectile.velocity.X = Projectile.velocity.X - num43;
						if (Projectile.velocity.X > 0f && num44 < 0f)
						{
							Projectile.velocity.X = Projectile.velocity.X - num43;
						}
					}
				}
				if (Projectile.velocity.Y < num45)
				{
					Projectile.velocity.Y = Projectile.velocity.Y + num43;
					if (Projectile.velocity.Y < 0f && num45 > 0f)
					{
						Projectile.velocity.Y = Projectile.velocity.Y + num43;
					}
				}
				else
				{
					if (Projectile.velocity.Y > num45)
					{
						Projectile.velocity.Y = Projectile.velocity.Y - num43;
						if (Projectile.velocity.Y > 0f && num45 < 0f)
						{
							Projectile.velocity.Y = Projectile.velocity.Y - num43;
						}
					}
				}

				if (Main.myPlayer == Projectile.owner)
				{
					Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
					Rectangle value2 = new Rectangle((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
					if (rectangle.Intersects(value2))
					{
						Projectile.Kill();
					}
				}
			}

			Projectile.rotation += 0.15f * (float)Projectile.direction;
		}
	}

	#endregion
}
