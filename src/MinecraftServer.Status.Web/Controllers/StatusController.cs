// -----------------------------------------------------------------------
// <copyright file="StatusController.cs" company="Jon Rowlett">
//  Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MinecraftServer.Status.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using System.Configuration;
    using Minecraft.Management;

    /// <summary>
    /// API to get server status.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/v1/status")]
    public class StatusController : ApiController
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>The status for the local server on the default port.</returns>
        [Route]
        public Task<ServerStatus> Get()
        {
            return ServerPingUtility.GetStatusAsync(GetMinecraftServer());
        }

        /// <summary>
        /// Gets the minecraft server.
        /// </summary>
        /// <returns>The host name of the server.</returns>
        private static string GetMinecraftServer()
        {
            string host = ConfigurationManager.AppSettings.Get("MinecraftServer");
            if (string.IsNullOrEmpty(host))
            {
                host = "localhost";
            }

            return host;
        }
    }
}
