// -----------------------------------------------------------------------
// <copyright file="ClientCertificateAuthenticationProvider.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Owin.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;

    /// <summary>
    /// An authentication provider that maps a client certificate to a claims user.
    /// </summary>
    /// <seealso cref="Common.Web.Owin.Middleware.AuthenticationProvider" />
    public class ClientCertificateAuthenticationProvider : AuthenticationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCertificateAuthenticationProvider" /> class.
        /// </summary>
        /// <param name="next">The next function in the OWIN pipeline.</param>
        /// <param name="userRoleMapper">A callback function to add role claims to the user.</param>
        private ClientCertificateAuthenticationProvider(
            AppFunc next,
            Func<ClaimsIdentity, Task> userRoleMapper)
            : base(next, userRoleMapper)
        {
        }

        /// <summary>
        /// Gets a function to create the middleware provider.
        /// </summary>
        /// <param name="startupProperties">The startup properties.</param>
        /// <param name="userRoleMapper">A callback function to add role claims to the user.</param>
        /// <returns>
        /// The function to create the provider.
        /// </returns>
        public static MidFunc Create(
            IDictionary<string, object> startupProperties,
            Func<ClaimsIdentity, Task> userRoleMapper)
        {
            return (next) =>
            {
                ClientCertificateAuthenticationProvider provider = new ClientCertificateAuthenticationProvider(
                    next, 
                    userRoleMapper);
                return provider.ProcessRequest;
            };
        }

        /// <summary>
        /// Authenticates the user based on the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// An awaitable task which returns the user identity when complete.
        /// </returns>
        protected override Task<ClaimsIdentity> AuthenticateUser(OwinContext context)
        {
            return Task.Run(() =>
            {
                X509Certificate clientCert = context.ClientCertificate;
                if (clientCert == null)
                {
                    return null;
                }

                Claim nameClaim = new Claim(ClaimTypes.Name, clientCert.Subject);
                Claim thumbprintClaim = new Claim(ClaimTypes.Thumbprint, clientCert.GetCertHashString());
                ClaimsIdentity identity = new ClaimsIdentity(
                    "Certificate",
                    ClaimTypes.Name, 
                    ClaimTypes.Role);
                identity.AddClaim(nameClaim);
                identity.AddClaim(thumbprintClaim);
                return identity;
            });
        }

        /// <summary>
        /// Determines whether or not a challenge should be returned to the client.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A value indicating whether the provider should perform a challenge with the client.
        /// </returns>
        protected override bool ShouldChallenge(OwinContext context)
        {
            return false;
        }

        /// <summary>
        /// Responds to the client request with a challenge to authenticate instead of continuing
        /// normal processing of the pipeline.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// An awaitable task for the response.
        /// </returns>
        protected override Task Challenge(OwinContext context)
        {
            return Task.Run(() => { });
        }
    }
}
