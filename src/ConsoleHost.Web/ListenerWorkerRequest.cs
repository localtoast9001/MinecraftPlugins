// -----------------------------------------------------------------------
// <copyright file="ListenerWorkerRequest.cs" company="Jon Rowlett">
//  Copyright (C) 2013 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost.Web
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;

    /// <summary>
    /// Worker request used to adapt System.Net http APIs and System.Web APIs.
    /// </summary>
    public class ListenerWorkerRequest : SimpleWorkerRequest
    {
        /// <summary>
        /// The context for the request.
        /// </summary>
        private System.Net.HttpListenerContext context;

        /// <summary>
        /// The client cert received from the client.
        /// </summary>
        private X509Certificate2 clientCert;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListenerWorkerRequest"/> class.
        /// </summary>
        /// <param name="appVirtualDir">The app virtual directory.</param>
        /// <param name="context">The listener context.</param>
        public ListenerWorkerRequest(
            string appVirtualDir,
            System.Net.HttpListenerContext context)
            : base(
            context.Request.Url.AbsolutePath.Substring(appVirtualDir.Length), 
            context.Request.Url.Query, 
            new StreamWriter(context.Response.OutputStream))
        {
            this.context = context;
            this.clientCert = this.context.Request.GetClientCertificate();
        }

        /// <summary>
        /// Terminates the connection with the client.
        /// </summary>
        public override void CloseConnection()
        {
            base.CloseConnection();
        }

        /// <summary>
        /// Notifies the <see cref="T:System.Web.HttpWorkerRequest" /> that request processing for the current request is complete.
        /// </summary>
        public override void EndOfRequest()
        {
            base.EndOfRequest();
            this.context.Response.Close();
        }

        /// <summary>
        /// Returns the HTTP request verb.
        /// </summary>
        /// <returns>
        /// The HTTP verb for this request.
        /// </returns>
        public override string GetHttpVerbName()
        {
            return this.context.Request.HttpMethod;
        }

        /// <summary>
        /// Returns the HTTP version string of the request (for example, "HTTP/1.1").
        /// </summary>
        /// <returns>
        /// The HTTP version string returned in the request header.
        /// </returns>
        public override string GetHttpVersion()
        {
            return this.context.Request.ProtocolVersion.ToString();
        }

        /// <summary>
        /// Returns the server IP address of the interface on which the request was received.
        /// </summary>
        /// <returns>
        /// The server IP address of the interface on which the request was received.
        /// </returns>
        public override string GetLocalAddress()
        {
            return this.context.Request.LocalEndPoint.Address.ToString();
        }

        /// <summary>
        /// Returns the port number on which the request was received.
        /// </summary>
        /// <returns>
        /// The server port number on which the request was received.
        /// </returns>
        public override int GetLocalPort()
        {
            return this.context.Request.LocalEndPoint.Port;
        }

        /// <summary>
        /// Returns the query string specified in the request URL.
        /// </summary>
        /// <returns>
        /// The request query string.
        /// </returns>
        public override string GetQueryString()
        {
            return this.context.Request.Url.Query;
        }

        /// <summary>
        /// Returns the URL path contained in the header with the query string appended.
        /// </summary>
        /// <returns>
        /// The raw URL path of the request header.NoteThe returned URL is not normalized. 
        /// Using the URL for access control, or security-sensitive decisions can expose your application to 
        /// canonicalization security vulnerabilities.
        /// </returns>
        public override string GetRawUrl()
        {
            return this.context.Request.RawUrl;
        }

        /// <summary>
        /// Returns the IP address of the client.
        /// </summary>
        /// <returns>
        /// The client's IP address.
        /// </returns>
        public override string GetRemoteAddress()
        {
            return this.context.Request.RemoteEndPoint.Address.ToString();
        }

        /// <summary>
        /// Returns the client's port number.
        /// </summary>
        /// <returns>
        /// The client's port number.
        /// </returns>
        public override int GetRemotePort()
        {
            return this.context.Request.RemoteEndPoint.Port;
        }

        /// <summary>
        /// Returns the virtual path to the requested URI.
        /// </summary>
        /// <returns>
        /// The path to the requested URI.
        /// </returns>
        public override string GetUriPath()
        {
            return this.context.Request.Url.AbsolutePath;
        }

        /// <summary>
        /// Specifies the HTTP status code and status description of the response; for example, SendStatus(200, "Ok").
        /// </summary>
        /// <param name="statusCode">The status code to send</param>
        /// <param name="statusDescription">The status description to send.</param>
        public override void SendStatus(int statusCode, string statusDescription)
        {
            this.context.Response.StatusCode = statusCode;
            this.context.Response.StatusDescription = statusDescription;
        }

        /// <summary>
        /// Adds a standard HTTP header to the response.
        /// </summary>
        /// <param name="index">The header index. For example, <see cref="F:System.Web.HttpWorkerRequest.HeaderContentLength" />.</param>
        /// <param name="value">The header value.</param>
        public override void SendKnownResponseHeader(int index, string value)
        {
            this.context.Response.AddHeader(
                ListenerWorkerRequest.GetKnownResponseHeaderName(index), value);
        }

        /// <summary>
        /// Adds a nonstandard HTTP header to the response.
        /// </summary>
        /// <param name="name">The name of the header to send.</param>
        /// <param name="value">The value of the header.</param>
        public override void SendUnknownResponseHeader(string name, string value)
        {
            this.context.Response.AddHeader(name, value);
        }

        /// <summary>
        /// Adds the contents of a byte array to the response and specifies the number of bytes to send.
        /// </summary>
        /// <param name="data">The byte array to send.</param>
        /// <param name="length">The number of bytes to send.</param>
        public override void SendResponseFromMemory(byte[] data, int length)
        {
            this.context.Response.OutputStream.Write(data, 0, length);
        }

        /// <summary>
        /// Returns the standard HTTP request header that corresponds to the specified index.
        /// </summary>
        /// <param name="index">The index of the header. For example, the <see cref="F:System.Web.HttpWorkerRequest.HeaderAllow" /> 
        /// field.</param>
        /// <returns>
        /// The HTTP request header.
        /// </returns>
        public override string GetKnownRequestHeader(int index)
        {
            string value = this.context.Request.Headers[HttpWorkerRequest.GetKnownRequestHeaderName(index)];
            return value;
        }

        /// <summary>
        /// Returns a nonstandard HTTP request header value.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <returns>
        /// The header value.
        /// </returns>
        public override string GetUnknownRequestHeader(string name)
        {
            var value = this.context.Request.Headers[name];
            return value;
        }

        /// <summary>
        /// Get all nonstandard HTTP header name-value pairs.
        /// </summary>
        /// <returns>
        /// An array of header name-value pairs.
        /// </returns>
        public override string[][] GetUnknownRequestHeaders()
        {
            List<string[]> value = new List<string[]>();
            foreach (string key in this.context.Request.Headers.Keys)
            {
                value.Add(new string[] { key, this.context.Request.Headers[key] });
            }

            return value.ToArray();
        }

        /// <summary>
        /// Gets the length of the entire HTTP request body.
        /// </summary>
        /// <returns>
        /// An integer containing the length of the entire HTTP request body.
        /// </returns>
        public override int GetTotalEntityBodyLength()
        {
            return base.GetTotalEntityBodyLength();
        }

        /// <summary>
        /// Reads request data from the client (when not preloaded).
        /// </summary>
        /// <param name="buffer">The byte array to read data into.</param>
        /// <param name="size">The maximum number of bytes to read.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public override int ReadEntityBody(byte[] buffer, int size)
        {
            return this.ReadEntityBody(buffer, 0, size);
        }

        /// <summary>
        /// Reads request data from the client (when not preloaded) by using the specified buffer to read from, 
        /// byte offset, and maximum bytes.
        /// </summary>
        /// <param name="buffer">The byte array to read data into.</param>
        /// <param name="offset">The byte offset at which to begin reading.</param>
        /// <param name="size">The maximum number of bytes to read.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public override int ReadEntityBody(byte[] buffer, int offset, int size)
        {
            return this.context.Request.InputStream.Read(buffer, offset, size);
        }

        /// <summary>
        /// When overridden in a derived class, gets the certification fields (specified in the X.509 standard) from 
        /// a request issued by the client.
        /// </summary>
        /// <returns>
        /// A byte array containing the stream of the entire certificate content.
        /// </returns>
        public override byte[] GetClientCertificate()
        {
            if (this.clientCert == null)
            {
                return null;
            }

            return this.clientCert.RawData;
        }

        /// <summary>
        /// Gets the certificate issuer, in binary format.
        /// </summary>
        /// <returns>
        /// A byte array containing the certificate issuer expressed in binary format.
        /// </returns>
        public override byte[] GetClientCertificateBinaryIssuer()
        {
            if (this.clientCert == null)
            {
                return null;
            }

            return this.clientCert.IssuerName.RawData;
        }

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="T:System.Text.Encoding" /> object in which the client 
        /// certificate was encoded.
        /// </summary>
        /// <returns>
        /// The certificate encoding, expressed as an integer.
        /// </returns>
        public override int GetClientCertificateEncoding()
        {
            return 0;
        }

        /// <summary>
        /// When overridden in a derived class, gets a PublicKey object associated with the client certificate.
        /// </summary>
        /// <returns>
        /// A PublicKey object.
        /// </returns>
        public override byte[] GetClientCertificatePublicKey()
        {
            return this.clientCert.PublicKey.EncodedKeyValue.RawData;
        }

        /// <summary>
        /// When overridden in a derived class, gets the date when the certificate becomes valid. 
        /// The date varies with international settings.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.DateTime" /> object representing when the certificate becomes valid.
        /// </returns>
        public override DateTime GetClientCertificateValidFrom()
        {
            if (this.clientCert == null)
            {
                return DateTime.MaxValue;
            }

            return this.clientCert.NotBefore;
        }

        /// <summary>
        /// Gets the certificate expiration date.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.DateTime" /> object representing the date that the certificate expires.
        /// </returns>
        public override DateTime GetClientCertificateValidUntil()
        {
            if (this.clientCert == null)
            {
                return DateTime.MinValue;
            }

            return this.clientCert.NotAfter;
        }

        /// <summary>
        /// When overridden in a derived class, returns the ID of the current connection.
        /// </summary>
        /// <returns>
        /// Always returns 0.
        /// </returns>
        public override long GetConnectionID()
        {
            return base.GetConnectionID();
        }

        /// <summary>
        /// Returns a value indicating whether the connection uses SSL.
        /// </summary>
        /// <returns>
        /// true if the connection is an SSL connection; otherwise, false. The default is false.
        /// </returns>
        public override bool IsSecure()
        {
            return this.context.Request.IsSecureConnection;
        }

        /// <summary>
        /// Returns a single server variable from a dictionary of server variables associated with the request.
        /// </summary>
        /// <param name="name">The name of the requested server variable.</param>
        /// <returns>
        /// The requested server variable.
        /// </returns>
        public override string GetServerVariable(string name)
        {
            string varName = name.ToUpperInvariant();
            if (string.CompareOrdinal(varName, "CERT_FLAGS") == 0)
            {
                int flags = 0;
                if (this.clientCert != null)
                {
                    flags = 1;
                    if (this.clientCert.Verify())
                    {
                        flags |= 2;
                    }
                }

                return flags.ToString();
            }

            if (this.clientCert != null)
            {
                if (string.CompareOrdinal(varName, "CERT_SUBJECT") == 0)
                {
                    return this.clientCert.Subject;
                }

                if (string.CompareOrdinal(varName, "CERT_ISSUER") == 0)
                {
                    return this.clientCert.Issuer;
                }

                if (string.CompareOrdinal(varName, "CERT_SERIALNUMBER") == 0)
                {
                    return this.clientCert.SerialNumber;
                }

                if (string.CompareOrdinal(varName, "CERT_KEYSIZE") == 0)
                {
                    return this.clientCert.PublicKey.Key.KeySize.ToString();
                }
            }

            return base.GetServerVariable(name);
        }
    }
}
