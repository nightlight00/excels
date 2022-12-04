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
using System.Collections.Generic;

namespace excels.Items.Accessories.Cleric.Healing
{
    internal class Antitoxins : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti-Toxins");
            Tooltip.SetDefault("Healing poisoned allies cures and heals them");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 28;
            Item.rare = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().antitoxinBottle = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledHoney)
                .AddIngredient(ItemID.JungleSpores, 5)
                .AddIngredient(ItemID.Stinger, 3)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }

    internal class JungleBrew : ModItem
    {
        public override string Texture => "excels/Items/Accessories/Cleric/Healing/MedicsHandbag";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Medic's Handbag");
            Tooltip.SetDefault("Healing allies cures them of On Fire!, Poisoned, and Bleeding and gives minor healing over time");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 30;
            Item.height = 34;
            Item.rare = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().medicBag = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ApothSatchel>())
                .AddIngredient(ModContent.ItemType<Antitoxins>())
                .AddIngredient(ModContent.ItemType<NectarBottle>())
                .AddIngredient(ModContent.ItemType<SoothingCream>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    internal class NectarBottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nectar Bottle");
            Tooltip.SetDefault("Buffing allies lasts an additional 3 seconds \nHealing allies additionally increases their life regeneration");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 28;
            Item.rare = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().buffBonus += 3;
            player.GetModPlayer<excelPlayer>().nectarBottle = true;
        }
    }

    [AutoloadEquip(EquipType.Waist)]
    internal class ApothSatchel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Apothecary's Satchel");
            Tooltip.SetDefault("'Contains a plentitude of medcinial herbs' \nHealing gives an additional 1 health");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 30;
            Item.height = 34;
            Item.rare = 0;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().healBonus += 1;
        }
    }

    internal class MiniatureCross : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stained Glass Cross");
            Tooltip.SetDefault("While under a damaging effect, healing gives an additional 2 health");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 30;
            Item.height = 34;
            Item.rare = 2;
        }

        public override void UpdateEquip(Player player)
        {
            // bonus healing is done after updatebadliferegen
            player.GetModPlayer<excelPlayer>().glassCross = true;
        }
    }

    internal class SoothingCream : ModItem 
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Decreases the duration of On Fire! for you and healed allies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 30;
            Item.height = 34;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().soothingCream = true;
            for (var i = 0; i < player.CountBuffs(); i++)
            {
                if (player.buffType[i] == BuffID.OnFire)
                {
                    player.buffTime[i]--;
                }
            }
        }
    }

    #region Book of List Kin
    internal class BookOfLostKin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Book of Lost Kin");
            Tooltip.SetDefault("Healing allies generates dark bolts to attack enemies\nHealing gives an additional 1 health");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 28;
            Item.rare = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().healBonus += 1;
            player.GetModPlayer<excelPlayer>().lostKin = true;
        }
    }

    public class DarkEnergy : clericHealProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.extraUpdates = 3;

            canDealDamage = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.MediumPurple.ToVector3() * .75f);
            if (++Projectile.ai[0] > 35)
            {
                Vector2 targetPos = Vector2.Zero;
                float targetDistance = 300;
                bool target = false;
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC npc = Main.npc[k];
                    if (Vector2.Distance(Projectile.position, npc.position) < targetDistance && npc.CanBeChasedBy() && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                    {
                        targetDistance = Vector2.Distance(Projectile.position, npc.position);
                        targetPos = npc.Center;
                        target = true;
                    }
                }
                if (target)
                {
                    Vector2 move = targetPos - Projectile.Center;
                    AdjustMagnitude(ref move);
                    // the 16 is now much it 'turns'
                    Projectile.velocity = (16 * Projectile.velocity + move) / 5f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
                HealDistance(Main.LocalPlayer, Main.player[Projectile.owner], 20);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.ShadowFire2>());
            d.velocity = -Projectile.velocity * 0.7f;
            d.noGravity = true;
            d.scale *= 0.8f;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.ShadowFire2>());
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(1.6f, 1.6f);
                d.scale *= Main.rand.NextFloat(0.76f, .9f);
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                // the 2 is the velocity
                vector *= 1.4f / magnitude;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color*.5f, Projectile.rotation, drawOrigin, 0.8f, SpriteEffects.None, 0);
            }

            return true;
        }
    }
    #endregion
}
