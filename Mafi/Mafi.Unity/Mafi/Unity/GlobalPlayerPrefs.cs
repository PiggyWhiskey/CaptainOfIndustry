// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.GlobalPlayerPrefs
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.SaveGame;
using Mafi.Logging.Graylog;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  /// <summary>
  /// A wrapper around <see cref="T:UnityEngine.PlayerPrefs" /> for storing player preferences.
  /// On Windows, PlayerPrefs are stored in 'HKCU\Software\ExampleCompanyName\ExampleProductName' key.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class GlobalPlayerPrefs : ISaveConfig, IErrorLoggerConfig
  {
    private static int? s_autosaveIntervalMins;
    private static bool? s_disableAnonymousErrorLogs;
    private static bool? s_enableLogs;
    private static bool? s_disableFlashes;

    public static int AutosaveIntervalMins
    {
      get
      {
        if (GlobalPlayerPrefs.s_autosaveIntervalMins.HasValue)
          return GlobalPlayerPrefs.s_autosaveIntervalMins.Value;
        GlobalPlayerPrefs.s_autosaveIntervalMins = new int?(PlayerPrefs.GetInt(nameof (AutosaveIntervalMins), SaveManager.AUTOSAVE_DEFAULT_INTERVAL_MINUTES));
        if (SaveManager.AUTOSAVE_OPTIONS_MINUTES.IndexOf(GlobalPlayerPrefs.s_autosaveIntervalMins.Value) == -1)
          GlobalPlayerPrefs.s_autosaveIntervalMins = new int?(SaveManager.AUTOSAVE_DEFAULT_INTERVAL_MINUTES);
        return GlobalPlayerPrefs.s_autosaveIntervalMins.Value;
      }
      set
      {
        GlobalPlayerPrefs.s_autosaveIntervalMins = new int?(value);
        try
        {
          PlayerPrefs.SetInt(nameof (AutosaveIntervalMins), value);
          PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to save player perf");
        }
      }
    }

    Duration? ISaveConfig.AutoSaveInterval
    {
      get
      {
        return GlobalPlayerPrefs.AutosaveIntervalMins <= 0 ? new Duration?() : new Duration?(Duration.FromMin((double) GlobalPlayerPrefs.AutosaveIntervalMins));
      }
    }

    SaveCompressionType ISaveConfig.SaveCompressionType => SaveCompressionType.Gzip;

    public static bool DisableAnonymousErrorLogs
    {
      get
      {
        if (GlobalPlayerPrefs.s_disableAnonymousErrorLogs.HasValue)
          return GlobalPlayerPrefs.s_disableAnonymousErrorLogs.Value;
        GlobalPlayerPrefs.s_disableAnonymousErrorLogs = new bool?(PlayerPrefs.GetInt("SendAnonymousErrorLogs", 0) > 0);
        return GlobalPlayerPrefs.s_disableAnonymousErrorLogs.Value;
      }
      set
      {
        GlobalPlayerPrefs.s_disableAnonymousErrorLogs = new bool?(value);
        try
        {
          PlayerPrefs.SetInt("SendAnonymousErrorLogs", value ? 1 : 0);
          PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to save player perf");
        }
      }
    }

    bool IErrorLoggerConfig.DisableAnonymousErrorLogs
    {
      get => GlobalPlayerPrefs.DisableAnonymousErrorLogs;
    }

    public static bool EnableMods
    {
      get
      {
        if (GlobalPlayerPrefs.s_enableLogs.HasValue)
          return GlobalPlayerPrefs.s_enableLogs.Value;
        GlobalPlayerPrefs.s_enableLogs = new bool?(PlayerPrefs.GetInt(nameof (EnableMods), 0) > 0);
        return GlobalPlayerPrefs.s_enableLogs.Value;
      }
      set
      {
        GlobalPlayerPrefs.s_enableLogs = new bool?(value);
        try
        {
          PlayerPrefs.SetInt(nameof (EnableMods), value ? 1 : 0);
          PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to save player perf");
        }
      }
    }

    public static bool DisableFlashes
    {
      get
      {
        if (GlobalPlayerPrefs.s_disableFlashes.HasValue)
          return GlobalPlayerPrefs.s_disableFlashes.Value;
        GlobalPlayerPrefs.s_disableFlashes = new bool?(PlayerPrefs.GetInt(nameof (DisableFlashes), 0) > 0);
        return GlobalPlayerPrefs.s_disableFlashes.Value;
      }
      set
      {
        GlobalPlayerPrefs.s_disableFlashes = new bool?(value);
        try
        {
          PlayerPrefs.SetInt(nameof (DisableFlashes), value ? 1 : 0);
          PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to save player perf");
        }
      }
    }

    public GlobalPlayerPrefs()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
