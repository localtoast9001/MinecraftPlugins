//-----------------------------------------------------------------------
// <copyright file="FileLogMessageStream.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// A message stream that logs to files.
    /// </summary>
    /// <seealso cref="Common.Logging.ILogMessageStream" />
    /// <seealso cref="System.IDisposable" />
    public class FileLogMessageStream : ILogMessageStream, IDisposable
    {
        /// <summary>
        /// The root path of the log files.
        /// </summary>
        private string rootPath;

        /// <summary>
        /// The synchronize root.
        /// </summary>
        private object syncRoot = new object();

        /// <summary>
        /// The messages.
        /// </summary>
        private Queue<LogMessage> messages = new Queue<LogMessage>();

        /// <summary>
        /// The pending messages event.
        /// </summary>
        private ManualResetEvent pendingMessagesEvent = new ManualResetEvent(false);

        /// <summary>
        /// The collector thread.
        /// </summary>
        private Thread collectorThread;

        /// <summary>
        /// The exit source.
        /// </summary>
        private CancellationTokenSource exitSource = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogMessageStream"/> class.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        private FileLogMessageStream(string rootPath)
        {
            this.rootPath = rootPath;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FileLogMessageStream"/> class.
        /// </summary>
        ~FileLogMessageStream()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Creates and starts the stream under the given root path.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <returns>A new instance of the <see cref="FileLogMessageStream"/> class.</returns>
        public static FileLogMessageStream Create(string rootPath)
        {
            FileLogMessageStream result = new FileLogMessageStream(rootPath);
            result.Start();
            return result;
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(LogMessage message)
        {
            lock (this.syncRoot)
            {
                this.messages.Enqueue(message);
                this.pendingMessagesEvent.Set();
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
            if (disposing && this.collectorThread != null)
            {
                this.exitSource.Cancel();
                this.collectorThread.Join();
                this.exitSource.Dispose();
                this.pendingMessagesEvent.Dispose();
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
            if (!Directory.Exists(this.rootPath))
            {
                Directory.CreateDirectory(this.rootPath);
            }

            this.collectorThread = new Thread(this.CollectorThreadStart);
            this.collectorThread.Start();
        }

        /// <summary>
        /// Thread procedure for the collector. 
        /// </summary>
        private void CollectorThreadStart()
        {
            WaitHandle[] waitHandles = new WaitHandle[]
            {
                this.exitSource.Token.WaitHandle,
                this.pendingMessagesEvent
            };

            Dictionary<DateTime, StreamWriter> openLogs = new Dictionary<DateTime, StreamWriter>();
            try
            {
                do
                {
                    int handleIndex = WaitHandle.WaitAny(waitHandles, TimeSpan.FromMinutes(1.0));

                    // flush pending messages before exiting.
                    LogMessage[] messagesSnap = null;
                    lock (this.syncRoot)
                    {
                        messagesSnap = this.messages.ToArray();
                        this.messages.Clear();
                        this.pendingMessagesEvent.Reset();
                    }

                    foreach (LogMessage message in messagesSnap)
                    {
                        DateTime logTime = message.Timestamp.Date;
                        StreamWriter openStream = null;
                        if (!openLogs.TryGetValue(logTime, out openStream))
                        {
                            string logFileName = string.Format(
                                System.Globalization.CultureInfo.InvariantCulture,
                                "{0:yyyy}-{0:MM}-{0:dd}.log",
                                logTime);
                            FileStream stream = new FileStream(
                                Path.Combine(this.rootPath, logFileName),
                                FileMode.OpenOrCreate,
                                FileAccess.Write,
                                FileShare.ReadWrite);
                            stream.Seek(0, SeekOrigin.End);
                            openStream = new StreamWriter(stream, Encoding.UTF8);
                            openLogs[logTime] = openStream;
                        }

                        openStream.WriteLine("[{0:O}]\t[{1}]:\t{2}", message.Timestamp, message.Severity, message.Text);
                    }

                    // perform file maint.
                    if (handleIndex != 1)
                    {
                        foreach (StreamWriter stream in openLogs.Values)
                        {
                            stream.Close();
                        }

                        openLogs.Clear();
                    }
                }
                while (!this.exitSource.IsCancellationRequested);
            }
            finally
            {
                foreach (StreamWriter stream in openLogs.Values)
                {
                    stream.Close();
                }

                openLogs.Clear();
            }
        }
    }
}
