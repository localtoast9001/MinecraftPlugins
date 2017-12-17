using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Minecraft.Management;

namespace MinecraftServer.Service
{
    internal class ServerApiController
    {
        private string rootPath;

        private ServerHost host;

        private List<Tuple<string, Func<HttpListenerContext, Task>>> routeTable = new List<Tuple<string, Func<HttpListenerContext, Task>>>();


        public ServerApiController(string rootPath, ServerHost host)
        {
            this.rootPath = rootPath;
            this.host = host;
            this.routeTable.Add(new Tuple<string, Func<HttpListenerContext, Task>>(
                "console",
                this.ProcessConsoleRequest));
        }

        public async Task ProcessRequest(HttpListenerContext context)
        {
            var response = context.Response;
            var request = context.Request;
            try
            {
                Func<HttpListenerContext, Task> routeFunc = null;
                foreach (var route in this.routeTable)
                {
                    string testRoute = this.rootPath + "/" + route.Item1;
                    if (request.Url.AbsolutePath.StartsWith(testRoute))
                    {
                        string remainder = request.Url.AbsolutePath.Substring(testRoute.Length);
                        if (remainder.Length == 0 || remainder[0] != '/')
                        {
                            routeFunc = route.Item2;
                            break;
                        }
                    }
                }

                if (routeFunc != null)
                {
                    await routeFunc(context);
                }
                else
                {
                    response.ContentType = "text/plain";
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(response.OutputStream)))
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("type");
                    writer.WriteValue(ex.GetType().FullName);
                    writer.WritePropertyName("message");
                    writer.WriteValue(ex.Message);
                    if (request.IsLocal)
                    {
                        writer.WritePropertyName("details");
                        writer.WriteValue(ex.ToString());
                    }

                    writer.WriteEndObject();
                }
            }
        }

        private Task ProcessConsoleRequest(HttpListenerContext context)
        {
            return Task.Run(() =>
            {
                var request = context.Request;
                var response = context.Response;
                if (string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    IEnumerable<MinecraftLogEntry> messages = this.host.Messages;
                    using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(response.OutputStream)))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(writer, messages);
                    }

                    response.ContentType = "application/json";
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    response.Close();
                }
            });
        }
    }
}
