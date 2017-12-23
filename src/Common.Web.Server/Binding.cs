// -----------------------------------------------------------------------
// <copyright file="Binding.cs" company="Jon Rowlett">
// Copyright (C) 2013-2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Server
{
    using System;

    /// <summary>
    /// Web server binding information.
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// The default HTTP port.
        /// </summary>
        public const int DefaultHttpPort = 80;

        /// <summary>
        /// The default HTTPS port.
        /// </summary>
        public const int DefaultHttpsPort = 443;

        /// <summary>
        /// The HTTP URI scheme.
        /// </summary>
        public const string HttpUriScheme = "http";

        /// <summary>
        /// The HTTPS URI scheme.
        /// </summary>
        public const string HttpsUriScheme = "https";

        /// <summary>
        /// The request path base.
        /// </summary>
        private string requestPathBase = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Binding"/> class.
        /// </summary>
        public Binding()
        {
            this.Port = DefaultHttpPort;
            this.UriScheme = HttpUriScheme;
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the URI scheme.
        /// </summary>
        public string UriScheme { get; set; }

        /// <summary>
        /// Gets or sets the request path base.
        /// </summary>
        /// <value>
        /// The request path base.
        /// </value>
        /// <exception cref="System.ArgumentNullException">value is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// value ends with <c>'/'</c>
        /// or
        /// value does not start with <c>'/'</c>.
        /// </exception>
        public string RequestPathBase
        {
            get
            {
                return this.requestPathBase;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (value.Length == 0)
                {
                    this.requestPathBase = string.Empty;
                    return;
                }

                if (value.EndsWith("/"))
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                if (!value.StartsWith("/"))
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this.requestPathBase = value;
            }
        }

        /// <summary>
        /// Creates a default HTTP binding.
        /// </summary>
        /// <returns>A new instance of the <see cref="Binding"/> class.</returns>
        public static Binding Http()
        {
            return Http(string.Empty);
        }

        /// <summary>
        /// Creates an HTTP binding at the given request path base.
        /// </summary>
        /// <param name="requestPathBase">The request path base.</param>
        /// <returns>A new instance of the <see cref="Binding"/> class.</returns>
        public static Binding Http(string requestPathBase)
        {
            return new Binding
            {
                Port = DefaultHttpPort,
                UriScheme = HttpUriScheme,
                RequestPathBase = requestPathBase
            };
        }

        /// <summary>
        /// Creates a default HTTPS binding.
        /// </summary>
        /// <returns>A new instance of the <see cref="Binding"/> class.</returns>
        public static Binding Https()
        {
            return Https(string.Empty);
        }

        /// <summary>
        /// Creates an HTTPS binding at the given request base.
        /// </summary>
        /// <param name="requestPathBase">The request path base.</param>
        /// <returns>A new instance of the <see cref="Binding"/> class.</returns>
        public static Binding Https(string requestPathBase)
        {
            return new Binding
            {
                Port = DefaultHttpsPort,
                UriScheme = HttpsUriScheme,
                RequestPathBase = requestPathBase
            };
        }
    }
}
