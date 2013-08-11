//-----------------------------------------------------------------------
// <copyright file="ConsoleHostService.designer.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ConsoleHost.Service
{
    /// <summary>
    /// Service Component to process commands from the Windows SCM.
    /// </summary>
    public partial class ConsoleHostService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ConsoleHostService
            // 
            this.ServiceName = "ConsoleHost";

        }

        #endregion
    }
}
