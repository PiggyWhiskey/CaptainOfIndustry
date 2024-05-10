// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.Loc
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Localization
{
  /// <summary>Localization helper class.</summary>
  public static class Loc
  {
    public const string NAME_SUFFIX = "__name";
    public const string DESC_SUFFIX = "__desc";

    /// <summary>Creates localized string without any arguments.</summary>
    public static LocStr Str(string id, string enUs, string comment)
    {
      return LocalizationManager.GetLocalizedString0Arg(id, enUs, comment);
    }

    /// <summary>
    /// Creates localized string with one argument but without plural forms, the argument does not affect
    /// plurality. Note that some languages may need to change other words than nouns based on numeric argument.
    /// </summary>
    public static LocStr1 Str1(string id, string enUs, string comment)
    {
      return LocalizationManager.GetLocalizedString1Arg(id, enUs, comment);
    }

    /// <summary>
    /// Creates localized string with two arguments but without plural forms, the arguments do not affect
    /// plurality. Note that some languages may need to change other words than nouns based on numeric argument.
    /// </summary>
    public static LocStr2 Str2(string id, string enUs, string comment)
    {
      return LocalizationManager.GetLocalizedString2Arg(id, enUs, comment);
    }

    /// <summary>
    /// Creates localized string with three arguments but without plural forms, the arguments do not affect
    /// plurality. Note that some languages may need to change other words than nouns based on numeric argument.
    /// </summary>
    public static LocStr3 Str3(string id, string enUs, string comment)
    {
      return LocalizationManager.GetLocalizedString3Arg(id, enUs, comment);
    }

    /// <summary>
    /// Creates localized string with four arguments but without plural forms, the arguments do not affect
    /// plurality. Note that some languages may need to change other words than nouns based on numeric argument.
    /// </summary>
    public static LocStr4 Str4(string id, string enUs, string comment)
    {
      return LocalizationManager.GetLocalizedString4Arg(id, enUs, comment);
    }

    /// <summary>
    /// Creates localized string with one argument that has plural forms. The correct plural form will be
    /// chosen automatically during formatting using <see cref="M:Mafi.Localization.LocStr1Plural.Format(System.Int32)" />.
    /// </summary>
    public static LocStr1Plural Str1Plural(string id, string enUs, string plural, string comment)
    {
      return LocalizationManager.GetLocalizedString1Arg(id, enUs, plural, comment);
    }
  }
}
