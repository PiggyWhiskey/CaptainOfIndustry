using Mafi;

using System;
using System.Reflection;

namespace TerrainTower.Extras
{
    [GlobalDependency(RegistrationMode.AsSelf, false)]
    public static class Logger
    {
        private static readonly string s_prefix = string.Format("[{0}]", Assembly.GetExecutingAssembly().GetName().Name);
        private static int m_msgCount = 0;

        public static string AddPrefix(this string message) => string.Format("{0} {1:D4} {2}", s_prefix, ++m_msgCount, message);

        public static void Error(string message, params object[] values) => Error((object)string.Format(message, values));

        public static void Error(object message) => log(message, Log.Error);

        public static void Exception(Exception e, object message) => log(e, message, Log.Exception);

        public static void Info(string message, params object[] values) => Info((object)string.Format(message, values));

        public static void Info(object message) => log(message, Log.Info);

        public static void InfoDebug(string message, params object[] values) => InfoDebug((object)string.Format(message, values));

        public static void InfoDebug(object message) => log(message, Log.InfoDebug);

        public static void Warning(string message, params object[] values) => Warning((object)string.Format(message, values));

        public static void Warning(object message) => log(message, Log.Warning);

        public static void WarningOnce(string message, params object[] values) => WarningOnce((object)string.Format(message, values));

        public static void WarningOnce(object message) => log(message, Log.WarningOnce);

        private static void log(object message, Action<string> callback = null)
        {
            string msg = message?.ToString().AddPrefix();
            Console.WriteLine(msg);
            callback?.Invoke(msg);
        }

        private static void log(Exception e, object message, Action<Exception, string> callback = null)
        {
            string msg = message?.ToString().AddPrefix();
            Console.WriteLine(msg);
            callback?.Invoke(e, msg);
        }
    }
}