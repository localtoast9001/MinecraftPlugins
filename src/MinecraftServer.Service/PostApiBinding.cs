// -----------------------------------------------------------------------
// <copyright file="PostApiBinding.cs" company="Jon Rowlett">
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
    /// Templated binding for a post request.
    /// </summary>
    /// <typeparam name="T">The type of object serialized in the request body.</typeparam>
    /// <seealso cref="MinecraftServer.Service.ApiBinding" />
    internal class PostApiBinding<T> : ApiBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostApiBinding{T}"/> class.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        public PostApiBinding(Func<OwinContext, T, Task> implementation)
        {
            this.HttpMethod = "POST";
            this.Implementation = implementation;
        }

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        public Func<OwinContext, T, Task> Implementation { get; private set; }

        /// <summary>
        /// Dispatches the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// An awaitable task that completes when the request has finished processing.
        /// </returns>
        public override async Task Dispatch(OwinContext context)
        {
            Stream requestStream = context.RequestBody;
            T requestBody = default(T);
            using (JsonTextReader reader = new JsonTextReader(new StreamReader(requestStream)))
            {
                JsonSerializer serializer = new JsonSerializer();
                requestBody = serializer.Deserialize<T>(reader);
            }

            await this.Implementation(context, requestBody);
            context.ResponseStatusCode = (int)HttpStatusCode.OK;
            context.ResponseHeaders["ContentType"] = new string[] { "application/json" };
        }
    }
}
