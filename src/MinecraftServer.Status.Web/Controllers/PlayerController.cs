// -----------------------------------------------------------------------
// <copyright file="PlayerController.cs" company="Jon Rowlett">
//  Copyright (C) 2017 Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MinecraftServer.Status.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using System.Configuration;
    using Minecraft.Management;
    using System.Net.Http.Headers;
    using System.Runtime.Caching;

    /// <summary>
    /// Proxies player information to avoid cross origin calls
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/v1/player")]
    public class PlayerController : ApiController
    {
        /// <summary>
        /// Gets the skin texture.
        /// </summary>
        /// <param name="id">The player uuid.</param>
        /// <returns>A task to download the skin.</returns>
        [Route("{id}/skin")]
        public async Task<IHttpActionResult> GetSkinTexture(Guid id)
        {
            ObjectCache cache = this.GetCache();
            PlayerProfile profile = cache.Get(id.ToString()) as PlayerProfile;
            if (profile == null)
            {
                profile = await PlayerProfileUtility.GetProfileAsync(id);
                cache.Set(id.ToString(), profile, DateTimeOffset.UtcNow + TimeSpan.FromHours(1.0));
            }

            if (profile.Textures == null || profile.Textures.SkinTexture == null)
            {
                return this.NotFound();
            }

            HttpWebRequest request = WebRequest.CreateHttp(profile.Textures.SkinTexture);
            request.UserAgent = this.Request.Headers.UserAgent.ToString();
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                HttpResponseMessage message = new HttpResponseMessage(response.StatusCode);
                message.EnsureSuccessStatusCode();

                
                MemoryStream ms = new MemoryStream();
                using (Stream stream = response.GetResponseStream())
                {
                    await stream.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                }

                StreamContent streamContent = new StreamContent(ms);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(response.ContentType);
                message.Content = streamContent;
                return this.ResponseMessage(message);
            }
        }

        private ObjectCache GetCache()
        {
            return MemoryCache.Default;
        }
    }
}
