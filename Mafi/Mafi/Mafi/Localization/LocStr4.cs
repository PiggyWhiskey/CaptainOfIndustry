// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.LocStr4
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
  /// Localized string with four arguments but without plural forms. Serializable if unformatted.
  /// </summary>
  public readonly struct LocStr4
  {
    public static LocStr4 Empty;
    public readonly string Id;
    private readonly string m_translatedString;

    public LocStr4(string id, string enString, string translatedString)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Id = id;
      if (!LocAsserts.VerifyParams(id, translatedString, 4))
        translatedString = enString;
      this.m_translatedString = translatedString;
    }

    public LocStrFormatted Format(string arg1, string arg2, string arg3, string arg4)
    {
      try
      {
        return new LocStrFormatted(string.Format(this.m_translatedString, (object) arg1, (object) arg2, (object) arg3, (object) arg4));
      }
      catch (FormatException ex)
      {
        Log.Exception((Exception) ex, "Failed to format " + this.Id);
      }
      return new LocStrFormatted(this.m_translatedString);
    }

    public override string ToString()
    {
      Log.Error("Use .Format(arg1, arg2, arg3, arg4)!");
      return this.m_translatedString;
    }

    static LocStr4()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LocStr4.Empty = Loc.Str4("EmptyString__Arg4", "{0}{1}{2}{3}", "HIDE (this must remain '{0}{1}{2}{3}')");
    }
  }
}
