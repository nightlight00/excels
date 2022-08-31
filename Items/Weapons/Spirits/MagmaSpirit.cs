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

namespace excels.Items.Weapons.Spirits
{
    internal class MagmaRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Rod");
            Tooltip.SetDefault("Summons a magma mortar for you to command");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 28;
            Item.mana = 10;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 1;
            Item.knockBack = 3.5f;
            //Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MagmaSpirit>();
            Item.shootSpeed = 7;
            Item.height = Item.width = 30;
            Item.sellPrice(0, 0, 40);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .AddIngredient(ItemID.HellstoneBar, 4)
                .AddTile(TileID.Anvils)
                .Register();
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

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.GetModPlayer<excelPlayer>().SpiritDamageMult;
        }
    }

    public class MagmaSpirit : ModProjectile
    {
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
            if (player.HeldItem.type != ModContent.ItemType<MagmaRod>() || player.dead)
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
            if (Projectile.ai[0] > 53)
            {
                if (Main.myPlayer == Main.player[Projectile.owner].whoAmI && Main.mouseLeft && !Main.LocalPlayer.mouseInterface && !player.mapFullScreen)
                {
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
                        new Vector2(0, -12), ModContent.ProjectileType<MagmaEmber>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                    p.netUpdate = true;
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;

                    SoundEngine.PlaySound(SoundID.Item34, Projectile.Center);

                    for (var i = 0; i < 25; i++)
                    {
                        Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                        d.scale = Main.rand.NextFloat(1.2f, 1.4f);
                        d.fadeIn = d.scale * 1.13f;
                        d.noGravity = true;
                        d.velocity = new Vector2(0, -Main.rand.NextFloat(3.75f, 4.8f)).RotatedByRandom(MathHelper.ToRadians(26));
                    }
                }
            }

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                d.scale = Main.rand.NextFloat(1.05f, 1.15f);
                d.noGravity = true;
                d.velocity *= 0.6f;
            }

            // (10, 18)
            Dust d2 = Dust.NewDustDirect(Projectile.position + new Vector2(6, 13), 0, 0, 6);
            d2.noGravity = true;
            d2.velocity *= 0;
            d2.scale = 1.3f;
            d2.alpha = 100;

            Dust d3 = Dust.NewDustDirect(Projectile.position + new Vector2(12, 13), 0, 0, 6);
            d3.noGravity = true;
            d3.velocity *= 0;
            d3.scale = 1.3f;
        }
    }

    internal class MagmaEmber : ModProjectile
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
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] >= 1)
            {
                return true;
            }
            return false;
        }

        void Explode()
        {
            if (Projectile.ai[1] == 2)
            {
                return;
            }

            Projectile.ai[1] = 2;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;

            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 80;
            Projectile.Center = Projectile.position;
            Projectile.velocity = Vector2.Zero;

            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            for (var i = 0; i < 50; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                d.scale = Main.rand.NextFloat(1.2f, 1.4f);
                d.velocity = (Vector2.One * Main.rand.NextFloat(2, 4.5f)).RotatedByRandom(MathHelper.ToRadians(180));
                d.noGravity = true;
            }
            for (var i = 0; i < 30; i++)
            {
                Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                d2.velocity = (Vector2.One * Main.rand.NextFloat(2.8f, 5f)).RotatedByRandom(MathHelper.ToRadians(180));
                d2.noGravity = true;
            }

            for (var g = 0; g < 4; g++)
            {
                int Type = Main.rand.Next(3);
                switch (Type)
                {
                    case 1: Type = GoreID.Smoke1; break;
                    case 2: Type = GoreID.Smoke2; break;
                    default: Type = GoreID.Smoke3; break;
                }
                Gore gg = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(1.3f, 2), Main.rand.NextFloat(1.3f, 2)).RotatedBy(MathHelper.ToRadians(90 * g)), Type);
                gg.velocity = new Vector2(Main.rand.NextFloat(1.3f, 2), Main.rand.NextFloat(1.3f, 2)).RotatedBy(MathHelper.ToRadians(90 * g + Main.rand.Next(-20, 20)));
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 480);
            Explode();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return Projectile.tileCollide;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 40)
            {
                Projectile.ai[1] = 1;
                if (Main.myPlayer == Main.player[Projectile.owner].whoAmI)
                {
                    Projectile.Center = new Vector2(Main.MouseWorld.X, Main.player[Projectile.owner].position.Y - 500);
                    Projectile.velocity = -Projectile.velocity * 1.3f;
                }
                Projectile.tileCollide = true;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            for (var i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                d.scale = Main.rand.NextFloat(1.1f, 1.25f);
                d.noGravity = true;
            }

            Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
            d2.scale = Main.rand.NextFloat(1.15f, 1.3f);
            d2.noGravity = true;
        }
    }
}
