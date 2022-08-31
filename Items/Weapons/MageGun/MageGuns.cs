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


namespace excels.Items.Weapons.MageGun
{
	#region Thunder Lord
	public class ThunderLord : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Not to be confused with a cobalt hammer'");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 46;
			Item.knockBack = 4;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = Item.useAnimation = 35;
			Item.mana = 6;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<Thunder>();
			Item.shootSpeed = 2f;
			Item.damage = 37;
			Item.rare = 3;
			Item.UseSound = SoundID.Item11;
			Item.noMelee = true;
			Item.sellPrice(0, 1, 10);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0f);
		}

	}

	public class Thunder : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.GolfBallDyedViolet}";

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 4;
			Projectile.timeLeft = 400;
			Projectile.extraUpdates = 22;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 255;
		}

		Vector2 initialVel = Vector2.Zero;
		int DustTimer = 0;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[1] == 1)
            {
				CallStorm();
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
			if (Projectile.ai[1] == 1)
			{
				CallStorm();
			}
		}

		private void CallStorm()
        {
			SoundEngine.PlaySound(SoundID.NPCDeath56, Projectile.Center);
			for (var i = 0; i < 3; i++)
            {
				Projectile.NewProjectile(Projectile.GetSource_FromThis(),
					new Vector2(Projectile.Center.X + Main.rand.Next(-200, 201), Projectile.Center.Y - 600), new Vector2(0, 5),
					ModContent.ProjectileType<Thunder>(), (int)(Projectile.damage * 0.66f), Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
            }
        }

        public override void AI()
		{
			if (initialVel == Vector2.Zero)
			{
				initialVel = Projectile.velocity;
			}
			if (++Projectile.ai[0] > 4)
			{
				Projectile.velocity = initialVel.RotatedByRandom(MathHelper.ToRadians(35));
				Projectile.ai[0] = 0;
			}

			DustTimer++;
			if (DustTimer > 17 || Projectile.ai[1] == 2)
			{
				Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 91); //204
				d.velocity *= 0;
				d.fadeIn = 1.3f;
				d.noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 28; i++)
			{
				Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
				Dust d = Dust.NewDustPerfect(Projectile.Center, 91, speed * 2.4f); //204
																				   //d.velocity *= 0;
				d.fadeIn = 1.3f;
				d.noGravity = true;
			}
		}
	}
	#endregion

	#region Storm Caller
	public class StormCaller : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Striking foes calls upon a storm");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 28;
			Item.knockBack = 4;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = Item.useAnimation = 30;
			Item.mana = 9;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<Thunder>();
			Item.shootSpeed = 2.5f;
			Item.damage = 51;
			Item.rare = 6;
			Item.UseSound = SoundID.NPCHit53;
			Item.noMelee = true;
			Item.sellPrice(0, 1, 90);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0f);
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			// do this to assign ai[1]
			Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			p.ai[1] = 1;
			return false;
        }

    }
	#endregion

	#region Zeus
	public class Zeus : ModItem
	{
		//public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.GolfBallDyedViolet}";

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("The wrath of the heavens within your palms");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 28;
			Item.knockBack = 5.2f;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = Item.useAnimation = 42;
			Item.mana = 12;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<ZeusOrb>();
			Item.shootSpeed = 2.5f;
			Item.damage = 82;
			Item.rare = 8;
			Item.UseSound = SoundID.NPCHit53;
			Item.noMelee = true;
			Item.sellPrice(0, 5);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, 0f);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// do this to assign ai[1]
			Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<StormCaller>())
				.AddIngredient(ItemID.MagnetSphere)
				.AddIngredient(ItemID.SpectreBar, 8)
				.AddIngredient(ItemID.SoulofMight, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}

	}

	public class ZeusOrb : ModProjectile
    {
		//public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.GolfBallDyedViolet}";

        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Thunder Orb");
			Main.projFrames[Projectile.type] = 2;
		}

        public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 32;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 255;
			Projectile.scale = 1;
			Projectile.penetrate = -1;
		}

        public override bool? CanDamage() => Projectile.ai[1] == 0;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			for (var i = 0; i < 28; i++)
			{
				Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
				Dust d2 = Dust.NewDustPerfect(Projectile.Center + (speed * 4), 91, speed * 6f); //204
				d2.scale = 1.1f;
				d2.fadeIn = 1.4f;
				d2.noGravity = true;
			}
			return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 300;
			Projectile.ai[1]++;
			Projectile.alpha = 255;
		}

		float dustInc = 0;

        public override void AI()
        {
			if (Projectile.ai[1] == 0)
			{
				if (Projectile.alpha > 0)
				{
					//Projectile.scale += 0.02f;
					Projectile.alpha -= 2;
					Projectile.rotation = Projectile.velocity.ToRotation();
				}
				else
				{
					Projectile.ai[0]++;
					if (Projectile.ai[0] % 60 == 0)
					{
						NPC closestNPC = FindClosestNPC(1200);
						if (closestNPC != null)
						{
							Vector2 shootVel = Projectile.Center - closestNPC.Center;
							if (shootVel == Vector2.Zero)
							{
								shootVel = new Vector2(0f, 1f);
							}
							shootVel.Normalize();
							shootVel *= -3;
							Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Thunder>(), Projectile.damage / 2, 0, Main.player[Projectile.owner].whoAmI);
							p.ai[1] = 2;
						}
					}
				}
				// dust
				dustInc += 0.2f;
				if (dustInc > 16) dustInc = 16;
				for (var i = 0; i < 3; i++)
				{
					Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
					Dust d = Dust.NewDustPerfect(Projectile.Center + (speed * dustInc), 91);
					d.scale = 0.5f;
					d.fadeIn = 0.9f;
					d.velocity *= 0;
					d.noGravity = true;
					d.alpha = Projectile.alpha / 4;
				}
				if (Projectile.timeLeft == 1)
                {
					Projectile.alpha = 255;
					SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
					Projectile.velocity = Vector2.Zero;
					Projectile.timeLeft = 300;
					Projectile.ai[1]++;
                }
				if (++Projectile.frameCounter >= 10)
				{
					Projectile.frameCounter = 0;
					if (++Projectile.frame >= Main.projFrames[Projectile.type])
						Projectile.frame = 0;
				}

			}
			else
            {
				if (Projectile.timeLeft % 80 == 0)
				{
					SoundEngine.PlaySound(SoundID.NPCDeath56, Projectile.Center);
					for (var i = 0; i < 28; i++)
					{
						Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
						Dust d2 = Dust.NewDustPerfect(Projectile.Center + (speed * 4), 91, speed * 6f); //204
						d2.scale = 1.1f;
						d2.fadeIn = 1.4f;
						d2.noGravity = true;
					}
					for (var i = 0; i < 3; i++)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(),
						new Vector2(Projectile.Center.X + Main.rand.Next(-300, 301), Projectile.Center.Y - 600), new Vector2(0, 5),
						ModContent.ProjectileType<Thunder>(), Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
					}
				}
				Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 91);
				d.velocity.X = Main.rand.NextFloat(-3.5f, 3.6f);
				d.velocity.Y = Main.rand.NextFloat(-3.5f, 3.6f);
				d.scale = Main.rand.NextFloat(0.7f, 0.9f);
				d.fadeIn = d.scale + .2f;
				d.noGravity = true;
			}
		}

		public NPC FindClosestNPC(float maxDetectDistance)
		{
			NPC closestNPC = null;
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
			for (int k = 0; k < Main.maxNPCs; k++)
			{
				NPC target = Main.npc[k];
				if (target.CanBeChasedBy())
				{
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}

			return closestNPC;
		}
	}
	#endregion
}
