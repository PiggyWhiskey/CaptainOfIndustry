using Mafi;

using System;
using System.Reflection;

namespace TerrainTower.Extras
{
    [GlobalDependency(RegistrationMode.AsSelf, false)]
    public static class Logger
    {
        private static readonly string s_prefix = string.Format("[{0}] {1} ", Assembly.GetExecutingAssembly().GetName().Name, ++m_msgCount);
        private static int m_msgCount = 0;

        public static string AddPrefix(this string message) => $"{s_prefix}{message}";

        public static void Error(object message) => log(message, Mafi.Log.Error);

        public static void Exception(Exception e, object message) => log(e, message, Mafi.Log.Exception);

        public static void Info(object message) => log(message, Mafi.Log.Info);

        public static void InfoDebug(object message) => log(message, Mafi.Log.InfoDebug);

        public static void Warning(object message) => log(message, Mafi.Log.Warning);

        public static void WarningOnce(object message) => log(message, Mafi.Log.WarningOnce);

        private static void log(object message, Action<string> callback = null)
        {
            Console.WriteLine(message?.ToString().AddPrefix());
            callback?.Invoke(message?.ToString().AddPrefix());
        }

        private static void log(Exception e, object message, Action<Exception, string> callback = null)
        {
            Console.WriteLine(message?.ToString().AddPrefix());
            callback?.Invoke(e, message?.ToString().AddPrefix());
        }
    }
}