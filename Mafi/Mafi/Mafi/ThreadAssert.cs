// Decompiled with JetBrains decompiler
// Type: Mafi.ThreadAssert
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class ThreadAssert
  {
    [ThreadStatic]
    private static bool s_isSetup;
    [ThreadStatic]
    private static bool s_isDisabled;
    [ThreadStatic]
    private static bool s_isMainThread;
    [ThreadStatic]
    private static bool s_isSimThread;
    [ThreadStatic]
    private static bool s_isInSync;

    public static bool IsDisabled => ThreadAssert.s_isDisabled;

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Enable()
    {
      Assert.That<bool>(ThreadAssert.s_isSetup).IsFalse("Thread asserts are already enabled on this thread.");
      ThreadAssert.s_isSetup = true;
      ThreadAssert.s_isDisabled = false;
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void SetThreadType(bool isMain, bool isSim)
    {
      if (ThreadAssert.s_isDisabled)
        return;
      Assert.That<bool>(ThreadAssert.s_isSetup).IsTrue("Thread asserts are not setup on this thread.");
      ThreadAssert.s_isMainThread = isMain;
      ThreadAssert.s_isSimThread = isSim;
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void SetSyncState(bool state)
    {
      if (ThreadAssert.s_isDisabled)
        return;
      Assert.That<bool>(ThreadAssert.s_isSetup).IsTrue("Thread asserts are not setup on this thread.");
      ThreadAssert.s_isInSync = state;
    }

    public static void Disable() => ThreadAssert.s_isDisabled = true;

    public static void Reset()
    {
      ThreadAssert.s_isSetup = false;
      ThreadAssert.s_isDisabled = false;
      ThreadAssert.s_isMainThread = false;
      ThreadAssert.s_isSimThread = false;
    }

    public static void ResetIfNotDisabled()
    {
      if (ThreadAssert.s_isDisabled)
        return;
      ThreadAssert.Reset();
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsOnThread(ThreadType thread, string message = "")
    {
      if (ThreadAssert.s_isDisabled)
        return;
      bool flag;
      switch (thread)
      {
        case ThreadType.Any:
          return;
        case ThreadType.Main:
          flag = ThreadAssert.s_isMainThread;
          break;
        case ThreadType.Sim:
          flag = ThreadAssert.s_isSimThread;
          break;
        default:
          Assert.Fail(string.Format("Unknown thread: {0}", (object) thread));
          return;
      }
      Assert.That<bool>(ThreadAssert.s_isSetup).IsTrue<string, string>("Thread asserts are not setup on thread '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
      Assert.That<bool>(flag).IsTrue<ThreadType, string, string>("Called on wrong thread. Expected '{0}' thread, got '{1}'. {2}", thread, ThreadUtils.ThreadNameFast, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsOnThreadOrInSync(ThreadType thread, string message = "")
    {
      if (ThreadAssert.s_isDisabled || ThreadAssert.s_isInSync)
        return;
      bool flag;
      switch (thread)
      {
        case ThreadType.Any:
          return;
        case ThreadType.Main:
          flag = ThreadAssert.s_isMainThread;
          break;
        case ThreadType.Sim:
          flag = ThreadAssert.s_isSimThread;
          break;
        default:
          Assert.Fail(string.Format("Unknown thread: {0}", (object) thread));
          return;
      }
      Assert.That<bool>(ThreadAssert.s_isSetup).IsTrue<string, string>("Thread asserts are not setup on thread '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
      Assert.That<bool>(flag).IsTrue<ThreadType, string, string>("Called on wrong thread. Expected sync or '{0}' thread, got '{1}' and sync=false. {2}", thread, ThreadUtils.ThreadNameFast, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsOnMainThread(string message = "")
    {
      if (ThreadAssert.s_isDisabled)
        return;
      Assert.That<bool>(ThreadAssert.s_isSetup).IsTrue<string, string>("Thread asserts are not setup on thread '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
      Assert.That<bool>(ThreadAssert.s_isMainThread).IsTrue<string, string>("Called on wrong thread. Expected main thread, got '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsOnSimThread(string message = "")
    {
      if (ThreadAssert.s_isDisabled)
        return;
      Assert.That<bool>(ThreadAssert.s_isSetup).IsTrue<string, string>("Thread asserts are not setup on thread '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
      Assert.That<bool>(ThreadAssert.s_isSimThread | ThreadAssert.s_isInSync).IsTrue<string, string>("Called on wrong thread. Expected sim thread, got '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsOnMainOrSimThread(string message = "")
    {
      if (ThreadAssert.s_isDisabled)
        return;
      Assert.That<bool>(ThreadAssert.s_isSetup).IsTrue<string, string>("Thread asserts are not setup on thread '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
      Assert.That<bool>(ThreadAssert.s_isSimThread | ThreadAssert.s_isMainThread).IsTrue<string, string>("Called on wrong thread. Expected sim or main thread, got '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsInSync(string message = "")
    {
      if (ThreadAssert.s_isDisabled)
        return;
      Assert.That<bool>(ThreadAssert.s_isSetup).IsTrue<string, string>("Thread asserts are not setup on thread '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
      Assert.That<bool>(ThreadAssert.s_isInSync).IsTrue<string>("Called outside of sync. {0}", message);
    }

    /// <summary>
    /// Asserts that caller is on sim thread or on main thread during sync.
    /// </summary>
    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsOnSimThreadOrInSync(string message = "")
    {
      if (ThreadAssert.s_isDisabled || ThreadAssert.s_isInSync)
        return;
      Assert.That<bool>(ThreadAssert.s_isSetup).IsTrue<string, string>("Thread asserts are not setup on thread '{0}'. {1}", ThreadUtils.ThreadNameFast, message);
      Assert.That<bool>(ThreadAssert.s_isSimThread).IsTrue<string, string>("Called on wrong thread. Expected sim thread or in sync, got '{0}' thread and sync=false. {1}", ThreadUtils.ThreadNameFast, message);
    }
  }
}
