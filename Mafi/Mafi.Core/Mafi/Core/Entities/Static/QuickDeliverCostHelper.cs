// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.QuickDeliverCostHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public static class QuickDeliverCostHelper
  {
    private static readonly Fix32 A_MUL;
    private static readonly Fix32 B_SUB;
    private static readonly Fix32 TEN;
    private static readonly Upoints MIN_COST;

    internal static Upoints? QuantityToUnityCost(int quantity, Percent multiplier)
    {
      if (quantity <= 0)
        return new Upoints?();
      if (quantity > Fix32.MaxValue)
        quantity = Fix32.MaxIntValue.IntegerPart;
      return new Upoints?((((QuickDeliverCostHelper.A_MUL * quantity.Sqrt() - QuickDeliverCostHelper.B_SUB) * QuickDeliverCostHelper.TEN).ToIntRounded() / QuickDeliverCostHelper.TEN).Upoints().ScaledBy(multiplier).Max(QuickDeliverCostHelper.MIN_COST));
    }

    static QuickDeliverCostHelper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      QuickDeliverCostHelper.A_MUL = 0.47.ToFix32();
      QuickDeliverCostHelper.B_SUB = 0.8.ToFix32();
      QuickDeliverCostHelper.TEN = 10.ToFix32();
      QuickDeliverCostHelper.MIN_COST = 0.1.Upoints();
    }
  }
}
