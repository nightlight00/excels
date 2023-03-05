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
using excels.Buffs.ClericBonus;

namespace excels.Items.Armor.DungeonNecro.Ragged
{
    [AutoloadEquip(EquipType.Body)]
    internal class RaggedRobe : ModItem
    {
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ragged Robe");
            Tooltip.SetDefault(Language.GetTextValue("Mods.excels.ItemDescriptions.Generic.BloodCostReduce", 13));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 32;
            Item.rare = 8;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().bloodCostMult -= 0.13f;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<PossessedSkull>() && body.type == ModContent.ItemType<RaggedRobe>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Damage taken is delayed for 3 seconds\nWhile delayed, become invincible and increase necrotic damage by 25%\nDealing damage while invincible decreases the damage taken and increases invunrability duration";

            player.GetModPlayer<RaggedPlayer>().setActive = true;
        }
    }

    internal class RaggedPlayer : ModPlayer
    {
        public bool setActive = false;

        int damageDealt = 0;
        int damageTaken = 0;

        bool overrideDamage = false;
        int timeSinceDamaged = 0;
        PlayerDeathReason source;

        public override void ResetEffects()
        {
            setActive = false;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (timeSinceDamaged <= 0 || !setActive)
                return;

            if (target.lifeMax > 5 && !target.friendly && !target.townNPC && target.type != NPCID.TargetDummy)
            {
                damageDealt += damage;

                Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<RaggedBuff>())] += damage / 40;
                timeSinceDamaged += damage / 40;
                Player.immuneTime += damage / 40;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (timeSinceDamaged <= 0 || !setActive)
                return;

            if (target.lifeMax > 5 && !target.friendly && !target.townNPC && target.type != NPCID.TargetDummy)
            {
                damageDealt += damage;

                Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<RaggedBuff>())] += damage / 40;
                timeSinceDamaged += damage / 40;
                Player.immuneTime += damage / 40;
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (setActive && !overrideDamage)
            {
                Player.AddBuff(ModContent.BuffType<RaggedBuff>(), 180);
                damageTaken = damage;
                source = damageSource;

                timeSinceDamaged = 181;
                Player.immune = true;
                Player.immuneTime = 180;
                return false;
            }
            overrideDamage = false;
            return true;
        }

        public override void PreUpdate()
        {
            timeSinceDamaged--;
            if (timeSinceDamaged == 0)
            {
                int originalDamageTaken = damageTaken;

                damageTaken -= (int)Math.Floor((decimal)damageDealt / 350);
                if (damageTaken < originalDamageTaken / 2)
                    damageTaken = originalDamageTaken / 2;

                overrideDamage = true;
                Player.Hurt(source, damageTaken, 0);
            }
        }
    }

    internal class RaggedBuff : ClericBonusBuff
    {
        public override void Names()
        {
            BuffName = "Manifested Misery";
            BuffDesc = "Become temporal with greatly increased necrotic damage";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.25f;

            Dust d = Dust.NewDustPerfect(player.Center-new Vector2(0,12), 175);
            d.scale = 2.35f;
            d.velocity = Vector2.Zero;
            d.noGravity = true;
        }
    }
}
