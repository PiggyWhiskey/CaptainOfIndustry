// Decompiled with JetBrains decompiler
// Type: Mafi.GameVersion
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi
{
  public static class GameVersion
  {
    /// <summary>Name of the current (minor) game version.</summary>
    /// <remarks>Update <see cref="M:Mafi.GameVersion.GetMinorVersionName(System.String)" /> when changing this.</remarks>
    public const string NAME = "Update 2";
    /// <summary>Major version. Guaranteed to be an int.</summary>
    public const string MAJOR_VERSION = "0";
    /// <summary>Represents a large update. Guaranteed to be an int.</summary>
    /// <remarks>Update <see cref="M:Mafi.GameVersion.GetMinorVersionName(System.String)" /> when changing this.</remarks>
    public const string MINOR_VERSION = "6";
    /// <summary>
    /// Represents a small update, something that adds minor features and fixes bugs. Guaranteed to be an int.
    /// </summary>
    public const string REVISION_VERSION = "3";
    /// <summary>
    /// Hotfix name. Guaranteed to be a char or empty string.
    /// Use hotfix when patch is just fixing bugs, no new features.
    /// </summary>
    public const string HOTFIX_NAME = "a";
    public const string FULL_VERSION = "0.6.3a";
    public const string UNKNOWN_VERSION_NAME = "unknown version";
    public static readonly LocStrFormatted FULL_DISPLAY_VALUE;

    public static bool TryParseMinorVersion(string versionStr, out int minorVersion)
    {
      minorVersion = 0;
      int num1 = versionStr.IndexOf(".", StringComparison.Ordinal);
      if (num1 <= 0)
        return false;
      int num2 = versionStr.IndexOf(".", num1 + 1, StringComparison.Ordinal);
      return num2 > 0 && num2 > num1 + 1 && int.TryParse(versionStr.Substring(num1 + 1, num2 - num1 - 1), out minorVersion);
    }

    public static string GetMinorVersionName(string minorVersion)
    {
      string minorVersionName;
      switch (minorVersion)
      {
        case "6":
          minorVersionName = "Update 2";
          break;
        case "5":
          minorVersionName = "Update 1";
          break;
        case "4":
          minorVersionName = "Early Access";
          break;
        case "3":
          minorVersionName = "Beta";
          break;
        case "2":
          minorVersionName = "Alpha";
          break;
        case "1":
          minorVersionName = "Pre-alpha";
          break;
        default:
          minorVersionName = "unknown version";
          break;
      }
      return minorVersionName;
    }

    static GameVersion()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      GameVersion.FULL_DISPLAY_VALUE = string.Format("{0} | v{1} (b{2})", (object) "Update 2", (object) "0.6.3a", (object) 333U).AsLoc();
    }
  }
}
