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
using excels.Items.Materials.Ores;

namespace excels.Items.Weapons.Hyperion
{

    #region Throwing Axe
    internal class HyperionAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyperion Throwing Axe");
            Tooltip.SetDefault("33% chance to not be consumed on use");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.damage = 49;
            Item.knockBack = 3.5f;
            Item.rare = 4;
            Item.UseSound = SoundID.Item71;
            Item.shoot = ModContent.ProjectileType<HyperionAxeProj>();
            Item.shootSpeed = 4f;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.sellPrice(0, 0, 0, 25);
        }

        public override bool ConsumeItem(Player player)
        {
            return Main.rand.NextFloat() >= 0.33f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(9));
        }

        public override void AddRecipes()
        {
            CreateRecipe(150)
                .AddIngredient(ModContent.ItemType<DarksteelBar>())
                .AddIngredient(ModContent.ItemType<HyperionCrystal>(), 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class HyperionAxeProj : ModProjectile
    {
        public override string Texture => "excels/Items/Weapons/Hyperion/HyperionAxe";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 21;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 26;
            Projectile.timeLeft = 400;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.extraUpdates = 3;
            Projectile.friendly = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Weapons/Hyperion/HyperionAxeShadow").Value;

            // Redraw the projectile with the color not influenced by light
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k % 3 == 0)
                {
                    Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = (Color.White * 0.66f) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, 1, SpriteEffects.None, 0);
                }
            }

            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Weapons/Hyperion/HyperionAxe_Glow").Value;
            Main.EntitySpriteDraw(texture, Projectile.position, null, Color.White * 0.66f, Projectile.rotation, new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f), 1, SpriteEffects.None, 0);
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X) * 2.7f;
            Lighting.AddLight(Projectile.Center, new Vector3(255 / 124, 255 / 255, 255 / 234) / 3);

            if (++Projectile.ai[0] > 60)
            {
                Projectile.velocity.Y += 0.05f;
                Projectile.velocity.X *= 0.995f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            for (var i = 0; i < 20; i++)
            {
                int type = ModContent.DustType<Dusts.HyperionMetalDust>();
                if (Main.rand.NextBool(4))
                    type = ModContent.DustType<Dusts.HyperionEnergyDust>();
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, type);
                d.scale *= Main.rand.NextFloat(1, 1.1f);
                d.velocity = (Projectile.velocity / 2) + new Vector2(Main.rand.NextFloat(0.33f, 1)).RotatedByRandom(MathHelper.ToRadians(180));
                if (Main.rand.NextBool(3))
                    d.noGravity = true;
            }
            return true;
        }
    }
    #endregion

    #region Longsword
    internal class HyperionLongsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Striking down foes lunges you forwards\nWhile lunging, you are immune to damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Weapons/Hyperion/HyperionLongsword_Glow").Value;
            Main.EntitySpriteDraw(texture, Item.position, null, Color.White * 0.66f, rotation, new Vector2(texture.Width * 0.5f, Item.height * 0.5f), 1, SpriteEffects.None, 0);
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 40;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.damage = 62;
            Item.knockBack = 1.4f;
            Item.rare = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HyperionLunge>();
            Item.sellPrice(0, 1, 90);
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Lighting.AddLight(hitbox.Center(), new Vector3(255 / 124, 255 / 255, 255 / 234) / 3);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        { 
            if (target.life <= 0)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, (target.Center - player.Center).SafeNormalize(Vector2.Zero) * 15, ModContent.ProjectileType<HyperionLunge>(), (int)(Item.damage * 1.5f), 4.7f, player.whoAmI);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DarksteelBar>(), 12)
                .AddIngredient(ModContent.ItemType<HyperionCrystal>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class HyperionLunge : ModProjectile
    {
        public override string Texture => "excels/Items/Weapons/Hyperion/HyperionLongsword";

        protected virtual float HoldoutRangeMin => 20f;
        protected virtual float HoldoutRangeMax => 50f;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 20;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.active)
                Projectile.Kill();

            player.immune = true;
            player.immuneTime = 20;
            player.immuneNoBlink = true;
            player.heldProj = Projectile.whoAmI;
      //      player.itemTime = player.itemAnimation = 2;
            player.velocity = Projectile.velocity;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

            player.direction = 1;
            Projectile.spriteDirection = 1;
            if (Projectile.velocity.X < 0)
            {
                player.direction = -1;
                Projectile.spriteDirection = -1;
                Projectile.rotation += MathHelper.ToRadians(90);
            }

            // Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
            Projectile.Center = player.MountedCenter + (Projectile.velocity * 1.3f);

            Projectile.ai[1]++;
            Lighting.AddLight(Projectile.Center, new Vector3(255 / 124, 255 / 255, 255 / 234) / 3);
        }
    }
    #endregion

    #region Power Drain
    public class HyperionDrain : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Power Drain");
            Tooltip.SetDefault("Grants Energy Flow to allies and even yourself on enemy hits\nPower Drain becomes overcharged while you have Energy Flow");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.width = Item.height = 34;
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.useTime = Item.useAnimation = 14;
            Item.knockBack = 2;
            Item.damage = 56;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<HyperionDrainWave>();
            Item.shootSpeed = 4.2f;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;

            clericRadianceCost = 3;
            Item.sellPrice(0, 1, 80);
        }

        public override void UpdateInventory(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.ClericBonus.EnergyFlowBuff>()))
            {
                Item.useTime = Item.useAnimation = 14;
            }
            else
            {
                Item.useTime = Item.useAnimation = 21;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.ClericBonus.EnergyFlowBuff>()))
            {
                for (var i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(14)) * (0.9f + (0.1f * i)), type, damage, knockback, player.whoAmI);
                }
                return false;
            }

            Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(9)) * 0.95f, type, damage, knockback, player.whoAmI);
            
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DarksteelBar>(), 8)
                .AddIngredient(ModContent.ItemType<HyperionCrystal>(), 18)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class HyperionDrainWave : clericHealProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 200;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 100;
            Projectile.alpha = 70;
            Projectile.friendly = true;
            Projectile.extraUpdates = 2;

            canDealDamage = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(BuffID.Electrified, 240);
            else
                target.AddBuff(BuffID.Electrified, 60);

            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), target.Center, -Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(40))*0.5f, ModContent.ProjectileType<HyperionLifeDrain>(), 0, 0, Main.player[Projectile.owner].whoAmI);
            p.ai[0] = Main.player[Projectile.owner].whoAmI;
            p.ai[1] = 2;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.HyperionEnergyDust>());
                d.velocity = Projectile.velocity * Main.rand.NextFloat(1.2f, 1.4f);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.2f, 1.45f);
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(255 / 124, 255 / 255, 255 / 234) / 3);
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool())
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.HyperionEnergyDust>());
                d.velocity = -Projectile.velocity * Main.rand.NextFloat(0.7f, 0.9f);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.15f, 1.3f);
            }

            BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 20);
        }

        public override void BuffEffects(Player target, Player healer)
        {
            target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.EnergyFlowBuff>(), GetBuffTime(healer, 4));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Weapons/Hyperion/HyperionDrainWave").Value;

            // Redraw the projectile with the color not influenced by light
            float scale = 0.9f;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = (Color.White * 0.66f) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
                scale -= 0.3f / 16;
            }

            return true;
        }

    }

    public class HyperionLifeDrain : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FireArrow}";

        public override void SetDefaults()
        {
            Projectile.timeLeft = 1000;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            int num492 = (int)Projectile.ai[0];
            if (Projectile.ai[0] == -1)
            {
                for (int k = 0; k < 200; k++)
                {
                    Player player = Main.player[k];
                    if ((player.statLife < Main.player[num492].statLife) && !player.dead && player.active && player.statLife > 0)
                    {
                        num492 = player.whoAmI;
                    }
                }
            }

            float num493 = 4f;
            Vector2 vector39 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
            float num494 = Main.player[num492].Center.X - vector39.X;
            float num495 = Main.player[num492].Center.Y - vector39.Y;
            float num496 = (float)Math.Sqrt((double)(num494 * num494 + num495 * num495));
            if (num496 < 50f && Projectile.position.X < Main.player[num492].position.X + (float)Main.player[num492].width && Projectile.position.X + (float)Projectile.width > Main.player[num492].position.X && Projectile.position.Y < Main.player[num492].position.Y + (float)Main.player[num492].height && Projectile.position.Y + (float)Projectile.height > Main.player[num492].position.Y)
            {
                if (Projectile.owner == Main.myPlayer && !Main.player[Main.myPlayer].moonLeech)
                {
                    int num497 = (int)Projectile.ai[1];
                    Player player = Main.player[num492];

                    player.AddBuff(ModContent.BuffType<Buffs.ClericBonus.EnergyFlowBuff>(), 180);
                }
                Projectile.Kill();
            }
            num496 = num493 / num496;
            num494 *= num496;
            num495 *= num496;
            Projectile.velocity.X = (Projectile.velocity.X * 15f + num494) / 16f;
            Projectile.velocity.Y = (Projectile.velocity.Y * 15f + num495) / 16f;

            Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.HyperionEnergyDust>());
            d.velocity = Vector2.Zero;
            d.noGravity = true;
            d.scale = 1.2f;
        }
    }
    #endregion

    #region Assault Rifle
    public class HyperionRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyperion Shock Rifle"); // vindicate
            Tooltip.SetDefault("50% chance to not consume ammo\nFires high-frequency rounds");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 12;
            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.height = 26;
            Item.width = 62;
            Item.knockBack = 4.2f;
            Item.rare = 4;
            Item.value = 5000;
            Item.shoot = 10;
            Item.shootSpeed = 11;
            Item.UseSound = SoundID.Item11;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.sellPrice(0, 2);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ModContent.ProjectileType<HyperionBolt>();
            Vector2 newSpeed = velocity * Main.rand.NextFloat(0.95f, 1.06f);
            newSpeed = newSpeed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(3)));
            Projectile.NewProjectile(source, position, newSpeed, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 0f);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.50f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DarksteelBar>(), 14)
                .AddIngredient(ModContent.ItemType<HyperionCrystal>(), 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class HyperionBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.NanoBullet);
            AIType = ProjectileID.NanoBullet;
            Projectile.extraUpdates = 3;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Electrified, 150);
        }

        int dust = 0;

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (++dust > 4)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 226);
                d.velocity = Projectile.velocity * 0.25f;
                d.noGravity = true;
                d.scale = 0.7f;
            }
        }
    }
    #endregion

    #region Lamp Sentry
    internal class HyperionLampPost : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a sentry\nSummons hovering lamps that fires bursts of brilliant bouncing bolts");

            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 30;
            Item.shoot = ModContent.ProjectileType<HyperionLamp>();
            Item.rare = 4;
            Item.damage = 33;
            Item.knockBack = 3;
            Item.sentry = true;
            Item.DamageType = DamageClass.Summon;
            Item.UseSound = SoundID.Item46;
        }

        public override bool CanUseItem(Player player)
        {
            return !Collision.SolidTiles(Main.MouseWorld - new Vector2(16), 32, 32) && Collision.CanHitLine(player.position, player.width, player.height, Main.MouseWorld, 1, 1); ; 
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
            Projectile.NewProjectile(source, Main.MouseWorld.X, Main.MouseWorld.Y, 0f, 0f, type, damage, 0, Main.myPlayer);

            player.UpdateMaxTurrets();

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DarksteelBar>(), 10)
                .AddIngredient(ModContent.ItemType<HyperionCrystal>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    internal class HyperionLamp : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 26;
            Projectile.height = 42;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 8;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.sentry = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
        }

        public override bool? CanHitNPC(NPC target) => false;

        bool target;
        Vector2 targetPos;
        int burstFire = 0;

        public override void OnSpawn(IEntitySource source)
        {
            for (var ii = 0; ii < 26; ii++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(8, 4), 16, 8, ModContent.DustType<Dusts.HyperionEnergyDust>());
                d.noGravity = true;
                d.noLight = true;
                d.velocity = new Vector2(Main.rand.NextFloat(2.2f, 2.8f) * ((ii % 2 == 0) ? -1 : 1), Main.rand.NextFloat(1, -1));
                d.scale = Main.rand.NextFloat(1.4f, 1.7f);
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(255 / 124, 255 / 255, 255 / 234) * 1.25f);

            Projectile.ai[1]++;
            burstFire--;

            // Main projectile attack
            if (!target)
            {
                if (++Projectile.ai[0] > 60)
                {
                    target = false;
                    targetPos = Vector2.Zero;
                    float distance = 700;
                    if (Main.player[Projectile.owner].HasMinionAttackTargetNPC)
                    {
                        NPC npc = Main.npc[Main.player[Projectile.owner].MinionAttackTargetNPC];
                        if (npc.CanBeChasedBy() && Vector2.Distance(Projectile.Center, npc.Center) < distance && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                        {
                            target = true;
                            targetPos = npc.Center;
                            distance = Vector2.Distance(Projectile.Center, npc.Center);
                            burstFire = 21;
                        }
                    }
                    else
                    {
                        for (var i = 0; i < Main.maxNPCs; i++)
                        {
                            if (Main.npc[i].CanBeChasedBy())
                            {
                                if (Vector2.Distance(Projectile.Center, Main.npc[i].Center) < distance && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
                                {
                                    target = true;
                                    targetPos = Main.npc[i].Center;
                                    distance = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
                                    burstFire = 21;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (burstFire % 7 == 0)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Vector2 shootVel = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * 4.3f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel.RotatedByRandom(MathHelper.ToRadians(7)), ModContent.ProjectileType<HyperLampBolt>(), Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
                    }
                    for (var i = 0; i < 15; i++)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.HyperionEnergyDust>());
                        d.velocity = Main.rand.NextVector2Unit((float)MathHelper.Pi / 2, (float)MathHelper.Pi / 4).RotatedBy((targetPos-Projectile.Center).SafeNormalize(Vector2.Zero).ToRotation() - MathHelper.ToRadians(112.5f)) * Main.rand.NextFloat(4.2f, 5.3f);
                        d.noGravity = true;
                        d.scale = Main.rand.NextFloat(1.4f, 1.7f);
                    }
                    for (var ii = 0; ii < 10; ii++)
                    {
                        Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(8, 4), 16, 8, ModContent.DustType<Dusts.HyperionEnergyDust>());
                        d.noGravity = true;
                        d.noLight = true;
                        d.velocity = new Vector2(Main.rand.NextFloat(1.3f, 2.5f)*((ii % 2 == 0) ? -1:1), Main.rand.NextFloat(1, -1));
                    }

                    SoundEngine.PlaySound(SoundID.Item84, Projectile.Center);
                    Projectile.netUpdate = true;
                }
                if (burstFire <= 0)
                {
                    target = false;
                    Projectile.ai[0] = 0;
                }
            }

            Projectile.rotation = MathHelper.ToRadians(MathF.Sin(Projectile.ai[1] / 7) * 5);
            // Secondary attack that creates a light aura around the lantern
            // Used primarily to hit 
        }
    }

    internal class HyperLampBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.extraUpdates = 2;
            Projectile.scale = 0.5f;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 500;
            Projectile.alpha = 100;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, new Vector3(255 / 124, 255 / 255, 255 / 234) * 0.85f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Electrified, 240);
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(6, 6), 12, 12, ModContent.DustType<Dusts.HyperionEnergyDust>());
                d.noGravity = true;
                d.velocity = Projectile.velocity * Main.rand.NextFloat(1.6f, 1.8f);
                d.scale = Main.rand.NextFloat(1.6f, 1.9f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.X != Projectile.velocity.X)
                Projectile.velocity.X = (0f - oldVelocity.X);

            if (oldVelocity.Y != Projectile.velocity.Y)
                Projectile.velocity.Y = (0f - oldVelocity.Y);

            Collision.HitTiles(Projectile.position, Projectile.oldVelocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(projectileTexture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects spriteEffects = SpriteEffects.None;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(projectileTexture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length / 3, spriteEffects, 0f);
            }

            return false;
        }
    }
    #endregion

}
