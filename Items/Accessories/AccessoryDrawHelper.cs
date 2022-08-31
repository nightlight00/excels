using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.Audio;
using Terraria.GameContent;

namespace excels.Items.Accessories
{
    internal class AccessoryDrawHelper : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.Mushroom}";
        public override void SetDefaults()
        {
            Projectile.timeLeft = 2;
            Projectile.alpha = 255;
        }

        float beetleRotation = 0;
        int beetleFrameCounter = 0;
        int beetleFrame = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.position = player.position;
            Projectile.timeLeft = 2;
            switch (Projectile.ai[0])
            {
                case 1:
                    if (!player.GetModPlayer<excelPlayer>().BeetleShield)
                    {
                        Projectile.Kill();
                    }
                    if (++beetleFrameCounter >= 2)
                    {
                        if (++beetleFrame >= 3)
                        {
                            beetleFrame = 0;
                        }
                        beetleFrameCounter = 0;
                    }
                    break;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            switch (Projectile.ai[0])
            {
                case 1:
                    beetleRotation += 0.08f;

                    SpriteEffects spriteEffects = SpriteEffects.None;
                    if (Projectile.spriteDirection == -1)
                        spriteEffects = SpriteEffects.FlipHorizontally;

                    Texture2D texture = TextureAssets.Beetle.Value; // [ExtrasID. TextureAssets.Projectile[ProjectileID.beet].Value;
                    Rectangle sourceRectangle = new Rectangle(0, (texture.Height / 3) * beetleFrame, texture.Width, texture.Height / 3);
                    Vector2 drawOrigin = sourceRectangle.Size() / 2;


                    Vector2 drawPos = new Vector2(MathF.Cos(beetleRotation) * 48, MathF.Sin(beetleRotation) * 48);
                    Vector2 drawPos2 = new Vector2(MathF.Cos(beetleRotation + MathHelper.ToRadians(120)) * 48, MathF.Sin(beetleRotation + MathHelper.ToRadians(120)) * 48);
                    Vector2 drawPos3 = new Vector2(MathF.Cos(beetleRotation + MathHelper.ToRadians(240)) * 48, MathF.Sin(beetleRotation + MathHelper.ToRadians(240)) * 48);

                    Main.EntitySpriteDraw(texture, player.Center - Main.screenPosition + drawPos, sourceRectangle, Color.White,
                        drawPos.ToRotation() + MathHelper.ToRadians(90), drawOrigin, 1, spriteEffects, 0);
                    Main.EntitySpriteDraw(texture, player.Center - Main.screenPosition + drawPos2, sourceRectangle, Color.White,
                        drawPos2.ToRotation() + MathHelper.ToRadians(90), drawOrigin, 1, spriteEffects, 0);
                    Main.EntitySpriteDraw(texture, player.Center - Main.screenPosition + drawPos3, sourceRectangle, Color.White,
                        drawPos3.ToRotation() + MathHelper.ToRadians(90), drawOrigin, 1, spriteEffects, 0);
                    break;
            }
            return false;
        }
    }
}
