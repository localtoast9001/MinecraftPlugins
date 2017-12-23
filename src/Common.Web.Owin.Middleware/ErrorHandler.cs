// -----------------------------------------------------------------------
// <copyright file="ErrorHandler.cs" company="Jon Rowlett">
// Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Owin.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using BuildFunc = System.Action<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>>;
    using MidFactory = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>;
    using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;

    /// <summary>
    /// Catches exceptions and formats responses to the client.
    /// </summary>
    public class ErrorHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandler"/> class.
        /// </summary>
        /// <param name="next">The next processor in the chain.</param>
        private ErrorHandler(AppFunc next)
        {
            this.Next = next;
        }

        /// <summary>
        /// Gets or sets the next processor in the chain.
        /// </summary>
        private AppFunc Next { get; set; }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="startupProperties">The startup properties.</param>
        /// <returns>The Function to create the handler.</returns>
        public static MidFunc Create(IDictionary<string, object> startupProperties)
        {
            return (next) =>
            {
                ErrorHandler handler = new ErrorHandler(next);
                return handler.ProcessRequest;
            };
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>A task for the request.</returns>
        public async Task ProcessRequest(
            IDictionary<string, object> environment)
        {
            OwinContext context = new OwinContext(environment);
            try
            {
                await this.Next(environment);
            }
            catch (Exception ex)
            {
                IDictionary<string, string[]> responseHeaders = context.ResponseHeaders;
                responseHeaders["ContentType"] = new string[] { "application/json" };
                context.ResponseStatusCode = (int)HttpStatusCode.InternalServerError;
                using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(context.ResponseBody)))
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("type");
                    writer.WriteValue(ex.GetType().FullName);
                    writer.WritePropertyName("message");
                    writer.WriteValue(ex.Message);
                    if (context.IsLocal)
                    {
                        writer.WritePropertyName("details");
                        writer.WriteValue(ex.ToString());
                    }

                    writer.WriteEndObject();
                }
            }
        }
    }
}
