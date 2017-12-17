//-----------------------------------------------------------------------
// <copyright file="ConsoleLogMessageStream.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.Logging
{
    using System;

    /// <summary>
    /// Writes log messages to the console.
    /// </summary>
    /// <seealso cref="Common.Logging.ILogMessageStream" />
    public class ConsoleLogMessageStream : ILogMessageStream
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(LogMessage message)
        {
            if (message == null)
            {
                return;
            }

            Console.WriteLine(
                "{0}: {1}: {2}",
                message.Timestamp,
                message.Severity,
                message.Text);
        }
    }
}
