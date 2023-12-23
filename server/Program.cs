using OIChatRoomServer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OIChatRoomServer
{
    internal class Program
    {
        public static string NAME = "OI Chatroom Server";
        public static string NAMECODE = "OiChatRoomServer";
        public static string VERSION = "0.1.0.0";
        public static List<string> ProgramArgs = new();
        public static void PrintHelpInfo()
        {
            Log.DisplayLine(FluentConsole.Yellow, "OI Chatroom Server 帮助信息");
            Log.DisplayLine(FluentConsole.White, "============================");
            Log.DisplayLine(FluentConsole.DarkYellow, "参数");
            Log.DisplayLine(FluentConsole.Blue, "-help 显示这个页面");
            Log.DisplayLine(FluentConsole.Blue, "-license 将开放源代码许可保存到文件并打开");
            Log.DisplayLine(FluentConsole.Blue, "-license display 将开放源代码许可输出到控制台");
        }
        public static void OpenLicenses()
        {
            try
            {
                Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OIChatRoomServer.Resources.licenses.txt");
                if (stream == null)
                {
                    return;
                }
                string content = new StreamReader(stream).ReadToEnd();
                File.WriteAllText("LICENSE.txt", content, Encoding.UTF8);
                Console.Clear();
                Console.WriteLine("\nThe license has been saved in the run directory under the file name LICENSE.txt");
                Console.WriteLine("已在运行目录中以 LICENSE.txt 的文件名保存许可证");
                Console.WriteLine("\nIf you need to display the license directly on the console, use the parameter -displayLicense");
                Console.WriteLine("如果需要直接将许可证显示在控制台，请使用参数 -license display");
                Console.WriteLine("\nPress any key to continue");
                Console.WriteLine("按下任意键继续");
                Console.ReadKey();
                Console.Clear();
            }
            catch
            {
                Log.Error("Main");
                FluentConsole
                    .Red.Text("Error. Please contact to author")
                    .NewLine();
                Console.ReadKey();
            }
        }
        public static void DisplayLicenses()
        {
            try
            {
                Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OIChatRoomServer.Resources.licenses.txt");
                if (stream == null)
                {
                    return;
                }
                string content = new StreamReader(stream).ReadToEnd();
                Console.Clear();
                Console.WriteLine(content);
                Console.WriteLine("\nPress any key to continue");
                Console.WriteLine("按下任意键继续");
                Console.ReadKey();
            }
            catch
            {
                Log.Error("Main");
                FluentConsole
                    .Red.Text("Error. Please contact to author")
                    .NewLine();
                Console.ReadKey();
            }
        }
        static void Main(string[] args)
        {
            ProgramArgs = new List<string>(args);
            if (ProgramArgs.Count == 1 && ProgramArgs[0] == "-help")
            {
                PrintHelpInfo();
                return;
            }
            if (ProgramArgs.Count == 1 && ProgramArgs[0] == "-license")
            {
                OpenLicenses();
                return;
            }
            if (ProgramArgs.Count == 2 && ProgramArgs[0] == "-license" && ProgramArgs[1] == "display")
            {
                DisplayLicenses();
                return;
            }
            ServerStartOption startOption = ConfigUtils.GetStartOption();
            Server server = new(startOption);
            server.SetInstance();
        }
    }
}
