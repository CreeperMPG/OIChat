using OIChatRoomClient.Utils;
using System.Windows;
using System.Windows.Controls;

namespace OIChatRoomClient.Main.Setting
{
    /// <summary>
    /// SettingInfo.xaml 的交互逻辑
    /// </summary>
    public partial class SettingInfo : Page
    {
        public SettingInfo()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                string infoString = ResourceUtils.getResourceString("OIChatRoomClient.Resources.Info.info.txt");
                infoString = string.Format(infoString, App.VERSION);
                introductionText.Text = infoString;
            };
            LicenseButton.Click += (s, e) =>
            {
                License.LicenseWindow? licenseWindow = License.LicenseWindow.Instance;
                if (licenseWindow != null)
                {
                    if (licenseWindow.WindowState == WindowState.Minimized)
                    {
                        licenseWindow.WindowState = WindowState.Normal;
                    }
                    licenseWindow.Focus();
                }
                else
                {
                    licenseWindow = new License.LicenseWindow();
                    licenseWindow.Show();
                }
            };
            UpdateLogButton.Click += (s, e) =>
            {
                UpdateLog.UpdateLogWindow? updateLogWindow = UpdateLog.UpdateLogWindow.Instance;
                if (updateLogWindow != null)
                {
                    if (updateLogWindow.WindowState == WindowState.Minimized)
                    {
                        updateLogWindow.WindowState = WindowState.Normal;
                    }
                    updateLogWindow.Focus();
                }
                else
                {
                    updateLogWindow = new UpdateLog.UpdateLogWindow();
                    updateLogWindow.Show();
                }
            };
        }
    }
}
