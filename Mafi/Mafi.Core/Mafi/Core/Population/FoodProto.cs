// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.FoodProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Population
{
  public class FoodProto : Proto
  {
    public readonly ProductProto Product;
    public readonly FoodCategoryProto FoodCategory;
    private readonly Upoints UpointsWhenProvided;
    private readonly Fix32 ConsumedPerHundredPopsPerMonth;

    public FoodProto(
      Proto.ID id,
      ProductProto product,
      FoodCategoryProto foodCategory,
      Fix32 consumedPerHundredPopsPerMonth,
      Upoints upointsWhenProvided)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.Product = product;
      this.FoodCategory = foodCategory;
      this.UpointsWhenProvided = upointsWhenProvided.CheckNotNegative();
      this.ConsumedPerHundredPopsPerMonth = consumedPerHundredPopsPerMonth.CheckPositive();
    }

    public int GetPopDaysFromQuantity(Quantity quantity, Percent consumptionMult)
    {
      Fix32 fix32 = this.ConsumedPerHundredPopsPerMonth.ScaledBy(consumptionMult);
      long longFloored = (quantity.Value.ToFix64() / fix32 * 3000).ToLongFloored();
      return longFloored < (long) int.MaxValue ? (int) longFloored : int.MaxValue;
    }

    public PartialQuantity GetConsumedQuantityFromPopDays(int popDays, Percent consumptionMult)
    {
      Fix32 fix32 = this.ConsumedPerHundredPopsPerMonth.ScaledBy(consumptionMult);
      return popDays >= 10000 ? new PartialQuantity(popDays / 3000 * fix32) : new PartialQuantity(popDays * fix32 / 3000);
    }

    public Upoints GetUnityProvided(Percent upointsMultiplier)
    {
      return this.UpointsWhenProvided.ScaledBy(upointsMultiplier);
    }
  }
}
