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
    using Common.Logging;
    using ConsoleHost;
    using ConsoleHost.Utility;
    using Minecraft.Management;

    /// <summary>
    /// Wraps the process and adds management abstractions.
    /// </summary>
    public class ServerHost : IDisposable
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILogMessageStream log;

        /// <summary>
        /// The process.
        /// </summary>
        private readonly ProcessHost process;

        /// <summary>
        /// The message consumer.
        /// </summary>
        private readonly MessageConsumer messageConsumer;

        /// <summary>
        /// The messages.
        /// </summary>
        private readonly List<MinecraftLogEntry> messages = new List<MinecraftLogEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerHost" /> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="minecraftDirectory">The minecraft directory.</param>
        /// <param name="jarFileName">Name of the jar file.</param>
        /// <exception cref="System.ArgumentNullException">log</exception>
        public ServerHost(
            ILogMessageStream log,
            string minecraftDirectory,
            string jarFileName)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            this.log = log;
            this.messageConsumer = new MessageConsumer(this.log);
            this.messageConsumer.PostCallback = this.OnPost;

            string arguments = string.Format(
                "-Xmx1024M -Xms1024M -jar {0} nogui",
                jarFileName);
            this.process = new ProcessHost(
                "java.exe", 
                arguments, 
                "stop",
                minecraftDirectory);
            this.process.RegisterOutputConsumer(this.messageConsumer);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ServerHost"/> class.
        /// </summary>
        ~ServerHost()
        {
            this.Dispose(false);
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

        /// <summary>
        /// Says the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <exception cref="System.ArgumentNullException">text is null or empty.</exception>
        public void Say(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            string[] lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                Message message = new Message
                {
                    Text = "/say " + line + Environment.NewLine,
                    Severity = Severity.Output,
                    Time = DateTime.UtcNow
                };

                this.process.Post(message);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.process.Dispose();
            }
        }

        /// <summary>
        /// Called when an output message is posted.
        /// </summary>
        /// <param name="message">The message.</param>
        private void OnPost(MinecraftLogEntry message)
        {
            lock (this.messages)
            {
                this.messages.Add(message);
            }
        }

        /// <summary>
        /// Callback to consume messages.
        /// </summary>
        /// <seealso cref="ConsoleHost.IMessageConsumer" />
        private class MessageConsumer : IMessageConsumer
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MessageConsumer"/> class.
            /// </summary>
            /// <param name="log">The log.</param>
            /// <exception cref="System.ArgumentNullException">log is <c>null</c>.</exception>
            public MessageConsumer(ILogMessageStream log)
            {
                if (log == null)
                {
                    throw new ArgumentNullException("log");
                }

                this.Log = log;
            }

            /// <summary>
            /// Gets or sets the log.
            /// </summary>
            public ILogMessageStream Log { get; private set; }
            
            /// <summary>
            /// Gets or sets the post callback.
            /// </summary>
            public Action<MinecraftLogEntry> PostCallback { get; set; }

            /// <summary>
            /// Posts the specified message.
            /// </summary>
            /// <param name="message">The message.</param>
            public void Post(Message message)
            {
                this.Log.Log(LogMessage.Create(
                    message.Severity == Severity.Error ? LogMessageSeverity.Error : LogMessageSeverity.Information,
                    message.Text));
                try
                {
                    MinecraftLogEntry logEntry = MinecraftLogReader.ParseLine(message.Text);
                    if (this.PostCallback != null)
                    {
                        this.PostCallback(logEntry);
                    }
                }
                catch (Exception ex)
                {
                    this.Log.Log(LogMessage.Critical(ex.ToString()));
                }
            }
        }
    }
}
