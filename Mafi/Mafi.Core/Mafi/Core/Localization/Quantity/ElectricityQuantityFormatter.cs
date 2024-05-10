// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Localization.Quantity.ElectricityQuantityFormatter
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Localization.Quantity
{
  public class ElectricityQuantityFormatter : QuantityFormatter
  {
    public static readonly ElectricityQuantityFormatter Instance;
    private static readonly LocStr1 KW_UNIT;
    private static readonly LocStr1 MW_UNIT;
    private static readonly LocStr1 GW_UNIT;

    private ElectricityQuantityFormatter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public override FormatInfo GetFormatInfo(IProtoWithIconAndName proto, QuantityLarge quantity)
    {
      return ElectricityQuantityFormatter.GetFormatInfo(quantity);
    }

    public static FormatInfo GetFormatInfo(QuantityLarge quantity)
    {
      if (quantity.Value < 1000L)
        return new FormatInfo(LocStr1Plural.Empty, new LocStr1?(ElectricityQuantityFormatter.KW_UNIT), 1);
      return quantity.Value < 1000000L ? new FormatInfo(LocStr1Plural.Empty, new LocStr1?(ElectricityQuantityFormatter.MW_UNIT), 1000) : new FormatInfo(LocStr1Plural.Empty, new LocStr1?(ElectricityQuantityFormatter.GW_UNIT), 1000000);
    }

    public LocStrFormatted FormatNumberAndUnitOnly(QuantityLarge quantity)
    {
      return this.FormatNumberAndUnitOnly(quantity, ElectricityQuantityFormatter.GetFormatInfo(quantity));
    }

    static ElectricityQuantityFormatter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ElectricityQuantityFormatter.Instance = new ElectricityQuantityFormatter();
      ElectricityQuantityFormatter.KW_UNIT = Loc.Str1("Electricity__Kw", "{0} KW", "Electricity in kilo-watts");
      ElectricityQuantityFormatter.MW_UNIT = Loc.Str1("Electricity__Mw", "{0} MW", "Electricity in mega-watts");
      ElectricityQuantityFormatter.GW_UNIT = Loc.Str1("Electricity__Gw", "{0} GW", "Electricity in giga-watts");
    }
  }
}
