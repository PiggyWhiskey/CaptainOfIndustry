using Mafi;

using System;
using System.Reflection;

namespace TerrainTower.Extras
{
    [GlobalDependency(RegistrationMode.AsSelf, false)]
    public static class Logger
    {
        private static int m_msgCount = 0;
        private static readonly string s_prefix = string.Format("[{0}]", Assembly.GetExecutingAssembly().GetName().Name);
        //[CallerMemberName] string callerName = null,
        public static void Info(string message, params object[] args)
        {
            Log.Info($"{s_prefix} {++m_msgCount} {string.Format(message, args)}");
        }

        public static void InfoDebug(string message, params object[] args)
        {
            Log.InfoDebug($"{s_prefix} {++m_msgCount} {string.Format(message, args)}");
        }

        public static void Warning(string message, params object[] args)
        {
            Log.Warning($"{s_prefix} {++m_msgCount} {string.Format(message, args)}");
        }

        public static void WarningOnce(string message, params object[] args)
        {
            Log.WarningOnce($"{s_prefix} {++m_msgCount} {string.Format(message, args)}");
        }

        public static void Error(string message, params object[] args)
        {
            Log.Error($"{s_prefix} {++m_msgCount} {string.Format(message, args)}");
        }

        public static void Exception(Exception e, string message, params object[] args)
        {
            Log.Exception(e, $"{s_prefix} {++m_msgCount} {string.Format(message, args)}");
        }
    }
}
