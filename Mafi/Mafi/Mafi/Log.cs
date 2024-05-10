// Decompiled with JetBrains decompiler
// Type: Mafi.Log
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// High throughput, culture-agnostic, timezone-agnostic logging system. All messages are formatted with invariant
  /// culture. Please remember that date format of invariant culture is MM/dd/YYYY so format date explicitly by its
  /// ToString method if needed.
  /// </summary>
  /// <remarks>
  /// PERF: All log methods are calling the <see cref="M:Mafi.Log.onLogReceived(Mafi.Logging.LogType,System.String,System.Exception,System.Boolean,System.String,System.Collections.Generic.KeyValuePair{System.String,System.Int64}[],System.Collections.Generic.KeyValuePair{System.String,System.String}[])" /> method directly to avoid unnecessary function
  /// calls.
  /// </remarks>
  public static class Log
  {
    public const int LOG_FILE_RETENTION_DAYS = 5;
    [ThreadStatic]
    private static Action<LogEntry> s_logReceivedThreadStatic;
    [ThreadStatic]
    private static int s_isInLogReceivedThreadStatic;
    [ThreadStatic]
    private static string s_lastLog;
    /// <summary>
    /// Matches paths in stack trace in format "in D:\Path\To\src\Mafi\Class.cs" (or starting with "at ...") and
    /// removes the absolute path so that all paths are relative to the project.
    /// </summary>
    private static readonly Regex MAKE_PATHS_RELATIVE;
    private static readonly Regex CLEAR_HEX_FILES;
    private static Option<ISimStepProvider> s_simStepProvider;
    private static readonly ConcurrentBag<string> s_debugInfos;
    private static ConcurrentDictionary<string, int> s_priorWarnings;

    public static LogType AcceptedLogTypes { get; set; }

    /// <summary>
    /// Log received callback. Note that the handler have to be thread safe!
    /// </summary>
    public static event Action<LogEntry> LogReceived;

    /// <summary>
    /// Thread-static version of <see cref="E:Mafi.Log.LogReceived" /> event. Useful in tests that run in parallel.
    /// </summary>
    public static event Action<LogEntry> LogReceivedThreadStatic
    {
      add => Log.s_logReceivedThreadStatic += value;
      remove => Log.s_logReceivedThreadStatic -= value;
    }

    public static IEnumerable<string> DebugInfos => (IEnumerable<string>) Log.s_debugInfos;

    public static void RegisterSimStepProvider(ISimStepProvider simStepProvider)
    {
      Assert.That<bool>(simStepProvider.IsTerminated).IsFalse();
      Log.s_simStepProvider = Option<ISimStepProvider>.Some(simStepProvider);
    }

    /// <summary>
    /// Logs a debug message. This call will be removed in release mode.
    /// </summary>
    [Conditional("DEBUG")]
    public static void Debug(string message)
    {
      if ((Log.AcceptedLogTypes & LogType.Debug) <= (LogType) 0)
        return;
      Log.onLogReceived(LogType.Debug, "[DEBUG] " + message);
    }

    /// <summary>Logs a game progress message.</summary>
    public static void GameProgress(
      string message,
      KeyValuePair<string, long>[] additionalIntegers = null,
      KeyValuePair<string, string>[] additionalStrings = null)
    {
      if ((Log.AcceptedLogTypes & LogType.GameProgress) <= (LogType) 0)
        return;
      Log.onLogReceived(LogType.GameProgress, message, additionalIntData: additionalIntegers, additionalStringData: additionalStrings);
    }

    /// <summary>Logs an information message.</summary>
    public static void Info(string message)
    {
      if ((Log.AcceptedLogTypes & LogType.Info) <= (LogType) 0)
        return;
      Log.onLogReceived(LogType.Info, message);
    }

    /// <summary>Logs a debug info message.</summary>
    public static void InfoDebug(string message)
    {
      Log.s_debugInfos.Add(message);
      if ((Log.AcceptedLogTypes & LogType.Info) <= (LogType) 0)
        return;
      Log.onLogReceived(LogType.Info, message);
    }

    /// <summary>
    /// Logs a warning message. Use this for logging non-critical issues that are fully recoverable and have
    /// low priority for fixing.
    /// </summary>
    public static void Warning(string message)
    {
      if ((Log.AcceptedLogTypes & LogType.Warning) <= (LogType) 0)
        return;
      Log.onLogReceived(LogType.Warning, message, generateStackTrace: true);
    }

    /// <summary>
    /// Logs a warning message. Use this for logging non-critical issues that are fully recoverable and have
    /// low priority for fixing. If the warning has ever been logged before with the same string, it won't
    /// be repeated.
    /// </summary>
    public static void WarningOnce(string message)
    {
      if ((Log.AcceptedLogTypes & LogType.Warning) <= (LogType) 0 || !Log.s_priorWarnings.TryAdd(message, 1))
        return;
      Log.onLogReceived(LogType.Warning, message, generateStackTrace: true);
    }

    [Conditional("DEBUG")]
    public static void Warning_DebugOnly(string message)
    {
      if ((Log.AcceptedLogTypes & LogType.Warning) <= (LogType) 0)
        return;
      Log.onLogReceived(LogType.Warning, "[DEBUG] " + message, generateStackTrace: true);
    }

    /// <summary>
    /// Logs an error message. Use this for issues that need fixing.
    /// </summary>
    public static void Error(string message)
    {
      if ((Log.AcceptedLogTypes & LogType.Error) <= (LogType) 0)
        return;
      Log.onLogReceived(LogType.Error, message, generateStackTrace: true);
    }

    [Conditional("DEBUG")]
    public static void Error_DebugOnly(string message)
    {
      if ((Log.AcceptedLogTypes & LogType.Error) <= (LogType) 0)
        return;
      Log.onLogReceived(LogType.Error, message, generateStackTrace: true);
    }

    /// <summary>
    /// Logs an exception. Prefer this to <see cref="M:Mafi.Log.Error(System.String)" /> since we get much more info from the exception such as
    /// inner exceptions and stack trace.
    /// </summary>
    public static void Exception(System.Exception ex, string message = null)
    {
      if ((Log.AcceptedLogTypes & LogType.Exception) <= (LogType) 0)
        return;
      Log.onLogReceived(LogType.Exception, message ?? "", ex, additionalStringData: new KeyValuePair<string, string>[1]
      {
        Make.Kvp<string, string>("exception", ex.GetType().Name)
      });
    }

    public static bool IsLogged(LogType type) => (Log.AcceptedLogTypes & type) > (LogType) 0;

    public static void Custom(LogType type, string message, string customStackTrace = null)
    {
      if ((Log.AcceptedLogTypes & type) <= (LogType) 0)
        return;
      Log.onLogReceived(type, message, customStackTrace: customStackTrace);
    }

    private static void onLogReceived(
      LogType type,
      string message,
      System.Exception exception = null,
      bool generateStackTrace = false,
      string customStackTrace = null,
      KeyValuePair<string, long>[] additionalIntData = null,
      KeyValuePair<string, string>[] additionalStringData = null)
    {
      if (Log.s_isInLogReceivedThreadStatic > 0)
      {
        if (Log.s_isInLogReceivedThreadStatic > 3)
          return;
        message = "LOG WITHIN A LOG: " + message;
      }
      if (Log.s_lastLog == message)
        return;
      Log.s_lastLog = message;
      ++Log.s_isInLogReceivedThreadStatic;
      string stackTrace = customStackTrace ?? (exception != null || !generateStackTrace ? (string) null : Assert.GetCleanStackTrace(3));
      LogEntry logEntry = new LogEntry(type, message, ThreadUtils.ThreadNameFast, Log.TryGetSimStep(), stackTrace, exception, additionalIntData, additionalStringData);
      try
      {
        Action<LogEntry> logReceived = Log.LogReceived;
        if (logReceived != null)
          logReceived(logEntry);
        Action<LogEntry> receivedThreadStatic = Log.s_logReceivedThreadStatic;
        if (receivedThreadStatic == null)
          return;
        receivedThreadStatic(logEntry);
      }
      catch (System.Exception ex)
      {
        Log.Exception(ex, "Exception during logging.");
      }
      finally
      {
        --Log.s_isInLogReceivedThreadStatic;
      }
    }

    public static int? TryGetSimStep()
    {
      Option<ISimStepProvider> simStepProvider = Log.s_simStepProvider;
      if (simStepProvider.HasValue)
      {
        if (!simStepProvider.Value.IsTerminated)
          return new int?(simStepProvider.Value.CurrentSimStep);
        Log.s_simStepProvider = Option<ISimStepProvider>.None;
      }
      return new int?();
    }

    public static void ClearAllCallbacks()
    {
      Log.LogReceived = (Action<LogEntry>) null;
      Log.s_logReceivedThreadStatic = (Action<LogEntry>) null;
    }

    public static void ClearPriorWarnings()
    {
      Log.s_priorWarnings = new ConcurrentDictionary<string, int>();
    }

    public static string CleanStackTrace(string stackTrace)
    {
      return Log.CLEAR_HEX_FILES.Replace(stackTrace, "");
    }

    static Log()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: reference to a compiler-generated field
      Log.\u003CAcceptedLogTypes\u003Ek__BackingField = LogType.All;
      Log.MAKE_PATHS_RELATIVE = new Regex("in .*\\\\src\\\\(.*):(line )?([0-9]+)", RegexOptions.Compiled);
      Log.CLEAR_HEX_FILES = new Regex("in <[a-f0-9]*>:[0-9]+", RegexOptions.Compiled);
      Log.s_debugInfos = new ConcurrentBag<string>();
      Log.s_priorWarnings = new ConcurrentDictionary<string, int>();
    }
  }
}
