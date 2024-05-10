// Decompiled with JetBrains decompiler
// Type: Mafi.PerfCounter
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Helper class to count number of calls to interesting methods such as caching to help evaluate effectivity.
  /// 
  /// Intended usage is:
  /// <code>
  /// PerfCounter.Inc("ClassName: YourKey OptinalyWithMore Parts");
  /// </code>
  /// Please try to not use any string concatenation or formatting for the key to make the operation allocation-free.
  /// For generic classes that want to report their type please use static readonly strings as keys.
  /// 
  /// All public methods are conditionally compiled and are not gonna be called in the release build to avoid overhead.
  /// </summary>
  public static class PerfCounter
  {
    public const bool PERF_COUNTERS_ENABLED = false;
    private static readonly ConcurrentDictionary<string, PerfCounter.Counter> s_perfCounters;

    /// <summary>
    /// Increments given counter. This method can be considered thread safe (for detail see remarks section of this
    /// class).
    /// </summary>
    [Conditional("MAFI_PERF_COUNTERS")]
    public static void Inc(string name)
    {
      PerfCounter.Counter counter;
      if (PerfCounter.s_perfCounters.TryGetValue(name, out counter))
        counter.Inc();
      else
        PerfCounter.s_perfCounters.TryAdd(name, new PerfCounter.Counter(1L));
    }

    /// <summary>
    /// Adds given number to the counter. This method can be considered thread safe (for detail see remarks section
    /// of this class).
    /// </summary>
    [Conditional("MAFI_PERF_COUNTERS")]
    public static void Add(string name, long count)
    {
      PerfCounter.Counter counter;
      if (PerfCounter.s_perfCounters.TryGetValue(name, out counter))
        counter.Add(count);
      else
        PerfCounter.s_perfCounters.TryAdd(name, new PerfCounter.Counter(count));
    }

    /// <summary>Returns count of requested counter.</summary>
    public static long GetCounter(string name)
    {
      PerfCounter.Counter counter;
      return !PerfCounter.s_perfCounters.TryGetValue(name, out counter) ? 0L : counter.Count;
    }

    /// <summary>Returns a copy of internal counters.</summary>
    public static Dict<string, long> GetAllCounters()
    {
      lock (PerfCounter.s_perfCounters)
        return PerfCounter.s_perfCounters.ToDict<KeyValuePair<string, PerfCounter.Counter>, string, long>((Func<KeyValuePair<string, PerfCounter.Counter>, string>) (kvp => kvp.Key), (Func<KeyValuePair<string, PerfCounter.Counter>, long>) (kvp => kvp.Value.Count));
    }

    static PerfCounter()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      PerfCounter.s_perfCounters = new ConcurrentDictionary<string, PerfCounter.Counter>();
    }

    /// <summary>
    /// Helper class to use interlocked increment inside of Dictionary.
    /// </summary>
    private class Counter
    {
      private long m_count;

      public long Count => this.m_count;

      public Counter(long initialValue)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_count = initialValue;
      }

      public void Inc() => Interlocked.Increment(ref this.m_count);

      public void Add(long count) => Interlocked.Add(ref this.m_count, count);
    }
  }
}
