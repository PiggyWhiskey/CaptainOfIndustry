// Decompiled with JetBrains decompiler
// Type: Mafi.Logging.Graylog.GraylogLogger
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Mafi.Logging.Graylog
{
  public sealed class GraylogLogger
  {
    public const int MAX_MESSAGES_PER_BATCH = 10;
    private readonly IErrorLoggerConfig m_config;
    private IGelfClient m_gelfClient;
    private BlockingCollection<LogEntry> m_entries;
    private Thread m_sendingThread;
    private long m_lastBatchStart;
    private int m_logsReceivedThisBatch;
    private int m_messagesReceived;
    private int m_messagesSent;
    private long m_gameId;
    private long m_sessionId;
    private string m_startedAt;
    private string m_versionStart;
    private bool m_loggingStarted;
    private bool m_tooManyMessages;
    private long m_timeIntervalBetweenBatchesTicks;
    private readonly string m_operatingSystem;
    private string m_mapName;
    private bool m_isEditor;

    public int MessagesReceived => this.m_messagesReceived;

    public int MessagesSent => this.m_messagesSent;

    public bool IsLoggingStarted => this.m_loggingStarted;

    public GraylogLogger(IErrorLoggerConfig config, string operatingSystem)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_lastBatchStart = DateTime.UtcNow.Ticks;
      this.m_startedAt = "";
      this.m_versionStart = "";
      this.m_timeIntervalBetweenBatchesTicks = 100000000L;
      this.m_mapName = "";
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_config = config;
      this.m_operatingSystem = operatingSystem ?? "";
    }

    public void Start(IGelfClient customClient = null)
    {
      if (this.m_loggingStarted)
      {
        Log.Warning("Logging already started.");
      }
      else
      {
        if (this.m_config.DisableAnonymousErrorLogs)
          return;
        this.m_gelfClient = customClient ?? (IGelfClient) new UdpGelfClient("3.130.193.195");
        if (!this.m_gelfClient.IsOperational)
          return;
        this.m_entries = new BlockingCollection<LogEntry>();
        this.m_sendingThread = new Thread(new ThreadStart(this.senderThreadMain))
        {
          Name = "GraylogSender"
        };
        this.m_sendingThread.Start();
        Log.LogReceived += new Action<LogEntry>(this.SendLog);
        this.m_loggingStarted = true;
      }
    }

    public void StopAndDispose()
    {
      if (this.m_loggingStarted)
      {
        Log.LogReceived -= new Action<LogEntry>(this.SendLog);
        this.m_loggingStarted = false;
      }
      if (this.m_entries != null)
      {
        this.m_entries.CompleteAdding();
        this.m_sendingThread.Join(1000);
        if (this.m_sendingThread.IsAlive)
        {
          Log.Error("Logger thread failed to terminate in 1000 ms, aborting.");
          this.m_sendingThread.Abort();
        }
        this.m_entries.Dispose();
        this.m_entries = (BlockingCollection<LogEntry>) null;
      }
      this.m_gelfClient?.Dispose();
      this.m_gelfClient = (IGelfClient) null;
    }

    public void ForceSendLog(LogEntry log)
    {
      if (this.m_gelfClient == null)
        return;
      Interlocked.Increment(ref this.m_messagesReceived);
      this.m_entries.Add(log);
    }

    public void SendLog(LogEntry entry)
    {
      if ((entry.Type & (LogType.Exception | LogType.Error | LogType.Assert | LogType.Warning | LogType.GameProgress)) == (LogType) 0)
        return;
      if (this.m_config.DisableAnonymousErrorLogs)
        this.StopAndDispose();
      else if (this.m_gelfClient == null)
      {
        Log.LogReceived -= new Action<LogEntry>(this.SendLog);
        Log.Error("No GELF client but log received.");
      }
      else
      {
        Interlocked.Increment(ref this.m_messagesReceived);
        if (this.m_entries.Count >= 10)
          return;
        if (Interlocked.Increment(ref this.m_logsReceivedThisBatch) > 10)
        {
          long ticks = DateTime.UtcNow.Ticks;
          if (DateTime.UtcNow.Ticks - this.m_lastBatchStart < this.m_timeIntervalBetweenBatchesTicks)
          {
            this.m_tooManyMessages = true;
            return;
          }
          this.m_lastBatchStart = ticks;
          this.m_logsReceivedThisBatch = 0;
          if (this.m_tooManyMessages)
          {
            this.m_tooManyMessages = false;
            this.m_timeIntervalBetweenBatchesTicks += 100000000L;
          }
        }
        this.m_entries.Add(entry);
      }
    }

    private void senderThreadMain()
    {
      while (true)
      {
        try
        {
          LogEntry entry;
          try
          {
            entry = this.m_entries.Take();
          }
          catch (InvalidOperationException ex)
          {
            break;
          }
          GelfMessage message = this.createMessage(entry);
          if (this.m_gelfClient.TrySendMessage(message))
            Interlocked.Increment(ref this.m_messagesSent);
          message.Dispose();
        }
        catch (ThreadAbortException ex)
        {
          break;
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception on the '" + ThreadUtils.ThreadNameFast + "' thread! ");
        }
      }
    }

    private GelfMessage createMessage(LogEntry entry)
    {
      SyslogSeverity syslogSeverity;
      switch (entry.Type)
      {
        case LogType.Exception:
          syslogSeverity = SyslogSeverity.Alert;
          break;
        case LogType.Error:
          syslogSeverity = SyslogSeverity.Critical;
          break;
        case LogType.Assert:
          syslogSeverity = SyslogSeverity.Error;
          break;
        case LogType.Warning:
          syslogSeverity = SyslogSeverity.Warning;
          break;
        case LogType.Info:
          syslogSeverity = SyslogSeverity.Notice;
          break;
        case LogType.GameProgress:
          syslogSeverity = SyslogSeverity.Informational;
          break;
        case LogType.Debug:
          syslogSeverity = SyslogSeverity.Debug;
          break;
        default:
          syslogSeverity = SyslogSeverity.Critical;
          break;
      }
      SyslogSeverity level = syslogSeverity;
      double num1 = Math.Round((double) new DateTimeOffset(entry.TimestampUtc).ToUnixTimeMilliseconds() / 1000.0, 3);
      string shortMessage = !entry.Exception.HasValue ? entry.Message : (!(entry.Exception.Value is NullReferenceException referenceException) ? entry.Message + " " + entry.Exception.Value.Message : "NullReferenceException " + entry.Message + " " + referenceException.StackTrace.FirstLine());
      KeyValuePair<string, string>[] valueOrNull1 = entry.AdditionalStringData.ValueOrNull;
      int length1 = valueOrNull1 != null ? valueOrNull1.Length : 0;
      PooledArray<KeyValuePair<string, string>> pooled1 = PooledArray<KeyValuePair<string, string>>.GetPooled(6 + length1);
      pooled1[0] = Make.Kvp<string, string>("thread", entry.ThreadName);
      pooled1[1] = Make.Kvp<string, string>("version", "0.6.3a");
      pooled1[2] = Make.Kvp<string, string>("game_start", this.m_startedAt);
      pooled1[3] = Make.Kvp<string, string>("version_start", this.m_versionStart);
      pooled1[4] = Make.Kvp<string, string>("os", this.m_operatingSystem);
      pooled1[5] = Make.Kvp<string, string>("map", this.m_mapName);
      if (valueOrNull1 != null)
      {
        for (int index = 0; index < length1; ++index)
          pooled1[6 + index] = valueOrNull1[index];
      }
      KeyValuePair<string, long>[] valueOrNull2 = entry.AdditionalIntData.ValueOrNull;
      int length2 = valueOrNull2 != null ? valueOrNull2.Length : 0;
      int num2 = 6;
      if (this.m_isEditor)
        ++num2;
      PooledArray<KeyValuePair<string, long>> pooled2 = PooledArray<KeyValuePair<string, long>>.GetPooled(num2 + length2);
      pooled2[0] = Make.Kvp<string, long>("build", 333L);
      pooled2[1] = Make.Kvp<string, long>("save_version", 168L);
      pooled2[2] = Make.Kvp<string, long>("sim_step", (long) entry.SimStep ?? -1L);
      pooled2[3] = Make.Kvp<string, long>("game_id", this.m_gameId & (long) int.MaxValue);
      pooled2[4] = Make.Kvp<string, long>("session_id", this.m_sessionId & (long) int.MaxValue);
      if (this.m_isEditor)
        pooled2[5] = Make.Kvp<string, long>("is_editor", 1L);
      if (valueOrNull2 != null)
      {
        for (int index = 0; index < length2; ++index)
          pooled2[num2 + index] = valueOrNull2[index];
      }
      return new GelfMessage(shortMessage, level, entry.ToString(entry.Type <= LogType.Error ? 10 : 6, true, true), new double?(num1), new PooledArray<KeyValuePair<string, string>>?(pooled1), new PooledArray<KeyValuePair<string, long>>?(pooled2));
    }

    public void NotifyNewGameStarted(DependencyResolver resolver)
    {
      IGameIdProvider dep1;
      if (resolver.TryResolve<IGameIdProvider>(out dep1))
      {
        this.m_gameId = dep1.GameId;
        this.m_sessionId = dep1.SessionId;
        this.m_startedAt = dep1.GameStartedAtUtc.ToString("yy-MM-dd HH:mm");
        this.m_versionStart = dep1.GameStartedAtVersion;
      }
      IMapInfoProvider dep2;
      if (resolver.TryResolve<IMapInfoProvider>(out dep2))
        this.m_mapName = string.Format("{0} v{1}", (object) dep2.Name, (object) dep2.MapVersion);
      IMapStartInfoProvider dep3;
      if (resolver.TryResolve<IMapStartInfoProvider>(out dep3))
        this.m_mapName += string.Format(" s{0}", (object) dep3.StartingLocationIndex);
      this.m_isEditor = resolver.TryResolve<IMapEditorInfo>(out IMapEditorInfo _);
    }

    public void NotifyGameTerminated()
    {
      this.m_gameId = 0L;
      this.m_sessionId = 0L;
      this.m_startedAt = (string) null;
      this.m_versionStart = "";
      this.m_mapName = "";
    }
  }
}
