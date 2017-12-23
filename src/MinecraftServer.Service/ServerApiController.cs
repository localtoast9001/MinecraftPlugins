using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Web.Owin;
using Newtonsoft.Json;
using Minecraft.Management;
using System.Security.Claims;

namespace MinecraftServer.Service
{
    internal class ServerApiController : ApiController
    {
        private ServerHost host;

        public ServerApiController(ServerHost host)
        {
            this.host = host;
            this.RequestPathBase = "/console";
            GetApiBinding<IEnumerable<MinecraftLogEntry>> getBinding = new GetApiBinding<IEnumerable<MinecraftLogEntry>>(
                this.GetMessages)
            {
                Path = string.Empty,
                AllowedRoles = new string[] { "User" }
            };
            PostApiBinding<string> sayBinding = new PostApiBinding<string>(this.Say)
            {
                Path = "/say",
                AllowedRoles = new string[] { "User" }
            };

            this.Bindings.Add(getBinding);
            this.Bindings.Add(sayBinding);
        }

        protected Task<IEnumerable<MinecraftLogEntry>> GetMessages(OwinContext context)
        {
            return Task.Run(() => this.host.Messages);
        }

        protected Task Say(OwinContext context, string text)
        {
            return Task.Run(() => this.host.Say(text));
        }
    }
}
