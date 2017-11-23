// -----------------------------------------------------------------------
// <copyright file="AtomWriter.cs" company="Jon Rowlett">
//  Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Common.Web.Syndication
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;

    /// <summary>
    /// Writes an atom feed or entry to XML.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class AtomWriter : IDisposable
    {
        /// <summary>
        /// The namespace.
        /// </summary>
        public const string Namespace = "http://www.w3.org/2005/Atom";

        /// <summary>
        /// The inner writer.
        /// </summary>
        private XmlWriter inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        public AtomWriter(Stream stream, Encoding encoding)
            : this(XmlWriter.Create(stream, CreateXmlWriterSettings(encoding)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomWriter"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <exception cref="System.ArgumentNullException">inner is null.</exception>
        public AtomWriter(XmlWriter inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            this.inner = inner;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AtomWriter"/> class.
        /// </summary>
        ~AtomWriter()
        {
        }

        /// <summary>
        /// Writes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        public void Write(AtomFeed feed)
        {
            this.WriteAsync(feed, CancellationToken.None).Wait();
        }

        /// <summary>
        /// Writes the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void Write(AtomEntry entry)
        {
            this.WriteAsync(entry, CancellationToken.None).Wait();
        }

        /// <summary>
        /// Writes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task.</returns>
        public async Task WriteAsync(AtomFeed feed, CancellationToken cancellationToken)
        {
            await this.inner.WriteStartDocumentAsync();
            await this.inner.WriteStartElementAsync(null, "feed", Namespace);
            await this.inner.WriteAttributeStringAsync(null, "xmlns", null, Namespace);

            await this.inner.WriteElementStringAsync(null, "title", null, feed.Title ?? "(null)");

            await this.inner.WriteStartElementAsync(null, "updated", null);
            await this.inner.WriteStringAsync(XmlConvert.ToString(feed.Updated, XmlDateTimeSerializationMode.Utc));
            await this.inner.WriteEndElementAsync(); // updated.

            foreach (AtomEntry entry in feed.Entries)
            {
                await this.InnerWriteEntryAsync(entry, false, cancellationToken);
            }

            await this.inner.WriteFullEndElementAsync(); // feed.

            await this.inner.WriteEndDocumentAsync();
        }

        /// <summary>
        /// Writes the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task WriteAsync(AtomEntry entry, CancellationToken cancellationToken)
        {
            await this.inner.WriteStartDocumentAsync();
            await this.InnerWriteEntryAsync(entry, true, cancellationToken);
            await this.inner.WriteEndDocumentAsync();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, 
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.inner != null)
            {
                this.inner.Dispose();
            }
        }

        /// <summary>
        /// Inner implementation to write an entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="writeNamespace">if set to <c>true</c> write the namespace.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task.</returns>
        private async Task InnerWriteEntryAsync(
            AtomEntry entry, 
            bool writeNamespace,
            CancellationToken cancellationToken)
        {
            await this.inner.WriteStartElementAsync(null, "entry", writeNamespace ? Namespace : null);
            if (writeNamespace)
            {
                await this.inner.WriteAttributeStringAsync(null, "xmlns", null, Namespace);
            }

            await this.inner.WriteElementStringAsync(null, "title", null, entry.Title ?? "(null)");

            await this.inner.WriteStartElementAsync(null, "updated", null);
            await this.inner.WriteStringAsync(XmlConvert.ToString(entry.Updated, XmlDateTimeSerializationMode.Utc));
            await this.inner.WriteEndElementAsync(); // updated.

            if (entry.Summary != null)
            {
                await this.inner.WriteElementStringAsync(null, "summary", null, entry.Summary ?? "(null)");
            }

            await this.inner.WriteFullEndElementAsync(); // entry
        }

        /// <summary>
        /// Creates the XML writer settings.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns>A new instance of the <see cref="XmlWriterSettings"/> class.</returns>
        private static XmlWriterSettings CreateXmlWriterSettings(Encoding encoding)
        {
            return new XmlWriterSettings
            {
                Encoding = encoding,
                Async = true,
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = true
            };
        }
    }
}
