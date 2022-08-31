using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace excels.Items.Weapons.Eclipse
{
    internal class Harbringer : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbinger");
            Tooltip.SetDefault("Spawns a controllable infection");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.width = Item.height = 30;
            Item.useTime = Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 10000;
            Item.rare = 4;
            Item.UseSound = SoundID.Item43;
            Item.shoot = ModContent.ProjectileType<HarbringerAura>();
            Item.shootSpeed = 2f;
            Item.noMelee = true;
            Item.knockBack = 0.3f;
            Item.channel = true;
            Item.sellPrice(0, 3);

            clericEvil = true;
            clericBloodCost = 20;
            skullPendantOverride = true;
        }


        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] < 1)
            {
                CheckSkullPendant(player, clericBloodCostTrue);
                return true;
            }

            return false;
        }
    }

    internal class HarbringerAura : clericHealProj
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.width = Projectile.height = 128;
            Projectile.ownerHitCheck = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 50;

            clericEvil = true;
            canDealDamage = true;
        }

        int HealAmount = 0;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.type == NPCID.TargetDummy || HealAmount >= 30)
                return;

            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, new Vector2(4).RotatedByRandom(MathHelper.ToRadians(180)), ProjectileID.VampireHeal, 0, 0, Main.player[Projectile.owner].whoAmI);
            p.ai[0] = Main.player[Projectile.owner].whoAmI;
            p.ai[1] = 2;

            HealAmount += 2;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.active)
                Projectile.Kill();
            player.itemAnimation = 2; // player.itemTime = 2;
            Projectile.timeLeft = 2;
            player.heldProj = Projectile.whoAmI;
            Projectile.velocity = Vector2.Zero;

            if (Main.myPlayer == Projectile.owner)
            {
                if (player.channel)
                    Projectile.Center = Main.MouseWorld;
                else if (!player.channel || Projectile.damage <= 5)
                    Projectile.Kill();
            }

            if (++Projectile.ai[0] % 15 == 14)
            {
                player.statLife -= 3;
                CombatText.NewText(player.getRect(), Color.Red, 3);
                CheckSkullPendant(player, 3);
                if (player.statLife <= 0)
                {
                    player.KillMe(PlayerDeathReason.ByPlayer(player.whoAmI), 3, 0);
                }
            }

            // 60 = -30 healamount

            if (++Projectile.ai[1] % 10 == 0)
                HealAmount -= 5;
            if (HealAmount < 0)
                HealAmount = 0;

            Projectile.rotation += MathHelper.ToRadians(3);
        }
    }
}
