// -----------------------------------------------------------------------
// <copyright file="WebServer.cs" company="">
// TODO: Update copyright text.
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class WebServer
    {
        private HttpListener listener = new HttpListener();
        private Thread requestProcessor;
        private Action<HttpListenerContext> requestHandlerCallback;

        public WebServer(Action<HttpListenerContext> requestHandlerCallback)
        {
            this.requestHandlerCallback = requestHandlerCallback;
        }

        public void Start()
        {
            listener.Prefixes.Add("http://+:80/Temporary_Listen_Addresses/ConsoleHost/");
            requestProcessor = new Thread(RequestProcessorThread);
            requestProcessor.Start();
        }

        public void Stop()
        {
            listener.Stop();
            requestProcessor.Join();
        }

        private void RequestProcessorThread()
        {
            listener.Start();
            do
            {
                HttpListenerContext context = null;
                try
                {
                    context = listener.GetContext();
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

            } while (true);
        }
    }
}
