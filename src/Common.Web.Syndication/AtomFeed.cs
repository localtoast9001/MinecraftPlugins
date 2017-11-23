// -----------------------------------------------------------------------
// <copyright file="AtomFeed.cs" company="Jon Rowlett">
//  Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Syndication
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// An Atom Feed.
    /// </summary>
    public class AtomFeed
    {
        /// <summary>
        /// The entries.
        /// </summary>
        private Collection<AtomEntry> entries = new Collection<AtomEntry>();

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        public Uri Link { get; set; }

        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AtomAuthor Author { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        public Collection<AtomEntry> Entries
        {
            get { return this.entries; }
        }
    }
}
