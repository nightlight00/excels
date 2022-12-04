using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace excels.Utilities
{
    internal class EffectsSystem : ModSystem
    {
        public class ScreenShake
        {
            public float Intensity;
            public float MultiplyPerTick;

            private Vector2 _shake;

            public void Update()
            {
                if (Intensity > 0f)
                {
                    _shake = (Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(Intensity * 0.8f, Intensity)).Floor();
                    Intensity *= MultiplyPerTick;
                }
                else
                {
                    Clear();
                }
            }

            public Vector2 GetScreenOffset()
            {
                return _shake;
            }

            public void Set(float intensity, float multiplier = 0.9f)
            {
                Intensity = intensity;
                MultiplyPerTick = multiplier;
            }
            public void Clear()
            {
                Intensity = 0f;
                MultiplyPerTick = 0.9f;
                _shake = new Vector2();
            }
        }

        public static ScreenShake Shake { get; private set; }

        public override void Load()
        {
            Shake = new ScreenShake();
        }

        public override void Unload()
        {
            Shake = null;
        }

        public override void PreUpdatePlayers()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Shake.Update();
            }
        }

        internal static void UpdateScreenPosition()
        {
            Main.screenPosition += Shake.GetScreenOffset() * ModContent.GetInstance<excelConfig>().ScreenShakeIntensity;
        }
    }
}
