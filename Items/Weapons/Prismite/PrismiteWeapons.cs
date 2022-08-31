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

namespace excels.Items.Weapons.Prismite
{
    #region Bow
    internal class PrismiteBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystalline Bow");
            Tooltip.SetDefault("Fires fragile crystal arrows");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 33;
            Item.knockBack = 2.6f;
            Item.useTime = Item.useAnimation = 27;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = 5;
            Item.shoot = 10;
            Item.shootSpeed = 9.75f;
            Item.UseSound = SoundID.Item5;
            Item.noMelee = true;
            Item.sellPrice(0, 1, 10);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2, 0f);
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (var i = 0; i < 2; i++)
            {
                Vector2 crystal = velocity.RotatedByRandom(MathHelper.ToRadians(10));
                Projectile.NewProjectile(source, position, crystal, ModContent.ProjectileType<PrismiteArrow>(), damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PearlwoodBow)
                .AddIngredient(ItemID.CrystalShard, 8)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    
    public class PrismiteArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.alpha = 100;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k % 2 == 0)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }
            }

            return true;
        }

        public override void AI()
        {
            int dType = 70;
            if (Main.rand.NextBool(3)) dType = 68;
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dType);
            d.noGravity = true;
            d.noLight = true;
            d.scale = Main.rand.NextFloat(0.95f, 1.2f);
            d.velocity *= 0.7f;
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            for (var i = 0; i < 2; i++)
            {
                Vector2 crystalSpeed = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.6f, 1.6f);
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, crystalSpeed, ProjectileID.CrystalShard, Projectile.damage / 2, 0, Main.player[Projectile.owner].whoAmI);
                p.penetrate = 3;
                p.usesLocalNPCImmunity = true;
                p.localNPCHitCooldown = 20; 
            }
            for (var i = 0; i < 12; i++)
            {
                int dType = 70;
                if (Main.rand.NextBool(3)) dType = 68;
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dType);
                d.noGravity = true;
                d.noLight = true;
                d.scale = Main.rand.NextFloat(1.1f, 1.5f);
                d.velocity *= 1.3f;
            }

        }
    }
    #endregion

    #region Sword
    public class PrismiteSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystalline Sword");
            Tooltip.SetDefault("Shatters on hit");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 52;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.height = Item.width = 40;
            Item.knockBack = 6.2f;
            Item.rare = 5;
            Item.value = 5000;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.sellPrice(0, 1, 20);
        }

        public override bool? UseItem(Player player)
        {
            Item.noMelee = false;
            Item.noUseGraphic = false;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PearlwoodSword)
                .AddIngredient(ItemID.CrystalShard, 10)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int dstType = 68;
            if (Main.rand.NextBool()) dstType = 70;
            Dust d = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dstType);
            d.noGravity = true;
            d.scale = Main.rand.NextFloat(0.9f, 1.2f);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Item27, player.Center);
            for (var i = 0; i < 3; i++)
            {
                Vector2 shootVel = target.Center - player.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= Main.rand.NextFloat(14, 18);
                shootVel = shootVel.RotatedByRandom(MathHelper.ToRadians(25));
                Projectile.NewProjectile(player.GetSource_FromThis(),
                    player.Center, shootVel, ModContent.ProjectileType<PrismiteShard>(), (int)(damage * 0.8f), knockBack / 2, player.whoAmI);
            }
            Item.noUseGraphic = true;
           // Item.noMelee = true;
        }
    }

    public class PrismiteShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CrystalShard);
            AIType = ProjectileID.CrystalShard;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        int dstType = 68; 

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.frame = Main.rand.Next(4);
                Projectile.ai[0]++;
                if (Main.rand.NextBool()) dstType = 70;
            }
            Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, dstType);
            d.noGravity = true;
            d.scale = Main.rand.NextFloat(0.9f, 1.2f) * Projectile.scale;
            d.velocity *= 0;
        }
    }
    #endregion

    #region Staff
    public class PrismiteStaff : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystalline Staff");
            Tooltip.SetDefault("Striking foes generates additional prismatic bolts");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.width = Item.height = 40;
            Item.useTime = Item.useAnimation = 16;
            Item.reuseDelay = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 10000;
            Item.rare = 5;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PrismiteBolt>();
            Item.shootSpeed = 2f;
            Item.noMelee = true;
            Item.crit = 5;

            clericRadianceCost = 5;
            Item.knockBack = 4;
            Item.sellPrice(0, 1, 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Pearlwood, 30)
                .AddIngredient(ItemID.CrystalShard, 8)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class PrismiteBolt : clericHealProj 
    {
        public override string Texture => "excels/Items/Weapons/Prismite/PrismiteShard";

        public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 500;
            Projectile.extraUpdates = 40;
            Projectile.width = Projectile.height = 14;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            canDealDamage = true;
        }

        int dstType = 68;

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dstType);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.3f, 1.6f);
                
            }
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.frame = Main.rand.Next(4);
                if (Main.rand.NextBool()) dstType = 70;
            }

            if (Projectile.ai[0] > 20 || Projectile.ai[1] == 1)
            {
                for (var i = 0; i < 2; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, dstType);
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(0.9f, 1.2f) * Projectile.scale;
                    d.velocity *= 0;
                }
            }

            Projectile.ai[0]++;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[1] == 0)
            {
                for (var i = 0; i < 3; i++)
                {
                    Vector2 pos = target.Center + new Vector2(64 * Main.rand.NextFloat(0.9f, 1.1f)).RotatedByRandom(MathHelper.ToRadians(360));
                    Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pos, vel, ModContent.ProjectileType<PrismiteBolt>(), (int)(Projectile.damage * 0.66f), Projectile.knockBack / 2, Main.player[Projectile.owner].whoAmI);
                    p.ai[1] = 1;
                    p.tileCollide = false;
                    p.timeLeft /= 3;
                }
            }
        }
    }


    #endregion

    #region Tome
    /*
    internal class PrismaticBolt : ModItem
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
            Item.useTime = Item.useAnimation = 17;
            Item.UseSound = SoundID.Item21;
            Item.knockBack = 0.8f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = 1;
            Item.mana = 7;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EnergyDischarge>();
            Item.shootSpeed = 8.3f;
            Item.noMelee = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PearlwoodBow)
                .AddIngredient(ItemID.CrystalShard, 8)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    */
    #endregion
}
