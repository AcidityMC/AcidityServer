using AcidityServer.Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AcidityServer.Server.Entity
{
    public class EntityPlayer
    {
        public string Username { get; private set; }
        public TcpClient Client { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public bool OnGround { get; private set; }

        private NetworkStream stream;

        public EntityPlayer(string username, TcpClient client)
        {
            Username = username;
            Client = client;
            stream = client.GetStream();
            X = Y = Z = 0.0;
            Yaw = Pitch = 0.0f;
            OnGround = true;
        }

        public void UpdatePosition(double x, double y, double z, float yaw, float pitch, bool onGround)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
            // Additional logic such as collision detection, movement validation, etc.
        }

        public async Task SendPacketAsync(Packet packet)
        {
            try
            {
                byte[] data = packet.ToBytes();
                byte[] lengthPrefix = BitConverter.GetBytes(data.Length);
                await stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
                await stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Player {Username} SendPacketAsync Error: {ex.Message}");
                // Handle disconnection or packet sending failure
            }
        }

        public async Task SendMessageAsync(string message)
        {
            // Implement sending a chat message to the client
            // Define a ChatPacket similar to other packet types
            // For example:
            // ChatPacket chatPacket = new ChatPacket { Message = message };
            // await SendPacketAsync(chatPacket);
        }
    }
}
