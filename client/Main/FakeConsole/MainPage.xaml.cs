using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace OIChatRoomClient.Main.FakeConsole
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            // ApplicationThemeManager.Apply(ApplicationTheme.Light);
            // appTitle.ApplicationTheme = ApplicationTheme.Light;
            appTitle.Background = Brushes.White;
            appTitle.Foreground = Brushes.Black;
            appTitle.ButtonsForeground = Brushes.Black;
            fakeConsoleContent.FontFamily = new FontFamily("宋体");
            fakeConsoleContent.FontSize = 16;
            fakeConsoleContent.Text = randomData();
            DispatcherTimer timer = new();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += (s, e) =>
            {
                if (pausePoint.Fill == Brushes.White)
                {
                    pausePoint.Fill = Brushes.Black;
                }
                else
                {
                    pausePoint.Fill = Brushes.White;
                }
            };
            timer.Start();
        }
        private string randomData()
        {
            string result = string.Empty;
            int lineCount = 0;
            Random random = new();
            bool lineHasChar = false;
            while (true)
            {
                int num = random.Next();
                if (lineCount == 5)
                {
                    break;
                }
                else if (num % 10 == 1 && lineHasChar)
                {
                    result += "\n";
                    lineCount++;
                    lineHasChar = false;
                }
                else
                {
                    result += (random.Next(100000) / (random.Next(800) + 1)).ToString();
                    result += " ";
                    lineHasChar = true;
                }
            }
            result += $"\n---------------------\nProcess exited after {Math.Round(random.NextDouble() * random.Next(5), 2)} seconds with return value 0\n请按任意键继续...";
            return result;
        }
    }
}
