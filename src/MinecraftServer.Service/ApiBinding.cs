using Common.Web.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Service
{
    internal abstract class ApiBinding
    {
        public string HttpMethod { get; protected set; }

        public string Path { get; set; }

        public string[] AllowedRoles { get; set; }

        public bool AllowAnonymous { get; set; }

        public bool CheckAccess(ClaimsPrincipal user)
        {
            if (this.AllowAnonymous)
            {
                return true;
            }

            if (user == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            foreach (string allowedRole in this.AllowedRoles ?? new string[0])
            {
                if (user.IsInRole(allowedRole))
                {
                    return true;
                }
            }

            return false;
        }

        public abstract Task Dispatch(OwinContext context);
    }
}
