using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Weapons.Fungal
{
    public class ShroomSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Spreads a fungal disease");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 15;
            Item.knockBack = 4.2f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.crit = 3;
            Item.useAnimation = Item.useTime = 14;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.scale = 1.1f;
            Item.sellPrice(0, 0, 10);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            player.GetModPlayer<excelPlayer>().DebuffMycosis = true;
            Dust d = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 176);
            d.noGravity = true;
            d.scale = Main.rand.NextFloat(0.9f, 1.2f);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 150);
        }
        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 150);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GlowingMushroom, 20)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class ShroomStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Spreads a fungal disease");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 13;
            Item.mana = 4;
            Item.knockBack = 3.8f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item8;
            Item.crit = 3;
            Item.useAnimation = Item.useTime = 24;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Shroomfection>();
            Item.shootSpeed = 8.7f;
            Item.sellPrice(0, 0, 10);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GlowingMushroom, 20)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class Shroomfection : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.GlowingMushroom}";

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.timeLeft = 70;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            for (var i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 176);
                d.noGravity = true;
                d.velocity *= 0.5f;
                d.scale = Main.rand.NextFloat(0.9f, 1.4f);
                if (Main.rand.NextBool(5))
                {
                    d.scale = Main.rand.NextFloat(1.75f, 2.1f);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 150);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 150);
        }
    }

    #region Best Boi
    public class ShroomyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sentient Shroom");
            Description.SetDefault("The little shroom will fight for you");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // If the minions exist reset the buff time, otherwise remove the buff from the player
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ShroomyBoi>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }

    public class StrangeShroom : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange Shroom");
            Tooltip.SetDefault("Grows sentient shrooms to fight for you");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 9;
            Item.useTime = Item.useAnimation = 25;
            Item.knockBack = 2.3f;
            Item.noMelee = true;
            Item.height = Item.width = 42;
            Item.rare = 0;
            Item.shoot = ModContent.ProjectileType<ShroomyBoi>();
            Item.buffType = ModContent.BuffType<ShroomyBuff>();
            Item.shootSpeed = 1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.mana = 10;
            Item.UseSound = SoundID.Item44;
            Item.sellPrice(0, 0, 15);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GlowingMushroom, 20)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;
            return false;
        }
    }

    public class ShroomyBoi : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomy Boi");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // Projectile is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to Projectile Projectile, as it's resistant to all homing Projectiles.
        }

        public sealed override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 28;
            Projectile.height = 30;
            Projectile.tileCollide = false;

            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
        }

        public override bool? CanCutTiles() => false;

        public override bool MinionContactDamage() => false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // if owner alive and buff handling
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<ShroomyBuff>());
            }
            if (player.HasBuff(ModContent.BuffType<ShroomyBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            if (Projectile.velocity.X < 0f)
            {
                Projectile.spriteDirection = Projectile.direction = -1;
            }
            else if (Projectile.velocity.X > 0f)
            {
                Projectile.spriteDirection = Projectile.direction = 1;
            }

            /// Projectile.ai[0] 
            // 0 => idle
            // 1 => return to player if too far away
            // 2 => find target
            // 3 => attack target

            #region Check for Target + Idle Pos
            Vector2 targetPos = Projectile.position;
            float targetDist = 800;
            bool target = false;
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                // changed so that it uses checks if player has line of sight with npc, not the actual summon
                if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                {
                    targetDist = Vector2.Distance(Projectile.Center, targetPos);
                    targetPos = npc.Center;
                    target = true;
                }
            }
            else
            {
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];
                    if (npc.CanBeChasedBy(this, false))
                    {
                        float distance = Vector2.Distance(npc.Center, Projectile.Center);
                        if ((distance < targetDist || !target) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                        {
                            targetDist = distance;
                            targetPos = npc.Center;
                            target = true;
                        }
                    }
                }
            }


            tPosCarryOver = Vector2.Zero;

            if (Projectile.ai[0] == 1)// || !Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, player.position, player.width, player.height))
            {
                Projectile.tileCollide = false;
                target = false;
                targetPos = player.Center;

                Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 9;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
                for (var i = 0; i < 3; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 176);
                    d.noGravity = true;
                    d.velocity *= 0.5f;
                    d.scale = Main.rand.NextFloat(0.9f, 1.4f);
                }

                if (Vector2.Distance(player.Center, Projectile.Center) > 4000)
                {
                    Projectile.ai[0] = 0;
                    Projectile.Center = player.Center;
                }
                if (Vector2.Distance(player.Center, Projectile.Center) < 100)
                {
                    Projectile.ai[0] = 0;
                }

                if (++Projectile.frameCounter >= 5)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= 4)
                    {
                        Projectile.frame = 0;
                    }
                }
            }
            else if (target)
            {
                tPosCarryOver = targetPos;
                // movement
                if (Vector2.Distance(targetPos, Projectile.Center) > 600)
                {
                    // vertical movement handled in tile collide
                    if (targetPos.X > Projectile.position.X)
                    {
                        Projectile.velocity.X += 0.3f;
                        if (Projectile.velocity.X > 5)
                        {
                            Projectile.velocity.X = 5;
                        }
                    }
                    else
                    {
                        Projectile.velocity.X -= 0.3f;
                        if (Projectile.velocity.X < -5)
                        {
                            Projectile.velocity.X = -5;
                        }
                    }
                }
                else
                {
                    Projectile.velocity.X *= 0.8f;


                    if (targetPos.X > 0f)
                    {
                        Projectile.spriteDirection = Projectile.direction = -1;
                    }
                    else if (targetPos.X < 0f)
                    {
                        Projectile.spriteDirection = Projectile.direction = 1;
                    }

                    Projectile.ai[1]++;
                    if (Main.rand.NextBool(4))
                    {
                        Projectile.ai[1]++;
                    }
                    if (Projectile.ai[1] > 80)
                    {
                        for (var i = 0; i < 2; i++)
                        {
                            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_None(), targetPos + Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * Main.rand.Next(10, 35),
                                Vector2.Zero, ModContent.ProjectileType<ShroomSpore>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                            p.netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                        Projectile.ai[1] = 0;
                    }

                    Projectile.velocity.Y += 0.4f;
                }

                // animation
                if (Projectile.velocity.X < 0.2f && Projectile.velocity.X > -0.2f)
                {
                    if (++Projectile.frameCounter >= 5)
                    {
                        Projectile.frameCounter = 0;
                        if (++Projectile.frame >= 4)
                        {
                            Projectile.frame = 0;
                        }
                    }
                }
                else
                {
                    if (++Projectile.frameCounter >= 5)
                    {
                        Projectile.frameCounter = 0;
                        if (++Projectile.frame >= 8)
                        {
                            Projectile.frame = 4;
                        }
                    }
                }

                // travel to player if too far away
                // overrules attack cus hes too cute to stay off screen
                if (Vector2.Distance(player.Center, Projectile.Center) > 700)
                {
                    Projectile.ai[0] = 1;
                }
            }
            else if (Projectile.ai[0] == 0)
            {
                Projectile.rotation = 0;
                Projectile.tileCollide = true;

                // move to idle pos
                Vector2 direction = player.Center;
                Projectile.netUpdate = true;
                int num = 1;
                for (int k = 0; k < Projectile.whoAmI; k++)
                {
                    if (Main.projectile[k].active && Main.projectile[k].owner == Projectile.owner && Main.projectile[k].type == Projectile.type)
                    {
                        num++;
                    }
                }
                direction.X -= (float)((10 + num * 40) * player.direction);

                targetPos = direction;
                // travel to player if too far away
                // overrules attack cus hes too cute to stay off screen
                if (Vector2.Distance(Projectile.Center, targetPos) > 800)
                {
                    Projectile.ai[0] = 1;
                }

                Projectile.velocity.Y += 0.4f;

                if (Vector2.Distance(targetPos, Projectile.Center) > 20) {
                    // vertical movement handled in tile collide
                    if (targetPos.X > Projectile.position.X)
                    {
                        Projectile.velocity.X += 0.3f;
                        if (Projectile.velocity.X > 5)
                        {
                            Projectile.velocity.X = 5;
                        }
                    }
                    else
                    {
                        Projectile.velocity.X -= 0.3f;
                        if (Projectile.velocity.X < -5)
                        {
                            Projectile.velocity.X = -5;
                        }
                    }
                }
                else
                {
                    Projectile.velocity.X *= 0.8f;
                }

                if (Projectile.velocity.X < 0.2f && Projectile.velocity.X > -0.2f)
                {
                    if (++Projectile.frameCounter >= 5)
                    {
                        Projectile.frameCounter = 0;
                        if (++Projectile.frame >= 4)
                        {
                            Projectile.frame = 0;
                        }
                    }
                }
                else
                {
                    if (++Projectile.frameCounter >= 5)
                    {
                        Projectile.frameCounter = 0;
                        if (++Projectile.frame >= 8)
                        {
                            Projectile.frame = 4;
                        }
                    }
                }
            }

            #endregion

            // unbury the child
            if (Collision.SolidTiles(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.position.Y -= 2f;
            }
        }


        Vector2 tPosCarryOver = Vector2.Zero;


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.tileCollide)
            {
                if (Projectile.velocity.Y > 0)
                {
                    Projectile.velocity.Y = 0;
                }
                if (tPosCarryOver != Vector2.Zero)
                {
                    if (Vector2.Distance(tPosCarryOver, Projectile.Center) > 200 &&
                        (tPosCarryOver.Y + 120) < Projectile.Center.Y && Main.rand.NextBool(7))
                    {
                        Projectile.velocity.Y -= 9;
                    }
                }
                else if (Projectile.ai[0] == 0 && Projectile.velocity.Y == 0)
                {
                    if (Vector2.Distance(Main.player[Projectile.owner].Center, Projectile.Center) > 80 &&
                        (Main.player[Projectile.owner].Center.Y + 14) < Projectile.Center.Y && Main.player[Projectile.owner].velocity.Y < 0 && Main.rand.NextBool(5))
                    {
                        Projectile.velocity.Y -= 9;
                    }
                }
                //   return true;
            }
            return false;
        }
    }

    public class ShroomSpore : ModProjectile
    {
       // public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.TruffleSpore}";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.TruffleSpore);
            AIType = ProjectileID.TruffleSpore;
            Projectile.timeLeft /= 3;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 180);
        }

        public override void AI()
        {
            int num3 = Projectile.frameCounter + 1;
            Projectile.frameCounter = num3;
            if (num3 >= 8)
            {
                Projectile.frameCounter = 0;
                num3 = Projectile.frame + 1;
                Projectile.frame = num3;
                if (num3 >= 3)
                {
                    Projectile.frame = 0;
                }
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.alpha == 0)
            {
                float num972 = (float)Main.rand.Next(28, 42) * 0.005f;
                num972 += (float)(270 - (int)Main.mouseTextColor) / 500f;
                float num973 = 0.1f;
                float num974 = 0.3f + num972 / 2f;
                float num975 = 0.6f + num972;
                float num976 = 0.35f;
                num973 *= num976;
                num974 *= num976;
                num975 *= num976;
                Lighting.AddLight(Projectile.Center, num973, num974, num975);
            }
            Projectile.velocity = new Vector2(0f, (float)Math.Sin((double)(6.28318548f * Projectile.ai[0] / 180f)) * 0.15f);
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 180f)
            {
                Projectile.ai[0] = 0f;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, new Vector2((int)Projectile.position.X, (int)Projectile.position.Y));//, 27, 1f, 0f);
            for (int num147 = 0; num147 < 10; num147++)
            {
                int num148 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 165, 0f, 0f, 50, default(Color), 1.5f);
                Dust dust = Main.dust[num148];
                dust.velocity *= 2f;
                Main.dust[num148].noGravity = true;
            }
            float num149 = 0.6f + Main.rand.NextFloat() * 0.4f;
            int num150 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, 375, num149);
            Gore gore = Main.gore[num150];
            gore.velocity *= 0.3f;
            num150 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, 376, num149);
            gore = Main.gore[num150];
            gore.velocity *= 0.3f;
            num150 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, 377, num149);
            gore = Main.gore[num150];
            gore.velocity *= 0.3f;
        }
    }
    #endregion

}
