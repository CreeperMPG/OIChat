using OIChatRoomClient.Main.Setting;
using System.Windows.Controls;

namespace OIChatRoomClient.Main
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            SettingInfoFrame.Navigate(new SettingInfo());
        }
    }
}
