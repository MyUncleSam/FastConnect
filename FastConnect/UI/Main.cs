using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace FastConnect.UI
{
    internal class Main : Window
    {
        public ListView listView { get; set; }

        public Main() {
            Title = $"FastConnect (c) 2023-{DateTime.Now.Year} Stefan Ruepp";

            // get connections from database
            var connections = Repositories.Database.GetConnections()
                .OrderBy(ob => ob.ToString())
                .ToList();

            // draw list
            listView = new ListView()
            {
                X = 1,
                Y = 2,
                Height = Dim.Fill(),
                Width = Dim.Fill(),
                AllowsMarking = false,
                AllowsMultipleSelection = false,
            };
            listView.RowRender += ListView_RowRender;
            listView.KeyPress += ListView_KeyPress;
            listView.SetSource(connections);

            var statusBar = new StatusBar
            {
                Visible = false,
                Items = new StatusItem[]
                {
                    new StatusItem(Key.Enter, "~Enter~ Connect", () => Enter()),
                    //new StatusItem(Key.CtrlMask | Key.A, "~^A~ Add", () => Add()), // TODO: add "add" feature
                    new StatusItem(Key.Esc, "~Esc~ Quit", () => Quit()),
                }
            };

            Add(listView, statusBar);
        }

        private void ListView_KeyPress(KeyEventEventArgs keyEventArg)
        {
            var keyEvent = keyEventArg.KeyEvent;
            
            if (keyEvent.Key == Key.f)
            {
                // TODO: implement search feature (also maybe filter by typing)
            }
        }

        private void ListView_RowRender(ListViewRowEventArgs obj)
        {
            if (obj.Row == listView.SelectedItem)
            {
                obj.RowAttribute = new Terminal.Gui.Attribute(Color.Black, Color.Gray);
                return;
            }

            if (obj.Row %2 == 0)
            {
                obj.RowAttribute = new Terminal.Gui.Attribute(Color.White, Color.Blue);
            }
            else
            {
                obj.RowAttribute = new Terminal.Gui.Attribute(Color.White, Color.BrightBlue);
            }
        }

        private void Enter()
        {
            var selectedIndex = listView.SelectedItem;
            var curSelected = listView.Source.ToList()[selectedIndex] as Models.SQLite.ConnectionEntry;
            
            Repositories.Ssh.Connect(curSelected.Host, curSelected.Port);
        }

        private void Add()
        {
            Application.Run<UI.Add>();
        }

        private void Quit()
        {
            Application.RequestStop();
        }
    }
}
