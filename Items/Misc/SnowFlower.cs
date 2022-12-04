using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader.IO;

namespace excels.Items.Misc
{
    internal class SnowFlower : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Blossom");
            Tooltip.SetDefault("The sigil of Niflheim's followers\nPermanantly increases max radiance by 20");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 22;
            Item.rare = 5;
            Item.consumable = true;
            Item.sellPrice(gold: 5);
            Item.maxStack = 999;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.UseSound = new SoundStyle($"{nameof(excels)}/Audio/SnowFlowerSound");
        }

        public override bool? UseItem(Player player)
        {
            if (player.GetModPlayer<SnowFlowerPlayer>().SnowFlowerConsumed)
                return null;

            player.GetModPlayer<SnowFlowerPlayer>().SnowFlowerConsumed = true;
            player.GetModPlayer<ClericClassPlayer>().radianceStatMax2 += 20;
            player.GetModPlayer<ClericClassPlayer>().radianceStatCurrent += 20;
            CombatText.NewText(player.getRect(), new Color(104, 171, 251), 20);

            return base.UseItem(player);
        }
    }

    internal class SnowFlowerPlayer : ModPlayer
    {
        public bool SnowFlowerConsumed = false;

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)excels.MessageType.SnowFlowerPlayerSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write((bool)SnowFlowerConsumed);
            packet.Send(toWho, fromWho);
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            SnowFlowerConsumed = reader.ReadBoolean();
        }

        public override void clientClone(ModPlayer clientClone)
        {
            SnowFlowerPlayer clone = clientClone as SnowFlowerPlayer;
            clone.SnowFlowerConsumed = SnowFlowerConsumed;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            SnowFlowerPlayer clone = clientPlayer as SnowFlowerPlayer;

            if (SnowFlowerConsumed != clone.SnowFlowerConsumed)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }

        public override void SaveData(TagCompound tag)
        {
            tag["snowFlower"] = SnowFlowerConsumed;
        }

        public override void LoadData(TagCompound tag)
        {
            SnowFlowerConsumed = tag.GetBool("snowFlower");
        }
    }
}
