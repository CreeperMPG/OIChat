using OIChatRoomServer.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OIChatRoomServer
{
    internal class Server
    {
        public static Server? Instance;
        public ServerStartOption ServerConfig;
        public int ConnectionCount;
        public Dictionary<int, ChatRoom> Chatrooms = new();
        public Dictionary<int, Client> Clients = new();

        public void UpdateOnlineCount()
        {
            Console.Title = $"在线人数: {ConnectionCount} - OI Chatroom Server {Program.VERSION}";
        }
        public void CloseConnection(int id, bool displayMessage = true)
        {
            try
            {
                Clients[id].ClientSocket.Close();
            }
            catch { }
            if (displayMessage)
            {
                Log.Info("Listener");
                FluentConsole
                    .Yellow.Text("[Disconnect] ");
                if (!Clients.ContainsKey(id) || Clients[id].Name == string.Empty)
                {
                    FluentConsole
                        .White.Text($"ID ")
                        .Yellow.Text($"{id} ");
                }
                else
                {
                    FluentConsole
                        .Cyan.Text(Clients[id].Name)
                        .White.Text($" (ID {id}) ");
                }
                FluentConsole
                    .Yellow.Text("断开连接")
                    .NewLine();
            }
            if (Clients.ContainsKey(id))
            {
                Clients.Remove(id);
            }
            ConnectionCount--;
            UpdateOnlineCount();
        }
        public void Listen(object extraValue)
        {
            Client client = (Client)extraValue;
            Socket? clientSocket = client.ClientSocket;
            client.ClientIP = (IPEndPoint?)clientSocket.RemoteEndPoint;
            client.ClientSocket = clientSocket;
            if (ConnectionCount >= ServerConfig.MaxConnection)
            {
                clientSocket.Send(new byte[] { 0xFC, 0x84, 0xFF, 0x3A });
                Log.Warn("Listener");
                FluentConsole
                    .White.Text("服务器")
                    .Yellow.Text("已满")
                    .NewLine();
                Thread.Sleep(1000);
                clientSocket.Close();
                return;
            }
            int clientID = -1;
            Random clientRandom = new();
            while (clientID == -1 || Clients.ContainsKey(clientID))
            {
                clientID = clientRandom.Next(0, ServerConfig.MaxConnection);
            }
            ConnectionCount++;
            Log.Info("Listener");
            FluentConsole
                .Yellow.Text("[Connect] ")
                .White.Text($"ID {clientID} [{client.ClientIP}] ")
                .Yellow.Text("已连接")
                .NewLine();
            UpdateOnlineCount();
            client.ConnectionID = clientID;
            Clients.Add(clientID, client);
            try
            {
                byte[] buffer = new byte[8192];
                int bufferSize = 0;
                while ((bufferSize = clientSocket.Receive(buffer)) > 0)
                {
                    byte[] receiveBytes = ByteUtils.GetFrontBytes(buffer, bufferSize);
                    byte[] frontBytes = ByteUtils.GetFrontBytes(receiveBytes);
                    byte[] backBytes = ByteUtils.GetBackBytes(receiveBytes);
                    string receiveString = Encoding.UTF8.GetString(backBytes, 0, backBytes.Length);
                    if (!client.IsLogin)
                    {
                        if (ByteUtils.IsEqual(frontBytes, 0xCF, 0x7E, 0x53, 0xAE))
                        {
                            client.Name = receiveString;
                            client.IsLogin = true;
                            Log.Info("Listener");
                            FluentConsole
                                .White.Text($"ID {clientID} ")
                                .Yellow.Text("使用 ")
                                .Cyan.Text(client.Name)
                                .Yellow.Text(" 的身份")
                                .Green.Text("登录")
                                .NewLine();
                        }
                        else if (ByteUtils.IsEqual(frontBytes, 0xCF, 0xAD, 0x42, 0x7B))
                        {
                            Log.Info("Listener");
                            FluentConsole
                                .White.Text($"ID {clientID} ")
                                .Green.Text("Ping ")
                                .Yellow.Text("了服务器")
                                .NewLine();
                            clientSocket.Close();
                            CloseConnection(clientID, false);
                            return;
                        }
                        else
                        {
                            Log.Warn("Listener");
                            FluentConsole
                                .Yellow.Text($"ID 为 {clientID} 的用户未登录但是发送了消息")
                                .White.Text("，")
                                .Red.Text("此用户客户端版本可能与服务端不相符")
                                .NewLine();
                        }
                    }
                    else if (ByteUtils.IsEqual(frontBytes, 0xCF, 0x12, 0x9E, 0xD3))
                    {
                        int chatroomId = -1;
                        bool parseResult = int.TryParse(receiveString, out chatroomId);
                        if (!parseResult)
                        {
                            Log.Warn("Listener");
                            FluentConsole
                                .Cyan.Text(client.Name)
                                .White.Text($" ID {clientID} ")
                                .Yellow.Text("请求切换的聊天室 ")
                                .Blue.Text(receiveString)
                                .Yellow.Text(" 无法识别")
                                .White.Text("，")
                                .Red.Text("此用户客户端版本可能与服务端不相符")
                                .NewLine();
                        }
                        else if (!Chatrooms.ContainsKey(chatroomId))
                        {
                            Log.Warn("Listener");
                            FluentConsole
                                .Cyan.Text(client.Name)
                                .White.Text($" ID {clientID} ")
                                .Yellow.Text("请求切换的聊天室 ")
                                .Blue.Text(receiveString)
                                .Yellow.Text(" 无法找到")
                                .NewLine();
                        }
                        else
                        {
                            Log.Info("Listener");
                            FluentConsole
                                .Cyan.Text(client.Name)
                                .White.Text($" ID {clientID} ")
                                .Yellow.Text("加入聊天室 ")
                                .Blue.Text(chatroomId)
                                .NewLine();
                            Chatrooms[chatroomId].Listen(client);
                        }
                    }
                    else if (ByteUtils.IsEqual(frontBytes, 0xCF, 0x51, 0x9A, 0x68))
                    {
                        Log.Info("Listener");
                        FluentConsole
                            .Cyan.Text(client.Name)
                            .White.Text($" ID {clientID} ")
                            .Yellow.Text("未加入聊天室但请求退出聊天室")
                            .NewLine();
                    }
                    else if (ByteUtils.IsEqual(frontBytes, 0xCF, 0x7B, 0xC4, 0xFF))
                    {
                        Log.Info("Listener");
                        FluentConsole
                            .Cyan.Text(client.Name)
                            .White.Text($" ID {clientID} ")
                            .Yellow.Text("未加入聊天室但发送消息")
                            .NewLine();
                    }
                    else
                    {
                        Log.Warn("Listener");
                        FluentConsole
                            .Cyan.Text(client.Name)
                            .White.Text($" ID {clientID} ")
                            .Yellow.Text("发送的信息无法识别")
                            .White.Text("，")
                            .Red.Text("此用户客户端版本可能与服务端不相符")
                            .NewLine();
                    }
                }
            }
            catch
            {
                CloseConnection(clientID);
                return;
            }
            CloseConnection(clientID);
            return;
        }
        public Server(ServerStartOption option)
        {
            Console.Title = "Starting";
            FluentConsole
                .Blue.Text("Starting Server (Version ")
                .Cyan.Text(Program.VERSION)
                .Blue.Text(")")
                .NewLine();
            FluentConsole
                .Cyan.Text("Use the parameter -help to get help")
                .NewLine()
                .Cyan.Text("Use the parameter -license [display] to view LICENSE")
                .NewLine();
            ServerConfig = option;
            ConnectionCount = 0;
            Chatrooms.Add(1, new ChatRoom(this));
            UpdateOnlineCount();
            try
            {
                Socket serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, ServerConfig.Port));
                serverSocket.Listen(ServerConfig.MaxConnection + 10);
                Log.Info("Main");
                FluentConsole
                    .Green.Text($"Started {ServerConfig.ServerName} on port ")
                    .Blue.Text(ServerConfig.Port)
                    .NewLine();
                while (true)
                {
                    Client client = new();
                    Socket clientSocket = serverSocket.Accept();
                    client.ClientSocket = clientSocket;
                    Thread messageThread = new(Listen);
                    messageThread.Start(client);
                }
            }
            catch (Exception e)
            {
                ExceptionUtils.Exception(e);
            }
        }
        public void SetInstance()
        {
            Instance = this;
        }
    }
}
