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
using Terraria.GameContent;
using ReLogic.Content;

namespace excels.Items.Weapons.Skyline
{
    #region Staff
    public class SkylineStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plummage Staff");
            Tooltip.SetDefault("Tickles foes with deadly feathers");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 13;
            Item.useTime = 7;
            Item.useAnimation = 21;
            Item.reuseDelay = 9;
            Item.knockBack = 2.3f;
            Item.noMelee = true;
            Item.height = Item.width = 46;
            Item.rare = 1;
            Item.mana = 8;
            Item.shoot = ModContent.ProjectileType<SkylineFeather>();
            Item.shootSpeed = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.sellPrice(0, 0, 20);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SkylineBar>(), 6)
                .AddIngredient(ItemID.Feather, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(6));
        }
    }

    public class SkylineFeather : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.HarpyFeather}";

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.HarpyFeather);
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 120;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 200;
        }

        public override void AI()
        {
            Projectile.alpha -= 10;
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15, newColor: new Color(0, 180, 230));
                d.noGravity = true;
                d.alpha = 150;
                d.scale = 0.9f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15, newColor: new Color(0, 180, 230));
                d.noGravity = true;
                d.alpha = 80;
                d.scale = 1.3f;
            }
        }
    }
    #endregion

    #region Bow
    public class SkylineBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drizzlebow");
            Tooltip.SetDefault("A soft downpour of arrows");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 15;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.reuseDelay = 14;
            Item.autoReuse = true;
            Item.knockBack = 3.4f;
            Item.noMelee = true;
            Item.height = 36;
            Item.width = 16;
            Item.rare = 1;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = 10;
            Item.shootSpeed = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.sellPrice(0, 0, 18);
            // Item.UseSound = SoundID.Item43;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SkylineBar>(), 6)
                .AddIngredient(ItemID.Feather, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            float ceilingLimit = target.Y;
            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }
            position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
            position.X = (player.Center.X + target.X) / 2 + Main.rand.NextFloat(-80, 80);
            position.Y -= 100;
            Vector2 heading = target - position;

            if (heading.Y < 0f)
            {
                heading.Y *= -1f;
            }

            if (heading.Y < 20f)
            {
                heading.Y = 20f;
            }

            heading.Normalize();
            heading *= velocity.Length();
            heading.Y += Main.rand.Next(-40, 41) * 0.02f;
            Projectile.NewProjectile(source, position, heading, type, damage, knockback, player.whoAmI);

            SoundEngine.PlaySound(SoundID.Item5, player.Center);
            for (var i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustDirect(position, 0, 0, 15, newColor: new Color(0, 180, 230));
                d.noGravity = true;
                d.alpha = 80;
                d.scale = 1.3f;
            }
            return false;
        }
    }
    #endregion

    #region Yoyo
    public class SkylineYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arial");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 17;
            Item.useTime = Item.useAnimation = 25;
            Item.knockBack = 1.8f;
            Item.noMelee = true;
            Item.height = 22;
            Item.width = 26;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<SkylineYoyoProj>();
            Item.shootSpeed = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.sellPrice(0, 0, 14);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SkylineBar>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class SkylineYoyoProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arial");
            // Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 7f;
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 240f;
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 18.5f;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 0;
        }
    }
    #endregion

    #region Whip
    public class FeatherDuster : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("8% summon tag damage\nYour summons will focus struck enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // This method quickly sets the whip's properties.
            // Mouse over to see its parameters.
            Item.DefaultToWhip(ModContent.ProjectileType<FeatherDusterWhip>(), 10, 2, 4, 20);

            Item.shootSpeed = 4;
            Item.rare = ItemRarityID.Blue;
            Item.sellPrice(0, 0, 16);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SkylineBar>(), 4)
                .AddIngredient(ItemID.Feather, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class FeatherDusterWhip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // This makes the projectile use whip collision detection and allows flasks to be applied to it.
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();

            Projectile.WhipSettings.Segments = 10;
            Projectile.WhipSettings.RangeMultiplier = 0.75f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Whips.SkylineWhipDebuff>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(damage * 0.85f);
        }

        // This method draws a line between all points of the whip, in case there's empty space between the sprites.
        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.White);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);

            DrawLine(list);

            Main.DrawWhip_WhipBland(Projectile, list);
            return false;
        }
    }
    #endregion

    #region Flail 
    public class Birdie : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;

            // This line will make the damage shown in the tooltip twice the actual Item.damage. This multiplier is used to adjust for the dynamic damage capabilities of the projectile.
            // When thrown directly at enemies, the flail projectile will deal double Item.damage, matching the tooltip, but deals normal damage in other modes.
            ItemID.Sets.ToolTipDamageMultiplier[Type] = 2f;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.useAnimation = 45; // The item's use time in ticks (60 ticks == 1 second.)
            Item.useTime = 45; // The item's use time in ticks (60 ticks == 1 second.)
            Item.knockBack = 5.5f; // The knockback of your flail, this is dynamically adjusted in the projectile code.
            Item.width = 32; // Hitbox width of the item.
            Item.height = 32; // Hitbox height of the item.
            Item.damage = 14; // The damage of your flail, this is dynamically adjusted in the projectile code.
            Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand
            Item.shoot = ModContent.ProjectileType<BirdieBall>(); // The flail projectile
            Item.shootSpeed = 12f; // The speed of the projectile measured in pixels per frame.
            Item.UseSound = SoundID.Item1; // The sound that this item makes when used
            Item.rare = ItemRarityID.Blue; // The color of the name of your item
            Item.DamageType = DamageClass.Melee; // Deals melee damage
            Item.channel = true;
            Item.noMelee = true; // This makes sure the item does not deal damage from the swinging animation
            Item.sellPrice(0, 0, 18);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SkylineBar>(), 8)
                .AddIngredient(ItemID.Feather, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BirdieBall : ModProjectile
    {
        private const string ChainTexturePath = "excels/Items/Weapons/Skyline/BirdieChain"; // The folder path to the flail chain sprite
        private enum AIState
        {
            Spinning,
            LaunchingForward,
            Retracting,
            UnusedState,
            ForcedRetracting,
            Ricochet,
            Dropping
        }

        // These properties wrap the usual ai and localAI arrays for cleaner and easier to understand code.
        private AIState CurrentAIState
        {
            get => (AIState)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }
        public ref float StateTimer => ref Projectile.ai[1];
        public ref float CollisionCounter => ref Projectile.localAI[0];
        public ref float SpinningStateTimer => ref Projectile.localAI[1];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Birdie");

            // These lines facilitate the trail drawing
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true; // This ensures that the projectile is synced when other players join the world.
            Projectile.width = 24; // The width of your projectile
            Projectile.height = 24; // The height of your projectile
            Projectile.friendly = true; // Deals damage to enemies
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.DamageType = DamageClass.Melee; // Deals melee damage
            Projectile.usesLocalNPCImmunity = true; // Used for hit cooldown changes in the ai hook
            Projectile.localNPCHitCooldown = 10; // This facilitates custom hit cooldown logic

            // Vanilla flails all use aiStyle 15, but the code isn't customizable so an adaption of that aiStyle is used in the AI method
        }

        // This AI code was adapted from vanilla code: Terraria.Projectile.AI_015_Flails() 
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            // Kill the projectile if the player dies or gets crowd controlled
            if (!player.active || player.dead || player.noItems || player.CCed || Vector2.Distance(Projectile.Center, player.Center) > 900f)
            {
                Projectile.Kill();
                return;
            }
            if (Main.myPlayer == Projectile.owner && Main.mapFullscreen)
            {
                Projectile.Kill();
                return;
            }

            Vector2 mountedCenter = player.MountedCenter;
            bool shouldOwnerHitCheck = false;
            int launchTimeLimit = 15;  // How much time the projectile can go before retracting (speed and shootTimer will set the flail's range)
            float launchSpeed = 18f; // How fast the projectile can move
            float maxLaunchLength = 800f; // How far the projectile's chain can stretch before being forced to retract when in launched state
            float retractAcceleration = 5f; // How quickly the projectile will accelerate back towards the player while retracting
            float maxRetractSpeed = 23f; // The max speed the projectile will have while retracting
            float forcedRetractAcceleration = 6f; // How quickly the projectile will accelerate back towards the player while being forced to retract
            float maxForcedRetractSpeed = 35f; // The max speed the projectile will have while being forced to retract
            float unusedRetractAcceleration = 1f;
            float unusedMaxRetractSpeed = 14f;
            int unusedChainLength = 60;
            int defaultHitCooldown = 10; // How often your flail hits when resting on the ground, or retracting
            int spinHitCooldown = 20; // How often your flail hits when spinning
            int movingHitCooldown = 10; // How often your flail hits when moving
            int ricochetTimeLimit = launchTimeLimit + 5;

            // Scaling these speeds and accelerations by the players meleeSpeed make the weapon more responsive if the player boosts their meleeSpeed
            float meleeSpeed = player.GetAttackSpeed(DamageClass.Melee);
            float meleeSpeedMultiplier = 1f / meleeSpeed;
            launchSpeed *= meleeSpeedMultiplier;
            unusedRetractAcceleration *= meleeSpeedMultiplier;
            unusedMaxRetractSpeed *= meleeSpeedMultiplier;
            retractAcceleration *= meleeSpeedMultiplier;
            maxRetractSpeed *= meleeSpeedMultiplier;
            forcedRetractAcceleration *= meleeSpeedMultiplier;
            maxForcedRetractSpeed *= meleeSpeedMultiplier;
            float launchRange = launchSpeed * launchTimeLimit;
            float maxDroppedRange = launchRange + 160f;
            Projectile.localNPCHitCooldown = defaultHitCooldown;

            switch (CurrentAIState)
            {
                case AIState.Spinning:
                    {
                        shouldOwnerHitCheck = true;
                        if (Projectile.owner == Main.myPlayer)
                        {
                            Vector2 unitVectorTowardsMouse = mountedCenter.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.UnitX * player.direction);
                            player.ChangeDir((unitVectorTowardsMouse.X > 0f) ? 1 : (-1));
                            if (!player.channel) // If the player releases then change to moving forward mode
                            {
                                CurrentAIState = AIState.LaunchingForward;
                                StateTimer = 0f;
                                Projectile.velocity = unitVectorTowardsMouse * launchSpeed + player.velocity;
                                Projectile.Center = mountedCenter;
                                Projectile.netUpdate = true;
                                Projectile.ResetLocalNPCHitImmunity();
                                Projectile.localNPCHitCooldown = movingHitCooldown;
                                break;
                            }
                        }
                        SpinningStateTimer += 1f;
                        // This line creates a unit vector that is constantly rotated around the player. 10f controls how fast the projectile visually spins around the player
                        Vector2 offsetFromPlayer = new Vector2(player.direction).RotatedBy((float)Math.PI * 10f * (SpinningStateTimer / 60f) * player.direction);

                        offsetFromPlayer.Y *= 0.8f;
                        if (offsetFromPlayer.Y * player.gravDir > 0f)
                        {
                            offsetFromPlayer.Y *= 0.5f;
                        }
                        Projectile.Center = mountedCenter + offsetFromPlayer * 30f;
                        Projectile.velocity = Vector2.Zero;
                        Projectile.localNPCHitCooldown = spinHitCooldown; // set the hit speed to the spinning hit speed
                        break;
                    }
                case AIState.LaunchingForward:
                    {
                        bool shouldSwitchToRetracting = StateTimer++ >= launchTimeLimit;
                        shouldSwitchToRetracting |= Projectile.Distance(mountedCenter) >= maxLaunchLength;
                        if (player.controlUseItem) // If the player clicks, transition to the Dropping state
                        {
                            CurrentAIState = AIState.Dropping;
                            StateTimer = 0f;
                            Projectile.netUpdate = true;
                            Projectile.velocity *= 0.2f;
                            // This is where Drippler Crippler spawns its projectile
                            /*
							if (Main.myPlayer == Projectile.owner)
								Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity, 928, Projectile.damage, Projectile.knockBack, Main.myPlayer);
							*/
                            break;
                        }
                        if (shouldSwitchToRetracting)
                        {
                            CurrentAIState = AIState.Retracting;
                            StateTimer = 0f;
                            Projectile.netUpdate = true;
                            Projectile.velocity *= 0.3f;
                            // This is also where Drippler Crippler spawns its projectile, see above code.
                        }
                        player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        Projectile.localNPCHitCooldown = movingHitCooldown;
                        break;
                    }
                case AIState.Retracting:
                    {
                        Vector2 unitVectorTowardsPlayer = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
                        if (Projectile.Distance(mountedCenter) <= maxRetractSpeed)
                        {
                            Projectile.Kill(); // Kill the projectile once it is close enough to the player
                            return;
                        }
                        if (player.controlUseItem) // If the player clicks, transition to the Dropping state
                        {
                            CurrentAIState = AIState.Dropping;
                            StateTimer = 0f;
                            Projectile.netUpdate = true;
                            Projectile.velocity *= 0.2f;
                        }
                        else
                        {
                            Projectile.velocity *= 0.98f;
                            Projectile.velocity = Projectile.velocity.MoveTowards(unitVectorTowardsPlayer * maxRetractSpeed, retractAcceleration);
                            player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        }
                        break;
                    }
                case AIState.UnusedState: // Projectile.ai[0] == 3; This case is actually unused, but maybe a Terraria update will add it back in, or maybe it is useless, so I left it here.
                    {
                        if (!player.controlUseItem)
                        {
                            CurrentAIState = AIState.ForcedRetracting; // Move to super retracting mode if the player taps
                            StateTimer = 0f;
                            Projectile.netUpdate = true;
                            break;
                        }
                        float currentChainLength = Projectile.Distance(mountedCenter);
                        Projectile.tileCollide = StateTimer == 1f;
                        bool flag3 = currentChainLength <= launchRange;
                        if (flag3 != Projectile.tileCollide)
                        {
                            Projectile.tileCollide = flag3;
                            StateTimer = Projectile.tileCollide ? 1 : 0;
                            Projectile.netUpdate = true;
                        }
                        if (currentChainLength > unusedChainLength)
                        {

                            if (currentChainLength >= launchRange)
                            {
                                Projectile.velocity *= 0.5f;
                                Projectile.velocity = Projectile.velocity.MoveTowards(Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero) * unusedMaxRetractSpeed, unusedMaxRetractSpeed);
                            }
                            Projectile.velocity *= 0.98f;
                            Projectile.velocity = Projectile.velocity.MoveTowards(Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero) * unusedMaxRetractSpeed, unusedRetractAcceleration);
                        }
                        else
                        {
                            if (Projectile.velocity.Length() < 6f)
                            {
                                Projectile.velocity.X *= 0.96f;
                                Projectile.velocity.Y += 0.2f;
                            }
                            if (player.velocity.X == 0f)
                            {
                                Projectile.velocity.X *= 0.96f;
                            }
                        }
                        player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        break;
                    }
                case AIState.ForcedRetracting:
                    {
                        Projectile.tileCollide = false;
                        Vector2 unitVectorTowardsPlayer = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
                        if (Projectile.Distance(mountedCenter) <= maxForcedRetractSpeed)
                        {
                            Projectile.Kill(); // Kill the projectile once it is close enough to the player
                            return;
                        }
                        Projectile.velocity *= 0.98f;
                        Projectile.velocity = Projectile.velocity.MoveTowards(unitVectorTowardsPlayer * maxForcedRetractSpeed, forcedRetractAcceleration);
                        Vector2 target = Projectile.Center + Projectile.velocity;
                        Vector2 value = mountedCenter.DirectionFrom(target).SafeNormalize(Vector2.Zero);
                        if (Vector2.Dot(unitVectorTowardsPlayer, value) < 0f)
                        {
                            Projectile.Kill(); // Kill projectile if it will pass the player
                            return;
                        }
                        player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        break;
                    }
                case AIState.Ricochet:
                    if (StateTimer++ >= ricochetTimeLimit)
                    {
                        CurrentAIState = AIState.Dropping;
                        StateTimer = 0f;
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        Projectile.localNPCHitCooldown = movingHitCooldown;
                        Projectile.velocity.Y += 0.6f;
                        Projectile.velocity.X *= 0.95f;
                        player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                    }
                    break;
                case AIState.Dropping:
                    if (!player.controlUseItem || Projectile.Distance(mountedCenter) > maxDroppedRange)
                    {
                        CurrentAIState = AIState.ForcedRetracting;
                        StateTimer = 0f;
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        Projectile.velocity.Y += 0.8f;
                        Projectile.velocity.X *= 0.95f;
                        player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                    }
                    break;
            }

            // This is where Flower Pow launches projectiles. Decompile Terraria to view that code.

            Projectile.direction = (Projectile.velocity.X > 0f) ? 1 : -1;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.ownerHitCheck = shouldOwnerHitCheck; // This prevents attempting to damage enemies without line of sight to the player. The custom Colliding code for spinning makes this necessary.

            // If you have a ball shaped flail, you can use this simplified rotation code instead
            
			if (Projectile.velocity.Length() > 1f)
				Projectile.rotation = Projectile.velocity.ToRotation() + Projectile.velocity.X * 0.1f; // skid
			else
				Projectile.rotation += Projectile.velocity.X * 0.1f; // roll
			

            Projectile.timeLeft = 2; // Makes sure the flail doesn't die (good when the flail is resting on the ground)
            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2); //Add a delay so the player can't button mash the flail
            player.itemRotation = Projectile.DirectionFrom(mountedCenter).ToRotation();
            if (Projectile.Center.X < mountedCenter.X)
            {
                player.itemRotation += (float)Math.PI;
            }
            player.itemRotation = MathHelper.WrapAngle(player.itemRotation);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int defaultLocalNPCHitCooldown = 10;
            int impactIntensity = 0;
            Vector2 velocity = Projectile.velocity;
            float bounceFactor = 0.2f;
            if (CurrentAIState == AIState.LaunchingForward || CurrentAIState == AIState.Ricochet)
                bounceFactor = 0.4f;

            if (CurrentAIState == AIState.Dropping)
                bounceFactor = 0f;

            if (oldVelocity.X != Projectile.velocity.X)
            {
                if (Math.Abs(oldVelocity.X) > 4f)
                    impactIntensity = 1;

                Projectile.velocity.X = (0f - oldVelocity.X) * bounceFactor;
                CollisionCounter += 1f;
            }

            if (oldVelocity.Y != Projectile.velocity.Y)
            {
                if (Math.Abs(oldVelocity.Y) > 4f)
                    impactIntensity = 1;

                Projectile.velocity.Y = (0f - oldVelocity.Y) * bounceFactor;
                CollisionCounter += 1f;
            }

            // If in the Launched state, spawn sparks
            if (CurrentAIState == AIState.LaunchingForward)
            {
                CurrentAIState = AIState.Ricochet;
                Projectile.localNPCHitCooldown = defaultLocalNPCHitCooldown;
                Projectile.netUpdate = true;
                Point scanAreaStart = Projectile.TopLeft.ToTileCoordinates();
                Point scanAreaEnd = Projectile.BottomRight.ToTileCoordinates();
                impactIntensity = 2;
                Projectile.CreateImpactExplosion(2, Projectile.Center, ref scanAreaStart, ref scanAreaEnd, Projectile.width, out bool causedShockwaves);
                Projectile.CreateImpactExplosion2_FlailTileCollision(Projectile.Center, causedShockwaves, velocity);
                Projectile.position -= velocity;
            }

            // Here the tiles spawn dust indicating they've been hit
            if (impactIntensity > 0)
            {
                Projectile.netUpdate = true;
                for (int i = 0; i < impactIntensity; i++)
                {
                    Collision.HitTiles(Projectile.position, velocity, Projectile.width, Projectile.height);
                }

                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }

            // Force retraction if stuck on tiles while retracting
            if (CurrentAIState != AIState.UnusedState && CurrentAIState != AIState.Spinning && CurrentAIState != AIState.Ricochet && CurrentAIState != AIState.Dropping && CollisionCounter >= 10f)
            {
                CurrentAIState = AIState.ForcedRetracting;
                Projectile.netUpdate = true;
            }

            // tModLoader currently does not provide the wetVelocity parameter, this code should make the flail bounce back faster when colliding with tiles underwater.
            //if (Projectile.wet)
            //	wetVelocity = Projectile.velocity;

            return false;
        }

        public override bool? CanDamage()
        {
            // Flails in spin mode won't damage enemies within the first 12 ticks. Visually this delays the first hit until the player swings the flail around for a full spin before damaging anything.
            if (CurrentAIState == AIState.Spinning && SpinningStateTimer <= 12f)
                return false;
            return base.CanDamage();
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Flails do special collision logic that serves to hit anything within an ellipse centered on the player when the flail is spinning around the player. For example, the projectile rotating around the player won't actually hit a bee if it is directly on the player usually, but this code ensures that the bee is hit. This code makes hitting enemies while spinning more consistant and not reliant of the actual position of the flail projectile.
            if (CurrentAIState == AIState.Spinning)
            {
                Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
                Vector2 shortestVectorFromPlayerToTarget = targetHitbox.ClosestPointInRect(mountedCenter) - mountedCenter;
                shortestVectorFromPlayerToTarget.Y /= 0.8f; // Makes the hit area an ellipse. Vertical hit distance is smaller due to this math.
                float hitRadius = 55f; // The length of the semi-major radius of the ellipse (the long end)
                return shortestVectorFromPlayerToTarget.Length() <= hitRadius;
            }
            // Regular collision logic happens otherwise.
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void ModifyDamageScaling(ref float damageScale)
        {
            // Flails do 20% more damage while spinning
            if (CurrentAIState == AIState.Spinning)
                damageScale *= 1.2f;

            // Flails do 100% more damage while launched or retracting. This is the damage the item tooltip for flails aim to match, as this is the most common mode of attack. This is why the item has ItemID.Sets.ToolTipDamageMultiplier[Type] = 2f;
            if (CurrentAIState == AIState.LaunchingForward || CurrentAIState == AIState.Retracting)
                damageScale *= 2f;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            // Flails do a few custom things, you'll want to keep these to have the same feel as vanilla flails.

            // The hitDirection is always set to hit away from the player, even if the flail damages the npc while returning
            hitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);

            // Knockback is only 25% as powerful when in spin mode
            if (CurrentAIState == AIState.Spinning)
                knockback *= 0.25f;
            // Knockback is only 50% as powerful when in drop down mode
            if (CurrentAIState == AIState.Dropping)
                knockback *= 0.5f;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        // PreDraw is used to draw a chain and trail before the projectile is drawn normally.
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);

            // This fixes a vanilla GetPlayerArmPosition bug causing the chain to draw incorrectly when stepping up slopes. The flail itself still draws incorrectly due to another similar bug. This should be removed once the vanilla bug is fixed.
            playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;

            Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>(ChainTexturePath);
            Rectangle? chainSourceRectangle = null;
            // Drippler Crippler customizes sourceRectangle to cycle through sprite frames: sourceRectangle = asset.Frame(1, 6);
            float chainHeightAdjustment = 0f; // Use this to adjust the chain overlap. 

            Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 2f) : (chainTexture.Size() / 2f);
            Vector2 chainDrawPosition = Projectile.Center;
            Vector2 vectorFromProjectileToPlayerArms = playerArmPosition.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
            Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
                chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
            float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
            int chainCount = 0;
            float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

            // This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
            while (chainLengthRemainingToDraw > 0f)
            {
                // This code gets the lighting at the current tile coordinates
                Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));

                // Flaming Mace and Drippler Crippler use code here to draw custom sprite frames with custom lighting.
                // Cycling through frames: sourceRectangle = asset.Frame(1, 6, 0, chainCount % 6);
                // This example shows how Flaming Mace works. It checks chainCount and changes chainTexture and draw color at different values

                var chainTextureToDraw = chainTexture;
                // Here, we draw the chain texture at the coordinates
                Main.spriteBatch.Draw(chainTextureToDraw.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

                // chainDrawPosition is advanced along the vector back to the player by the chainSegmentLength
                chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
                chainCount++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }

            // Add a motion trail when moving forward, like most flails do (don't add trail if already hit a tile)
            if (CurrentAIState == AIState.LaunchingForward)
            {
                Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 drawOrigin = new Vector2(projectileTexture.Width * 0.5f, Projectile.height * 0.5f);
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (Projectile.spriteDirection == -1)
                    spriteEffects = SpriteEffects.FlipHorizontally;
                for (int k = 0; k < Projectile.oldPos.Length && k < StateTimer; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.spriteBatch.Draw(projectileTexture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length / 3, spriteEffects, 0f);
                }
            }
            return true;
        }
    }
    #endregion

    #region Summon 

    public class SkylineCaneBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lil' Sentinal");
            Description.SetDefault("Danger from above");

            Main.buffNoSave[Type] = true; 
            Main.buffNoTimeDisplay[Type] = true; 
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // If the minions exist reset the buff time, otherwise remove the buff from the player
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SkylineCaneMinion>()] > 0)
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

    public class SkylineCane : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skyline Cane");
            Tooltip.SetDefault("Summons a lil' sentinal to fight for you");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 11;
            Item.useTime = Item.useAnimation = 25;
            Item.knockBack = 2.1f;
            Item.noMelee = true;
            Item.height = Item.width = 42;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<SkylineCaneMinion>();
            Item.buffType = ModContent.BuffType<SkylineCaneBuff>();
            Item.shootSpeed = 1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.mana = 10;
            Item.UseSound = SoundID.Item44;
            Item.sellPrice(0, 0, 25);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SkylineBar>(), 6)
                .AddIngredient(ItemID.Feather, 4)
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

    public class SkylineCaneMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lil' Sentinal");
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // Projectile is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to Projectile Projectile, as it's resistant to all homing Projectiles.
        }

        public sealed override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.tileCollide = false;

            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
        }

        public override bool? CanCutTiles() => false;

        public override bool MinionContactDamage() => false;

        protected float viewDist = 450f;
        protected float maxSpeed = 3;
        protected float idleAmt = 0.3f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // if owner alive and buff handling
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<SkylineCaneBuff>());
            }
            if (player.HasBuff(ModContent.BuffType<SkylineCaneBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            #region Check for Target
            Vector2 targetPos = Projectile.position;
            float targetDist = viewDist;
            bool target = false;
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                // changed so that it uses checks if player has line of sight with npc, not the actual summon
                if (Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height))
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

            // idle position
            if (!target)
            {
                Vector2 direction = player.Center;
                Projectile.ai[1] = 3600f;
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
                direction.Y -= 40f;

                targetPos = direction;
                if (Vector2.Distance(targetPos, Projectile.Center) > 2000)
                {
                    Projectile.Center = targetPos;
                }
            }
            #endregion

            #region Visuals
            Projectile.rotation = Projectile.velocity.X * 0.05f;
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
            if (Projectile.velocity.X > 0f)
            {
                Projectile.spriteDirection = Projectile.direction = -1;
            }
            else if (Projectile.velocity.X < 0f)
            {
                Projectile.spriteDirection = Projectile.direction = 1;
            }
            #endregion

            // this handles different movement options
            if (target)
            {
                maxSpeed = 4;
                // only adjust speed if distant to target
                if (Vector2.Distance(targetPos, Projectile.Center) > 140)
                {
                    AdjustVelocity(targetPos, 1.2f);
                }
                // only shooting can occur if there is a target
                Projectile.ai[0]++;
                if (Main.rand.NextBool(5))
                {
                    Projectile.ai[0]++;
                }
                if (Projectile.ai[0] > 30 && Vector2.Distance(targetPos, Projectile.Center) < 300) // idk the actual range of the projectile just a guess
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Vector2 shootVel = targetPos - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 5;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, shootVel.X, shootVel.Y, ModContent.ProjectileType<SkylineCaneProj>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                        Projectile.ai[0] = 0;
                    }
                }
            }
            else
            {
                // increases max speed if far from player
                if (Vector2.Distance(targetPos, Projectile.Center) > 500) {
                    maxSpeed = 7;
                }
                else {
                    maxSpeed -= 0.05f;
                    if (maxSpeed < 1.4f)
                    {
                        maxSpeed = 2;
                    }
                }
                AdjustVelocity(targetPos, idleAmt);
                // semi prepared to attack, but not completely (just so feels more alert)
                Projectile.ai[0] = 12;
                if (++Projectile.ai[1] > 60)
                {
                    // add a little randomness to idle speed
                    idleAmt = Main.rand.NextFloat(0.25f, 0.36f);
                    Projectile.ai[1] = 0;
                }
            }
        }

        private void AdjustVelocity(Vector2 pos, float mult)
        {
            if (pos.X > Projectile.Center.X)
            {
                Projectile.velocity.X += 0.2f * mult;
                if (Projectile.velocity.X > maxSpeed) { Projectile.velocity.X = maxSpeed; }
            }
            else
            {
                Projectile.velocity.X -= 0.2f * mult;
                if (Projectile.velocity.X < -maxSpeed) { Projectile.velocity.X = -maxSpeed; }
            }
            if (pos.Y > Projectile.Center.Y - 40)
            {
                Projectile.velocity.Y += 0.1f * mult;
                if (Projectile.velocity.Y > maxSpeed) { Projectile.velocity.Y = maxSpeed; }
            }
            else
            {
                Projectile.velocity.Y -= 0.2f * mult;
                if (Projectile.velocity.Y < -maxSpeed) { Projectile.velocity.Y = -maxSpeed; }
            }
        }
    }
    
    public class SkylineCaneProj : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.HarpyFeather}";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Space Laser");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 69;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 5;
        }
        public override bool MinionContactDamage() => true;

        public override void AI()
        {
            Projectile.ai[0]++;
            // so it appears to spawn in front and not on top of the sentinal
            if (Projectile.ai[0] > 6)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15, newColor: new Color(0, 180, 230));
                d.noGravity = true;
                d.velocity *= 0.25f;
                d.alpha = 150;
                d.scale = 0.9f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15, newColor: new Color(0, 180, 230));
                d.noGravity = true;
                d.alpha = 80;
                d.scale = 1.3f;
            }
        }
    }

    #endregion 
}
