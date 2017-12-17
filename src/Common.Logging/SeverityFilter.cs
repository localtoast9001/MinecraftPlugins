//-----------------------------------------------------------------------
// <copyright file="SeverityFilter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.Logging
{
    /// <summary>
    /// A filter that logs messages more severe or in equal severity to a given level.
    /// </summary>
    /// <seealso cref="Common.Logging.MessageStreamFilter" />
    public class SeverityFilter : MessageStreamFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeverityFilter"/> class.
        /// </summary>
        /// <param name="level">The lowest level severity that should be logged.</param>
        /// <param name="inner">The inner stream.</param>
        public SeverityFilter(LogMessageSeverity level, ILogMessageStream inner)
            : base(inner)
        {
            this.Level = level;
        }

        /// <summary>
        /// Gets the lowest level of severity that should be logged.
        /// </summary>
        public LogMessageSeverity Level { get; private set; }

        /// <summary>
        /// Determines whether or not a message should be logged to the inner stream.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// A value indicating whether the message should be logged.
        /// </returns>
        protected override bool ShouldLog(LogMessage message)
        {
            return message.Severity <= this.Level;
        }
    }
}
