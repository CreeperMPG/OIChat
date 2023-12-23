using OIChatRoomClient.Utils;
using Wpf.Ui.Controls;

namespace OIChatRoomClient.Main.License
{
    /// <summary>
    /// LicenseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LicenseWindow : FluentWindow
    {
        public static LicenseWindow? Instance = null;
        public LicenseWindow()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            InitializeComponent();
            Loaded += (s, e) =>
            {
                string license = ResourceUtils.getResourceString("OIChatRoomClient.Resources.Info.license.txt");
                LicenseText.Text = license;
            };
            Closing += (s, e) =>
            {
                Instance = null;
            };
        }
    }
}
