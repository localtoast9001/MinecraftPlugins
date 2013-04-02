// -----------------------------------------------------------------------
// <copyright file="MemoryMessageStream.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemoryMessageStream : IMessageConsumer
    {
        const int MaxMessages = 100;
        List<Message> messageList = new List<Message>();

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
    }
}
