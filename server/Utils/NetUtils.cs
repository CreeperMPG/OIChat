using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OIChatRoomServer.Utils
{
    internal static class NetUtils
    {
        public static IPAddress GetAddressIP()
        {
            IPAddress resultIp = IPAddress.Loopback;
            foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    resultIp = ipAddress;
                }
            }
            return resultIp;
        }
    }
}
