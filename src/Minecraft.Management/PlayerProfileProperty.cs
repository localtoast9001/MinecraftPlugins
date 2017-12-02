//-----------------------------------------------------------------------
// <copyright file="PlayerProfileProperty.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Minecraft.Management
{
    /// <summary>
    /// A property name/value pair that is part of the player profile.
    /// </summary>
    /// <seealso cref="PlayerProfile"/>
    public class PlayerProfileProperty
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public byte[] Value { get; set; }
    }
}
