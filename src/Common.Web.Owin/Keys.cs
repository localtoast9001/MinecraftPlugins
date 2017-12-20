// -----------------------------------------------------------------------
// <copyright file="Keys.cs" company="Jon Rowlett">
// Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Owin
{
    /// <summary>
    /// Keys for values provided in the environment dictionary.
    /// </summary>
    public static class Keys
    {
        /// <summary>
        /// The request body key.
        /// </summary>
        public const string RequestBody = "owin.RequestBody";

        /// <summary>
        /// The request headers key.
        /// </summary>
        public const string RequestHeaders = "owin.RequestHeaders";

        /// <summary>
        /// The request method key.
        /// </summary>
        public const string RequestMethod = "owin.RequestMethod";

        /// <summary>
        /// The request path key.
        /// </summary>
        public const string RequestPath = "owin.RequestPath";

        /// <summary>
        /// The request path base key.
        /// </summary>
        public const string RequestPathBase = "owin.RequestPathBase";

        /// <summary>
        /// The request protocol key.
        /// </summary>
        public const string RequestProtocol = "owin.RequestProtocol";

        /// <summary>
        /// The request query string key.
        /// </summary>
        public const string RequestQueryString = "owin.RequestQueryString";

        /// <summary>
        /// The request scheme key.
        /// </summary>
        public const string RequestScheme = "owin.RequestScheme";

        /// <summary>
        /// The response body key.
        /// </summary>
        public const string ResponseBody = "owin.ResponseBody";

        /// <summary>
        /// The response headers key.
        /// </summary>
        public const string ResponseHeaders = "owin.ResponseHeaders";

        /// <summary>
        /// The response status code key.
        /// </summary>
        public const string ResponseStatusCode = "owin.ResponseStatusCode";

        /// <summary>
        /// The remote ip address key.
        /// </summary>
        public const string RemoteIpAddress = "server.RemoteIpAddress";

        /// <summary>
        /// The is local key.
        /// </summary>
        public const string IsLocal = "server.IsLocal";

        /// <summary>
        /// The client certificate key.
        /// </summary>
        public const string ClientCertificate = "ssl.ClientCertificate";
    }
}
