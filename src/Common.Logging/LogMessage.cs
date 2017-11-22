//-----------------------------------------------------------------------
// <copyright file="LogMessage.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.Logging
{
    using System;

    /// <summary>
    /// The severity of a log message.
    /// </summary>
    public enum LogMessageSeverity
    {
        /// <summary>
        /// The critical severity level.
        /// </summary>
        Critical,

        /// <summary>
        /// The error severity level.
        /// </summary>
        Error,

        /// <summary>
        /// The warning severity level.
        /// </summary>
        Warning,

        /// <summary>
        /// The information severity level.
        /// </summary>
        Information,

        /// <summary>
        /// The verbose severity level.
        /// </summary>
        Verbose,

        /// <summary>
        /// The debug severity level.
        /// </summary>
        Debug
    }

    /// <summary>
    /// A structured log message.
    /// </summary>
    [Serializable]
    public class LogMessage
    {
        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        public LogMessageSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Creates the message with specified severity.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="text">The text message.</param>
        /// <returns>A new instance of the <see cref="LogMessage"/> class.</returns>
        public static LogMessage Create(LogMessageSeverity severity, string text)
        {
            return new LogMessage
            {
                Severity = severity,
                Timestamp = DateTime.UtcNow,
                Text = text
            };
        }

        /// <summary>
        /// Creates a critical log message with the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A new instance of the <see cref="LogMessage"/> class.</returns>
        public static LogMessage Critical(string text)
        {
            return Create(LogMessageSeverity.Critical, text);
        }

        /// <summary>
        /// Creates an error log message with the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A new instance of the <see cref="LogMessage"/> class.</returns>
        public static LogMessage Error(string text)
        {
            return Create(LogMessageSeverity.Error, text);
        }

        /// <summary>
        /// Creates an information log message with the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A new instance of the <see cref="LogMessage"/> class.</returns>
        public static LogMessage Information(string text)
        {
            return Create(LogMessageSeverity.Information, text);
        }
    }
}
