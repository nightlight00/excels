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

namespace excels.Items.Weapons.Guns1
{
    #region Purple Haze
    class PurpleHaze : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses gel for ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Flamethrower);

            Item.damage = 28;
            Item.rare = ItemRarityID.Pink;
            Item.shootSpeed = 15f;
            Item.shoot = ModContent.ProjectileType<Inferno>();
            Item.height = 16;
            Item.width = 68;
            Item.sellPrice(0, 1, 30);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < player.itemAnimationMax - 4)
            {
                return false;
            }
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Musket)
                .AddIngredient(ItemID.Ichor, 8)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int InheritedType = type;
            type = Item.shoot;
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }

    class Inferno : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 30;
            Projectile.width = Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.penetrate = 4;
            Projectile.velocity *= 2f;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1; // 1 hit per npc max
            Projectile.localNPCHitCooldown = 15;
            WaterKills = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 1200);
            Projectile.damage = (int)(Projectile.damage * 0.88f);
            HitCheck(Main.player[Projectile.owner], target);
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 27)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dst = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 75, Projectile.velocity.X * 0.6f, Projectile.velocity.Y * 0.6f, 0, default(Color), 2.5f);
                    dst.noGravity = true;
                    dst.rotation += 0.2f;
                    dst.fadeIn = Main.rand.NextFloat(2.9f, 3.5f);
                    dst.scale = dst.fadeIn * 0.7f;
                }
                if (Main.rand.NextBool())
                {
                    Dust dst2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 75, 0, 0, 0, default(Color), 1.25f);
                    dst2.rotation += 0.2f;
                  //  dst2.velocity.X += Projectile.velocity.X / 5;
                }

            }
        }
    }
    #endregion

    #region Deadly Kiss ( formerly velvet maw )
    public class VelvetMaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deadly Kiss");
            Tooltip.SetDefault("Converts all ammunition into penetrating ichor shots\n'Tired of waiting'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 97;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 22;
            Item.useTime = Item.useAnimation = 33;
            Item.reuseDelay = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6.4f;
            Item.value = 10000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item11;
            Item.shoot = 10;
            Item.shootSpeed = 3;
            Item.useAmmo = AmmoID.Bullet;
            Item.scale = 1.1f;
            Item.crit = 9;
            Item.sellPrice(0, 1, 30);

            Item.UseSound = SoundID.Item36;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<IchorBlast>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TheUndertaker)
                .AddIngredient(ItemID.Ichor, 8)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -2);
        }

    }

    public class IchorBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 9;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 240);
        }

        public override void AI()        
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, new Vector3(5 / 255, 5 / 211, 0));

            const int HalfSpriteWidth = 14 / 2;
            const int HalfSpriteHeight = 2 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            // Vanilla configuration for "hitbox in middle of sprite"
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f); // - new Vector2(DrawOffsetX, DrawOriginOffsetY);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);          
            }
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return base.OnTileCollide(oldVelocity);
        }
    }

    #endregion

    #region Mean Greens
    public class MeanGreens : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mean Greens");
            Tooltip.SetDefault("33% chance to not consume ammo" +
                             "\nConverts ammunition into chlorophyte");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 22;
            Item.useTime = Item.useAnimation = 34;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = 10000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Bullet;
            Item.sellPrice(0, 2, 80);

            Item.UseSound = SoundID.Item36;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .33f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -2);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ProjectileID.ChlorophyteBullet;
            for (int i = 0; i < 4; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(24));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 14)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    #endregion

    #region Tommy Gun
    public class TommyGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tommy Gun");
            Tooltip.SetDefault("33% chance to not consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 22;
            Item.useTime = Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = 10000;
            Item.rare = 5;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Bullet;
            Item.sellPrice(0, 2, 10);

            Item.UseSound = SoundID.Item41;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .33f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 newSpeed = velocity * Main.rand.NextFloat(0.85f, 1.15f);
            newSpeed = newSpeed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(11)));
            Projectile.NewProjectile(source, position, newSpeed, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, 0);
        }
    }
    #endregion

    #region Flurry Hand-Cannon
    public class SnowballCannonEX : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flurry Hand-Cannon");
            Tooltip.SetDefault("50% chance to not consume ammo" +
                             "\n'Now you too can control a snowstorm from the comforts of your home!'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 22;
            Item.useTime = Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = 10000;
            Item.rare = 6;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.SnowBallFriendly;
            Item.shootSpeed = 13f;
            Item.useAmmo = AmmoID.Snowball;
            Item.sellPrice(0, 4, 30);

            Item.UseSound = SoundID.Item11;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .50f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (var i = 0; i < 1 + Main.rand.Next(3); i++)
            {
                Vector2 newSpeed = velocity * Main.rand.NextFloat(0.85f - (0.05f * i), 1.15f + (0.025f * i));
                newSpeed = newSpeed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(19 + (i * 3))));
                float scale = Main.rand.NextFloat(0.7f - (0.05f * i), 1.2f + (0.05f * i));
                Projectile p = Projectile.NewProjectileDirect(source, position, newSpeed, type, (int)(damage * scale), knockback * scale, player.whoAmI);
                p.scale = scale;
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -4);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SnowBlock, 40);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddTile(TileID.CrystalBall);
            recipe.ReplaceResult(ItemID.SnowGlobe);
            recipe.Register();
        }
    }
    #endregion

    /*
    public class GrenadePistol : ModItem
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.Revolver}";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Gives all ammunition explosive properties \n'The demolitionist's backup when bombs are not enough'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 37;
            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.height = 22;
            Item.width = 46;
            Item.knockBack = 1f;
            Item.rare = 1;
            Item.value = 100000;
            Item.shoot = 10;
            Item.shootSpeed = 6.4f;
            Item.scale = 0.9f;
            Item.UseSound = SoundID.Item41;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 0f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileID.ExplosiveBullet, damage, knockback, player.whoAmI, 0);
            return false;
        }
    }
    */
}
