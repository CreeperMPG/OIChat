using System;
using System.Linq;
using System.Text;

namespace OIChatRoomClient.Utils
{
    internal static class ByteUtils
    {
        public static byte[] combine(byte[] a, byte[] b)
        {
            byte[] combinedBytes = new byte[a.Length + b.Length];
            Array.Copy(a, combinedBytes, a.Length);
            Array.Copy(b, 0, combinedBytes, a.Length, b.Length);
            return combinedBytes;
        }
        public static byte[] combine(byte[] a, string str)
        {
            byte[] b = toByte(str);
            return combine(a, b);
        }
        public static byte[] toByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        public static byte[] getFrontBytes(byte[] bytes, int length)
        {
            if (bytes.Length <= length)
            {
                return bytes;
            }
            byte[] result = new byte[length];
            Array.Copy(bytes, 0, result, 0, length);
            return result;
        }
        public static byte[] getFrontBytes(byte[] bytes)
        {
            if (bytes.Length <= 4)
            {
                return bytes;
            }
            byte[] result = new byte[4];
            Array.Copy(bytes, 0, result, 0, 4);
            return result;
        }
        public static byte[] getBackBytes(byte[] bytes)
        {
            if (bytes.Length <= 4)
            {
                return new byte[0];
            }
            int length = bytes.Length - 4;
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 4, length);
            return segment.ToArray();
        }
        public static bool isEqual(byte[] a, byte[] b)
        {
            return a.SequenceEqual(b);
        }
        public static bool isEqual(byte[] x, byte a, byte b, byte c, byte d)
        {
            return x.SequenceEqual(new byte[] { a, b, c, d });
        }
        public static void removeEmpty(ref byte[] bytes)
        {
            int count = bytes.Length;
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                if (bytes[i] == 0)
                {
                    count--;
                }
                else
                {
                    break;
                }
            }
            Array.Resize(ref bytes, count);
        }
    }
}
