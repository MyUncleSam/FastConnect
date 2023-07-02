using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastConnect.Repositories
{
    public static class Ssh
    {
        public static void Connect(string host, int port = 22)
        {
            List<string> args = new List<string>();

            if (port != 22) { args.Add($"-p {port}"); }

            var connectSsh = new ProcessStartInfo("ssh", $"{string.Join(" ", args)} {host}");
            connectSsh.UseShellExecute = true;

            using (var proc = Process.Start(connectSsh))
            {
            }
        }

        public static IEnumerable<Models.SQLite.ConnectionEntry> GetKnownHostEntries(string filePath)
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                if(string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var firstSplitWord = line.Trim().Split(' ')[0].Split(':');

                // host
                var host = firstSplitWord[0].Trim(new char[] { '[', ']' });

                // port
                var port = 22;

                if(firstSplitWord.Length > 1)
                {
                    port = int.Parse(firstSplitWord[1]);
                }

                yield return new Models.SQLite.ConnectionEntry() 
                { 
                    Host = host,
                    Port = port 
                };
            }
        }
    }
}
