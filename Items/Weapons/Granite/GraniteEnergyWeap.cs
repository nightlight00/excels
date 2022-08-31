using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;


namespace excels.Items.Weapons.Granite
{
    #region Energy Sword
    internal class EnergySword : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 18;
            Item.useTime = Item.useAnimation = 11;
            Item.UseSound = SoundID.Item15;
            Item.knockBack = 1.5f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.scale = 1.1f;
            Item.sellPrice(0, 0, 8, 50);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust d = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.EnergyDust>());
            d.noGravity = true;
            d.velocity = (d.position - player.position).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0.9f, 1.3f);
            if (d.velocity.X > 0)
                d.velocity = d.velocity.RotatedBy(MathHelper.ToRadians(90));
            else
                d.velocity = d.velocity.RotatedBy(MathHelper.ToRadians(-90));
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.EnergizedGranite>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    #endregion

    /*
    #region Energy Pickaxe
    internal class EnergyPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Can mine meteorite");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void SetDefaults()
        {
            Item.height = Item.width = 40;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 7;
            Item.useTime = Item.useAnimation = 8;
            Item.UseSound = SoundID.Item15;
            Item.knockBack = 0.9f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.pick = 50;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust d = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.EnergyDust>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.EnergizedGranite>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    #endregion
    */

    #region Seeking Discharge
    internal class EnergyStorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seeking Discharge");
            Tooltip.SetDefault("Conjures unstable charges of energy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 15;
            Item.useTime = Item.useAnimation = 16;
            Item.UseSound = SoundID.Item21;
            Item.knockBack = 1.1f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = 1;
            Item.mana = 12;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EnergyDischarge>();
            Item.shootSpeed = 8.3f;
            Item.noMelee = true;
            Item.sellPrice(0, 0, 9);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.EnergizedGranite>(), 8)
                .AddIngredient(ItemID.Book)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }

    internal class EnergyDischarge : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.alpha = 100;
            Projectile.timeLeft = 200;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] < 2)
            {
                Explosion();
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] < 2)
            {
                Explosion();
            }
        }

        private void Explosion()
        { 
            Projectile.alpha = 255;
            Projectile.velocity *= 0;
            Projectile.timeLeft = 2;
            Projectile.knockBack *= 1.45f;
            Projectile.tileCollide = false;
            Projectile.ai[0] = 2;
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 60;
            Projectile.Center = Projectile.position;
            for (var i = 0; i < 32; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 9, 9, ModContent.DustType<Dusts.EnergyDust>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                d.scale = Main.rand.NextFloat(1) / 4 + 1.2f;
                d.velocity = new Vector2(Main.rand.NextFloat(1.8f, 2.4f)).RotatedByRandom(MathHelper.ToRadians(180));
                d.noGravity = true;
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            for (var i = 0; i < 2; i++) 
            { 
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.EnergyDust>());
                d.velocity = -Projectile.velocity * 0.33f;
                d.scale = 1.2f;
            }

            if (Projectile.ai[0] == 0)
            {
                Vector2 targetPos = Vector2.Zero;
                float targetDist = 300;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];
                    if (npc.CanBeChasedBy(this, false))
                    {
                        float distance = Vector2.Distance(npc.Center, Projectile.Center);
                        if ((distance < targetDist) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                        {
                            targetDist = distance;
                            targetPos = npc.Center;
                            target = true;
                        }
                    }
                }
                if (target)
                {
                    Vector2 move = targetPos - Projectile.Center;
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (20 * Projectile.velocity + move); // / 5f;
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 8.3f / magnitude;
            }
        }
    }
    #endregion

    #region Granite Surge
    
    internal class GraniteSurge : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Explodes into restorative granite energy on enemy impact");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.width = Item.height = 70;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3.6f;
            Item.value = 10000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EnergySurge>();
            Item.shootSpeed = 8;
            Item.noMelee = true;

            clericEvil = true;
            clericBloodCost = 4;
            Item.sellPrice(0, 0, 6, 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.EnergizedGranite>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class EnergySurge : clericProj
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.alpha = 255;

            clericEvil = true;

        }

        public override void AI()
        {
            Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.EnergyDust>());
            d.noGravity = true;
            d.scale *= Main.rand.NextFloat(1.3f, 1.45f);
            d.velocity *= -Projectile.velocity * 0.08f;

            if (Main.rand.NextBool(3))
            {
                Dust d2 = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<Dusts.EnergyDust>());
                d.scale *= 0.87f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (var i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_None(), Projectile.Center, new Vector2(Main.rand.NextFloat(-3, 3), -5 - Main.rand.NextFloat(2)), ModContent.ProjectileType<EnergySpark>(), 1, 1, Main.player[Projectile.owner].whoAmI);
            }
        }
    }

    public class EnergySpark : clericHealProj
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.alpha = 255;

            clericEvil = true;
            canHealOwner = true;
            healPenetrate = 1;

            healPower = 3;
            healRate = 0;
        }

        public override void AI()
        {
            Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.EnergyDust>());
            d.noGravity = true;
            d.scale *= 1.12f;
            d.velocity *= 0;

            Projectile.velocity.Y += 0.2f;
            HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 28);
        }
    }

    #endregion

    #region Grock
    public class Grock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Converts all ammunitions to short ranged lasers");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 22;
            Item.useTime = Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.2f;
            Item.value = 10000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 1.2f;
            Item.useAmmo = AmmoID.Bullet;

            Item.UseSound = SoundID.Item11;
            Item.sellPrice(0, 0, 6, 80);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<GrockLaser>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 0f);
        }
    }

    public class GrockLaser : clericProj
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Fireball}";

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.timeLeft = 70;
            Projectile.friendly = true;
            Projectile.alpha = 255;

            Projectile.extraUpdates = 400;
            clericEvil = true;

        }

        public override void AI()
        {
            if (++Projectile.ai[0] < 5)
                return;

            Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.EnergyDust>());
            d.noGravity = true;
            d.scale *= Main.rand.NextFloat(1.3f, 1.45f);
            d.velocity = Projectile.velocity * 0.13f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (var i = 0; i < 24; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 9, 9, ModContent.DustType<Dusts.EnergyDust>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                d.scale = Main.rand.NextFloat(1) / 4 + 1.2f;
                d.velocity = new Vector2(Main.rand.NextFloat(1.2f, 2.4f)).RotatedByRandom(MathHelper.ToRadians(180));
                d.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (var i = 0; i < 24; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 9, 9, ModContent.DustType<Dusts.EnergyDust>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                d.scale = Main.rand.NextFloat(1) / 4 + 1.2f;
                d.velocity = new Vector2(Main.rand.NextFloat(1.2f, 2.4f)).RotatedByRandom(MathHelper.ToRadians(180));
                d.noGravity = true;
            }
            return base.OnTileCollide(oldVelocity);
        }
    }
   
    #endregion

}
