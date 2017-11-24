//-----------------------------------------------------------------------
// <copyright file="ServerStatus.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Minecraft.Management
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Status information for a Minecraft server.
    /// </summary>
    public class ServerStatus
    {
        /// <summary>
        /// The sample players.
        /// </summary>
        private Collection<PlayerInfo> samplePlayers = new Collection<PlayerInfo>();

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the maximum players.
        /// </summary>
        public int MaxPlayers { get; set; }

        /// <summary>
        /// Gets or sets the online players.
        /// </summary>
        public int OnlinePlayers { get; set; }

        /// <summary>
        /// Gets the sample players.
        /// </summary>
        public Collection<PlayerInfo> SamplePlayers
        {
            get { return this.samplePlayers; }
        }

        /// <summary>
        /// Gets or sets the minecraft version.
        /// </summary>
        public string MinecraftVersion { get; set; }

        /// <summary>
        /// Gets or sets the protocol version.
        /// </summary>
        public int ProtocolVersion { get; set; }
    }
}
