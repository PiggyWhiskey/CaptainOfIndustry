// Decompiled with JetBrains decompiler
// Type: Mafi.Logging.LogEntry
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Mafi.Logging
{
  public readonly struct LogEntry
  {
    [ThreadStatic]
    private static StringBuilder s_sbTmp;
    public readonly DateTime TimestampUtc;
    public readonly LogType Type;
    public readonly string Message;
    public readonly string ThreadName;
    public readonly int? SimStep;
    public readonly Option<string> StackTrace;
    public readonly Option<System.Exception> Exception;
    public readonly Option<KeyValuePair<string, long>[]> AdditionalIntData;
    public readonly Option<KeyValuePair<string, string>[]> AdditionalStringData;

    public LogEntry(
      LogType type,
      string message,
      string threadName = null,
      int? simStep = null,
      string stackTrace = null,
      System.Exception exception = null,
      KeyValuePair<string, long>[] additionalIntData = null,
      KeyValuePair<string, string>[] additionalStringData = null,
      DateTime? customTimestampUtc = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.TimestampUtc = customTimestampUtc ?? DateTime.UtcNow;
      this.Type = type;
      this.Message = message ?? "";
      this.ThreadName = threadName ?? "";
      this.SimStep = simStep;
      this.StackTrace = string.IsNullOrWhiteSpace(stackTrace) ? Option<string>.None : (Option<string>) stackTrace;
      this.Exception = (Option<System.Exception>) exception;
      this.AdditionalIntData = (Option<KeyValuePair<string, long>[]>) additionalIntData;
      this.AdditionalStringData = (Option<KeyValuePair<string, string>[]>) additionalStringData;
    }

    /// <summary>Serializes this log to given string builder.</summary>
    public void ToString(
      StringBuilder sb,
      int stackTraceMaxLines = -1,
      bool omitHeader = false,
      bool omitMessage = false)
    {
      if (!omitHeader)
      {
        sb.Append(LogEntry.logTypeToChar(this.Type));
        sb.Append(" ");
        sb.Append(this.TimestampUtc.ToString("HH:mm:ss,fff"));
        sb.Append(" S");
        sb.Append(this.SimStep.HasValue ? this.SimStep.Value.ToString("000000") : "------");
        sb.Append(" ~");
        string str = string.IsNullOrEmpty(this.ThreadName) ? "---" : (this.ThreadName.Length >= 3 ? this.ThreadName.Substring(0, 3) : this.ThreadName + new string(' ', 3 - this.ThreadName.Length));
        sb.Append(str);
      }
      if (this.Message.IsNotEmpty() && !omitMessage)
      {
        if (!omitHeader)
          sb.Append(": ");
        sb.Append(this.Message);
      }
      if (this.Exception.HasValue)
      {
        sb.Append("\n ---> ");
        System.Exception exception = this.Exception.Value;
        sb.Append(exception.GetType().Name);
        sb.Append(": ");
        sb.Append(exception.Message);
        if (stackTraceMaxLines != 0 && !string.IsNullOrEmpty(exception.StackTrace))
          appendStackTrace(exception.StackTrace);
        for (System.Exception innerException = exception.InnerException; innerException != null; innerException = innerException.InnerException)
        {
          sb.Append("\n ---> ");
          sb.Append(innerException.GetType().Name);
          sb.Append(": ");
          sb.Append(innerException.Message);
          if (stackTraceMaxLines != 0 && !string.IsNullOrEmpty(innerException.StackTrace))
            appendStackTrace(innerException.StackTrace);
        }
      }
      else
      {
        if (!this.StackTrace.HasValue || stackTraceMaxLines == 0)
          return;
        appendStackTrace(this.StackTrace.Value);
      }

      void appendStackTrace(string stackTrace)
      {
        sb.Append("\n");
        sb.Append(Log.CleanStackTrace(stackTraceMaxLines < 0 ? stackTrace : stackTrace.FirstLines(stackTraceMaxLines)));
      }
    }

    /// <summary>
    /// Serializes this log to a string. Serialization does not append any new lines, however, new lines may be in
    /// the message itself.
    /// </summary>
    public override string ToString() => this.ToString(3);

    public string ToString(int stackTraceMaxLines, bool omitHeader = false, bool omitMessage = false)
    {
      if (LogEntry.s_sbTmp == null)
        LogEntry.s_sbTmp = new StringBuilder();
      LogEntry.s_sbTmp.Clear();
      this.ToString(LogEntry.s_sbTmp, stackTraceMaxLines, omitHeader, omitMessage);
      return LogEntry.s_sbTmp.ToString();
    }

    private static char logTypeToChar(LogType type)
    {
      switch (type)
      {
        case LogType.Exception:
          return 'X';
        case LogType.Error:
          return 'E';
        case LogType.Assert:
          return 'A';
        case LogType.Warning:
          return 'W';
        case LogType.Info:
          return 'I';
        case LogType.GameProgress:
          return 'G';
        case LogType.Debug:
          return 'D';
        default:
          return 'U';
      }
    }
  }
}
