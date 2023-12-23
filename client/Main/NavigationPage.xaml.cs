using System.Windows.Controls;

namespace OIChatRoomClient.Main
{
    /// <summary>
    /// NavigationPage.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            InitializeComponent();
            RootNavigation.Loaded += (sender, args) =>
            {
                RootNavigation.Navigate(typeof(HomePage));
            };
            RootNavigation.SelectionChanged += (sender, args) =>
            {
                if (RootNavigation.SelectedItem.TargetPageType.Equals(typeof(NavigateFakeConsole)))
                {
                    MainWindow.Instance.fakeConsoleMode();
                }
            };
        }
    }
}
