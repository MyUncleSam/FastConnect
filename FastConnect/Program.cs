using FastConnect.Repositories;
using Terminal.Gui;

namespace FastConnect
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // prepare database
            if(Repositories.Database.Create())
            {
                // as default add known hosts
                var knownFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh", "known_hosts");

                if(File.Exists(knownFile))
                {
                    var knownEntries = Repositories.Ssh.GetKnownHostEntries(knownFile);
                    foreach (var knownEntry in knownEntries)
                    {
                        Repositories.Database.AddConnection(knownEntry);
                    }
                }
            }

            // show main ui
            Application.Run<UI.Main>();
        }
    }
}