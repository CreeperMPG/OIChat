using OIChatRoomClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace OIChatRoomClient.Main.UpdateLog
{
    /// <summary>
    /// UpdateLogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateLogWindow : FluentWindow
    {
        public static UpdateLogWindow? Instance = null;
        public UpdateLogWindow()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            InitializeComponent();
            Loaded += (s, e) =>
            {
                string updateLog = ResourceUtils.getResourceString("OIChatRoomClient.Resources.Info.updateLog.txt");
                UpdateLogText.Text = updateLog;
            };
            Closing += (s, e) =>
            {
                Instance = null;
            };
        }
    }
}
