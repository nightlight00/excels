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

namespace excels.Items.Weapons.Blood
{
    internal class DripplingStick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drippling Stick");
            Tooltip.SetDefault("Summons an itsy bitsy drippler for you to command");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.GetModPlayer<excelPlayer>().SpiritDamageMult;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 12;
            Item.mana = 10;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 1;
            Item.knockBack = 3.5f;
            //Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DripplingSpirit>();
            Item.shootSpeed = 7;
            Item.height = Item.width = 30;
            Item.sellPrice(0, 0, 80);
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

    public class DripplingSpirit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
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
            if (player.HeldItem.type != ModContent.ItemType<DripplingStick>() || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            rot += 0.07f;
            Projectile.Center = player.Center;
            Projectile.position.X += MathF.Cos(rot) * 10 + Projectile.width / 4;
            Projectile.position.Y += -46 + MathF.Sin(rot) * 5;

            Projectile.ai[0] += 1 * player.GetModPlayer<excelPlayer>().SpiritAttackSpeed;
            if (Projectile.ai[0] > 30)
            {
                if (Main.myPlayer == Main.player[Projectile.owner].whoAmI && Main.mouseLeft && !Main.LocalPlayer.mouseInterface && !player.mapFullScreen)
                {
                    Vector2 velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 8;

                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
                        velocity, ModContent.ProjectileType<DripplingEye>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                    p.netUpdate = true;
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;

                    SoundEngine.PlaySound(SoundID.NPCHit18, Projectile.Center);

                    for (var i = 0; i < 17; i++)
                    {
                        Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
                        d.fadeIn = d.scale * 1.13f;
                        d.noGravity = true;
                        d.velocity = velocity.RotatedByRandom(MathHelper.ToRadians(20)) * Main.rand.NextFloat(0.82f, 1.18f);
                    }
                }
            }

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
                d.scale = Main.rand.NextFloat(1.05f, 1.15f);
                d.noGravity = true;
                d.velocity = new Vector2(0, Main.rand.NextFloat(1.8f, 2.3f));
            }

            if (++Projectile.frameCounter % 7 == 0)
            {
                if (++Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
            }
        }
    }

    public class DripplingEye : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 240;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (int)(Projectile.damage * 0.9f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate == 0)
            {
                Projectile.Kill();
            }
            Projectile.velocity = -Projectile.velocity * 0.85f;
            Projectile.velocity.Y -= 1.5f;
            Projectile.damage = (int)(Projectile.damage * 0.9f);
            return false;
        }

        public override void AI()
        {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
            d.noGravity = true;
            d.velocity = Projectile.velocity * 0.2f;

            Projectile.velocity.Y += 0.115f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
