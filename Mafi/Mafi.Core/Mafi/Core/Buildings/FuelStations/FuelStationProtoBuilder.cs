// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.FuelStations.FuelStationProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.FuelStations
{
  public class FuelStationProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public FuelStationProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public FuelStationProtoBuilder.State Start(string name, StaticEntityProto.ID labId)
    {
      return new FuelStationProtoBuilder.State(this, labId, name);
    }

    public class State : LayoutEntityBuilderState<FuelStationProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_id;
      private ProductProto m_fuelProto;
      private Quantity? m_capacity;
      private Quantity? m_maxTransferQuantityPerVehicle;
      private int m_vehicleQueuesCount;
      private Option<FuelStationProto> m_nextTier;

      public State(FuelStationProtoBuilder builder, StaticEntityProto.ID id, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_vehicleQueuesCount = 1;
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, id, name);
        this.m_id = id;
      }

      [MustUseReturnValue]
      public FuelStationProtoBuilder.State SetFuelProto(Proto.ID fuelTypeId)
      {
        return this.SetFuelProto(this.Builder.ProtosDb.GetOrThrow<ProductProto>(fuelTypeId));
      }

      [MustUseReturnValue]
      public FuelStationProtoBuilder.State SetFuelProto(ProductProto fuelType)
      {
        this.m_fuelProto = fuelType.CheckNotNull<ProductProto>();
        return this;
      }

      [MustUseReturnValue]
      public FuelStationProtoBuilder.State SetCapacity(int capacity)
      {
        this.m_capacity = new Quantity?(new Quantity(capacity).CheckPositive());
        return this;
      }

      [MustUseReturnValue]
      public FuelStationProtoBuilder.State SetMaxTransferQuantityPerVehicle(
        int maxTransferQuantityPerVehicle)
      {
        this.m_maxTransferQuantityPerVehicle = new Quantity?(new Quantity(maxTransferQuantityPerVehicle).CheckPositive());
        return this;
      }

      [MustUseReturnValue]
      public FuelStationProtoBuilder.State SetVehicleQueuesCount(int queuesCount)
      {
        Assert.That<int>(queuesCount).IsPositive();
        this.m_vehicleQueuesCount = queuesCount;
        return this;
      }

      [MustUseReturnValue]
      public FuelStationProtoBuilder.State SetNextTier(FuelStationProto nextTier)
      {
        this.m_nextTier = (Option<FuelStationProto>) nextTier;
        return this;
      }

      public FuelStationProto BuildAndAdd()
      {
        return this.AddToDb<FuelStationProto>(new FuelStationProto(this.m_id, this.m_fuelProto, this.ValueOrThrow<Quantity>(this.m_capacity, "Capacity"), this.ValueOrThrow<Quantity>(this.m_maxTransferQuantityPerVehicle, "MaxTransferQuantityPerVehicle"), this.m_vehicleQueuesCount, this.Strings, this.LayoutOrThrow, this.m_nextTier, this.Costs, this.Graphics));
      }
    }
  }
}
