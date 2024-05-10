// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.JobProviders.FuelStationTruckJobProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks.JobProviders
{
  /// <summary>
  /// Truck job provider providing jobs to trucks assigned to a fuel station.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class FuelStationTruckJobProvider : TruckJobProviderBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly FuelStation m_fuelStation;
    private readonly Mafi.Core.Buildings.FuelStations.FuelStationsManager m_fuelStationsManager;

    public static void Serialize(FuelStationTruckJobProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FuelStationTruckJobProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FuelStationTruckJobProvider.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      FuelStation.Serialize(this.m_fuelStation, writer);
      Mafi.Core.Buildings.FuelStations.FuelStationsManager.Serialize(this.m_fuelStationsManager, writer);
    }

    public static FuelStationTruckJobProvider Deserialize(BlobReader reader)
    {
      FuelStationTruckJobProvider truckJobProvider;
      if (reader.TryStartClassDeserialization<FuelStationTruckJobProvider>(out truckJobProvider))
        reader.EnqueueDataDeserialization((object) truckJobProvider, FuelStationTruckJobProvider.s_deserializeDataDelayedAction);
      return truckJobProvider;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<FuelStationTruckJobProvider>(this, "m_fuelStation", (object) FuelStation.Deserialize(reader));
      reader.SetField<FuelStationTruckJobProvider>(this, "m_fuelStationsManager", (object) Mafi.Core.Buildings.FuelStations.FuelStationsManager.Deserialize(reader));
    }

    public FuelStationTruckJobProvider(
      FuelStation fuelStation,
      TruckJobProviderContext context,
      Mafi.Core.Buildings.FuelStations.FuelStationsManager fuelStationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(context);
      this.m_fuelStationsManager = fuelStationsManager;
      this.m_fuelStation = fuelStation.CheckNotNull<FuelStation>();
    }

    public override bool TryGetJobFor(Truck truck)
    {
      if (truck.HasJobs)
      {
        Assert.Fail("The truck already has a job assigned!");
        return false;
      }
      if (!truck.Cargo.CanAdd(this.m_fuelStation.FuelProto))
        return this.TryGetRidOfCargoAndUpdateCannotDeliverNotif(truck, true);
      truck.DeactivateCannotDeliver();
      if (!this.m_fuelStation.IsEnabled)
        return this.ParkAndWaitJobFactory.TryEnqueueParkingJobIfNeeded((Vehicle) truck, (ILayoutEntity) this.m_fuelStation);
      bool flag = this.ParkAndWaitJobFactory.IsConsideredParkedAt((Vehicle) truck, (ILayoutEntity) this.m_fuelStation);
      if (flag && this.m_fuelStation.TryRefuelTruck(truck))
      {
        this.WaitingJobFactory.EnqueueJob((Vehicle) truck, 10.Ticks());
        return true;
      }
      if (this.TryGetVehicleRefuelingJob((Vehicle) truck, out bool _) || this.FuelStationsManager.TryGetRefuelOtherVehicleJob(this.m_fuelStation, truck))
        return true;
      return !flag && this.ParkAndWaitJobFactory.TryEnqueueParkingJobIfNeeded((Vehicle) truck, (ILayoutEntity) this.m_fuelStation);
    }

    static FuelStationTruckJobProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FuelStationTruckJobProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TruckJobProviderBase) obj).SerializeData(writer));
      FuelStationTruckJobProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TruckJobProviderBase) obj).DeserializeData(reader));
    }
  }
}
