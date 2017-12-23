// -----------------------------------------------------------------------
// <copyright file="ServerWrapperComponent.cs" company="Jon Rowlett">
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
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Logging;
    using Common.Web.Owin;
    using Common.Web.Owin.Middleware;
    using Common.Web.Server;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using BuildFunc = System.Action<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>>;
    using MidFactory = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>;
    using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;

    /// <summary>
    /// Core service logic that can be invoked in console mode or service mode.
    /// </summary>
    internal class ServerWrapperComponent : IDisposable
    {
        /// <summary>
        /// The server host.
        /// </summary>
        private ServerHost host;

        /// <summary>
        /// The web server.
        /// </summary>
        private WebServer webServer;

        /// <summary>
        /// The controller.
        /// </summary>
        private ServerApiController controller;

        /// <summary>
        /// Finalizes an instance of the <see cref="ServerWrapperComponent"/> class.
        /// </summary>
        ~ServerWrapperComponent()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="args">Data passed by the start command.</param>
        public void Start(ILogMessageStream log, string[] args)
        {
            this.host = new ServerHost(
                ConfigurationManager.AppSettings["MinecraftDirectory"],
                ConfigurationManager.AppSettings["JarFileName"]);
            try
            {
                Binding binding = Binding.Https("/MinecraftServer");
                this.controller = new ServerApiController(this.host);
                this.webServer = new WebServer(
                    log, 
                    this.BuilderCallback,
                    this.controller.ProcessRequest,
                    binding);
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; 
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.webServer.Dispose();
                this.host.Dispose();
            }
        }

        /// <summary>
        /// Callback to build the web server components.
        /// </summary>
        /// <param name="builder">The builder function.</param>
        private void BuilderCallback(BuildFunc builder)
        {
            builder
                .UseClientCertificateAuthenticationProvider(this.UserRoleMapper)
                .UseErrorHandler();
        }

        /// <summary>
        /// The callback that maps user claims to roles.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns>An awaitable task which completes when the role claims have been added to the given identity.</returns>
        private Task UserRoleMapper(ClaimsIdentity identity)
        {
            return Task.Run(() =>
            {
                Claim thumbprintClaim = identity.Claims.FirstOrDefault(
                    e => string.Equals(e.Type, ClaimTypes.Thumbprint, StringComparison.OrdinalIgnoreCase));
                if (thumbprintClaim == null)
                {
                    return;
                }

                string thumbprint = thumbprintClaim.Value;
                string allowedThumbprints = ConfigurationManager.AppSettings["AllowedThumbprints"];
                if (string.IsNullOrEmpty(allowedThumbprints))
                {
                    return;
                }

                foreach (string allowedThumb in allowedThumbprints.Split(','))
                {
                    if (string.Equals(allowedThumb, thumbprint, StringComparison.OrdinalIgnoreCase))
                    {
                        Claim roleClaim = new Claim(ClaimTypes.Role, "User");
                        identity.AddClaim(roleClaim);
                        return;
                    }
                }
            });
        }
    }
}
