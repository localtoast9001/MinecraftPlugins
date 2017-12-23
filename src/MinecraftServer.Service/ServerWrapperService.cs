// -----------------------------------------------------------------------
// <copyright file="ServerWrapperService.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MinecraftServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Logging;

    /// <summary>
    /// The Service that wraps the Minecraft program.
    /// </summary>
    /// <seealso cref="System.ServiceProcess.ServiceBase" />
    public partial class ServerWrapperService : ServiceBase
    {
        /// <summary>
        /// The component.
        /// </summary>
        private ServerWrapperComponent component = new ServerWrapperComponent();

        /// <summary>
        /// The log stream.
        /// </summary>
        private SourceLogMessageStream log = new SourceLogMessageStream();

        /// <summary>
        /// The file log.
        /// </summary>
        private FileLogMessageStream fileLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerWrapperService"/> class.
        /// </summary>
        public ServerWrapperService()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            EventLogMessageStream eventLogStream = new EventLogMessageStream(this.serviceEventLog);
            this.log.Targets.Add(new SeverityFilter(LogMessageSeverity.Error, eventLogStream));
            this.fileLog = FileLogMessageStream.Create(System.Configuration.ConfigurationManager.AppSettings["LogRootPath"]);
            this.log.Targets.Add(this.fileLog);
            this.component.Start(this.log, args);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            this.component.Stop();
            if (this.fileLog != null)
            {
                this.fileLog.Dispose();
            }

            this.component.Dispose();
        }
    }
}
