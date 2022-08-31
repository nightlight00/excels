using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using IL.Terraria.Graphics.Shaders;
using Terraria.GameContent;

namespace excels.Items.Accessories.Banner
{
    internal class FireBanner : ModItem
    {
        //public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FlamingJack}";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fiendish Battle Standard");
            Tooltip.SetDefault("Minions are surrounded by a fiery aura");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().SummonBanner = 1;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            return player.GetModPlayer<excelPlayer>().SummonBanner == 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    internal class VenomBanner : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrosive Battle Standard");
            Tooltip.SetDefault("Minions are surrounded by a venomous aura");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.rare = 7;
            Item.accessory = true;
            Item.buyPrice(0, 25);
            Item.sellPrice(0, 5);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().SummonBanner = 2;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            return player.GetModPlayer<excelPlayer>().SummonBanner == 0;
        }
    }

    internal class ShadowflameBanner : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Battle Standard");
            Tooltip.SetDefault("Minions are surrounded by a shadowfire aura");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.rare = 5;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().SummonBanner = 3;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            return player.GetModPlayer<excelPlayer>().SummonBanner == 0;
        }
    }

    internal class RegenBanner : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inspirtational Banner");
            Tooltip.SetDefault("Minions are surrounded by a regenerative aura \n'They love you, They love you not!'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.rare = 1;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().SummonBanner = 4;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            return player.GetModPlayer<excelPlayer>().SummonBanner == 0;
        }
    }

    public class BannerEffect : GlobalProjectile
    {
        public override bool InstancePerEntity => true;


        float featherRotation = 0;

        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].GetModPlayer<excelPlayer>().SummonBanner > 0 && (projectile.minionSlots > 0 || projectile.sentry))
            {
                float Distance = 64 + ((projectile.height + projectile.width) / 2);
                int PartAmount = 12;

                bool Evil = true;
                int DebuffType = BuffID.OnFire;
                int DustType = 6;
                float Mult = 1;
                // do a switch event here if anymore are added
                switch (Main.player[projectile.owner].GetModPlayer<excelPlayer>().SummonBanner)
                {
                    case 2:
                        DebuffType = BuffID.Venom;
                        DustType = 171;
                        PartAmount = 14;
                        Mult = 1.2f;
                        break;
                    case 3:
                        DebuffType = BuffID.ShadowFlame;
                        DustType = 27;
                        break;
                    case 4:
                        Evil = false;
                        DebuffType = BuffID.Regeneration;
                        DustType = 90;
                        Mult = 1.5f;
                        break;
                }

                if (!Evil)
                {
                    Distance *= Mult;
                    PartAmount = 18;
                }

                // particles
                for (var i = 0; i < PartAmount; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                    Dust d = Dust.NewDustPerfect(projectile.Center + (speed * Distance), DustType, Vector2.Zero); //204
                    d.noGravity = true;
                    d.noLight = true;
                    switch (DustType) {
                        case 6: d.scale = 1.1f; break;
                        case 171: d.fadeIn = 1.1f; d.alpha = 90; break;
                        case 27: d.scale = 1.15f; d.noLight = false; break;
                        case 90: d.scale = 1.1f; d.noLight = false; break;
                    } 
                }

                if (Evil)
                {
                    // add debuff to nearby enemies
                    float targetDist = Distance;
                    for (int k = 0; k < 200; k++)
                    {
                        NPC npc = Main.npc[k];
                        if (npc.lifeMax > 5 && !npc.friendly)
                        {
                            float distance = Vector2.Distance(npc.Center, projectile.Center);
                            if ((distance < targetDist) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                            {
                                npc.AddBuff(DebuffType, 60);
                            }
                        }

                    }
                }
                else
                {
                    // add buff to nearby players
                    float targetDist = Distance;
                    for (int k = 0; k < 200; k++)
                    {
                        Player player = Main.player[k];

                        float distance = Vector2.Distance(player.Center, projectile.Center);
                        if ((distance < targetDist) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, player.position, player.width, player.height))
                        {
                            player.AddBuff(DebuffType, 600);
                        }
                    }
                }
            }
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            Main.instance.LoadProjectile(projectile.type);
            
            // purely visual
            if (Main.player[projectile.owner].GetModPlayer<excelPlayer>().AvianSet && (projectile.minionSlots > 0 || projectile.sentry))
            {
                featherRotation += 0.055f;

                Texture2D texture = TextureAssets.Projectile[ProjectileID.HarpyFeather].Value;
                Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                Vector2 drawOrigin = sourceRectangle.Size() / 2f;
                Color drawColor = projectile.GetAlpha(lightColor);


                Vector2 drawPos = new Vector2(MathF.Cos(featherRotation) * 64, MathF.Sin(featherRotation) * 64);
                Vector2 drawPos2 = new Vector2(MathF.Cos(featherRotation + MathHelper.ToRadians(120)) * 64, MathF.Sin(featherRotation + MathHelper.ToRadians(120)) * 64);
                Vector2 drawPos3 = new Vector2(MathF.Cos(featherRotation + MathHelper.ToRadians(240)) * 64, MathF.Sin(featherRotation + MathHelper.ToRadians(240)) * 64);
                
                Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition + drawPos, sourceRectangle, 
                    drawColor, drawPos.ToRotation(), drawOrigin, projectile.scale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition + drawPos2, sourceRectangle, drawColor,
                     drawPos2.ToRotation(), drawOrigin, projectile.scale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition + drawPos3, sourceRectangle, drawColor,
                       drawPos3.ToRotation(), drawOrigin, projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
