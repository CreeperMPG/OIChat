using System.Windows;
using System.Windows.Controls;

namespace OIChatRoomClient.Main.Home
{
    /// <summary>
    /// ChatHome.xaml 的交互逻辑
    /// </summary>
    public partial class ChatHome : Page
    {
        public static ChatHome? Instance;
        public ChatHome()
        {
            InitializeComponent();
            Instance ??= this;
            ChatroomFrame.Navigate(new Chat());
        }
    }
}
