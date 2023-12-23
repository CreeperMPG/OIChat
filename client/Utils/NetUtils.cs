using OIChatRoomClient.Main.Home;
using OIChatRoomClient.TcpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace OIChatRoomClient.Utils
{
    internal static class NetUtils
    {
        private async static void _pingLocal(object extraData)
        {
            Chat chat = (Chat)extraData;
            try
            {
                Socket socket = new(SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(IPAddress.Loopback, 18056);
                socket.Send(new byte[] { 0xCF, 0xAD, 0x42, 0x7B });
                socket.Close();
                MessageBoxResult result = new();
                App.Current.Dispatcher.Invoke((Action)(async () =>
                {
                    result = await MessageBoxUtils.OpenAskBox("发现本地服务器", $"是否连接本地服务器 {IPAddress.Loopback}:18056", "连接");
                }));
                if (result == MessageBoxResult.Primary)
                {
                    if (Chat.client == null)
                    {
                        Chat.client = new TcpClient.Client(chat);
                    }
                    Chat.client.ConnectServer(IPAddress.Loopback.ToString(), 18056);
                }
            }
            catch { }
        }
        public static void PingLocal(Chat chat)
        {
            Thread thread = new(_pingLocal);
            thread.IsBackground = true;
            thread.Start(chat);
        }
    }
}
