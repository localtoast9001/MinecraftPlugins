// -----------------------------------------------------------------------
// <copyright file="ProcessHost.cs" company="">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Controlls execution and monitoring of a process.
    /// </summary>
    internal class ProcessHost : IDisposable, IProcessHost
    {
        private Process process;
        private string stopCommand;
        private bool exited;
        private IMessageConsumer outputConsumer;
        private List<Message> tempQueue = new List<Message>();

        public int Id
        {
            get
            {
                return this.process.Id;
            }
        }

        public string Name
        {
            get
            {
                return this.process.ProcessName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHost"/> class.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="stopCommand">The stop command.</param>
        public ProcessHost(
            string program,
            string arguments,
            string stopCommand)
        {
            this.stopCommand = stopCommand;
            ProcessStartInfo startInfo = new ProcessStartInfo
            { 
                Arguments = arguments,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true, 
                RedirectStandardInput = true, 
                UseShellExecute = false, 
                FileName = program
            };

            this.process = Process.Start(startInfo);
            this.process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
            this.process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
            this.process.Exited += new EventHandler(process_Exited);
            this.process.EnableRaisingEvents = true;
            this.process.BeginErrorReadLine();
            this.process.BeginOutputReadLine();
        }

        public void RegisterOutputConsumer(IMessageConsumer outputConsumer)
        {
            lock (this.tempQueue)
            {
                this.outputConsumer = outputConsumer;
                foreach (var m in this.tempQueue)
                {
                    this.outputConsumer.Post(m);
                }

                this.tempQueue.Clear();
            }
        }

        void process_Exited(object sender, EventArgs e)
        {
            this.exited = true;
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e == null || e.Data == null)
            {
                return;
            }

            Message m = new Message { Time = DateTime.UtcNow, Text = e.Data, Severity = Severity.Error };
            this.InnerPost(m);
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e == null || e.Data == null)
            {
                return;
            }

            Message m = new Message { Time = DateTime.UtcNow, Text = e.Data, Severity = Severity.Output };
            this.InnerPost(m);
        }

        private void InnerPost(Message m)
        {
            lock (this.tempQueue)
            {
                if (this.outputConsumer != null)
                {
                    this.outputConsumer.Post(m);
                }
                else
                {
                    this.tempQueue.Add(m);
                }
            }
        }

        public void Dispose()
        {
            this.process.Dispose();
            this.process = null;
        }

        public void Stop()
        {
            if (!this.exited)
            {
                Message stopMessage = new Message
                {
                    Text = stopCommand + "\r\n", 
                    Time = DateTime.UtcNow
                };

                this.Post(stopMessage);
                if (!this.process.WaitForExit(30000))
                {
                    this.process.Kill();
                }
            }
        }

        public void Post(Message message)
        {
            if (!this.exited)
            {
                 this.process.StandardInput.Write(message.Text);
            }
        }
    }
}
