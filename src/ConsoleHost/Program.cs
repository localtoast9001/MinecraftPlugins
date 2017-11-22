//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ConsoleHost.Service
{
    using System;
    using System.ServiceProcess;
    using Common.Logging;

    /// <summary>
    /// Static class implementing the main entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            if (args.Length == 1 && string.Equals(args[0], "/DEBUG", StringComparison.OrdinalIgnoreCase))
            {
                ConsoleLogMessageStream log = new ConsoleLogMessageStream();
                try
                {
                    ConsoleHostComponent service = new ConsoleHostComponent();
                    service.Start(args);
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    service.Stop();
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
                    new ConsoleHostService() 
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
