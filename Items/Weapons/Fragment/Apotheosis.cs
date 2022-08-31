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

namespace excels.Items.Weapons.Fragment
{
    internal class Apotheosis : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Conducts the final dance of twilight before oblivion'");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 53;
            Item.useTime = Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.height = Item.width = 40;
            Item.knockBack = 6.2f;
            Item.rare = 10;
            Item.value = 5000;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<ApotheosisLaser>();
            Item.shootSpeed = 1;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.sellPrice(0, 10);

            clericEvil = true;
            clericBloodCost = 25;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<ApotheosisHoldOut>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<ApotheosisLaser2>(), damage * 2, knockback, player.whoAmI);
            for (var i = 0; i < 8; i++)
            {
                Projectile p = Projectile.NewProjectileDirect(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
                p.ai[1] = i * (360 / 8);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.BlackholeFragment>(), 18)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class ApotheosisHoldOut : ModProjectile
    {
        public override string Texture => "excels/Items/Weapons/Fragment/Apotheosis";
        public override string GlowTexture => "excels/Items/Weapons/Fragment/Apotheosis_Glow";

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.width = Projectile.height = 64;
        }

        public override bool? CanDamage() => false;

        public override bool? CanCutTiles() => false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.channel)
            {
                Projectile.Kill();
            }
            player.heldProj = Projectile.whoAmI;
            player.itemAnimation = player.itemTime = 2;
            Projectile.timeLeft = 2;

            const int HalfSpriteWidth = 48;
            const int HalfSpriteHeight = 48;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            // Vanilla configuration for "hitbox in middle of sprite"
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

            float rot = 0;
            if (Main.myPlayer == Main.player[Projectile.owner].whoAmI && !player.mapFullScreen)
            {
                Projectile.rotation = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).ToRotation() + MathHelper.ToRadians(45);
                rot = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).ToRotation() - MathHelper.ToRadians(90);
            }
            Projectile.Center = player.Center + new Vector2(48).RotatedBy(rot + MathHelper.ToRadians(45));
        }
    }
    
    public class HolySteal : clericHealProj
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FireArrow}";

        public override void SafeSetDefaults()
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
                    Player healer = Main.player[Projectile.owner];

                    num497 = (int)Math.Round((double)num497);
                    Main.player[num492].HealEffect(num497, false);
                    player.statLife += num497;
                    if (Main.player[num492].statLife > Main.player[num492].statLifeMax2)
                    {
                        Main.player[num492].statLife = Main.player[num492].statLifeMax2;
                    }
                    NetMessage.SendData(66, -1, -1, null, num492, (float)num497, 0f, 0f, 0, 0, 0);
                    
                }
                Projectile.Kill();
            }
            num496 = num493 / num496;
            num494 *= num496;
            num495 *= num496;
            Projectile.velocity.X = (Projectile.velocity.X * 15f + num494) / 16f;
            Projectile.velocity.Y = (Projectile.velocity.Y * 15f + num495) / 16f;

            Dust d = Dust.NewDustPerfect(Projectile.Center, 204);
            d.velocity = Vector2.Zero;
            d.noGravity = true;
            d.scale = 1.2f;
        }
    }
    
    public class ApotheosisLaser2 : clericHealProj
    {
        public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 500;
            Projectile.width = Projectile.height = 24;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            canDealDamage = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] < 160)
                return false;

            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Weapons/Fragment/ApotheosisLaserBeam2").Value;

            for (float i = 8; i <= 800; i += 8)
            {
                Color c = Projectile.GetAlpha(lightColor);
                var origin = Projectile.Center + i * Projectile.velocity;
                Vector2 pos = (Projectile.position + (new Vector2(i).RotatedBy(Projectile.rotation + MathHelper.ToRadians(45))) - Main.screenPosition) + new Vector2(Projectile.width / 2, Projectile.height / 2);
                Main.EntitySpriteDraw(texture, pos, null, c, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(600).RotatedBy(Projectile.rotation + MathHelper.ToRadians(45)));
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] > 160)
                return true;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), target.Center, new Vector2(10).RotatedByRandom(360), ModContent.ProjectileType<HolySteal>(), 1, 1, Main.player[Projectile.owner].whoAmI);
            p.ai[0] = Main.player[Projectile.owner].whoAmI;
            p.ai[1] = 3;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.channel)
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 2;

            if (Projectile.ai[0] > 120)
            {
                Projectile.alpha -= 1;
                if (Projectile.alpha < 100)
                {
                    Projectile.alpha = 100;
                }
            }
            Projectile.ai[0]++;

            if (++Projectile.ai[1] % 20 == 0)
            {
                player.statLife -= 5;
                CombatText.NewText(player.getRect(), Color.Red, 5);
                CheckSkullPendant(player, 5);
                if (player.statLife <= 0)
                {
                    player.KillMe(PlayerDeathReason.ByPlayer(player.whoAmI), 5, 0);
                }
            }

            Lighting.AddLight(Projectile.Center, new Vector3(2.55f, 2.55f, .76f) * 0.33f);

            float rot = 0;

            if (Main.myPlayer == Main.player[Projectile.owner].whoAmI && !player.mapFullScreen)
            {
                rot = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).ToRotation() - MathHelper.ToRadians(90);
            }
            Projectile.Center = player.Center + new Vector2(64).RotatedBy(rot + MathHelper.ToRadians(45));

            Projectile.rotation = (Projectile.Center - player.Center).SafeNormalize(Vector2.Zero).ToRotation() - MathHelper.ToRadians(90);
        }
    }

    public class ApotheosisLaser : clericHealProj
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 500;
            Projectile.width = Projectile.height = 20;
            Projectile.alpha = 200;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            canDealDamage = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Weapons/Fragment/ApotheosisLaserBeam").Value;

            for (float i = 0; i <= 600; i += 10)
            {
                Color c = Projectile.GetAlpha(lightColor);
                var origin = Projectile.Center + i * Projectile.velocity;
                Vector2 pos = (Projectile.position + (new Vector2(i).RotatedBy(Projectile.rotation + MathHelper.ToRadians(45))) - Main.screenPosition) + new Vector2(Projectile.width / 2, Projectile.height / 2);
                Main.EntitySpriteDraw(texture, pos, null, c, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(600).RotatedBy(Projectile.rotation + MathHelper.ToRadians(45)));
        }

        float incAmount = 0.1f;
        float inc2 = 0.01f;

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * (incAmount / 7));
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.channel)
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 2;

            Projectile.alpha -= 3;
            if (Projectile.alpha < 100)
            {
                Projectile.alpha = 100;
            }

            if (Projectile.ai[0] < 64)
            {
                Projectile.ai[0] += 1;
            }
            Projectile.ai[1] += incAmount;

            if (Projectile.ai[0] > 32)
            {
                incAmount += inc2;
                if (incAmount > 7)
                    incAmount = 7;

                inc2 *= 1.02f;
                if (inc2 > 1)
                    inc2 = 1;
            }

            float rot = 0;

            if (Main.myPlayer == Main.player[Projectile.owner].whoAmI && !player.mapFullScreen)
            {
                rot = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).ToRotation() - MathHelper.ToRadians(90);
            }
            Projectile.Center = player.Center + new Vector2(Projectile.ai[0]).RotatedBy(rot + MathHelper.ToRadians(45)) + new Vector2(24, 24).RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));
           
            Projectile.rotation = (Projectile.Center - player.Center).SafeNormalize(Vector2.Zero).ToRotation() - MathHelper.ToRadians(90);
        }
    }
}
