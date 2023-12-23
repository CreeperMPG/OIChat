using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace OIChatRoomClient.Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public static MainWindow Instance;
        public bool isFakeConsoleMode = false;
        public MainWindow()
        {
            Instance = this;
            ApplicationThemeManager.Apply(ApplicationTheme.Dark);
            InitializeComponent();
            Loaded += (s, e) =>
            {
                realMode();
            };
            KeyDown += (s, e) =>
            {
                if (isFakeConsoleMode && e.Key == Key.Escape)
                {
                    realMode();
                }
            };
            Closing += (s, e) =>
            {
                if (License.LicenseWindow.Instance != null)
                {
                    License.LicenseWindow.Instance.Close();
                }
                if (UpdateLog.UpdateLogWindow.Instance != null)
                {
                    UpdateLog.UpdateLogWindow.Instance.Close();
                }
            };
        }
        public void fakeConsoleMode()
        {
            isFakeConsoleMode = true;
            Title = "C:\\Users\\Default\\Desktop\\Program.exe";
            appFrame.Navigate(new FakeConsole.MainPage());
            Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/system.png"));
        }
        public void realMode()
        {
            isFakeConsoleMode = false;
            Title = "OI Chatroom";
            appFrame.Navigate(new NavigationPage());
            Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/icon.png"));
        }
    }
}
