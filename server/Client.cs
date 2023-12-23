using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OIChatRoomServer
{
    internal class Client
    {
        public int ConnectionID = 0;
        public string Name = string.Empty;
        public bool IsLogin = false;
        public IPEndPoint? ClientIP = null;
        public Socket? ClientSocket = null;
        public long LastMessageTime = 0;
    }
}
