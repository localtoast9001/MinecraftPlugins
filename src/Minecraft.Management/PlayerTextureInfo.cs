//-----------------------------------------------------------------------
// <copyright file="PlayerTextureInfo.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Minecraft.Management
{
    using System;

    /// <summary>
    /// Player texture info returned with the player profile.
    /// </summary>
    /// <see cref="PlayerProfile"/>
    public class PlayerTextureInfo
    {
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the name of the profile.
        /// </summary>
        public string ProfileName { get; set; }

        /// <summary>
        /// Gets or sets the skin texture.
        /// </summary>
        public Uri SkinTexture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PlayerTextureInfo"/> is slim.
        /// </summary>
        public bool Slim { get; set; }

        /// <summary>
        /// Gets or sets the cape texture.
        /// </summary>
        public Uri CapeTexture { get; set; }
    }
}
