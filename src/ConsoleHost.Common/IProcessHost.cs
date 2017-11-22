// -----------------------------------------------------------------------
// <copyright file="IProcessHost.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Interface for a running process.
    /// </summary>
    public interface IProcessHost : IMessageConsumer
    {
        /// <summary>
        /// Gets the PID.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Registers the output consumer.
        /// </summary>
        /// <param name="outputConsumer">The output consumer.</param>
        void RegisterOutputConsumer(IMessageConsumer outputConsumer);
    }
}
