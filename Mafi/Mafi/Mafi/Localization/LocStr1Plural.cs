// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.LocStr1Plural
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using System;

#nullable disable
namespace Mafi.Localization
{
  /// <summary>
  /// Localized string with one argument with plural forms. Serializable if unformatted.
  /// </summary>
  public readonly struct LocStr1Plural
  {
    public static LocStr1Plural Empty;
    public readonly string Id;
    private readonly ImmutableArray<string> m_translations;
    private readonly Func<Fix64, int> m_indexSelector;

    internal LocStr1Plural(
      string id,
      ImmutableArray<string> enTranslations,
      Func<Fix64, int> enIndexSelector,
      ImmutableArray<string> translations,
      Func<Fix64, int> indexSelector)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Id = id;
      Assert.That<ImmutableArray<string>>(translations).IsNotEmpty<string>();
      bool flag = true;
      foreach (string translation in translations)
        flag &= LocAsserts.VerifyParams(id, translation, 1);
      if (flag)
      {
        this.m_translations = translations;
        this.m_indexSelector = indexSelector;
      }
      else
      {
        this.m_translations = enTranslations;
        this.m_indexSelector = enIndexSelector;
      }
    }

    public LocStrFormatted Format(int arg) => this.Format(arg.ToStringCached(), arg.ToFix64());

    public LocStrFormatted Format(string arg, int quantity) => this.Format(arg, quantity.ToFix64());

    public LocStrFormatted Format(string arg, Fix64 quantity)
    {
      int index = this.m_indexSelector(quantity);
      if ((uint) index >= (uint) this.m_translations.Length)
      {
        if (this.m_translations.Length == 0)
        {
          Log.Error("No translation plural forms!");
          return LocStrFormatted.Empty;
        }
        Log.Warning(string.Format("Not enough plural forms of {0}, requested index {1}, length {2}", (object) this.Id, (object) index, (object) this.m_translations.Length));
        index = index.Clamp(0, this.m_translations.Length - 1);
      }
      try
      {
        return new LocStrFormatted(string.Format(this.m_translations[index], (object) arg));
      }
      catch (FormatException ex)
      {
        Log.Exception((Exception) ex, "Failed to format " + this.Id);
      }
      return new LocStrFormatted(this.m_translations[index]);
    }

    public override string ToString()
    {
      Log.Error("Use .Format(arg)!");
      return this.m_translations.FirstOrDefault() ?? "";
    }

    static LocStr1Plural()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LocStr1Plural.Empty = Loc.Str1Plural("EmptyString__Arg1Plural", "{0}", "{0}", "HIDE (this must remain '{0}')");
    }
  }
}
