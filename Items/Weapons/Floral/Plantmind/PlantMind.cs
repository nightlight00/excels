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

namespace excels.Items.Weapons.Floral.Plantmind
{
    internal class PlantMind : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Photosynthesis Staff");
            Tooltip.SetDefault("Manifests a Solar Beam that creates plant cells that can be collected by allies\nHitting multiple foes generates additional plant cells");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.width = Item.height = 40;
            Item.useTime = Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 10000;
            Item.rare = 7;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SolarBeam>();
            Item.shootSpeed = 1.4f;
            Item.noMelee = true;
            Item.knockBack = 2.1f;

            Item.damage = 66;
            clericRadianceCost = 8;
        }
    }

    internal class SolarBeam : clericHealProj
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 12;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            canDealDamage = true;
        }

        public override bool ShouldUpdatePosition() => false;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + unit * Projectile.ai[0], 8, ref point); 
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (int)(Projectile.damage * 0.9f);
            Projectile.localAI[0] += 1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Calculate length
            if (Projectile.ai[1] == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();

                for (Projectile.ai[0] = 0; Projectile.ai[0] < 300; Projectile.ai[0] += 8)
                {
                    var start = player.Center + Projectile.velocity * Projectile.ai[0];
                    if (!Collision.CanHit(player.Center, 1, 1, start, 1, 1))
                    {
                        Projectile.ai[0] -= 8f;
                        break;
                    }

                    if (Main.rand.NextBool(2))
                    {
                        Dust d = Dust.NewDustDirect(start - new Vector2(4, 4), 8, 8, 204);
                        d.noGravity = true;
                        d.scale = Main.rand.NextFloat(1.2f, 1.3f);
                    }
                }

                // Kill the projectile if it's too small
                if (Projectile.ai[0] < 40)
                {
                    Projectile.Kill();
                    return;
                }

                Projectile.ai[1] = 1;
            }

            // Create light
            for (var i = 0; i < 6; i++)
            {
                Vector2 pos = Projectile.Center + Projectile.velocity * ((Projectile.ai[0] / 6) * i);
                Lighting.AddLight(pos, Color.LightGoldenrodYellow.ToVector3() * 0.7f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Weapons/Floral/Plantmind/SolarBeam").Value;

            Vector2 pos;
            Color c = Projectile.GetAlpha(lightColor);
            for (float i = 40; i < Projectile.ai[0]; i += 8)
            {
                var origin = Projectile.Center + i * Projectile.velocity;
                pos = (Projectile.position + (new Vector2(i).RotatedBy(Projectile.rotation - MathHelper.ToRadians(45))) - Main.screenPosition) + new Vector2(Projectile.width / 2, Projectile.height / 2);
                Main.EntitySpriteDraw(texture, pos, texture.Frame(1, 2, 0, 0), c, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0);
            }

            pos = (Projectile.position + (new Vector2(Projectile.ai[0]).RotatedBy(Projectile.rotation - MathHelper.ToRadians(45))) - Main.screenPosition) + new Vector2(Projectile.width / 2, Projectile.height / 2);
            Main.EntitySpriteDraw(texture, pos, texture.Frame(1, 2, 0, 1), c, 
                Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), 
                Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override void Kill(int timeLeft)
        {
            // Doesn't create any if too small
            if (Projectile.ai[0] < 40)
                return;

            int amount = 1;
            if (Projectile.localAI[0] >= 2)
            {
                amount = 2;
                if (Projectile.localAI[0] >= 5)
                    amount = 3;
            }

            for (var i = 0; i < amount; i++)
            {
                CreateHealProjectile(Main.player[Projectile.owner], Projectile.Center + Projectile.velocity * ((Projectile.ai[0] / (amount + 1) * (i + 1))+Main.rand.Next(-8, 8)), Main.rand.NextVector2Circular(0.3f, 0.3f) * Main.rand.NextFloat(1.5f, 2.2f), ModContent.ProjectileType<PlantMindCells>(), 0, 0);
            }
        }
    }

    internal class PlantMindCells : clericHealProj
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 18;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 400;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.Length() / 3;

            if (Projectile.ai[0] == 0)
                Projectile.frame = Main.rand.Next(3);

            // Add light
            Color c = Color.White;
            switch (Projectile.frame)
            {
                case 0:
                    c = Color.Turquoise;
                    break;
                case 1:
                    c = Color.Magenta;
                    break;
                case 2:
                    c = Color.Lime;
                    break;
            }
            Lighting.AddLight(Projectile.Center, c.ToVector3() * (0.45f * (1 - Projectile.alpha / 255)));

            Projectile.ai[0] += 0.8f;
            if (Projectile.ai[0] > 90 || Projectile.ai[1] == 1)
            {
                Projectile.alpha += 3;
                if (Projectile.alpha >= 250)
                {
                    Projectile.Kill();
                }
            }

            if (Projectile.ai[1] == 0)
                BuffDistance(Main.LocalPlayer, Main.player[Projectile.owner], 20);
        }

        public override void BuffEffects(Player target, Player healer)
        {
            target.GetModPlayer<HealOverTime>().AddHeal(healer, "Plant Cell", 4, 2);
            Projectile.ai[1] = 1;
        }
    }
}
