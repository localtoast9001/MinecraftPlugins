//-----------------------------------------------------------------------
// <copyright file="SourceLogMessageStream.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.Logging
{
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// A log message stream that can be bound to multiple target streams.
    /// </summary>
    /// <seealso cref="Common.Logging.ILogMessageStream" />
    public class SourceLogMessageStream : ILogMessageStream
    {
        /// <summary>
        /// The targets.
        /// </summary>
        private Collection<ILogMessageStream> targets = new Collection<ILogMessageStream>();

        /// <summary>
        /// Gets the targets.
        /// </summary>
        public Collection<ILogMessageStream> Targets
        {
            get { return this.targets; }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(LogMessage message)
        {
            ILogMessageStream[] targetArray = this.targets.ToArray();
            foreach (ILogMessageStream target in targetArray)
            {
                target.Log(message);
            }
        }
    }
}
