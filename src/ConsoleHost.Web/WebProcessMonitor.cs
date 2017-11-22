// -----------------------------------------------------------------------
// <copyright file="WebProcessMonitor.cs" company="Jon Rowlett">
// Copyright (C) 2013 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#define TRACE
namespace ConsoleHost.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.Remoting.Lifetime;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;
    using Common.Logging;

    /// <summary>
    /// Brokers input and output between the process and a web interface.
    /// </summary>
    public class WebProcessMonitor : ProcessMonitor
    {
        /// <summary>
        /// Buffer for output.
        /// </summary>
        private MemoryMessageStream outputStream = new MemoryMessageStream();

        /// <summary>
        /// Wraps the process host.
        /// </summary>
        private ProcessHostWrapper hostWrapper;

        /// <summary>
        /// Exposes the process to the web interface.
        /// </summary>
        private ProcessMonitorHost host;

        /// <summary>
        /// Used to keep the host alive.
        /// </summary>
        private ClientSponsor hostSponser = new ClientSponsor();

        /// <summary>
        /// Initializes a new instance of the <see cref="WebProcessMonitor"/> class.
        /// </summary>
        /// <param name="process">The process.</param>
        public WebProcessMonitor(ConsoleHost.IProcessHost process)
            : base(process)
        {
            process.RegisterOutputConsumer(this.outputStream);
            this.hostWrapper = new WebProcessMonitor.ProcessHostWrapper(process);
            this.host = WebProcessMonitor.ProcessMonitorHost.Create();
            this.hostSponser.Register(this.host);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void Start()
        {
            this.host.Start(this.outputStream, this.hostWrapper);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            this.host.Stop();
            this.hostSponser.Unregister(this.host);
            this.hostSponser.Close();
        }

        /// <summary>
        /// Wrapper to expose the host to a different AppDomain.
        /// </summary>
        internal class ProcessHostWrapper : Component, ConsoleHost.IProcessHost
        {
            /// <summary>
            /// The inner interface.
            /// </summary>
            private ConsoleHost.IProcessHost inner;

            /// <summary>
            /// Initializes a new instance of the <see cref="ProcessHostWrapper"/> class.
            /// </summary>
            /// <param name="inner">The inner interface.</param>
            public ProcessHostWrapper(ConsoleHost.IProcessHost inner)
            {
                this.inner = inner;
            }

            /// <summary>
            /// Gets the id.
            /// </summary>
            public int Id
            {
                get { return this.inner.Id; }
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            public string Name
            {
                get { return this.inner.Name; }
            }

            /// <summary>
            /// Registers the output consumer.
            /// </summary>
            /// <param name="outputConsumer">The output consumer.</param>
            /// <exception cref="System.NotImplementedException">this is not needed.</exception>
            public void RegisterOutputConsumer(IMessageConsumer outputConsumer)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Posts the specified message.
            /// </summary>
            /// <param name="message">The message.</param>
            public void Post(Message message)
            {
                this.inner.Post(message);
            }
        }

        /// <summary>
        /// Brokers the process host to the web interface.
        /// </summary>
        internal class ProcessMonitorHost : Component
        {
            /// <summary>
            /// The log stream.
            /// </summary>
            private SourceLogMessageStream log = new SourceLogMessageStream();

            /// <summary>
            /// The internal web server.
            /// </summary>
            private WebServer webServer;

            /// <summary>
            /// The output stream.
            /// </summary>
            private MemoryMessageStream outputStream;

            /// <summary>
            /// The interface to the process.
            /// </summary>
            private ConsoleHost.IProcessHost processHost;

            /// <summary>
            /// Keeps the host alive.
            /// </summary>
            private ClientSponsor sponsor = new ClientSponsor();

            /// <summary>
            /// Initializes a new instance of the <see cref="ProcessMonitorHost" /> class.
            /// </summary>
            /// <param name="log">The log stream.</param>
            /// <exception cref="System.ArgumentNullException">log</exception>
            public ProcessMonitorHost()
            {
                this.webServer = new WebServer(this.log, this.OnRuntimeRequest);
            }

            /// <summary>
            /// Creates this instance.
            /// </summary>
            /// <returns>a new ProcessMonitorHost proxy.</returns>
            public static ProcessMonitorHost Create()
            {
                return (ProcessMonitorHost)ApplicationHost.CreateApplicationHost(
                    typeof(ProcessMonitorHost),
                    "/ConsoleHost/",
                    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            }

            /// <summary>
            /// Starts the specified output stream.
            /// </summary>
            /// <param name="outputStream">The output stream.</param>
            /// <param name="processHost">The process host.</param>
            public void Start(MemoryMessageStream outputStream, ConsoleHost.IProcessHost processHost)
            {
                this.log.Targets.Add(new ConsoleLogMessageStream());
                this.outputStream = outputStream;
                this.processHost = processHost;
                AppDomain.CurrentDomain.SetData(".output", this.outputStream);
                AppDomain.CurrentDomain.SetData(".processHost", this.processHost);
                this.sponsor.Register((MarshalByRefObject)this.processHost);
                this.sponsor.Register(this.outputStream);
                this.webServer.Start();
            }

            /// <summary>
            /// Stops this instance.
            /// </summary>
            public void Stop()
            {
                this.webServer.Stop();
                this.sponsor.Unregister((MarshalByRefObject)this.processHost);
                this.sponsor.Unregister(this.outputStream);
            }

            /// <summary>
            /// Called to process a runtime request.
            /// </summary>
            /// <param name="context">The context.</param>
            private void OnRuntimeRequest(HttpListenerContext context)
            {
                ListenerWorkerRequest request = new ListenerWorkerRequest(
                    "/ConsoleHost/",
                    context);
                this.log.Log(LogMessage.Information(string.Format(
                    "{0} {1} {2} {3}",
                    context.Request.HttpMethod,
                    context.Request.RawUrl,
                    context.Request.UserHostAddress,
                    context.Request.UserAgent)));
                HttpRuntime.ProcessRequest(request);
            }
        }
    }
}
