// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Ships.CargoShipFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Stats;
using Mafi.Core.World;
using Mafi.Core.World.Contracts;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Ships
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class CargoShipFactory : ICargoShipFactory
  {
    private readonly EntityId.Factory m_idFactory;
    private readonly EntitiesManager m_entitiesManager;
    private readonly EntityContext m_context;
    private readonly CargoDepotManager m_cargoDepotManager;
    private readonly WorldMapCargoManager m_worldMapCargoManager;
    private readonly ContractsManager m_contractsManager;
    private readonly IFuelStatsCollector m_fuelStatsCollector;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoShipFactory(
      EntityId.Factory idFactory,
      EntitiesManager entitiesManager,
      EntityContext context,
      CargoDepotManager cargoDepotManager,
      WorldMapCargoManager worldMapCargoManager,
      ContractsManager contractsManager,
      IFuelStatsCollector fuelStatsCollector)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_idFactory = idFactory;
      this.m_entitiesManager = entitiesManager;
      this.m_context = context;
      this.m_cargoDepotManager = cargoDepotManager;
      this.m_worldMapCargoManager = worldMapCargoManager;
      this.m_contractsManager = contractsManager;
      this.m_fuelStatsCollector = fuelStatsCollector;
    }

    public CargoShip AddCargoShip(
      CargoDepot cargoDepot,
      CargoShipProto proto,
      Option<ProductProto> fuelProto)
    {
      ProductProto fuelProto1 = fuelProto.ValueOrNull ?? proto.AvailableFuels.First.FuelProto;
      CargoShip cargoShip = new CargoShip(this.m_idFactory.GetNextId(), proto, fuelProto1, this.m_context, cargoDepot, this.m_cargoDepotManager, this.m_worldMapCargoManager, this.m_contractsManager, (ICargoShipFactory) this, this.m_fuelStatsCollector);
      this.m_entitiesManager.AddEntityNoChecks((IEntity) cargoShip);
      return cargoShip;
    }

    public static void Serialize(CargoShipFactory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoShipFactory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoShipFactory.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      CargoDepotManager.Serialize(this.m_cargoDepotManager, writer);
      ContractsManager.Serialize(this.m_contractsManager, writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<IFuelStatsCollector>(this.m_fuelStatsCollector);
      EntityId.Factory.Serialize(this.m_idFactory, writer);
      WorldMapCargoManager.Serialize(this.m_worldMapCargoManager, writer);
    }

    public static CargoShipFactory Deserialize(BlobReader reader)
    {
      CargoShipFactory cargoShipFactory;
      if (reader.TryStartClassDeserialization<CargoShipFactory>(out cargoShipFactory))
        reader.EnqueueDataDeserialization((object) cargoShipFactory, CargoShipFactory.s_deserializeDataDelayedAction);
      return cargoShipFactory;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<CargoShipFactory>(this, "m_cargoDepotManager", (object) CargoDepotManager.Deserialize(reader));
      reader.RegisterResolvedMember<CargoShipFactory>(this, "m_context", typeof (EntityContext), true);
      reader.SetField<CargoShipFactory>(this, "m_contractsManager", (object) ContractsManager.Deserialize(reader));
      reader.SetField<CargoShipFactory>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<CargoShipFactory>(this, "m_fuelStatsCollector", (object) reader.ReadGenericAs<IFuelStatsCollector>());
      reader.SetField<CargoShipFactory>(this, "m_idFactory", (object) EntityId.Factory.Deserialize(reader));
      reader.SetField<CargoShipFactory>(this, "m_worldMapCargoManager", (object) WorldMapCargoManager.Deserialize(reader));
    }

    static CargoShipFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoShipFactory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CargoShipFactory) obj).SerializeData(writer));
      CargoShipFactory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CargoShipFactory) obj).DeserializeData(reader));
    }
  }
}
