using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent;
using ReLogic.Content;
using excels.Utilities;

namespace excels.Items.Weapons.Stellar
{
    public abstract class StellarWeapon : ModItem
    {
        public override bool IsCloneable => true;

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.GetModPlayer<excelPlayer>().StellarDamageBonus;
            if (Item.type == ModContent.ItemType<StellarCommandRod>())
            {
                damage *= player.GetModPlayer<excelPlayer>().SpiritDamageMult;
            }
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += player.GetModPlayer<excelPlayer>().StellarCritBonus;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return 1 * player.GetModPlayer<excelPlayer>().StellarUseSpeed;
        }

        public string NormTip = ""; // "Strikes foes with high frequency energy";
        public string PowerTip = ""; // "\nSet bonus: Even higher frequency energy increases damage against defense";

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.Mod == "Terraria")
                {
                    if (line2.Name == "Tooltip0")
                    {
                        if (Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().StellarSet)
                        {
                            line2.Text = NormTip + PowerTip;
                        }
                        else
                        {
                            line2.Text = NormTip;
                        }
                    }
                }
            }
        }
    }

    #region Stellar Sword
    public class StellarEnergyBlade : StellarWeapon
    {
        public static Asset<Texture2D> glowmask;

        public override void Unload()
        {
            glowmask = null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellar Edge");
            Tooltip.SetDefault(NormTip);
            if (!Main.dedServ)
            {
                glowmask = ModContent.Request<Texture2D>(Texture + "_Glow");
            }
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 55;
            Item.useTime = Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.knockBack = 2.7f;
            Item.scale = 1.12f;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.width = Item.height = 56;
            Item.sellPrice(0, 0, 75);

            NormTip = ""; // "Strikes foes with high frequency energy";
            PowerTip = "Set bonus: Melee strikes generate stars that seek out foes";
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.BasicInWorldGlowmask(spriteBatch, glowmask.Value, new Color(255, 255, 255, 50) * 0.4f, rotation, scale);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (target.type == NPCID.TargetDummy || target.friendly || target.lifeMax <= 5)
                return;

            for (var i = 0; i < 2; i++)
            {
                Projectile p = Projectile.NewProjectileDirect(player.GetSource_FromThis(), target.position + new Vector2(Main.rand.Next(target.width), Main.rand.Next(target.height)), new Vector2(Main.rand.Next(-2, 2), -4.8f), ModContent.ProjectileType<StellarSwordStar>(), Item.damage / 2, Item.knockBack, player.whoAmI);
            }
        }
    }

    public class StellarSwordStar : ModProjectile
    {
        public override string Texture => "excels/Items/Tools/Sets/Stellar/StellarMinibomb";
        
        public override void SetDefaults()
        {
            Projectile.scale = 0.5f;
            Projectile.width = Projectile.height = 12;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return (Projectile.ai[0] > 30 && !target.friendly);
        }

        public override void AI()
        {
            if (Projectile.wet)
                Projectile.Kill();

            Dust d = Dust.NewDustPerfect(Projectile.Center, 292);
            d.noGravity = true;
            d.velocity *= 0;
            d.scale = 0.9f;

            if (++Projectile.ai[0] > 30)
            {
                Vector2 targetPos = Vector2.Zero;
                float targetDistance = 1000;
                bool target = false;
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC npc = Main.npc[k];
                    if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, Projectile.Center) < targetDistance)
                    {
                        targetDistance = Vector2.Distance(npc.Center, Projectile.Center);
                        targetPos = npc.Center;
                        target = true;
                    }
                }
                if (target)
                {
                    vel += 0.05f;
                    if (Projectile.timeLeft % 15 == 0)
                        turnAmount -= 1;

                    Vector2 move = targetPos - Projectile.Center;
                    AdjustMagnitude(ref move);
                    // the 22 is now much it 'turns'
                    Projectile.velocity = (turnAmount * Projectile.velocity + move) / 5f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }

            // 64 is the sprite size (here both width and height equal)
            const int HalfSpriteWidth = 22 / 2;
            const int HalfSpriteHeight = 22 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            // Vanilla configuration for "hitbox in middle of sprite"
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

            if (Projectile.velocity.Length() != 0)
                Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 292);
                d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f) * 7;
                d.noGravity = true;
                d.scale = Main.rand.NextFromList(1.3f, 1.48f);
            }
        }

        float vel = 2.4f;
        int turnAmount = 22;

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                // the 2 is the velocity
                vector *= vel / magnitude;
            }
        }
    }

    #endregion

    #region Cosmos Book
    public class StellarAlienSidearm : StellarWeapon
    {
        public override string Texture => "excels/Items/Weapons/Stellar/StellarHolobook";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Holobook of the Cosmos");
            Tooltip.SetDefault(NormTip);
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 24;
            Item.mana = 5;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.knockBack = 2.1f;
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StellarBookStar>();
            Item.shootSpeed = 7;
            Item.height = 24;
            Item.width = 36;
            Item.sellPrice(0, 0, 80);

            NormTip = "Conjures falling stars";
            PowerTip = "\nSet bonus: Even more stars fall from the sky";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int amt = 1;
            if (player.GetModPlayer<excelPlayer>().StellarSet)
                amt = 3;
            for (var i = 0; i < amt; i++)
            {
                float inaccuracy = (Main.MouseWorld.Y - (player.Center.Y - (Main.screenHeight / 2))) * 0.66f + 25;

                if (i > 0)
                {
                    type = ModContent.ProjectileType<StellarBookMiniStar>();
                    inaccuracy *= 1.25f;
                }              
                Vector2 pos = new Vector2(Main.MouseWorld.X + Main.rand.NextFloat(-inaccuracy, inaccuracy), player.Center.Y - (Main.screenHeight / 2));
                Projectile.NewProjectile(source, pos, (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2.8f, 3.2f), type, (int)(damage*((i==0)?1:0.66f)), knockback, player.whoAmI);
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }

    public class StellarBookStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 39;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 6;
            Projectile.width = 24;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 160;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] == 0)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

                Projectile.ai[0] = 1;
                Projectile.rotation = MathHelper.ToRadians(-90);
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 30;

                for (var i = 0; i < 16; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, 292);
                    d.scale = Main.rand.NextFloat(1.4f, 1.65f);
                    d.velocity = Main.rand.NextVector2Circular(0.25f, 0.25f) * 20;// (Vector2.One * Main.rand.NextFloat(2, 4.5f)).RotatedByRandom(MathHelper.ToRadians(180));
                    d.noGravity = true;
                }
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
                Projectile.rotation = MathHelper.ToRadians(-90);
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 30;
            }
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.alpha += 8;
                Projectile.scale += 0.1f;
                return;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool())
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 292);
                d.scale = 1.1f;
                d.velocity = Projectile.velocity * -0.33f;
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            // Redraw the projectile with the color not influenced by light
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k % 3 == 0)
                {
                    Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale*1.33f, SpriteEffects.None, 0);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            return true;
        }
    }

    public class StellarBookMiniStar : StellarBookStar
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 3;
            Projectile.width = Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 160;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] == 0)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

                Projectile.ai[0] = 1;
                Projectile.rotation = MathHelper.ToRadians(-90);
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 30;

                for (var i = 0; i < 16; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, 45);
                    d.scale = Main.rand.NextFloat(1.4f, 1.65f);
                    d.velocity = Main.rand.NextVector2Circular(0.25f, 0.25f) * 12;// (Vector2.One * Main.rand.NextFloat(2, 4.5f)).RotatedByRandom(MathHelper.ToRadians(180));
                    d.noGravity = true;
                    d.alpha = 170;
                }
            }
            return false;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.alpha += 8;
                Projectile.scale += 0.1f;
                return;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 45);
                d.scale = 1.1f;
                d.velocity = Projectile.velocity * -0.33f;
                d.noGravity = true;
                d.alpha = 170;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            // Redraw the projectile with the color not influenced by light
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k % 3 == 0)
                {
                    Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 1.5f, SpriteEffects.None, 0);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            return true;
        }
    }
    #endregion

    #region Stellar Cannon
    public class StellarBow : StellarWeapon
    {
        public override string Texture => "excels/Items/Weapons/Stellar/StellarCannon";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Startillery");
            Tooltip.SetDefault(NormTip);
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 52;
            Item.useTime = 11;
            Item.useAnimation = 22;
            Item.reuseDelay = 30;
            Item.autoReuse = true;
            Item.knockBack = 7.4f;
            Item.noMelee = true;
            Item.height = 38;
            Item.width = 24;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.useAmmo = AmmoID.Flare;
            Item.shoot = 10;
            Item.shootSpeed = 14.7f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item38;
            Item.sellPrice(0, 0, 82);

            NormTip = "Converts Flares into powerful rockets";
            PowerTip = "\nSet bonus: Rockets now create more explosions after death";
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<StellarRocketR>();
        }
    }
    public class StellarRocketR : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            // AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 26;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] == 1 && Projectile.ai[0] < 32)
                return false;

            return (!target.friendly);
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 1)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 292);
                d.noGravity = true;
                d.velocity *= 0;
                d.scale = 0.9f;

                Projectile.velocity.Y += 0.13f;
                if (++Projectile.ai[0] == 38)
                    Explode();
                return;
            }

            if (++Projectile.ai[0] > 8)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center - Projectile.velocity, 31);
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(0.2f, 0.2f);
                d.scale = 1.3f;
                d.alpha = 100;

                d = Dust.NewDustPerfect(Projectile.Center-Projectile.velocity, 292);
                d.noGravity = true;
                d.velocity *= 0;
                d.scale = 0.6f;
            }

            Projectile.velocity.Y += 0.08f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[1] == 0 && Projectile.timeLeft > 6)
                Explode();
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] == 1)
                return false;

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.33f;
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, 1, SpriteEffects.None, 0);
            }

            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[1] == 0 && Projectile.timeLeft > 6)
                Explode();
        }

        void Explode()
        {
            Projectile.alpha = 255;

            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 100;
            if (Projectile.ai[1] == 1)
                Projectile.width = Projectile.height = 40;
            Projectile.Center = Projectile.position;
            Projectile.velocity = Vector2.Zero;

            Projectile.timeLeft = 4;
            Projectile.velocity = Vector2.Zero;

            if (Projectile.ai[1] == 1){

                for (var i = 0; i < 16; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, 292);
                    d.scale = Main.rand.NextFloat(1.4f, 1.65f);
                    d.velocity = Main.rand.NextVector2CircularEdge(0.25f, 0.25f) * 12;// (Vector2.One * Main.rand.NextFloat(2, 4.5f)).RotatedByRandom(MathHelper.ToRadians(180));
                    d.noGravity = true;
                }

                for (var i = 0; i < 36; i++)
                {
                    Dust d2 = Dust.NewDustDirect(Projectile.Center - new Vector2(15, 15), 30, 30, 31);
                    d2.velocity = (Vector2.One * Main.rand.NextFloat(1.4f, 3f)).RotatedByRandom(MathHelper.ToRadians(180));
                    d2.noGravity = true;
                }

                //Projectile.Kill();
                return;
            }
            float distance = Vector2.Distance(Projectile.Center, Main.player[Projectile.owner].Center);
            if (distance < 800)
            {
                EffectsSystem.Shake.Set((int)(800f - distance) / 64f);
            }
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            for (var i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(25, 25), 50, 50, 292);
                d.scale = Main.rand.NextFloat(1.4f, 1.65f);
                d.velocity = Main.rand.NextVector2Circular(0.25f, 0.25f) * 25;// (Vector2.One * Main.rand.NextFloat(2, 4.5f)).RotatedByRandom(MathHelper.ToRadians(180));
                d.noGravity = true;
            }
            for (var i = 0; i < 50; i++)
            {
                Dust d2 = Dust.NewDustDirect(Projectile.Center - new Vector2(30, 30), 60, 60, 31);
                d2.velocity = (Vector2.One * Main.rand.NextFloat(2.8f, 5f)).RotatedByRandom(MathHelper.ToRadians(180));
                d2.noGravity = true;
            }

            for (var g = 0; g < 4; g++)
            {
                int Type = Main.rand.Next(3);
                switch (Type)
                {
                    case 1: Type = GoreID.Smoke1; break;
                    case 2: Type = GoreID.Smoke2; break;
                    default: Type = GoreID.Smoke3; break;
                }
                Gore gg = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(1.3f, 2), Main.rand.NextFloat(1.3f, 2)).RotatedBy(MathHelper.ToRadians(90 * g)), Type);
                gg.velocity = new Vector2(Main.rand.NextFloat(1.3f, 2), Main.rand.NextFloat(1.3f, 2)).RotatedBy(MathHelper.ToRadians(90 * g + Main.rand.Next(-20, 20)));
            }

            if (Main.player[Projectile.owner].GetModPlayer<excelPlayer>().StellarSet && Projectile.ai[1] == 0)
            {
                int dir = Main.rand.Next(320);
                for (var i = 0; i < 3; i++)
                {
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-2, -2.5f)), ModContent.ProjectileType<StellarRocketR>(), Projectile.damage / 2, Projectile.knockBack / 2, Main.player[Projectile.owner].whoAmI);
                    p.ai[1] = 1;
                }
            }

            //Projectile.Kill();
        }
    }
    #endregion

    #region Starship Command Rod
    internal class StellarCommandRod : StellarWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starship Command Rod");
            Tooltip.SetDefault(NormTip);
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 35;
            Item.mana = 10;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.knockBack = 3.5f;
            //Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StellarCommandShip>();
            Item.shootSpeed = 7;
            Item.height = Item.width = 30;
            Item.sellPrice(0, 0, 90);

            NormTip = "Summons a stellar ship for you to command";
            PowerTip = "\nSet bonus: The stellar ship additionally fires rocket volleys periodicaly";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void UpdateInventory(Player player)
        {
            if (player.ownedProjectileCounts[Item.shoot] >= 1)
            {
                Item.mana = 0;
                Item.channel = true;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            else
            {
                Item.mana = 10;
                Item.channel = false;
                Item.useStyle = ItemUseStyleID.Swing;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] >= 1)
            {
                return false;
            }
            return true;
        }
    }

    public class StellarCommandShip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //Item.staff[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 10;
            Projectile.width = 32;
            Projectile.height = 24;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
        }

        float rot = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity *= 0;
            if (player.HeldItem.type != ModContent.ItemType<StellarCommandRod>() || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            rot += 0.07f;
            Projectile.Center = player.Center;
            Projectile.position.X += MathF.Cos(rot) * 10 + Projectile.width / 4;
            Projectile.position.Y += -46 + MathF.Sin(rot) * 5;

            Projectile.ai[0] += (1 * player.GetModPlayer<excelPlayer>().SpiritAttackSpeed) * player.GetModPlayer<excelPlayer>().StellarUseSpeed;
            if (Projectile.ai[0] > 18)
            {
                if (Main.myPlayer == Main.player[Projectile.owner].whoAmI && Main.mouseLeft && !Main.LocalPlayer.mouseInterface && !player.mapFullScreen)
                {
                    // spawns on random x, confirmed y
                    int xx = Main.rand.Next((int)player.Center.X - Main.screenWidth / 2, (int)player.Center.X + Main.screenWidth / 2);
                    int yy = (int)player.Center.Y - Main.screenHeight / 2;
                    if (Main.rand.NextBool())
                    {
                        yy = (int)player.Center.Y + Main.screenHeight / 2;
                    }
                    // spawns on randomo y, confirmed x
                    if (Main.rand.NextBool())
                    {
                        yy = Main.rand.Next((int)player.Center.Y - Main.screenHeight / 2, (int)player.Center.Y + Main.screenHeight / 2);
                        xx = (int)player.Center.X - Main.screenWidth / 2;
                        if (Main.rand.NextBool())
                        {
                            xx = (int)player.Center.X + Main.screenWidth / 2;
                        }
                    }

                    for (var i = 0; i < 20; i++)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.TopLeft+new Vector2(8, 6), 292);
                        d.velocity = Main.rand.NextVector2Unit((float)MathHelper.Pi / 4, (float)MathHelper.Pi / 2).RotatedBy(MathHelper.ToRadians(180)) * 4;
                        d.noGravity = true;
                    }

                    Vector2 vel = (Main.MouseWorld - new Vector2(xx, yy)).SafeNormalize(Vector2.Zero) * 3;
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), new Vector2(xx, yy), vel, ModContent.ProjectileType<StellarLaser>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                    p.netUpdate = true;
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;
                    SoundEngine.PlaySound(SoundID.Item157, Projectile.Center);
                }
            }

            if (player.GetModPlayer<excelPlayer>().StellarSet)
            {
                Projectile.ai[1] += 1;// * player.GetModPlayer<excelPlayer>().StellarUseSpeed;
                if (Projectile.ai[1] > 240 && Projectile.ai[1]%5==0)
                {
                    if (Projectile.ai[1] > 260)
                    {
                        Projectile.ai[1] = 0;
                        return;
                    }

                    bool target = false;
                    int enemDanger = -1;

                    for (var i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.lifeMax > 5 && Vector2.Distance(Projectile.Center, npc.Center) < 2000 && npc.active && !npc.friendly && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                        {
                            int curDanger = (npc.lifeMax * (npc.defense / 2)) * npc.damage;
                            if (curDanger > enemDanger)
                            {
                                enemDanger = curDanger;
                                target = true;
                            }
                        }
                    }

                    if (target)
                    {
                        Vector2 vel = new Vector2(0, -7.9f).RotatedBy(MathHelper.ToRadians((Projectile.ai[1]%10==0)?45:-45)); //  (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * 9.7f;
                        Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<StellarCommandRocket>(), (int)(Projectile.damage * 2f), Projectile.knockBack * 3, player.whoAmI);
                        p.netUpdate = true;
                        Projectile.netUpdate = true;
                        SoundEngine.PlaySound(new SoundStyle("excels/Audio/WeaponKnock"), Projectile.Center);
                    }
                }
            }
        }
    }

    public class StellarLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 6;
            Projectile.width = Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 700;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
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
                Color color = Projectile.GetAlpha(Color.White);// * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 2; i++)
            {
                for (var p = 0; p < Projectile.oldPos.Length; p++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.oldPos[p], Projectile.width, Projectile.height, 292);
                    d.noGravity = true;
                    d.velocity = Projectile.velocity * 3;
                    d.scale = 0.9f;
                }
            }
        }
    }

    public class StellarCommandRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellar Rocket");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 150;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = Projectile.velocity.Length();
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            Vector2 targetPos = Vector2.Zero;
            float targetDistance = 1000;
            bool target = false;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, Projectile.Center) < targetDistance)
                {
                    targetDistance = Vector2.Distance(npc.Center, Projectile.Center);
                    targetPos = npc.Center;
                    target = true;
                }
            }
            if (target)
            {
                Vector2 move = targetPos - Projectile.Center;
                AdjustMagnitude(ref move);
                // the 15 is now much it 'turns'
                Projectile.velocity = (15 * Projectile.velocity + move) / 5f;
                AdjustMagnitude(ref Projectile.velocity);
            }

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
            d.velocity = -Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(7)) * Main.rand.NextFloat(0.23f, 0.35f);
            d.scale = Main.rand.NextFloat(1.24f, 1.34f);
            d.fadeIn = d.scale * Main.rand.NextFloat(1, 1.17f);
            d.noGravity = true;

            if (Main.rand.NextBool())
            {
                Dust d2 = Dust.NewDustPerfect(Projectile.Center + new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 6);
                d2.scale = 1.34f + Main.rand.NextFloat(0.5f);
                d2.noGravity = true;
                d2.velocity = -Projectile.velocity * 0.4f;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                // the 2 is the velocity
                vector *= Projectile.ai[0] / magnitude;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, 6);
                d.velocity = new Vector2(Main.rand.NextFloat(1.2f)).RotatedByRandom(MathHelper.ToRadians(180)) * Main.rand.NextFloat(2, 5);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.2f, 1.4f);

                d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                d.velocity = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)) * 3;
                d.noGravity = true;
                d.scale = 1.6f;
            }
        }
    }
    #endregion

    #region Starborn Staff
    public class StarbornStaff : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.width = Item.height = 42;
            Item.useTime = Item.useAnimation = 41;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3;
            Item.value = 10000;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StarbornStar>();
            Item.shootSpeed = 7.8f;
            Item.noMelee = true;

            clericRadianceCost = 7;
            healAmount = 7;
            healRate = 1;
            Item.sellPrice(0, 0, 80);
        }

        #region Ughh overlap 
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.GetModPlayer<excelPlayer>().StellarDamageBonus;
            if (Item.type == ModContent.ItemType<StellarCommandRod>())
            {
                damage *= player.GetModPlayer<excelPlayer>().SpiritDamageMult;
            }
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += player.GetModPlayer<excelPlayer>().StellarCritBonus;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return 1 * player.GetModPlayer<excelPlayer>().StellarUseSpeed;
        }

        public string NormTip = "Striking foes drops healing supply pods";//"Increases healing potency for each ally healed, and increases damage for each enemy hit"; // "Strikes foes with high frequency energy";
        public string PowerTip = "\nSet bonus: Supply pods temporarily augment offensive stats"; // "\nSet bonus: Even higher frequency energy increases damage against defense";
      
        public override void ExtraTooltipModifications(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.Mod == "Terraria")
                {
                    if (line2.Name == "Tooltip0")
                    {
                        if (Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().StellarSet)
                        {
                            line2.Text = NormTip + PowerTip;
                        }
                        else
                        {
                            line2.Text = NormTip;
                        }
                    }
                }
            }
        }
        #endregion
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
            return false;
        }

        public override float ExtraDamage()
        {
            return Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().StellarDamageBonus;
        }
    }

    public class StarbornStar : clericHealProj
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 22;
            Projectile.timeLeft = 76;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;

            healPenetrate = -1;
            canDealDamage = true;
            healPenetrate = 3;
            //buffConsumesPenetrate = true;
        }

        public override void AI()
        {
            HealCollision(Main.LocalPlayer, Main.player[Projectile.owner]);

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 292);
            d.velocity = Projectile.velocity * 0.8f;
            d.noGravity = true;

            Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.ToRadians(45);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.lifeMax > 5 && target.type != NPCID.TargetDummy && Projectile.ai[0] == 0)
            {
                CreateHealProjectile(Main.player[Projectile.owner], target.Center, new Vector2(4).RotatedByRandom(MathHelper.ToRadians(180)), ModContent.ProjectileType<StellarSupply>(), 0, 0, Projectile.GetGlobalProjectile<excelProjectile>().healStrength);
                Projectile.ai[0] = 1;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 292);
                d.noGravity = true;
                d.velocity *= Main.rand.NextFloat(1.2f, 1.6f);
                d.scale = Main.rand.NextFloat(1.1f, 1.3f);
                d.velocity += Projectile.velocity * 0.2f;
            }
        }
    }

    internal class StellarSupply : clericHealProj
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {

            Projectile.width = Projectile.height = 20;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;

            canHealOwner = true;
            healPenetrate = 1;
        }

        public override void AI()
        {
            Dust d = Dust.NewDustPerfect(Projectile.Center, 292);
            d.noGravity = true;
            d.velocity = Vector2.Zero;

            Projectile.rotation += Projectile.velocity.X * 2;
            Projectile.velocity.Y += 0.1f;
            Projectile.velocity.X *= 0.97f;

            HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 20);
        }

        public override void PostHealEffects(Player target, Player healer)
        {
            if (healer.GetModPlayer<excelPlayer>().StellarSet)
            {
                target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.Supplied>(), 300);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = (0f - oldVelocity.X) * 0.33f;
            }
            if (oldVelocity.Y != Projectile.velocity.Y)
            {
                Projectile.velocity.Y = (0f - oldVelocity.Y) * 0.33f;
            }

            return false;
        }

    }
    #endregion
}
