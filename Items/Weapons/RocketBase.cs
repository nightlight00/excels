using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using System.IO;
using Terraria.Audio;

namespace excels.Items.Weapons
{
    public abstract class RocketLauncher : ModItem
    {
        private int FireVanillaRocket(Projectile desiredRocket, string type = "grenade")
        {
            switch (type)
            {
                case "grenade":
                    switch (desiredRocket)
                    {
                     //   case ProjectileID.GrenadeI:
                     //       break;
                    }
                    break;
            }
            return 0;
        }

        private void FireModdedRocket(Projectile proj, Item ammo)
        {
           
        }
    }


    public abstract class RocketProjectileBase : ModProjectile
    {
        public override void Kill(int timeLeft)
        {
            switch (Projectile.ai[0])
            {

            }
        }
    }
}
