// -----------------------------------------------------------------------
// <copyright file="RootSection.cs" company="Jon Rowlett">
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
    /// The root of the custom config section.
    /// </summary>
    public class RootSection : ConfigurationSection
    {
        /// <summary>
        /// the authorization section.
        /// </summary>
        private static ConfigurationProperty authorizationProperty = new ConfigurationProperty(
            "authorization",
            typeof(AuthorizationSection));

        /// <summary>
        /// the set of properties on this section.
        /// </summary>
        private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

        /// <summary>
        /// Initializes static members of the <see cref="RootSection"/> class.
        /// </summary>
        static RootSection()
        {
            properties.Add(authorizationProperty);
        }

        /// <summary>
        /// Gets the authorization.
        /// </summary>
        [ConfigurationProperty("authorization")]
        public AuthorizationSection Authorization
        {
            get
            {
                return (AuthorizationSection)this[authorizationProperty];
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
