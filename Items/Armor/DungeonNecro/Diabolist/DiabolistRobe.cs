using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using excels.Utilities;

namespace excels.Items.Armor.DungeonNecro.Diabolist
{
    [AutoloadEquip(EquipType.Body)]
    internal class DiabolistRobe : ModItem
    {
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Diabolical Jacket");
            Tooltip.SetDefault("7% increased necrotic damage and clerical critical strike chance\nEnemies are more likely to target you");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 26;
            Item.rare = 8;
            Item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.07f;
            modPlayer.clericCrit += 7;
            player.aggro += 400;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<PossessedSkull>() && body.type == ModContent.ItemType<DiabolistRobe>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "All nearby enemies are burned with hellish flames\nThe more enemies nearby, the more potent the flames and your damage become\nPress [Activate Set Bonus] to enable / disable the visuals";

            if (Keybinds.ActivateArmorSet.JustPressed)
            {
                player.GetModPlayer<DiabloistPlayer>().showVisual = !player.GetModPlayer<DiabloistPlayer>().showVisual;
            }
            player.GetModPlayer<DiabloistPlayer>().setActive = true;
        }
    }

    internal class DiabloistPlayer : ModPlayer
    {
        public bool setActive = true;
        public bool showVisual = true;
        int isBig = 0;
        int distance = 300;

        public override void ResetEffects()
        {
            setActive = false;
        }

        public override void PostUpdate()
        {
            if (!setActive)
                return;

            // Check how many enemies nearby
            int amountNearby = 0;
            for (var i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.townNPC && npc.active && !npc.friendly && npc.lifeMax > 5 && npc.type != NPCID.TargetDummy && Vector2.Distance(npc.Center, Player.Center) < distance)
                {
                    amountNearby++;
                    if (npc.boss)
                        amountNearby += 5;
                }
            }

            // Apply debuffs
            for (var i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.townNPC && !npc.friendly && Vector2.Distance(npc.Center, Player.Center) < distance/2)
                {
                    npc.AddBuff(BuffID.OnFire3, 30);
                    if (isBig > 0)
                        npc.AddBuff(BuffID.ShadowFlame, 30);
                }
            }

            if (showVisual)
            {
                // Draw dusts
                for (var i = 0; i < 12; i++)
                {
                    int dust = 6;
                    if (isBig > 0)
                        if (!Main.rand.NextBool(9))
                            dust = 27;

                    Dust d = Dust.NewDustPerfect(Player.Center + Main.rand.NextVector2CircularEdge(distance / 2, distance / 2), dust);
                    d.scale = 2.4f;
                    d.noGravity = true;
                    d.velocity *= 1.8f;
                    if (dust == 27)
                        d.alpha = 180;

                    if (Main.rand.NextBool())
                    {
                        dust = 6;
                        if (isBig > 0)
                            if (!Main.rand.NextBool(4))
                                dust = 27;

                        Dust d2 = Dust.NewDustPerfect(Player.Center + Main.rand.NextVector2Circular(distance / 2, distance / 2), dust);
                        d2.scale = Main.rand.NextFloat(1.2f, 1.5f);
                        d2.noGravity = true;
                        d2.velocity *= Main.rand.NextFloat(0.8f, 1.2f);
                        if (dust == 27)
                            d2.alpha = 180;
                    }
                }
            }

            // Keep it big for a little while longer if enemies become out of range
            isBig--;
            if (amountNearby >= 5)
                isBig = 30;

            amountNearby = Math.Clamp(amountNearby, 0, 20);
            distance = 300 + (amountNearby * 15);
            var modPlayer = ClericClassPlayer.ModPlayer(Player);
            modPlayer.clericNecroticMult += (0.02f * amountNearby);
        }
    }
}
