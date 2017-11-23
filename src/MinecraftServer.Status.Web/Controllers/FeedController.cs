// -----------------------------------------------------------------------
// <copyright file="FeedController.cs" company="Jon Rowlett">
//  Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MinecraftServer.Status.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Common.Web.Syndication;
    using System.Configuration;

    /// <summary>
    /// Publishes an Atom feed for server events based on the latest log.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("feed")]
    public class FeedController : ApiController
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>The Atom feed.</returns>
        [Route]
        public FormattedContentResult<AtomFeed> Get()
        {
            string path = System.IO.Path.Combine(
                ConfigurationManager.AppSettings["MinecraftLocation"],
                "Logs\\latest.log");
            AtomFeed feed = new AtomFeed
            {
                Title = "Minecraft Events"
            };

            using (MinecraftLogReader reader = new MinecraftLogReader(path))
            {
                MinecraftLogEntry entry = reader.ReadNext();
                while (entry != null)
                {
                    AtomEntry targetEntry = new AtomEntry
                    {
                        Title = entry.Text,
                        Updated = entry.Timestamp,
                        Summary = string.Format("[{0}]: {1}", entry.Source, entry.Text)
                    };

                    feed.Entries.Add(targetEntry);
                    feed.Updated = targetEntry.Updated;

                    entry = reader.ReadNext();
                }
            }

            return this.Content<AtomFeed>(
                HttpStatusCode.OK,
                feed,
                new AtomMediaTypeFormatter(),
                AtomMediaTypeFormatter.MediaType);
        }
    }
}
