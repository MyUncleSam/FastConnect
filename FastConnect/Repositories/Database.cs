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

        /// <summary>
        /// add a new connection to the list
        /// </summary>
        /// <param name="connectionEntry"></param>
        /// <returns></returns>
        public static bool AddConnection(Models.SQLite.ConnectionEntry connectionEntry)
        {
            return AddConnection(connectionEntry.Host, connectionEntry.Port);
        }

        /// <summary>
        /// add a new connection to the list
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool AddConnection(string host, int port)
        {
            using (var con = new SQLiteConnection(ConnectionString))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Connections (Host, Port) VALUES (@Host, @Port);";
                    cmd.Parameters.Add(new SQLiteParameter("@Host", host));
                    cmd.Parameters.Add(new SQLiteParameter("@Port", port));

                    try
                    {
                        return cmd.ExecuteNonQuery() == 1;
                    }
                    catch(Exception ex)
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// removes a connection
        /// </summary>
        /// <param name="connectionEntry"></param>
        /// <returns></returns>
        public static bool DeleteConnection(Models.SQLite.ConnectionEntry connectionEntry)
        {
            return RemoveConnection(connectionEntry.Host, connectionEntry.Port);
        }

        /// <summary>
        /// removes a connection
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool RemoveConnection(string host, int port)
        {
            using (var con = new SQLiteConnection(ConnectionString))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Connections WHERE Host = @Host AND Port = @Port;";
                    cmd.Parameters.Add(new SQLiteParameter("@Host", host));
                    cmd.Parameters.Add(new SQLiteParameter("@Port", port));

                    try
                    {
                        return cmd.ExecuteNonQuery() == 1;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }
    }
}
