// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.ChangelogUtils
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Utils
{
  public static class ChangelogUtils
  {
    public const string CHANGELOG_NAME = "changelog.txt";
    internal static readonly char[] HOTFIX_SUFFIX_LETTERS;
    public static readonly IReadOnlyDictionary<string, string> PATCH_NOTES_TRANSLATIONS;

    public static string GetChangeLogForCurrentMinorVersion(
      IEnumerable<string> changelogContentOverride = null)
    {
      Lyst<string> strings = new Lyst<string>();
      int? nullable = new int?();
      try
      {
        foreach (KeyValuePair<string, string> keyValuePair in ChangelogUtils.ParseChangelog(changelogContentOverride))
        {
          int minorVersion;
          if (!GameVersion.TryParseMinorVersion(keyValuePair.Key, out minorVersion))
            Log.Warning("Failed to parse game version from changelog");
          else if (!nullable.HasValue)
            nullable = new int?(minorVersion);
          else if (nullable.Value != minorVersion)
            break;
          strings.Add(keyValuePair.Value);
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to load changelog.");
        return "";
      }
      return strings.JoinStrings("\r\n\r\n\r\n");
    }

    public static string GetCleanVersionStr(string str, bool removeV = false, bool removeSuffix = false)
    {
      str = str.Trim();
      int length = str.IndexOf(' ');
      str = length > 0 ? str.Substring(0, length).Trim() : str;
      if (removeV && str.Length > 0 && str[0] == 'v')
        str = str.Substring(1).Trim();
      if (removeSuffix)
        str = str.TrimEnd(ChangelogUtils.HOTFIX_SUFFIX_LETTERS);
      return str;
    }

    public static IEnumerable<ChangelogEntry> ParseChangelogEntries()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<ChangelogEntry>) new ChangelogUtils.\u003CParseChangelogEntries\u003Ed__5(-2);
    }

    public static IEnumerable<KeyValuePair<string, string>> ParseChangelog(
      IEnumerable<string> changelogContentOverride = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<KeyValuePair<string, string>>) new ChangelogUtils.\u003CParseChangelog\u003Ed__6(-2)
      {
        \u003C\u003E3__changelogContentOverride = changelogContentOverride
      };
    }

    public static string GetChangeLog(
      string onlyAfterVersion,
      bool reversePatchesOrder,
      IEnumerable<string> changelogContentOverride = null)
    {
      onlyAfterVersion = onlyAfterVersion.Trim();
      bool flag = false;
      Lyst<string> strings = new Lyst<string>();
      try
      {
        foreach (KeyValuePair<string, string> keyValuePair in ChangelogUtils.ParseChangelog(changelogContentOverride))
        {
          if (keyValuePair.Key == onlyAfterVersion)
          {
            flag = true;
            break;
          }
          strings.Add(keyValuePair.Value);
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to load changelog.");
        return "";
      }
      if (!flag)
        return "";
      if (reversePatchesOrder)
        strings.Reverse();
      return strings.JoinStrings("\r\n\r\n\r\n");
    }

    public static string GetLatestPatchChangeLog(
      bool untilMajorVersion,
      bool reversePatchesOrder,
      IEnumerable<string> changelogContentOverride = null)
    {
      string str = (string) null;
      Lyst<string> strings = new Lyst<string>();
      try
      {
        foreach (KeyValuePair<string, string> keyValuePair in ChangelogUtils.ParseChangelog(changelogContentOverride))
        {
          if (!untilMajorVersion)
            return keyValuePair.Value;
          if (str == null)
            str = keyValuePair.Key.TrimEnd(ChangelogUtils.HOTFIX_SUFFIX_LETTERS);
          else if (keyValuePair.Key.TrimEnd(ChangelogUtils.HOTFIX_SUFFIX_LETTERS) != str)
            break;
          strings.Add(keyValuePair.Value);
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to load changelog.");
        return "";
      }
      if (reversePatchesOrder)
        strings.Reverse();
      return strings.JoinStrings("\r\n\r\n\r\n");
    }

    public static IEnumerable<string> EnumerateAllVersions(
      IEnumerable<string> changelogContentOverride = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<string>) new ChangelogUtils.\u003CEnumerateAllVersions\u003Ed__9(-2)
      {
        \u003C\u003E3__changelogContentOverride = changelogContentOverride
      };
    }

    static ChangelogUtils()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ChangelogUtils.HOTFIX_SUFFIX_LETTERS = Enumerable.Range(0, 26).Select<int, char>((Func<int, char>) (x => (char) (97 + x))).ToArray<char>();
      ChangelogUtils.PATCH_NOTES_TRANSLATIONS = (IReadOnlyDictionary<string, string>) new Dict<string, string>()
      {
        {
          "",
          ""
        }
      };
    }
  }
}
