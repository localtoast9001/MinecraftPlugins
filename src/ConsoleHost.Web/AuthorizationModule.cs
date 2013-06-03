// -----------------------------------------------------------------------
// <copyright file="AuthorizationModule.cs" company="Jon Rowlett">
// Copyright (C) 2013 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Http Module to process authorization.
    /// </summary>
    public class AuthorizationModule : IHttpModule
    {
        /// <summary>
        /// the authorization config.
        /// </summary>
        private Configuration.AuthorizationSection config;

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, 
        /// and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.AuthorizeRequest += new EventHandler(this.AuthorizeRequestHandler);
            Configuration.RootSection root = (Configuration.RootSection)ConfigurationManager.GetSection("consolehost.web");
            this.config = root.Authorization;
        }

        /// <summary>
        /// Called to authorize a request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>whether or not the cert is authorized.</returns>
        protected virtual bool OnAuthorizeRequest(HttpContext context)
        {
            HttpClientCertificate cert = context.Request.ClientCertificate;
            if (!cert.IsPresent)
            {
                return false;
            }

            X509Certificate2 certificate = new X509Certificate2(cert.Certificate);
            bool allow = MatchAny(certificate, this.config.Allow.OfType<Configuration.AuthorizationElement>());
            bool deny = MatchAny(certificate, this.config.Deny.OfType<Configuration.AuthorizationElement>());
            
            return allow && !deny;
        }

        /// <summary>
        /// Matches any element in the config against the certificate.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>If one or more matches in the elements are found.</returns>
        private static bool MatchAny(X509Certificate2 cert, IEnumerable<Configuration.AuthorizationElement> elements)
        {
            foreach (Configuration.AuthorizationElement element in elements)
            {
                switch (element.Type)
                {
                    case Configuration.AuthorizationType.Subject:
                        {
                            X500DistinguishedName testValue = new X500DistinguishedName(
                                element.Value, 
                                X500DistinguishedNameFlags.UseCommas);
                            if (string.CompareOrdinal(testValue.Name, cert.SubjectName.Name) == 0)
                            {
                                return true;
                            }
                        } 
                        
                        break;
                    case Configuration.AuthorizationType.Thumbprint:
                        {
                            if (string.CompareOrdinal(cert.Thumbprint, element.Value) == 0)
                            {
                                return true;
                            }
                        } 
                        
                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Sends the access denied response.
        /// </summary>
        /// <param name="response">The response.</param>
        private static void SendAccessDeniedResponse(HttpResponse response)
        {
            response.StatusCode = 403;
            response.StatusDescription = "Access Denied.";
            response.ContentType = "text/html";
            response.Write("<html><head><title>Access Denied</title></head><body><h1>Access Denied</h1></body></html>");
            response.End();
        }

        /// <summary>
        /// Callback to Authorize the request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AuthorizeRequestHandler(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            if (!this.OnAuthorizeRequest(context))
            {
                SendAccessDeniedResponse(context.Response);
            }
        }
    }
}
