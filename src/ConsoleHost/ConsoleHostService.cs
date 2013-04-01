//-----------------------------------------------------------------------
// <copyright file="ConsoleHostService.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ConsoleHost
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;

    /// <summary>
    /// Service Component to process commands from the Windows SCM.
    /// </summary>
    public partial class ConsoleHostService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleHostService"/> class.
        /// </summary>
        public ConsoleHostService()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
        }
    }
}
