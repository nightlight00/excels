using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace excels.Buffs.Flasks
{
    public class ImbuedProjectile : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            // debuff is handled in excelPlayer, and since vanilla flasks dont visually effect whips, only melee projectiles get particles
            if (projectile.DamageType == DamageClass.Melee)
            {
                Player player = Main.player[projectile.owner];

                if (player.GetModPlayer<excelPlayer>().FlaskFrostfire)
                {
                    if (Main.rand.NextBool())
                    {
                        Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 92);
                        d.noGravity = true;
                        d.scale = Main.rand.NextFloat(1.1f, 1.23f) * projectile.scale;
                        d.velocity = projectile.velocity * -0.05f;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 92);
                        d.scale = Main.rand.NextFloat(0.4f, 0.7f) * projectile.scale;
                        d.velocity += new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(0, 1.5f));
                    }
                }
            }
        }
    }


    internal class GlacialImbueBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weapon Imbue: Frostfire");
            Description.SetDefault("Melee and Whip attacks burns foes with frostfire");
            Main.buffNoTimeDisplay[Type] = false;
            Main.meleeBuff[Type] = true;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<excelPlayer>().FlaskFrostfire = true;
        }
    }
}
