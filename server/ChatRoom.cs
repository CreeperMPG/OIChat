using OIChatRoomServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OIChatRoomServer
{
    internal class ChatRoom
    {
        public int userCount = 0;
        public Server chatroomServer;
        public Dictionary<int, Client> users = new();
        private void CloseConnection(int userId)
        {
            userCount--;
            users.Remove(userId);
        }
        public ChatRoom(Server chatroomServer)
        {
            this.chatroomServer = chatroomServer;
        }
        public void Listen(Client client)
        {
            int clientID = client.ConnectionID;
            users.Add(clientID, client);
            userCount++;
            foreach (KeyValuePair<int, Client> userElement in users)
            {
                Client user = userElement.Value;
                user.ClientSocket.Send(ByteUtils.Combine(new byte[] { 0xFC, 0xC4, 0x7B, 0xFF }, $"{client.Name} 加入了聊天室"));
            }
            Socket? clientSocket = client.ClientSocket;
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
                    if (ByteUtils.IsEqual(frontBytes, 0xCF, 0x51, 0x9A, 0x68))
                    {
                        Log.Info("Listener");
                        FluentConsole
                            .White.Text(client.Name)
                            .Gray.Text($" (ID {clientID}) ")
                            .Yellow.Text("已退出聊天室").NewLine();
                        CloseConnection(clientID);
                        return;
                    }
                    else if (ByteUtils.IsEqual(frontBytes, 0xCF, 0x7B, 0xC4, 0xFF))
                    {
                        Log.Info("Message");
                        FluentConsole
                            .Cyan.Text(client.Name)
                            .White.Text(" 发送了消息: ")
                            .Magenta.Text(receiveString)
                            .NewLine();
                        foreach (KeyValuePair<int, Client> userElement in users)
                        {
                            int userID = userElement.Key;
                            Client user = userElement.Value;
                            if (userID != clientID)
                            {
                                user.ClientSocket.Send(ByteUtils.Combine(new byte[] { 0xFC, 0xC4, 0x7B, 0xFF }, $"{client.Name} 发送了: {receiveString}"));
                            }
                        }
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
    }
}
