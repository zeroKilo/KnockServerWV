using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KnockServerWV
{
    public static class Logger
    {
        public static int maxEntries = 1000;
        private static readonly object _sync = new object();
        private static List<LogEntry> logEntries = new List<LogEntry>();
        private static FileStream fs;

        public static void Init()
        {
            if (File.Exists("log.txt"))
                File.Delete("log.txt");
            fs = new FileStream("log.txt", FileMode.CreateNew, FileAccess.Write);
        }

        public static void Log(string s, bool display = true)
        {
            lock (_sync)
            {
                LogEntry e = new LogEntry(s, display);
                if (display)
                    logEntries.Add(e);
                string t = e.time.ToShortDateString() + " " + e.time.ToLongTimeString() + " " + e.text + "\n";
                byte[] buff = Encoding.UTF8.GetBytes(t);
                fs.Write(buff, 0, buff.Length);
                fs.Flush();
                while (logEntries.Count > maxEntries)
                    logEntries.RemoveAt(0);
            }
        }

        public static List<LogEntry> LogEntries()
        {
            List<LogEntry> result = new List<LogEntry>();
            lock (_sync)
            {
                foreach (LogEntry e in logEntries)
                    if (e.display)
                        result.Add(e);
            }
            return result;
        }
    }
}
