using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FastConnect.Repositories
{
    public static class Database
    {
        public static string DbFile => "db.sqlite";

        public static string ConnectionString => $"Data Source={DbFile}";

        /// <summary>
        /// creates the datbase and default tables (only if missing)
        /// </summary>
        /// <param name="filePath">only used for testings</param>
        /// <returns>true if new database was created</returns>
        public static bool Create(string filePath = null)
        {
            if(filePath == null)
            {
                filePath = DbFile;
            }

            if (!File.Exists(filePath))
            {
                // get all sql files to execute
                var sqlFiles = Directory
                    .GetFiles("Sql", "*.sql", SearchOption.TopDirectoryOnly)
                    .OrderBy(ob => ob);

                using (var con = new SQLiteConnection($"Data Source={filePath}"))
                {
                    con.Open();

                    foreach (var sqlFile in sqlFiles)
                    {
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = File.ReadAllText(sqlFile);
                            var result = cmd.ExecuteNonQuery();
                        }
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// return a list of all known connections
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Models.SQLite.ConnectionEntry> GetConnections()
        {
            using (var con = new SQLiteConnection(ConnectionString))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT Host, Port FROM Connections;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            yield return new Models.SQLite.ConnectionEntry()
                            {
                                Host = Convert.ToString(reader["Host"])!,
                                Port = Convert.ToInt32(reader["Port"])
                            };
                        }
                    }
                }
            }
        }
    }
}
