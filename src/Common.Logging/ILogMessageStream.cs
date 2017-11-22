//-----------------------------------------------------------------------
// <copyright file="ILogMessageStream.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.Logging
{
    /// <summary>
    /// Marshalable interface to log messages.
    /// </summary>
    public interface ILogMessageStream
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Log(LogMessage message);
    }
}
