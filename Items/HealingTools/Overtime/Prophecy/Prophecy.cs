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
using excels.Buffs.HealOverTime;
using excels.Items.Materials.Ores;

namespace excels.Items.HealingTools.Overtime.Prophecy
{
	internal class Prophecy : ClericDamageItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Generates a wall of healing energy\nHealing is applied over time");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 30;
			Item.reuseDelay = 9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PropheticSurge>();
			Item.shootSpeed = 3.2f;
			Item.noMelee = true;
			Item.sellPrice(0, 1, 35);

			Item.mana = 25;
			healAmount = 12;
			healRate = 1;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (var i = 0; i < 5; i++)
            {
				Vector2 pos = position + new Vector2(8, (i * 16) - 32).RotatedBy(((Main.MouseWorld - position).SafeNormalize(Vector2.Zero).ToRotation()));
				CreateHealProjectile(player, source, pos, velocity.RotatedBy(MathHelper.ToRadians(i*5-10)), type, damage, knockback);
			}
			return false;
		}
	}

	public class PropheticSurge : clericHealProj
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}


		public override void SafeSetDefaults()
        {
			Projectile.width = Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 200;
			Projectile.extraUpdates = 2;

			clericEvil = false;
			healPenetrate = 2;
			buffConsumesPenetrate = true;

			canHealOwner = false;
        }

        public override void AI()
        {
			BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 14);
			Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.ToRadians(45);

			if (Main.rand.NextBool(3)) { 
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 220);
				d.velocity = Projectile.velocity * 0.8f;
				d.scale = 0.9f;
				d.noGravity = true;
			}
        }

        public override void BuffEffects(Player target, Player healer)
        {
			target.GetModPlayer<HealOverTime>().AddHeal(healer, "Prohecy", 3, 4);
        }

        public override void Kill(int timeLeft)
        {
			for (var i = 0; i < 20; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 220);
				d.scale = Main.rand.NextFloat(0.9f, 1.15f);
				if (Main.rand.NextBool(3))
				{
					d.noGravity = true;
					d.scale *= 1.1f; 
				}
				if (!Main.rand.NextBool(4))
				{
					d.velocity += (Projectile.velocity * Main.rand.NextFloat(1.2f, 1.6f));
				}
			}
        }

		public override bool PreDraw(ref Color lightColor) // thumbs up!!!!
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				var offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
				var frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + offset;
				Color color = new Color(220, 126, 255, 255) * (1f - Projectile.alpha) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, frame, color, Projectile.oldRot[k], frame.Size() / 2, (k==0)?1:0.8f, SpriteEffects.None, 0);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

			return false;
		}
	}
}
