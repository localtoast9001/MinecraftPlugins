// -----------------------------------------------------------------------
// <copyright file="Default.aspx.cs" company="Jon Rowlett">
// Copyright (C) 2013 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Default page the runs the console.
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        /// <summary>
        /// Gets or sets The display of output.
        /// </summary>
        protected Repeater Display { get; set; }

        /// <summary>
        /// Gets or sets Text box to submit commands.
        /// </summary>
        protected TextBox CommandTextBox { get; set; }

        /// <summary>
        /// Gets or sets The submit button.
        /// </summary>
        protected Button SubmitButton { get; set; }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var clientCert = this.Request.ClientCertificate;
            if (clientCert == null || !clientCert.IsPresent)
            {
                this.Response.StatusCode = 403;
                this.Response.SuppressContent = true;
                this.Response.StatusDescription = "Client cert required.";
                this.Response.End();
            }

            MemoryMessageStream outputStream = AppDomain.CurrentDomain.GetData(".output") as MemoryMessageStream;
            if (outputStream != null)
            {
                this.Display.DataSource = outputStream.Messages;
                this.DataBind();
            }
        }

        /// <summary>
        /// Called when [submit button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnSubmitButtonClick(object sender, EventArgs e)
        {
            ConsoleHost.IProcessHost processHost = AppDomain.CurrentDomain.GetData(".processHost") as IProcessHost;
            if (processHost != null)
            {
                Message m = new Message 
                { 
                    Severity = Severity.Output, 
                    Text = this.CommandTextBox.Text + "\r\n", 
                    Time = DateTime.UtcNow 
                };
                processHost.Post(m);

                MemoryMessageStream outputStream = AppDomain.CurrentDomain.GetData(".output") as MemoryMessageStream;
                if (outputStream != null)
                {
                    this.Display.DataSource = outputStream.Messages;
                    this.DataBind();
                }
            }
        }
    }
}
