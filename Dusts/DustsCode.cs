using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;


namespace excels.Dusts
{
    internal class EnergyDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 1.1f;
        //    dust.scale = 1.3f;
            dust.noGravity = true;
            dust.alpha = 230;
        }
        
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            if (!dust.noLight)
            {
                dust.color = Color.White * 0.8f;
                Lighting.AddLight(dust.position, Color.LightBlue.ToVector3() * 0.18f);
                return Color.White;
            }
            return base.GetAlpha(dust, lightColor);
        }

        public override bool Update(Dust dust)
        {
            return true;
            if (!dust.noGravity)
            {
                return true;
                dust.velocity.Y += 0.02f;
                dust.velocity.X *= 0.97f;
            }
            else
            {
                dust.velocity *= 0.987f;
            }

            if (dust.velocity.X < 0.1f || dust.velocity.X > -0.1f)
            {
                dust.rotation += MathHelper.ToRadians(dust.velocity.X * 2);
            }

            if (!dust.noLight)
            {
                Lighting.AddLight(dust.position, 1.14f * dust.scale * 0.16f, 2.36f * dust.scale * 0.16f, 2.55f * dust.scale * 0.16f);
            }

            dust.scale -= 0.05f;
            if (dust.scale < 0.3f)
            {
                dust.active = false;
            }

            return false;
        }
    }

    internal class FloralDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = true;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            if (!dust.noLight)
            {
                dust.color = Color.White * 0.8f;
                Lighting.AddLight(dust.position, Color.LightBlue.ToVector3() * 0.18f);
                return Color.White;
            }
            return base.GetAlpha(dust, lightColor);
        }
    }

    internal class ShadowFire2 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity.Y = Main.rand.Next(-10, 6) * 0.1f;
            dust.velocity.X *= 0.3f;
            dust.scale *= 0.7f;
        }

        public override bool MidUpdate(Dust dust)
        {
            if (dust.noLight)
            {
                return false;
            }

            float strength = dust.scale * 1.4f;
            if (strength > 1f)
            {
                strength = 1f;
            }
            Lighting.AddLight(dust.position, 0.5f * strength, 0.15f * strength, 0.95f * strength);
            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
            => new Color(lightColor.R, lightColor.G, lightColor.B, 140);
    }

    internal class ChasmHeadDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = Main.rand.NextFloat(1, 1.3f);
        }

        public override bool Update(Dust dust)
        {
            return true;
        }
    }

    internal class ChasmBodyDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = Main.rand.NextFloat(1, 1.3f);
        }

        public override bool Update(Dust dust)
        {
            return true;
        }
    }

    internal class GlacialDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = Main.rand.NextFloat(1, 1.2f);
        }

        public override bool Update(Dust dust)
        {
            return true;
        }
    }

    internal class SkylineDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = Main.rand.NextFloat(1, 1.2f);
        }

        public override bool Update(Dust dust)
        {
            return true;
        }
    }

    internal class StellarDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = Main.rand.NextFloat(1, 1.2f);
        }

        public override bool Update(Dust dust)
        {
            return true;
        }
    }
    internal class FossilBoneDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = Main.rand.NextFloat(1.15f, 1.35f);
        }

        public override bool Update(Dust dust)
        {
            return true;
        }
    }

    internal class HyperionEnergyDust : ModDust
    {
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            if (!dust.noLight)
            {
                dust.color = Color.White * 0.8f;
                Lighting.AddLight(dust.position, Color.CornflowerBlue.ToVector3() * 0.18f);
                return Color.White;
            }
            return base.GetAlpha(dust, lightColor);
        }

        public override bool Update(Dust dust)
        {
            return true;
        }

    }

    internal class HyperionMetalDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = Main.rand.NextFloat(0.9f, 1.15f);
        }
        public override bool Update(Dust dust)
        {
            return true;
        }
    }
}
