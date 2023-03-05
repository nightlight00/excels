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
using excels.Items.Materials.Ores;

namespace excels.Items.Weapons.Staff1
{
	#region Singe Staff
	public class SingeStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Singe Staff");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 46;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = Item.useAnimation = 26;
			Item.mana = 6;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<Singe>();
			Item.shootSpeed = 2f;
			Item.damage = 37;
			Item.rare = 3;
			Item.UseSound = SoundID.Item20;
			Item.noMelee = true;
			Item.knockBack = 5.7f;
			Item.sellPrice(0, 0, 40);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class Singe : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.width = Projectile.height = 7;
			Projectile.scale = 2;
			Projectile.timeLeft = 120;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 18;
		}

		// make an explosion on collision

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 180);
			Explosion();
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 180);
			Explosion();
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.tileCollide)
			{
				Explosion();
			}
			return Projectile.tileCollide;
		}

		private void Explosion()
		{
			Projectile.alpha = 255;
			Projectile.velocity *= 0;
			Projectile.timeLeft = 10;
			Projectile.knockBack *= 1.45f;
			Projectile.tileCollide = false;
			Projectile.ai[0] = 2;
			Projectile.position = Projectile.Center;
			Projectile.width = Projectile.height = 50;
			Projectile.Center = Projectile.position;
			SoundEngine.PlaySound(SoundID.Item100, Projectile.Center); // grenade explosion
			for (var i = 0; i < 23; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
				d.scale = Main.rand.NextFloat(1) / 4 + 0.9f;
				d.velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-0.25f, -2));
			}
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.scale > 1)
			{
				Projectile.scale -= 0.06f;
				Projectile.velocity *= 1.13f;
			}
			for (var i = 0; i < 2; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
				d.velocity *= 0.1f;
				d.alpha = 100;
				d.scale = Main.rand.NextFloat(0.85f, 1.2f) * Projectile.scale;
				d.noGravity = true;
			}
		}
	}

	#endregion

	#region Dance of Ice and Fire
	public class DanceIceFire : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dance of Ice and Fire");
			Tooltip.SetDefault("'The beauty of perfect harmony'");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 46;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = Item.useAnimation = 24;
			Item.mana = 8;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<IceFire>();
			Item.shootSpeed = 9.6f;
			Item.damage = 39;
			Item.rare = 5;
			Item.UseSound = SoundID.Item20;
			Item.noMelee = true;
			Item.knockBack = 3.2f;
			Item.sellPrice(0, 0, 50);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float ai = Main.rand.NextFloat(0.8f, 1.4f);
			float ai1 = Main.rand.Next(2);
			Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			p.ai[0] = ai;
			p.ai[1] = ai1;
			Projectile p2 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			p2.ai[0] = -ai;
			p2.ai[1] = ai1;
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 9)
				.AddIngredient(ModContent.ItemType<GlacialBar>(), 9)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class IceFire : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SnowBallFriendly}";

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 14;
			Projectile.scale = 0.6f;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 100;
			Projectile.friendly = true;
			Projectile.alpha = 255;
		}

		float timer = 0;
		bool first = true;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.ai[0] > 0)
			{
				target.AddBuff(BuffID.OnFire, 240);
			}
			else
			{
				target.AddBuff(BuffID.Frostburn, 240);
			}
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			if (Projectile.ai[0] > 0)
			{
				target.AddBuff(BuffID.OnFire, 240);
			}
			else
			{
				target.AddBuff(BuffID.Frostburn, 240);
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Projectile.ai[0] > 0)
			{
				for (var i = 0; i < 15; i++)
				{
					Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
					d.scale = Main.rand.NextFloat(1.15f, 1.4f);
					d.noGravity = true;
					d.velocity *= 0.35f * i;//3f ;
				}
			}
			else
			{
				for (var i = 0; i < 15; i++)
				{
					Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92);
					d.scale = Main.rand.NextFloat(0.9f, 1.3f);
					d.noGravity = true;
					d.velocity *= 0.25f * i; // 3f;
				}
			}
		}

		int timerMax = 11;

		public override void AI()
		{
			timer++;
			if (timer > timerMax)
			{/*
				switch (Projectile.ai[1])
				{
					// first time its halved so its more cool looking
					case 0: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(15 * Projectile.ai[0])); break;
					case 1: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-10 * Projectile.ai[0])); break;
				//	case 2: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(9 * Projectile.ai[0])); break;
				//	case 3: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-16 * Projectile.ai[0])); break;
				//	case 4: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(5 * Projectile.ai[0])); break;
				//	case 5: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-3 * Projectile.ai[0])); break;
				//	case 6: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(13 * Projectile.ai[0])); break;
				//	case 7: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-10 * Projectile.ai[0])); break;
				//	case 8: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(13 * Projectile.ai[0])); break;
				//	case 9: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-17 * Projectile.ai[0])); break;
				}
				*/
				int amt = 28;
				if (Projectile.ai[1] == 0)
				{
					amt = -amt;
				}
				if (first)
				{
					amt /= 2;
					first = false;
				}
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(amt * Projectile.ai[0]));
				timer = 0;
				Projectile.ai[1]++;
				if (Projectile.ai[1] > 1)
				{
					Projectile.ai[1] = 0;
				}
			}
			if (Projectile.timeLeft < 95)
			{
				for (var i = 0; i < 2; i++)
				{
					if (Projectile.ai[0] > 0)
					{
						Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
						d.velocity *= 0.2f;
						d.scale = Main.rand.NextFloat(0.95f, 1.4f);
						d.noGravity = true;
					}
					else
					{
						Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92);
						d.velocity *= 0.2f;
						d.scale = Main.rand.NextFloat(0.8f, 1.3f);
						d.noGravity = true;
					}
				}
			}
		}
	}
	#endregion

	#region Blade Wand
	public class BladeWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blade Baton");
			Tooltip.SetDefault("Conducts mystical blades to slice your foes \n'The seventh flower blooms it's petals of steel'");

			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 46;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = Item.useAnimation = 16;
			Item.mana = 4;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<BladeMagic>();
			Item.shootSpeed = 2f;
			Item.damage = 27;
			Item.rare = 3;
			Item.UseSound = SoundID.Item20;
			Item.noMelee = true;
			Item.knockBack = 1.6f;
			Item.sellPrice(0, 0, 20);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (var i = 0; i < 3; i++)
			{
				int outer = 50;
				if (Main.rand.NextBool())
				{
					outer = -50;
				}
				Vector2 pos = position + new Vector2(outer, Main.rand.Next(-50, 51));
				if (Main.rand.NextBool())
				{
					pos = position + new Vector2(Main.rand.Next(-50, 51), outer);
				}

				Projectile.NewProjectile(source, pos, Vector2.Zero, type, damage, knockback, player.whoAmI);
			}
			return false; // base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<MysticCrystal>(), 6)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class BladeMagic : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 240;
			Projectile.alpha = 255;
		}

		public override bool? CanDamage()
		{
			if (Projectile.ai[1] < 30)
			{
				return false;
			}
			return true;
		}

		public override void AI()
		{
			Projectile.ai[1]++;
			if (Projectile.ai[1] < 30)
			{
				Dust dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 60, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1.1f);
				dust.noGravity = true;
				dust.fadeIn = 1.3f;
				dust.noLight = true;
			}
			else if (Projectile.ai[1] == 30)
			{
				for (var i = 0; i < 10; i++)
				{
					Dust dust = Terraria.Dust.NewDustDirect(Projectile.Center, 0, 0, 60, 0, 0, 0, new Color(255, 255, 255), 1.575f);
					dust.noGravity = true;
					dust.fadeIn = 1.5f;
					dust.noLight = true;
					dust.velocity = new Vector2(2, 2).RotatedBy(MathHelper.ToRadians(36 * i));
				}
				Projectile.alpha = 0;
				Projectile.timeLeft = 120;
				//Projectile.ai[1] = 0;

				if (Main.myPlayer == Main.player[Projectile.owner].whoAmI)
				{
					Vector2 shootVel = Main.MouseWorld - Projectile.Center;
					if (shootVel == Vector2.Zero)
					{
						shootVel = new Vector2(0f, 1f);
					}
					shootVel.Normalize();
					shootVel *= 5;
					Projectile.velocity = shootVel;

					Projectile.netUpdate = true;
				}
				SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
			}
			else
			{
				Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 9, 9, 43, newColor: Color.White);
				d.velocity = -Projectile.velocity / 5;
				d.noLight = true;
				d.noGravity = true;

				if (Projectile.ai[1] < 70 && Main.myPlayer == Main.player[Projectile.owner].whoAmI)
				{
					Vector2 move = Main.MouseWorld - Projectile.Center;
					AdjustMagnitude(ref move);
					Projectile.velocity = (20 * Projectile.velocity + move); // / 5f;
					AdjustMagnitude(ref Projectile.velocity);
					Projectile.rotation = Projectile.velocity.ToRotation();

					Projectile.netUpdate = true;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 11; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(9, 9), 19, 19, 43, newColor: Color.White);
				d.scale = Main.rand.NextFloat(1, 1.2f);
				d.velocity = (Projectile.velocity / (1.5f * d.scale)).RotatedByRandom(MathHelper.ToRadians(8));
				d.noLight = true;
				d.noGravity = true;
			}
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 10f / magnitude;
			}
		}
	}
	#endregion

	#region FrozenScarf 
	public class FrozenScarf : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Casts seeking icicles to impale foes");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 46;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = Item.useAnimation = 24;
			Item.mana = 8;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<IcicleP>();
			Item.shootSpeed = 9f;
			Item.damage = 48;
			Item.rare = 5;
			Item.knockBack = 4;
			Item.UseSound = SoundID.Item20;
			Item.noMelee = true;
		}
	}

	public class IcicleP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Icicle");
		}

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 14;
			Projectile.scale = 0.6f;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 200;
			Projectile.friendly = true;
			Projectile.alpha = 40;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = 3;
			Projectile.scale = 1.3f;
		}

		float maxVel = 7;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 120);
			maxVel += 2.3f;
			Projectile.damage = (int)(Projectile.damage * 1.15f);
			Projectile.timeLeft += 70;
		}

		public override void AI()
		{
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92);
			d.velocity *= -0.4f;
			d.noGravity = true;
			d.scale *= 0.94f;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

			Vector2 targetPos = Vector2.Zero;
			float targetDist = 600;
			bool target = false;
			for (int k = 0; k < 200; k++)
			{
				NPC npc = Main.npc[k];
				if (npc.CanBeChasedBy(this, false))
				{
					float distance = Vector2.Distance(npc.Center, Projectile.Center);
					if (distance < targetDist)
					{
						targetDist = distance;
						targetPos = npc.Center;
						target = true;
					}
				}
			}
			if (target)
			{
				float num145 = 15f;
				float num146 = 0.0833333358f;
				Vector2 vec = targetPos - Projectile.Center;
				vec.Normalize();
				if (vec.HasNaNs())
				{
					vec = new Vector2((float)Projectile.direction, 0f);
				}
				Projectile.velocity = (Projectile.velocity * (num145 - 1f) + vec * (Projectile.velocity.Length() + num146)) / num145;
			}

			if (Projectile.velocity.Length() < maxVel)
			{
				Projectile.velocity *= 1.1f;
				return;
			}
		}
	}
	#endregion

	#region Spore Staff
	public class SporeStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Staff");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 46;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = Item.useAnimation = 31;
			Item.mana = 5;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<Spores>();
			Item.shootSpeed = 8.7f;
			Item.damage = 11;
			Item.rare = 3;
			Item.UseSound = SoundID.Item20;
			Item.noMelee = true;
			Item.knockBack = 2.1f;
			Item.sellPrice(0, 0, 0, 14);
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(18)), type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-18)), type, damage, knockback, player.whoAmI);
			return true;
        }

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.JungleSpores, 8)
				.AddIngredient(ItemID.Vine, 3)
				.AddIngredient(ItemID.RichMahogany, 20)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class Spores : ModProjectile 
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SnowBallFriendly}";

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 14;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 100;
			Projectile.friendly = true;
			Projectile.alpha = 255;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.Poisoned, 240);
        }

		public override void AI()
		{
			if (++Projectile.ai[0] < 7)
            {
				return;
            } 
			for (var i = 0; i < 3; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 44);
				d.noGravity = true;
				d.velocity = Projectile.velocity * -0.3f;
				d.alpha = 80;
				d.scale = Main.rand.NextFloat(0.9f, 1.25f);
			}
			if (Main.rand.NextBool(4))
            {
				Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
				d2.noGravity = true;
				d2.velocity = Projectile.velocity * -0.55f;
				d2.scale = 1.4f;
			}
		}

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 10; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 44);
				d.noGravity = true;
				d.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(25)) * -0.9f;
				d.alpha = 30;
				d.scale = Main.rand.NextFloat(1.1f, 1.35f);

				Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
				d2.noGravity = true;
				d2.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(25)) * -0.6f;
				d2.scale = Main.rand.NextFloat(1.25f, 1.5f);
			}
        }
    }
	#endregion

	#region Elemental Envoy
	public class ElementalEnvoy : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Conjures the four primary elements to strike foes" +
							 "\nFires burns foes\nWater deals massive knockback\nEarth inflicts poison and deals increased damage\nAir penetrates foes");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 48;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = Item.useAnimation = 45;
			Item.mana = 8;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<ElementalBlast>();
			Item.shootSpeed = 9.1f;
			Item.damage = 20;
			Item.rare = 3;
			Item.UseSound = SoundID.Item20;
			Item.noMelee = true;
			Item.knockBack = 3.3f;
			Item.sellPrice(0, 0, 70);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// fire
			Projectile p1 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, 0);
			p1.ai[0] = 0;
			// water
			Projectile p2 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback*5, player.whoAmI, 1);
			p2.ai[0] = 1;
			// earth
			Projectile p3 = Projectile.NewProjectileDirect(source, position, velocity, type, (int)(damage*1.4f), knockback, player.whoAmI, 2);
			p3.ai[0] = 2;
			// air
			Projectile p4 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, 3);
			p4.ai[0] = 3;
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SporeStaff>())
				.AddIngredient(ModContent.ItemType<SingeStaff>())
				.AddIngredient(ItemID.AquaScepter)
				.AddIngredient(ModContent.ItemType<Skyline.SkylineStaff>())
				.AddTile(ModContent.TileType<Tiles.Stations.StarlightAnvilTile>())
				.Register();
		}
	}

	public class ElementalBlast : ModProjectile
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SnowBallFriendly}";

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 8;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 140;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] == 0)
            {
				target.AddBuff(BuffID.OnFire, 120);
            }
			else if (Projectile.ai[0] == 2)
            {
				target.AddBuff(BuffID.Poisoned, 180);
            }
        }

        Vector2 posRef = Vector2.Zero;
        
		public override void AI()
        {
			if (Projectile.ai[1] == 0)
            {
				posRef = Main.player[Projectile.owner].Center;
				if (Projectile.ai[0] == 3)
                {
					Projectile.penetrate = 3;
                }
            }
			posRef += Projectile.velocity;
			Projectile.ai[1] += 6;

			Projectile.Center = posRef + Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.ai[1] + (Projectile.ai[0]*90))) * 10;

            switch (Projectile.ai[0])
            {
				case 0: // fire
					Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 6);
					d.noGravity = true;
					d.velocity = Vector2.Zero;
					d.scale = 1.9f;
					break;
				case 1: // water
					Dust d2 = Dust.NewDustDirect(Projectile.Center, 0, 0, 41);
					d2.noGravity = true;
					d2.velocity = Vector2.Zero;
					d2.alpha = 120;
					d2.scale = 1.5f;
					break;
				case 2: // earth
					Dust d3 = Dust.NewDustDirect(Projectile.Center, 0, 0, 44);
					d3.noGravity = true;
					d3.velocity = Vector2.Zero;
					d3.scale = 1.5f;
					break;
				case 3: // air
					Dust d4 = Dust.NewDustDirect(Projectile.Center, 0, 0, 16);
					d4.noGravity = true;
					d4.velocity = Vector2.Zero;
					d4.scale = 1.5f;
					break;
			}
        }
    }
	#endregion
}
