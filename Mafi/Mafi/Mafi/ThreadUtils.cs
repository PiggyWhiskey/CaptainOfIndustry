// Decompiled with JetBrains decompiler
// Type: Mafi.ThreadUtils
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Threading;

#nullable disable
namespace Mafi
{
  public static class ThreadUtils
  {
    [ThreadStatic]
    private static string s_threadName;
    [ThreadStatic]
    private static string s_threadIdAndName;

    /// <summary>
    /// Returns current thread's name without allocations. The <see cref="P:System.Threading.Thread.Name" /> GC-allocates.
    /// </summary>
    public static string ThreadNameFast
    {
      get
      {
        if (ThreadUtils.s_threadName != null)
          return ThreadUtils.s_threadName;
        ThreadUtils.s_threadName = Thread.CurrentThread.Name ?? "Unknown thread";
        return ThreadUtils.s_threadName;
      }
    }

    public static string ThreadIdAndNameNameFast
    {
      get
      {
        if (ThreadUtils.s_threadIdAndName != null)
          return ThreadUtils.s_threadIdAndName;
        Thread currentThread = Thread.CurrentThread;
        string name = currentThread.Name;
        ThreadUtils.s_threadIdAndName = currentThread.ManagedThreadId.ToString();
        if (name != null)
          ThreadUtils.s_threadIdAndName = ThreadUtils.s_threadIdAndName + " " + name;
        return ThreadUtils.s_threadIdAndName;
      }
    }
  }
}
