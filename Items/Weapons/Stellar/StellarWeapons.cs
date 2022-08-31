using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace excels.Items.Weapons.Stellar
{
    public abstract class StellarWeapon : ModItem
    {
        public override bool IsCloneable => true;

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.GetModPlayer<excelPlayer>().StellarDamageBonus;
            if (Item.type == ModContent.ItemType<StellarCommandRod>())
            {
                damage *= player.GetModPlayer<excelPlayer>().SpiritDamageMult;
            }
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += player.GetModPlayer<excelPlayer>().StellarCritBonus;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return 1 * player.GetModPlayer<excelPlayer>().StellarUseSpeed;
        }

        public string NormTip = ""; // "Strikes foes with high frequency energy";
        public string PowerTip = ""; // "\nSet bonus: Even higher frequency energy increases damage against defense";

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.Mod == "Terraria")
                {
                    if (line2.Name == "Tooltip0")
                    {
                        if (Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().StellarSet)
                        {
                            line2.Text = NormTip + PowerTip;
                        }
                        else
                        {
                            line2.Text = NormTip;
                        }
                    }
                }
            }
        }
    }

    public class StellarEnergyBlade : StellarWeapon
    {
        public override void SetStaticDefaults()
        {
            
            Tooltip.SetDefault(NormTip);
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 36;
            Item.useTime = Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.knockBack = 2.7f;
            Item.scale = 1.12f;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.width = Item.height = 34;
            Item.sellPrice(0, 0, 75);

            NormTip = ""; // "Strikes foes with high frequency energy";
            PowerTip = "Set bonus: Penetrates some enemy defense with high voltage";
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (player.GetModPlayer<excelPlayer>().StellarSet)
            {
                int dmgUp = 40 - target.defense;
                // prevents too much increased damage against things like slimes
                if (dmgUp > target.defense)
                {
                    dmgUp = target.defense;
                }
                if (dmgUp > 18)
                {
                    dmgUp = 18;
                }
                if (dmgUp < 0)
                {
                    dmgUp = 0;
                }
                damage += dmgUp;

                for (var i = 0; i < dmgUp; i++)
                {
                    Dust d = Dust.NewDustDirect(target.position, target.width, target.height, 226);
                    d.scale = Main.rand.NextFloat(0.8f, 0.95f);
                    d.velocity = (target.Center - player.Center).SafeNormalize(Vector2.Zero).RotatedByRandom(MathHelper.ToRadians(7)) * 7;

                }
            }
        }
    }
    
    public class StellarAlienSidearm : StellarWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellar Sidearm");
            Tooltip.SetDefault(NormTip);
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 21;
            Item.mana = 5;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.knockBack = 2.1f;
            //Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.EyeLaser;
            Item.shootSpeed = 7;
            Item.height = 24;
            Item.width = 36;
            Item.sellPrice(0, 0, 80);

            NormTip = "";
            PowerTip = "Set bonus: Fires two lasers with lower damage that can pierce even  more foes";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // dont like it but have to do this so it works out
            if (player.GetModPlayer<excelPlayer>().StellarSet)
            {
                damage = (int)(damage * 0.75f);
            }
               
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            p.friendly = true;
            p.hostile = false;
            p.DamageType = DamageClass.Magic;
            p.scale = 0.75f;
            p.penetrate = 2;
            if (player.GetModPlayer<excelPlayer>().StellarSet)
            {
                // add pierce to first laser
                p.penetrate = 5;
                p.usesLocalNPCImmunity = true;
                p.localNPCHitCooldown = 10;

                // second laser
                Projectile p2 = Projectile.NewProjectileDirect(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(11)), type, damage, knockback, player.whoAmI);
                p2.friendly = true;
                p2.hostile = false;
                p2.DamageType = DamageClass.Magic;
                p2.scale = 0.75f;
                p2.penetrate = 5;
                p2.usesLocalNPCImmunity = true;
                p2.localNPCHitCooldown = 10;
            }

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }

    public class StellarBow : StellarWeapon
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(NormTip);
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 23;
            Item.useTime = Item.useAnimation = 27;
            Item.autoReuse = true;
            Item.knockBack = 3.1f;
            Item.noMelee = true;
            Item.height = 38;
            Item.width = 24;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = 10;
            Item.shootSpeed = 14.7f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5;
            Item.sellPrice(0, 0, 82);

            NormTip = "";
            PowerTip = "Set bonus: Every third shot transforms arrows into a shooting star";
        }

        public int Turn = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<excelPlayer>().StellarSet)
            {
                Turn++;
                if (Turn == 3)
                {
                    Projectile.NewProjectile(source, position, velocity, ProjectileID.StarCannonStar, (int)(damage * 2f), knockback * 3, player.whoAmI);
                    Turn = 0;
                    return false;
                }
            }

            return true;
        }
    }

    
    internal class StellarCommandRod : StellarWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starship Command Rod");
            Tooltip.SetDefault(NormTip);
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 25;
            Item.mana = 10;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.knockBack = 3.5f;
            //Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StellarCommandShip>();
            Item.shootSpeed = 7;
            Item.height = Item.width = 30;
            Item.sellPrice(0, 0, 90);

            NormTip = "Summons a stellar ship for you to command";
            PowerTip = "\nSet bonus: The stellar ship automatically launches missiles at the strongest enemy onscreen";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void UpdateInventory(Player player)
        {
            if (player.ownedProjectileCounts[Item.shoot] >= 1)
            {
                Item.mana = 0;
                Item.channel = true;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            else
            {
                Item.mana = 10;
                Item.channel = false;
                Item.useStyle = ItemUseStyleID.Swing;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] >= 1)
            {
                return false;
            }
            return true;
        }
    }

    public class StellarCommandShip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //Item.staff[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 10;
            Projectile.width = 32;
            Projectile.height = 24;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
        }

        float rot = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity *= 0;
            if (player.HeldItem.type != ModContent.ItemType<StellarCommandRod>() || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            rot += 0.07f;
            Projectile.Center = player.Center;
            Projectile.position.X += MathF.Cos(rot) * 10 + Projectile.width / 4;
            Projectile.position.Y += -46 + MathF.Sin(rot) * 5;

            Projectile.ai[0] += (1 * player.GetModPlayer<excelPlayer>().SpiritAttackSpeed) * player.GetModPlayer<excelPlayer>().StellarUseSpeed;
            if (Projectile.ai[0] > 20)
            {
                if (Main.myPlayer == Main.player[Projectile.owner].whoAmI && Main.mouseLeft && !Main.LocalPlayer.mouseInterface && !player.mapFullScreen)
                {
                    // spawns on random x, confirmed y
                    int xx = Main.rand.Next((int)player.Center.X - Main.screenWidth / 2, (int)player.Center.X + Main.screenWidth / 2);
                    int yy = (int)player.Center.Y - Main.screenHeight / 2;
                    if (Main.rand.NextBool())
                    {
                        yy = (int)player.Center.Y + Main.screenHeight / 2;
                    }
                    // spawns on randomo y, confirmed x
                    if (Main.rand.NextBool())
                    {
                        yy = Main.rand.Next((int)player.Center.Y - Main.screenHeight / 2, (int)player.Center.Y + Main.screenHeight / 2);
                        xx = (int)player.Center.X - Main.screenWidth / 2;
                        if (Main.rand.NextBool())
                        {
                            xx = (int)player.Center.X + Main.screenWidth / 2;
                        }
                    }

                    Vector2 vel = (Main.MouseWorld - new Vector2(xx, yy)).SafeNormalize(Vector2.Zero) * 11;
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), new Vector2(xx, yy), vel, ProjectileID.EyeLaser, Projectile.damage, Projectile.knockBack, player.whoAmI);
                    p.friendly = true;
                    p.hostile = false;
                    p.DamageType = DamageClass.Summon;
                    p.tileCollide = false;
                    p.ignoreWater = true;
                    p.netUpdate = true;
                    ProjectileID.Sets.MinionShot[p.type] = true;
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;
                }
            }

            if (player.GetModPlayer<excelPlayer>().StellarSet)
            {
                Projectile.ai[1] += 1 * player.GetModPlayer<excelPlayer>().StellarUseSpeed;
                if (Projectile.ai[1] > 130)
                {
                    Vector2 pos = Vector2.Zero;
                    bool target = false;
                    int enemDanger = -1;

                    for (var i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.lifeMax > 5 && Vector2.Distance(Projectile.Center, npc.Center) < 2000 && npc.active && !npc.friendly && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                        {
                            int curDanger = (npc.lifeMax * (npc.defense / 2)) * npc.damage;
                            if (curDanger > enemDanger)
                            {
                                pos = npc.Center;
                                enemDanger = curDanger;
                                target = true;
                            }
                        }
                    }

                    if (target)
                    {
                        Vector2 vel = (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * 9.7f;
                        Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<StellarRocketS>(), (int)(Projectile.damage * 1.8f), Projectile.knockBack * 3, player.whoAmI);
                        p.netUpdate = true;
                        Projectile.netUpdate = true;
                        Projectile.ai[1] = 0;
                    }
                }
            }
        }
    }

    public class StellarRocketS : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellar Rocket");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 150;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
            d.velocity = -Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(7)) * Main.rand.NextFloat(0.23f, 0.35f);
            d.scale = Main.rand.NextFloat(1.24f, 1.34f);
            d.fadeIn = d.scale * Main.rand.NextFloat(1, 1.17f);
            d.noGravity = true;

            if (Main.rand.NextBool())
            {
                Dust d2 = Dust.NewDustPerfect(Projectile.Center + new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 6);
                d2.scale = 1.34f + Main.rand.NextFloat(0.5f);
                d2.noGravity = true;
                d2.velocity = -Projectile.velocity * 0.4f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                Dust d1 = Dust.NewDustDirect(Projectile.Center, 0, 0, 6);
                d1.velocity = Vector2.One.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToDegrees(18 * i)) * Main.rand.NextFloat(1.8f, 3f);
              //  d1.noGravity = true;
                d1.scale = Main.rand.NextFloat(1.2f, 1.4f);

                Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                d2.velocity = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                d2.noGravity = true;
            }
        }
    }

    public class StarbornStaff : ClericDamageItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases healing potency for each ally healed, and increases damage for each enemy hit");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = ModContent.GetInstance<ClericClass>();
            Item.width = Item.height = 42;
            Item.useTime = Item.useAnimation = 41;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3;
            Item.value = 10000;
            Item.rare = ModContent.RarityType<StellarRarity>();
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StarbornFragment>();
            Item.shootSpeed = 7.8f;
            Item.noMelee = true;

            clericRadianceCost = 7;
            healAmount = 5;
            healRate = 1;
            Item.sellPrice(0, 0, 80);
        }

        #region Ughh overlap 
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.GetModPlayer<excelPlayer>().StellarDamageBonus;
            if (Item.type == ModContent.ItemType<StellarCommandRod>())
            {
                damage *= player.GetModPlayer<excelPlayer>().SpiritDamageMult;
            }
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += player.GetModPlayer<excelPlayer>().StellarCritBonus;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return 1 * player.GetModPlayer<excelPlayer>().StellarUseSpeed;
        }

        public string NormTip = "Damaging enemies increases healing potency and healing allies increases damage";//"Increases healing potency for each ally healed, and increases damage for each enemy hit"; // "Strikes foes with high frequency energy";
        public string PowerTip = "\nSet bonus: Star fragments ricochet"; // "\nSet bonus: Even higher frequency energy increases damage against defense";
      
        public override void ExtraTooltipModifications(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.Mod == "Terraria")
                {
                    if (line2.Name == "Tooltip0")
                    {
                        if (Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().StellarSet)
                        {
                            line2.Text = NormTip + PowerTip;
                        }
                        else
                        {
                            line2.Text = NormTip;
                        }
                    }
                }
            }
        }
        #endregion
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.StellarPlating>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CreateHealProjectile(player, source, position, velocity, type, damage, knockback);
            return false;
        }

        public override float ExtraDamage()
        {
            return Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().StellarDamageBonus;
        }
    }

    public class StarbornFragment : clericHealProj
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 18;
            Projectile.timeLeft = 76;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;

            healPenetrate = -1;
            canDealDamage = true;
            healPenetrate = 3;
            //buffConsumesPenetrate = true;
        }

        int healAmount = 5;

        public override void AI()
        {
            HealCollision(Main.LocalPlayer, Main.player[Projectile.owner]);

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 292);
            d.velocity = Projectile.velocity * 0.8f;
            d.noGravity = true;

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void PostHealEffects(Player target, Player healer)
        {
            Projectile.damage = (int)(Projectile.damage * 1.15f);
            SetBonus();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.lifeMax > 5 && target.type != NPCID.TargetDummy)
            {
                healAmount += 1;
            }
            SetBonus();
        }

        public virtual void SetBonus()
        {
            if (!Main.player[Projectile.owner].GetModPlayer<excelPlayer>().StellarSet)
                return;

            canHealOwner = false;
            bool target = false;
            if (healAmount > 7)
            {
                Vector2 targetPos = Vector2.Zero;
                float targetHealth = 1000;
                float targetDistance = 1000;
                for (int k = 0; k < 200; k++)
                {
                    Player player = Main.player[k];
                    float health = player.statLife;
                    float distance = Vector2.Distance(player.Center, Projectile.Center);
                    if (health < targetHealth && distance < targetDistance && health < player.statLifeMax2) // && player != Main.player[Projectile.owner])
                    {
                        targetDistance = distance;
                        targetHealth = health;
                        targetPos = player.Center;
                        target = true;
                    }
                    /*
                    if (!target && Vector2.Distance(Main.player[Projectile.owner].Center, Projectile.Center) < 1000)
                    {
                        canHealOwner = true;
                        targetPos = Main.player[Projectile.owner].Center;
                        target = true;
                    }
                    */
                }
                if (target)
                {
                    Projectile.velocity = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * 7.8f;
                }
            }
            if (!target)
            {
                Vector2 targetPos = Vector2.Zero;
                float targetDistance = 3000;
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < targetDistance && npc.CanBeChasedBy())
                    {
                        targetDistance = distance;
                        targetPos = npc.Center;
                        target = true;
                    }
                }
                if (target)
                {
                    Projectile.velocity = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * 7.8f;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 292);
                d.noGravity = true;
                d.velocity *= Main.rand.NextFloat(1.2f, 1.6f);
                d.scale = Main.rand.NextFloat(1.1f, 1.3f);
                d.velocity += Projectile.velocity * 0.2f;
            }
        }
    }
}
