// -----------------------------------------------------------------------
// <copyright file="WebServer.cs" company="Jon Rowlett">
// Copyright (C) 2013 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace ConsoleHost.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Web.Hosting;
    using Common.Logging;

    /// <summary>
    /// Hosts an ASP.Net web server in the process.
    /// </summary>
    internal class WebServer : System.ComponentModel.Component
    {
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
        private Action<HttpListenerContext> requestHandlerCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServer" /> class.
        /// </summary>
        /// <param name="log">The log stream.</param>
        /// <param name="requestHandlerCallback">The request handler callback.</param>
        /// <exception cref="System.ArgumentNullException">log</exception>
        public WebServer(
            ILogMessageStream log,
            Action<HttpListenerContext> requestHandlerCallback)
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
            this.listener.Prefixes.Add("https://+:443/ConsoleHost/");
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

                this.requestHandlerCallback(context);
            } 
            while (true);
        }
    }
}
