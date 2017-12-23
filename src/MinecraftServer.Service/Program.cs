// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MinecraftServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Logging;

    /// <summary>
    /// Main entry point for the program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            if (args.Length == 1 && string.Equals(args[0], "/DEBUG", StringComparison.OrdinalIgnoreCase))
            {
                ConsoleLogMessageStream log = new ConsoleLogMessageStream();
                try
                {
                    using (ServerWrapperComponent service = new ServerWrapperComponent())
                    {
                        service.Start(log, args);
                        Console.WriteLine("Press enter to exit.");
                        Console.ReadKey();
                        service.Stop();
                    }
                }
                catch (Exception ex)
                {
                    log.Log(LogMessage.Error(ex.ToString()));
                }
            }
            else
            {
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[]
                {
                    new ServerWrapperService()
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
