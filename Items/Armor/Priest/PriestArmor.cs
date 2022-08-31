using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace excels.Items.Armor.Priest
{
    [AutoloadEquip(EquipType.Head)]
    internal class PriestHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priest's Hood");
            Tooltip.SetDefault("5% increased radiant damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 32;
            Item.rare = 2;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericRadiantMult += 0.05f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PriestChest>() && legs.type == ModContent.ItemType<PriestBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            player.setBonus = "Creates a radiant aura around you that heals all nearby allies \nIncreases radiance regeneration";

            modPlayer.radianceRegenRate -= 0.2f;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PriestsAura>()] == 0)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<PriestsAura>(), 0, 0, player.whoAmI);
            }

            // player.GetModPlayer<excelPlayer>().healBonus += 1;
            player.GetModPlayer<excelPlayer>().PriestSet = true;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class PriestChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priest's Robes");
            Tooltip.SetDefault("Healing gives an extra 1 health \nNecrotic blood cost reduced by 10%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 22;
            Item.width = 24;
            Item.rare = 2;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<excelPlayer>().healBonus += 1;
            player.GetModPlayer<excelPlayer>().bloodCostMult -= 0.1f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class PriestBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priest's Boots");
            Tooltip.SetDefault("5% increased necrotic damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 22;
            Item.width = 24;
            Item.rare = 2;
            Item.defense = 2;
        }
        
        public override void UpdateEquip(Player player)
        {
            var modPlayer = ClericClassPlayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.05f;
        }
    }

    public class PriestsAura : clericHealProj
    {
        public override string Texture => "excels/Items/WeaponHeal/Generic/HealingBolt";
        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.alpha = 255;

            healPenetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.timeLeft = 2;
            Projectile.Center = player.Center;
            if (!player.GetModPlayer<excelPlayer>().PriestSet)
                Projectile.Kill();

            for (var i = 0; i < 6; i++)
            {
                Dust d = Dust.NewDustPerfect(player.Center + new Vector2(0, -196).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] + ((360 / 6) * i))), 204);
                d.noGravity = true;
                d.scale = 1.7f;
                d.velocity = Vector2.Zero;

                Dust d2 = Dust.NewDustPerfect(player.Center + new Vector2(0, -196).RotatedBy(MathHelper.ToRadians(-Projectile.ai[0] + ((360 / 6) * i))), 204);
                d2.noGravity = true;
                d2.scale = 1.7f;
                d2.velocity = Vector2.Zero;
            }
            Projectile.ai[0] += 1.7f;

            // using custom heal code so it doesnt interfere with the list thing
            if (++Projectile.ai[1] % 50 == 0) 
            {
                for (var i = 0; i < Main.maxPlayers; i++)
                {
                    Player p2 = Main.player[i];
                    if (p2.statLife < p2.statLifeMax2 && Vector2.Distance(p2.Center, Projectile.Center) < 204 && p2 != player)
                    {
                        int heal = 1 + (player.GetModPlayer<excelPlayer>().healBonus / 2);
                        p2.statLife += heal;
                        p2.HealEffect(heal);
                        if (p2.statLife > p2.statLifeMax2)
                        {
                            p2.statLife = p2.statLifeMax2;
                        }
                    }
                }
                for (var i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (n.townNPC && n.life < n.lifeMax && Vector2.Distance(n.Center, Projectile.Center) < 204)
                    {
                        int heal = 1 + (player.GetModPlayer<excelPlayer>().healBonus / 2);
                        n.life += heal;
                        n.HealEffect(heal);
                        if (n.life > n.lifeMax)
                        {
                            n.life = n.lifeMax;
                        }
                    }
                }
            }
        }
    }
}
