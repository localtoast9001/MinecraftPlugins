// -----------------------------------------------------------------------
// <copyright file="ProcessMonitor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class ProcessMonitor
    {
        protected IProcessHost Process { get; private set; }
        protected ProcessMonitor(IProcessHost process)
        {
            this.Process = process;
        }

        public abstract void Start();
        public abstract void Stop();
    }
}
