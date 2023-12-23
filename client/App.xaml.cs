using OIChatRoomClient.Main;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace OIChatRoomClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string VERSION = "0.0.3.1";
        public MainWindow? mainWindow;
        public string[]? args;
        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            args = e.Args;
            if (args.Length == 0)
            {
                mainWindow = new MainWindow();
            }
            else if (args.Contains("-console"))
            {
                MessageBox.Show("Console 版本目前无法使用");
            }
            mainWindow.Show();
        }
    }
}
