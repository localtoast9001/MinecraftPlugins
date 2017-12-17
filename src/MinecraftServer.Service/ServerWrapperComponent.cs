// -----------------------------------------------------------------------
// <copyright file="ServerWrapperService.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MinecraftServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Logging;

    /// <summary>
    /// Core service logic that can be invoked in console mode or service mode.
    /// </summary>
    internal class ServerWrapperComponent
    {
        private ServerHost host;

        private WebServer webServer;

        private ServerApiController controller;

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        public void Start(ILogMessageStream log, string[] args)
        {
            this.host = new ServerHost(
                ConfigurationManager.AppSettings["MinecraftDirectory"],
                ConfigurationManager.AppSettings["JarFileName"]);
            try
            {
                this.controller = new ServerApiController("/MinecraftServer", this.host);
                this.webServer = new WebServer(log, this.controller.ProcessRequest);
                this.webServer.Start();
            }
            catch
            {
                this.host.Stop();
                throw;
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        public void Stop()
        {
            this.webServer.Stop();
            this.host.Stop();
        }
    }
}
