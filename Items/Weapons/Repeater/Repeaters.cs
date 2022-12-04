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

namespace excels.Items.Weapons.Repeater
{
    /*
    internal class Marksman : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Huntsman");
            Tooltip.SetDefault("Fires a beam of darkness that 'marks' foes it hits \nWhile a foe is marked, doubles firing speed and shoots explosive-stalking arrows");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 42;
            Item.knockBack = 8;
            Item.useTime = Item.useAnimation = 28;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = 5;
            Item.shoot = 10;
            Item.shootSpeed = 13f;
            Item.UseSound = SoundID.Item5;
            Item.noMelee = true;
            Item.sellPrice(0, 2, 70);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            for (var i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active)
                {
                    if (npc.GetGlobalNPC<excelNPC>().MarkedTimer > 0)
                    {
                        type = ModContent.ProjectileType<StalkingArrow2>();
                        Item.useTime = Item.useAnimation = 14;
                        return;
                    }
                }
            }

            type = ModContent.ProjectileType<Bows.DarkBeam>();
            damage /= 2;
            Item.useTime = Item.useAnimation = 28;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Bows.StalkersBow>())
                .AddIngredient(ItemID.HallowedRepeater)
                .AddIngredient(ItemID.ShadowFlameBow)
                .AddIngredient(ItemID.SoulofFright, 8)
                .AddTile(ModContent.TileType<Tiles.Misc.StarlightAnvilTile>())
                .Register();

        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
    */
    public class StalkingArrow2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        bool target = false;

        public override void AI()
        {
            Vector2 targetPos = Vector2.Zero;
            float targetDist = 600;
            target = false;
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy(this, false) && npc.GetGlobalNPC<excelNPC>().MarkedTimer > 0)
                {
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < targetDist)
                    {
                        targetDist = distance;
                        targetPos = npc.Center;
                        target = true;
                    }
                }
            }
            if (target)
            {
                if (++Projectile.ai[0] % 4 == 0)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 27);
                    d.velocity = Projectile.velocity * -0.1f;
                    d.scale += 0.3f;
                }

                Projectile.velocity = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
            }
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!target)
                return true;

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.active)
            {
                if (target.GetGlobalNPC<excelNPC>().MarkedTimer > 0 && target.GetGlobalNPC<excelNPC>().MarkedTimer < 300)
                {
                    target.GetGlobalNPC<excelNPC>().MarkedTimer += 30;
                    return;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowfireBoom>(), (int)(Projectile.damage * 0.7f), 0, Main.player[Projectile.owner].whoAmI);
        }
    }

    public class ShadowfireBoom : ModProjectile 
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FireArrow}";

        public override void SetDefaults()
        {
            Projectile.timeLeft = 10;
            Projectile.width = Projectile.height = 80;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.friendly = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 280);
        }

        public override void AI()
        {
            if (++Projectile.ai[0] == 1)
            {
                for (var i = 0; i < 30; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 8, 8, 27);
                    d.noGravity = true;
                    d.velocity = new Vector2(Main.rand.NextFloat(3, 5)).RotatedByRandom(MathHelper.ToRadians(360));
                    d.scale = 2 - (d.velocity.Length() * 0.06f);
                }
            }
        }
    }

}
