// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Localization.Quantity.FormatInfo
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Localization.Quantity
{
  public struct FormatInfo
  {
    public readonly LocStr1Plural ProductPluralStr;
    public readonly LocStr1? UnitStr;
    public readonly int Denominator;

    public FormatInfo(LocStr1Plural productPluralStr, LocStr1? unitStr, int denominator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ProductPluralStr = productPluralStr;
      this.UnitStr = unitStr;
      this.Denominator = denominator;
    }
  }
}
