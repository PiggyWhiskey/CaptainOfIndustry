// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.LocalizationManager
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Localization
{
  /// <summary>
  /// Manages strings localizations.
  /// 
  /// To create localized strings use helper class <see cref="T:Mafi.Localization.Loc" />. All strings MUST be created during the
  /// initialization phase. Errors will be thrown for strings created later.
  /// 
  /// Each string is identified by ID which must be unique and non-changing. This ID is also uses for saving/loading
  /// of localized strings, so that translations can be fixed for saved games.
  /// </summary>
  public static class LocalizationManager
  {
    internal const bool FUZZ_ENABLED = false;
    public const string EN_US_CULTURE_INFO_ID = "en-US";
    public const string TODO_HIDE = "TODO";
    public const string HIDE_HIDE = "HIDE";
    private static Dict<string, LocalizationManager.LocDataEnUs> s_enUsData;
    private static readonly Func<Fix64, int> PLURAL_FN_NO_PLURAL;
    private static readonly Func<Fix64, int> PLURAL_FN_EN;
    private static readonly Func<Fix64, int> PLURAL_FN_CZ;
    private static readonly Func<Fix64, int> PLURAL_FN_PL;
    private static readonly Func<Fix64, int> PLURAL_FN_RU;
    private static readonly Func<Fix64, int> PLURAL_FN_PT;
    public static readonly ImmutableArray<LocalizationManager.LangInfo> LanguagesAvailable;
    private static Func<Fix64, int> s_currentLanguagePluralIndexFn;
    private static Dict<string, LocalizationManager.LocData> s_data;
    private static Dict<string, LocalizationManager.LocData> s_changelogData;
    private static Set<string> s_skipForExport;
    private static bool s_ignoreDuplicates;
    private static readonly Lyst<string> s_translationsWarnings;
    private static readonly Lyst<string> s_translationsErrors;

    public static LocalizationManager.LangInfo CurrentLangInfo { get; private set; }

    public static string TrInfoStr { get; private set; }

    public static CultureInfo CurrentCultureInfo { get; private set; }

    public static IIndexable<string> TranslationWarnings
    {
      get => (IIndexable<string>) LocalizationManager.s_translationsWarnings;
    }

    public static IIndexable<string> TranslationErrors
    {
      get => (IIndexable<string>) LocalizationManager.s_translationsErrors;
    }

    /// <summary>
    /// When called, all duplicates will be ignored and re-registering data will override current ones.
    /// </summary>
    public static void IgnoreDuplicates() => LocalizationManager.s_ignoreDuplicates = true;

    internal static void TestOnly_DisableLocalization()
    {
      LocalizationManager.s_enUsData = (Dict<string, LocalizationManager.LocDataEnUs>) null;
      LocalizationManager.s_data = (Dict<string, LocalizationManager.LocData>) null;
      LocalizationManager.s_skipForExport = (Set<string>) null;
    }

    public static void EnableLocalization()
    {
      LocalizationManager.s_enUsData = new Dict<string, LocalizationManager.LocDataEnUs>(1024);
      LocalizationManager.s_skipForExport = new Set<string>();
    }

    /// <summary>
    /// To make sure all static fields are initialized, this should be called on all loaded assemblies.
    /// </summary>
    public static void ScanForStaticLocStrFields(IEnumerable<Assembly> assemblies)
    {
      foreach (Assembly assembly in assemblies)
      {
        try
        {
          LocalizationManager.ScanForStaticLocStrFields(assembly);
        }
        catch (Exception ex)
        {
          Log.Warning("Failed to scan assembly '" + assembly.FullName + "' for static str fields, likely due to incompatible mod: " + ex.Message);
        }
      }
    }

    public static void ScanForStaticLocStrFields(Assembly assembly)
    {
      if (LocalizationManager.s_enUsData == null)
        return;
      foreach (Type type in assembly.GetTypes())
      {
        try
        {
          foreach (FieldInfo field in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
          {
            if (field.FieldType == typeof (LocStr))
            {
              LocStr locStr = (LocStr) field.GetValue((object) null);
              if (locStr != LocStr.Empty)
                Assert.That<Dict<string, LocalizationManager.LocDataEnUs>>(LocalizationManager.s_enUsData).ContainsKey<string, LocalizationManager.LocDataEnUs>(locStr.Id);
            }
            else if (field.FieldType == typeof (LocStr1))
            {
              LocStr1 locStr1 = (LocStr1) field.GetValue((object) null);
              Assert.That<Dict<string, LocalizationManager.LocDataEnUs>>(LocalizationManager.s_enUsData).ContainsKey<string, LocalizationManager.LocDataEnUs>(locStr1.Id);
            }
            else if (field.FieldType == typeof (LocStr1Plural))
            {
              LocStr1Plural locStr1Plural = (LocStr1Plural) field.GetValue((object) null);
              Assert.That<Dict<string, LocalizationManager.LocDataEnUs>>(LocalizationManager.s_enUsData).ContainsKey<string, LocalizationManager.LocDataEnUs>(locStr1Plural.Id);
            }
            else if (field.FieldType == typeof (LocStr2))
            {
              LocStr2 locStr2 = (LocStr2) field.GetValue((object) null);
              Assert.That<Dict<string, LocalizationManager.LocDataEnUs>>(LocalizationManager.s_enUsData).ContainsKey<string, LocalizationManager.LocDataEnUs>(locStr2.Id);
            }
            else if (field.FieldType == typeof (LocStr3))
            {
              LocStr3 locStr3 = (LocStr3) field.GetValue((object) null);
              Assert.That<Dict<string, LocalizationManager.LocDataEnUs>>(LocalizationManager.s_enUsData).ContainsKey<string, LocalizationManager.LocDataEnUs>(locStr3.Id);
            }
          }
        }
        catch (Exception ex)
        {
          Log.Warning("Failed to scan type '" + type.FullName + "' for static str fields: " + ex.Message);
        }
      }
    }

    public static LocStr CreateAlreadyLocalizedStr(string id, string enUs)
    {
      return LocalizationManager.GetLocalizedString0Arg(id, enUs, "No export", true);
    }

    public static LocStr CreateAlreadyLocalizedFormatted(string id, LocStrFormatted enUs)
    {
      return LocalizationManager.GetLocalizedString0Arg(id + "_formatted", enUs.Value, "No export", true);
    }

    public static LocStr GetLocalizedString0Arg(
      string id,
      string enUs,
      string comment,
      bool skipForExport = false,
      bool ignoreDuplicates = false)
    {
      if (LocalizationManager.s_enUsData != null)
      {
        LocalizationManager.addToEnUs(id, new LocalizationManager.LocDataEnUs(enUs, Option<string>.None, comment), ignoreDuplicates);
        if (skipForExport)
          LocalizationManager.s_skipForExport.Add(id);
        LocalizationManager.LocData locData;
        if (LocalizationManager.s_data != null && LocalizationManager.s_data.TryGetValue(id, out locData))
        {
          LocalizationManager.checkSingleString(id, enUs, locData.TranslatedStrings);
          return new LocStr(id, locData.TranslatedStrings.FirstOrDefault() ?? enUs);
        }
      }
      return new LocStr(id, enUs);
    }

    public static LocStr1 CreateAlreadyLocalizedStr1(string id, string enUs)
    {
      return LocalizationManager.GetLocalizedString1Arg(id, enUs, "No export", true);
    }

    public static LocStr1 GetLocalizedString1Arg(
      string id,
      string enUs,
      string comment,
      bool skipForExport = false,
      bool ignoreDuplicates = false)
    {
      if (LocalizationManager.s_enUsData != null)
      {
        LocalizationManager.addToEnUs(id, new LocalizationManager.LocDataEnUs(enUs, Option<string>.None, comment), ignoreDuplicates);
        if (skipForExport)
          LocalizationManager.s_skipForExport.Add(id);
        LocalizationManager.LocData locData;
        if (LocalizationManager.s_data != null && LocalizationManager.s_data.TryGetValue(id, out locData))
        {
          LocalizationManager.checkSingleString(id, enUs, locData.TranslatedStrings);
          return new LocStr1(id, enUs, locData.TranslatedStrings.FirstOrDefault() ?? enUs);
        }
      }
      return new LocStr1(id, enUs, enUs);
    }

    public static LocStr2 GetLocalizedString2Arg(string id, string enUs, string comment)
    {
      if (LocalizationManager.s_enUsData != null)
      {
        LocalizationManager.addToEnUs(id, new LocalizationManager.LocDataEnUs(enUs, Option<string>.None, comment));
        LocalizationManager.LocData locData;
        if (LocalizationManager.s_data != null && LocalizationManager.s_data.TryGetValue(id, out locData))
        {
          LocalizationManager.checkSingleString(id, enUs, locData.TranslatedStrings);
          return new LocStr2(id, enUs, locData.TranslatedStrings.FirstOrDefault() ?? enUs);
        }
      }
      return new LocStr2(id, enUs, enUs);
    }

    public static LocStr3 GetLocalizedString3Arg(string id, string enUs, string comment)
    {
      if (LocalizationManager.s_enUsData != null)
      {
        LocalizationManager.addToEnUs(id, new LocalizationManager.LocDataEnUs(enUs, Option<string>.None, comment));
        LocalizationManager.LocData locData;
        if (LocalizationManager.s_data != null && LocalizationManager.s_data.TryGetValue(id, out locData))
        {
          LocalizationManager.checkSingleString(id, enUs, locData.TranslatedStrings);
          return new LocStr3(id, enUs, locData.TranslatedStrings.FirstOrDefault() ?? enUs);
        }
      }
      return new LocStr3(id, enUs, enUs);
    }

    public static LocStr4 GetLocalizedString4Arg(string id, string enUs, string comment)
    {
      if (LocalizationManager.s_enUsData != null)
      {
        LocalizationManager.addToEnUs(id, new LocalizationManager.LocDataEnUs(enUs, Option<string>.None, comment));
        LocalizationManager.LocData locData;
        if (LocalizationManager.s_data != null && LocalizationManager.s_data.TryGetValue(id, out locData))
        {
          LocalizationManager.checkSingleString(id, enUs, locData.TranslatedStrings);
          return new LocStr4(id, enUs, locData.TranslatedStrings.FirstOrDefault() ?? enUs);
        }
      }
      return new LocStr4(id, enUs, enUs);
    }

    public static LocStr1Plural GetLocalizedString1Arg(
      string id,
      string enUs,
      string plural,
      string comment)
    {
      ImmutableArray<string> immutableArray = ImmutableArray.Create<string>(enUs, plural);
      if (LocalizationManager.s_enUsData != null)
      {
        LocalizationManager.addToEnUs(id, new LocalizationManager.LocDataEnUs(enUs, (Option<string>) plural, comment));
        LocalizationManager.LocData locData;
        if (LocalizationManager.s_data != null && LocalizationManager.s_data.TryGetValue(id, out locData))
        {
          if (locData.TranslatedStrings.IsEmpty)
          {
            LocalizationManager.RecordTranslationError(id, "No translations found.");
            return new LocStr1Plural(id, immutableArray, LocalizationManager.PLURAL_FN_EN, immutableArray, LocalizationManager.PLURAL_FN_EN);
          }
          ImmutableArray<string> translations = locData.TranslatedStrings;
          if (LocalizationManager.CurrentLangInfo.CultureInfoId != "en-US")
          {
            int index1 = 0;
            for (int index2 = immutableArray.Length.Min(translations.Length); index1 < index2; ++index1)
            {
              if (immutableArray[index1] == translations[index1])
                LocalizationManager.RecordTranslationWarning(id, string.Format("Potentially untranslated plural {0}: {1}", (object) index1, (object) immutableArray[index1]));
            }
          }
          if (translations.Length < LocalizationManager.CurrentLangInfo.PluralFormsCount)
          {
            LocalizationManager.RecordTranslationError(id, string.Format("Not enough plural forms, have {0}, ", (object) translations.Length) + string.Format("expected {0}.", (object) LocalizationManager.CurrentLangInfo.PluralFormsCount));
            translations = translations.Concat(((ICollection<string>) translations.Last.Repeat<string>(LocalizationManager.CurrentLangInfo.PluralFormsCount - translations.Length)).ToImmutableArray<string>());
          }
          else if (translations.Length > LocalizationManager.CurrentLangInfo.PluralFormsCount)
            LocalizationManager.RecordTranslationError(id, string.Format("Too many plural forms, have {0}, ", (object) translations.Length) + string.Format("expected {0}.", (object) LocalizationManager.CurrentLangInfo.PluralFormsCount));
          return new LocStr1Plural(id, immutableArray, LocalizationManager.PLURAL_FN_EN, translations, LocalizationManager.s_currentLanguagePluralIndexFn);
        }
      }
      return new LocStr1Plural(id, immutableArray, LocalizationManager.PLURAL_FN_EN, immutableArray, LocalizationManager.PLURAL_FN_EN);
    }

    private static void addToEnUs(
      string id,
      LocalizationManager.LocDataEnUs data,
      bool ignoreDuplicates = false)
    {
      if (LocalizationManager.s_enUsData.TryAdd(id, data) || LocalizationManager.s_ignoreDuplicates || ignoreDuplicates)
        return;
      Log.Error("Duplicate translation ID '" + id + "'.");
    }

    private static void checkSingleString(string id, string enUs, ImmutableArray<string> strings)
    {
      if (strings.IsEmpty)
      {
        LocalizationManager.RecordTranslationError(id, "No translations found.");
      }
      else
      {
        if (strings.Length != 1)
          LocalizationManager.RecordTranslationError(id, string.Format("Expected only 1 translation, but got {0}.", (object) strings.Length));
        if (!(LocalizationManager.CurrentLangInfo.CultureInfoId != "en-US") || !strings.IsNotEmpty || !(strings.First == enUs))
          return;
        LocalizationManager.RecordTranslationWarning(id, "Potentially untranslated: " + enUs);
      }
    }

    public static void RecordTranslationWarning(string id, string warning)
    {
      string str = "[" + id + "]: " + warning;
      LocalizationManager.s_translationsWarnings.Add(str);
    }

    public static void RecordTranslationError(string id, string error)
    {
      string str = "[" + id + "]: " + error;
      LocalizationManager.s_translationsErrors.Add(str);
    }

    public static LocStr LoadLocalizedString0(string id)
    {
      if (LocalizationManager.s_enUsData == null)
        return new LocStr(id, "DISABLED: " + id);
      LocalizationManager.LocData locData;
      if (LocalizationManager.s_data != null && LocalizationManager.s_data.TryGetValue(id, out locData))
      {
        if (locData.TranslatedStrings.Length > 1)
          Log.Error(string.Format("Expected 1 translation for '{0}', but got {1}.", (object) id, (object) locData.TranslatedStrings.Length));
        return new LocStr(id, locData.TranslatedStrings.First);
      }
      LocalizationManager.LocDataEnUs locDataEnUs;
      if (LocalizationManager.s_enUsData.TryGetValue(id, out locDataEnUs))
        return new LocStr(id, locDataEnUs.EnUs);
      Log.Error("Translation '" + id + "' missing during load.");
      return new LocStr(id, "MISSING: " + id);
    }

    public static LocStr LoadOrCreateLocalizedString0(string id, string enUs)
    {
      if (LocalizationManager.s_enUsData == null)
        return new LocStr(id, enUs);
      LocalizationManager.LocData locData;
      if (LocalizationManager.s_data != null && LocalizationManager.s_data.TryGetValue(id, out locData))
      {
        if (locData.TranslatedStrings.Length > 1)
          Log.Error(string.Format("Expected 1 translation for '{0}', but got {1}.", (object) id, (object) locData.TranslatedStrings.Length));
        return new LocStr(id, locData.TranslatedStrings.First);
      }
      LocalizationManager.LocDataEnUs locDataEnUs;
      return LocalizationManager.s_enUsData.TryGetValue(id, out locDataEnUs) ? new LocStr(id, locDataEnUs.EnUs) : LocalizationManager.CreateAlreadyLocalizedStr(id, enUs);
    }

    public static bool TryGetLocalizedPathNotesForVersion(
      string version,
      out LocStrFormatted result)
    {
      if (LocalizationManager.s_changelogData == null)
      {
        result = LocStrFormatted.Empty;
        return false;
      }
      LocalizationManager.LocData locData;
      if (!LocalizationManager.s_changelogData.TryGetValue(version, out locData))
      {
        result = LocStrFormatted.Empty;
        return false;
      }
      if (locData.TranslatedStrings.Length == 0)
      {
        result = LocStrFormatted.Empty;
        return false;
      }
      if (locData.TranslatedStrings.Length > 1)
        Log.Warning("Too many translated strings for changelog '" + version + "' in lang '" + LocalizationManager.CurrentLangInfo.CultureInfoId + "'");
      result = new LocStrFormatted(locData.TranslatedStrings.First);
      return true;
    }

    public static bool TryLoadTranslationsFrom(
      string langData,
      string changelogData,
      LocalizationManager.LangInfo lang)
    {
      LocalizationManager.s_translationsWarnings.Clear();
      LocalizationManager.s_translationsErrors.Clear();
      if (LocalizationManager.s_enUsData.IsNotEmpty)
      {
        Log.Error("Some translation was already requested. Translation file must be loaded first!");
        return false;
      }
      if (LocalizationManager.s_data != null)
      {
        Log.Error("Some translation were already loaded, discarding!");
        LocalizationManager.s_data = (Dict<string, LocalizationManager.LocData>) null;
      }
      LocalizationManager.LangInfo currentLangInfo = LocalizationManager.CurrentLangInfo;
      LocalizationManager.CurrentLangInfo = lang;
      try
      {
        string trInfoStr;
        Dict<string, LocalizationManager.LocData> outData;
        bool poFileData = LocalizationUtils.TryParsePoFileData(langData, out trInfoStr, out outData);
        if (poFileData)
        {
          LocalizationManager.s_data = outData;
          LocalizationManager.TrInfoStr = trInfoStr;
          LocalizationManager.s_currentLanguagePluralIndexFn = lang.PluralIndexFunction;
        }
        try
        {
          LocalizationManager.CurrentCultureInfo = CultureInfo.CreateSpecificCulture(lang.CultureInfoId);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to create cultureInfo for " + lang.CultureInfoId);
        }
        LocalizationUtils.TryParsePoFileData(changelogData, out string _, out LocalizationManager.s_changelogData);
        return poFileData;
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed parsing a translation file");
        LocalizationManager.CurrentLangInfo = currentLangInfo;
      }
      return false;
    }

    public static void ExportGettextPoFile(TextWriter writer)
    {
      writeId("");
      writeStr(((IEnumerable<string>) new string[6]
      {
        "Language: en",
        "IME-Version: 1.0",
        "Content-Type: text/plain; charset=UTF-8",
        "Content-Transfer-Encoding: 8bit",
        "Plural-Forms: nplurals=2; plural=(n != 1);",
        "X-Generator: Captain of Industry v0.6.3a"
      }).JoinStrings("\n"));
      foreach (KeyValuePair<string, LocalizationManager.LocDataEnUs> keyValuePair in (IEnumerable<KeyValuePair<string, LocalizationManager.LocDataEnUs>>) LocalizationManager.s_enUsData.OrderBy<KeyValuePair<string, LocalizationManager.LocDataEnUs>, string>((Func<KeyValuePair<string, LocalizationManager.LocDataEnUs>, string>) (x => x.Key)))
      {
        LocalizationManager.LocDataEnUs locDataEnUs = keyValuePair.Value;
        if (!locDataEnUs.EnUs.IsEmpty() && !locDataEnUs.EnUs.Contains("TODO") && !locDataEnUs.EnUs.Contains("HIDE") && !locDataEnUs.Description.Contains("TODO") && !locDataEnUs.Description.Contains("HIDE") && !LocalizationManager.s_skipForExport.Contains(keyValuePair.Key))
        {
          writer.Write('\n');
          writeMeta(' ', locDataEnUs.Description);
          writeId(keyValuePair.Key);
          if (locDataEnUs.Plural.HasValue)
          {
            writeId(keyValuePair.Key, true);
            writeStr(locDataEnUs.EnUs, 0);
            writeStr(locDataEnUs.Plural.Value, 1);
          }
          else
            writeStr(locDataEnUs.EnUs);
        }
      }

      void writeId(string id, bool isPlural = false)
      {
        writer.Write("msgid");
        if (isPlural)
          writer.Write("_plural");
        writer.Write(" \"");
        writer.Write(id);
        writer.Write("\"\n");
      }

      void writeStr(string str, int index = -1)
      {
        writer.Write("msgstr");
        if (index >= 0)
        {
          writer.Write("[");
          writer.Write(index);
          writer.Write("]");
        }
        writer.Write(" \"");
        if (str.Length == 0)
        {
          writer.Write(" \"\n");
        }
        else
        {
          Lyst<string> lines = str.SplitToLines();
          if (lines.Count == 1)
          {
            writer.Write(str);
            writer.Write("\"\n");
          }
          else
          {
            writer.Write("\"\n");
            foreach (string str1 in lines)
            {
              writer.Write("\"");
              writer.Write(str1);
              writer.Write("\\n\"\n");
            }
          }
        }
      }

      void writeMeta(char type, string value)
      {
        writer.Write("#");
        writer.Write(type);
        writer.Write(" ");
        writer.Write(value);
        writer.Write("\n");
      }
    }

    public static void ExportAllEnglishText(TextWriter writer)
    {
      foreach (KeyValuePair<string, LocalizationManager.LocDataEnUs> keyValuePair in (IEnumerable<KeyValuePair<string, LocalizationManager.LocDataEnUs>>) LocalizationManager.s_enUsData.OrderBy<KeyValuePair<string, LocalizationManager.LocDataEnUs>, string>((Func<KeyValuePair<string, LocalizationManager.LocDataEnUs>, string>) (x => x.Key)))
      {
        LocalizationManager.LocDataEnUs locDataEnUs = keyValuePair.Value;
        if (!locDataEnUs.EnUs.IsEmpty() && !locDataEnUs.EnUs.Contains("TODO") && !locDataEnUs.EnUs.Contains("HIDE") && !locDataEnUs.Description.Contains("TODO") && !locDataEnUs.Description.Contains("HIDE") && !LocalizationManager.s_skipForExport.Contains(keyValuePair.Key))
        {
          writer.WriteLine(locDataEnUs.EnUs);
          if (locDataEnUs.Plural.HasValue)
            writer.WriteLine(locDataEnUs.Plural.Value);
          writer.WriteLine(locDataEnUs.Description);
          writer.WriteLine();
        }
      }
    }

    public static string GetTrCommentFor(string id)
    {
      LocalizationManager.LocDataEnUs locDataEnUs;
      return !LocalizationManager.s_enUsData.TryGetValue(id, out locDataEnUs) ? "" : locDataEnUs.Description;
    }

    public static string GetTrCommentFor(LocStr locStr)
    {
      return LocalizationManager.GetTrCommentFor(locStr.Id);
    }

    public static string GetUsEnStringFor(LocStr locStr)
    {
      Dict<string, LocalizationManager.LocDataEnUs> enUsData = LocalizationManager.s_enUsData;
      LocalizationManager.LocDataEnUs locDataEnUs;
      if ((enUsData != null ? (enUsData.TryGetValue(locStr.Id, out locDataEnUs) ? 1 : 0) : 0) != 0)
        return locDataEnUs.EnUs;
      Log.Error("Original string for " + locStr.Id + " not found!");
      return locStr.TranslatedString;
    }

    static LocalizationManager()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LocalizationManager.s_enUsData = new Dict<string, LocalizationManager.LocDataEnUs>(1024);
      LocalizationManager.PLURAL_FN_NO_PLURAL = (Func<Fix64, int>) (x => 0);
      LocalizationManager.PLURAL_FN_EN = (Func<Fix64, int>) (x => !(x == Fix64.One) ? 1 : 0);
      LocalizationManager.PLURAL_FN_CZ = (Func<Fix64, int>) (x =>
      {
        long integerPart = x.IntegerPart;
        if (integerPart == 1L)
          return 0;
        return integerPart < 2L || integerPart > 4L ? 2 : 1;
      });
      LocalizationManager.PLURAL_FN_PL = (Func<Fix64, int>) (x =>
      {
        long integerPart = x.IntegerPart;
        if (integerPart == 1L)
          return 0;
        return integerPart % 10L < 2L || integerPart % 10L > 4L || integerPart % 100L >= 10L && integerPart % 100L < 20L ? 2 : 1;
      });
      LocalizationManager.PLURAL_FN_RU = (Func<Fix64, int>) (x =>
      {
        long integerPart = x.IntegerPart;
        if (integerPart % 10L == 1L && integerPart % 100L != 11L)
          return 0;
        return integerPart % 10L < 2L || integerPart % 10L > 4L || integerPart % 100L >= 10L && integerPart % 100L < 20L ? 2 : 1;
      });
      LocalizationManager.PLURAL_FN_PT = (Func<Fix64, int>) (x => x > 1 ? 1 : 0);
      LocalizationManager.LanguagesAvailable = ((ICollection<LocalizationManager.LangInfo>) new LocalizationManager.LangInfo[20]
      {
        new LocalizationManager.LangInfo("en-US", "English", "en.po", 100.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("ca-ES", "Català", "ca.po", 100.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("cs-CZ", "Čeština ", "cs.po", 67.Percent(), 3, LocalizationManager.PLURAL_FN_CZ),
        new LocalizationManager.LangInfo("de-DE", "Deutsch", "de.po", 100.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("es-ES", "Español", "es.po", 81.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("et-EE", "Eesti keel", "et.po", 100.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("fr-FR", "Français", "fr.po", 100.Percent(), 2, (Func<Fix64, int>) (x => x > 1 ? 1 : 0)),
        new LocalizationManager.LangInfo("hu-HU", "Magyar nyelv", "hu.po", 91.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("it-IT", "Italiano", "it.po", 55.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("ja-JP", "日本語", "ja.po", 100.Percent(), 1, LocalizationManager.PLURAL_FN_NO_PLURAL, true),
        new LocalizationManager.LangInfo("ko-KR", "한국어", "ko.po", 100.Percent(), 1, LocalizationManager.PLURAL_FN_NO_PLURAL, true),
        new LocalizationManager.LangInfo("nl-NL", "Nederlands", "nl.po", 100.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("pl-PL", "Polski", "pl.po", 100.Percent(), 3, LocalizationManager.PLURAL_FN_PL),
        new LocalizationManager.LangInfo("pt-BR", "Português brasileiro", "pt_BR.po", 100.Percent(), 2, LocalizationManager.PLURAL_FN_PT),
        new LocalizationManager.LangInfo("ru-RU", "Русский", "ru.po", 100.Percent(), 3, LocalizationManager.PLURAL_FN_RU),
        new LocalizationManager.LangInfo("sv-SE", "Svenska", "sv.po", 41.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("tr-TR", "Türkçe", "tr.po", 79.Percent(), 2, LocalizationManager.PLURAL_FN_EN),
        new LocalizationManager.LangInfo("uk-UA", "Українська", "uk.po", 51.Percent(), 3, LocalizationManager.PLURAL_FN_RU),
        new LocalizationManager.LangInfo("zh-CN", "简体中文", "zh_Hans.po", 100.Percent(), 1, LocalizationManager.PLURAL_FN_NO_PLURAL, true),
        new LocalizationManager.LangInfo("zh-Hant", "繁體中文", "zh_Hant.po", 96.Percent(), 1, LocalizationManager.PLURAL_FN_NO_PLURAL, true)
      }).ToImmutableArray<LocalizationManager.LangInfo>();
      LocalizationManager.s_currentLanguagePluralIndexFn = LocalizationManager.PLURAL_FN_NO_PLURAL;
      LocalizationManager.s_data = (Dict<string, LocalizationManager.LocData>) null;
      LocalizationManager.s_changelogData = (Dict<string, LocalizationManager.LocData>) null;
      LocalizationManager.s_skipForExport = new Set<string>();
      LocalizationManager.s_ignoreDuplicates = false;
      // ISSUE: reference to a compiler-generated field
      LocalizationManager.\u003CCurrentLangInfo\u003Ek__BackingField = LocalizationManager.LanguagesAvailable[0];
      // ISSUE: reference to a compiler-generated field
      LocalizationManager.\u003CTrInfoStr\u003Ek__BackingField = "";
      // ISSUE: reference to a compiler-generated field
      LocalizationManager.\u003CCurrentCultureInfo\u003Ek__BackingField = CultureInfo.CreateSpecificCulture("en-US");
      LocalizationManager.s_translationsWarnings = new Lyst<string>();
      LocalizationManager.s_translationsErrors = new Lyst<string>();
    }

    public readonly struct LocData
    {
      public readonly ImmutableArray<string> TranslatedStrings;

      public LocData(ImmutableArray<string> translatedStrings)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.TranslatedStrings = translatedStrings;
      }
    }

    private readonly struct LocDataEnUs
    {
      public readonly string EnUs;
      public readonly Option<string> Plural;
      public readonly string Description;

      public LocDataEnUs(string enUs, Option<string> plural, string description)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.EnUs = enUs;
        this.Plural = plural;
        this.Description = description;
      }
    }

    public readonly struct LangInfo
    {
      /// <summary>
      /// Culture ID as used by <see cref="T:System.Globalization.CultureInfo" />. This is also used as a key save in user preferences.
      /// </summary>
      public readonly string CultureInfoId;
      /// <summary>
      /// Title of a language in already localized in the language.
      /// </summary>
      public readonly string LanguageTitle;
      /// <summary>Filename including extension.</summary>
      public readonly string FileName;
      public readonly Percent PercentTranslated;
      public readonly int PluralFormsCount;
      public readonly Func<Fix64, int> PluralIndexFunction;
      /// <summary>
      /// Whether this languages uses fonts that contains complex symbols such as Chinese. This allows
      /// the UI to accomodate to it by applying a different style or sizing.
      /// </summary>
      public readonly bool UsesSymbols;

      public LangInfo(
        string cultureInfoId,
        string languageTitle,
        string fileName,
        Percent percentTranslated,
        int pluralFormsCount,
        Func<Fix64, int> pluralIndexFunction,
        bool usesSymbols = false)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.CultureInfoId = cultureInfoId;
        this.LanguageTitle = languageTitle;
        this.FileName = fileName;
        this.PercentTranslated = percentTranslated.CheckNotNegative();
        this.PluralFormsCount = pluralFormsCount.CheckPositive();
        this.PluralIndexFunction = pluralIndexFunction.CheckNotNull<Func<Fix64, int>>();
        this.UsesSymbols = usesSymbols;
      }
    }
  }
}
