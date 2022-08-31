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

namespace excels.Items.Weapons.Bows
{
    #region Hunter's Longbow
    internal class HunterLongbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hunter's Longbow");
            Tooltip.SetDefault("Fires special arrows that guarantee critical hits after traveling far enough" 
                           + "\n'A hunting bow made from the carcass of a grand hunt, fitting isn't it?'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 14;
            Item.knockBack = 5.6F;
            Item.useTime = Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = 2;
            Item.shoot = 10;
            Item.shootSpeed = 12f;
            Item.UseSound = SoundID.Item5;
            Item.noMelee = true;
            Item.sellPrice(0, 1, 10);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<HuntingArrow>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                  .AddIngredient(ItemID.WoodenBow)
                  .AddRecipeGroup("IronBar", 8)
                  .AddIngredient(ItemID.Lens, 2)
                  .AddIngredient(ItemID.TissueSample, 12)
                  .AddTile(TileID.Anvils)
                  .Register();

            CreateRecipe()
                    .AddIngredient(ItemID.WoodenBow)
                    .AddRecipeGroup("IronBar", 8)
                    .AddIngredient(ItemID.Lens, 2)
                    .AddIngredient(ItemID.ShadowScale, 12)
                    .AddTile(TileID.Anvils)
                    .Register();
        }
    }

    public class HuntingArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Projectile.ai[1] >= 35)
            {
                crit = true;
            }
            else
            {
                crit = false;
            }
        }

        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] >= 35 && Main.rand.NextBool())
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
                d.velocity = Projectile.velocity * 0.4f;
                d.noGravity = true;
                d.scale *= Main.rand.NextFloat(1.1f, 1.25f);
                d.fadeIn = d.scale * 1.1f;
                d.alpha = 70;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 8; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 122);
            }
        }
    }
    #endregion

    #region Stalker's Longbow
    internal class StalkersBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stalker's Longbow");
            Tooltip.SetDefault("Fires a beam of darkness that 'marks' foes it hits\nMarked foes will be automatically hunted by arrows");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 32;
            Item.knockBack = 5;
            Item.useTime = Item.useAnimation = 31;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = 3;
            Item.shoot = 10;
            Item.shootSpeed = 11.5f;
            Item.UseSound = SoundID.Item5;
            Item.noMelee = true;
            Item.sellPrice(0, 2, 50);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(12)), ModContent.ProjectileType<StalkingArrow>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-12)), ModContent.ProjectileType<StalkingArrow>(), damage, knockback, player.whoAmI);

            Projectile.NewProjectile(source, position, velocity/2, ModContent.ProjectileType<DarkBeam>(), damage/2, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HunterLongbow>())
                .AddIngredient(ItemID.DemonBow)
                .AddIngredient(ItemID.MoltenFury)
                .AddIngredient(ItemID.BeesKnees)
                .AddIngredient(ItemID.BloodRainBow)
                .AddTile(ModContent.TileType<Tiles.Misc.StarlightAnvilTile>())
                .Register();

            CreateRecipe()
                  .AddIngredient(ModContent.ItemType<HunterLongbow>())
                  .AddIngredient(ItemID.TendonBow)
                  .AddIngredient(ItemID.MoltenFury)
                  .AddIngredient(ItemID.BeesKnees)
                  .AddIngredient(ItemID.BloodRainBow)
                  .AddTile(ModContent.TileType<Tiles.Misc.StarlightAnvilTile>())
                  .Register();
        }
    }

    public class DarkBeam : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SnowBallFriendly}";

        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 8;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 50;
        }

        public override void AI()
        {
            for (var i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 27);
                d.velocity = Projectile.velocity * ((i+1) * 0.1f);
                d.scale += 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (var i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active)
                {
                    npc.GetGlobalNPC<excelNPC>().MarkedTimer = -10;
                }
            }
            
            target.GetGlobalNPC<excelNPC>().MarkedTimer = 160;
        }
    }

    public class StalkingArrow : ModProjectile
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

                float num145 = 15f;
                float num146 = 0.0833333358f;
                Vector2 vec = targetPos - Projectile.Center;
                vec.Normalize();
                if (vec.HasNaNs())
                {
                    vec = new Vector2((float)Projectile.direction, 0f);
                }
                Projectile.velocity = (Projectile.velocity * (num145 - 1f) + vec * (Projectile.velocity.Length() + num146)) / num145;
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

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 8; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 77);

            }
        }
    }
    #endregion

    #region Jungle Bow
    internal class JungleBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stalker's Longbow");
            Tooltip.SetDefault("Coats wooden arrows with poisonous spores");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 9;
            Item.knockBack = 3;
            Item.useTime = Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = 2;
            Item.shoot = 10;
            Item.shootSpeed = 9.5f;
            Item.UseSound = SoundID.Item5;
            Item.noMelee = true;
            Item.sellPrice(0, 0, 12);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<PoisonArrow>(); 
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                  .AddIngredient(ItemID.RichMahoganyBow)
                  .AddIngredient(ItemID.JungleSpores, 6)
                  .AddIngredient(ItemID.Vine, 2)
                  .AddTile(TileID.WorkBenches)
                  .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }

    public class PoisonArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Poisoned, 300);
            }
            else
            {
                target.AddBuff(BuffID.Poisoned, 180);
            }
        }

        public override void AI()
        {
           if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
                d.scale *= Main.rand.NextFloat(1.2f, 1.3f);
                d.fadeIn = d.scale * 1.2f;
                d.alpha = 180;
                d.noGravity = true;
                d.velocity = Projectile.velocity * 0.2f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 8; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 7);

                Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 46);
                d2.scale *= Main.rand.NextFloat(1.2f, 1.3f);
                d2.fadeIn = d.scale * 1.2f;
                d2.alpha = 180;
                d2.noGravity = true;
                d2.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(360)) * 0.45f;
            }
        }
    }
    #endregion
}
