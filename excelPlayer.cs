using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace excels
{
    public class excelPlayer : ModPlayer
    {
        // buffs
        public int BerserkerCount = 0;
        public bool FlaskFrostfire = false;
        // debuffs
        public bool DebuffMycosis = false;
        // cleric bonuses
        public bool GlacialGuard = false;
        public bool HereticBreakerBuff = false;
        public bool SonicSyringeBuff = false;

        // accessories
        public bool MimicNecklace = false;
        public int SummonBanner = 0;
        public bool ShieldReflect = false;
        public bool BeetleShield = false;
        public bool FriendshipRegen = false;
        public bool SnowflakeAura = false;
        int SnowflakeCooldown = 0;
        public bool FireBadge = false;
        public bool ArtemisSigil = false;
        public bool VirtualShades = false;
        public bool BlizzardAura = false;
        // expert
        public bool NiflheimAcc = false;
        public bool ChasmAcc = false;
        public bool StellarAcc = false;

        // armor sets
        public bool GlacialSet = false;
        public bool AvianSet = false; // used in Banner : GlobalProjectile
        public bool FossilSet = false;
        int FossilSetReset = 0;
        public bool StellarSet = false;
        public float StellarDamageBonus = 1;
        public int StellarCritBonus = 0;
        public float StellarUseSpeed = 1;
        public bool WyvernSet = false;
        public bool HeartbeatSet = false;
        public bool HolyKnightSet = false;
        public int HolyKnightSetBonus = 0;
        public bool PriestSet = false;
        public bool FloralSet = false;
        public bool PrismiteSet = false;
        public bool CrusaderSet = false;

        // spirits
        public float SpiritDamageMult = 1;
        public float SpiritAttackSpeed = 1;

        // cleric
        public int healBonus = 0;
        public int buffBonus = 0;

        public int bloodCostMinus = 0;
        public float bloodCostMult = 1;
        public bool AnguishSoul = false;
        // cleric accessories
        public bool antitoxinBottle = false;
        public bool nectarBottle = false;
        public bool glassCross = false;
        public bool skullPendant = false;
        public int skullPendantBlood = 0;
        public bool skullPendant2 = false;
        public bool skullPendantFrost = false;
        public bool hyperionHeart = false;

        public override void ResetEffects()
        {
            // buffs
            BerserkerCount = 0;
            FlaskFrostfire = false;
            // debuffs
            DebuffMycosis = false;
            // cleric bonuses
            GlacialGuard = false;
            HereticBreakerBuff = false;
            SonicSyringeBuff = false;

            // accessories
            MimicNecklace = false;
            SummonBanner = 0;
            ShieldReflect = false;
            BeetleShield = false;
            FriendshipRegen = false;
            SnowflakeAura = false;
            FireBadge = false;
            ArtemisSigil = false;
            BlizzardAura = false;
            // expert
            NiflheimAcc = false;
            ChasmAcc = false;
            StellarAcc = false;

            // armor sets
            GlacialSet = false;
            AvianSet = false;
            FossilSet = false;
            StellarSet = false;
            StellarDamageBonus = 1;
            StellarCritBonus = 0;
            StellarUseSpeed = 1;
            WyvernSet = false;
            HeartbeatSet = false;
            HolyKnightSet = false;
            PriestSet = false;
            FloralSet = false;
            PrismiteSet = false;
            CrusaderSet = false;

            // spirits
            SpiritDamageMult = 1;
            SpiritAttackSpeed = 1;

            // cleric
            healBonus = 0;
            buffBonus = 0;

            bloodCostMinus = 0;
            bloodCostMult = 1;
            AnguishSoul = false;
            // cleric accessories
            antitoxinBottle = false;
            nectarBottle = false;
            glassCross = false;
            skullPendant = false;
            skullPendant2 = false;
            skullPendantFrost = false;
            hyperionHeart = false;
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (!mediumCoreDeath)
                return new[] { new Item(ModContent.ItemType<Items.Misc.ClericsHandbook>()) };
            return base.AddStartingItems(mediumCoreDeath);
        }

        public override void PostUpdateRunSpeeds()
        {
            if (GlacialGuard)
            {
                Player.moveSpeed *= 0.66f;
                Player.runAcceleration *= 0.66f;
                Player.maxRunSpeed *= 0.66f;
                Player.accRunSpeed *= 0.66f;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (CrusaderSet && !Player.HasBuff(ModContent.BuffType<Buffs.ClericCld.RevivalRecover>()))
            {
                Player.AddBuff(ModContent.BuffType<Buffs.ClericCld.RevivalRecover>(), 10800);
                Player.statLife += (Player.statLifeMax2 / 2) + (healBonus * (Player.statLifeMax2 / 20));
                Player.HealEffect((Player.statLifeMax2 / 2) + (healBonus * (Player.statLifeMax2 / 20)));
                Player.immune = true;
                Player.immuneTime = 90;
                if (Player.longInvince)
                    Player.immuneTime += 45;
                return false;
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void UpdateBadLifeRegen()
        {
            if (DebuffMycosis)
            {
                int dot = 8;
                if (Main.hardMode) {
                    dot += 6;
                    if (NPC.downedPlantBoss) { dot += 10; }
                }
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegen -= dot;
            }
            if (ChasmAcc)
            {
                // since its a negative, need to cancel it out
                Player.lifeRegen += (int)(Player.lifeRegen * -0.2f);
            }
            if (glassCross && Player.lifeRegen < 0)
            {
                Player.GetModPlayer<excelPlayer>().healBonus += 2;
            }
        }

        public override void UpdateLifeRegen()
        {
            if (FriendshipRegen)
            {
                for (var i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.minionSlots > 0)
                    {
                        Player.lifeRegen += 1;
                    }
                }
            }
            if (AnguishSoul && Player.lifeRegen > 0)
            {
                Player.lifeRegen /= 2;
            }
        }

        public override void PreUpdate()
        {
            // using this for variable timers
            if (SnowflakeAura || skullPendantFrost || BlizzardAura)
            {
                SnowflakeCooldown--;
                if (skullPendantFrost)
                    SnowflakeCooldown--;
            }
            if (FossilSet)
            {
                FossilSetReset--;
            }
            if (HolyKnightSet)
            {
                if (HolyKnightSetBonus >= 35)
                {
                    Player.statLife += 3;
                    if (Player.statLife > Player.statLifeMax2)
                    {
                        Player.statLife = Player.statLifeMax2;
                    }
                    Player.HealEffect(3);
                    HolyKnightSetBonus -= 35;
                }
            }
        }

        public override bool? CanAutoReuseItem(Item item)
        {
            if (item.DamageType == DamageClass.Ranged && ArtemisSigil)
                return true;

            return base.CanAutoReuseItem(item);
        }

        public override void PostUpdate()
        {
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].TileType == ModContent.TileType<Tiles.Misc.OilKit>())
            {
                if (Player.position.X / 16f - (float)Player.tileRangeX - (float)Player.inventory[Player.selectedItem].tileBoost - (float)Player.blockRange <= (float)Player.tileTargetX && (Player.position.X + (float)Player.width) / 16f + (float)Player.tileRangeX + (float)Player.inventory[Player.selectedItem].tileBoost - 1f + (float)Player.blockRange >= (float)Player.tileTargetX && Player.position.Y / 16f - (float)Player.tileRangeY - (float)Player.inventory[Player.selectedItem].tileBoost - (float)Player.blockRange <= (float)Player.tileTargetY && (Player.position.Y + (float)Player.height) / 16f + (float)Player.tileRangeY + (float)Player.inventory[Player.selectedItem].tileBoost - 2f + (float)Player.blockRange >= (float)Player.tileTargetY && Player.itemTime == 0 && Player.itemAnimation > 0 && Player.controlUseItem)
                {
                    bool Activate = false;
                    if (Player.inventory[Player.selectedItem].type == ModContent.ItemType<Items.Materials.AncientFossil>())
                        OilKitLoot(4);
                    else if (Player.inventory[Player.selectedItem].type == ItemID.DesertFossil)
                        OilKitLoot(1);
                    else if (Player.inventory[Player.selectedItem].type == ItemID.FossilOre)
                        OilKitLoot(2);
                    
                    if (Activate)
                    {
                        Player.inventory[Player.selectedItem].stack--;
                        Player.itemTime = Player.inventory[Player.selectedItem].useTime; // Player.TotalUseTime((float)Player.inventory[Player.selectedItem].useTime, this, Player.inventory[Player.selectedItem]);
                        Player.itemAnimation = Player.inventory[Player.selectedItem].useAnimation;
                    }
                }
            }
        }

        private void OilKitLoot(int level = 0)
        {
            int oilAmount = 0;
            int oilType = 0;
            int coinAmount = 0;

            switch (level)
            {
                case 1:
                    oilAmount = Main.rand.Next(1, 3);
                    if (Main.rand.Next(5) <= 3)
                        oilAmount++;
                    if (Main.rand.Next(7) <= 5)
                        oilType = ModContent.ItemType<Items.Ammo.Other.LampOil>();
                    if (Main.rand.NextBool(20))
                        oilType = ModContent.ItemType<Items.Ammo.Other.LighterFluid>();

                    coinAmount = Main.rand.Next(5, 13);
                    if (Main.rand.NextBool(4))
                        coinAmount = (int)(coinAmount * .5f);
                    break;
                case 2:
                    oilAmount = Main.rand.Next(4, 8);
                    if (Main.rand.Next(8) <= 5)
                        oilAmount += 2;
                    if (Main.rand.Next(13) <= 11)
                        oilType = ModContent.ItemType<Items.Ammo.Other.LampOil>();
                    if (Main.rand.NextBool(5))
                        oilType = ModContent.ItemType<Items.Ammo.Other.LighterFluid>();

                    coinAmount = Main.rand.Next(14, 23);
                    if (Main.rand.NextBool(3))
                        coinAmount = (int)(coinAmount * .9f);
                    break;
                case 4:
                    oilAmount = Main.rand.Next(9, 14);
                    if (Main.rand.Next(11) <= 2)
                        oilAmount = (int)(oilAmount * 0.8f);
                    if (Main.rand.Next(13) <= 11)
                        oilType = ModContent.ItemType<Items.Ammo.Other.LighterFluid>();
                    if (Main.rand.NextBool(9))
                        oilType = ModContent.ItemType<Items.Ammo.Other.LampOil>();

                    coinAmount = Main.rand.Next(28, 39);
                    if (Main.rand.NextBool(6))
                        coinAmount = (int)(coinAmount * .9f);
                    break;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (oilType != 0)
                Item.NewItem(Player.GetSource_TileInteraction(Player.tileTargetX, Player.tileTargetY), new Vector2(Player.tileTargetX * 16, Player.tileTargetY * 16), oilType, oilAmount);

            if (coinAmount > 0)
                Item.NewItem(Player.GetSource_TileInteraction(Player.tileTargetX, Player.tileTargetY), new Vector2(Player.tileTargetX * 16, Player.tileTargetY * 16), ItemID.CopperCoin, coinAmount);
        }

        
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {   
            if (item.DamageType == DamageClass.Ranged && FossilSetReset < 0)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(8)), ModContent.ProjectileType<Items.Weapons.Fossil.FossilChunkR>(), 30, 1, Player.whoAmI);
                FossilSetReset = 23;
            }

            if (VirtualShades && item.DamageType == DamageClass.Ranged && item.noUseGraphic && item.consumable && Main.rand.NextBool(5))
            {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.9f, 1.1f), type, damage, knockback, Player.whoAmI);
            }

            return true;
        }

        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (NiflheimAcc || (FlaskFrostfire && item.DamageType == DamageClass.Melee))
            {   
                Dust d = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 92);
                d.scale = item.scale;
                d.noGravity = true;
                d.noLight = true;
            }
        }

        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            if (MimicNecklace)
            {
                SoundEngine.PlaySound(SoundID.NPCHit26, Player.Center);
                FakeHitPVP(target, (int)(damage * 1.25f), target.getRect());
                target.AddBuff(BuffID.Bleeding, damage * 45);
            }

            if (NiflheimAcc)
            {
                target.AddBuff(BuffID.Frostburn, damage * 40);
            }

            if (FlaskFrostfire && item.DamageType == DamageClass.Melee)
            {
                if (Main.rand.NextBool(3))
                {
                    target.AddBuff(BuffID.Frostburn, 300);
                }
                else
                {
                    target.AddBuff(BuffID.Frostburn, 180);
                }
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (BlizzardAura)
                BlizzardNecklace();
            else if (SnowflakeAura)
                SnowflakeAmulet();

            // if returning a hit to an npc, a some basic checks need to be done first
            if (npc.lifeMax > 5 && damage >= 1) // prevents from working with spell enemies and if attack was dodged
            {
                if (MimicNecklace)
                {
                    SoundEngine.PlaySound(SoundID.NPCHit26, Player.Center);
                    FakeHitNPC(npc, damage * 2, npc.getRect());
                    npc.AddBuff(BuffID.Bleeding, damage * 45);
                }
                if (ShieldReflect)
                {
                    if (BeetleShield)
                        FakeHitNPC(npc, (int)(npc.damage * 1.5f), npc.getRect());
                    else
                        FakeHitNPC(npc, (int)(npc.damage * 1.33f), npc.getRect());
                }
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (BlizzardAura)
                BlizzardNecklace();
            else if (SnowflakeAura)
                SnowflakeAmulet();
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (SonicSyringeBuff && Main.rand.NextBool(10))
            {
               // Player.ClearBuff(ModContent.BuffType<>)
                for (var i = 0; i < 10; i++)
                {
                    for (var s = 0; s < 8; s++)
                    {
                        Vector2 pos = Player.Center + new Vector2(20+(i*0.5f)).RotatedBy(MathHelper.ToRadians(45 * s));
                        Dust d = Dust.NewDustPerfect(pos, 156);
                        d.velocity = (pos - Player.Center).SafeNormalize(Vector2.Zero) * (2+(i*0.4f));
                        d.noGravity = true;
                        d.scale = 1.2f - (i * 0.25f);
                    }
                }
                return false;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }



        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (NiflheimAcc)
            {
                target.AddBuff(BuffID.Frostburn, damage * 40);
            }
            if (FlaskFrostfire && item.DamageType == DamageClass.Melee)
            {
                if (Main.rand.NextBool(3))
                {
                    target.AddBuff(BuffID.Frostburn, 300);
                }
                else
                {
                    target.AddBuff(BuffID.Frostburn, 180);
                }
            }
            if (HereticBreakerBuff && crit && target.type != NPCID.TargetDummy)
            {
                Player.Heal((int)(damage * 0.075)-Main.rand.Next(3));
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            Item item = Player.HeldItem;
            if (NiflheimAcc) // && (Main.rand.Next(70) < (item.useAnimation + (item.mana * 2))))
            {
                int dmg = damage * (Math.Abs(60 - item.useTime) / 5);
                target.AddBuff(BuffID.Frostburn, dmg * 60);
            }

            if (FlaskFrostfire && (proj.DamageType == DamageClass.Melee || ProjectileID.Sets.IsAWhip[proj.type]))
            {
                if (Main.rand.NextBool(3))
                {
                    target.AddBuff(BuffID.Frostburn, 300);
                }
                else
                {
                    target.AddBuff(BuffID.Frostburn, 180);
                }
            }

            if (proj.DamageType == DamageClass.Summon && proj.type != ModContent.ProjectileType<Items.Weapons.Whips.FloralPower>() && !ProjectileID.Sets.IsAWhip[proj.type] && target.HasBuff(ModContent.BuffType<Buffs.Whips.PerrenialWhipDebuff>()))
            {
                Projectile p = Projectile.NewProjectileDirect(proj.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<Items.Weapons.Whips.FloralPower>(), (int)(proj.damage * 1.5f), 0, Main.player[proj.owner].whoAmI);
            }

            if (AvianSet && (proj.DamageType == DamageClass.Summon || proj.minionSlots > 0) && proj.type != ModContent.ProjectileType<Items.Armor.Avian.AvianSkyFeather>())
            {
                for (var i = 0; i < 3; i++) {
                    if (Main.rand.NextBool((int)((5 - proj.minionSlots) * (i * 1.7f + 1))))
                    {
                        Vector2 pos = target.position + new Vector2(Main.rand.Next(-120, 121), -400);
                        Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 11;

                        Projectile.NewProjectile(proj.GetSource_FromThis(), pos, vel, ModContent.ProjectileType<Items.Armor.Avian.AvianSkyFeather>(),
                            14, 2, Main.player[proj.owner].whoAmI);
                    }
                }
            }

            if (GlacialSet && crit && proj.DamageType == DamageClass.Ranged && proj.type != ModContent.ProjectileType<Items.Armor.Glacial.GlacialBlast>())
            {
                Projectile.NewProjectile(proj.GetSource_FromThis(), proj.Center, Vector2.Zero, ModContent.ProjectileType<Items.Armor.Glacial.GlacialBlast>(), 30, 0, Main.player[proj.owner].whoAmI);
            }

            if (HereticBreakerBuff && crit && target.type != NPCID.TargetDummy)
            {
                Player.Heal((int)(damage * 0.075) - Main.rand.Next(3));
            }
        }

        #region Hit Effects
        public void SnowflakeAmulet()
        {
            if (SnowflakeCooldown < 0)
            {
                // creates six ice shards
                for (var i = 0; i < 6; i++) {
                    Projectile p = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<Items.Armor.Glacial.GlacialShard>(), 20, 1, Player.whoAmI);
                    p.ai[0] = (3.6f / 3.5f) * (i + 1);
                    p.ai[1] = 100;
                }

                SnowflakeCooldown = 300;
            }
        }

        public void BlizzardNecklace()
        {
            if (SnowflakeCooldown < 0)
            {
                // creates a blizzard
                for (var i = 0; i < 12; i++)
                {
                    Projectile p = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<Items.Armor.Glacial.GlacialShard>(), 25, 1, Player.whoAmI);
                    p.ai[0] = (3.6f / 3.5f) * (i + 1) + Main.rand.NextFloat(-0.3f, 0.3f);
                    p.ai[1] = 60+Main.rand.Next(60);
                }

                SnowflakeCooldown = 300;
            }
        }

        public void FakeHitNPC(NPC target, int damage, Rectangle position)
        {
            if (target.immortal)
            {
                return;
            }
            target.life -= damage;
            target.HitEffect();
            SoundEngine.PlaySound(target.HitSound.Value, target.Center);
            CombatText.NewText(position, Color.Orange, damage);
            if (target.life <= 0)
            {
                target.checkDead();
            }
        }

        public void FakeHitPVP(Player target, int damage, Rectangle position)
        {
            target.statLife -= damage;
            CombatText.NewText(position, Color.Orange, damage);
            if (target.statLife <= 0)
            {
                target.KillMe(PlayerDeathReason.ByCustomReason($"{target.name} was biten"), 40, 0, true);
            }
        }
        #endregion
    }

    public class excelItem : GlobalItem
    {
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (player.GetModPlayer<excelPlayer>().FireBadge && item.useAmmo == AmmoID.Gel)
            {
                damage *= 1.15f;
            }
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.useAmmo == AmmoID.Gel)
            {
                switch (type)
                {
                    // Lighter Fluid
                    case ProjectileID.MolotovFire:
                        type = item.shoot;
                        velocity = velocity * 0.9f;
                        break;

                    // Shadowfire Oil
                    case ProjectileID.MolotovFire2:
                        switch (item.shoot)
                        {
                            case ProjectileID.Flames:
                                type = ModContent.ProjectileType<Items.Weapons.Flamethrower.ShadowfireFlames>();
                                break;
                        }
                        velocity = velocity * 0.8f;
                        break;

                    // Frostfire Oil
                    case ProjectileID.MolotovFire3:
                        switch (item.shoot)
                        {
                            case ProjectileID.Flames:
                                type = ModContent.ProjectileType<Items.Weapons.Flamethrower.FrostfireFlames>();
                                break;
                        }
                        velocity = velocity * 0.9f;
                        break;
                }
            }
        }

        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            // This method shows adding items to Fishrons boss bag. 
            // Typically you'll also want to also add an item to the non-expert boss drops, that code can be found in ExampleGlobalNPC.NPCLoot. Use this and that to add drops to bosses.

            if (context == "bossBag")
            {
                switch (arg)
                {
                    case ItemID.QueenBeeBossBag:
                        if (Main.rand.NextBool(3))
                            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Accessories.Cleric.Healing.NectarBottle>());
                        break;

                    case ItemID.WallOfFleshBossBag:
                        if (Main.rand.NextBool(3))
                            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Accessories.Cleric.Damage.ClericEmblem>());
                        if (Main.rand.NextBool(3))
                            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.WeaponHeal.Holyiest.PhoenixScepter>());
                        break;

                    case ItemID.GolemBossBag:
                        if (Main.rand.NextBool(7))
                            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Weapons.Flamethrower.SolarEngine>());
                        break;
                }
            }

            if (context == "crate")
            {
                switch (arg)
                {
                    case ItemID.WoodenCrate:
                    case ItemID.WoodenCrateHard:
                        if (Main.rand.NextBool(3))
                        {
                            switch (Main.rand.Next(2))
                            {
                                case 0:
                                    player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Accessories.Cleric.Healing.ApothSatchel>());
                                    break;

                                case 1:
                                    player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Accessories.Banner.RegenBanner>());
                                    break;
                            }
                        }
                        if (Main.rand.NextBool(4))
                        {
                            switch (Main.rand.Next(2))
                            {
                                case 0:
                                    player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Misc.Herbs.Gladiolus>(), Main.rand.Next(1, 3));
                                    break;

                                case 1:
                                    player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Misc.Herbs.GladiolusSeeds>(), Main.rand.Next(2, 5));
                                    break;
                            }
                        }
                        break;

                    case ItemID.GoldenCrate:
                    case ItemID.GoldenCrateHard:
                        if (Main.rand.NextBool(3))
                            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Potions.Potions.SweetPotion>(), Main.rand.Next(1, 4));
                        break;

                    case ItemID.FrozenCrateHard:
                        if (Main.rand.NextBool(3))
                        {
                            switch (Main.rand.Next(2))
                            {
                                case 0:
                                    player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Materials.GlacialBar>(), Main.rand.Next(1, 4));
                                    break;

                                case 1:
                                    player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Materials.GlacialOre>(), Main.rand.Next(2, 7));
                                    break;
                            }
                        }
                        break;

                    case ItemID.FloatingIslandFishingCrate:
                    case ItemID.FloatingIslandFishingCrateHard:
                        if (Main.rand.NextBool(3))
                        {
                            switch (Main.rand.Next(2))
                            {
                                case 0:
                                    player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Materials.SkylineBar>(), Main.rand.Next(1, 4));
                                    break;

                                case 1:
                                    player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Materials.SkylineOre>(), Main.rand.Next(2, 7));
                                    break;
                            }
                        }
                        break;

                    case ItemID.DungeonFishingCrate:
                    case ItemID.DungeonFishingCrateHard:
                        if (Main.rand.NextBool(4))
                            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Materials.ShatteredHeartbeat>(), Main.rand.Next(1, 3));
                        break;

                    case ItemID.ObsidianLockbox:
                        if (Main.rand.NextBool(7))
                            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Weapons.Flamethrower.Roaster>());
                        break;
                }
            }
        }
    }
}
