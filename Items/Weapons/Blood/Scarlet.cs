using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.Blood
{
    public class ScarletScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Releases life-draining slashes");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 58;
            Item.useTime = Item.useAnimation = 43;
            Item.DamageType = DamageClass.Melee;
            Item.width = 70;
            Item.height = 72;
            Item.rare = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<ScarletSlash>();
            Item.shootSpeed = 9;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5);
                d.scale = Main.rand.NextFloat(1.1f, 1.4f);
                d.noGravity = true;
            }
        }
    }

    public class ScarletSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.width = Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
            Projectile.timeLeft = 72;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.lifeMax > 5 && !target.friendly)
            {
                Projectile.damage = (int)(Projectile.damage * 0.85f);
                if (target.type != NPCID.TargetDummy)
                {
                    Main.player[Projectile.owner].AddBuff(BuffID.SoulDrain, 240, true);
                }
                for (var i = 0; i < 7; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 182, Projectile.velocity.X, Projectile.velocity.Y);
                    d.scale = Main.rand.NextFloat(0.8f, 1f);
                    d.alpha = 130;
                    d.noGravity = true;
                }
            }
        }

        float rot = 16;
        public override void AI()
        {
            Projectile.velocity *= 0.97f;
            Projectile.alpha += 1;
            Projectile.rotation += MathHelper.ToRadians(rot);
            rot += 0.02f;
            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 182);
                d.scale = Main.rand.NextFloat(0.4f, 0.6f);
                d.alpha = 200;
                d.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 182);
                d.scale = Main.rand.NextFloat(0.9f, 1.1f);
                d.alpha = 100;
                d.noGravity = true;
            }
        }
    }


    internal class Hemorrhage : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Conjures sanguine rings that expand into blood clots \nBlood clots can be collected for health, or left alone to spill damaging blood drops");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 52;
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.width = Item.height = 42;
            Item.useTime = Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3.5f;
            Item.value = 10000;
            Item.rare = 4;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BloodyOrb>();
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.sellPrice(0, 2, 40);

            clericEvil = true;
            clericBloodCost = 12;
        }
    }

    public class BloodyOrb : clericHealProj
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.BloodShot}";

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 26;
            Projectile.alpha = 255;
            Projectile.timeLeft = 60;
            Projectile.friendly = true;

            canDealDamage = true; 
            clericEvil = true;
        }

        public override void AI()
        {
            for (var i = 0; i < 4; i++)
            {
                Vector2 pos = Projectile.Center + Main.rand.NextVector2CircularEdge(Projectile.width/2, Projectile.height/2);
                Vector2 vel = (pos - (Projectile.Center - (Projectile.velocity * 10))).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0.9f, 1.2f) * 3;
                Dust d = Dust.NewDustDirect(pos, 0, 0, 5);
                d.velocity = vel;
                d.noGravity = true;
                d.scale = 1.35f;
            }   
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                Vector2 pos = Main.rand.NextVector2Circular(Projectile.width / 4, Projectile.height / 4);
                Vector2 vel = pos * Main.rand.NextFloat(1.2f, 1.8f);
                Dust d = Dust.NewDustDirect(Projectile.Center + pos, 0, 0, 5);
                d.velocity = vel;
                d.noGravity = true;
                d.scale = 1.8f;
            }

            for (var i = 0; i < 8; i++)
            {
                Vector2 pos = Main.rand.NextVector2Circular(Projectile.width / 4, Projectile.height / 4);
                Vector2 vel = pos * Main.rand.NextFloat(1.4f, 2.1f);
                Dust d = Dust.NewDustDirect(Projectile.Center + pos, 0, 0, 5);
                d.velocity = vel;
                d.scale = 1.3f;
            }

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HemoClot>(), Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
        }
    }

    public class HemoClot : clericHealProj
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.alpha = 60;

            canHealOwner = true;
            healPower = 15;
        }

        public override void AI()
        {
            Projectile.GetGlobalProjectile<excelProjectile>().healStrength = healPower;
            HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 32);

            Projectile.timeLeft = 2;
            Projectile.ai[1]++;
            Projectile.scale = 1 + MathF.Sin(Projectile.ai[1] * 0.5f) / 15;

            if (++Projectile.ai[0] % 30 == 29)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height)), Main.rand.NextVector2CircularEdge(2,2)*1.4f, ModContent.ProjectileType<BloodDrop>(), (int)(Projectile.damage * 0.7f), 0, Main.player[Projectile.owner].whoAmI);
                healPower--;
            }

            if (healPower < 5)
                Projectile.Kill();
        }
    }
}
