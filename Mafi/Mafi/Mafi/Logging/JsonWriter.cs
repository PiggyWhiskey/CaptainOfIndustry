// Decompiled with JetBrains decompiler
// Type: Mafi.Logging.JsonWriter
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Mafi.Logging
{
  public class JsonWriter
  {
    private readonly StringBuilder m_jsonSb;
    private bool m_isFirstField;

    public JsonWriter(int capacity)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_jsonSb = new StringBuilder(capacity);
    }

    public void AppendStartObject()
    {
      this.m_jsonSb.Append("{\n");
      this.m_isFirstField = true;
    }

    public void AppendEndObject()
    {
      this.m_jsonSb.Append("\n}");
      this.m_isFirstField = false;
    }

    private void handleCommas()
    {
      if (this.m_isFirstField)
        this.m_isFirstField = false;
      else
        this.m_jsonSb.Append(",\n");
    }

    public void AppendStringField(string name, string value)
    {
      this.handleCommas();
      this.m_jsonSb.Append("\"");
      JsonWriter.JsonEscapeString(name, this.m_jsonSb);
      this.m_jsonSb.Append("\":\"");
      JsonWriter.JsonEscapeString(value, this.m_jsonSb);
      this.m_jsonSb.Append("\"");
    }

    public void AppendNumberField(string name, int value)
    {
      this.handleCommas();
      this.m_jsonSb.Append("\"");
      this.m_jsonSb.Append(name);
      this.m_jsonSb.Append("\":");
      this.m_jsonSb.Append(value);
    }

    public void AppendNumberField(string name, long value)
    {
      this.handleCommas();
      this.m_jsonSb.Append("\"");
      this.m_jsonSb.Append(name);
      this.m_jsonSb.Append("\":");
      this.m_jsonSb.Append(value);
    }

    public void AppendNumberField(string name, double value)
    {
      this.handleCommas();
      this.m_jsonSb.Append("\"");
      this.m_jsonSb.Append(name);
      this.m_jsonSb.Append("\":");
      this.m_jsonSb.Append(value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public string GetJsonAndClear()
    {
      string jsonAndClear = this.m_jsonSb.ToString();
      this.m_jsonSb.Clear();
      this.m_isFirstField = true;
      return jsonAndClear;
    }

    public static void JsonEscapeString(string str, StringBuilder sb)
    {
      if (str == null)
      {
        Log.Warning("Trying to escape a null string");
      }
      else
      {
        foreach (char ch in str)
        {
          switch (ch)
          {
            case '\b':
              sb.Append("\\b");
              continue;
            case '\t':
              sb.Append("\\t");
              continue;
            case '\n':
              sb.Append("\\n");
              continue;
            case '\f':
              sb.Append("\\f");
              continue;
            case '\r':
              continue;
            case '"':
              sb.Append("\\\"");
              continue;
            case '\\':
              sb.Append("\\\\");
              continue;
            default:
              if (ch < ' ')
              {
                sb.Append("\\u");
                sb.AppendFormat(((int) ch).ToString("X4"));
                continue;
              }
              sb.Append(ch);
              continue;
          }
        }
      }
    }
  }
}
