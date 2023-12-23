using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace OIChatRoomServer.Utils
{
    internal static class ByteUtils
    {
        public static byte[] Combine(byte[] a, byte[] b)
        {
            byte[] combinedBytes = new byte[a.Length + b.Length];
            Array.Copy(a, combinedBytes, a.Length);
            Array.Copy(b, 0, combinedBytes, a.Length, b.Length);
            return combinedBytes;
        }
        public static byte[] Combine(byte[] a, string str)
        {
            byte[] b = ToByte(str);
            return Combine(a, b);
        }
        public static byte[] ToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        public static byte[] GetFrontBytes(byte[] bytes, int length)
        {
            if (bytes.Length <= length)
            {
                return bytes;
            }
            byte[] result = new byte[length];
            Array.Copy(bytes, 0, result, 0, length);
            return result;
        }
        public static byte[] GetFrontBytes(byte[] bytes)
        {
            if (bytes.Length <= 4)
            {
                return bytes;
            }
            byte[] result = new byte[4];
            Array.Copy(bytes, 0, result, 0, 4);
            return result;
        }
        public static byte[] GetBackBytes(byte[] bytes)
        {
            if (bytes.Length <= 4)
            {
                return Array.Empty<byte>();
            }
            int length = bytes.Length - 4;
            ArraySegment<byte> segment = new(bytes, 4, length);
            return segment.ToArray();
        }
        public static bool IsEqual(byte[] a, byte[] b)
        {
            return a.SequenceEqual(b);
        }
        public static bool IsEqual(byte[] x, byte a, byte b, byte c, byte d)
        {
            return x.SequenceEqual(new byte[] { a, b, c, d });
        }
        public static void RemoveEmpty(ref byte[] bytes)
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
