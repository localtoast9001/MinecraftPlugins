// -----------------------------------------------------------------------
// <copyright file="WebServer.cs" company="Jon Rowlett">
// Copyright (C) 2013-2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
    using Common.Web.Owin;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;
    using MidFactory = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>;
    using BuildFunc = System.Action<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>>;

    /// <summary>
    /// Hosts an OWIN web server in the process.
    /// </summary>
    public class WebServer
    {
        /// <summary>
        /// The maximum connections.
        /// </summary>
        private const int MaxConnections = 50;

        /// <summary>
        /// Max wait time to exit.
        /// </summary>
        private static readonly TimeSpan RequestThreadExitTimeout = new TimeSpan(0, 0, 30); // 30s.         
        
        /// <summary>
        /// The log stream.
        /// </summary>
        private ILogMessageStream log;

        /// <summary>
        /// The http listener.
        /// </summary>
        private HttpListener listener = new HttpListener();

        /// <summary>
        /// The thread that will process requests.
        /// </summary>
        private Thread requestProcessor;

        /// <summary>
        /// The synchronize root.
        /// </summary>
        private object syncRoot = new object();

        /// <summary>
        /// The connections available event.
        /// </summary>
        private ManualResetEvent connectionsAvailableEvent = new ManualResetEvent(true);

        /// <summary>
        /// The active connections.
        /// </summary>
        private List<HttpListenerContext> activeConnections = new List<HttpListenerContext>();

        /// <summary>
        /// The application.
        /// </summary>
        private AppFunc app;

        /// <summary>
        /// The startup callback.
        /// </summary>
        private Action<BuildFunc> startup;

        /// <summary>
        /// The request handler.
        /// </summary>
        private AppFunc requestHandler;

        /// <summary>
        /// The binding.
        /// </summary>
        private readonly Binding binding;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServer" /> class.
        /// </summary>
        /// <param name="log">The log stream.</param>
        /// <param name="startup">The startup.</param>
        /// <param name="app">The application.</param>
        /// <param name="binding">The binding.</param>
        /// <exception cref="ArgumentNullException">
        /// log is null
        /// or
        /// app is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">log is null.</exception>
        public WebServer(
            ILogMessageStream log,
            Action<BuildFunc> startup,
            AppFunc app,
            Binding binding = null)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            this.log = log;
            this.app = app;
            this.startup = startup;
            this.binding = binding ?? Binding.Http();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            IDictionary<string, object> startupProperties = new Dictionary<string, object>();
            Builder builder = new Builder();
            if (this.startup != null)
            {
                this.startup(builder.Use);
            }

            this.requestHandler = this.app;
            foreach (MidFactory factory in builder.Factories)
            {
                MidFunc middleware = factory(startupProperties);
                this.requestHandler = middleware(this.requestHandler);
            }

            string prefix = this.binding.UriScheme + "://+:" + this.binding.Port.ToString() + this.binding.RequestPathBase + "/";
            this.log.Log(LogMessage.Information("Starting web server..."));
            this.listener.Prefixes.Add(prefix);
            this.listener.Start();
            this.requestProcessor = new Thread(this.RequestProcessorThread);
            this.requestProcessor.Start();
            this.log.Log(LogMessage.Information("Web server started."));
        }

        /// <summary>
        /// Stops the web server.
        /// </summary>
        public void Stop()
        {
            this.log.Log(LogMessage.Information("Stopping web server..."));
            this.listener.Stop();
            this.requestProcessor.Join(RequestThreadExitTimeout);
            this.log.Log(LogMessage.Information("Web server stopped."));
        }

        /// <summary>
        /// Thread procedure to process Requests.
        /// </summary>
        private void RequestProcessorThread()
        {
            do
            {
                // TODO: Cancel for thread exit.
                this.connectionsAvailableEvent.WaitOne();
                HttpListenerContext context = null;

                try
                {
                    context = this.listener.GetContext();
                    if (context == null)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    this.log.Log(LogMessage.Error(ex.ToString()));
                    break;
                }

                lock (this.syncRoot)
                {
                    this.activeConnections.Add(context);
                    if (this.activeConnections.Count >= MaxConnections)
                    {
                        this.connectionsAvailableEvent.Reset();
                    }
                }

                InvokeRequestHandler(context);
            } 
            while (true);
        }

        /// <summary>
        /// Invokes the request handler.
        /// </summary>
        /// <param name="context">The context.</param>
        private async void InvokeRequestHandler(HttpListenerContext context)
        {
            OwinContext environment = this.CreateOwinEnvironment(context);
            try
            {
                Task inner = await Task.Run<Task>(() => this.requestHandler(environment.Inner));
                await inner;
                int statusCode = environment.ResponseStatusCode;

                IDictionary<string, string[]> responseHeaders =
                    environment.ResponseHeaders;
                foreach (string headerName in responseHeaders.Keys)
                {
                    context.Response.Headers.Set(headerName, string.Join(", ", responseHeaders[headerName]));
                }

                context.Response.StatusCode = statusCode;
                string logText = string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "{0} {1} {2} {3} {4}",
                    environment.RemoteIpAddress,
                    environment.RequestMethod,
                    environment.RequestPath,
                    environment.ResponseStatusCode,
                    environment.RequestProtocol);
                this.log.Log(LogMessage.Information(logText));
                context.Response.Close();
            }
            finally
            {
                lock (this.syncRoot)
                {
                    this.activeConnections.Remove(context);
                    if (this.activeConnections.Count < MaxConnections)
                    {
                        this.connectionsAvailableEvent.Set();
                    }
                }
            }
        }

        /// <summary>
        /// Creates the owin environment.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The environment properties.</returns>
        private OwinContext CreateOwinEnvironment(HttpListenerContext context)
        {
            string requestPathBase = this.binding.RequestPathBase;
            string requestPath = context.Request.Url.AbsolutePath.Substring(requestPathBase.Length);
            X509Certificate2 clientCert = context.Request.GetClientCertificate();
            return new OwinContext
            {
                RequestBody = context.Request.InputStream,
                RequestHeaders = new Dictionary<string, string[]> { },
                RequestMethod = context.Request.HttpMethod,
                RequestPath = requestPath,
                RequestPathBase = requestPathBase,
                RequestProtocol = "HTTP/" + context.Request.ProtocolVersion.ToString(),
                RequestQueryString = context.Request.Url.Query,
                RequestScheme = context.Request.Url.Scheme,
                ResponseBody = context.Response.OutputStream,
                ResponseHeaders = new Dictionary<string, string[]> { },
                IsLocal = context.Request.IsLocal,
                RemoteIpAddress = context.Request.RemoteEndPoint.Address.ToString(),
                ClientCertificate = clientCert
            };
        }

        /// <summary>
        /// Builder callback to handle composition of middleware components.
        /// </summary>
        private class Builder
        {
            /// <summary>
            /// The factories for middleware.
            /// </summary>
            private List<MidFactory> factories = new List<MidFactory>();

            /// <summary>
            /// Gets the factories.
            /// </summary>
            public IList<MidFactory> Factories
            {
                get { return this.factories; }
            }

            /// <summary>
            /// Uses the specified factory.
            /// </summary>
            /// <param name="factory">The factory.</param>
            public void Use(MidFactory factory)
            {
                this.factories.Add(factory);
            }
        }
    }
}
