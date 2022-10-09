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
using Terraria.GameContent;

namespace excels.Items.Weapons.Spirits
{
    internal class PurificationRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons two Purity Orbs that fire cards at foes\nKilling enemies boosts the power of the Purity Orbs");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 43;
            Item.mana = 10;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 4;
            Item.knockBack = 3.5f;
            //Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TouhouOrb>();
            Item.shootSpeed = 7;
            Item.height = Item.width = 30;
            Item.sellPrice(0, 0, 40);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Pearlwood, 20)
                .AddIngredient(ItemID.LightShard)
                .AddIngredient(ItemID.DarkShard)
                .AddTile(TileID.MythrilAnvil)
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
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            p.ai[1] = 160;
            return true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.GetModPlayer<excelPlayer>().SpiritDamageMult;
        }
    }

    public class TouhouOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 10;
            Projectile.width = Projectile.height = 40;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.hide = true;
            Projectile.alpha = 80;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity *= 0;
            if (player.HeldItem.type != ModContent.ItemType<PurificationRod>() || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            Projectile.ai[1] += 0.12f;
            Projectile.Center = player.Center;
            Projectile.position.X += (MathF.Cos(Projectile.ai[1]) * 30);
            Projectile.position.Y += MathF.Sin(Projectile.ai[1]) * 5;

            Projectile.rotation += MathHelper.ToRadians(19);

            Projectile.ai[0] += 1 * player.GetModPlayer<excelPlayer>().SpiritAttackSpeed;
            if (Projectile.ai[0] > 27)
            {
                if (Main.myPlayer == Main.player[Projectile.owner].whoAmI && Main.mouseLeft && !Main.LocalPlayer.mouseInterface && !player.mapFullScreen)
                {
                    if (player.HasBuff(ModContent.BuffType<SoulUpBuff>()))
                    {
                        Projectile p2 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
                            (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Main.rand.Next(7)+14)) * 11,
                            ModContent.ProjectileType<TouhouCard>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                        p2.netUpdate = true;

                        Projectile p3 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
                            (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Main.rand.Next(7)-14)) * 11,
                            ModContent.ProjectileType<TouhouCard>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                        p3.netUpdate = true;
                    }

                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
                        (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero).RotatedByRandom(MathHelper.ToRadians(7)) * 11, 
                        ModContent.ProjectileType<TouhouCard>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                    p.netUpdate = true;
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;

                    //SoundEngine.PlaySound(SoundID.Item34, Projectile.Center);
                }
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {

            if (Projectile.position.Y > Main.player[Projectile.owner].position.Y)
            {
                overPlayers.Remove(index);
                behindNPCs.Add(index);

            }
            else
            {
                overPlayers.Add(index);
                behindNPCs.Remove(index);
            }
        }
    }

    public class SoulUpBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            DisplayName.SetDefault("Soul Up");
            Description.SetDefault("Purity Orb's power is increased");
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = false;
        }
    }

    public class TouhouCard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.timeLeft = 200;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life < 0)
            {
                Main.player[Projectile.owner].AddBuff(ModContent.BuffType<SoulUpBuff>(), 300);
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            Vector2 targetPos = Vector2.Zero;
            float targetDist = 600;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy(this, false))
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
                Vector2 move = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * 9;

                AdjustMagnitude(ref move);
                Projectile.velocity = (Projectile.velocity.Length() * Projectile.velocity + move); // / 5f;
                AdjustMagnitude(ref Projectile.velocity);
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 10 / magnitude;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 15; i++)
            {
                int dType = 130;
                if (i % 3 == 0) dType = 132;
                Dust d = Dust.NewDustDirect(Projectile.Center-new Vector2(4, 4), 8, 8, dType);
                d.noGravity = true;
                d.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(14)) * Main.rand.NextFloat(0.13f, 0.56f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k % 2 == 0)
                {
                    Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * (((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) / 2);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, 1, SpriteEffects.None, 0);
                }
            }

            return true;
        }
    }
}
