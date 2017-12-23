using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Web.Owin;
using System.Net;
using System.Security.Claims;

namespace MinecraftServer.Service
{
    internal abstract class ApiController
    {
        private Collection<ApiBinding> bindings = new Collection<ApiBinding>();

        public string RequestPathBase { get; set; }

        public Collection<ApiBinding> Bindings
        {
            get { return this.bindings; }
        }

        public async Task ProcessRequest(IDictionary<string, object> environment)
        {
            OwinContext context = new OwinContext(environment);
            string method = context.RequestMethod;
            string requestPathBase = this.RequestPathBase ?? string.Empty;
            ApiBinding matchedBinding = null;
            foreach (ApiBinding binding in this.bindings)
            {
                if (!string.Equals(binding.HttpMethod, method, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (context.RequestPath.StartsWith(requestPathBase + binding.Path, StringComparison.OrdinalIgnoreCase))
                {
                    string remainder = context.RequestPath.Substring(requestPathBase.Length + binding.Path.Length);
                    if (remainder.Length == 0 || remainder[0] == '/')
                    {
                        matchedBinding = binding;
                        break;
                    }
                }
            }

            if (matchedBinding != null)
            {
                ClaimsPrincipal user = context.RequestUser;
                if (!matchedBinding.CheckAccess(user))
                {
                    context.ResponseStatusCode = (int)HttpStatusCode.Forbidden;
                    context.ResponseHeaders["ContentType"] = new string[] { "text/plain" };
                    return;
                }

                await matchedBinding.Dispatch(context);
            }
            else
            {
                context.ResponseStatusCode = (int)HttpStatusCode.NotFound;
                context.ResponseHeaders["ContentType"] = new string[] { "text/plain" };
            }
        }
    }
}
