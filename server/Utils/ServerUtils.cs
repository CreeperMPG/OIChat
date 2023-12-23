using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OIChatRoomServer.Utils
{
    internal static class ServerUtils
    {
        public static void KickUser(Socket socket, string reason = "你被踢出了服务器")
        {
            try
            {
                socket.Send(ByteUtils.Combine(new byte[] { 0xFC, 0xFF, 0xFF, 0xFF }, reason));
                byte[] buffer = new byte[1024];
                socket.Receive(buffer);
                socket.Close();
            }
            catch { }
        }
    }
}
