// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Localization.Quantity.IntegerSiSuffixFormatter
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Localization.Quantity
{
  public abstract class IntegerSiSuffixFormatter
  {
    public static LocStrFormatted FormatNumber(long number)
    {
      long num = number.Abs();
      if (num < 1000L)
        return new LocStrFormatted(((int) number).ToStringCached());
      return num < 1000000L ? (num < 10000L ? new LocStrFormatted(((float) number / 1000f).RoundToSigDigits(2, false, false, false) + "k") : new LocStrFormatted((number / 1000L).ToString() + "k")) : (num < 1000000000L ? (num < 10000000L ? new LocStrFormatted(((float) number / 1000000f).RoundToSigDigits(2, false, false, false) + "M") : new LocStrFormatted((number / 1000000L).ToString() + "M")) : (num < 10000000000L ? new LocStrFormatted(((float) number / 1E+09f).RoundToSigDigits(2, false, false, false) + "G") : new LocStrFormatted((number / 1000000000L).ToString() + "G")));
    }

    protected IntegerSiSuffixFormatter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
