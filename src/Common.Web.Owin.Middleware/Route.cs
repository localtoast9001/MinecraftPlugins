// -----------------------------------------------------------------------
// <copyright file="Route.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Owin.Middleware
{
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    /// <summary>
    /// Describes a mapping between a path and a handler application function.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the route includes child paths.
        /// </summary>
        public bool IncludeChildPaths { get; set; }

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        public AppFunc App { get; set; }
    }
}
