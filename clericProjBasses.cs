using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections;
using System.Collections.Generic;

namespace excels
{
    public abstract class clericProj : ModProjectile
    {
        public bool clericEvil = false;
        public bool clericPotion = false;

        public int healPower = -1;
        public float healRate = 1;

        public virtual void SafeSetDefaults()
        {
        }

        public sealed override void SetDefaults()
        {
            Projectile.netImportant = true;
            PreDefaults();
            SafeSetDefaults();
            // Projectile.DamageType = ModContent.GetInstance<ClericClass>();


        }

        public virtual void PreDefaults()
        {

        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.GetGlobalNPC<excelNPC>().SpellCurse > 0 && !clericPotion)
            {
                damage = (int)(damage * 1.2f);
            }
           // base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public void CheckSkullPendant(Player player, int increase)
        {
            if (!player.GetModPlayer<excelPlayer>().skullPendant)
                return;

            player.GetModPlayer<excelPlayer>().skullPendantBlood += increase;
            if (player.GetModPlayer<excelPlayer>().skullPendantBlood >= 60)
            {
                player.GetModPlayer<excelPlayer>().skullPendantBlood -= 60;

                player.statLife += 10;
                if (player.statLife > player.statLifeMax2)
                    player.statLife = player.statLifeMax2;
                player.HealEffect(10);

                if (player.GetModPlayer<excelPlayer>().skullPendant2)
                {
                    player.immune = true;
                    player.immuneTime = 45;
                }
                if (player.GetModPlayer<excelPlayer>().skullPendantFrost)
                    player.GetModPlayer<excelPlayer>().SnowflakeAmulet();

                for (var i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustPerfect(player.Center, 90);
                    d.velocity = new Vector2(2).RotatedBy(MathHelper.ToRadians((360 / 20) * i));
                    d.noGravity = true;
                    d.scale = 1.3f;
                }

                NetMessage.SendData(66, -1, -1, null, player.whoAmI, 10, 0f, 0f, 0, 0, 0);
            }

        }
    }


    public abstract class clericHealProj : clericProj
    {
        public List<int> heallist = new List<int>();

        public int healPenetrate = 1;
        public bool healUsesBuffs = false;
        public bool canHealOwner = false;
        public bool buffConsumesPenetrate = false;

        public bool canDealDamage = false;

        public override void PreDefaults()
        {
            healPower = Projectile.GetGlobalProjectile<excelProjectile>().healStrength;
            healRate = Projectile.GetGlobalProjectile<excelProjectile>().healRate;
        }

        public override bool? CanCutTiles() => canDealDamage;
        public override bool? CanHitNPC(NPC target)
        {
            if (target.friendly)
                return false;

            return canDealDamage;
        }
        public override bool CanHitPvp(Player target) => canDealDamage;


        public void BuffCollision(Player target, Player healer)
        {
            if (!canHealOwner && (target == healer))
            {
                return;
            }
            if (((target.Center.X > (Projectile.Center.X - Projectile.width / 2)) && (target.Center.X < (Projectile.Center.X + Projectile.width / 2))
                && (target.Center.Y > (Projectile.Center.Y - Projectile.height / 2)) && (target.Center.Y < (Projectile.Center.Y + Projectile.height / 2))))
            {
                BuffEffects(target, healer);
                Projectile.netUpdate = true;
                if (buffConsumesPenetrate)
                {
                    if (heallist.Contains(target.whoAmI))
                    {
                        return;
                    }
                    Projectile.penetrate--;
                    if (Projectile.penetrate == 0)
                    {
                        Projectile.Kill();
                        return;
                    }
                    heallist.Add(target.whoAmI);
                }
            }
        }

        public void BuffDistance(Player target, Player healer, int distance)
        {
            if (!canHealOwner && (target == healer))
            {
                return;
            }
            if (Vector2.Distance(target.Center, Projectile.Center) < distance)
            {
                BuffEffects(target, healer);
                Projectile.netUpdate = true;
                if (buffConsumesPenetrate)
                {
                    if (heallist.Contains(target.whoAmI))
                    {
                        return;
                    }
                    Projectile.penetrate--;
                    if (Projectile.penetrate == 0)
                    {
                        Projectile.Kill();
                        return;
                    }
                    heallist.Add(target.whoAmI);
                }
            }
        }

        // BuffEffects will be done manually in heal projectiles since it could potentioally be complex
        public virtual void BuffEffects(Player target, Player healer)
        {

        }
        public virtual int GetBuffTime(Player healer, float timeInSeconds)
        {
            return (int)((timeInSeconds + healer.GetModPlayer<excelPlayer>().buffBonus) * 60);
        }

        /// - HEALING CODE - ///

        public void HealCollision(Player target, Player healer)
        {
            // find whichever is closest for most accurate 'collision'
            Vector2 pos = target.Center;
            float dist = Vector2.Distance(pos, Projectile.Center);
            if (Vector2.Distance(target.TopLeft, Projectile.Center) < dist) { pos = target.TopLeft; }
            if (Vector2.Distance(target.TopRight, Projectile.Center) < dist) { pos = target.TopRight; }
            if (Vector2.Distance(target.BottomLeft, Projectile.Center) < dist) { pos = target.BottomLeft; }
            if (Vector2.Distance(target.BottomRight, Projectile.Center) < dist) { pos = target.BottomRight; }
            // check if can heal and if yes, heal player
            if (((pos.X > (Projectile.Center.X - Projectile.width / 2)) && (pos.X < (Projectile.Center.X + Projectile.width / 2))
                && (pos.Y > (Projectile.Center.Y - Projectile.height / 2)) && (pos.Y < (Projectile.Center.Y + Projectile.height / 2))))
            {
                HealEffects(target, healer);
                if (healUsesBuffs)
                {
                    if (target == healer && !canHealOwner)
                        return;
                    BuffEffects(target, healer);
                }
            }
        }

