using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace FastConnect.UI
{
    public class Add : Window
    {
        public TextField Host { get; set; }
        public TextField Port { get; set; }

        public Add()
        {
            Title = "Add New Connection";

            var hostLabel = new Label() 
            { 
                Text = "Host:" 
            };

            Host = new TextField("")
            {
                X = Pos.Right(hostLabel) + 1,
                Width = Dim.Fill()
            };

            var portLabel = new Label()
            {
                Text = "Port:",
                Y = Pos.Bottom(hostLabel) + 1
            };

            Port = new TextField("22")
            {
                X = Pos.Right(portLabel) + 1,
                Y = portLabel.Y,
                Width = Dim.Fill()
            };

            var btnAdd = new Button()
            {
                Text = "Add",
                Y = Pos.Bottom(portLabel) + 1,
                X = Pos.Center(),
                IsDefault = true
            };

            var btnCancel = new Button()
            {
                Text = "Cancel",
                Y = btnAdd.Y,
                X = Pos.Right(btnAdd) + 1,
                HotKey = Key.Esc
            };

            btnAdd.Clicked += () =>
            {
                // verify host input
                if (string.IsNullOrWhiteSpace(Host.Text.ToString()))
                {
                    MessageBox.ErrorQuery("Host", "Host cannot be empty");
                    return;
                }

                // verify port input
                if (string.IsNullOrWhiteSpace(Port.Text.ToString()))
                {
                    MessageBox.ErrorQuery("Port", "Port cannot be empty");
                    return;
                }

                if (int.TryParse(Port.Text.ToString(), out var port))
                {
                    if (port <= 0 || port > 25565)
                    {
                        MessageBox.ErrorQuery("Port", "Port needs to be in a valid port range");
                        return;
                    }
                }
                else
                {
                    MessageBox.ErrorQuery("Port", "Port needs to be a number");
                    return;
                }

                Application.RequestStop();
            };

            btnCancel.Clicked += () =>
            {
                Host = null;
                Port = null;
                Application.RequestStop();
            };

            Add(hostLabel, portLabel, Host, Port, btnAdd, btnCancel);
            
            this.FocusFirst();
        }
    }
}
