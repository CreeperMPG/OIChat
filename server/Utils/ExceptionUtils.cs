using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OIChatRoomServer.Utils
{
    internal static class ExceptionUtils
    {
        public static void Exception(Exception e, bool isContinue = true, string message = "运行时出现错误")
        {
            Log.Error("Exception Reporter");
            FluentConsole
                .Red.Text(message)
                .NewLine();
            Log.Error("Exception Reporter");
            FluentConsole
                .Yellow.Text($"错误信息\n\n{e.GetType().FullName}: {e.Message}\n\n以下是发生错误时调用栈中内容:\n\n{e.StackTrace}\n\n")
                .NewLine();
            if (isContinue)
            {
                Log.Error("Exception Reporter");
                FluentConsole
                    .DarkYellow.Text("按下任意键继续")
                    .NewLine();
                Console.ReadKey();
            }
        }
    }
}
