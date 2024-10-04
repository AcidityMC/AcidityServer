using AcidityServer.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AcidityServer.Server.Network
{
    public class PacketHandler
    {
        private TcpClient client;
        private NetworkStream stream;
        private EntityPlayer player;
        private MinecraftServer server;

        public PacketHandler(TcpClient client, MinecraftServer server)
        {
            this.client = client;
            this.stream = client.GetStream();
            this.server = server;
        }

        public async Task HandleClientAsync()
        {
            try
            {
                while (client.Connected)
                {
                    // Assuming packets are prefixed with their length (for simplicity)
                    byte[] lengthBytes = new byte[4];
                    int bytesRead = await stream.ReadAsync(lengthBytes, 0, 4);
                    if (bytesRead == 0)
                        break; // Client disconnected

                    int packetLength = BitConverter.ToInt32(lengthBytes, 0);
                    byte[] packetData = new byte[packetLength];
                    bytesRead = 0;
                    while (bytesRead < packetLength)
                    {
                        int read = await stream.ReadAsync(packetData, bytesRead, packetLength - bytesRead);
                        if (read == 0)
                            break;
                        bytesRead += read;
                    }

                    if (bytesRead == 0)
                        break; // Client disconnected

                    Packet packet = Packet.FromBytes(packetData);
                    if (packet != null)
                    {
                        await ProcessPacketAsync(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PacketHandler Error: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        private async Task ProcessPacketAsync(Packet packet)
        {
            switch (packet.PacketId)
            {
                case 0: // Handshake
                    HandleHandshake(packet as HandshakePacket);
                    break;
                case 1: // Login
                    await HandleLogin(packet as LoginPacket);
                    break;
                case 2: // Movement
                    HandleMovement(packet as MovementPacket);
                    break;
                // Handle other packet types here
                default:
                    Console.WriteLine($"Unknown Packet ID: {packet.PacketId}");
                    break;
            }
        }

        private void HandleHandshake(HandshakePacket packet)
        {
            Console.WriteLine($"Handshake received: ProtocolVersion={packet.ProtocolVersion}, Username={packet.ServerAddress}");
            // Process handshake (e.g., validate protocol version, prepare for login)
        }

        private async Task HandleLogin(LoginPacket packet)
        {
            Console.WriteLine($"Login received: Username={packet.Username}");
            // Create a new player and add to the server
            player = new Player(packet.Username, client);
            server.AddPlayer(player);

            // Send acknowledgment or spawn packets here
            // For example, send a welcome message
            string welcomeMessage = $"Welcome, {player.Username}!";
            await SendMessageAsync(welcomeMessage);
        }

        private void HandleMovement(MovementPacket packet)
        {
            if (player != null)
            {
                player.UpdatePosition(packet.X, packet.Y, packet.Z, packet.Yaw, packet.Pitch, packet.OnGround);
                Console.WriteLine($"Player {player.Username} moved to ({packet.X}, {packet.Y}, {packet.Z})");
                // Update world state, notify other players, etc.
            }
        }

        private async Task SendPacketAsync(Packet packet)
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
                Console.WriteLine($"SendPacketAsync Error: {ex.Message}");
                Disconnect();
            }
        }

        private void Disconnect()
        {
            if (player != null)
            {
                server.RemovePlayer(player);
                Console.WriteLine($"Player {player.Username} disconnected.");
            }
            client.Close();
        }
    }
}
