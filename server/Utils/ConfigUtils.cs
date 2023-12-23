using SharpConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OIChatRoomServer.Utils
{
    internal static class ConfigUtils
    {
        public static ServerStartOption GetStartOption()
        {
            try
            {
                ServerStartOption option = new();
                Configuration config = new();
                if (File.Exists("oichat.cfg"))
                {
                    config = Configuration.LoadFromFile("oichat.cfg");
                }
                else if (File.Exists("config.ini"))
                {
                    config = Configuration.LoadFromFile("config.ini");
                    Log.Info("Main");
                    FluentConsole
                        .White.Text("配置文件不存在，已发现旧版格式的配置文件 ")
                        .Yellow.Text("config.ini")
                        .White.Text("，将会尝试读取并将配置文件更新为最新的 ")
                        .Yellow.Text("oichat.cfg ")
                        .White.Text("格式")
                        .NewLine();
                }
                else
                {
                    Log.Info("Main");
                    FluentConsole
                        .Yellow.Text("oichat.cfg ")
                        .White.Text("文件不存在，将会尝试生成名为 ")
                        .Yellow.Text("oichat.cfg ")
                        .White.Text("的配置文件")
                        .NewLine();
                }
                Section section = config[Program.NAMECODE];
                Type startOptionType = typeof(ServerStartOption);
                foreach (FieldInfo field in startOptionType.GetFields())
                {
                    try
                    {
                        if (!section.Contains(field.Name))
                        {
                            section[field.Name].SetValue(field.GetValue(option).ToString());
                        }
                        object configContent = section[field.Name].GetValue(field.FieldType);
                        field.SetValue(option, configContent);
                    }
                    catch
                    {
                        Log.V1_Error("Main", $"读取配置文件出现错误：解析 {field.Name} 项时出现错误", FluentConsole.Red);
                    }
                }
                config.SaveToFile("oichat.cfg");
                /*
                IniFile ini = new("config.ini");
                Type startOptionType = typeof(ServerStartOption);
                foreach (FieldInfo field in startOptionType.GetFields())
                {
                    if (!ini.KeyExists(field.Name))
                    {
                        ini.Write(field.Name, field.GetValue(option).ToString());
                    }
                    object? confValue = null;
                    string config = ini.Read(field.Name);
                    confValue = config;
                    try
                    {
                        if (field.FieldType == typeof(int))
                        {
                            confValue = int.Parse(config);
                        }
                        else if (field.FieldType == typeof(bool))
                        {
                            confValue = bool.Parse(config);
                        }
                    }
                    catch
                    {
                        Log.V1_Error("Main", $"读取配置文件出现错误：解析 {field.Name} 项时出现错误", FluentConsole.Red);
                    }
                    field.SetValue(option, confValue);
                }*/
                return option;
            }
            catch (Exception e)
            {
                ExceptionUtils.Exception(e, false, "读取配置文件时出现错误");
                return new ServerStartOption();
            }
        }
    }
}
