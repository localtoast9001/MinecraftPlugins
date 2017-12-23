using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Web.Owin;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace MinecraftServer.Service
{
    internal class GetApiBinding<T> : ApiBinding
    {
        public GetApiBinding(Func<OwinContext, Task<T>> implementation)
        {
            this.Implementation = implementation;
            this.HttpMethod = "GET";
        }

        public Func<OwinContext, Task<T>> Implementation { get; private set; }

        public override async Task Dispatch(OwinContext context)
        {
            T result = await this.Implementation(context);

            using (JsonTextWriter writer = new JsonTextWriter(
                new StreamWriter(context.ResponseBody)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, result);
            }

            context.ResponseStatusCode = (int)HttpStatusCode.OK;
            context.ResponseHeaders["ContentType"] = new string[] { "application/json" };
        }
    }
}
