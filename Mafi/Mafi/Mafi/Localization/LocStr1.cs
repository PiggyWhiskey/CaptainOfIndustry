// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.LocStr1
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
  public readonly struct LocStr1
  {
    public static LocStr1 Empty;
    public readonly string Id;
    private readonly string m_translatedString;

    internal LocStr1(string id, string enString, string translatedString)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Id = id;
      if (!LocAsserts.VerifyParams(id, translatedString, 1))
        translatedString = enString;
      this.m_translatedString = translatedString;
    }

    public LocStrFormatted Format(LocStr arg) => this.Format(arg.TranslatedString);

    public LocStrFormatted Format(string arg)
    {
      try
      {
        return new LocStrFormatted(string.Format(this.m_translatedString, (object) arg));
      }
      catch (FormatException ex)
      {
        Log.Exception((Exception) ex, "Failed to format " + this.Id);
      }
      return new LocStrFormatted(this.m_translatedString);
    }

    public LocStrFormatted Format(Percent arg) => this.Format(arg.ToString());

    public LocStrFormatted Format(LocStrFormatted arg) => this.Format(arg.Value);

    public override string ToString()
    {
      Log.Error("Use .Format(arg)!");
      return this.m_translatedString;
    }

    static LocStr1()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LocStr1.Empty = Loc.Str1("EmptyString__Arg1", "{0}", "HIDE (this must remain '{0}')");
    }
  }
}
