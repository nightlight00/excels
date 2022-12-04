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
using System.Collections.Generic;

namespace excels.Items.Weapons.Midnight
{
    internal class LunarReflection : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Reflection");
			Tooltip.SetDefault("Casts a shard of the moon to strike your foes\nThe moon shard returns, healing you proportionate to enemies hit"); // Conjures a lava geyser at the cursor \nTODO : Better attack = fires two firballs (like betsy wrath) that each create fiery auras on hit \n TODO (cont) : auras steal life from enemies");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			// aw shit this is kinda like thorium omen in hindsight, i didnt even mean to goddammit
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 26;
			Item.width = Item.height = 66;
			Item.useTime = Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.8f;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item88;
			Item.shootSpeed = 8.6f;
			Item.shoot = ModContent.ProjectileType<LunarSlash>();
			Item.noMelee = true;
			Item.sellPrice(0, 1, 95);

			clericRadianceCost = 7;
		}
	}

	public class LunarSlash : clericHealProj
    {
		public override void SetStaticDefaults()
		{
			//	Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 36;
			Projectile.timeLeft = 300;
			Projectile.alpha = 0; // 255;
			Projectile.friendly = true;
			Projectile.penetrate = -1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 22;
			Projectile.tileCollide = false;

			healPenetrate = 1;
			Projectile.GetGlobalProjectile<excelProjectile>().healRate = -1;
			canDealDamage = true;
			canHealOwner = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.damage = (int)(Projectile.damage * 0.9f);
			if (target.CanBeChasedBy())
			{
				if (++Projectile.ai[1] > 9)
					Projectile.ai[1] = 9;
			}
        }

		float speed;

        public override void AI()
        {
			Projectile.timeLeft = 2;

            if (++Projectile.ai[0] > 30)
            {
				if (Projectile.ai[0] == 31)
                {
					speed = MathF.Abs(Projectile.velocity.Length());
                }
				Projectile.velocity = (Main.player[Projectile.owner].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (-(30+speed) + Projectile.ai[0]);

				HealDistance(Main.player[Projectile.owner], Main.player[Projectile.owner], 30, false);
            }
			Projectile.rotation += MathHelper.ToRadians(15);
        }

		public override void PostHealEffects(Player target, Player healer)
		{
			if (Projectile.ai[1] > 0) {
				target.HealEffect((int)Projectile.ai[1], true);
				target.statLife += (int)Projectile.ai[1];

				if (target.statLife > target.statLifeMax2)
				{
					target.statLife = target.statLifeMax2;
				}
				NetMessage.SendData(66, -1, -1, null, target.whoAmI, Projectile.ai[1], 0f, 0f, 0, 0, 0);
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
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, 1, SpriteEffects.None, 0);
			}

			return true;
		}
	}
}
