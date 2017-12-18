// -----------------------------------------------------------------------
// <copyright file="ProcessHost.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#define TRACE
namespace ConsoleHost.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Controlls execution and monitoring of a process.
    /// </summary>
    public class ProcessHost : IDisposable, IProcessHost
    {
        /// <summary>
        /// The process.
        /// </summary>
        private Process process;

        /// <summary>
        /// The stop command.
        /// </summary>
        private string stopCommand;

        /// <summary>
        /// The exited flag.
        /// </summary>
        private bool exited;

        /// <summary>
        /// The output consumer.
        /// </summary>
        private IMessageConsumer outputConsumer;

        /// <summary>
        /// The temporary queue.
        /// </summary>
        private List<Message> tempQueue = new List<Message>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHost" /> class.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="stopCommand">The stop command.</param>
        /// <param name="workingDirectory">The working directory.</param>
        public ProcessHost(
            string program,
            string arguments,
            string stopCommand,
            string workingDirectory = null)
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

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                startInfo.WorkingDirectory = workingDirectory;
            }

            this.process = Process.Start(startInfo);
            this.process.OutputDataReceived += new DataReceivedEventHandler(this.OnProcessOutputDataReceived);
            this.process.ErrorDataReceived += new DataReceivedEventHandler(this.OnProcessErrorDataReceived);
            this.process.Exited += new EventHandler(this.OnProcessExited);
            this.process.EnableRaisingEvents = true;
            this.process.BeginErrorReadLine();
            this.process.BeginOutputReadLine();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ProcessHost"/> class.
        /// </summary>
        ~ProcessHost()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the PID.
        /// </summary>
        public int Id
        {
            get
            {
                return this.process.Id;
            }
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        public string Name
        {
            get
            {
                return this.process.ProcessName;
            }
        }

        /// <summary>
        /// Registers the output consumer.
        /// </summary>
        /// <param name="outputConsumer">The output consumer.</param>
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (!this.exited)
            {
                Message stopMessage = new Message
                {
                    Text = this.stopCommand + Environment.NewLine,
                    Time = DateTime.UtcNow
                };

                this.Post(stopMessage);
                if (!this.process.WaitForExit(30000))
                {
                    this.process.Kill();
                }
            }
        }

        /// <summary>
        /// Posts the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Post(Message message)
        {
            if (!this.exited)
            {
                this.process.StandardInput.Write(message.Text);
            }
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
                this.process = null;
            }
        }

        /// <summary>
        /// Handles the Exited event of the process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnProcessExited(object sender, EventArgs e)
        {
            this.exited = true;
        }

        /// <summary>
        /// Handles the ErrorDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        private void OnProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e == null || e.Data == null)
            {
                return;
            }

            Message m = new Message { Time = DateTime.UtcNow, Text = e.Data, Severity = Severity.Error };
            this.InnerPost(m);
        }

        /// <summary>
        /// Handles the OutputDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        private void OnProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e == null || e.Data == null)
            {
                return;
            }

            Message m = new Message { Time = DateTime.UtcNow, Text = e.Data, Severity = Severity.Output };
            this.InnerPost(m);
        }

        /// <summary>
        /// Inner implementation of the Post call.
        /// </summary>
        /// <param name="m">The message.</param>
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
    }
}
