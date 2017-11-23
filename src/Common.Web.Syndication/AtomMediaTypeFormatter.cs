// -----------------------------------------------------------------------
// <copyright file="AtomMediaTypeFormatter.cs" company="Jon Rowlett">
//  Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Common.Web.Syndication
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Web API <see cref="MediaTypeFormatter"/> for formatting ATOM feeds and entries.
    /// </summary>
    /// <seealso cref="System.Net.Http.Formatting.MediaTypeFormatter" />
    public class AtomMediaTypeFormatter : MediaTypeFormatter
    {
        /// <summary>
        /// The media type.
        /// </summary>
        public const string MediaType = "application/atom+xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomMediaTypeFormatter"/> class.
        /// </summary>
        public AtomMediaTypeFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaType));
            this.SupportedEncodings.Add(Encoding.UTF8);
        }

        /// <summary>
        /// Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserializean object of the specified type.
        /// </summary>
        /// <param name="type">The type to deserialize.</param>
        /// <returns>
        /// true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserialize the type; otherwise, false.
        /// </returns>
        public override bool CanReadType(Type type)
        {
            return typeof(AtomFeed).IsAssignableFrom(type) || typeof(AtomEntry).IsAssignableFrom(type);
        }

        /// <summary>
        /// Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serializean object of the specified type.
        /// </summary>
        /// <param name="type">The type to serialize.</param>
        /// <returns>
        /// true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serialize the type; otherwise, false.
        /// </returns>
        public override bool CanWriteType(Type type)
        {
            return typeof(AtomFeed).IsAssignableFrom(type) || typeof(AtomEntry).IsAssignableFrom(type);
        }

        /// <summary>
        /// Asynchronously writes an object of the specified type.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object value to write.  It may be null.</param>
        /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> if available. It may be null.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" /> if available. It may be null.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that will perform the write.
        /// </returns>
        public override Task WriteToStreamAsync(
            Type type, 
            object value, 
            Stream writeStream, 
            HttpContent content, 
            TransportContext transportContext)
        {
            return this.WriteToStreamAsync(
                type, 
                value, 
                writeStream,
                content, 
                transportContext, 
                CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously writes an object of the specified type.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object value to write.  It may be null.</param>
        /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> if available. It may be null.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" /> if available. It may be null.</param>
        /// <param name="cancellationToken">The token to cancel the operation.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that will perform the write.
        /// </returns>
        public override async Task WriteToStreamAsync(
            Type type,
            object value,
            Stream writeStream,
            HttpContent content,
            TransportContext transportContext,
            CancellationToken cancellationToken)
        {
            if (value == null)
            {
                return;
            }

            Encoding encoding = content != null ?
                this.SelectCharacterEncoding(content.Headers) :
                Encoding.UTF8;
            using (AtomWriter writer = new AtomWriter(writeStream, encoding))
            {
                AtomFeed feed = value as AtomFeed;
                if (feed != null)
                {
                    await writer.WriteAsync(feed, cancellationToken);
                    return;
                }

                AtomEntry entry = value as AtomEntry;
                if (entry != null)
                {
                    await writer.WriteAsync(entry, cancellationToken);
                }
            }
        }
    }
}
