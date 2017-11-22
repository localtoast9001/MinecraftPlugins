// -----------------------------------------------------------------------
// <copyright file="Message.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost
{
    using System;

    /// <summary>
    /// Severity of the message.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// the message has output severity
        /// </summary>
        Output,

        /// <summary>
        /// The message has Error severity.
        /// </summary>
        Error
    }

    /// <summary>
    /// Chunk of input or output sent to or received from the program.
    /// </summary>
    [Serializable]
    public class Message
    {
        /// <summary>
        /// Gets or sets the severity of the message.
        /// </summary>
        public Severity Severity { get; set; }

        /// <summary>
        /// Gets or sets the time the message entered the system.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the text that was communicated.
        /// </summary>
        public string Text { get; set; }
    }
}
