// -----------------------------------------------------------------------
// <copyright file="AtomAuthor.cs" company="Jon Rowlett">
//  Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Common.Web.Syndication
{
    using System;

    /// <summary>
    /// Information about an author for use in an atom feed.
    /// </summary>
    public class AtomAuthor
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
    }
}
