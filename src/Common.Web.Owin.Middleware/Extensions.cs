// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Owin.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using BuildFunc = System.Action<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>>;
    using MidFactory = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>;
    using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Uses an error handler as middleware in the OWIN pipeline.
        /// </summary>
        /// <param name="builder">The builder function.</param>
        /// <returns>The same builder function.</returns>
        public static BuildFunc UseErrorHandler(this BuildFunc builder)
        {
            builder(ErrorHandler.Create);
            return builder;
        }

        /// <summary>
        /// Uses the client certificate authentication provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="userRoleMapper">A callback function to add role claims to the user.</param>
        /// <returns>
        /// The builder function.
        /// </returns>
        public static BuildFunc UseClientCertificateAuthenticationProvider(
            this BuildFunc builder,
            Func<ClaimsIdentity, Task> userRoleMapper)
        {
            builder((startupProperties) => ClientCertificateAuthenticationProvider.Create(startupProperties, userRoleMapper));
            return builder;
        }

        /// <summary>
        /// Uses a route to route requests to different application handlers.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="routes">The routes.</param>
        /// <returns>The builder function.</returns>
        public static BuildFunc UseRouter(
            this BuildFunc builder,
            IEnumerable<Route> routes)
        {
            builder((startupProperties) => Router.Create(startupProperties, routes));
            return builder;
        }
    }
}
