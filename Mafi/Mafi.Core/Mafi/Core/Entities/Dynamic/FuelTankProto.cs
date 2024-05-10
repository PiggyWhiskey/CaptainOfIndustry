// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.FuelTankProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  /// <summary>
  /// Proto specifying a fuel tank. Does not inherit <see cref="T:Mafi.Core.Prototypes.Proto" /> as fuel tank it is not a separate
  /// game entity, it only works as part of <see cref="T:Mafi.Core.Entities.Dynamic.DrivingEntity" />.
  /// </summary>
  public class FuelTankProto : Proto
  {
    /// <summary>Type of product accepted by fuel tank.</summary>
    public readonly ProductProto Product;
    /// <summary>Waste produced by using fuel.</summary>
    public readonly ProductProto WasteProduct;
    /// <summary>Percent of fuel that ends up as pollution.</summary>
    public readonly Percent PollutionPercent;
    /// <summary>How much of this product can the fuel tank accept.</summary>
    public readonly Quantity Capacity;
    /// <summary>How long will a vehicle run on a full fuel tank.</summary>
    public readonly Duration Duration;
    /// <summary>How long a vehicle can operate on a reserve.</summary>
    public readonly Duration ReserveDuration;
    /// <summary>
    /// How many percent of full (normal) fuel consumption the vehicle consumes while idle.
    /// </summary>
    public readonly Percent IdleFuelConsumption;

    public FuelTankProto(
      Proto.ID id,
      ProductProto product,
      ProductProto wasteProduct,
      Percent pollutionPercent,
      Quantity capacity,
      Duration duration,
      Duration reserveDuration,
      Percent idleFuelConsumption)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.Product = product;
      this.WasteProduct = wasteProduct;
      this.PollutionPercent = pollutionPercent.CheckNotNegative();
      this.Capacity = capacity.CheckPositive();
      this.Duration = duration.CheckPositive();
      this.ReserveDuration = reserveDuration;
      this.IdleFuelConsumption = idleFuelConsumption.CheckWithinIncl(Percent.Zero, Percent.Hundred);
      if (reserveDuration >= duration)
        throw new ProtoBuilderException("Reserve duration has to be less than fuel duration." + string.Format("Product: {0}, duration: {1}, reserve: {2}", (object) product, (object) duration, (object) reserveDuration));
    }

    public Duration QuantityToDuration(Quantity q)
    {
      return q.Value * this.Duration.Ticks.Ticks() / this.Capacity.Value;
    }

    /// <remarks>Rounds up if the conversion is not exact.</remarks>
    public Quantity DurationToQuantity(Duration d)
    {
      int num = d.Ticks * this.Capacity.Value;
      Quantity quantity = new Quantity(num / this.Duration.Ticks);
      if (num % this.Duration.Ticks != 0)
        quantity += Quantity.One;
      return quantity;
    }
  }
}
