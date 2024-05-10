// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.LocStr2
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Localization
{
  /// <summary>
  /// Localized string with one argument but without plural forms. Serializable if unformatted.
  /// </summary>
  public readonly struct LocStr2
  {
    public static LocStr2 Empty;
    public readonly string Id;
    private readonly string m_translatedString;

    public LocStr2(string id, string enString, string translatedString)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Id = id;
      if (!LocAsserts.VerifyParams(id, translatedString, 2))
        translatedString = enString;
      this.m_translatedString = translatedString;
    }

    public LocStrFormatted Format(Percent arg1, Percent arg2)
    {
      return this.Format(arg1.ToString(), arg2.ToString());
    }

    public LocStrFormatted Format(LocStr arg1, LocStr arg2)
    {
      return this.Format(arg1.TranslatedString, arg2.TranslatedString);
    }

    public LocStrFormatted Format(LocStrFormatted arg1, LocStrFormatted arg2)
    {
      return this.Format(arg1.Value, arg2.Value);
    }

    public LocStrFormatted Format(string arg1, string arg2)
    {
      try
      {
        return new LocStrFormatted(string.Format(this.m_translatedString, (object) arg1, (object) arg2));
      }
      catch (FormatException ex)
      {
        Log.Exception((Exception) ex, "Failed to format " + this.Id);
      }
      return new LocStrFormatted(this.m_translatedString);
    }

    public override string ToString()
    {
      Log.Error("Use .Format(arg1, arg2)!");
      return this.m_translatedString;
    }

    static LocStr2()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LocStr2.Empty = Loc.Str2("EmptyString__Arg2", "{0}{1}", "HIDE (this must remain '{0}{1}')");
    }
  }
}
