using System.IO;
using Terraria;
using Terraria.ID;
using excels.Items.Misc;

namespace excels
{
    partial class excels
    {
        internal enum MessageType : byte
        {
            SnowFlowerPlayerSync
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                // This message syncs ExampleStatIncreasePlayer.exampleLifeFruits and ExampleStatIncreasePlayer.exampleManaCrystals
                case MessageType.SnowFlowerPlayerSync:
                    byte playernumber = reader.ReadByte();
                    SnowFlowerPlayer snowPlayer = Main.player[playernumber].GetModPlayer<SnowFlowerPlayer>();
                    snowPlayer.ReceivePlayerSync(reader);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        // Forward the changes to the other clients
                        snowPlayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
            }
        }
    }
    }
