// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.VehicleDepots.VehicleDepot
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.VehicleDepots
{
  [GenerateSerializer(false, null, 0)]
  public class VehicleDepot : VehicleDepotBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    public VehicleDepot(
      EntityId id,
      VehicleDepotProto proto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      IVehiclesManager vehiclesManager,
      TerrainManager terrainManager,
      SpawnJob.Factory spawnJobFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IProductsManager productsManager,
      IInstaBuildManager instaBuildManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      UpointsManager upointsManager,
      EntitiesCreator entitiesCreator,
      EntitiesCloneConfigHelper cloneConfigHelper)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (VehicleDepotBaseProto) proto, transform, context, simLoopEvents, vehiclesManager, terrainManager, spawnJobFactory, vehicleBuffersRegistry, productsManager, instaBuildManager, upgraderFactory, upointsManager, entitiesCreator, cloneConfigHelper);
    }

    public static void Serialize(VehicleDepot value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleDepot>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleDepot.s_serializeDataDelayedAction);
    }

    public static VehicleDepot Deserialize(BlobReader reader)
    {
      VehicleDepot vehicleDepot;
      if (reader.TryStartClassDeserialization<VehicleDepot>(out vehicleDepot))
        reader.EnqueueDataDeserialization((object) vehicleDepot, VehicleDepot.s_deserializeDataDelayedAction);
      return vehicleDepot;
    }

    static VehicleDepot()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleDepot.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      VehicleDepot.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
