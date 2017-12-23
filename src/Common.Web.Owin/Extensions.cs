// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Jon Rowlett">
// Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Owin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using OwinEnvironment = System.Collections.Generic.IDictionary<string, object>;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the request body.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The request body stream.</returns>
        public static Stream RequestBody(this OwinEnvironment environment)
        {
            return environment[Keys.RequestBody] as Stream;
        }

        /// <summary>
        /// Sets the request body.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRequestBody(this OwinEnvironment environment, Stream value)
        {
            environment[Keys.RequestBody] = value;
        }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The headers in a dictionary.</returns>
        public static IDictionary<string, string[]> RequestHeaders(this OwinEnvironment environment)
        {
            return environment[Keys.RequestHeaders] as IDictionary<string, string[]>;
        }

        /// <summary>
        /// Sets the request headers.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRequestHeaders(this OwinEnvironment environment, IDictionary<string, string[]> value)
        {
            environment[Keys.RequestHeaders] = value;
        }

        /// <summary>
        /// Gets the request method.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The request method.</returns>
        public static string RequestMethod(this OwinEnvironment environment)
        {
            return environment[Keys.RequestMethod] as string;
        }

        /// <summary>
        /// Sets the request method.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRequestMethod(this OwinEnvironment environment, string value)
        {
            environment[Keys.RequestMethod] = value;
        }

        /// <summary>
        /// Gets the request path.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The request path.</returns>
        public static string RequestPath(this OwinEnvironment environment)
        {
            return environment[Keys.RequestPath] as string;
        }

        /// <summary>
        /// Sets the request path.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRequestPath(this OwinEnvironment environment, string value)
        {
            environment[Keys.RequestPath] = value;
        }

        /// <summary>
        /// Gets the request path base.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The request path base.</returns>
        public static string RequestPathBase(this OwinEnvironment environment)
        {
            return environment[Keys.RequestPathBase] as string;
        }

        /// <summary>
        /// Sets the request path base.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRequestPathBase(this OwinEnvironment environment, string value)
        {
            environment[Keys.RequestPathBase] = value;
        }

        /// <summary>
        /// Gets the request protocol.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The request protocol.</returns>
        public static string RequestProtocol(this OwinEnvironment environment)
        {
            return environment[Keys.RequestProtocol] as string;
        }

        /// <summary>
        /// Sets the request protocol.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRequestProtocol(this OwinEnvironment environment, string value)
        {
            environment[Keys.RequestProtocol] = value;
        }

        /// <summary>
        /// Gets the request query string.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The request query string.</returns>
        public static string RequestQueryString(this OwinEnvironment environment)
        {
            return environment[Keys.RequestQueryString] as string;
        }

        /// <summary>
        /// Sets the request query string.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRequestQueryString(this OwinEnvironment environment, string value)
        {
            environment[Keys.RequestQueryString] = value;
        }

        /// <summary>
        /// Gets the request scheme.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The request scheme.</returns>
        public static string RequestScheme(this OwinEnvironment environment)
        {
            return environment[Keys.RequestScheme] as string;
        }

        /// <summary>
        /// Sets the request scheme.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRequestScheme(this OwinEnvironment environment, string value)
        {
            environment[Keys.RequestScheme] = value;
        }

        /// <summary>
        /// Gets the response body.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The response body.</returns>
        public static Stream ResponseBody(this OwinEnvironment environment)
        {
            return environment[Keys.ResponseBody] as Stream;
        }

        /// <summary>
        /// Sets the response body.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetResponseBody(this OwinEnvironment environment, Stream value)
        {
            environment[Keys.ResponseBody] = value;
        }

        /// <summary>
        /// Gets the response headers.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The response headers.</returns>
        public static IDictionary<string, string[]> ResponseHeaders(this OwinEnvironment environment)
        {
            return environment[Keys.ResponseHeaders] as IDictionary<string, string[]>;
        }

        /// <summary>
        /// Sets the response headers.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetResponseHeaders(this OwinEnvironment environment, IDictionary<string, string[]> value)
        {
            environment[Keys.ResponseHeaders] = value;
        }

        /// <summary>
        /// Gets the response status code.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The response status code.</returns>
        public static int ResponseStatusCode(this OwinEnvironment environment)
        {
            int statusCode = (int)HttpStatusCode.OK;
            object rawStatusCode = null;
            if (environment.TryGetValue(Keys.ResponseStatusCode, out rawStatusCode))
            {
                statusCode = Convert.ToInt32(rawStatusCode);
            }

            return statusCode;
        }

        /// <summary>
        /// Sets the response status code.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetResponseStatusCode(this OwinEnvironment environment, int value)
        {
            environment[Keys.ResponseStatusCode] = value;
        }

        /// <summary>
        /// Gets the remote IP address.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The remote IP address.</returns>
        public static string RemoteIpAddress(this OwinEnvironment environment)
        {
            object raw = null;
            if (!environment.TryGetValue(Keys.RemoteIpAddress, out raw))
            {
                return null;
            }

            return raw as string;
        }

        /// <summary>
        /// Sets the remote IP address.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRemoteIpAddress(this OwinEnvironment environment, string value)
        {
            environment[Keys.RemoteIpAddress] = value;
        }

        /// <summary>
        /// Determines whether this request is local.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>
        ///   <c>true</c> if the specified request is local; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLocal(this OwinEnvironment environment)
        {
            object rawValue = null;
            if (!environment.TryGetValue(Keys.IsLocal, out rawValue))
            {
                return false;
            }

            return Convert.ToBoolean(rawValue);
        }

        /// <summary>
        /// Sets a value indicating whether this request is local.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">if set to <c>true</c> the request is local.</param>
        public static void SetIsLocal(this OwinEnvironment environment, bool value)
        {
            environment[Keys.IsLocal] = value;
        }

        /// <summary>
        /// Gets the client certificate.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The client certificate.</returns>
        public static X509Certificate ClientCertificate(this OwinEnvironment environment)
        {
            object result = null;
            if (!environment.TryGetValue(Keys.ClientCertificate, out result))
            {
                return null;
            }

            return result as X509Certificate;
        }

        /// <summary>
        /// Sets the client certificate.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetClientCertificate(this OwinEnvironment environment, X509Certificate value)
        {
            environment[Keys.ClientCertificate] = value;
        }

        /// <summary>
        /// Gets the request user.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The request user or <c>null</c>.</returns>
        public static ClaimsPrincipal RequestUser(this OwinEnvironment environment)
        {
            object raw = null;
            if (!environment.TryGetValue(Keys.RequestUser, out raw))
            {
                return null;
            }

            return raw as ClaimsPrincipal;
        }

        /// <summary>
        /// Sets the request user.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="value">The value.</param>
        public static void SetRequestUser(this OwinEnvironment environment, ClaimsPrincipal value)
        {
            environment[Keys.RequestUser] = value;
        }
    }
}
