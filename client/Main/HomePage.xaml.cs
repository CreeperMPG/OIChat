using OIChatRoomClient.Main.Home;
using System.Windows;
using System.Windows.Controls;

namespace OIChatRoomClient.Main
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            if (ChatHome.Instance != null)
            {
                homeFrame.Navigate(ChatHome.Instance);
            }
            else
            {
                homeFrame.Navigate(new ChatHome());
            }
        }
    }
}
