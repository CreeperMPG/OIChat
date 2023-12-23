using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace OIChatRoomClient.Utils
{
    internal static class MessageBoxUtils
    {
        public static async void OpenMessageBox(string title, string content, string closeContent = "确定")
        {
            var uiMessageBox = new MessageBox
            {
                Title = title,
                Content = content,
                CloseButtonText = closeContent,
            };

            var result = await uiMessageBox.ShowDialogAsync();
        }
        public static async Task<MessageBoxResult> OpenAskBox(string title, string content, string firstContent = "确定", string closeContent = "取消")
        {
            var uiMessageBox = new MessageBox
            {
                Title = title,
                Content = content,
                PrimaryButtonText = firstContent,
                CloseButtonText = closeContent
            };

            return await uiMessageBox.ShowDialogAsync();
        }
        public static async Task<MessageBoxResult> OpenOptionBox(string title, string content, string firstContent = "确定", string secondContent = "取消", string closeContent = "关闭")
        {
            var uiMessageBox = new MessageBox
            {
                Title = title,
                Content = content,
                PrimaryButtonText = firstContent,
                SecondaryButtonText = secondContent,
                CloseButtonText = closeContent
            };

            return await uiMessageBox.ShowDialogAsync();
        }
    }
}
