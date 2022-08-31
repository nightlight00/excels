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

namespace excels.Items.Weapons.Fossil
{
    #region Mage
    internal class AncientDebris : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Conjures numerous fossilised chunks");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.rare = 6;
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = Item.useAnimation = 37;
            Item.knockBack = 2.4f;
            Item.UseSound = SoundID.Item20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 7;
            Item.shoot = ModContent.ProjectileType<FossilChunks>();
            Item.autoReuse = true;
            Item.mana = 8;
            Item.noMelee = true;
            Item.sellPrice(0, 1, 15);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        { 
            // 6 proj at 22 dmg = 132
            // 5 proj at 24 dmg = 120
            for (var i = 0; i < 5; i++)
            {
                Vector2 vel = velocity.RotatedByRandom(MathHelper.ToRadians(24)) * Main.rand.NextFloat(0.8f, 1.1f);
                Projectile p = Projectile.NewProjectileDirect(source, position, vel, type, damage, knockback, player.whoAmI);
                p.ai[0] = i;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpellTome)
                .AddIngredient(ModContent.ItemType<Materials.AncientFossil>(), 12)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }

    public class FossilChunks : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fossil Chunk");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.FragileBones>(), 120);
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.FragileBones>(), 120);
            }
        }

        public override void AI()
        {
            Projectile.frame = (int)Projectile.ai[0];
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.FossilBoneDust>());
            d.noGravity = true;
            d.scale *= 0.93f;
            d.velocity = Projectile.velocity * 0.4f;

            if (Main.rand.NextBool(10))
            {
                Dust f = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.FossilBoneDust>());
                f.scale *= 0.6f;
                f.velocity = Projectile.velocity * 0.7f;
            }

            Projectile.rotation += Projectile.velocity.X * 2;
            Projectile.velocity.Y += 0.08f;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 15; i++)
            {
                float vel = Main.rand.NextFloat(0.5f, 1.75f);
                Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<Dusts.FossilBoneDust>());
                d.velocity = new Vector2(vel, vel).RotatedByRandom(MathHelper.ToRadians(180));
                d.scale *= Main.rand.NextFloat(0.98f, 1.08f);
                d.noGravity = true;
            }
        }
    }
    #endregion

    #region Melee
    public class FossilisedBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Killing foes causes an eruption of fossils");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 40;
            Item.DamageType = DamageClass.Melee;
            Item.rare = 6;
            Item.knockBack = 6.2f;
            Item.damage = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useTime = Item.useAnimation = 20;
            Item.scale = 1.2f;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.crit = 3;
            Item.sellPrice(0, 1, 35);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.FragileBones>(), 180);
            if (target.life <= 0 && target.lifeMax > 5)
            {
                for (var i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, new Vector2(Main.rand.NextFloat(-1.8f, 1.8f), Main.rand.NextFloat(-6, -4)), ModContent.ProjectileType<FossilChunkM>(), (int)(damage * 0.7f), knockBack / 2, player.whoAmI);
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.FragileBones>(), 180);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust d = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.FossilBoneDust>());
            d.noGravity = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.AncientFossil>(), 18)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class FossilChunkM : ModProjectile
    {
        public override string Texture => "excels/Items/Weapons/Fossil/FossilChunks";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fossil Chunk");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 160;
            Projectile.friendly = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(4))
            {
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.FragileBones>(), 240);
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(4))
            {
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.FragileBones>(), 240);
            }
        }

        public override void AI()
        {
            Projectile.frame = (int)Projectile.ai[0];
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.FossilBoneDust>());
            d.noGravity = true;
            d.scale *= 0.93f;
            d.velocity = Projectile.velocity * 0.4f;

            if (Main.rand.NextBool(10))
            {
                Dust f = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.FossilBoneDust>());
                f.scale *= 0.6f;
                f.velocity = Projectile.velocity * 0.7f;
            }

            Projectile.rotation += Projectile.velocity.X * 2;
            Projectile.velocity.Y += 0.08f;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 15; i++)
            {
                float vel = Main.rand.NextFloat(0.5f, 1.75f);
                Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<Dusts.FossilBoneDust>());
                d.velocity = new Vector2(vel, vel).RotatedByRandom(MathHelper.ToRadians(180));
                d.scale *= Main.rand.NextFloat(0.98f, 1.08f);
                d.noGravity = true;
            }
        }
    }

    #endregion

    #region Ranged 
    public class FossilCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fossilised Chomper");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.shoot = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.rare = 6;
            Item.damage = 33;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 46;
            Item.height = 24;
            Item.shootSpeed = 9;
            Item.useTime = Item.useAnimation = 28;
            Item.knockBack = 4;
            Item.UseSound = SoundID.Item36;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.sellPrice(0, 1, 25);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.AncientFossil>(), 18)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 0f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(7)) * 0.5f, type, (int)(damage * 0.8f), knockback / 2, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-7)) * 0.5f, type, (int)(damage * 0.8f), knockback / 2, player.whoAmI);

            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FossilChunkR>(), damage, knockback, player.whoAmI);
            return false;
        }
    }
    
    public class FossilChunkR : ModProjectile
    {
        public override string Texture => "excels/Items/Weapons/Fossil/FossilChunks";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fossil Chunk");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.FragileBones>(), 200);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.FragileBones>(), 200);
        }

        public override void AI()
        {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.FossilBoneDust>());
            d.noGravity = true;
            d.scale *= 1.13f;
            d.velocity = Projectile.velocity * 0.4f;

            if (Main.rand.NextBool(10))
            {
                Dust f = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.FossilBoneDust>());
                f.scale *= 0.6f;
                f.velocity = Projectile.velocity * 0.7f;
            }

            Projectile.rotation += Projectile.velocity.X * 2;
        }
        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 15; i++)
            {
                float vel = Main.rand.NextFloat(0.5f, 1.75f);
                Dust d = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<Dusts.FossilBoneDust>());
                d.velocity = new Vector2(vel, vel).RotatedByRandom(MathHelper.ToRadians(180));
                d.scale *= Main.rand.NextFloat(0.98f, 1.08f);
                d.noGravity = true;
            }
        }
    }
    #endregion
}
