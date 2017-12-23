// -----------------------------------------------------------------------
// <copyright file="GetApiBinding.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MinecraftServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Web.Owin;
    using Newtonsoft.Json;

    /// <summary>
    /// Templated binding for a get request.
    /// </summary>
    /// <typeparam name="T">The type of object that is sent back in the response.</typeparam>
    /// <seealso cref="MinecraftServer.Service.ApiBinding" />
    internal class GetApiBinding<T> : ApiBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetApiBinding{T}"/> class.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        public GetApiBinding(Func<OwinContext, Task<T>> implementation)
        {
            this.Implementation = implementation;
            this.HttpMethod = "GET";
        }

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        public Func<OwinContext, Task<T>> Implementation { get; private set; }

        /// <summary>
        /// Dispatches the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// An awaitable task that completes when the request has finished processing.
        /// </returns>
        public override async Task Dispatch(OwinContext context)
        {
            T result = await this.Implementation(context);

            using (JsonTextWriter writer = new JsonTextWriter(
                new StreamWriter(context.ResponseBody)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, result);
            }

            context.ResponseStatusCode = (int)HttpStatusCode.OK;
            context.ResponseHeaders["ContentType"] = new string[] { "application/json" };
        }
    }
}
