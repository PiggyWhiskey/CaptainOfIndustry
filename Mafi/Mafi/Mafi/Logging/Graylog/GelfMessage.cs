// Decompiled with JetBrains decompiler
// Type: Mafi.Logging.Graylog.GelfMessage
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Logging.Graylog
{
  public struct GelfMessage
  {
    /// <summary>GELF spec version.</summary>
    public const string VERSION = "1.1";
    /// <summary>A short descriptive message.</summary>
    public readonly string ShortMessage;
    /// <summary>
    /// Long message that can i.e. contain a backtrace; optional.
    /// </summary>
    public readonly Option<string> FullMessage;
    /// <summary>
    /// Seconds since UNIX epoch with optional decimal places for milliseconds; SHOULD be set by client library.
    /// Will be set to the current timestamp (now) by the server if absent.
    /// </summary>
    public readonly double? Timestamp;
    /// <summary>
    /// The level equal to the standard syslog levels; optional, default is 1 (ALERT).
    /// </summary>
    public readonly SyslogSeverity Level;
    private PooledArray<KeyValuePair<string, string>> m_additionalStringFields;
    private PooledArray<KeyValuePair<string, long>> m_additionalIntFields;

    public GelfMessage(
      string shortMessage,
      SyslogSeverity level,
      string fullMessage = null,
      double? timestamp = null,
      PooledArray<KeyValuePair<string, string>>? additionalStringFields = null,
      PooledArray<KeyValuePair<string, long>>? additionalIntFields = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.ShortMessage = shortMessage;
      this.Level = level;
      this.FullMessage = (Option<string>) fullMessage;
      this.Timestamp = timestamp;
      this.m_additionalStringFields = additionalStringFields ?? PooledArray<KeyValuePair<string, string>>.Empty;
      this.m_additionalIntFields = additionalIntFields ?? PooledArray<KeyValuePair<string, long>>.Empty;
    }

    public void Dispose()
    {
      this.m_additionalStringFields.ReturnToPool();
      this.m_additionalStringFields = PooledArray<KeyValuePair<string, string>>.Empty;
      this.m_additionalIntFields.ReturnToPool();
      this.m_additionalIntFields = PooledArray<KeyValuePair<string, long>>.Empty;
    }

    public readonly void ToJson(JsonWriter writer)
    {
      writer.AppendStartObject();
      writer.AppendStringField("version", "1.1");
      writer.AppendStringField("short_message", this.ShortMessage);
      if (this.FullMessage.HasValue)
        writer.AppendStringField("full_message", this.FullMessage.Value);
      if (this.Timestamp.HasValue)
        writer.AppendNumberField("timestamp", this.Timestamp.Value);
      writer.AppendNumberField("level", (int) this.Level);
      foreach (KeyValuePair<string, string> backing in this.m_additionalStringFields.BackingArray)
      {
        if (backing.Key == null)
          Log.Warning("Trying to gray-log null string.");
        else if (backing.Value == null)
          Log.Warning("Trying to gray-log null value '" + backing.Key + "'.");
        else
          writer.AppendStringField("_" + backing.Key, backing.Value);
      }
      foreach (KeyValuePair<string, long> backing in this.m_additionalIntFields.BackingArray)
      {
        if (backing.Key == null)
          Log.Warning("Trying to gray-log null string (int value).");
        else
          writer.AppendNumberField("_" + backing.Key, backing.Value);
      }
      writer.AppendEndObject();
    }
  }
}
