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
using System.Collections.Generic;

namespace excels.Items.Weapons.Sentries
{
    internal class SpinMillStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a sentry\nSummons a spinning blade to help hold down fort");

            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.mana = 10;   
            Item.useTime = Item.useAnimation = 30;
            Item.shoot = ModContent.ProjectileType<SpinMillStand>();
            Item.damage = 8;
            Item.knockBack = 3;
            Item.sentry = true;
            Item.DamageType = DamageClass.Summon;
            Item.UseSound = SoundID.Item46;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp); 
            Projectile.NewProjectile(source, worldX, worldY - pushYUp, 0f, 0f, type, damage, 0, Main.myPlayer);

            player.UpdateMaxTurrets();

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 28)
                .AddRecipeGroup("IronBar", 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    internal class SpinMillStand : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 22;
            Projectile.height = 28;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 8;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.sentry = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Top, DustID.Lead);
                d.noGravity = true;
                d.noLight = true;
                d.velocity = new Vector2(Main.rand.NextFloat(2.1f, 3.5f) * ((i % 2 == 0) ? -1 : 1), Main.rand.NextFloat(0.3f, -0.3f));
                d.scale = Main.rand.NextFloat() * 0.7f + 1.2f;

                Dust w = Dust.NewDustDirect(Projectile.Center-new Vector2(4,6), 8, 12, DustID.WoodFurniture);
                w.noGravity = true;
                w.velocity = Main.rand.NextVector2Circular(1.8f, 2.5f) * 3;
                w.scale = Main.rand.NextFloat(1.4f, 1.6f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.Center.X > Projectile.Center.X)
                target.velocity.X += 3 * target.knockBackResist;
            else
                target.velocity.X -= 3 * target.knockBackResist;
            target.velocity.Y -= 2 * target.knockBackResist;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(19);

            if (Collision.SolidTiles(Projectile.BottomLeft + new Vector2(0, -6), Projectile.width, 4, true))
                Projectile.velocity.Y = 0;
            else
                Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + 0.12f, 0, 12);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Projectile.Top - new Vector2(16, 16);
            return Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, new Vector2(32, 32));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Projectile.GetAlpha(lightColor), 0, texture.Size() / 2, 1, SpriteEffects.None, 0);

            texture = Mod.Assets.Request<Texture2D>("Items/Weapons/Sentries/SpinMillSpinner").Value;
            Main.EntitySpriteDraw(texture, Projectile.Top - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Projectile.GetAlpha(lightColor) * 0.33f, Projectile.rotation - MathHelper.ToRadians(60), texture.Size() / 2, 1.1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Top - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Projectile.GetAlpha(lightColor) * 0.66f, Projectile.rotation - MathHelper.ToRadians(30), texture.Size() / 2, 1.2f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Top - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Projectile.GetAlpha(lightColor), Projectile.rotation, texture.Size() / 2, 1.3f, SpriteEffects.None, 0);
            return false;
        }
    }


}
