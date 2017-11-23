using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinecraftServer.Status.Web
{
    public class MinecraftLogEntry
    {
        public DateTime Timestamp { get; set; }

        public string Source { get; set; }

        public string Text { get; set; }
    }
}