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
using System.Collections;
using System.Collections.Generic;

namespace excels.Items.Weapons.Midnight
{
    internal class BlazingMidNight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mid-Night Blaze");
            Tooltip.SetDefault("Swing around a powerful blade that creates illusionary slashes on hit \n'Would you please have this midnight waltz with me'");//'Would you walk this waltz with me'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Melee;
            Item.width = Item.height = 74;
            Item.useStyle = 1;
            Item.knockBack = 5.4f;
            Item.value = 100;
            Item.useTime = Item.useAnimation = 67;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.autoReuse = true;
          //  Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<MidnightBlazeSwing>();
            Item.shootSpeed = 3;
            Item.sellPrice(0, 3, 20);

            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                // add starlight sword later
                .AddIngredient(ItemID.MeteoriteBar, 9)
                .AddIngredient(ItemID.HellstoneBar, 9)
                .AddTile(ModContent.TileType<Tiles.Stations.StarlightAnvilTile>())
                .Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            for (var i = 0; i < Main.rand.Next(1, 4); i++)
            {
                Dust d = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
                d.noGravity = true;
                d.scale = 1.5f + Main.rand.NextFloat() * 0.7f;
                d.velocity *= Main.rand.NextFloat(1, 1.2f);
                if (Main.rand.NextBool(4))
                {
                    d.scale /= 2;
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void UseItemFrame(Player player)
        {
            // TODO : Make arm point towards blade
        }
    }

    public class MidnightBlazeSwing : ModProjectile
    {
        public override string Texture => "excels/Items/Weapons/Midnight/BlazingMidNight";

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 400;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 2;
            Projectile.localNPCHitCooldown = 15 * Projectile.extraUpdates;
        }

        float Direction = 0;
        bool Create = true;
        float Strength = 1;
        int mult = 1;
        
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            Projectile.ai[0]++;
            if (Projectile.ai[0] < (15 * Projectile.extraUpdates))
            {
                Projectile.ai[1] += (5.2f * Strength) / Projectile.extraUpdates;
                Strength -= 0.09f / Projectile.extraUpdates;
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= (255 / 7) / Projectile.extraUpdates;
                }
                //.Center = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) + new Vector2(Projectile.ai[1]).RotatedBy(Projectile.rotation);
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
                Direction = Projectile.velocity.ToRotation() - MathHelper.ToRadians(45);
                if (Projectile.Center.X > player.Center.X)
                {
                    mult = -1;
                }
            }
            else
            {
                if (Projectile.ai[0] == 15 * Projectile.extraUpdates)
                {
                    Strength = -0.4f * mult;
                }

                Projectile.velocity = Vector2.Zero;

                float dir = MathHelper.ToRadians((360 / 40) * Strength) / Projectile.extraUpdates;
                Direction += dir;
                Projectile.rotation += dir;

        //        Strength += 0.013f;

                if (Projectile.ai[0] > 50 * Projectile.extraUpdates)
                {
                    Projectile.alpha += (255 / 10) / Projectile.extraUpdates;
                    if (Projectile.alpha > 250)
                    {
                        Projectile.Kill();
                    }
                    Strength -= (0.094f * mult) / Projectile.extraUpdates;
                }
                else
                {
                    Strength += (0.03f * mult) / Projectile.extraUpdates;
                    if (Projectile.ai[0] > 25 * Projectile.extraUpdates)
                    {
                        Strength += (0.072f * mult) / Projectile.extraUpdates;
                    }
                }
            }
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) + new Vector2(Projectile.ai[1]).RotatedBy(Direction);

            
            for (var i = 0; i < 5; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + new Vector2(37 / 4 * i).RotatedBy(Projectile.rotation - MathHelper.ToRadians(90)), 6);
                d.noGravity = true;
                d.scale = (0.8f + (0.29f * i)) * (1 - Projectile.alpha / 255);
                d.velocity = Projectile.velocity * 0.6f;
            }
            
            SetVisualOffsets();
        }

        private void SetVisualOffsets()
        {
            // 32 is the sprite size (here both width and height equal)
            const int HalfSpriteWidth = 74 / 2;
            const int HalfSpriteHeight = 74 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            // Vanilla configuration for "hitbox in middle of sprite"
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }

        public float CollisionWidth => 10f * Projectile.scale;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // "Hit anything between the player and the tip of the sword"
            // shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
            Vector2 start = Projectile.Center;
            Vector2 end = start + new Vector2(Main.rand.Next(37)).RotatedBy(Projectile.rotation - MathHelper.ToRadians(90));
            float collisionPoint = 0f; // Don't need that variable, but required as parameter
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 480);

            float dist = (target.width + target.height) / 2 * 2f;
            if (dist < 70)
            {
                dist = 70;
            }
            Vector2 pos = target.Center + new Vector2(dist).RotatedByRandom(MathHelper.ToRadians(360));
            Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 0.2f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, vel, ModContent.ProjectileType<MidnightBlazeSlash>(), (int)(Projectile.damage * 0.7f), Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
        }
    }


    public class MidnightBlazeSlash : ModProjectile
    {
        public override string Texture => "excels/Items/Weapons/Midnight/BlazingMidNight";

        bool Creation = true;
        float Direction = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 80;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
         //   Projectile.hide = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 20)
            {
                Projectile.alpha += 255 / 15;
                if (Projectile.alpha > 250)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.velocity *= 1.236f;
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 255 / 8;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
            SetVisualOffsets();

            for (var i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + new Vector2(Main.rand.Next(37)).RotatedBy(Projectile.rotation - MathHelper.ToRadians(90)), 6);
                d.noGravity = true;
                d.scale = 1.2f;
                d.velocity = -Projectile.velocity / 2;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        private void SetVisualOffsets()
        {
            // 32 is the sprite size (here both width and height equal)
            const int HalfSpriteWidth = 74 / 2;
            const int HalfSpriteHeight = 74 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            // Vanilla configuration for "hitbox in middle of sprite"
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

            // Vanilla configuration for "hitbox towards the end"
            //if (Projectile.spriteDirection == 1) {
            //	DrawOriginOffsetX = -(HalfProjWidth - HalfSpriteWidth);
            //	DrawOffsetX = (int)-DrawOriginOffsetX * 2;
            //	DrawOriginOffsetY = 0;
            //}
            //else {
            //	DrawOriginOffsetX = (HalfProjWidth - HalfSpriteWidth);
            //	DrawOffsetX = 0;
            //	DrawOriginOffsetY = 0;
            //}
        }

        /*
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        */
    }
}
