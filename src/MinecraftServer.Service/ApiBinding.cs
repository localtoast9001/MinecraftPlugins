// -----------------------------------------------------------------------
// <copyright file="ApiBinding.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MinecraftServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Web.Owin;

    /// <summary>
    /// Binds requests to implementation methods in classes derived from <see cref="ApiController"/>.
    /// </summary>
    internal abstract class ApiBinding
    {
        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        public string HttpMethod { get; protected set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the allowed roles.
        /// </summary>
        public string[] AllowedRoles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow anonymous access.
        /// </summary>
        public bool AllowAnonymous { get; set; }

        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if access is allowed; otherwise, <c>false</c>.</returns>
        public bool CheckAccess(ClaimsPrincipal user)
        {
            if (this.AllowAnonymous)
            {
                return true;
            }

            if (user == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            foreach (string allowedRole in this.AllowedRoles ?? new string[0])
            {
                if (user.IsInRole(allowedRole))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Dispatches the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>An awaitable task that completes when the request has finished processing.</returns>
        public abstract Task Dispatch(OwinContext context);
    }
}