        public void HealDistance(Player target, Player healer, int distance = 10, bool affectedByHeartreach = true, bool canHealFullHealth = false)
        {
            float mult = 1;
            if (target.HasBuff(BuffID.Heartreach) && affectedByHeartreach) { mult = 1.4f; }
            // find whichever is closest for most accurate 'collision'
            Vector2 pos = target.Center;
            float dist = Vector2.Distance(pos, Projectile.Center);
            if (Vector2.Distance(target.TopLeft, Projectile.Center) < dist) { pos = target.TopLeft; dist = Vector2.Distance(target.TopLeft, Projectile.Center);  }
            if (Vector2.Distance(target.TopRight, Projectile.Center) < dist) { pos = target.TopRight; dist = Vector2.Distance(target.TopRight, Projectile.Center); }
            if (Vector2.Distance(target.BottomLeft, Projectile.Center) < dist) { pos = target.BottomLeft; dist = Vector2.Distance(target.BottomLeft, Projectile.Center); }
            if (Vector2.Distance(target.BottomRight, Projectile.Center) < dist) { pos = target.BottomRight; dist = Vector2.Distance(target.BottomRight, Projectile.Center); }
            // check if within range, then apply heal
            if (Vector2.Distance(pos, Projectile.Center) < (distance))
            {
                HealEffects(target, healer);
                if (healUsesBuffs)
                {
                    if (!canHealOwner && target.whoAmI == healer.whoAmI)
                    {
                        return;
                    }
                    BuffEffects(target, healer);
                }
            }
        }

        public virtual void HealEffects(Player target, Player healer)
        {
            if (heallist.Contains(target.whoAmI)) {
                return;
            }
            if (!canHealOwner && target.whoAmI == healer.whoAmI)
            {
                return;
            }

            int healAmount = Projectile.GetGlobalProjectile<excelProjectile>().healStrength;
            float healRate = Projectile.GetGlobalProjectile<excelProjectile>().healRate;

            var lifeBeforeHeal = target.statLife;
            heallist.Add(target.whoAmI);

            // put post heal here so percentage heals still have this option
            PostHealEffects(target, healer);

            if (healRate != -1)
            {
                if (healAmount < 1)
                {
                    healAmount = 1;
                }
                int trueHeal = healAmount;

                trueHeal += (int)(healer.GetModPlayer<excelPlayer>().healBonus * healRate);

                if (healer.GetModPlayer<excelPlayer>().PrismiteSet && Main.rand.Next(100) < 5)
                {
                    trueHeal += healAmount / 2;
                    target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.PrismiteHeart>(), GetBuffTime(healer, trueHeal * 0.7f));
                }


                target.HealEffect(trueHeal, true);
                target.statLife += trueHeal;             
            }

            #region Accessory Bonuses
            
            // Antitoxin Bottle - If target is poisoned : remove poison and heal 7 health
            if (healer.GetModPlayer<excelPlayer>().antitoxinBottle && target.HasBuff(BuffID.Poisoned))
            {
                target.ClearBuff(BuffID.Poisoned);
                target.HealEffect(7, true);
                target.statLife += 7;
            }
            if (healer.GetModPlayer<excelPlayer>().nectarBottle)
            {
                target.AddBuff(BuffID.Honey, GetBuffTime(healer, 10));
            }

            #endregion

            #region Armor Bonuses 

            if (healer.GetModPlayer<excelPlayer>().HolyKnightSet)
            {
                healer.GetModPlayer<excelPlayer>().HolyKnightSetBonus += (target.statLife - lifeBeforeHeal);
            }
            if (healer.GetModPlayer<excelPlayer>().FloralSet)
            {
                // doesnt work if healer healed themself
                if (target != healer)
                {
                    target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.FloralBeauty>(), GetBuffTime(healer, 8));
                    healer.AddBuff(ModContent.BuffType<Buffs.ClericBonus.FloralBeauty>(), GetBuffTime(healer, 4));
                }
            }

            #endregion


            // Decreases anguished soul time when healing allies
            if (healer.HasBuff(ModContent.BuffType<Buffs.ClericCld.AnguishedSoul>()))
            {
                var soul = healer.FindBuffIndex(ModContent.BuffType<Buffs.ClericCld.AnguishedSoul>());
                healer.buffTime[soul] -= (target.statLife - lifeBeforeHeal) * 30;
            }

            var GetClassPlayer = healer.GetModPlayer<ClericClassPlayer>();
            GetClassPlayer.radianceStatCurrent += (target.statLife - lifeBeforeHeal);

            if (target.statLife > target.statLifeMax2)
            {
                target.statLife = target.statLifeMax2;
            }
            Projectile.netUpdate = true;
            NetMessage.SendData(66, -1, -1, null, target.whoAmI, target.statLife - lifeBeforeHeal, 0f, 0f, 0, 0, 0);

            // still want this to happen to heals with special properties
            healPenetrate--;
            if (healPenetrate == 0)
            {
                Projectile.Kill();
            }
        }

        // used to start some other effects
        public virtual void PostHealEffects(Player target, Player healer)
        {

        }
    }

    // priest armor set bonus
    // ability : prayer
    // while prayer is active, increases ally health healed by 15% and increases their endurance, but hurts you for 10 health
}
