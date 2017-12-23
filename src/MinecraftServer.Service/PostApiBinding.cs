using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Web.Owin;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace MinecraftServer.Service
{
    internal class PostApiBinding<T> : ApiBinding
    {
        public PostApiBinding(Func<OwinContext, T, Task> implementation)
        {
            this.HttpMethod = "POST";
            this.Implementation = implementation;
        }

        public Func<OwinContext, T, Task> Implementation { get; private set; }

        public override async Task Dispatch(OwinContext context)
        {
            Stream requestStream = context.RequestBody;
            T requestBody = default(T);
            using (JsonTextReader reader = new JsonTextReader(new StreamReader(requestStream)))
            {
                JsonSerializer serializer = new JsonSerializer();
                requestBody = serializer.Deserialize<T>(reader);
            }

            await this.Implementation(context, requestBody);
            context.ResponseStatusCode = (int)HttpStatusCode.OK;
            context.ResponseHeaders["ContentType"] = new string[] { "application/json" };
        }
    }
}
