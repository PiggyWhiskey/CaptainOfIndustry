// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Contracts.ContractProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.World.Contracts
{
  public class ContractProto : Proto
  {
    public readonly ProductProto ProductToBuy;
    private readonly Quantity QuantityToBuy;
    public readonly ProductProto ProductToPayWith;
    public readonly Quantity QuantityToPayWith;
    public readonly ImmutableArray<ProductProto> AllProducts;
    /// <summary>Unity to pay for active contract</summary>
    public readonly Upoints UpointsPerMonth;
    /// <summary>Unity to pay per 100 quantity traded</summary>
    public readonly Upoints UpointsPer100ProductsBought;
    /// <summary>Initial cost to establish this contract.</summary>
    public readonly Upoints UpointsToEstablish;
    public readonly int MinReputationRequired;

    public ContractProto(
      Proto.ID id,
      ProductQuantity productToBuy,
      ProductQuantity productToPayWith,
      Upoints upointsPerMonth,
      Upoints upointsPer100ProductsBought,
      int minReputationRequired)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.ProductToBuy = productToBuy.Product;
      this.QuantityToBuy = productToBuy.Quantity;
      this.ProductToPayWith = productToPayWith.Product;
      this.QuantityToPayWith = productToPayWith.Quantity;
      this.AllProducts = ImmutableArray.Create<ProductProto>(this.ProductToBuy, this.ProductToPayWith);
      this.UpointsPerMonth = upointsPerMonth.CheckNotNegative();
      this.UpointsPer100ProductsBought = upointsPer100ProductsBought.CheckNotNegative();
      this.UpointsToEstablish = (this.UpointsPerMonth.Value * 60).ToIntRounded().Upoints().Max(5.Upoints());
      this.MinReputationRequired = minReputationRequired.CheckNotNegative();
    }

    public Upoints CalculateUpointsForQuantityBought(Quantity bought)
    {
      return bought.IsNotPositive ? Upoints.Zero : (bought.Value * this.UpointsPer100ProductsBought.Value / 100.ToFix32()).Upoints();
    }

    public Quantity GetQuantityToBuy(Percent contractMultiplier)
    {
      return this.QuantityToBuy.ScaledBy(contractMultiplier);
    }
  }
}
