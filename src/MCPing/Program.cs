//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MCPing
{
    using System;
    using Minecraft.Management;

    /// <summary>
    /// The Main program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: \n\tMCPing <host> [port]\nhost - host server name or IP\nport - server port (default 25565)");
                return;
            }

            string host = args[0];
            int port = ServerPingUtility.DefaultPort;
            if (args.Length > 1)
            {
                port = int.Parse(args[1]);
            }

            ServerStatus status = ServerPingUtility.GetStatus(host, port);
            Console.WriteLine("Description: {0}", status.Description);
            Console.WriteLine("Players: {0}/{1}", status.OnlinePlayers, status.MaxPlayers);
            foreach (PlayerInfo player in status.SamplePlayers)
            {
                Console.WriteLine("  {0} ({1})", player.Name, player.Id);
            }

            Console.WriteLine("Minecraft Version: {0}", status.MinecraftVersion);
            Console.WriteLine("Protocol Version: {0}", status.ProtocolVersion);
        }
    }
}
