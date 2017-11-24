//-----------------------------------------------------------------------
// <copyright file="ServerPingUtility.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Minecraft.Management
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// Pings a Minecraft server for status.
    /// </summary>
    public static class ServerPingUtility
    {
        /// <summary>
        /// The default port.
        /// </summary>
        public const int DefaultPort = 25565;

        /// <summary>
        /// The protocol version.
        /// </summary>
        private const int ProtocolVersion = 47;

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns>A new instance of the <see cref="ServerStatus"/> class.</returns>
        public static ServerStatus GetStatus(
            string host,
            int port = DefaultPort)
        {
            Task<ServerStatus> task = GetStatusAsync(host, port);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Gets the status asynchronously.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns>An awaitable task with the result.</returns>
        public static Task<ServerStatus> GetStatusAsync(
            string host,
            int port = DefaultPort)
        {
            return GetStatusAsync(host, port, CancellationToken.None);
        }

        /// <summary>
        /// Gets the status asynchronous.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task with the result.</returns>
        public static async Task<ServerStatus> GetStatusAsync(
            string host,
            int port,
            CancellationToken cancellationToken)
        { 
            using (Socket sock = new Socket(SocketType.Stream, ProtocolType.Tcp))
            {
                await sock.ConnectAsync(host, port);
                cancellationToken.ThrowIfCancellationRequested();
                await sock.SendAsync(CreateHandshakeRequest(host, port));
                cancellationToken.ThrowIfCancellationRequested();
                await sock.SendAsync(CreateStatusRequest());

                int packetSize = await ReadVariableInt32Async(sock, cancellationToken);
                byte[] data = new byte[packetSize];

                cancellationToken.ThrowIfCancellationRequested();
                int byteCount = await sock.ReceiveAsync(data);
                if (byteCount < packetSize)
                {
                    throw new IOException("Expected more data.");
                }

                using (MemoryStream ms = new MemoryStream(data, 0, packetSize))
                {
                    int responseCode = ReadVariableInt32(ms);
                    if (responseCode != 0)
                    {
                        throw new IOException("Invalid status response code.");
                    }

                    string json = ReadUtfString(ms);
                    return DeserializeStatusJson(json);
                }
            }
        }

        /// <summary>
        /// Deserializes the status json.
        /// </summary>
        /// <param name="value">The value to deserialize.</param>
        /// <returns>A new instance of the <see cref="ServerStatus"/> class.</returns>
        private static ServerStatus DeserializeStatusJson(string value)
        {
            dynamic source = JsonConvert.DeserializeObject(value);
            ServerStatus target = new ServerStatus();
            if (source.description != null)
            {
                target.Description = source.description.text;
            }

            if (source.players != null)
            {
                target.MaxPlayers = source.players.max;
                target.OnlinePlayers = source.players.online;
                if (source.players.sample != null)
                {
                    foreach (dynamic sourcePlayer in source.players.sample)
                    {
                        PlayerInfo targetPlayer = new PlayerInfo
                        {
                            Id = Guid.Parse((string)sourcePlayer.id),
                            Name = sourcePlayer.name
                        };

                        target.SamplePlayers.Add(targetPlayer);
                    }
                }
            }

            if (source.version != null)
            {
                target.MinecraftVersion = source.version.name;
                target.ProtocolVersion = source.version.protocol;
            }

            return target;
        }

        /// <summary>
        /// Reads the variable int32 asynchronously.
        /// </summary>
        /// <param name="sock">The sock.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The variable length integer from the stream.</returns>
        /// <exception cref="System.IO.IOException">
        /// Error receiving response.
        /// or
        /// Variable integer out of range.
        /// </exception>
        private static async Task<int> ReadVariableInt32Async(
            Socket sock,
            CancellationToken cancellationToken)
        {
            int result = 0;
            for (int i = 0; i < 5; i++)
            {
                byte[] buffer = new byte[1];
                cancellationToken.ThrowIfCancellationRequested();
                int byteCount = await sock.ReceiveAsync(buffer);
                if (byteCount < buffer.Length)
                {
                    throw new IOException("Error receiving response.");
                }

                int part = (int)buffer[0];
                result |= (part & 0x7F) << (7 * i);
                if ((part & 0x80) == 0)
                {
                    return result;
                }
            }

            throw new IOException("Variable integer out of range.");
        }

        /// <summary>
        /// Reads the variable length int32.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The variable length integer from the stream.</returns>
        /// <exception cref="System.IO.IOException">
        /// Unexpected end of stream.
        /// or
        /// Variable integer out of range.
        /// </exception>
        private static int ReadVariableInt32(Stream stream)
        {
            int result = 0;
            for (int i = 0; i < 5; i++)
            {
                int part = stream.ReadByte();
                if (part < 0)
                {
                    throw new IOException("Unexpected end of stream.");
                }

                result |= (part & 0x7F) << (7 * i);
                if ((part & 0x80) == 0)
                {
                    return result;
                }
            }

            throw new IOException("Variable integer out of range.");
        }

        /// <summary>
        /// Creates the status request.
        /// </summary>
        /// <returns>The status request.</returns>
        private static byte[] CreateStatusRequest()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] packet = CreateStatusRequestPacket();
                WriteVariableInt32(ms, packet.Length);
                ms.Write(packet, 0, packet.Length);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Creates the status request packet.
        /// </summary>
        /// <returns>The status request packet.</returns>
        private static byte[] CreateStatusRequestPacket()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WriteVariableInt32(ms, 0);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Creates the handshake request.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns>The handshake request.</returns>
        private static byte[] CreateHandshakeRequest(string host, int port)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] packet = CreateHandshakeRequestPacket(host, port);
                WriteVariableInt32(ms, packet.Length);
                ms.Write(packet, 0, packet.Length);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Creates the handshake request packet.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns>The handshake request.</returns>
        private static byte[] CreateHandshakeRequestPacket(string host, int port)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WriteVariableInt32(ms, 0);
                WriteVariableInt32(ms, ProtocolVersion);
                WriteUtfString(ms, host);
                WriteUInt16(ms, (ushort)port);
                WriteVariableInt32(ms, 1);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Writes the utf string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        private static void WriteUtfString(Stream stream, string value)
        {
            WriteVariableInt32(stream, value.Length);
            byte[] raw = Encoding.UTF8.GetBytes(value);
            stream.Write(raw, 0, value.Length);
        }

        /// <summary>
        /// Reads the utf string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The deserialized string.</returns>
        /// <exception cref="System.IO.IOException">Expected more string data.</exception>
        private static string ReadUtfString(Stream stream)
        {
            int length = ReadVariableInt32(stream);
            if (length == 0)
            {
                return string.Empty;
            }

            byte[] data = new byte[length];
            if (stream.Read(data, 0, length) != length)
            {
                throw new IOException("Expected more string data.");
            }

            return Encoding.UTF8.GetString(data, 0, length);
        }

        /// <summary>
        /// Writes a UInt16 to the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        private static void WriteUInt16(Stream stream, ushort value)
        {
            stream.WriteByte((byte)(value & 0xff));
            stream.WriteByte((byte)(value >> 8));
        }

        /// <summary>
        /// Writes the variable int32.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">value is out of range.</exception>
        private static void WriteVariableInt32(Stream stream, int value)
        {
            int remaining = value;
            for (int i = 0; i < 5; i++)
            {
                if ((remaining & ~0x7f) == 0)
                {
                    stream.WriteByte((byte)(remaining & 0x7f));
                    return;
                }

                stream.WriteByte((byte)(remaining & 0x7F | 0x80));
                remaining >>= 7;
            }

            throw new ArgumentOutOfRangeException("value");
        }
    }
}
