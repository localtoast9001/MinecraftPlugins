//-----------------------------------------------------------------------
// <copyright file="PlayerProfileUtility.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Minecraft.Management
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// Utility to get profile information using the Mojang web service.
    /// </summary>
    public static class PlayerProfileUtility
    {
        /// <summary>
        /// The profile URL format.
        /// </summary>
        private const string ProfileUrl = "https://sessionserver.mojang.com/session/minecraft/profile/{0:N}";

        /// <summary>
        /// The user agent.
        /// </summary>
        private const string UserAgent = "Mozilla/5.0";

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="id">The player uuid.</param>
        /// <returns>The player profile.</returns>
        public static PlayerProfile GetProfile(Guid id)
        {
            Task<PlayerProfile> task = GetProfileAsync(id);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Gets the profile asynchronously.
        /// </summary>
        /// <param name="id">The player uuid.</param>
        /// <returns>A task to receive the player profile.</returns>
        /// <exception cref="InvalidOperationException">Player not found.</exception>
        public static async Task<PlayerProfile> GetProfileAsync(Guid id)
        {
            string url = string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                ProfileUrl,
                id);
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.UserAgent = UserAgent;
            string json = null;
            using (WebResponse response = await req.GetResponseAsync())
            {
                HttpWebResponse webResponse = (HttpWebResponse)response;
                if (webResponse.StatusCode == HttpStatusCode.NoContent ||
                    webResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new InvalidOperationException("Player not found.");
                }

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    json = await reader.ReadToEndAsync();
                }
            }

            dynamic profileData = JsonConvert.DeserializeObject(json);
            PlayerProfile profile = new PlayerProfile
            {
                Id = Guid.Parse((string)profileData.id),
                Name = profileData.name
            };

            foreach (dynamic property in profileData.properties)
            {
                PlayerProfileProperty targetProperty = new PlayerProfileProperty
                {
                    Name = property.name,
                    Value = Convert.FromBase64String((string)property.value)
                };

                profile.Properties.Add(targetProperty);

                if (string.Equals(targetProperty.Name, "textures", StringComparison.OrdinalIgnoreCase))
                {
                    string texturesJson = Encoding.UTF8.GetString(targetProperty.Value);
                    dynamic textureData = JsonConvert.DeserializeObject(texturesJson);
                    PlayerTextureInfo textureInfo = new PlayerTextureInfo
                    {
                        Slim = HasOddJavaHashCode(profile.Id),
                        ProfileId = Guid.Parse((string)textureData.profileId),
                        ProfileName = textureData.profileName,
                        Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + 
                            TimeSpan.FromMilliseconds((long)textureData.timestamp)
                    };

                    if (textureData.textures != null)
                    {
                        if (textureData.textures.SKIN != null)
                        {
                            textureInfo.SkinTexture = new Uri((string)textureData.textures.SKIN.url);
                            if (textureData.textures.SKIN.metadata != null)
                            {
                                if (textureData.textures.SKIN.metadata.model != null)
                                {
                                    textureInfo.Slim = string.Equals(
                                        (string)textureData.textures.SKIN.metadata.model,
                                        "slim",
                                        StringComparison.OrdinalIgnoreCase);
                                }
                            }
                        }

                        if (textureData.textures.CAPE != null)
                        {
                            textureInfo.CapeTexture = new Uri((string)textureData.textures.CAPE.url);
                        }
                    }

                    profile.Textures = textureInfo;
                }
            }

            return profile;
        }

        /// <summary>
        /// Determines whether the specified guid has an odd java hash code.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>
        ///   <c>true</c> if the hash code is odd; otherwise, <c>false</c>.
        /// </returns>
        private static bool HasOddJavaHashCode(Guid guid)
        {
            byte[] raw = guid.ToByteArray();
            byte hash = (byte)((int)raw[0] ^ (int)raw[6] ^ (int)raw[11] ^ (int)raw[15]);
            return (hash & 0x1) != 0;
        }
    }
}
