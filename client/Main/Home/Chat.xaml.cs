using OIChatRoomClient.TcpClient;
using OIChatRoomClient.Utils;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace OIChatRoomClient.Main.Home
{
    /// <summary>
    /// Chat.xaml 的交互逻辑
    /// </summary>
    public partial class Chat : Page
    {
        public enum ChatState
        {
            NotConnected, EnterName, Chat
        }
        public static ChatState state = ChatState.NotConnected;
        public static Client? client = null;
        public Chat()
        {
            InitializeComponent();
            Init();
            NetUtils.PingLocal(this);
            MessageTextBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    ConfirmSend();
                }
            };
            MessageConfirmButton.Click += (s, e) =>
            {
                ConfirmSend();
            };
            DeleteButton.Click += (s, e) =>
            {
                client?.ClientSocket.Close();
                client = null;
                state = ChatState.NotConnected;
                Init();
            };
        }
        private void Init()
        {
            switch (state)
            {
                case ChatState.NotConnected:
                    ChangeTextBoxDocument("在下面的输入框中输入服务器地址");
                    MessageTextBox.PlaceholderText = "服务器地址";
                    break;
                case ChatState.EnterName:
                    ChangeTextBoxDocument("请在下方输入框输入用户名");
                    MessageTextBox.PlaceholderText = "用户名";
                    break;
                case ChatState.Chat:
                    TextBoxDocument.Blocks.Clear();
                    MessageTextBox.PlaceholderText = "输入信息";
                    break;
            }
        }
        private void ConfirmSend()
        {
            switch (state)
            {
                case ChatState.NotConnected:
                    Navigate();
                    MessageTextBox.Text = "";
                    break;
                case ChatState.EnterName:
                    try
                    {
                        client.SendData(ByteUtils.combine(new byte[] { 0xCF, 0x7E, 0x53, 0xAE }, MessageTextBox.Text)); // 注册名字
                        client.SendData(ByteUtils.combine(new byte[] { 0xCF, 0x12, 0x9E, 0xD3 }, "1")); // 加入聊天室
                    }
                    catch
                    {
                        ChangeTextBoxDocument("加入聊天室失败");
                    }
                    MessageTextBox.PlaceholderText = "输入信息";
                    state = ChatState.Chat;
                    TextBoxDocument.Blocks.Clear();
                    MessageTextBox.Text = "";
                    break;
                case ChatState.Chat:
                    if (MessageTextBox.Text != string.Empty)
                    {
                        try
                        {
                            client.SendData(ByteUtils.combine(new byte[] { 0xCF, 0x7B, 0xC4, 0xFF }, MessageTextBox.Text));
                        }
                        catch
                        {
                            AddTextBoxDocument("发送信息失败");
                        }
                        AddTextBoxDocument(MessageTextBox.Text);
                        MessageTextBox.Text = "";
                    }
                    break;
            }
        }
        public void ChangeTextBoxDocument(string text)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                TextBoxDocument.Blocks.Clear();
                Paragraph navigating = new();
                navigating.Inlines.Add(text);
                TextBoxDocument.Blocks.Add(navigating);
            }));
        }
        public void AddTextBoxDocument(string text)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                Paragraph navigating = new();
                navigating.Inlines.Add(text);
                TextBoxDocument.Blocks.Add(navigating);
            }));
        }
        public void Connected()
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                ChangeTextBoxDocument("请在下方输入框输入用户名");
                state = ChatState.EnterName;
                MessageTextBox.IsEnabled = true;
                MessageTextBox.PlaceholderText = "用户名";
            }));
        }
        public void EnableTextBox()
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                MessageTextBox.IsEnabled = true;
            }));
        }
        public void Navigate()
        {
            if (client == null)
            {
                MessageTextBox.IsEnabled = false;
                ChangeTextBoxDocument("正在连接");
                client = new Client(this);
                string[] ipPort = MessageTextBox.Text.Split(':');
                if (ipPort.Length > 2)
                {
                    ChangeTextBoxDocument("不是正确的服务器地址");
                }
                else if (ipPort.Length == 2)
                {
                    int port = 18056;
                    if (!int.TryParse(ipPort[1], out port))
                    {
                        ChangeTextBoxDocument("不是正确的服务器地址");
                    }
                    else
                    {
                        client.ConnectServer(ipPort[0], port);
                    }
                }
                else if (ipPort.Length == 1)
                {
                    client.ConnectServer(ipPort[0], 18056);
                }
                else
                {
                    ChangeTextBoxDocument("不是正确的服务器地址");
                }
            }
        }
    }
}
