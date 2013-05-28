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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class WebServer : System.ComponentModel.Component
    {
        /// <summary>
        /// Max wait time to exit.
        /// </summary>
        private static readonly TimeSpan RequestThreadExitTimeout = new TimeSpan(0, 0, 30); // 30s. 

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
        /// Initializes a new instance of the <see cref="WebServer"/> class.
        /// </summary>
        /// <param name="requestHandlerCallback">The request handler callback.</param>
        public WebServer(Action<HttpListenerContext> requestHandlerCallback)
        {
            this.requestHandlerCallback = requestHandlerCallback;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.listener.Prefixes.Add("https://+:443/ConsoleHost/");
            this.requestProcessor = new Thread(this.RequestProcessorThread);
            this.requestProcessor.Start();
        }

        /// <summary>
        /// Stops the web server.
        /// </summary>
        public void Stop()
        {
            this.listener.Stop();
            this.requestProcessor.Join(RequestThreadExitTimeout);
        }

        /// <summary>
        /// Thread procedure to process Requests.
        /// </summary>
        private void RequestProcessorThread()
        {
            this.listener.Start();
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
                catch (Exception)
                {
                    break;
                }

                this.requestHandlerCallback(context);
            } 
            while (true);
        }
    }
}
