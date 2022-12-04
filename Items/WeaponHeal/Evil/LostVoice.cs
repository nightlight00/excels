using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace excels.Items.WeaponHeal.Evil
{
    #region Lost Voices
    internal class LostVoices : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Releases lost souls upon destruction");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 28;
			Item.useTime = Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.value = 50000;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<LostVoiceThrown>();
			Item.shootSpeed = 8.7f;
			Item.noMelee = true;

			Item.mana = 10;
			healAmount = 8;
			healRate = 0.5f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
        }

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.SpectreBar, 6)
				.AddIngredient(ItemID.Bone, 45)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
    }

	internal class LostVoiceThrown : clericHealProj
	{
		public override string Texture => "excels/Items/WeaponHeal/Evil/LostVoices";
        public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 28;
			Projectile.timeLeft = 999;
			Projectile.friendly = true;

			clericEvil = true;
			canDealDamage = false;
		}

		public override void AI()
		{
			Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X) * 1.8f;
			Projectile.ai[0]++;
			Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() / 2);

			if (Projectile.ai[0] > 20)
			{
				Projectile.velocity.Y += 0.3f;
				Projectile.velocity.X *= 0.983f;

				if (Projectile.velocity.X > 0)
				{
					Projectile.rotation += MathHelper.ToRadians((Projectile.ai[0] - 10) * 0.5f);
				}
				else
				{
					Projectile.rotation -= MathHelper.ToRadians((Projectile.ai[0] - 10) * 0.5f);
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Death();
			return true;
		}

		void Death()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (var p = 0; p < 3; p++)
                {
					Vector2 vel = new Vector2(2).RotatedBy(MathHelper.ToRadians((p * 120) + Main.rand.Next(-20, 20)));
					Projectile proj2 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<LostVoice>(), 0, 0, Main.player[Projectile.owner].whoAmI);
					proj2.GetGlobalProjectile<excelProjectile>().healStrength = Projectile.GetGlobalProjectile<excelProjectile>().healStrength;
					proj2.GetGlobalProjectile<excelProjectile>().healRate = Projectile.GetGlobalProjectile<excelProjectile>().healRate;
					proj2.netUpdate = true;
				}
			}

			for (var i = 0; i < 25; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 180);
				d.noGravity = true;
				d.velocity = Main.rand.NextVector2Circular(2.7f, 2.7f);
				d.scale *= Main.rand.NextFloat(1.3f, 1.7f);
				if (Main.rand.NextBool(6))
                {
					d.velocity *= 1.8f;
					d.scale *= 1.2f;
					d.noGravity = false;
                }
			}

			if (Main.netMode == NetmodeID.Server)
			{
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			for (var i = 0; i < 5; i++)
			{
				Gore g = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, new Vector2(2, 2).RotatedBy(MathHelper.ToRadians(90 * i)), GoreID.Smoke1);
				g.velocity = new Vector2(0, Main.rand.NextFloat(1.1f, 2.6f)).RotatedBy(MathHelper.ToRadians((360 / 5) * i + Main.rand.Next(-30, 31)));
				g.rotation = MathHelper.ToRadians(Main.rand.Next(360));
				for (var e = 0; e < 4; e++)
				{
					Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 13);
					d.velocity = new Vector2(0, Main.rand.NextFloat(0.8f, 3)).RotatedByRandom(MathHelper.ToRadians(360));
				}
			}
			SoundEngine.PlaySound(SoundID.Item107, Projectile.Center);
		}
	}

	internal class LostVoice : clericHealProj
    {
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 12;
			Projectile.timeLeft = 400;
			Projectile.alpha = 100; // 255;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 3;

			canDealDamage = false;
			healPenetrate = 3;
			clericEvil = true;
		}

        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * .75f);
			if (++Projectile.ai[0] > 25 && Projectile.ai[1] == 0)
            {
				Vector2 targetPos = Vector2.Zero;
				float targetHealth = 1000;
				bool target = false;
				for (int k = 0; k < 200; k++)
				{
					Player player = Main.player[k];
					float health = player.statLife;
					if (health < targetHealth && health < player.statLifeMax2 && player != Main.player[Projectile.owner] && !heallist.Contains(player.whoAmI))
					{
						targetHealth = health;
						targetPos = player.Center;
						target = true;
					}
				}
				if (target)
                {
					Vector2 move = targetPos - Projectile.Center;
					AdjustMagnitude(ref move);
					// the 10 is now much it 'turns'
					Projectile.velocity = (10 * Projectile.velocity + move) / 5f;
					AdjustMagnitude(ref Projectile.velocity);
				}
				HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 20);
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 180);
			d.velocity = -Projectile.velocity * 0.7f;
			d.noGravity = true;
			d.scale *= 0.8f;
		}

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 25; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 180);
				d.noGravity = true;
				d.velocity = Main.rand.NextVector2Circular(1.6f, 1.6f);
				d.scale *= Main.rand.NextFloat(0.76f, .9f);
			}
        }

        private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				// the 2 is the velocity
				vector *= 2f / magnitude;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, 0.8f, SpriteEffects.None, 0);
			}

			return true;
		}
	}
    #endregion

    #region Healing Haunter
	internal class HealingHaunter : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Fires lost souls at an extreme velocity\nRight click to change swap firing modes");
			//Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 28;
			Item.useTime = Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 50000;
			Item.rare = 8;
			Item.UseSound = SoundID.Item40;
			Item.shoot = ModContent.ProjectileType<LostVoice>();
			Item.shootSpeed = 10f;
			Item.noMelee = true;
			Item.scale = 1.1f;

			Item.mana = 20;
			healAmount = 25;
			healRate = 2;
		}

		bool fireModeSniper = true;

        public override bool AltFunctionUse(Player player) => true;

        public override void UpdateInventory(Player player)
        {
			if (player.HeldItem != Item)
				return;

			if (player.altFunctionUse == 2)
			{
				fireModeSniper = !fireModeSniper;
				if (fireModeSniper)
				{
					Item.autoReuse = false;
					Item.mana = 20;
					healAmount = 25;
					healRate = 2;
					Item.useTime = Item.useAnimation = 45;
					Item.shootSpeed = 10f;
					CombatText.NewText(player.getRect(), Color.LightSkyBlue, "Fire Mode : Rifle");
				}
				else
				{
					Item.autoReuse = true;
					Item.mana = 6;
					healAmount = 6;
					healRate = 1;
					Item.useTime = Item.useAnimation = 11;
					Item.shootSpeed = 6.4f;
					CombatText.NewText(player.getRect(), Color.LightSkyBlue, "Fire Mode : Full Auto");
				}
			}
        }

        public override Vector2? HoldoutOffset()
        {
			return new Vector2(-4, 0);
        }

        public override bool CanUseItem(Player player)
        {
			return !(player.altFunctionUse == 2);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (fireModeSniper)
			{
				Projectile p = CreateHealProjectile(player, source, position, velocity, type, damage, knockback, ai1: 1);
				p.timeLeft = 100;
				p.extraUpdates++;
			}
			else
            {
				Projectile p = CreateHealProjectile(player, source, position, velocity.RotatedByRandom(MathHelper.ToRadians(8))*Main.rand.NextFloat(0.9f, 1f), type, damage, knockback, ai1: 1);
				p.timeLeft = 100;
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SpectreBar, 9)
				.AddIngredient(ItemID.Bone, 55)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	#endregion
}

