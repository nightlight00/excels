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


namespace excels.Items.Weapons.Thrown
{
    #region Floral Hatchet
    internal class FloralHatchetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Floral Hatchet");
            Tooltip.SetDefault("33% chance to not be consumed on use");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.damage = 69;
            Item.knockBack = 3.9f;
            Item.rare = 7;
            Item.UseSound = SoundID.Item71;
            Item.shoot = ModContent.ProjectileType<FloralHatchet>();
            Item.shootSpeed = 4.8f;
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
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(14));
        }

        public override void AddRecipes()
        {
            CreateRecipe(120)
                .AddIngredient(ItemID.ChlorophyteBar, 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class FloralHatchet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 36;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 400;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.extraUpdates = 3;
            Projectile.friendly = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k % 3 == 0)
                {
                    var frame = texture.Frame(1, 2, 0, 1);
                    Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = (Color.White * 0.66f) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, frame, color, Projectile.oldRot[k], drawOrigin, 1, SpriteEffects.None, 0);
                }
            }

            return true;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X) * 4.5f;
            Lighting.AddLight(Projectile.Center, TorchID.Jungle);

            if (++Projectile.ai[0] > 70)
            {
                Projectile.velocity.Y += 0.042f;
                Projectile.velocity.X *= 0.995f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            for (var i = 0; i < 20; i++)
            {
                int type = 128;
                if (Main.rand.NextBool(3))
                    type = ModContent.DustType<Dusts.FloralDust>();
                // 111
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
}
