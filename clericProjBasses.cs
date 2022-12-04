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
using System.IO;

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


        public virtual Projectile CreateHealProjectile(Player player, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int healAmount = 1, float healRate = 1, float ai0 = 0, float ai1 = 0)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile p = Projectile.NewProjectileDirect(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI);
                p.GetGlobalProjectile<excelProjectile>().healStrength = healAmount;
                p.GetGlobalProjectile<excelProjectile>().healRate = healRate;
                p.ai[0] = ai0;
                p.ai[1] = ai1;
                //p.netUpdate = true;

                return Main.projectile[p.whoAmI];
            }

            return Main.projectile[-1];
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
        public List<int> healTimer = new List<int>();

        public int healPenetrate = 1;
        public bool healUsesBuffs = false;
        public bool canHealOwner = false;
        public bool buffConsumesPenetrate = false;

        public int timeBetweenHeal = 30;
        public bool canDealDamage = false;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(healPenetrate);
            writer.Write(timeBetweenHeal);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            healPenetrate = reader.ReadInt32(); 
            timeBetweenHeal = reader.ReadInt32();
        }

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

        public override bool PreAI()
        {
            for (var i = 0; i < healTimer.Count; i++)
            {
                if (healTimer[i]-- <= 0)
                {
                    healTimer.RemoveAt(i);
                    heallist.RemoveAt(i);
                }

            }
            return base.PreAI();
        }

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
                    healTimer.Add(timeBetweenHeal); 
                }
            }
        }

        public void BuffDistance(Player target, Player healer, int distance, int locationStyle = 0)
        {
            Vector2 pos = Projectile.Center;
            if (locationStyle == 1)
                pos = healer.Center;

            if (!canHealOwner && (target == healer))
            {
                return;
            }
            if (Vector2.Distance(target.Center, pos) < distance)
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
                    healTimer.Add(timeBetweenHeal);
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
            // check if can heal and if yes, heal player
            if (Collision.CheckAABBvAABBCollision(Projectile.position, new Vector2(Projectile.width, Projectile.height), target.position, new Vector2(target.height, target.width)))
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
            if (Main.myPlayer != Projectile.owner)
                return;

            if (heallist.Contains(target.whoAmI)) {
                return;
            }
            if (!canHealOwner && target.whoAmI == healer.whoAmI)
            {
                return;
            }

            heallist.Add(target.whoAmI);
            healTimer.Add(timeBetweenHeal);

            var priorHealth = target.statLife;
            int healAmount = Projectile.GetGlobalProjectile<excelProjectile>().healStrength;
            float healRate = Projectile.GetGlobalProjectile<excelProjectile>().healRate;

            healAmount += (int)(healer.GetModPlayer<excelPlayer>().healBonus * healRate);

            #region Armor Effects
            // Holy Knight : Charges up self-heal effect
            if (healer.GetModPlayer<excelPlayer>().HolyKnightSet)
                healer.GetModPlayer<excelPlayer>().HolyKnightSetBonus += (target.statLife - priorHealth);

            // Floral : Applies health regeneration to both players
            if (healer.GetModPlayer<excelPlayer>().FloralSet && target != healer)
            {
                target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.FloralBeauty>(), GetBuffTime(healer, 8));
                healer.AddBuff(ModContent.BuffType<Buffs.ClericBonus.FloralBeauty>(), GetBuffTime(healer, 4));
            }

            // Crystalline : Critical heal chance & Increases max health
            if (healer.GetModPlayer<excelPlayer>().PrismiteSet && Main.rand.NextBool(20))
            {
                healAmount += healAmount / 2;
                target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.PrismiteHeart>(), GetBuffTime(healer, healAmount * 0.7f));
            }
            #endregion

            #region Accessory Bonuses

            // Medic Bag : Applies health over time buff
            if (healer.GetModPlayer<excelPlayer>().medicBag && target != healer)
                target.AddBuff(ModContent.BuffType<Buffs.ClericBonus.SoothingSoul>(), GetBuffTime(healer, 6));

            // Antitoxin Bottle : If target is poisoned, remove it and heal 7 health
            if (healer.GetModPlayer<excelPlayer>().antitoxinBottle && target.HasBuff(BuffID.Poisoned))
            {
                target.ClearBuff(BuffID.Poisoned);
                healAmount += 7;
            }

            // Nectar Bottle : Apply Honey buff
            if (healer.GetModPlayer<excelPlayer>().nectarBottle)
                target.AddBuff(BuffID.Honey, GetBuffTime(healer, 10));
            
            // Lost Kin : Summon damaging projectiles proportionate to healing done
            if (healer.GetModPlayer<excelPlayer>().lostKin && target != healer)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (var i = 0; i < Math.Floor((double)(healAmount / 6 + 1)); i++)
                    {
                        Projectile.NewProjectile(healer.GetSource_FromThis(), target.Center, new Vector2(1.4f).RotateRandom(MathHelper.ToRadians(360)), ModContent.ProjectileType<Items.Accessories.Cleric.Healing.DarkEnergy>(), 20, 0, healer.whoAmI);
                    }
                }
            }

            // Soothing Cream : Removes time from On Fire!
            if (healer.GetModPlayer<excelPlayer>().soothingCream)
            {
                for (var i = 0; i < target.CountBuffs(); i++)
                {
                    if (target.buffType[i] == BuffID.OnFire)
                    {
                        target.buffTime[i] -= 30;
                    }
                }
            }

            #endregion

            // Prevents from not healing/reverse healing
            if (healAmount < 1)
                healAmount = 1;

            // this is done to allow unique healing calculations to be done without this been taken into consideration
            if (healRate != -1)
            {
                target.HealEffect(healAmount, true);
                target.statLife += healAmount;

                if (target.statLife > target.statLifeMax2)
                {
                    target.statLife = target.statLifeMax2;
                }
                NetMessage.SendData(66, -1, -1, null, target.whoAmI, healAmount, 0f, 0f, 0, 0, 0);
            }

            PostHealEffects(target, healer);
        
            // Decreases anguished soul time when healing allies
            if (healer.HasBuff(ModContent.BuffType<Buffs.ClericCld.AnguishedSoul>()))
            {
                var soul = healer.FindBuffIndex(ModContent.BuffType<Buffs.ClericCld.AnguishedSoul>());
                healer.buffTime[soul] -= (target.statLife - priorHealth) * 30;
            }

            var GetClassPlayer = healer.GetModPlayer<ClericClassPlayer>();
            if (target != healer)
                GetClassPlayer.radianceStatCurrent += (target.statLife - priorHealth);

            // still want this to happen to heals with special properties
            healPenetrate--;
            Projectile.netUpdate = true;

            Projectile.netUpdate = true;

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
