// -----------------------------------------------------------------------
// <copyright file="AuthorizationElement.cs" company="Jon Rowlett">
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
    /// Type of authorization.
    /// </summary>
    public enum AuthorizationType
    {
        /// <summary>
        /// Match the subject.
        /// </summary>
        Subject,

        /// <summary>
        /// Match the thumbprint.
        /// </summary>
        Thumbprint
    }

    /// <summary>
    /// An element that defines a criterion for matching a client certificate for authorization.
    /// </summary>
    public class AuthorizationElement : ConfigurationElement
    {
        /// <summary>
        /// collection of properties.
        /// </summary>
        private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

        /// <summary>
        /// the type property.
        /// </summary>
        private static ConfigurationProperty typeProperty = new ConfigurationProperty(
            "type",
            typeof(AuthorizationType));

        /// <summary>
        /// the value property.
        /// </summary>
        private static ConfigurationProperty valueProperty = new ConfigurationProperty(
            "value",
            typeof(string));

        /// <summary>
        /// Initializes static members of the <see cref="AuthorizationElement"/> class.
        /// </summary>
        static AuthorizationElement()
        {
            properties.Add(typeProperty);
            properties.Add(valueProperty);
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [ConfigurationProperty("type")]
        public AuthorizationType Type
        {
            get
            {
                return (AuthorizationType)this[typeProperty];
            }

            set
            {
                this[typeProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [ConfigurationProperty("value")]
        public string Value
        {
            get
            {
                return (string)this[valueProperty];
            }

            set
            {
                this[valueProperty] = value;
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
