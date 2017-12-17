// -----------------------------------------------------------------------
// <copyright file="WebServer.cs" company="Jon Rowlett">
// Copyright (C) 2013 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MinecraftServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;

    /// <summary>
    /// Hosts an ASP.Net web server in the process.
    /// </summary>
    internal class WebServer
    {
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
        /// The request callback.
        /// </summary>
        private Func<HttpListenerContext, Task> requestHandlerCallback;

        private object syncRoot = new object();

        private ManualResetEvent connectionsAvailableEvent = new ManualResetEvent(true);

        private List<HttpListenerContext> activeConnections = new List<HttpListenerContext>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServer" /> class.
        /// </summary>
        /// <param name="log">The log stream.</param>
        /// <param name="requestHandlerCallback">The request handler callback.</param>
        /// <exception cref="System.ArgumentNullException">log</exception>
        public WebServer(
            ILogMessageStream log,
            Func<HttpListenerContext, Task> requestHandlerCallback)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            if (requestHandlerCallback == null)
            {
                throw new ArgumentNullException("requestHandlerCallback");
            }

            this.log = log;
            this.requestHandlerCallback = requestHandlerCallback;    
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.log.Log(LogMessage.Information("Starting web server..."));
            this.listener.Prefixes.Add("https://+:443/MinecraftServer/");
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

        private async void InvokeRequestHandler(HttpListenerContext context)
        {
            Task inner = await Task.Run<Task>(() => this.requestHandlerCallback(context));
            await inner;
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
}
