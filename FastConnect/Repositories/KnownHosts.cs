using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastConnect.Repositories
{
    public static class KnownHosts
    {
        /// <summary>
        /// tries to add all entries from the known hosts file to the database
        /// </summary>
        public static void AddKnownHostsToDatabase()
        {
            var knownFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh", "known_hosts");
            AddKnownHostsToDatabase(knownFile);
        }

        /// <summary>
        /// tries to add all entries from the known hosts file to the database
        /// </summary>
        /// <param name="knownFile"></param>
        public static void AddKnownHostsToDatabase(string knownFile)
        {
            if (File.Exists(knownFile))
            {
                var knownEntries = GetKnownHostEntries(knownFile);
                foreach (var knownEntry in knownEntries)
                {
                    Repositories.Database.AddConnection(knownEntry);
                }
            }
        }

        private static IEnumerable<Models.SQLite.ConnectionEntry> GetKnownHostEntries(string filePath)
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var firstSplitWord = line.Trim().Split(' ')[0].Split(':');

                // host
                var host = firstSplitWord[0].Trim(new char[] { '[', ']' });

                // port
                var port = 22;

                if (firstSplitWord.Length > 1)
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
