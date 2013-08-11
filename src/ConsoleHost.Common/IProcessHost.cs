// -----------------------------------------------------------------------
// <copyright file="IProcessHost.cs" company="">
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
    public interface IProcessHost : IMessageConsumer
    {
        int Id { get; }
        string Name { get; }
        void RegisterOutputConsumer(IMessageConsumer outputConsumer);
    }
}
