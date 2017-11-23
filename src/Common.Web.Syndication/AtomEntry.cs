// -----------------------------------------------------------------------
// <copyright file="AtomEntry.cs" company="Jon Rowlett">
//  Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Syndication
{
    using System;

    /// <summary>
    /// An entry in an Atom feed.
    /// </summary>
    public class AtomEntry
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        public Uri Link { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        public string Summary { get; set; }
    }
}
