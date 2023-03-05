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

namespace excels.Items.Armor.DungeonNecro.Necromancer
{
    [AutoloadEquip(EquipType.Body)]
    internal class NecromancerRobe : ModItem
    {
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Necromancer's Cloak");
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.NecroticDamage", 10));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 26;
            Item.width = 30;
            Item.rare = 8;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.1f;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<PossessedSkull>() && body.type == ModContent.ItemType<NecromancerRobe>();
        }

        int summonSkull = 0;
        bool setActive = false;

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Generate Ghastly Skulls overtime to attack enemies\nDrains 8 health for each one created\nPress [Activate Set Bonus] to activate / deactivate this effect";

            if (Keybinds.ActivateArmorSet.JustPressed)
            {
                setActive = !setActive;
                if (setActive)
                    CombatText.NewText(player.getRect(), Color.Crimson, "Raising the Dead");
                else
                    CombatText.NewText(player.getRect(), Color.Crimson, "'Til Next Dawn");
            }

            if (setActive)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<SetBonus.GhastlySkull>()] < 4 && player.statLife > 8)
                {
                    summonSkull++;
                    if (summonSkull > 30)
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit3, player.Center);
                        player.statLife -= 8;
                        CombatText.NewText(player.getRect(), Color.Red, 8);
                        Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Main.rand.NextVector2CircularEdge(2, 2), ModContent.ProjectileType<SetBonus.GhastlySkull>(), 180, 2, player.whoAmI);
                        summonSkull = 0;
                    }
                }
            }
        }
    }
}
