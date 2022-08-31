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

namespace excels.Items.Weapons.PumpkinMoon
{
    internal class WitchsCauldron : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Witch's Cauldron");
			Tooltip.SetDefault("The scalding brew can dish out a variety of debuffs");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 68;
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 40;
			Item.useTime = Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = 10000;
			Item.rare = 8;
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<WitchsBrew>();
			Item.shootSpeed = 12f;
			Item.noMelee = true;
			Item.sellPrice(0, 4, 55);

			clericEvil = true;
			clericBloodCost = 11;
			Item.knockBack = 5f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (var i = 0; i < 4; i++)
            {
				Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(11)) * Main.rand.NextFloat(0.75f, 1.15f), type, damage, knockback, player.whoAmI);
            }
			return false;
        }
    }

	public class WitchsBrew : clericHealProj
    {
        public override void SafeSetDefaults()
        {
			Projectile.width = Projectile.height = 16;
			Projectile.alpha = 70;
			Projectile.timeLeft = 200;
			Projectile.friendly = true;
			canDealDamage = true;
			clericEvil = true;
			Projectile.penetrate = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(SelectDebuff(), 360);
			target.AddBuff(SelectDebuff(), 240);
		}

		private int SelectDebuff()
        {
			switch (Main.rand.Next(16))
            {
				case 0:
				case 1:
				case 2:
					return BuffID.OnFire;
					break;

				case 3:
				case 4:
				case 5:
					return BuffID.Poisoned;
					break;

				case 6:
				case 7:
					return ModContent.BuffType<Buffs.Debuffs.Mycosis>();
					break;

				case 8:
				case 9:
					return BuffID.Frostburn;
					break;

				case 10:
				case 11:
					return BuffID.Oiled;
					break;

				case 12:
					return BuffID.Venom;
					break;

				case 13:
					return BuffID.CursedInferno;
					break;

				case 14:
					return BuffID.Ichor;
					break;

				case 15:
					return BuffID.ShadowFlame;
					break;
			}
			return BuffID.OnFire;
        }

        public override void AI()
        {
            if (++Projectile.ai[0] > 24 && Projectile.velocity.Y < 10)
            {
				Projectile.velocity.Y += 0.28f;
            }
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 33);
			d.color = Color.Lime;
			d.noGravity = true;
			d.velocity = -Projectile.velocity / 3;
			Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 33);
				d.color = Color.Lime;
				d.velocity *= Main.rand.NextFloat(1.1f, 1.5f);
				d.scale = Main.rand.NextFloat(1.1f, 1.5f);
				if (Main.rand.NextBool(3))
					d.noGravity = true;
				else
					d.velocity.Y -= 2;
			}
        }
    }
}
