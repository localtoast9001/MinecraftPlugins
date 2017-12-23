// -----------------------------------------------------------------------
// <copyright file="AuthenticationProvider.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Owin.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;

    /// <summary>
    /// Common base class of all authentication providers.
    /// </summary>
    public abstract class AuthenticationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationProvider" /> class.
        /// </summary>
        /// <param name="next">The next function in the OWIN pipeline.</param>
        /// <param name="userRoleMapper">A callback function to add role claims to the user.</param>
        /// <exception cref="System.ArgumentNullException">next is null.</exception>
        protected AuthenticationProvider(
            AppFunc next,
            Func<ClaimsIdentity, Task> userRoleMapper)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            this.Next = next;
            this.UserRoleMapper = userRoleMapper;
        }

        /// <summary>
        /// Gets the user role mapper.
        /// </summary>
        public Func<ClaimsIdentity, Task> UserRoleMapper { get; private set; }

        /// <summary>
        /// Gets the next function in the OWIN pipeline.
        /// </summary>
        protected AppFunc Next { get; private set; }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>A task to monitor when the request is complete.</returns>
        public async Task ProcessRequest(IDictionary<string, object> environment)
        {
            OwinContext context = new OwinContext(environment);
            ClaimsPrincipal user = context.RequestUser;
            if (user == null)
            {
                if (this.ShouldChallenge(context))
                {
                    await this.Challenge(context);
                    return;
                }

                ClaimsIdentity userIdentity = await this.AuthenticateUser(context);
                if (userIdentity != null && this.UserRoleMapper != null)
                {
                    await this.UserRoleMapper(userIdentity);
                }

                context.RequestUser = userIdentity != null ?
                    new ClaimsPrincipal(userIdentity) :
                    null;
            }

            // Allow an authorization filter to handle anonymous access.
            await this.Next(environment);
        }

        /// <summary>
        /// Determines whether or not a challenge should be returned to the client.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A value indicating whether the provider should perform a challenge with the client.</returns>
        protected abstract bool ShouldChallenge(OwinContext context);

        /// <summary>
        /// Authenticates the user based on the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>An awaitable task which returns the user identity when complete.</returns>
        protected abstract Task<ClaimsIdentity> AuthenticateUser(OwinContext context);

        /// <summary>
        /// Responds to the client request with a challenge to authenticate instead of continuing
        /// normal processing of the pipeline.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>An awaitable task for the response.</returns>
        protected abstract Task Challenge(OwinContext context);
    }
}
