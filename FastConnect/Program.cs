using FastConnect.Repositories;
using Terminal.Gui;

namespace FastConnect
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // prepare database
            Repositories.Database.Create();

            // show main ui
            Application.Run<UI.Main>();
        }
    }
}