// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Excavators.ExcavatorJobProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Excavators
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class ExcavatorJobProvider : IJobProvider<Excavator>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly ITerrainMiningManager m_miningManager;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly MiningJob.Factory m_miningJobFactory;
    private readonly ParkAndWaitJobFactory m_parkAndWaitJobFactory;
    private readonly CleanExcavatorJob.Factory m_cleanExcavatorJobFactory;
    private readonly IFuelStationsManager m_fuelStationsManager;

    public static void Serialize(ExcavatorJobProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ExcavatorJobProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ExcavatorJobProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IFuelStationsManager>(this.m_fuelStationsManager);
      writer.WriteGeneric<IVehiclesManager>(this.m_vehiclesManager);
    }

    public static ExcavatorJobProvider Deserialize(BlobReader reader)
    {
      ExcavatorJobProvider excavatorJobProvider;
      if (reader.TryStartClassDeserialization<ExcavatorJobProvider>(out excavatorJobProvider))
        reader.EnqueueDataDeserialization((object) excavatorJobProvider, ExcavatorJobProvider.s_deserializeDataDelayedAction);
      return excavatorJobProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.RegisterResolvedMember<ExcavatorJobProvider>(this, "m_cleanExcavatorJobFactory", typeof (CleanExcavatorJob.Factory), true);
      reader.SetField<ExcavatorJobProvider>(this, "m_fuelStationsManager", (object) reader.ReadGenericAs<IFuelStationsManager>());
      reader.RegisterResolvedMember<ExcavatorJobProvider>(this, "m_miningJobFactory", typeof (MiningJob.Factory), true);
      reader.RegisterResolvedMember<ExcavatorJobProvider>(this, "m_miningManager", typeof (ITerrainMiningManager), true);
      reader.RegisterResolvedMember<ExcavatorJobProvider>(this, "m_parkAndWaitJobFactory", typeof (ParkAndWaitJobFactory), true);
      reader.SetField<ExcavatorJobProvider>(this, "m_vehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
    }

    public ExcavatorJobProvider(
      ITerrainMiningManager miningManager,
      IVehiclesManager vehiclesManager,
      MiningJob.Factory miningJobFactory,
      ParkAndWaitJobFactory parkAndWaitJobFactory,
      CleanExcavatorJob.Factory cleanExcavatorJobFactory,
      IFuelStationsManager fuelStationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_miningManager = miningManager;
      this.m_vehiclesManager = vehiclesManager;
      this.m_miningJobFactory = miningJobFactory;
      this.m_parkAndWaitJobFactory = parkAndWaitJobFactory;
      this.m_cleanExcavatorJobFactory = cleanExcavatorJobFactory;
      this.m_fuelStationsManager = fuelStationsManager;
    }

    /// <summary>Gives the given spawned excavator a job.</summary>
    public bool TryGetJobFor(Excavator excavator)
    {
      Assert.That<bool>(excavator.IsSpawned).IsTrue("Not spawned excavator cannot get a job.");
      Assert.That<bool>(excavator.IsOnWayToDepotForScrap).IsFalse();
      Assert.That<bool>(excavator.IsOnWayToDepotForReplacement).IsFalse();
      if (excavator.HasTrueJob)
      {
        Log.Warning("Excavator already has a job assigned!");
        return false;
      }
      if (excavator.LastRefuelRequestIssue != RefuelRequestIssue.None)
      {
        if (this.m_fuelStationsManager.TryRequestTruckForRefueling((Vehicle) excavator))
        {
          excavator.LastRefuelRequestIssue = RefuelRequestIssue.None;
          return false;
        }
        if (this.tryEnqueueCleaningJobIfNotClean(excavator))
          return true;
        if (this.m_fuelStationsManager.TryRefuelSelf((Vehicle) excavator))
        {
          excavator.TruckQueue.Disable();
          return true;
        }
      }
      if (excavator.CannotWorkDueToLowFuel)
        return false;
      if (this.m_miningJobFactory.TryCreateAndEnqueueJob(excavator) || this.tryEnqueueCleaningJobIfNotClean(excavator))
        return true;
      return excavator.MineTower.HasValue && !excavator.MineTower.Value.Area.ContainsTile(excavator.GroundPositionTile2i) && this.m_parkAndWaitJobFactory.TryEnqueueParkingJobIfNeeded((Vehicle) excavator, (ILayoutEntity) excavator.MineTower.Value);
    }

    /// <summary>
    /// If the excavator is not "clean", enqueues a cleaning job. The cleaning job setups the excavator to a default
    /// state and releases trucks.
    /// </summary>
    private bool tryEnqueueCleaningJobIfNotClean(Excavator excavator)
    {
      if (CleanExcavatorJob.IsClean(excavator))
        return false;
      this.m_cleanExcavatorJobFactory.EnqueueJob(excavator);
      return true;
    }

    static ExcavatorJobProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ExcavatorJobProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ExcavatorJobProvider) obj).SerializeData(writer));
      ExcavatorJobProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ExcavatorJobProvider) obj).DeserializeData(reader));
    }
  }
}
