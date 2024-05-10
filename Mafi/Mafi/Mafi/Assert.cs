// Decompiled with JetBrains decompiler
// Type: Mafi.Assert
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Logging;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Debug-only assertions for ensuring that values are as expected. USE THIS EXTENSIVELY. Because those assertions
  /// are debug-only DO NOT rely on them. It's like little unit tests within methods where normal unit tests cannot
  /// reach.
  /// 
  /// Primary use is for places where certain values should have certain value 100% and you wanted just to make sure.
  /// 
  /// Consider usage of <see cref="T:Mafi.Log" /> class for logging of warnings or errors.
  /// </summary>
  public static class Assert
  {
    /// <summary>
    /// Assertions will be performed only when the project is compiled with this conditional.
    /// </summary>
    /// <remarks>
    /// We define this to be equal as Unity Assert condition because Unity keeps re-generating its project file and
    /// deleting our defined constants!
    /// </remarks>
    public const string ASSERT_CONDITIONAL = "MAFI_ASSERTIONS";
    public const string ASSERT_CONDITIONAL_DEBUG_ONLY = "MAFI_ASSERTIONS_DEBUG_ONLY";
    /// <summary>Whether asserts are currently enabled.</summary>
    public const bool ENABLED = true;
    [ThreadStatic]
    private static Action<LogEntry> s_assertFiredThreadStatic;
    [ThreadStatic]
    private static string s_lastAssertion;

    /// <summary>
    /// Event that is fired when an assertion fails. Note that the handler have to be thread safe!
    /// </summary>
    /// <remarks>This event is internal to not be exposed to mods.</remarks>
    public static event Action<LogEntry> AssertFired;

    /// <summary>
    /// Thread-static version of <see cref="E:Mafi.Assert.AssertFired" /> event. Useful in tests that run in parallel.
    /// </summary>
    public static event Action<LogEntry> AssertFiredThreadStatic
    {
      add => Assert.s_assertFiredThreadStatic += value;
      remove => Assert.s_assertFiredThreadStatic -= value;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void Fail(string message) => Assert.log(nameof (Fail), message);

    [Conditional("MAFI_ASSERTIONS")]
    public static void FailAssertion(string assertion, string message)
    {
      Assert.log(assertion, message, skipExtraFrames: 1);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void Fail<T0>(string message, T0 arg0)
    {
      Assert.log(nameof (Fail), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void Fail<T0, T1>(string message, T0 arg0, T1 arg1)
    {
      Assert.log(nameof (Fail), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void Fail<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
      Assert.log(nameof (Fail), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Fail_DebugOnly(string message) => Assert.log("Fail", message);

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Fail_DebugOnly<T0>(string message, T0 arg0)
    {
      Assert.log("Fail", string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Fail_DebugOnly<T0, T1>(string message, T0 arg0, T1 arg1)
    {
      Assert.log("Fail", string.Format(message, (object) arg0, (object) arg1));
    }

    public static void AssertTrue(this bool x, string message = "")
    {
      Assert.That<bool>(x).IsTrue(message);
    }

    /// <summary>
    /// Starts assertion about value of type <typeparamref name="T" />.
    /// </summary>
    [MustUseReturnValue("Must use the return value to actually perform the assertion otherwise it is no-op.")]
    [Pure]
    public static Assertion<T> That<T>(T value) => new Assertion<T>(value);

    private static void log(
      string assertion,
      string message,
      Exception exception = null,
      int skipExtraFrames = 0)
    {
      string message1 = string.IsNullOrEmpty(message) ? assertion : message + "\nASSERT: " + assertion;
      if (Assert.s_lastAssertion == message1)
        return;
      Assert.s_lastAssertion = message1;
      string cleanStackTrace = exception == null ? Assert.GetCleanStackTrace(3 + skipExtraFrames) : (string) null;
      LogEntry logEntry = new LogEntry(LogType.Assert, message1, ThreadUtils.ThreadNameFast, Log.TryGetSimStep(), cleanStackTrace, exception);
      Action<LogEntry> assertFired = Assert.AssertFired;
      if (assertFired != null)
        assertFired(logEntry);
      Action<LogEntry> firedThreadStatic = Assert.s_assertFiredThreadStatic;
      if (firedThreadStatic == null)
        return;
      firedThreadStatic(logEntry);
    }

    /// <summary>
    /// Cleans given stack trace or generates new stack trace if given stack trace is null.
    /// </summary>
    internal static string GetCleanStackTrace(int skipFrames)
    {
      return new StackTrace(skipFrames, false).ToString();
    }

    public static void ClearAllCallbacks()
    {
      Assert.AssertFired = (Action<LogEntry>) null;
      Assert.s_assertFiredThreadStatic = (Action<LogEntry>) null;
    }
  }
}
