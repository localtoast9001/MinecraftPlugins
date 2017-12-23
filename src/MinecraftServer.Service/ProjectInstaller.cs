// -----------------------------------------------------------------------
// <copyright file="ProjectInstaller.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MinecraftServer.Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Installer for the project.
    /// </summary>
    /// <seealso cref="System.Configuration.Install.Installer" />
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInstaller"/> class.
        /// </summary>
        public ProjectInstaller()
        {
            this.InitializeComponent();
        }
    }
}
