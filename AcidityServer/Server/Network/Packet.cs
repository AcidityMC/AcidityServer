using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcidityServer.Server.Network
{
    public abstract class Packet
    {
        public abstract int PacketId { get; }

        public abstract void Read(BinaryReader reader);
        public abstract void Write(BinaryWriter writer);

        public byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8))
            {
                writer.Write(PacketId);
                Write(writer);
                return ms.ToArray();
            }
        }

        public static Packet FromBytes(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8))
            {
                if (reader.BaseStream.Length < 4)
                    return null;

                int packetId = reader.ReadInt32();
                Packet packet = PacketFactory.CreatePacket(packetId);
                if (packet != null)
                {
                    packet.Read(reader);
                }
                return packet;
            }
        }
    }

    public static class PacketFactory
    {
        public static Packet CreatePacket(int packetId)
        {
            switch (packetId)
            {
                case 0: return new HandshakePacket();
                case 1: return new LoginPacket();
                case 2: return new MovementPacket();
                // Add other packet types here
                default: return null;
            }
        }
    }
}
