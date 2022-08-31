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

namespace excels.Items.Weapons.Glacial.Boss
{
    #region Yoyo
    public class DarkEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Leaves behind damaging afterimages");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 25;
            Item.useTime = Item.useAnimation = 25;
            Item.knockBack = 2.3f;
            Item.noMelee = true;
            Item.height = 22;
            Item.width = 26;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<DarkEyeProj>();
            Item.shootSpeed = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.sellPrice(0, 0, 65);
        }
    }

    public class DarkEyeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Eye");
            // Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 9.5f;
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 270f;
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12.5f;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 0;
        }

        int img = 0;
        public override void AI()
        {
            img++;
            if (img > 5)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<DarkEyeAfterimage>(), Projectile.damage / 2, 0, Main.player[Projectile.owner].whoAmI);
                img = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Frostburn, 180);
                return;
            }
            target.AddBuff(BuffID.Frostburn, 60);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Frostburn, 180);
                return;
            }
            target.AddBuff(BuffID.Frostburn, 60);
        }
    }

    public class DarkEyeAfterimage : ModProjectile
    {
        public override string Texture => "excels/Items/Weapons/Glacial/Boss/DarkEyeProj";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Eye");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.alpha = 40;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Projectile.alpha += 6;
            Projectile.rotation += MathHelper.ToRadians(15);
            if (Projectile.alpha > 230)
            {
                Projectile.Kill();
            }
        }
    }
    #endregion

    #region Staff
    public class Nastrond : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Conjures frost spears to impale your foes");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 32;
            Item.useTime = Item.useAnimation = 18;
            Item.knockBack = 2.3f;
            Item.noMelee = true;
            Item.height = 22;
            Item.width = 26;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<NastrondProj>();
            Item.shootSpeed = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.mana = 4;
            Item.sellPrice(0, 0, 70);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.MouseWorld.X > player.Center.X)
            {
                Projectile.NewProjectile(source, new Vector2(player.Center.X - Main.screenWidth / 2, Main.MouseWorld.Y), new Vector2(13, 0), type, damage, knockback, player.whoAmI);
            }
            else
            {
                Projectile.NewProjectile(source, new Vector2(player.Center.X + Main.screenWidth / 2, Main.MouseWorld.Y), new Vector2(-13, 0), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }

    public class NastrondProj : ModProjectile
    {
        public override string Texture => "excels/Items/Weapons/Glacial/Boss/Nastrond";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nastrond"); 
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 50;
            Projectile.penetrate = 4;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (var i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92);
                d.velocity = (Projectile.velocity * Main.rand.NextFloat(0.4f, 0.8f)).RotatedByRandom(MathHelper.ToRadians(20));
            }
            Projectile.damage = (int)(Projectile.damage * 0.9f);
            target.AddBuff(BuffID.Frostburn, 300);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            for (var i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 92);
                d.velocity = (Projectile.velocity * Main.rand.NextFloat(0.4f, 0.8f)).RotatedByRandom(MathHelper.ToRadians(20));
            }
            target.AddBuff(BuffID.Frostburn, 300);
        }

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

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

            /*
          // 64 is the sprite size (here both width and height equal)
          const int HalfSpriteWidth = 50 / 2;
          const int HalfSpriteHeight = 50 / 2;

          int HalfProjWidth = Projectile.width / 2;
          int HalfProjHeight = Projectile.height / 2;

          // Vanilla configuration for "hitbox in middle of sprite"
          DrawOriginOffsetX = 0;
          DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
          DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
              */
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * 6);
        }
    }
    #endregion

    #region Sword
    public class FrozenChainblade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("Conjures frost spears to impale your foes");
           // Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 45;
            Item.useTime = Item.useAnimation = 28;
            Item.knockBack = 2.8f;
            Item.noMelee = true;
            Item.height = 22;
            Item.width = 26;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<FrozenBlade>();
            Item.shootSpeed = 13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.sellPrice(0, 0, 70);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (var i = 0; i < 10; i++)
            {
                Projectile p = Projectile.NewProjectileDirect(source, position, velocity / 5 * (i+1), type, (int)(damage * (1 - i * 0.05f)), knockback, player.whoAmI);
                p.ai[1] = i;
            }
            return false;
        }
    }

    public class FrozenBlade : ModProjectile
    {
       // public override string Texture => "excels/Items/Misc/Fans/HarpyFan";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 24;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 120); 
        }

        float vel = 15;
        Vector2 pos = Vector2.Zero;
        public override void AI()
        {
            Main.player[Projectile.owner].heldProj = Projectile.whoAmI;

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
            if (Projectile.ai[0] == 0)
            {
                switch (Projectile.ai[1])
                {
                    case 0:
                        Projectile.frame = 0;
                        break;
                    case 9:
                        Projectile.frame = 4;
                        break;
                    default:
                        Projectile.frame = Main.rand.Next(1, 4);
                        break;
                }
            }

            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 13)
            {
                Projectile.velocity = (Main.player[Projectile.owner].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * vel;
                if (Vector2.Distance(Main.player[Projectile.owner].Center, Projectile.Center) < 10)
                {
                    Projectile.Kill();
                }
                vel += 0.1f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            }
            pos += Projectile.velocity;

            Projectile.Center = Main.player[Projectile.owner].Center + pos;
        }
    }
    #endregion
}
