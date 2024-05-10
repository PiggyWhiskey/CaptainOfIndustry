// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.MemoryLogStorage
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Logging;
using System;

#nullable disable
namespace Mafi.Utils
{
  /// <summary>
  /// High throughput, tread safe, non-blocking logging storage.
  /// </summary>
  internal class MemoryLogStorage : IDisposable
  {
    /// <summary>Subscription tracking to avoid over-subscription.</summary>
    private bool m_isSubscribed;
    private Lyst<LogEntry> m_logsStorage;

    public LogType AcceptedLogs { get; set; }

    public MemoryLogStorage()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_logsStorage = new Lyst<LogEntry>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AcceptedLogs = LogType.All;
    }

    public void Subscribe()
    {
      Assert.That<bool>(this.m_isSubscribed).IsFalse();
      if (this.m_isSubscribed)
        return;
      Log.LogReceived += new Action<LogEntry>(this.logReceived);
      this.m_isSubscribed = true;
    }

    public void Unsubscribe()
    {
      Assert.That<bool>(this.m_isSubscribed).IsTrue();
      if (!this.m_isSubscribed)
        return;
      Log.LogReceived -= new Action<LogEntry>(this.logReceived);
      this.m_isSubscribed = false;
    }

    public void Dispose() => this.Unsubscribe();

    /// <summary>
    /// Copies all stored logs to the given list in the order of their log time and clears the log storage.
    /// </summary>
    public Lyst<LogEntry> GetAndRemoveAllStoredLogs()
    {
      Lyst<LogEntry> logsStorage = this.m_logsStorage;
      lock (logsStorage)
        this.m_logsStorage = new Lyst<LogEntry>(16);
      return logsStorage;
    }

    private void logReceived(LogEntry log)
    {
      if ((log.Type & this.AcceptedLogs) == (LogType) 0)
        return;
      Lyst<LogEntry> logsStorage = this.m_logsStorage;
      lock (logsStorage)
        logsStorage.Add(log);
    }
  }
}
