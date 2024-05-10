// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.TranslationExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Localization
{
  public static class TranslationExtensions
  {
    /// <summary>
    /// Use this to ark temporary strings so you can search them to add translations.
    /// </summary>
    public static LocStrFormatted ToDoTranslate(this string str) => new LocStrFormatted(str);

    /// <summary>
    /// Use this if you deliberately decided that the string should not be translated.
    /// </summary>
    public static LocStrFormatted AsLoc(this string str)
    {
      return str == null ? LocStrFormatted.Empty : new LocStrFormatted(str);
    }
  }
}
