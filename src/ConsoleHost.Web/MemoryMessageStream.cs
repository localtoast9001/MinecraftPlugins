// -----------------------------------------------------------------------
// <copyright file="MemoryMessageStream.cs" company="Jon Rowlett">
// Copyright (C) 2013 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Message stream that uses memory as storage.
    /// </summary>
    public class MemoryMessageStream : MarshalByRefObject, IMessageConsumer
    {
        /// <summary>
        /// Max number of messages to save in the buffer.
        /// </summary>
        private const int MaxMessages = 100;

        /// <summary>
        /// List of messages in the buffer.
        /// </summary>
        private List<Message> messageList = new List<Message>();

        /// <summary>
        /// Gets the messages.
        /// </summary>
        public IEnumerable<Message> Messages
        {
            get
            {
                lock (this.messageList)
                {
                    return this.messageList.ToArray();
                }
            }
        }

        /// <summary>
        /// Posts the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Post(Message message)
        {
            lock (this.messageList)
            {
                this.messageList.Add(message);
                if (this.messageList.Count > MaxMessages)
                {
                    this.messageList.RemoveAt(0);
                }
            }
        }
    }
}
