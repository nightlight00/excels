using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.HealingTools.BuffTools.HardBar
{
    internal class HereticBreaker : ClericDamageItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Boosts ally's offensive abilities are improved and allow critical strikes to heal themselves");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			Item.width = Item.height = 42;
			Item.useTime = Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HereticBeam>();
			Item.shootSpeed = 2f;
			Item.noMelee = true;
			Item.mana = 10;
			Item.sellPrice(0, 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.TitaniumBar, 14)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class HereticBeam : clericHealProj
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

		public override void SafeSetDefaults()
		{
			Projectile.width = Projectile.height = 16;
			Projectile.timeLeft = 350;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 6;
			Projectile.penetrate = 3;
			//Projectile.usesIDStaticNPCImmunity
			buffConsumesPenetrate = true;
		}

        public override void AI()
        {
			if (++Projectile.ai[0] < 6*Projectile.extraUpdates)
				return;

			Dust d = Dust.NewDustPerfect(Projectile.Center, 156);
			d.noGravity = true;
			d.velocity = Projectile.velocity * 0.1f;
			d.scale = 1.125f;

			Dust ds1 = Dust.NewDustPerfect(Projectile.Center, 156);
			ds1.noGravity = true;
			ds1.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90)) * 0.55f;
			ds1.scale = 0.9f;

			Dust ds2 = Dust.NewDustPerfect(Projectile.Center, 156);
			ds2.noGravity = true;
			ds2.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-90)) * 0.55f;
			ds2.scale = 0.9f;
		}

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
				Dust d = Dust.NewDustPerfect(Projectile.Center, 156);
				d.noGravity = true;
				d.scale = 1;
				d.velocity = Main.rand.NextVector2Circular(3, 3);
            }
        }

        public override void BuffEffects(Player target, Player healer)
        {
			target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.HolySavagry>(), GetBuffTime(healer, 15));
        }
    }

}
