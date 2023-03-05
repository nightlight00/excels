using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;

namespace excels.Items.Armor.DungeonNecro.Necromancer.SetBonus
{
    internal class GhastlySkull : clericHealProj
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;

            clericEvil = true;
        }

        int timer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
                Projectile.Kill();

            // Dust if hasn't exploded
            if (Projectile.ai[0] != 3) {
                for (var i = 0; i < 1 + Projectile.ai[0]/2; i++) {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 180);
                    d.velocity = Projectile.velocity * 0.8f;
                    d.scale = 1.1f + Projectile.velocity.Length() / 5;
                    d.noGravity = true;
                }
                Projectile.timeLeft = 10;
            }

            // Visuals
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);

            // Logic
            switch (Projectile.ai[0]) {
                // Idling around owner
                case 0:

                    // Fly near player
                    if (Vector2.Distance(player.Center, Projectile.Center) > 30)
                    {
                        if (Vector2.Distance(player.Center, Projectile.Center) > 900)
                            Projectile.Center = player.Center;

                        AdjustVelocity(player.Center, 0.3f, 4);
                    }

                    // Check for target
                    int targetID = -1;
                    Vector2 targetPos = Vector2.Zero;
                    float targetDistance = 400;
                    for (var i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC checkTarget = Main.npc[i];
                        if (checkTarget.CanBeChasedBy() && Vector2.Distance(checkTarget.Center, Projectile.Center) < targetDistance && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, checkTarget.position, checkTarget.width, checkTarget.height))
                        {
                            targetID = i;
                            targetPos = checkTarget.Center;
                            targetDistance = Vector2.Distance(checkTarget.Center, Projectile.Center);
                        }
                    }

                    timer++;
                    if (targetID != -1 && timer > 45)
                    {
                        Projectile.ai[0] = 1;
                        Projectile.ai[1] = targetID;
                    }
                    break;

                // Preparing to hit target
                case 1:

                    NPC target = Main.npc[(int)Projectile.ai[1]];

                    // If target has died
                    if (!target.active) {
                        Projectile.ai[0] = 0;
                        return;
                    }

                    timer++;
                    if (timer > 30)
                    {
                        canDealDamage = true;
                        Projectile.ai[0] = 2;
                        Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 9;
                        timer = 0;
                        return;
                    }

                    break;

                // Flying towards target
                case 2:

                    // Unsuccessfully hit target
                    timer++;
                    if (timer > 30) {
                        canDealDamage = false;
                        Projectile.ai[0] = 0;
                        timer = 0;
                    }

                    break;

            }
        }

        private void AdjustVelocity(Vector2 pos, float mult, float maxSpeed)
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
}
