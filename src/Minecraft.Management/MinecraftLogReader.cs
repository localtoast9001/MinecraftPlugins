using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Minecraft.Management
{
    public class MinecraftLogReader : IDisposable
    {
        public TextReader inner;

        public MinecraftLogReader(string path)
        {
            this.inner = new StreamReader(
                new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete));
        }

        ~MinecraftLogReader()
        {
        }

        public static MinecraftLogEntry ParseLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            int timeStart = line.IndexOf('[');
            if (timeStart < 0)
            {
                return null;
            }

            timeStart++;
            int index = timeStart;
            while (index < line.Length && line[index] != ']')
            {
                index++;
            }

            string rawTimestamp = line.Substring(timeStart, index - timeStart);
            TimeSpan offset = TimeSpan.Parse(rawTimestamp);
            DateTime timestamp = DateTime.Today + offset;

            index++;
            while (index < line.Length && line[index] != '[')
            {
                index++;
            }

            index++;
            int sourceStart = index;
            while (index < line.Length && line[index] != ']')
            {
                index++;
            }

            string source = line.Substring(sourceStart, index - sourceStart);
            index++;
            while (index < line.Length && line[index] != ':')
            {
                index++;
            }

            index++;
            string text = string.Empty;
            if (index < line.Length)
            {
                text = line.Substring(index).Trim();
            }

            return new MinecraftLogEntry
            {
                Timestamp = timestamp,
                Source = source,
                Text = text
            };
        }

        public MinecraftLogEntry ReadNext()
        {
            string line = this.inner.ReadLine();
            return ParseLine(line);
        }

        public void Dispose()
        {
            if (this.inner != null)
            {
                this.inner.Dispose();
            }
        }
    }
}