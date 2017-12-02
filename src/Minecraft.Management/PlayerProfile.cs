//-----------------------------------------------------------------------
// <copyright file="PlayerProfile.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Minecraft.Management
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Player profile information.
    /// </summary>
    public class PlayerProfile
    {
        /// <summary>
        /// The properties.
        /// </summary>
        private Collection<PlayerProfileProperty> properties = new Collection<PlayerProfileProperty>();

        /// <summary>
        /// Gets or sets the player UUID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public Collection<PlayerProfileProperty> Properties
        {
            get { return this.properties; }
        }

        /// <summary>
        /// Gets or sets the textures.
        /// </summary>
        public PlayerTextureInfo Textures { get; set; }
    }
}
