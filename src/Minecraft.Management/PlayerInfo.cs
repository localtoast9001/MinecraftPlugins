//-----------------------------------------------------------------------
// <copyright file="PlayerInfo.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Minecraft.Management
{
    using System;

    /// <summary>
    /// Information about a player sent back with status for a server.
    /// </summary>
    /// <seealso cref="ServerStatus"/>
    public class PlayerInfo
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
