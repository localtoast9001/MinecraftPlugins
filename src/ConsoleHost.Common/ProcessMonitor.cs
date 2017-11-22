// -----------------------------------------------------------------------
// <copyright file="ProcessMonitor.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost
{
    using System;

    /// <summary>
    /// Interface for an async monitor for a process.
    /// </summary>
    public abstract class ProcessMonitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMonitor"/> class.
        /// </summary>
        /// <param name="process">The process.</param>
        protected ProcessMonitor(IProcessHost process)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }

            this.Process = process;
        }

        /// <summary>
        /// Gets the process.
        /// </summary>
        protected IProcessHost Process { get; private set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public abstract void Stop();
    }
}
