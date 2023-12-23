using System.IO;
using System.Reflection;

namespace OIChatRoomClient.Utils
{
    internal static class ResourceUtils
    {
        public static string getResourceString(string path)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream resStream = assembly.GetManifestResourceStream(path);
                StreamReader reader = new System.IO.StreamReader(resStream);
                return reader.ReadToEnd();
            }
            catch
            {
                MessageBoxUtils.OpenMessageBox("错误", "资源无法读取");
                return string.Empty;
            }
        }
    }
}
