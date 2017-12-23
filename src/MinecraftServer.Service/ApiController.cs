// -----------------------------------------------------------------------
// <copyright file="ApiController.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MinecraftServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Web.Owin;

    /// <summary>
    /// Base class of API controller implementations that process REST style requests.
    /// </summary>
    internal abstract class ApiController
    {
        /// <summary>
        /// The bindings.
        /// </summary>
        private Collection<ApiBinding> bindings = new Collection<ApiBinding>();

        /// <summary>
        /// Gets or sets the request path base.
        /// </summary>
        public string RequestPathBase { get; set; }

        /// <summary>
        /// Gets the bindings.
        /// </summary>
        public Collection<ApiBinding> Bindings
        {
            get { return this.bindings; }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>An awaitable task which completes when the request is processed.</returns>
        public async Task ProcessRequest(IDictionary<string, object> environment)
        {
            OwinContext context = new OwinContext(environment);
            string method = context.RequestMethod;
            string requestPathBase = this.RequestPathBase ?? string.Empty;
            ApiBinding matchedBinding = null;
            foreach (ApiBinding binding in this.bindings)
            {
                if (!string.Equals(binding.HttpMethod, method, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (context.RequestPath.StartsWith(requestPathBase + binding.Path, StringComparison.OrdinalIgnoreCase))
                {
                    string remainder = context.RequestPath.Substring(requestPathBase.Length + binding.Path.Length);
                    if (remainder.Length == 0 || remainder[0] == '/')
                    {
                        matchedBinding = binding;
                        break;
                    }
                }
            }

            if (matchedBinding != null)
            {
                ClaimsPrincipal user = context.RequestUser;
                if (!matchedBinding.CheckAccess(user))
                {
                    context.ResponseStatusCode = (int)HttpStatusCode.Forbidden;
                    context.ResponseHeaders["ContentType"] = new string[] { "text/plain" };
                    return;
                }

                await matchedBinding.Dispatch(context);
            }
            else
            {
                context.ResponseStatusCode = (int)HttpStatusCode.NotFound;
                context.ResponseHeaders["ContentType"] = new string[] { "text/plain" };
            }
        }
    }
}
