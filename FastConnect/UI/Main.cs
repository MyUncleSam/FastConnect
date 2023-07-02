using Microsoft.VisualBasic.FileIO;
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

            // draw list
            listView = new ListView()
            {
                X = 1,
                Y = 0,
                Height = Dim.Fill() - 1,
                Width = Dim.Fill(),
                AllowsMarking = false,
                AllowsMultipleSelection = false,
            };
            listView.RowRender += ListView_RowRender;
            listView.KeyPress += ListView_KeyPress;
            
            var statusBar = new StatusBar
            {
                Visible = true,
                Items = new StatusItem[]
                {
                    new StatusItem(Key.Enter, "~Enter~ Connect", () => Enter()),
                    new StatusItem(Key.CtrlMask | Key.A, "~^A~ Add", () => Add()),
                    new StatusItem(Key.CtrlMask | Key.D, "~^D~ Delete", () => Delete()),
                    new StatusItem(Key.Esc, "~Esc~ Quit", () => Quit()),
                }
            };

            Add(listView, statusBar);

            FillConnections();
        }

        private void FillConnections()
        {
            // get connections from database
            var connections = Repositories.Database.GetConnections()
                .OrderBy(ob => ob.Host)
                .ToList();

            listView.SetSource(connections);
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
                obj.RowAttribute = new Terminal.Gui.Attribute(Color.White, Color.Blue);
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
            var dia = new Dialog("Test");
            var add = new UI.Add();
            dia.Add(add);
            Application.Run(dia);

            if(add.Save)
            {
                if (Repositories.Database.AddConnection(add.Host.Text.ToString(), Convert.ToInt32(add.Port.Text.ToString())))
                {
                    FillConnections();
                }
                else
                {
                    MessageBox.ErrorQuery("Add", "Unable to add, entry already exists in database.");
                }
            }
        }

        private void Delete()
        {
            var selectedIndex = listView.SelectedItem;
            var curSelected = listView.Source.ToList()[selectedIndex] as Models.SQLite.ConnectionEntry;

            if(Repositories.Database.DeleteConnection(curSelected))
            {
                FillConnections();
            }
        }

        private void Quit()
        {
            Application.RequestStop();
        }
    }
}
