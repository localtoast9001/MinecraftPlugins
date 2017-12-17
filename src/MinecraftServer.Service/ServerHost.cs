// -----------------------------------------------------------------------
// <copyright file="ServerHost.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MinecraftServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ConsoleHost;
    using ConsoleHost.Utility;
    using Minecraft.Management;

    /// <summary>
    /// Wraps the process and adds management abstractios.
    /// </summary>
    public class ServerHost
    {
        /// <summary>
        /// The process.
        /// </summary>
        private ProcessHost process;

        private MessageConsumer messageConsumer = new MessageConsumer();

        private List<MinecraftLogEntry> messages = new List<MinecraftLogEntry>();

        public ServerHost(
            string minecraftDirectory,
            string jarFileName)
        {
            this.messageConsumer.PostCallback = this.OnPost;

            string arguments = string.Format(
                "-Xmx1024M -Xms1024M -jar {0} nogui",
                jarFileName);
            process = new ProcessHost(
                "java.exe", 
                arguments, 
                "stop",
                minecraftDirectory);
            process.RegisterOutputConsumer(this.messageConsumer);
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        public IEnumerable<MinecraftLogEntry> Messages
        {
            get
            {
                lock (this.messages)
                {
                    return this.messages.ToArray();
                }
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            this.process.Stop();
        }

        private void OnPost(MinecraftLogEntry message)
        {
            lock (this.messages)
            {
                this.messages.Add(message);
            }
        }

        private class MessageConsumer : IMessageConsumer
        {
            public Action<MinecraftLogEntry> PostCallback { get; set; }

            public void Post(Message message)
            {
                Console.WriteLine(message.Text);
                MinecraftLogEntry logEntry = MinecraftLogReader.ParseLine(message.Text);
                if (this.PostCallback != null)
                {
                    this.PostCallback(logEntry);
                }
            }
        }

        private class AsyncCommand : IAsyncResult
        {
            public bool IsCompleted { get; set; }

            public WaitHandle AsyncWaitHandle { get; set; }

            public object AsyncState { get; set; }

            public bool CompletedSynchronously { get; set; }
        }
    }
}
