using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using ReLogic.Content;
using excels.Buffs.HealOverTime;

namespace excels.Items.Weapons.HolyWeap.RocketFlail
{
    internal class RocketFlail : ClericHolyWeap
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/F6AC04:-Holy Weapon-]\nRight click to cast [c/F6DD04:Whip Shot] \nHitting foes with [c/F6DD04:Whip Shot] heals nearby allies\n'Breaking me down just builds me up'");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 23;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 10000;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<RocketFlailHead>();
			Item.shootSpeed = 4f;
			Item.noMelee = false;
			Item.knockBack = 5.3f;
			Item.damage = 65;
			Item.rare = ItemRarityID.Pink;

			clericEvil = false;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (player.HasBuff(ModContent.BuffType<Buffs.ClericCld.BlessingCooldown>())) { return false; }
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.channel = true;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noMelee = false;
				Item.channel = false;
				Item.noUseGraphic = false;
			}
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (target.GetGlobalNPC<excelNPC>().BlessedSpell < 180)
				target.GetGlobalNPC<excelNPC>().BlessedSpell = 180;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				CreateHealProjectile(player, source, position, velocity, type, (int)(damage*1.25f), knockback*3);
				player.AddBuff(ModContent.BuffType<Buffs.ClericCld.BlessingCooldown>(), 240);
				return false;
			}
			return false;
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<TemplarsMace>())
				.AddIngredient(ModContent.ItemType<Materials.BottleOfSunlight>(), 4)
				.AddIngredient(ItemID.HallowedBar, 8)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

    public class RocketFlailHead : clericHealProj {

		private const string ChainTexturePath = "excels/Items/Weapons/HolyWeap/RocketFlail/RocketFlail_Chain";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rocket Flail");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SafeSetDefaults()
		{
			Projectile.extraUpdates = 3;
			Projectile.width = Projectile.height = 24;
			Projectile.timeLeft = 2;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 300;

			canDealDamage = true;
			canHealOwner = true;
        }

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!player.active || player.dead || player.noItems || player.CCed || Vector2.Distance(Projectile.Center, player.Center) > 900f)
			{
				Projectile.Kill();
				return;
			}

			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(2);
			Projectile.timeLeft = 2;

			Vector2 direction = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			Projectile.rotation = direction.ToRotation() - MathHelper.ToRadians(90);

			switch (Projectile.ai[0]) {
				// Shot
				case 0:
					if (Vector2.Distance(player.Center, Projectile.Center) > 240)
                    {
						Projectile.ai[0] = 1;
						Projectile.damage = (int)(Projectile.damage * 0.75f);
					}
					break;

				// Return to player
				case 1:
				// Return to player (already activated inspired)
				case 2:
					if (Vector2.Distance(player.Center, Projectile.Center) < 20)
						Projectile.Kill();
					Projectile.velocity = direction * (Projectile.velocity.Length() * 1.02f);
					break;
			}

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if (Projectile.ai[0] != 2)
			{
				Player player = Main.player[Projectile.owner];

				if (Projectile.ai[0] == 0) {
					Projectile.damage = (int)(Projectile.damage * 0.75f);
				}

				for (var i = 0; i < 60; i++)
                {
					Dust d = Dust.NewDustPerfect(player.Center + new Vector2(180).RotatedBy(MathHelper.ToRadians((360 / 30) * i)), 204);
					d.scale = 0.8f;
                }

				for (var i = 0; i < 32; i++)
                {
					Dust d = Dust.NewDustDirect(player.position, player.width, player.height, 204);
					d.velocity = Main.rand.NextVector2Circular(6, 3) * 1.8f;
					d.scale = 1.2f + Main.rand.NextFloat()*0.3f;
                }

				BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 180, 1);
				Projectile.ai[0] = 2;
			}
        }

        public override void BuffEffects(Player target, Player healer)
        {
			target.GetModPlayer<HealOverTime>().AddHeal(healer, "Inspire", 6, 5);
			//target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.InspiredBuff>(), GetBuffTime(healer, 5));
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (Projectile.ai[0] == 0)
			{
				Collision.HitTiles(Projectile.position, oldVelocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
				Projectile.ai[0] = 1;
				Projectile.damage = (int)(Projectile.damage * 0.75f);
			}

            return false;
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);

			// This fixes a vanilla GetPlayerArmPosition bug causing the chain to draw incorrectly when stepping up slopes. The flail itself still draws incorrectly due to another similar bug. This should be removed once the vanilla bug is fixed.
			playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;

			Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>(ChainTexturePath);
			Rectangle? chainSourceRectangle = null;
			// Drippler Crippler customizes sourceRectangle to cycle through sprite frames: sourceRectangle = asset.Frame(1, 6);
			float chainHeightAdjustment = 0f; // Use this to adjust the chain overlap. 

			Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 2f) : (chainTexture.Size() / 2f);
			Vector2 chainDrawPosition = Projectile.Center;
			Vector2 vectorFromProjectileToPlayerArms = playerArmPosition.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
			Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
			float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
			if (chainSegmentLength == 0)
				chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
			float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
			int chainCount = 0;
			float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

			// This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
			while (chainLengthRemainingToDraw > 0f)
			{
				// This code gets the lighting at the current tile coordinates
				Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));

				// Flaming Mace and Drippler Crippler use code here to draw custom sprite frames with custom lighting.
				// Cycling through frames: sourceRectangle = asset.Frame(1, 6, 0, chainCount % 6);
				// This example shows how Flaming Mace works. It checks chainCount and changes chainTexture and draw color at different values

				var chainTextureToDraw = chainTexture;
				// Here, we draw the chain texture at the coordinates
				Main.spriteBatch.Draw(chainTextureToDraw.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

				// chainDrawPosition is advanced along the vector back to the player by the chainSegmentLength
				chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
				chainCount++;
				chainLengthRemainingToDraw -= chainSegmentLength;
			}

			// Add a motion trail when moving forward, like most flails do (don't add trail if already hit a tile)
			if (Projectile.ai[0] == 0)
			{
				Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
				Vector2 drawOrigin = new Vector2(projectileTexture.Width * 0.5f, Projectile.height * 0.5f);
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (Projectile.spriteDirection == -1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.spriteBatch.Draw(projectileTexture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length / 3, spriteEffects, 0f);
				}
			}
			return true;
		}
	}
}
