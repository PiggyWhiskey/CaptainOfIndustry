// Decompiled with JetBrains decompiler
// Type: Mafi.Tracing
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Docs: https://docs.google.com/document/d/1CvAClvFfyA5R-PhYUmn5OOQtYMH4h6I0nSsKchNAySU/preview
  /// </summary>
  public static class Tracing
  {
    public const string TRACING_CONDITIONAL = "MAFI_TRACING";
    public const char BEGIN_PHASE_NAME = 'B';
    public const char END_PHASE_NAME = 'E';
    public const char INSTANT_PHASE_NAME = 'i';
    public const bool IS_AVAILABLE = false;
    private static readonly StringBuilder s_stringBuilder;
    private static readonly Lyst<Tracing.TraceRecord> s_records;
    private static readonly Stopwatch s_stopwatch;

    public static bool IsEnabled { get; private set; }

    public static int RecordedEventsCount => Tracing.s_records.Count;

    /// <summary>
    /// Starts tracing. When tracing is already running, this restarts it.
    /// </summary>
    [Conditional("MAFI_TRACING")]
    public static void StartTracing()
    {
      lock (Tracing.s_records)
      {
        Tracing.s_records.Clear();
        Tracing.s_stopwatch.Restart();
        Tracing.IsEnabled = true;
      }
    }

    /// <summary>Stops tracing.</summary>
    [Conditional("MAFI_TRACING")]
    public static void StopTracing()
    {
      lock (Tracing.s_records)
        Tracing.IsEnabled = false;
    }

    /// <summary>
    /// Clears internal buffer but does not change state of tracing.
    /// </summary>
    [Conditional("MAFI_TRACING")]
    public static void ClearRecords()
    {
      lock (Tracing.s_records)
      {
        Tracing.s_records.Clear();
        Tracing.s_stopwatch.Restart();
      }
    }

    /// <summary>Records instant event.</summary>
    [Conditional("MAFI_TRACING")]
    public static void Instant(string name, string category)
    {
      if (!Tracing.IsEnabled)
        return;
      Tracing.saveEvent('i', name, category);
    }

    /// <summary>
    /// Records event begin (should be paired with <see cref="M:Mafi.Tracing.End(System.String,System.String)" />).
    /// </summary>
    [Conditional("MAFI_TRACING")]
    public static void Begin(string name, string category)
    {
      if (!Tracing.IsEnabled)
        return;
      Tracing.saveEvent('B', name, category);
    }

    [Conditional("MAFI_TRACING")]
    public static void Begin<T0>(string name, string category, T0 arg0)
    {
      if (!Tracing.IsEnabled)
        return;
      Tracing.saveEvent('B', name.FormatInvariant((object) arg0), category);
    }

    /// <summary>
    /// Records event end (should be paired with <see cref="M:Mafi.Tracing.Begin(System.String,System.String)" />).
    /// </summary>
    [Conditional("MAFI_TRACING")]
    public static void End(string name, string category)
    {
      if (!Tracing.IsEnabled)
        return;
      Tracing.saveEvent('E', name, category);
    }

    [Conditional("MAFI_TRACING")]
    public static void End<T0>(string name, string category, T0 arg0)
    {
      if (!Tracing.IsEnabled)
        return;
      Tracing.saveEvent('E', name.FormatInvariant((object) arg0), category);
    }

    /// <summary>Saves event to internal list in a thread-safe way.</summary>
    private static void saveEvent(char phase, string name, string category = "", string extraStr = "")
    {
      Tracing.TraceRecord traceRecord = new Tracing.TraceRecord(phase, name, category, ThreadUtils.ThreadIdAndNameNameFast, (long) (Tracing.s_stopwatch.Elapsed.TotalMilliseconds * 1000.0), extraStr);
      lock (Tracing.s_records)
      {
        if (!Tracing.IsEnabled)
          return;
        Tracing.s_records.Add(traceRecord);
      }
    }

    public static Lyst<Tracing.EventRecord> ComputeEventDurations(
      string categoryName = null,
      string threadName = null)
    {
      Lyst<KeyValuePair<string, Stak<Tracing.EventRecord>>> list = new Lyst<KeyValuePair<string, Stak<Tracing.EventRecord>>>();
      Lyst<Tracing.EventRecord> eventDurations = new Lyst<Tracing.EventRecord>();
      lock (Tracing.s_records)
      {
        foreach (Tracing.TraceRecord record in Tracing.s_records)
        {
          if ((categoryName == null || !(record.Category != categoryName)) && (threadName == null || !(record.ThreadName != threadName)) && (record.Phase == 'B' || record.Phase == 'E'))
          {
            Stak<Tracing.EventRecord> stak;
            if (!list.TryGetValue<string, Stak<Tracing.EventRecord>>(record.ThreadName, out stak))
            {
              stak = new Stak<Tracing.EventRecord>();
              list.Add<string, Stak<Tracing.EventRecord>>(record.ThreadName, stak);
            }
            if (record.Phase == 'B')
              stak.Push(new Tracing.EventRecord(record));
            else if (!stak.IsEmpty)
            {
              Tracing.EventRecord eventRecord = stak.Pop();
              if (record.Name != eventRecord.StartRecord.Name)
              {
                Log.Error("End of unexpected event '" + record.Name + "' on thread '" + record.ThreadName + "', expected '" + eventRecord.StartRecord.Name + "'.");
                break;
              }
              eventRecord.TotalDuration = record.Microseconds - eventRecord.StartRecord.Microseconds;
              if (stak.IsNotEmpty)
                stak.Peek().ChildrenDuration += eventRecord.TotalDuration;
              eventDurations.Add(eventRecord);
            }
          }
        }
      }
      return eventDurations;
    }

    public static void WriteTraceDataTo(string filePath)
    {
      using (StreamWriter sw = new StreamWriter(filePath))
        Tracing.WriteTraceDataTo(sw);
    }

    public static void WriteTraceDataTo(StreamWriter sw)
    {
      Assert.That<bool>(Monitor.IsEntered((object) Tracing.s_records)).IsFalse();
      sw.Write("[\n");
      Tracing.s_stringBuilder.Length = 0;
      foreach (Tracing.TraceRecord record in Tracing.s_records)
        Tracing.writeRecord(record, sw);
      Tracing.s_stringBuilder.Append("{\"name\":\"TracingEnded\",\"ph\":\"i\",\"ts\":\"");
      Tracing.s_stringBuilder.Append((Tracing.s_stopwatch.Elapsed.TotalMilliseconds * 1000.0).RoundToInt());
      Tracing.s_stringBuilder.Append("\",\"pid\":\"0\",\"tid\":\"");
      Tracing.s_stringBuilder.Append(ThreadUtils.ThreadIdAndNameNameFast);
      Tracing.s_stringBuilder.Append("\"}\n]");
      sw.Write(Tracing.s_stringBuilder.ToString());
    }

    private static void writeRecord(Tracing.TraceRecord record, StreamWriter sw)
    {
      Tracing.s_stringBuilder.Append("{\"name\":\"");
      Tracing.s_stringBuilder.Append(record.Name);
      Tracing.s_stringBuilder.Append("\",\"cat\":\"");
      Tracing.s_stringBuilder.Append(record.Category);
      Tracing.s_stringBuilder.Append("\",\"ph\":\"");
      Tracing.s_stringBuilder.Append(record.Phase);
      Tracing.s_stringBuilder.Append("\",\"ts\":\"");
      Tracing.s_stringBuilder.Append(record.Microseconds);
      Tracing.s_stringBuilder.Append("\",\"pid\":\"0\",\"tid\":\"");
      Tracing.s_stringBuilder.Append(record.ThreadName);
      Tracing.s_stringBuilder.Append("\"");
      Tracing.s_stringBuilder.Append(record.ExtraStr);
      Tracing.s_stringBuilder.Append("},\n");
    }

    static Tracing()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Tracing.s_stringBuilder = new StringBuilder();
      Tracing.s_records = new Lyst<Tracing.TraceRecord>(true);
      Tracing.s_stopwatch = new Stopwatch();
    }

    public sealed class EventRecord
    {
      public readonly Tracing.TraceRecord StartRecord;
      public long TotalDuration;
      public long ChildrenDuration;

      public EventRecord(Tracing.TraceRecord startRecord)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.StartRecord = startRecord;
      }
    }

    public readonly struct TraceRecord
    {
      public readonly char Phase;
      public readonly string Name;
      public readonly string Category;
      public readonly string ThreadName;
      public readonly long Microseconds;
      public readonly string ExtraStr;

      public TraceRecord(
        char phase,
        string name,
        string category,
        string threadName,
        long microseconds,
        string extraStr)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.Phase = phase;
        this.Name = name;
        this.Category = category;
        this.ThreadName = threadName;
        this.Microseconds = microseconds;
        this.ExtraStr = extraStr;
      }
    }
  }
}
