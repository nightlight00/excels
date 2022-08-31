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

namespace excels.Items.Weapons.Sword1
{
    internal class BerserkerBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting foes builds up your rage which boosts melee damage and speed \nGetting hit resets rage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        int BerserkerStrength = 0;
        int LastHealth = 0;

        public override void SetDefaults()
        {
            Item.width = Item.height = 40;
            Item.DamageType = DamageClass.Melee;
            Item.rare = 6;
            Item.knockBack = 4f;
            Item.damage = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useTime = Item.useAnimation = 26;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.sellPrice(0, 0, 60);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            BerserkerStrength++;
            if (BerserkerStrength > 30)
            {
                BerserkerStrength = 30;
            }
        }

        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
            BerserkerStrength++;
            if (BerserkerStrength > 30)
            {
                BerserkerStrength = 30;
            }
            //   UpdateBerserk();
        }

        // ends with damage of x2.5 (100), speed multiplier of 1.75, scale of 1.5

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage += (BerserkerStrength * 0.05f);
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return 1 + (BerserkerStrength * 0.025f);
        }

        public override void UpdateInventory(Player player)
        {
            Item.scale = 1 + (BerserkerStrength / 45);
            if (LastHealth > player.statLife)
            {
                BerserkerStrength = 0;
            }
            LastHealth = player.statLife;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 40)
                .AddIngredient(ItemID.FossilOre, 12)
                .AddIngredient(ItemID.PlatinumBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                   .AddIngredient(ItemID.Bone, 40)
                   .AddIngredient(ItemID.FossilOre, 12)
                   .AddIngredient(ItemID.GoldBar, 8)
                   .AddTile(TileID.Anvils)
                   .Register();
        }
    }

    public class CatsClaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cat's Claw");
            Tooltip.SetDefault("No catgirls?\n" +
                        "⣞⢽⢪⢣⢣⢣⢫⡺⡵⣝⡮⣗⢷⢽⢽⢽⣮⡷⡽⣜⣜⢮⢺⣜⢷⢽⢝⡽⣝\n"+
                        "⠸⡸⠜⠕⠕⠁⢁⢇⢏⢽⢺⣪⡳⡝⣎⣏⢯⢞⡿⣟⣷⣳⢯⡷⣽⢽⢯⣳⣫⠇\n" +
                        "⠀⠀⢀⢀⢄⢬⢪⡪⡎⣆⡈⠚⠜⠕⠇⠗⠝⢕⢯⢫⣞⣯⣿⣻⡽⣏⢗⣗⠏⠀\n" +
                        "⠀⠪⡪⡪⣪⢪⢺⢸⢢⢓⢆⢤⢀⠀⠀⠀⠀⠈⢊⢞⡾⣿⡯⣏⢮⠷⠁⠀⠀\n" +
                        "⠀⠀⠀⠈⠊⠆⡃⠕⢕⢇⢇⢇⢇⢇⢏⢎⢎⢆⢄⠀⢑⣽⣿⢝⠲⠉⠀⠀⠀⠀\n" +
                        "⠀⠀⠀  ⡿⠂⠠⠀⡇⢇⠕⢈⣀⠀⠁⠡⠣⡣⡫⣂⣿⠯⢪⠰⠂⠀⠀⠀⠀\n" +
                        " ⠀⠀ ⠀⡦⡙⡂⢀⢤⢣⠣⡈⣾⡃⠠⠄⠀⡄⢱⣌⣶⢏⢊⠂⠀⠀⠀⠀⠀⠀\n" +
                        "⠀⠀⠀⠀⢝⡲⣜⡮⡏⢎⢌⢂⠙⠢⠐⢀⢘⢵⣽⣿⡿⠁⠁⠀⠀⠀⠀⠀⠀⠀\n" +
                        "⠀⠀⠀⠀⠨⣺⡺⡕⡕⡱⡑⡆⡕⡅⡕⡜⡼⢽⡻⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\n" +
                        "⠀⠀⠀⠀⣼⣳⣫⣾⣵⣗⡵⡱⡡⢣⢑⢕⢜⢕⡝⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\n" +
                        "⠀⠀⠀⣴⣿⣾⣿⣿⣿⡿⡽⡑⢌⠪⡢⡣⣣⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\n" +
                        "⠀⠀⠀⡟⡾⣿⢿⢿⢵⣽⣾⣼⣘⢸⢸⣞⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\n"+
                        "⠀⠀⠀⠀⠁⠇⠡⠩⡫⢿⣝⡻⡮⣒⢽⠋⠀⠀⠀");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.damage = 75;
            Item.knockBack = 23;
            Item.crit = 50;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.value = 1000;
            Item.rare = ItemRarityID.Cyan;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Item57); 
        }
    }
}
