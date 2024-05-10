// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Localization.Quantity.NoUnitsQuantityFormatter
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
  public class NoUnitsQuantityFormatter : QuantityFormatter
  {
    public static readonly NoUnitsQuantityFormatter Instance;

    private NoUnitsQuantityFormatter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public override FormatInfo GetFormatInfo(IProtoWithIconAndName proto, QuantityLarge quantity)
    {
      return new FormatInfo(LocStr1Plural.Empty, new LocStr1?(), 1);
    }

    static NoUnitsQuantityFormatter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NoUnitsQuantityFormatter.Instance = new NoUnitsQuantityFormatter();
    }
  }
}
