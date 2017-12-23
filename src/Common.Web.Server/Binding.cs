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
        public const int DefaultHttpPort = 80;

        public const int DefaultHttpsPort = 443;

        public const string HttpUriScheme = "http";

        public const string HttpsUriScheme = "https";

        private string requestPathBase = string.Empty;

        public Binding()
        {
            this.Port = DefaultHttpPort;
            this.UriScheme = HttpUriScheme;
        }

        public int Port { get; set; }

        public string UriScheme { get; set; }

        public string RequestPathBase
        {
            get { return this.requestPathBase; }
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

        public static Binding Http()
        {
            return Http(string.Empty);
        }

        public static Binding Http(string requestPathBase)
        {
            return new Binding
            {
                Port = DefaultHttpPort,
                UriScheme = HttpUriScheme,
                RequestPathBase = requestPathBase
            };
        }

        public static Binding Https()
        {
            return Https(string.Empty);
        }

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
