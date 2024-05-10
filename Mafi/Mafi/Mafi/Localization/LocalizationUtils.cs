// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.LocalizationUtils
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using System;

#nullable disable
namespace Mafi.Localization
{
  public static class LocalizationUtils
  {
    public static bool TryParsePoFileData(
      string poFileText,
      out string trInfoStr,
      out Dict<string, LocalizationManager.LocData> outData)
    {
      Dict<string, LocalizationManager.LocData> data = new Dict<string, LocalizationManager.LocData>();
      outData = data;
      Lyst<string> messages = new Lyst<string>();
      string key = (string) null;
      string str = (string) null;
      bool nextMsgIsFuzzy = false;
      string infoStr = "";
      foreach (string line in poFileText.SplitToLines())
      {
        if (key != null)
        {
          if (!line.StartsWith("msgid_plural"))
          {
            if (str != null)
            {
              if (line.IsEmpty())
              {
                messages.Add(str);
                saveAndClearMessages(key);
                key = (string) null;
                str = (string) null;
              }
              else if (line.StartsWith("msgstr["))
              {
                messages.Add(str);
                int length = "msgstr[x] \"".Length;
                str = replaceEscapedChars(line.Substring(length, line.Length - length - 1));
              }
              else if (line.StartsWith("\""))
                str += replaceEscapedChars(line.Substring(1, line.Length - 2));
              else
                Log.Error("Unexpected line: " + line);
            }
            else if (line.StartsWith("msgstr "))
            {
              int length = "msgstr \"".Length;
              str = replaceEscapedChars(line.Substring(length, line.Length - length - 1));
            }
            else if (line.StartsWith("msgstr[0]"))
            {
              int length = "msgstr[0] \"".Length;
              str = replaceEscapedChars(line.Substring(length, line.Length - length - 1));
            }
            else
              Log.Error("Unexpected line: " + line);
          }
        }
        else if (line.StartsWith("msgid "))
        {
          Assert.That<string>(key).IsNull<string>();
          Assert.That<string>(str).IsNull<string>();
          int length = "msgid \"".Length;
          key = line.Substring(length, line.Length - length - 1);
        }
        else if (line.StartsWith("#, fuzzy"))
          nextMsgIsFuzzy = true;
        else if (!line.IsEmpty() && !line.StartsWith("#"))
          Log.Error("Unexpected line: " + line);
      }
      if (key != null && str != null)
      {
        messages.Add(str);
        saveAndClearMessages(key);
      }
      trInfoStr = infoStr;
      return true;

      void saveAndClearMessages(string key)
      {
        if (key.IsEmpty())
        {
          Assert.That<Lyst<string>>(messages).HasLength<string>(1);
          infoStr = messages.FirstOrDefault<string>() ?? "";
        }
        else if (!nextMsgIsFuzzy)
          Assert.That<bool>(data.TryAdd(key, new LocalizationManager.LocData(messages.ToImmutableArray()))).IsTrue(key + " already exists!");
        messages.Clear();
        nextMsgIsFuzzy = false;
      }

      static string replaceEscapedChars(string str)
      {
        if (str.EndsWith("\\n"))
          str = str.Substring(0, str.Length - 2) + "\n";
        str = str.Replace("\\\"", "\"");
        str = str.Replace("\\\\", "\\");
        return str;
      }
    }

    public static bool TryRedactTranslationData(string text, out string result, out string error)
    {
      int num1 = text.IndexOf("Last-Translator:", StringComparison.Ordinal);
      if (num1 < 0)
      {
        result = text;
        error = "";
        return true;
      }
      int startIndex = num1 + "Last-Translator:".Length;
      int num2 = text.IndexOf("\\n", startIndex, StringComparison.Ordinal);
      if (num2 <= startIndex)
      {
        result = "";
        error = "Failed to remove translator name, token '\\n'@";
        return false;
      }
      int count = num2 - startIndex;
      if (count > 100)
      {
        result = "";
        error = string.Format("Failed to remove translator name, match too long ({0})", (object) count);
        return false;
      }
      result = text.Remove(startIndex, count);
      error = "";
      return true;
    }
  }
}
