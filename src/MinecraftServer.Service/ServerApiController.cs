// -----------------------------------------------------------------------
// <copyright file="ServerApiController.cs" company="Jon Rowlett">
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
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Web.Owin;
    using Minecraft.Management;
    using Newtonsoft.Json;

    /// <summary>
    /// REST API to interface the Minecraft server.
    /// </summary>
    /// <seealso cref="MinecraftServer.Service.ApiController" />
    internal class ServerApiController : ApiController
    {
        /// <summary>
        /// The server host.
        /// </summary>
        private readonly ServerHost host;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerApiController"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public ServerApiController(ServerHost host)
        {
            this.host = host;
            this.RequestPathBase = "/console";
            GetApiBinding<IEnumerable<MinecraftLogEntry>> getBinding = new GetApiBinding<IEnumerable<MinecraftLogEntry>>(
                this.GetMessages)
            {
                Path = string.Empty,
                AllowedRoles = new string[] { "User" }
            };
            PostApiBinding<string> sayBinding = new PostApiBinding<string>(this.Say)
            {
                Path = "/say",
                AllowedRoles = new string[] { "User" }
            };

            this.Bindings.Add(getBinding);
            this.Bindings.Add(sayBinding);
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>An awaitable task that completes with a snapshot of the console messages.</returns>
        protected Task<IEnumerable<MinecraftLogEntry>> GetMessages(OwinContext context)
        {
            return Task.Run(() => this.host.Messages);
        }

        /// <summary>
        /// Says the given text on the server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="text">The text.</param>
        /// <returns>An awaitable task that completes when the text is posted.</returns>
        protected Task Say(OwinContext context, string text)
        {
            return Task.Run(() => this.host.Say(text));
        }
    }
}
