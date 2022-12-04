using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using excels.Items.Weapons.Stellar;

namespace excels.Items.Tools.Sets.Stellar
{
    internal class StellarDrill : StellarWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellar Drill");
            Tooltip.SetDefault(NormTip);

            ItemID.Sets.IsDrill[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 64;

			Item.pick = 100;

			Item.damage = 14;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 0;
			Item.mana = 0;

			Item.useTime = 5;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.Item23;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;

			Item.shootSpeed = 32;
			Item.rare = ModContent.RarityType<StellarRarity>();
			Item.value = Item.sellPrice(0, 1, 75);
			Item.shoot = ModContent.ProjectileType<StellarDrillProj>();

			NormTip = "";
			PowerTip = "Set bonus: Creates mini Star-Bombs while being used";
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class StellarDrillProj : ModProjectile
	{
       // public override string Texture => "excels/Items/Tools/Stellar/StellarDrill";
        public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = 20;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.ownerHitCheck = true; //so you can't hit enemies through walls
			Projectile.DamageType = DamageClass.Melee;
		}
		int minibombTimer = 0;

        public override void AI()
        {
            if (Main.player[Projectile.owner].GetModPlayer<excelPlayer>().StellarSet)
            {
				if (++minibombTimer % 20 == 19)
                {
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(12)) * Main.rand.NextFloat(0.15f, 0.25f), ModContent.ProjectileType<StellarMinibomb>(), Projectile.damage * 3, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
                }
            }
        }
    }

	public class StellarMinibomb : ModProjectile
    {
		//public override string Texture => "excels/Items/Tools/Stellar/StellarDrill";
		public override void SetDefaults()
        {
			Projectile.scale = 0.5f;
			Projectile.width = Projectile.height = 12;
			Projectile.damage = 50;
			Projectile.timeLeft = 45;
			Projectile.friendly = true;
        }

        public override void AI()
        {
			if (Projectile.wet)
				Projectile.Kill();

			if (!Collision.InTileBounds((int)Projectile.position.X-4, (int)Projectile.position.Y-4, 20, 20, 20, 20) && Projectile.ai[0]==1)
            {
				Projectile.ai[0] = -1;
            }

			if (Projectile.ai[0] == -1)
            {
				Projectile.ai[0] = 0;
				Projectile.ai[1] = 0;
				Projectile.velocity.Y += 0.12f;
			}

			Dust d = Dust.NewDustPerfect(Projectile.Center, 292);
			d.noGravity = true;
			d.velocity *= 0;
			d.scale = 0.9f;

			// 64 is the sprite size (here both width and height equal)
			const int HalfSpriteWidth = 22 / 2;
			const int HalfSpriteHeight = 22 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

			if (Projectile.velocity.Length()!=0)
				Projectile.rotation = Projectile.velocity.ToRotation();
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.velocity *= 0;
			Projectile.ai[0] = 2;
			if (++Projectile.ai[1] > 45)
				Projectile.Kill();
			Projectile.timeLeft++;
			return false;
        }

        public override void Kill(int timeLeft)
        {
			for (var i = 0; i < 20; i++)
            {
				Dust d = Dust.NewDustPerfect(Projectile.Center, 292);
				d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f)*7;
				d.noGravity = true;
				d.scale = Main.rand.NextFromList(1.3f, 1.48f);
            }

			if (Projectile.ai[0] <= 0)
				return;

			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			int explosionRadius = 2;
			for (var i = 0; i < 2; i++)
			{
				if (Main.rand.NextBool(4))
					explosionRadius++;
			}
			int minTileX = (int)(Projectile.position.X / 16f - (float)explosionRadius);
			int maxTileX = (int)(Projectile.position.X / 16f + (float)explosionRadius);
			int minTileY = (int)(Projectile.position.Y / 16f - (float)explosionRadius);
			int maxTileY = (int)(Projectile.position.Y / 16f + (float)explosionRadius);
			if (minTileX < 0)
			{
				minTileX = 0;
			}
			if (maxTileX > Main.maxTilesX)
			{
				maxTileX = Main.maxTilesX;
			}
			if (minTileY < 0)
			{
				minTileY = 0;
			}
			if (maxTileY > Main.maxTilesY)
			{
				maxTileY = Main.maxTilesY;
			}
			bool canKillWalls = false;
			for (int x = minTileX; x <= maxTileX; x++)
			{
				for (int y = minTileY; y <= maxTileY; y++)
				{
					float diffX = Math.Abs((float)x - Projectile.position.X / 16f);
					float diffY = Math.Abs((float)y - Projectile.position.Y / 16f);
					double distance = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
					if (distance < (double)explosionRadius && Main.tile[x, y] != null && Main.tile[x, y].WallType == 0)
					{
						canKillWalls = true;
						break;
					}
				}
			}
			for (int i = minTileX; i <= maxTileX; i++)
			{
				for (int j = minTileY; j <= maxTileY; j++)
				{
					float diffX = Math.Abs((float)i - Projectile.position.X / 16f);
					float diffY = Math.Abs((float)j - Projectile.position.Y / 16f);
					double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
					if (distanceToTile < (double)explosionRadius)
					{
						bool canKillTile = true;
						if (Main.tile[i, j] != null)
						{
							canKillTile = true;
							if (Main.tileDungeon[(int)Main.tile[i, j].TileType] || Main.tile[i, j].TileType == 88 || Main.tile[i, j].TileType == 21 || Main.tile[i, j].TileType == 26 || Main.tile[i, j].TileType == 107 || Main.tile[i, j].TileType == 108 || Main.tile[i, j].TileType == 111 || Main.tile[i, j].TileType == 226 || Main.tile[i, j].TileType == 237 || Main.tile[i, j].TileType == 221 || Main.tile[i, j].TileType == 222 || Main.tile[i, j].TileType == 223 || Main.tile[i, j].TileType == 211 || Main.tile[i, j].TileType == 404)
							{
								canKillTile = false;
							}
							if (!Main.hardMode && Main.tile[i, j].TileType == 58)
							{
								canKillTile = false;
							}
							if (!TileLoader.CanExplode(i, j))
							{
								canKillTile = false;
							}
							if (canKillTile)
							{
								WorldGen.KillTile(i, j, false, false, false);
								if (Main.netMode != NetmodeID.SinglePlayer)
								{
									NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
								}
							}
						}
						if (canKillTile)
						{
							for (int x = i - 1; x <= i + 1; x++)
							{
								for (int y = j - 1; y <= j + 1; y++)
								{
									if (Main.tile[x, y] != null && Main.tile[x, y].WallType > 0 && canKillWalls && WallLoader.CanExplode(x, y, Main.tile[x, y].WallType))
									{
										WorldGen.KillWall(x, y, false);
										if (Main.tile[x, y].WallType == 0 && Main.netMode != NetmodeID.SinglePlayer)
										{
											NetMessage.SendData(MessageID.TileChange, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);
										}
									}
								}
							}
						}
					}
				}
			}
		}
    }
}
