//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ConsoleHost.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;

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
            if (args.Length == 1 && string.Compare(args[0], "/DEBUG", StringComparison.OrdinalIgnoreCase) == 0)
            {
                var service = new ConsoleHostComponent();
                service.Start(args);
                Console.ReadKey();
                service.Stop();
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
