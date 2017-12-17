//-----------------------------------------------------------------------
// <copyright file="EventLogMessageStream.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.Logging
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Writes log messages to the Windows event log.
    /// </summary>
    /// <seealso cref="Common.Logging.ILogMessageStream" />
    public class EventLogMessageStream : ILogMessageStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogMessageStream"/> class.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <exception cref="ArgumentNullException">The value of <c>eventLog</c> is <c>null</c>.</exception>
        public EventLogMessageStream(EventLog eventLog)
        {
            if (eventLog == null)
            {
                throw new ArgumentNullException("eventLog");
            }

            this.EventLog = eventLog;
        }

        /// <summary>
        /// Gets the event log.
        /// </summary>
        public EventLog EventLog { get; private set; }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(LogMessage message)
        {
            EventLogEntryType entryType = Convert(message.Severity);
            string text = string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "[{0}]: {1}",
                message.Severity,
                message.Text);
            this.EventLog.WriteEntry(text, entryType);
        }

        /// <summary>
        /// Converts the specified severity.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <returns>The converted entry type.</returns>
        private static EventLogEntryType Convert(LogMessageSeverity severity)
        {
            switch (severity)
            {
                case LogMessageSeverity.Critical:
                    return EventLogEntryType.Error;
                case LogMessageSeverity.Error:
                    return EventLogEntryType.Error;
                case LogMessageSeverity.Warning:
                    return EventLogEntryType.Warning;
                case LogMessageSeverity.Information:
                    return EventLogEntryType.Information;
                case LogMessageSeverity.Verbose:
                    return EventLogEntryType.Information;
                default:
                    return EventLogEntryType.Information;
            }
        }
    }
}
