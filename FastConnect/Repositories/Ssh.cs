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
    }
}
