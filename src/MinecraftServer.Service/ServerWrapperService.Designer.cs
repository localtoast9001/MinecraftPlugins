﻿namespace MinecraftServer.Service
{
    partial class ServerWrapperService
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.serviceEventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.serviceEventLog)).BeginInit();
            // 
            // serviceEventLog
            // 
            this.serviceEventLog.Log = "Application";
            this.serviceEventLog.Source = "MinecraftServer";
            // 
            // ServerWrapperService
            // 
            this.ServiceName = "MinecraftServer";
            ((System.ComponentModel.ISupportInitialize)(this.serviceEventLog)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog serviceEventLog;
    }
}
