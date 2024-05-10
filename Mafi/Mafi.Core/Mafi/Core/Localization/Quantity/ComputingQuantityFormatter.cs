// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Localization.Quantity.ComputingQuantityFormatter
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
  public class ComputingQuantityFormatter : QuantityFormatter
  {
    public static readonly ComputingQuantityFormatter Instance;
    private static readonly LocStr1 TF_UNIT;
    private static readonly LocStr1 PF_UNIT;
    public static readonly LocStr1 TF_UNIT_SHORT;
    public static readonly LocStr1 PF_UNIT_SHORT;

    private ComputingQuantityFormatter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public override FormatInfo GetFormatInfo(IProtoWithIconAndName proto, QuantityLarge quantity)
    {
      return ComputingQuantityFormatter.GetFormatInfo(quantity);
    }

    public static FormatInfo GetFormatInfo(QuantityLarge quantity)
    {
      return quantity.Value < 1000L ? new FormatInfo(LocStr1Plural.Empty, new LocStr1?(ComputingQuantityFormatter.TF_UNIT), 1) : new FormatInfo(LocStr1Plural.Empty, new LocStr1?(ComputingQuantityFormatter.PF_UNIT), 1000);
    }

    public static FormatInfo GetFormatInfoShort(QuantityLarge quantity)
    {
      return quantity.Value < 1000L ? new FormatInfo(LocStr1Plural.Empty, new LocStr1?(ComputingQuantityFormatter.TF_UNIT_SHORT), 1) : new FormatInfo(LocStr1Plural.Empty, new LocStr1?(ComputingQuantityFormatter.PF_UNIT_SHORT), 1000);
    }

    public LocStrFormatted FormatNumberAndUnitOnly(QuantityLarge quantity)
    {
      return this.FormatNumberAndUnitOnly(quantity, ComputingQuantityFormatter.GetFormatInfo(quantity));
    }

    static ComputingQuantityFormatter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ComputingQuantityFormatter.Instance = new ComputingQuantityFormatter();
      ComputingQuantityFormatter.TF_UNIT = Loc.Str1("Computing__TFlop", "{0} TFlops", "Computing power in terra-flops");
      ComputingQuantityFormatter.PF_UNIT = Loc.Str1("Computing__PFlop", "{0} PFlops", "Computing power in peta-flops");
      ComputingQuantityFormatter.TF_UNIT_SHORT = Loc.Str1("Computing__TFlop_short", "{0} TF", "super short suffix used for TFlops (terra flops), example use: '24 TF', please keep short like this");
      ComputingQuantityFormatter.PF_UNIT_SHORT = Loc.Str1("Computing__PFlop_short", "{0} PF", "super short suffix used for PFlops (peta flops), example use: '24 PF', please keep short like this");
    }
  }
}
