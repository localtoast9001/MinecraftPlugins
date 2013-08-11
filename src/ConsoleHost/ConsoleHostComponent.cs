//-----------------------------------------------------------------------
// <copyright file="ConsoleHostService.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ConsoleHost.Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Configuration;
    using System.Net;
    using System.IO;
    using ConsoleHost.Web;

    /// <summary>
    /// Service Component to process commands from the Windows SCM.
    /// </summary>
    public partial class ConsoleHostComponent
    {
        /// <summary>
        /// The process being controlled.
        /// </summary>
        private ProcessHost process;

        /// <summary>
        /// Monitors and controls the process.
        /// </summary>
        private ProcessMonitor monitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleHostComponent"/> class.
        /// </summary>
        public ConsoleHostComponent()
        {
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        public void Start(string[] args)
        {
            this.process = new ProcessHost(
                ConfigurationManager.AppSettings["Program"],
                ConfigurationManager.AppSettings["Arguments"],
                ConfigurationManager.AppSettings["StopCommand"]);

            this.monitor = new WebProcessMonitor(this.process);
            this.monitor.Start();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        public void Stop()
        {
            this.process.Stop();
            this.monitor.Stop();
        }
    }
}
