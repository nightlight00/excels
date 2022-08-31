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

namespace excels.Items.Weapons.Flamethrower
{
    #region Hellslinger
    internal class Hellslinger : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses gel for ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 14;
            Item.knockBack = 1.3f;
            Item.noMelee = true;
            Item.useTime = 7;
            Item.useAnimation = 21;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.rare = 3;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 5;
            Item.useAmmo = ItemID.Gel;
            Item.shoot = ModContent.ProjectileType<HellFlames>();
            Item.sellPrice(0, 0, 40);

            Item.useTurn = false;
            Item.autoReuse = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, 0);
        }

        public override bool CanUseItem(Player player)
        {
            return !player.wet;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 16)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int InheritedType = type;
            type = Item.shoot;
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (InheritedType == ProjectileID.MolotovFire2)
                p.frame = 1;
            if (InheritedType == ProjectileID.MolotovFire3)
                p.frame = 2;
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < 18)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class HellFlames : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = (int)(60 * 0.8f);
            Projectile.extraUpdates = 2;
            Projectile.width = Projectile.height = 10;
            Projectile.alpha = 255;
            Projectile.velocity *= 2f;
            Projectile.penetrate = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(DebuffType, 900);
            Projectile.damage = (int)(Projectile.damage * 0.83f);
            HitCheck(Main.player[Projectile.owner], target, 900);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(DebuffType, 900);
        }

        public override void AI()
        {
            if (++Projectile.ai[0] > 7f)
            {
                float num297 = 1f;
                if (Projectile.ai[0] == 8f)
                {
                    num297 = 0.25f;
                }
                else
                {
                    if (Projectile.ai[0] == 9f)
                    {
                        num297 = 0.5f;
                    }
                    else
                    {
                        if (Projectile.ai[0] == 10f)
                        {
                            num297 = 0.75f;
                        }
                    }
                }
                for (int i = 0; i < 1; i++)
                {
                    int num300 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
                    Dust dust3;
                    if (!Main.rand.NextBool(3))
                    {
                        Main.dust[num300].noGravity = true;
                        dust3 = Main.dust[num300];
                        dust3.scale *= 3f;
                        Dust dust52 = Main.dust[num300];
                        dust52.velocity.X = dust52.velocity.X * 2f;
                        Dust dust53 = Main.dust[num300];
                        dust53.velocity.Y = dust53.velocity.Y * 2f;
                    }

                    dust3 = Main.dust[num300];
                    dust3.scale *= 1.5f;

                    Dust dust54 = Main.dust[num300];
                    dust54.velocity.X = dust54.velocity.X * 1.2f;
                    Dust dust55 = Main.dust[num300];
                    dust55.velocity.Y = dust55.velocity.Y * 1.2f;
                    dust3 = Main.dust[num300];
                    dust3.scale *= num297;

                    if (DustType == 92)
                    {
                        Main.dust[num300].velocity *= 0.35f;
                        Main.dust[num300].scale *= 0.85f;
                    }
                }
            }
            Projectile.rotation += 0.3f * (float)Projectile.direction;
        }
    }
    #endregion

    #region Hypothermia
    class Hypothermia : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Smothers gel with nitrogen and frostfire");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Flamethrower);

            Item.damage = 22;
            Item.rare = ItemRarityID.Pink;
            Item.shootSpeed = 13.4f;
            Item.shoot = ModContent.ProjectileType<HypothermalInferno>();
            Item.height = 16;
            Item.width = 68;
            Item.useTime = 6;
            Item.useAnimation = 24;
            Item.scale = 1.1f;
            Item.sellPrice(0, 1, 20);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < player.itemAnimationMax - 4)
            {
                return false;
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.wet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int InheritedType = type;
            type = Item.shoot;
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }

    class HypothermalInferno : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 30;
            Projectile.width = Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.velocity *= 2f;
            Projectile.penetrate = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1; // 1 hit per npc max
            Projectile.localNPCHitCooldown = 15;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 1200);
            Projectile.damage = (int)(Projectile.damage * 0.85f);
            HitCheck(Main.player[Projectile.owner], target, 1200);
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 27)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dst = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f, 0, default(Color), 2.5f);
                    dst.noGravity = true;
                    dst.rotation += 0.2f;
                    dst.fadeIn = Main.rand.NextFloat(2.2f, 2.9f);
                    dst.scale = dst.fadeIn * 0.85f;
                }
                if (Main.rand.NextBool())
                {
                    Dust dst2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185, 0, 0, 0, default(Color), 1.25f);
                    dst2.rotation += 0.2f;
                    //  dst2.velocity.X += Projectile.velocity.X / 5;
                }

            }
        }
    }
    #endregion

    #region Trailblazer
    internal class Trailblazer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("20% chance to not consume ammo\nRapidly spews lasting flames");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 30;
            Item.knockBack = 0.8f;
            Item.noMelee = true;
            Item.useTime = 5;
            Item.useAnimation = 10;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.rare = 8;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 8.75f;
            Item.useAmmo = ItemID.Gel;
            Item.shoot = ModContent.ProjectileType<TrailBlaze>();

            Item.useTurn = false;
            Item.autoReuse = true;
            Item.sellPrice(0, 5);
        }

        public override bool CanUseItem(Player player)
        {
            return !player.wet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-22, -8);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int InheritedType = type;
            type = Item.shoot;
            for (var i = 0; i < 2; i++)
            {
                Projectile p = Projectile.NewProjectileDirect(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(13)) * Main.rand.NextFloat(0.8f, 1.15f), type, damage, knockback, player.whoAmI);
                if (InheritedType == ProjectileID.MolotovFire2)
                    p.frame = 1;
                if (InheritedType == ProjectileID.MolotovFire3)
                    p.frame = 2;
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < player.itemAnimationMax - 2)
            {
                return false;
            }
            return Main.rand.Next() > .2f;
        }
    }

    public class TrailBlaze : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Flames);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.alpha = 100;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 7;
            Projectile.width = Projectile.height = 16;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.extraUpdates = 2;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(DebuffType, 300);
            HitCheck(Main.player[Projectile.owner], target, 300);
            Projectile.damage = (int)(Projectile.damage * 0.92f);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(DebuffType, 1800);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = (0f - oldVelocity.X) * 0.8f;
            }

            if (oldVelocity.Y != Projectile.velocity.Y)
            {
                Projectile.velocity.Y = (0f - oldVelocity.Y) * 0.4f;
            }

            return false;
        }

        public override void AI()
        {
            if (++Projectile.ai[0] > 13)
            {
                Projectile.velocity.Y += 0.07f;
            }
            Projectile.velocity.X *= 0.994f;

            Projectile.rotation = Projectile.velocity.X * -0.25f;

            if (Main.rand.NextBool()) { 
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType);
                d.scale = Main.rand.NextFloat(0.5f, 0.7f);
                d.velocity = -Projectile.velocity * 0.1f;
                d.noGravity = true;
            }
        }
    }
    #endregion

    #region Spark Spitter
    internal class SparkSpitter : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses gel for ammo\n'Who needs a wand when you got this!'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 10;
            Item.knockBack = 3.8f;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 41;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item61;
            Item.rare = 1;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 6;
            Item.useAmmo = ItemID.Gel;
            Item.shoot = ModContent.ProjectileType<Spark>();

            Item.useTurn = false;
            Item.autoReuse = true;
            Item.sellPrice(0, 0, 20);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -2);
        }

        public override bool CanUseItem(Player player)
        {
            return !player.wet;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 24)
                .AddIngredient(ItemID.Torch)
                .AddTile(TileID.LivingLoom)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int InheritedType = type;
            type = Item.shoot;
            for (var i = 0; i < 3; i++)
            {
                Projectile p = Projectile.NewProjectileDirect(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(16)) * (Main.rand.NextFloat(0.8f, 1.1f)+(0.2f*i)), type, damage, knockback, player.whoAmI);
                if (InheritedType == ProjectileID.MolotovFire2)
                    p.frame = 1;
                if (InheritedType == ProjectileID.MolotovFire3)
                    p.frame = 2;
            }
            return false;
        }
    }

    public class Spark : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";
        
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 4;
            Projectile.timeLeft = 40;
            Projectile.alpha = 255;
            Projectile.penetrate = 2;
            
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(DebuffType, 120);
            Projectile.damage = (int)(Projectile.damage * 0.83f);
            HitCheck(Main.player[Projectile.owner], target, 120);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(DebuffType, 120);
        }

        public override void AI() 
        { 
            if (++Projectile.ai[0] > 15)
                Projectile.velocity.Y += 0.07f;

            if (Projectile.ai[0] > 4)
            {
                if (Projectile.ai[0] % 3 == 0)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustType);
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(1.3f, 1.4f);
                    d.velocity = Vector2.Zero;
                }

                Dust dd = Dust.NewDustDirect(Projectile.Center, 0, 0, DustType);
                dd.noGravity = true;
                dd.velocity = Main.rand.NextVector2Circular(1.4f, 1.4f) * 2;
                dd.scale = Main.rand.NextFloat(0.6f, 0.8f);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                Dust dd = Dust.NewDustDirect(Projectile.Center, 0, 0, DustType);
                dd.noGravity = true;
                dd.velocity = Main.rand.NextVector2Circular(1.4f, 1.4f) * Main.rand.NextFloat(3, 3.8f);
                dd.scale = Main.rand.NextFloat(0.9f, 1.1f);
            }
        }
    }
    #endregion

    #region Roaster
    internal class Roaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses gel for ammo\nSpecial lava infused flames leave scorching marks");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 19;
            Item.knockBack = 1.8f;
            Item.noMelee = true;
            Item.useTime = 14;
            Item.useAnimation = 28;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.rare = ItemRarityID.LightRed;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 2.9f;
            Item.useAmmo = ItemID.Gel;
            Item.shoot = ModContent.ProjectileType<RoasterLava>();

            Item.useTurn = false;
            Item.autoReuse = true;
            Item.sellPrice(0, 2, 10);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, -2);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(14));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int InheritedType = type;
            type = Item.shoot;
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (InheritedType == ProjectileID.MolotovFire2)
                p.frame = 1;
            if (InheritedType == ProjectileID.MolotovFire3)
                p.frame = 2;
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.wet;
        }


        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < 18)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class RoasterLava : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 10;
            Projectile.timeLeft = 100;
            Projectile.extraUpdates = 4;
            Projectile.penetrate = 99;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.alpha = 255;
        }

        int fakePen = 4;
        int damage;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            fakePen--;
            if (Main.player[Projectile.owner].GetModPlayer<excelPlayer>().FireBadge)
                fakePen--;
            if (fakePen <= 0)
                Explosion();

            target.AddBuff(DebuffType, (int)(300*(Projectile.ai[1]+1)));
            Projectile.damage = (int)(Projectile.damage * 0.93f);
            HitCheck(Main.player[Projectile.owner], target, (int)(300 * (Projectile.ai[1] + 1)));
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(DebuffType, (int)(300 * (Projectile.ai[1] + 1)));
        }

        public override void AI()
        {
            if (++Projectile.ai[1] == 1)
            {
                damage = Projectile.damage;
            }

            if (Projectile.ai[0] == 0 && Projectile.ai[1] > 14)
            {
                for (var i = 0; i < 2; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType);
                    d.velocity = Projectile.velocity * 0.35f;
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(3.8f, 4.1f);
                }

                if (Main.rand.NextBool())
                {
                    Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType);
                    d2.velocity = Main.rand.NextVector2Circular(2, 2) * Main.rand.NextFloat(2, 2.5f);
                }
            }
            if (Projectile.timeLeft == 1)
                Explosion();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explosion();
            return false;
        }

        private void Explosion()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
                Projectile.tileCollide = false;
                Projectile.position = Projectile.Center;
                Projectile.width = Projectile.height = 120;
                Projectile.Center = Projectile.position;
                Projectile.timeLeft = 8;
                Projectile.penetrate = -1;
                Projectile.damage = (int)(damage * 1.75f);
                Projectile.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
                for (var i = 0; i < 70; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, DustType);
                    d.velocity = Main.rand.NextVector2CircularEdge(3, 3)*Main.rand.NextFloat(1, 3);
                    d.scale = Main.rand.NextFloat(3f, 3.7f);
                    d.noGravity = true;
                    if (i % 5 == 0)
                    {
                        Dust dd = Dust.NewDustDirect(Projectile.Center, 0, 0, DustType);
                        dd.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(2, 3.2f);
                    }
                }
            }
        }
    }
    #endregion

    #region Infernal Bubbler
    internal class FlameBubbler : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernal Bubbler");
            Tooltip.SetDefault("Uses gel for ammo\nSpews lava filled bubbles\nRight click to fire a stream of fire that can ignite the bubbles");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 47;
            Item.knockBack = 1.2f;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 14;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.rare = 8;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 2.7f;
            Item.useAmmo = ItemID.Gel;
            Item.shoot = ModContent.ProjectileType<RoasterLava>();

            Item.useTurn = false;
            Item.autoReuse = true;
            Item.sellPrice(0, 7);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, -6);
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ModContent.ProjectileType<IgnitionStream>();
                Item.shootSpeed = 3.2f;
                Item.useTime = Item.useAnimation = 14;
                Item.UseSound = SoundID.Item34;
            }
            else
            {
                Item.shoot = ModContent.ProjectileType<LavaBubble>();
                Item.shootSpeed = 11.2f;
                Item.useTime = 4;
                Item.useAnimation = 14;
                Item.UseSound = SoundID.Item85;
            }
            return !player.wet;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Roaster>())
                .AddIngredient(ItemID.BubbleGun)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
            else
            {
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(18)) * Main.rand.NextFloat(0.85f, 1.15f);
                damage /= 2;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int InheritedType = type;
            type = Item.shoot;
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (InheritedType == ProjectileID.MolotovFire2)
                p.frame = 1;
            if (InheritedType == ProjectileID.MolotovFire3)
                p.frame = 2;
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < 13)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class IgnitionStream : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 24;
            Projectile.timeLeft = 100;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 8;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(DebuffType, 120);
            Projectile.damage = (int)(Projectile.damage * 0.83f);
            HitCheck(Main.player[Projectile.owner], target, 120);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(DebuffType, 900);
        }

        public override void AI()
        { 
            for (var i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType);
                d.velocity = Projectile.velocity * 0.35f;
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(2.4f, 3f);
            }

            if (Main.rand.NextBool())
            {
                Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType);
                d2.velocity = (Projectile.velocity * 0.4f) + (Main.rand.NextVector2Circular(2, 2) * Main.rand.NextFloat(0.9f, 1.3f));
            }

            for (var i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active)
                {
                    if (p.type == ModContent.ProjectileType<LavaBubble>())
                    {
                        if (Collision.CheckAABBvAABBCollision(Projectile.position, new Vector2(Projectile.width, Projectile.height), p.position, new Vector2(p.width, p.height)))
                            if (p.ai[1] == 0)
                                p.ai[1] = 1;
                    }
                }
            }
        }
    }

    public class LavaBubble : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 500;
            Projectile.penetrate = 99;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.alpha = 80;
            Projectile.scale = 0.1f;
        }

        int fakePen = 10;
        float maxScale = Main.rand.NextFloat(0.6f, 1.1f);

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.damage = (int)(Projectile.damage * 0.95f);
                fakePen--;
                if (Main.player[Projectile.owner].GetModPlayer<excelPlayer>().FireBadge)
                    fakePen--;
                if (fakePen <= 0)
                    Projectile.Kill();
            }
            target.AddBuff(DebuffType, 900);
            HitCheck(Main.player[Projectile.owner], target, 900);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(DebuffType, 900);
        }

        public override void AI()
        {
            Projectile.velocity *= 0.96f;
            if (Projectile.ai[1] == 1)
                Explosion();
            else
            {
                if (Projectile.scale < maxScale)
                {
                    Projectile.position = Projectile.Center;
                    Projectile.scale += maxScale / 30;
                    Projectile.width = Projectile.height = (int)(32 * Projectile.scale);
                    Projectile.Center = Projectile.position;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = (0f - oldVelocity.X);
            }

            if (oldVelocity.Y != Projectile.velocity.Y)
            {
                Projectile.velocity.Y = (0f - oldVelocity.Y);
            }

            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[1] == 0)
            {
                for (var i = 0; i < 20; i++)
                {
                    Vector2 spd = Main.rand.NextVector2CircularEdge(16 * Projectile.scale, 16 * Projectile.scale);
                    Dust d = Dust.NewDustDirect(Projectile.Center+spd, 0, 0, DustType);
                    d.velocity = spd / 4;
                    d.noGravity = true; 
                }
                SoundEngine.PlaySound(SoundID.Item112, Projectile.Center);
            }
        }

        public void Explosion()
        {
            Projectile.ai[1] = 2;
            Projectile.tileCollide = false;
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 120;
            Projectile.Center = Projectile.position;
            Projectile.timeLeft = 8;
            Projectile.penetrate = -1;
            Projectile.damage *= 4; // since its original damage was halved, this just doubles that 
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
            for (var i = 0; i < 70; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, DustType);
                d.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(1, 3);
                d.scale = Main.rand.NextFloat(3f, 3.7f);
                d.noGravity = true;
                if (i % 5 == 0)
                {
                    Dust dd = Dust.NewDustDirect(Projectile.Center, 0, 0, DustType);
                    dd.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(2, 3.2f);
                }
            }

            for (var i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active)
                {
                    if (p.type == ModContent.ProjectileType<LavaBubble>())
                    {
                        if (Collision.CheckAABBvAABBCollision(Projectile.position, new Vector2(Projectile.width, Projectile.height), p.position, new Vector2(p.width, p.height)))
                            if (p.ai[1] == 0)
                                p.ai[1] = 1;
                    }
                }
            }
        }
    }
    #endregion

    #region Overcharged Flashlight
    internal class OverchargedFlashlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses gel for ammo\nIgnores 8 points of enemy defense\n'Great for exploring, or burning faces off!'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 6;
            Item.ArmorPenetration = 8;
            Item.noMelee = true;
            Item.useTime = 4;
            Item.useAnimation = 36;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.rare = 1;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 5;
            Item.useAmmo = AmmoID.Gel;
            Item.shoot = ModContent.ProjectileType<OverchargedLight>();
            
            Item.channel = true;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.sellPrice(0, 0, 15);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, 0);
        }

        public override bool CanUseItem(Player player)
        {
            return !player.wet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] < 1)
            {
                int InheritedType = type;
                type = Item.shoot;
                Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (InheritedType == ProjectileID.MolotovFire2)
                    p.frame = 1;
                if (InheritedType == ProjectileID.MolotovFire3)
                    p.frame = 2;
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < 34)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class OverchargedLight : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 24;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(DebuffType, 180);
            HitCheck(Main.player[Projectile.owner], target, 180);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.channel || player.wet || Projectile.wet)
                Projectile.Kill();

            Projectile.timeLeft = 2;
            player.heldProj = Projectile.whoAmI;

            Vector2 pos = player.Center;
            if (Main.myPlayer == Main.player[Projectile.owner].whoAmI) {
                for (var i = 0; i < 50; i++)
                {
                    pos += (Main.MouseWorld-pos).SafeNormalize(Vector2.Zero) * 4;
                    if (Collision.WetCollision(pos-new Vector2(Projectile.width, Projectile.height)/2, Projectile.width, Projectile.height))
                    {
                        Projectile.Kill();
                        return;
                    }
                    if (Collision.SolidTiles(pos+(new Vector2(Projectile.width, Projectile.height)/12), Projectile.width/6, Projectile.height/6))
                        break;

                }
            }

            Projectile.Center = pos;
            Lighting.AddLight(Projectile.Center, new Vector3(1.25f, 1.2f, 1.2f));
            Lighting.AddLight((player.Center + Projectile.Center) / 2, new Vector3(0.625f, 0.6f, 0.6f));

            for (var i = 0; i < 3; i++)
            {
                Vector2 endPos = pos + new Vector2(Projectile.width/2, 0).RotatedBy(MathHelper.ToRadians(120 * i + Projectile.ai[0]));
                Dust d = Dust.NewDustPerfect(endPos, DustType);
                d.noGravity = true;
                d.scale = 1.5f;
                d.velocity = Vector2.Zero;

                /*
                for (var s = 2; s < 8; s++)
                {
                    Dust w = Dust.NewDustPerfect(player.Center + (pos-player.Center+new Vector2()); //Dust.NewDustPerfect(player.Center + ((endPos-player.Center) / 8 * s), DustType);
                    w.noGravity = true;
                    w.scale = 0.8f;
                    w.velocity = Vector2.Zero;
                }
                */        }

             for (var s = 2; s < 8; s++)
             {
                Dust w = Dust.NewDustPerfect(player.Center + ((pos-player.Center) / 8) * s, DustType); //Dust.NewDustPerfect(player.Center + ((endPos-player.Center) / 8 * s), DustType);
                w.noGravity = true;
                w.scale = 0.8f;
                w.velocity = Vector2.Zero;
             } 
            
             Projectile.ai[0] += 5;
        }
    }
    #endregion

    #region Solar Reverie
    internal class SolarEngine : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Reverie");
            Tooltip.SetDefault("Uses gel for ammo\nIgnores 25 points of enemy defense\n'TASTE THE SUN'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 38;
            Item.ArmorPenetration = 25;
            Item.noMelee = true;
            Item.useTime = 3;
            Item.useAnimation = 33;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.rare = 8;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 5;
            Item.useAmmo = AmmoID.Gel;
            Item.shoot = ModContent.ProjectileType<SolarLight>();

            Item.channel = true;

            Item.useTurn = false;
            Item.autoReuse = true;
            Item.sellPrice(0, 4, 75);
        }

        public override bool CanUseItem(Player player)
        {
            return !player.wet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -2);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] < 1)
            {
                int InheritedType = type;
                type = Item.shoot;
                Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (InheritedType == ProjectileID.MolotovFire2)
                    p.frame = 1;
                if (InheritedType == ProjectileID.MolotovFire3)
                    p.frame = 2;
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < 31)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class SolarLight : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 80;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(DebuffType, 320);
            HitCheck(Main.player[Projectile.owner], target, 320);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.channel || player.wet || Projectile.wet)
                Projectile.Kill();

            Projectile.timeLeft = 2;
            player.heldProj = Projectile.whoAmI;

            Vector2 pos = player.Center;
            if (Main.myPlayer == Main.player[Projectile.owner].whoAmI)
            {
                for (var i = 0; i < 120; i++)
                {
                    pos += (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 4;
                    if (Collision.WetCollision(pos - new Vector2(Projectile.width, Projectile.height) / 2, Projectile.width, Projectile.height))
                    {
                        Projectile.Kill();
                        return;
                    }
                    if (Collision.SolidTiles(pos + (new Vector2(Projectile.width, Projectile.height) * 0.05f), Projectile.width / 10, Projectile.height / 10))
                        break;

                }
            }

            Projectile.Center = pos;
            Lighting.AddLight(Projectile.Center, new Vector3(2.2f, 1.95f, 1.95f));
            Lighting.AddLight((player.Center + Projectile.Center) / 2, new Vector3(1.2f, 1f, 1f));

            for (var i = 0; i < 4; i++)
            {
                Vector2 endPos = pos + new Vector2(Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians((360/4) * i + Projectile.ai[0]));
                Dust d = Dust.NewDustPerfect(endPos, DustType);
                d.noGravity = true;
                d.scale = 2.4f;
                d.velocity = Vector2.Zero;
            }

            for (var s = 2; s < 12; s++)
            {
                Dust w = Dust.NewDustPerfect(player.Center + ((pos - player.Center) / 12) * s, DustType); //Dust.NewDustPerfect(player.Center + ((endPos-player.Center) / 8 * s), DustType);
                w.noGravity = true;
                w.scale = 0.8f;
                w.velocity = Vector2.Zero;
            }

            Projectile.ai[0] += 5.2f;

            // Orbiting rays

            for (var o = 0; o < 4; o++)
            {
                Vector2 pos2 = Projectile.Center + new Vector2(96, 0).RotatedBy(MathHelper.ToRadians(90 * o - Projectile.ai[0] / 2));
                for (var i = 0; i < 3; i++)
                {
                    Dust w = Dust.NewDustPerfect(pos2+new Vector2(Projectile.width/4,0).RotatedBy(MathHelper.ToRadians(120*i+Projectile.ai[1])), DustType);
                    w.noGravity = true;
                    w.scale = 1.7f;
                    w.velocity = Vector2.Zero;
                }
            }

            Projectile.ai[1] += 16.9f; // 11.5
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool Col = false;
            for (var i = 0; i < 4; i++)
            {
                if (Collision.CheckAABBvAABBCollision(Projectile.position + new Vector2(96, 0).RotatedBy(MathHelper.ToRadians(90 * i - Projectile.ai[0] / 2)), new Vector2(Projectile.width, Projectile.height) / 2, new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height)))
                    Col = true;
            }
            return Col || Collision.CheckAABBvAABBCollision(Projectile.position, new Vector2(Projectile.width, Projectile.height), new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height));
        }
    }
    #endregion

    #region Wielder
    internal class BoltTorch : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wielder");
            Tooltip.SetDefault("Uses gel for ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 16;
            Item.knockBack = 1.2f;
            Item.noMelee = true;
            Item.useTime = 14;
            Item.useAnimation = 28;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.rare = 3;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 2.7f;
            Item.useAmmo = ItemID.Gel;
            Item.shoot = ModContent.ProjectileType<TorchBolt>();

            Item.useTurn = false;
            Item.autoReuse = true;
            Item.sellPrice(0, 0, 90);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }

        public override bool CanUseItem(Player player)
        {
            return !player.wet;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int InheritedType = type;
            type = Item.shoot;
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (InheritedType == ProjectileID.MolotovFire2)
                p.frame = 1;
            if (InheritedType == ProjectileID.MolotovFire3)
                p.frame = 2;
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < 22)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class TorchBolt : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 90;
            Projectile.extraUpdates = 2;
            Projectile.width = Projectile.height = 14;
            Projectile.alpha = 255;
            Projectile.velocity *= 2f;
            Projectile.penetrate = 4;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        int BounceAmount = 2;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(DebuffType, 900);
            Projectile.damage = (int)(Projectile.damage * 0.8f);
            HitCheck(Main.player[Projectile.owner], target, 900);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(DebuffType, 900);
        }

        public override void AI()
        {
            if (++Projectile.ai[0] > 8) {
                Projectile.position = Projectile.Center;
                Projectile.scale += 0.025f;
                Projectile.width = Projectile.height = (int)(14 * Projectile.scale);
                Projectile.Center = Projectile.position;

                for (var i = 0; i < 2; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType);
                    d.velocity = (d.position - (Projectile.Center - Projectile.velocity * 30)).SafeNormalize(Vector2.Zero) * 1.4f;
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(1.8f, 2.3f) * Projectile.scale;
                }
            }
        }
    }
    #endregion

     #region Afterburner
    internal class AfterBurner : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("");
            Tooltip.SetDefault("Uses gel for ammo\n'Isn't this a bit overkill?'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 23;
            Item.knockBack = 1.2f;
            Item.noMelee = true;
            Item.useTime = 9;
            Item.useAnimation = 36;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.rare = 4;

            Item.height = 16;
            Item.width = 46;
            Item.shootSpeed = 2.3f;
            Item.useAmmo = ItemID.Gel;
            Item.shoot = ModContent.ProjectileType<TorchBolt>();

            Item.useTurn = false;
            Item.autoReuse = true;
            Item.sellPrice(0, 1, 85);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }

        public override bool CanUseItem(Player player)
        {
            return !player.wet;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int InheritedType = type;
            type = Item.shoot;
            for (var i = -2; i < 3; i++)
            {
                Projectile p = Projectile.NewProjectileDirect(source, position, velocity.RotatedBy(MathHelper.ToRadians(i*35)), type, damage, knockback, player.whoAmI);
                if (InheritedType == ProjectileID.MolotovFire2)
                    p.frame = 1;
                if (InheritedType == ProjectileID.MolotovFire3)
                    p.frame = 2;
            }   
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < 33)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BoltTorch>())
                .AddIngredient(ItemID.Shotgun)
                .AddIngredient(ItemID.SoulofLight, 7)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    #endregion



    // For use with vanilla flamethrowers and different ammo types
    #region Extra Projectiles 
    public class ShadowfireFlames : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 2;
            Projectile.width = Projectile.height = 10;
            Projectile.alpha = 255;
            Projectile.velocity *= 2f;
            Projectile.penetrate = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 900);
            Projectile.damage = (int)(Projectile.damage * 0.83f);
            HitCheck(Main.player[Projectile.owner], target);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 900);
        }

        public override void AI()
        {
            if (++Projectile.ai[0] > 7f)
            {
                float num297 = 1f;
                if (Projectile.ai[0] == 8f)
                {
                    num297 = 0.25f;
                }
                else
                {
                    if (Projectile.ai[0] == 9f)
                    {
                        num297 = 0.5f;
                    }
                    else
                    {
                        if (Projectile.ai[0] == 10f)
                        {
                            num297 = 0.75f;
                        }
                    }
                }
                for (int i = 0; i < 1; i++)
                {
                    int num300 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.ShadowFire2>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
                    Dust dust3;
                    if (!Main.rand.NextBool(3))
                    {
                        Main.dust[num300].noGravity = true;
                        dust3 = Main.dust[num300];
                        dust3.scale *= 3f;
                        Dust dust52 = Main.dust[num300];
                        dust52.velocity.X = dust52.velocity.X * 2f;
                        Dust dust53 = Main.dust[num300];
                        dust53.velocity.Y = dust53.velocity.Y * 2f;
                    }

                    dust3 = Main.dust[num300];
                    dust3.scale *= 1.5f;

                    Dust dust54 = Main.dust[num300];
                    dust54.velocity.X = dust54.velocity.X * 1.2f;
                    Dust dust55 = Main.dust[num300];
                    dust55.velocity.Y = dust55.velocity.Y * 1.2f;
                    dust3 = Main.dust[num300];
                    dust3.scale *= num297;
                }
            }
            Projectile.rotation += 0.3f * (float)Projectile.direction;
        }
    }

    public class FrostfireFlames : FlamethrowerProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 2;
            Projectile.width = Projectile.height = 10;
            Projectile.alpha = 255;
            Projectile.velocity *= 2f;
            Projectile.penetrate = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 900);
            Projectile.damage = (int)(Projectile.damage * 0.83f);
            HitCheck(Main.player[Projectile.owner], target);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 900);
        }

        public override void AI()
        {
            if (++Projectile.ai[0] > 7f)
            {
                float num297 = 1f;
                if (Projectile.ai[0] == 8f)
                {
                    num297 = 0.25f;
                }
                else
                {
                    if (Projectile.ai[0] == 9f)
                    {
                        num297 = 0.5f;
                    }
                    else
                    {
                        if (Projectile.ai[0] == 10f)
                        {
                            num297 = 0.75f;
                        }
                    }
                }
                for (int i = 0; i < 1; i++)
                {
                    int num300 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 92, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
                    Dust dust3;
                    if (!Main.rand.NextBool(3))
                    {
                        Main.dust[num300].noGravity = true;
                        dust3 = Main.dust[num300];
                        dust3.scale *= 3f;
                        Dust dust52 = Main.dust[num300];
                        dust52.velocity.X = dust52.velocity.X * 2f;
                        Dust dust53 = Main.dust[num300];
                        dust53.velocity.Y = dust53.velocity.Y * 2f;
                    }

                    dust3 = Main.dust[num300];
                    dust3.scale *= 1.5f;

                    Dust dust54 = Main.dust[num300];
                    dust54.velocity.X = dust54.velocity.X * 1.2f;
                    Dust dust55 = Main.dust[num300];
                    dust55.velocity.Y = dust55.velocity.Y * 1.2f;
                    dust3 = Main.dust[num300];
                    dust3.scale *= num297;


                    Main.dust[num300].velocity *= 0.35f;
                    Main.dust[num300].scale *= 0.85f;
                }
            }
            Projectile.rotation += 0.3f * (float)Projectile.direction;
        }
    }
    #endregion
}
