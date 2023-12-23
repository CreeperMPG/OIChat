using OIChatRoomClient.Main.Home;
using OIChatRoomClient.Utils;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace OIChatRoomClient.TcpClient
{
    public class Client
    {
        public Socket ClientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        public Chat chat;
        public Client(Chat chat)
        {
            this.chat = chat;
        }
        public void Listen(object extraData)
        {
            try
            {
                KeyValuePair<string, int> server = (KeyValuePair<string, int>)extraData;
                ClientSocket.Connect(server.Key, server.Value);
                chat.Connected();
                byte[] buffer = new byte[8192];
                int bufferSize = 0;
                while ((bufferSize = ClientSocket.Receive(buffer)) > 0)
                {
                    byte[] receiveBytes = ByteUtils.getFrontBytes(buffer, bufferSize);
                    byte[] frontBytes = ByteUtils.getFrontBytes(receiveBytes);
                    byte[] backBytes = ByteUtils.getBackBytes(receiveBytes);
                    string receiveString = Encoding.UTF8.GetString(backBytes, 0, backBytes.Length);
                    if (ByteUtils.isEqual(frontBytes, 0xFC, 0x84, 0xFF, 0x3A))
                    {
                        chat.ChangeTextBoxDocument("服务器已满");
                    }
                    else if (ByteUtils.isEqual(frontBytes, 0xFC, 0xFF, 0xFF, 0xFF)) // 这是被踢出
                    {
                        chat.ChangeTextBoxDocument(receiveString);
                        ClientSocket.Send(new byte[] {0xFF, 0x00, 0xFF, 0x00}); // 这是确认
                    }
                    else if (ByteUtils.isEqual(frontBytes, 0xFC, 0xC4, 0x7B, 0xFF))
                    {
                        chat.AddTextBoxDocument(receiveString);
                    }
                    else
                    {
                        chat.AddTextBoxDocument($"[警告] 服务器向客户端发送的包内容无法识别，接收到长度为 {receiveString.Length} 的信息 [{receiveString}]");
                    }
                }
            }
            catch
            {
                if (Chat.state == Chat.ChatState.Chat)
                {
                    chat.AddTextBoxDocument("已断开连接，点击左下角删除可以重新开始会话");
                }
                chat.EnableTextBox();
            }
            if (Chat.state == Chat.ChatState.Chat)
            {
                chat.AddTextBoxDocument("已断开连接，点击左下角删除可以重新开始会话");
            }
            chat.EnableTextBox();
        }
        public void ConnectServer(string host, int port)
        {
            Thread messageThread = new Thread(Listen);
            messageThread.IsBackground = true;
            messageThread.Start(new KeyValuePair<string, int>(host, port));
        }
        public void SendData(byte[] data)
        {
            ClientSocket.Send(data);
        }
    }
}
