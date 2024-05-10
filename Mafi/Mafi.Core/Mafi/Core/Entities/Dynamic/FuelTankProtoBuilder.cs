// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.FuelTankProtoBuilder
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
  public class FuelTankProtoBuilder
  {
    private readonly ProtosDb m_protosDb;

    public FuelTankProtoBuilder(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
    }

    public FuelTankProtoBuilder.State Start(Proto.ID vehicleId)
    {
      return new FuelTankProtoBuilder.State(this, vehicleId);
    }

    public class State
    {
      /// <summary>
      /// How long a full fuel tank lasts after it gets down to reserve when used to fuel vehicle every tick.
      /// </summary>
      private Duration m_reserveDuration;
      private ProductProto m_product;
      private Quantity m_quantity;
      private Duration m_duration;
      private Percent m_idleFuelConsumption;
      private ProductProto m_wasteProduct;
      private Percent m_wastePercent;
      private readonly FuelTankProtoBuilder m_protoBuilder;
      private readonly Proto.ID m_vehicleId;

      public State(FuelTankProtoBuilder protoBuilder, Proto.ID vehicleId)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_idleFuelConsumption = 20.Percent();
        this.m_wasteProduct = ProductProto.Phantom;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_protoBuilder = protoBuilder;
        this.m_vehicleId = vehicleId;
      }

      public FuelTankProtoBuilder.State SetReserve(Duration reserveDuration)
      {
        this.m_reserveDuration = reserveDuration;
        return this;
      }

      public FuelTankProtoBuilder.State SetIdleFuelConsumption(Percent percent)
      {
        this.m_idleFuelConsumption = percent;
        return this;
      }

      public FuelTankProtoBuilder.State SetProduct(
        Proto.ID productId,
        Quantity quantity,
        Duration duration)
      {
        return this.SetProduct(this.m_protoBuilder.m_protosDb.GetOrThrow<ProductProto>(productId), quantity, duration);
      }

      public FuelTankProtoBuilder.State SetWasteProduct(Proto.ID productId, Percent ratio)
      {
        return this.SetWasteProduct(this.m_protoBuilder.m_protosDb.GetOrThrow<ProductProto>(productId), ratio);
      }

      public FuelTankProtoBuilder.State SetProduct(
        ProductProto product,
        Quantity quantity,
        Duration duration)
      {
        this.m_product = product;
        this.m_quantity = quantity;
        this.m_duration = duration;
        return this;
      }

      public FuelTankProtoBuilder.State SetWasteProduct(ProductProto product, Percent ratio)
      {
        this.m_wasteProduct = product;
        this.m_wastePercent = ratio;
        return this;
      }

      public FuelTankProto BuildTank()
      {
        return new FuelTankProto(new Proto.ID("FuelTank_" + this.m_vehicleId.Value + "_" + this.m_product.Id.ToString()), this.m_product.CheckNotNullOrPhantom<ProductProto>(), this.m_wasteProduct.CheckNotNull<ProductProto>(), this.m_wastePercent, this.m_quantity.CheckPositive(), this.m_duration.CheckPositive(), this.m_reserveDuration, this.m_idleFuelConsumption);
      }
    }
  }
}
