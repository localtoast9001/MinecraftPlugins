//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MCPlayer
{
    using System;
    using Minecraft.Management;

    /// <summary>
    /// The Main program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: \n\tMCPlayer <uuid>\nuuid - player UUID");
                return;
            }

            Guid id = Guid.Parse(args[0]);

            PlayerProfile profile = PlayerProfileUtility.GetProfile(id);
            Console.WriteLine("Id: {0}", profile.Id);
            Console.WriteLine("Name: {0}", profile.Name);
            Console.WriteLine("Properties:");
            foreach (PlayerProfileProperty prop in profile.Properties)
            {
                Console.WriteLine("  Name: {0}", prop.Name);
                Console.WriteLine("  Value: {0}", Convert.ToBase64String(prop.Value));
                Console.WriteLine();
            }

            if (profile.Textures != null)
            {
                Console.WriteLine("Textures:");
                Console.WriteLine("  Timestamp: {0}", profile.Textures.Timestamp);
                Console.WriteLine("  Slim?: {0}", profile.Textures.Slim);
                Console.WriteLine("  SkinTexture: {0}", profile.Textures.SkinTexture);
                Console.WriteLine("  CapeTexture: {0}", profile.Textures.CapeTexture);
            }
        }
    }
}
