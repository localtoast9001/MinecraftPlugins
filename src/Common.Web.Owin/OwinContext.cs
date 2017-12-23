// -----------------------------------------------------------------------
// <copyright file="OwinContext.cs" company="Jon Rowlett">
// Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Owin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Wrapper around a raw dictionary environment given by OWIN.
    /// </summary>
    public class OwinContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwinContext"/> class.
        /// </summary>
        public OwinContext()
        {
            this.Inner = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OwinContext"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <exception cref="System.ArgumentNullException">inner is null.</exception>
        public OwinContext(IDictionary<string, object> inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            this.Inner = inner;
        }

        /// <summary>
        /// Gets the inner environment.
        /// </summary>
        public IDictionary<string, object> Inner { get; private set; }

        /// <summary>
        /// Gets or sets the request body.
        /// </summary>
        public Stream RequestBody
        {
            get { return this.Inner.RequestBody(); }
            set { this.Inner.SetRequestBody(value); }
        }

        /// <summary>
        /// Gets or sets the request headers.
        /// </summary>
        public IDictionary<string, string[]> RequestHeaders
        {
            get { return this.Inner.RequestHeaders(); }
            set { this.Inner.SetRequestHeaders(value); }
        }

        /// <summary>
        /// Gets or sets the request method.
        /// </summary>
        public string RequestMethod
        {
            get { return this.Inner.RequestMethod(); }
            set { this.Inner.SetRequestMethod(value); }
        }

        /// <summary>
        /// Gets or sets the request path.
        /// </summary>
        public string RequestPath
        {
            get { return this.Inner.RequestPath(); }
            set { this.Inner.SetRequestPath(value); }
        }

        /// <summary>
        /// Gets or sets the request path base.
        /// </summary>
        public string RequestPathBase
        {
            get { return this.Inner.RequestPathBase(); }
            set { this.Inner.SetRequestPathBase(value); }
        }

        /// <summary>
        /// Gets or sets the request protocol.
        /// </summary>
        public string RequestProtocol
        {
            get { return this.Inner.RequestProtocol(); }
            set { this.Inner.SetRequestProtocol(value); }
        }

        /// <summary>
        /// Gets or sets the request query string.
        /// </summary>
        public string RequestQueryString
        {
            get { return this.Inner.RequestQueryString(); }
            set { this.Inner.SetRequestQueryString(value); }
        }

        /// <summary>
        /// Gets or sets the request scheme.
        /// </summary>
        public string RequestScheme
        {
            get { return this.Inner.RequestScheme(); }
            set { this.Inner.SetRequestScheme(value); }
        }

        /// <summary>
        /// Gets or sets the response body.
        /// </summary>
        public Stream ResponseBody
        {
            get { return this.Inner.ResponseBody(); }
            set { this.Inner.SetResponseBody(value); }
        }

        /// <summary>
        /// Gets or sets the response headers.
        /// </summary>
        public IDictionary<string, string[]> ResponseHeaders
        {
            get { return this.Inner.ResponseHeaders(); }
            set { this.Inner.SetResponseHeaders(value); }
        }

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        public int ResponseStatusCode
        {
            get { return this.Inner.ResponseStatusCode(); }
            set { this.Inner.SetResponseStatusCode(value); }
        }

        /// <summary>
        /// Gets or sets the remote ip address.
        /// </summary>
        public string RemoteIpAddress
        {
            get { return this.Inner.RemoteIpAddress(); }
            set { this.Inner.SetRemoteIpAddress(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this request is local.
        /// </summary>
        public bool IsLocal
        {
            get { return this.Inner.IsLocal(); }
            set { this.Inner.SetIsLocal(value); }
        }

        /// <summary>
        /// Gets or sets the client certificate.
        /// </summary>
        public X509Certificate ClientCertificate
        {
            get { return this.Inner.ClientCertificate(); }
            set { this.Inner.SetClientCertificate(value); }
        }

        /// <summary>
        /// Gets or sets the request user.
        /// </summary>
        public ClaimsPrincipal RequestUser
        {
            get { return this.Inner.RequestUser(); }
            set { this.Inner.SetRequestUser(value); }
        }
    }
}
