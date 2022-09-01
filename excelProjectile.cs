using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;
using System.IO;

namespace excels
{
    internal class excelProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public int healStrength = -1;
        public float healRate = 1;

        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.Flames)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 10;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.type == ProjectileID.Flames && Main.player[projectile.owner].GetModPlayer<excelPlayer>().FireBadge)
            {
                projectile.penetrate--;
                target.AddBuff(BuffID.Oiled, 600);
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.type == ProjectileID.Shroomerang)
            {
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 150);
            }
            if (Main.player[projectile.owner].GetModPlayer<excelPlayer>().NiflheimAcc)
            {
                target.AddBuff(BuffID.Frostburn, damage * 40);
            }
        }


        public override void OnHitPvp(Projectile projectile, Player target, int damage, bool crit)
        {
            if (projectile.type == ProjectileID.Shroomerang)
            {
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Mycosis>(), 150);
            }
            if (Main.player[projectile.owner].GetModPlayer<excelPlayer>().NiflheimAcc)
            {
                target.AddBuff(BuffID.Frostburn, damage * 40);
            }
        }


        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            //if (projectile.type == ProjectileID.
        }

        public override void AI(Projectile projectile)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (Main.player[projectile.owner].GetModPlayer<excelPlayer>().NiflheimAcc && projectile.friendly && projectile.damage > 0)
            {
                if (Main.rand.NextBool(3))
                {
                    Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 92);
                    d.scale = projectile.scale;
                    d.velocity = projectile.velocity * Main.rand.NextFloat(0.2f, 0.4f);
                    d.noLight = true;
                    d.noGravity = true;
                }
            }

            switch (projectile.type)
            {
                case ProjectileID.PurificationPowder:
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        if (Main.npc[i].active && Main.npc[i].type == NPCID.Nymph && projectile.Hitbox.Intersects(Main.npc[i].Hitbox))
                        {
                            Main.npc[i].Transform(ModContent.NPCType<NPCs.Town.Geologist>());
                            // this bool will be used to allow the geologist to move in without purifiying another nymph
                            if (!excelWorld.transformedNymph)
                            {
                                excelWorld.transformedNymph = true;
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                                }
                            }
                            NPC.SetEventFlagCleared(ref excelWorld.transformedNymph, -1);
                        }
                    }

                    List<Point> til = Collision.GetTilesIn(projectile.TopLeft, projectile.BottomRight);
                    for (var i = 0; i < til.Count; i++)
                    {
                        if (Main.tile[til[i]].TileType == TileID.Demonite || Main.tile[til[i]].TileType == TileID.Crimtane)
                            Main.tile[til[i]].TileType = (ushort)ModContent.TileType<Tiles.OresBars.PurityOre>();

                        if (Main.tile[til[i]].TileType == TileID.MetalBars && (Main.tile[til[i]].TileFrameX == 8 * 18 || Main.tile[til[i]].TileFrameX == 19 * 18))
                        {
                            Main.tile[til[i]].TileType = (ushort)ModContent.TileType<Tiles.OresBars.ExcelBarTiles>();
                            Main.tile[til[i]].TileFrameX = 2 * 18;
                        }
                    }
                    break;

                case ProjectileID.VilePowder:
                    List<Point> til1 = Collision.GetTilesIn(projectile.TopLeft, projectile.BottomRight);
                    for (var i = 0; i < til1.Count; i++)
                    {
                        if (Main.tile[til1[i]].TileType == ModContent.TileType<Tiles.OresBars.PurityOre>())
                            Main.tile[til1[i]].TileType = TileID.Demonite;

                        if (Main.tile[til1[i]].TileType == ModContent.TileType<Tiles.OresBars.ExcelBarTiles>() && Main.tile[til1[i]].TileFrameX == 2 * 18)
                        {
                            Main.tile[til1[i]].TileType = TileID.MetalBars;
                            Main.tile[til1[i]].TileFrameX = 8 * 18;
                        }
                    }
                    break;

                case ProjectileID.ViciousPowder:
                    List<Point> til2 = Collision.GetTilesIn(projectile.TopLeft, projectile.BottomRight);
                    for (var i = 0; i < til2.Count; i++)
                    {
                        if (Main.tile[til2[i]].TileType == ModContent.TileType<Tiles.OresBars.PurityOre>())
                            Main.tile[til2[i]].TileType = TileID.Crimtane;

                        if (Main.tile[til2[i]].TileType == ModContent.TileType<Tiles.OresBars.ExcelBarTiles>() && Main.tile[til2[i]].TileFrameX == 2 * 18)
                        {
                            Main.tile[til2[i]].TileType = TileID.MetalBars;
                            Main.tile[til2[i]].TileFrameX = 19 * 18;
                        }
                    }
                    break;
            }

        }
    }


    public abstract class FlamethrowerProjectile : ModProjectile
    {

        public bool WaterKills = true;

        public virtual void HitCheck(Player player, NPC target, int time = 600)
        {
            if (player.GetModPlayer<excelPlayer>().FireBadge) { 
                target.AddBuff(BuffID.Oiled, time);

                Projectile.penetrate--;
                if (Projectile.penetrate == 0 || Projectile.penetrate == -1)
                    Projectile.Kill();
            }
        }

        public int DustType = 6;
        public int DebuffType = BuffID.OnFire;
        public bool Start = false;

        public override bool PreAI()
        {
            if (!Start)
            {
                switch (Projectile.frame)
                {
                    case 0:
                        DustType = 6;
                        DebuffType = BuffID.OnFire;
                        break;
                    case 1:
                        DustType = ModContent.DustType<Dusts.ShadowFire2>();
                        DebuffType = BuffID.ShadowFlame;
                        break;
                    case 2:
                        DustType = 92;
                        DebuffType = BuffID.Frostburn;
                        break;
                }
                Start = true;
            }

            if (Projectile.wet && !Projectile.lavaWet && WaterKills)
                Projectile.Kill();
            return true;
        }
    }
}
