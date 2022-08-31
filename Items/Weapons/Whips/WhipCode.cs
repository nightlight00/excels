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
using Terraria.GameContent;

namespace excels.Items.Weapons.Whips
{
    #region Chlorophyte
    public class FloralWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Perrenial Bloom");
            Tooltip.SetDefault("15% summon tag damage\nYour summons will focus struck enemies\nFloral power bursts from enemies hit by summons");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // This method quickly sets the whip's properties.
            // Mouse over to see its parameters.
            Item.DefaultToWhip(ModContent.ProjectileType<FloralWhipProj>(), 85, 3.4f, 4, 30);

            Item.shootSpeed = 4;
            Item.rare = ItemRarityID.Lime;
            Item.sellPrice(0, 1, 20);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class FloralWhipProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // This makes the projectile use whip collision detection and allows flasks to be applied to it.
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();

            Projectile.WhipSettings.Segments = 22;
            Projectile.WhipSettings.RangeMultiplier = 1.5f;
        }
        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Whips.PerrenialWhipDebuff>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(damage * 0.9f);
        }

        // This method draws a line between all points of the whip, in case there's empty space between the sprites.
        private void DrawLine(List<Vector2> list)
        {
            //Timer++;

            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.Lime);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);

         //   DrawLine(list);

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.instance.LoadProjectile(Type);
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 pos = list[0];

            for (int i = 0; i < list.Count - 1; i++)
            {
                // These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
                // You can change them if they don't!
                Rectangle frame = new Rectangle(0, 0, 18, 26);
                Vector2 origin = new Vector2(5, 8);
                float scale = 1;

                // These statements determine what part of the spritesheet to draw for the current segment.
                // They can also be changed to suit your sprite.
                if (i == list.Count - 2)
                {
                    frame.Y = 26 * 4;

                    // For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Timer / timeToFlyOut;
                    scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i % 4 == 3)
                {
                    frame.Y = 26 * 3;
                }
                else if (i % 4 == 0 || i % 4 == 2)
                {
                    frame.Y = 26 * 2;
                }
                else if (i % 4 == 1)
                {
                    frame.Y = 26;
                }

                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

                pos += diff;
            }
            return false;
        }
    
    }
    
    public class FloralPower : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FlowerPow}";

        public override void SetDefaults()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 20;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = Projectile.height = 40;
            Projectile.alpha = 255;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation = MathHelper.ToRadians(Main.rand.Next(360));
                for (var p = 0; p < 5; p++)
                {
                    for (var i = -1; i < 1; i+=2)
                    {
                        for (var s = 0; s < 50; s++)
                        {
                            Vector2 pos = new Vector2(0.5f * ((s+1) / 5)).RotatedBy(Projectile.rotation + MathHelper.ToRadians(((360 / 5) * p) + (20*s*i)));
                            Dust d = Dust.NewDustPerfect(Projectile.Center + pos, 177);
                            d.noGravity = true;
                          //  d.noLight = true;
                            d.velocity = pos * 1.1f;
                            d.scale = 1.5f + ((s+1) / 20);
                        }
                    }
                }


                Projectile.ai[0]++;
            }
        }
    }

    #endregion

    /*
    #region Obsidian
    internal class ObsidianLash : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SummonerWeaponThatScalesWithAttackSpeed[Type] = true; // This makes the item be affected by the user's melee speed.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BoneWhip);
            Item.useTime = Item.useAnimation = 40;
            Item.knockBack = 6;
            Item.damage = 36;
            Item.shoot = ModContent.ProjectileType<ObsidianLashP>();
        }
    }

    public class ObsidianLashP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BoneWhip);
            AIType = ProjectileID.BoneWhip;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true; // This makes the projectile use whip collision detection and allows flasks to be applied to it.
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
        }

        // This method draws a line between all points of the whip, in case there's empty space between the sprites.
        private void DrawLine(SpriteBatch spriteBatch, List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;

            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.White);
                Vector2 scale = new Vector2(1f, (diff.Length() + 2) / frame.Height);

                spriteBatch.Draw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0f);

                pos += diff;
            }
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch;

            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);

            DrawLine(spriteBatch, list);

            // If you don't want to use custom code, you can instead call one of vanilla's DrawWhip methods. However, keep in mind that you must adhere to how they draw if you do.
            // Main.DrawWhip_WhipBland(Projectile, list);	SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 pos = list[0];

            // The frame and origin values in this loop were chosen to suit this projectile's sprite, but won't necessarily work for your own. Feel free to change them if they don't!
            for (int i = 0; i < list.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 10, 26);
                Vector2 origin = new Vector2(5, 8);
                float scale = 1;

                // These statements determine what part of the spritesheet to draw for the current segment, and can also be changed.
                if (i == list.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 18;

                    // To make it look more impactful, this scales the tip of the whip up when fully extended, and down when curled up.
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Timer / timeToFlyOut;
                    scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 10)
                {
                    frame.Y = 58;
                    frame.Height = 16;
                }
                else if (i > 5)
                {
                    frame.Y = 42;
                    frame.Height = 16;
                }
                else if (i > 0)
                {
                    frame.Y = 26;
                    frame.Height = 16;
                }

                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is again used to correct rotation.
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                spriteBatch.Draw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

                pos += diff;
            }
            return false;
        }
    }
    #endregion
    */

}

