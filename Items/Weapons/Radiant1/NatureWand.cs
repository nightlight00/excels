using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using excels.Buffs.HealOverTime;

namespace excels.Items.Weapons.Radiant1
{
    internal class NatureBombWand : ClericDamageItem
    {
        public override string Texture => "excels/Items/Weapons/Radiant1/AcornStaff";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acorn Staff");
            Tooltip.SetDefault("Critical strikes grant healing over time to nearby allies");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.width = Item.height = 40;
            Item.useTime = Item.useAnimation = 29;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 10000;
            Item.rare = 0;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<NatureWave>();
            Item.shootSpeed = 9.44f;
            Item.noMelee = true;
            Item.knockBack = 4.3f;

            Item.damage = 13;
            clericRadianceCost = 6;
            Item.sellPrice(0, 0, 0, 15);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 30)
                .AddIngredient(ItemID.Acorn, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class NatureWave : clericHealProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }


        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.penetrate = 2;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 80;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            clericEvil = false;
            canDealDamage = true;

            canHealOwner = true;
            healPenetrate = -1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool())
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 2);
                d.velocity = Projectile.velocity * 0.9f;
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(0.9f, 1.1f);
            }

            if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 2);
                d.velocity = Projectile.velocity * 0.4f;
                d.scale = Main.rand.NextFloat(0.75f, 0.9f);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 2);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.2f, 1.4f);
                d.velocity = Main.rand.NextVector2Circular(0.2f, 0.2f) * 20 + (Projectile.velocity / 3);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (crit)
            {
                BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 140, 1);
                for (var i = 0; i < 30; i++)
                {
                    Vector2 vel = new Vector2(0, -2).RotatedBy(MathHelper.ToRadians(360 / 20) * i);
                    Dust d = Dust.NewDustPerfect(Main.player[Projectile.owner].Center + vel * 70, 2);
                    d.velocity = -vel;
                    d.noGravity = true;
                    d.scale = 1.6f;
                }
            }
        }

        public override void BuffEffects(Player target, Player healer)
        {
            target.GetModPlayer<HealOverTime>().AddHeal(healer, "Acorn Staff", 2, 3);
            //target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.NaturesHeart>(), GetBuffTime(healer, 4));
        }


        public override bool PreDraw(ref Color lightColor) // thumbs up!!!!
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                var offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                var frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + offset;
                float sizec = Projectile.scale * (Projectile.oldPos.Length - k) / (Projectile.oldPos.Length * 0.8f);
                Color color = new Color(220, 126, 255, 255) * (1f - Projectile.alpha) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, frame, color, Projectile.oldRot[k], frame.Size() / 2, sizec, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            return true;
        }
    }
}
