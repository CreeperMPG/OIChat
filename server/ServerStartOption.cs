using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OIChatRoomServer
{
    internal class ServerStartOption
    {
        public int Port;
        public int MaxConnection;
        public string ServerName;
        public int MaxWarnTime;
        public ServerStartOption()
        {
            Port = 18056;
            MaxConnection = 1024;
            ServerName = "OI Chatroom";
            MaxWarnTime = 512;
        }
        public ServerStartOption(int port, int maxConnection, string serverName, int maxWarnTime)
        {
            Port = port;
            MaxConnection = maxConnection;
            ServerName = serverName;
            MaxWarnTime = maxWarnTime;
        }
    }
}
