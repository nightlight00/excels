using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;

namespace excels.Items.Armor.Heartbeat
{
    [AutoloadEquip(EquipType.Head)]
    internal class HeartbeatMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.HeartbeatHeadPiece"));
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.ClericCritChance", 5));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 32;
            Item.rare = 3;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericCrit += 5;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HeartbeatCloak>() && legs.type == ModContent.ItemType<HeartbeatGreaves>();
        }

        int timer = 0;
        bool heartbreak = false;
        float heartStrength = 0;

        public virtual string DeathMessage()
        {
            switch (Main.rand.Next(4))
            {
                default:
                    return Language.GetTextValue("Mods.excels.DeathMessages.HeartBreak.1", Main.player[Item.whoAmI].name); // "Is your heart broken?";
                case 1:
                    return Language.GetTextValue("Mods.excels.DeathMessages.HeartBreak.2", Main.player[Item.whoAmI].name);
                case 2:
                    return Language.GetTextValue("Mods.excels.DeathMessages.HeartBreak.3", Main.player[Item.whoAmI].name);
                case 3:
                    return Language.GetTextValue("Mods.excels.DeathMessages.HeartBreak.4", Main.player[Item.whoAmI].name);
            }
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            player.setBonus = Language.GetTextValue("Mods.excels.ItemDescriptions.ArmorSetBonus.HeartbeatSet");

            player.GetModPlayer<excelPlayer>().HeartbeatSet = true;
            if (player.controlDown && player.releaseDown && player.doubleTapCardinalTimer[0] < 15)
            {
                heartStrength = 0;
                heartbreak = !heartbreak;
                if (heartbreak)
                {
                    CombatText.NewText(player.getRect(), Color.Crimson, Language.GetTextValue("Mods.excels.Common.Heartbreak"));
                }
            }
            if (heartbreak)
            {
                if (player.statLife <= player.statLifeMax2 * 0.25f)
                {
                    player.AddBuff(BuffID.Panic, 2, true);
                }
                timer++;
                modPlayer.clericNecroticMult += heartStrength;
                if (timer == 3 || timer == 9)
                {
                    player.statLife--;
                    if (Main.hardMode) { player.statLife--; }
                    if (player.statLife <= 0) { heartbreak = false; player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(DeathMessage()), 0, 0); }
                }
                if (timer >= 12)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X, player.Center.Y + (player.height / 2)), 271, speed * 6, Scale: 1.25f);
                        d.noGravity = true;
                    }
                   // if (heartStrength < 0.25f) { heartStrength += 0.025f; }
                    timer = 0;
                }
                if (timer % 4 == 0){
                    if (heartStrength < 0.25f) { heartStrength += 0.025f / 4; }
                }
            }
            else
            {
                player.lifeRegen += 2;
            }
        }
        public override void ArmorSetShadows(Player player)
        {
            if (player.GetModPlayer<excelPlayer>().HeartbeatSet)
            {
                player.armorEffectDrawOutlines = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ShatteredHeartbeat>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class HeartbeatCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.HeartbeatChestPiece"));
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.NecroticDamage", 5));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 22;
            Item.width = 24;
            Item.rare = 3;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.05f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ShatteredHeartbeat>(), 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class HeartbeatGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.excels.ItemNames.HearbeatLegPiece"));
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.NecroticDamage", 4));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 22;
            Item.width = 24;
            Item.rare = 3;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.04f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ShatteredHeartbeat>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
