// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.LocalizationSettings
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using System;
using System.IO;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu
{
  /// <summary>
  /// Manages the current language selected and applies it on game start.
  /// </summary>
  internal class LocalizationSettings
  {
    public const string TRANSLATIONS_DIR = "Translations";
    public const string TR_CHANGELOG_DIR = "Changelog";
    public static ImmutableArray<string> LanguageStrOptions;

    public static int GetCurrentLangIndex()
    {
      string currentLangCode = LocalizationSettings.getCurrentLanguageKeyInPrefs();
      int currentLangIndex = LocalizationManager.LanguagesAvailable.IndexOf((Predicate<LocalizationManager.LangInfo>) (x => x.CultureInfoId == currentLangCode));
      if (currentLangIndex != -1)
        return currentLangIndex;
      Log.Warning("Invalid current language '" + currentLangCode + "'. Fallback to english.");
      return 0;
    }

    public static void SetNewLangIndex(int index)
    {
      if (index < 0 || index >= LocalizationManager.LanguagesAvailable.Length)
      {
        Log.Error(string.Format("Invalid lang index {0}", (object) index));
      }
      else
      {
        PlayerPrefs.SetString("UserInterfaceLanguage", LocalizationManager.LanguagesAvailable[index].CultureInfoId);
        PlayerPrefs.Save();
      }
    }

    public static void ApplyCurrentLanguage()
    {
      int currentLangIndex = LocalizationSettings.GetCurrentLangIndex();
      LocalizationManager.LangInfo lang = LocalizationManager.LanguagesAvailable[currentLangIndex];
      if (lang.CultureInfoId == "en-US")
      {
        Log.Info("Default language '" + lang.LanguageTitle + "'");
      }
      else
      {
        string path1 = Path.Combine("Translations", lang.FileName);
        if (!File.Exists(path1))
        {
          Log.Error("Failed to load language '" + lang.LanguageTitle + "', file '" + path1 + "' does not exist.");
        }
        else
        {
          string langData;
          try
          {
            langData = File.ReadAllText(path1);
          }
          catch (Exception ex)
          {
            Log.Exception(ex, "Failed to load language '" + lang.LanguageTitle + "', error while reading file '" + path1 + "'");
            return;
          }
          string path2 = Path.Combine("Translations", "Changelog", lang.FileName);
          string changelogData;
          try
          {
            changelogData = File.Exists(path2) ? File.ReadAllText(path2) : "";
          }
          catch (Exception ex)
          {
            Log.Exception(ex, "Failed to load changelog for language '" + lang.LanguageTitle + "', error while reading file '" + path2 + "'");
            changelogData = "";
          }
          if (LocalizationManager.TryLoadTranslationsFrom(langData, changelogData, lang))
            Log.Info("Language '" + lang.LanguageTitle + "' loaded");
          else
            Log.Error("Failed to load language '" + lang.LanguageTitle + "'");
        }
      }
    }

    private static string getCurrentLanguageKeyInPrefs()
    {
      return PlayerPrefs.GetString("UserInterfaceLanguage", "en-US");
    }

    public LocalizationSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static LocalizationSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LocalizationSettings.LanguageStrOptions = LocalizationManager.LanguagesAvailable.Select<string>((Func<LocalizationManager.LangInfo, string>) (x => !(x.PercentTranslated < Percent.Hundred) ? x.LanguageTitle : x.LanguageTitle + " (" + x.PercentTranslated.ToStringRounded() + ")")).ToImmutableArray<string>();
    }
  }
}
