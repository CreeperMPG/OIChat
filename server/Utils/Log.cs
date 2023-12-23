namespace OIChatRoomServer.Utils
{
    internal static class Log
    {
        public static void DisplayString(FluentConsole.ITextSyntax syntax, string text)
        {
            syntax.Text(text);
        }
        public static void DisplayLine(FluentConsole.ITextSyntax syntax, string text)
        {
            DisplayString(syntax, $"{text}\n");
        }
        public static void Info(string thread)
        {
            Console.Write($"[{thread} Thread] ");
            DisplayString(FluentConsole.White, "[Info] ");
        }
        public static void Warn(string thread)
        {
            Console.Write($"[{thread} Thread] ");
            DisplayString(FluentConsole.Yellow, "[Warn] ");
        }
        public static void Error(string thread)
        {
            Console.Write($"[{thread} Thread] ");
            DisplayString(FluentConsole.Red, "[Error] ");
        }
        public static void Debug(string thread)
        {
            Console.Write($"[{thread} Thread] [Debug] ");
        }
        public static void V1_Log(string thread, LogType type, string message, FluentConsole.IAfterColorSyntax color)
        {
            FluentConsole.Text($"[{thread} Thread] ");
            FluentConsole.IAfterColorSyntax logTypeColor = FluentConsole.White;
            switch (type)
            {
                case LogType.INFO:
                    logTypeColor = FluentConsole.White;
                    break;
                case LogType.WARN:
                    logTypeColor = FluentConsole.Yellow;
                    break;
                case LogType.ERROR:
                    logTypeColor = FluentConsole.Red;
                    break;
                case LogType.DEBUG:
                    logTypeColor = FluentConsole.Blue;
                    break;
            }
            logTypeColor.Text($"[{type}] ");
            color.Text(message);
            Console.WriteLine();
        }
        public static void V1_Log(string thread, LogType type, string message)
        {
            V1_Log(thread, type, message, FluentConsole.White);
        }
        public static void V1_Info(string thread, string message, FluentConsole.IAfterColorSyntax color)
        {
            V1_Log(thread, LogType.INFO, message, color);
        }
        public static void V1_Info(string thread, string message)
        {
            V1_Log(thread, LogType.INFO, message, FluentConsole.White);
        }
        public static void V1_Warn(string thread, string message, FluentConsole.IAfterColorSyntax color)
        {
            V1_Log(thread, LogType.WARN, message, color);
        }
        public static void V1_Warn(string thread, string message)
        {
            V1_Log(thread, LogType.WARN, message, FluentConsole.White);
        }
        public static void V1_Error(string thread, string message, FluentConsole.IAfterColorSyntax color)
        {
            V1_Log(thread, LogType.ERROR, message, color);
        }
        public static void V1_Error(string thread, string message)
        {
            V1_Log(thread, LogType.ERROR, message, FluentConsole.White);
        }
        public static void V1_Debug(string thread, string message, FluentConsole.IAfterColorSyntax color)
        {
            V1_Log(thread, LogType.DEBUG, message, color);
        }
        public static void V1_Debug(string thread, string message)
        {
            V1_Log(thread, LogType.DEBUG, message, FluentConsole.White);
        }
    }
}
