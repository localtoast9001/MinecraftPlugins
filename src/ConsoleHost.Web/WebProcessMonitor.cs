// -----------------------------------------------------------------------
// <copyright file="WebProcessMonitor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Net;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebProcessMonitor : ProcessMonitor
    {
        private MemoryMessageStream outputStream = new MemoryMessageStream();

        private WebServer webServer;

        public WebProcessMonitor(IProcessHost process)
            : base(process)
        {
            process.RegisterOutputConsumer(this.outputStream);
            this.webServer = new WebServer(OnRequest);
        }

        public override void Start()
        {
            this.webServer.Start();
        }

        public override void Stop()
        {
            this.webServer.Stop();
        }

        private void OnRequest(HttpListenerContext context)
        {
            if (string.Compare(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string postData = null;
                using (StreamReader reader = new StreamReader(context.Request.InputStream))
                {
                    postData = reader.ReadToEnd();
                }

                string[][] fieldPairs = postData.Split('&').Select(e => e.Split('=').ToArray()).ToArray();
                string[] commandPair = fieldPairs.Where(e => string.CompareOrdinal(e[0], "command") == 0).FirstOrDefault();
                if (commandPair != null)
                {
                    Process.Post(new Message { Text = commandPair[1] + "\r\n", Time = DateTime.UtcNow });
                }
            }

            string style = @"    <style type=""text/css"">
        a.message
        {
            font-family: monospace;
            white-space: pre;
        }
        
        a.message i
        {
            float: left;
            clear: none;
            display: none;
        }

        a.message span:hover
        {
            background-color: #ccccff;
        }
        
    </style>
";
            string template = "<html><head><title>Console Host</title>{1}</head><body><p>{0}</p><form method='post'><input type='text' name='command' /><input type='submit' value='submit' /></form></body></html>";
            string doc = string.Format(
                template,
                string.Join("<br/>", this.outputStream.Messages.Select(e => FormatMessageHtml(e))),
                style);
            context.Response.ContentType = "text/html";
            context.Response.StatusCode = 200;
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.Write(doc);
            }
        }

        private static string FormatMessageHtml(Message m)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<a class='message' title='{0}'>", m.Time);
            sb.AppendFormat(
                m.Severity == Severity.Error ? "<b><span>{0}</span></b>" : "<span>{0}</span>",
                System.Web.HttpUtility.HtmlEncode(m.Text));
            sb.Append("</a>");
            return sb.ToString();
        }
    }
}
