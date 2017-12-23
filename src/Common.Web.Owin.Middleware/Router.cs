// -----------------------------------------------------------------------
// <copyright file="Router.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Owin.Middleware
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;

    /// <summary>
    /// Middleware which routes requests to different handlers.
    /// </summary>
    public class Router
    {
        /// <summary>
        /// The routes.
        /// </summary>
        private readonly List<Route> routes = new List<Route>();

        /// <summary>
        /// The synchronize root.
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="routes">The routes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// routes is null
        /// or
        /// next is null
        /// </exception>
        public Router(
            AppFunc next,
            IEnumerable<Route> routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }

            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            this.Next = next;
            this.routes.AddRange(routes);
        }

        /// <summary>
        /// Gets or sets the next application function.
        /// </summary>
        private AppFunc Next { get; set; }

        /// <summary>
        /// Creates the middleware factory based on specified startup properties.
        /// </summary>
        /// <param name="startupProperties">The startup properties.</param>
        /// <param name="routes">The routes.</param>
        /// <returns>A function to create the middleware.</returns>
        public static MidFunc Create(IDictionary<string, object> startupProperties, IEnumerable<Route> routes)
        {
            return (next) =>
            {
                Router router = new Router(next, routes);
                return router.ProcessRequest;
            };
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>An awaitable task that completes when the request is processed.</returns>
        public async Task ProcessRequest(IDictionary<string, object> environment)
        {
            OwinContext context = new OwinContext(environment);
            Route matchingRoute = this.FindRoute(context.RequestPath);
            if (matchingRoute != null)
            {
                await matchingRoute.App(environment);
            }
            else
            {
                await this.Next(environment);
            }
        }

        /// <summary>
        /// Finds the route.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The matching route or <c>null</c>.</returns>
        private Route FindRoute(string path)
        {
            lock (this.syncRoot)
            {
                foreach (Route route in this.routes)
                {
                    if (path.StartsWith(route.Path, StringComparison.OrdinalIgnoreCase))
                    {
                        string remainder = path.Substring(route.Path.Length);
                        if (remainder.Length == 0 || remainder[0] == '/')
                        {
                            if (remainder.Length < 2 || route.IncludeChildPaths)
                            {
                                return route;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
