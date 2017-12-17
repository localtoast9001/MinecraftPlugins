//-----------------------------------------------------------------------
// <copyright file="MessageStreamFilter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.Logging
{
    using System;

    /// <summary>
    /// Base class for filters that control which messages are logged to an inner stream.
    /// </summary>
    /// <seealso cref="Common.Logging.ILogMessageStream" />
    public abstract class MessageStreamFilter : ILogMessageStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageStreamFilter"/> class.
        /// </summary>
        /// <param name="inner">The inner stream.</param>
        /// <exception cref="ArgumentNullException">The value of <c>inner</c> is <c>null</c>.</exception>
        protected MessageStreamFilter(ILogMessageStream inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            this.Inner = inner;
        }

        /// <summary>
        /// Gets the inner stream.
        /// </summary>
        public ILogMessageStream Inner { get; private set; }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(LogMessage message)
        {
            if (this.ShouldLog(message))
            {
                this.Inner.Log(message);
            }
        }

        /// <summary>
        /// Determines whether or not a message should be logged to the inner stream.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A value indicating whether the message should be logged.</returns>
        protected abstract bool ShouldLog(LogMessage message);
    }
}
