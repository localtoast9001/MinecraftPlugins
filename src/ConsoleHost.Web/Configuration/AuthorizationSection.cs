// -----------------------------------------------------------------------
// <copyright file="AuthorizationSection.cs" company="Jon Rowlett">
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
    /// Defines a set of allow and deny productions.
    /// </summary>
    public class AuthorizationSection : ConfigurationElement
    {
        /// <summary>
        /// the set of properties.
        /// </summary>
        private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

        /// <summary>
        /// the set of allow productions.
        /// </summary>
        private static ConfigurationProperty allowProperty = new ConfigurationProperty(
            "allow",
            typeof(AuthorizationElementCollection));

        /// <summary>
        /// the set of deny productions.
        /// </summary>
        private static ConfigurationProperty denyProperty = new ConfigurationProperty(
            "deny",
            typeof(AuthorizationElementCollection));

        /// <summary>
        /// Initializes static members of the <see cref="AuthorizationSection"/> class.
        /// </summary>
        static AuthorizationSection()
        {
            properties.Add(allowProperty);
            properties.Add(denyProperty);
        }

        /// <summary>
        /// Gets the allow.
        /// </summary>
        [ConfigurationProperty("allow")]
        public AuthorizationElementCollection Allow
        {
            get
            {
                return (AuthorizationElementCollection)this[allowProperty];
            }
        }

        /// <summary>
        /// Gets the deny.
        /// </summary>
        [ConfigurationProperty("deny")]
        public AuthorizationElementCollection Deny
        {
            get
            {
                return (AuthorizationElementCollection)this[denyProperty];
            }
        }

        /// <summary>
        /// Gets the collection of properties.
        /// </summary>
        /// <returns>The <see cref="T:System.Configuration.ConfigurationPropertyCollection" /> 
        /// of properties for the element.</returns>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return properties;
            }
        }
    }
}
