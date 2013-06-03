// -----------------------------------------------------------------------
// <copyright file="AuthorizationElementCollection.cs" company="Jon Rowlett">
// Copyright (C) 2013 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost.Web.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Collection of AuthorizationElements.
    /// </summary>
    [ConfigurationCollection(typeof(AuthorizationElement))]
    public class AuthorizationElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new AuthorizationElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that acts as the key for the specified 
        /// <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            AuthorizationElement authElement = element as AuthorizationElement;
            if (authElement == null)
            {
                return null;
            }

            return authElement.Type + authElement.Value;
        }
    }
}
