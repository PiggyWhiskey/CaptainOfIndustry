// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.TreeHarvesters.TreeHarvesterJobProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.TreeHarvesters
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TreeHarvesterJobProvider : IJobProvider<TreeHarvester>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly ITreesManager m_treeManager;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly IFuelStationsManager m_fuelStationsManager;
    private readonly TreeHarvestingJob.Factory m_treeHarvestingJobFactory;
    private readonly ParkAndWaitJobFactory m_parkAndWaitJobFactory;
    private readonly TreeHarvesterLoadTruckJob.Factory m_treeHarvesterLoadTruckJobFactory;

    public static void Serialize(TreeHarvesterJobProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreeHarvesterJobProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreeHarvesterJobProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IFuelStationsManager>(this.m_fuelStationsManager);
      writer.WriteGeneric<ITreesManager>(this.m_treeManager);
      writer.WriteGeneric<IVehiclesManager>(this.m_vehiclesManager);
    }

    public static TreeHarvesterJobProvider Deserialize(BlobReader reader)
    {
      TreeHarvesterJobProvider harvesterJobProvider;
      if (reader.TryStartClassDeserialization<TreeHarvesterJobProvider>(out harvesterJobProvider))
        reader.EnqueueDataDeserialization((object) harvesterJobProvider, TreeHarvesterJobProvider.s_deserializeDataDelayedAction);
      return harvesterJobProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TreeHarvesterJobProvider>(this, "m_fuelStationsManager", (object) reader.ReadGenericAs<IFuelStationsManager>());
      reader.RegisterResolvedMember<TreeHarvesterJobProvider>(this, "m_parkAndWaitJobFactory", typeof (ParkAndWaitJobFactory), true);
      reader.RegisterResolvedMember<TreeHarvesterJobProvider>(this, "m_treeHarvesterLoadTruckJobFactory", typeof (TreeHarvesterLoadTruckJob.Factory), true);
      reader.RegisterResolvedMember<TreeHarvesterJobProvider>(this, "m_treeHarvestingJobFactory", typeof (TreeHarvestingJob.Factory), true);
      reader.SetField<TreeHarvesterJobProvider>(this, "m_treeManager", (object) reader.ReadGenericAs<ITreesManager>());
      reader.SetField<TreeHarvesterJobProvider>(this, "m_vehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
    }

    public TreeHarvesterJobProvider(
      ITreesManager treeManager,
      IVehiclesManager vehiclesManager,
      IFuelStationsManager fuelStationsManager,
      TreeHarvestingJob.Factory treeHarvestingJobFactory,
      ParkAndWaitJobFactory parkAndWaitJobFactory,
      TreeHarvesterLoadTruckJob.Factory treeHarvesterLoadTruckJobFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treeManager = treeManager;
      this.m_vehiclesManager = vehiclesManager;
      this.m_fuelStationsManager = fuelStationsManager;
      this.m_treeHarvestingJobFactory = treeHarvestingJobFactory;
      this.m_parkAndWaitJobFactory = parkAndWaitJobFactory;
      this.m_treeHarvesterLoadTruckJobFactory = treeHarvesterLoadTruckJobFactory;
    }

    public bool TryGetJobFor(TreeHarvester harvester)
    {
      Assert.That<bool>(harvester.IsSpawned).IsTrue();
      Assert.That<bool>(harvester.IsOnWayToDepotForScrap).IsFalse();
      Assert.That<bool>(harvester.IsOnWayToDepotForReplacement).IsFalse();
      if (harvester.HasJobs)
      {
        Log.Warning("Tree harvester already has a job assigned!");
        return false;
      }
      if (this.tryGetTruckLoadingJob(harvester))
        return true;
      if (harvester.LastRefuelRequestIssue != RefuelRequestIssue.None)
      {
        if (this.m_fuelStationsManager.TryRequestTruckForRefueling((Vehicle) harvester))
        {
          harvester.LastRefuelRequestIssue = RefuelRequestIssue.None;
          return false;
        }
        if (this.m_fuelStationsManager.TryRefuelSelf((Vehicle) harvester))
        {
          harvester.TruckQueue.Disable();
          return true;
        }
      }
      if (harvester.CannotWorkDueToLowFuel)
      {
        harvester.TruckQueue.Disable();
        return false;
      }
      bool treeHarvestingJob = this.tryGetTreeHarvestingJob(harvester);
      if (!treeHarvestingJob)
        harvester.DidNotFindTreeToHarvest = true;
      if (treeHarvestingJob)
      {
        harvester.TruckQueue.Enable();
        return true;
      }
      return harvester.ForestryTower.HasValue && this.m_parkAndWaitJobFactory.TryEnqueueParkingJobIfNeeded((Vehicle) harvester, (ILayoutEntity) harvester.ForestryTower.Value);
    }

    /// <summary>
    /// Attempts to get a tree harvesting job for the harvester.
    /// </summary>
    private bool tryGetTreeHarvestingJob(TreeHarvester harvester)
    {
      Assert.That<ProductQuantity>(harvester.Cargo).IsEmpty();
      Option<ForestryTower> tower = Option<ForestryTower>.None;
      IReadOnlySet<TreeId> unreachableTreesFor = this.m_treeHarvestingJobFactory.UnreachablesManager.GetUnreachableTreesFor((IPathFindingVehicle) harvester);
      IReadOnlySet<Chunk2i> unreachableTreeChunksFor = this.m_treeHarvestingJobFactory.UnreachablesManager.GetUnreachableTreeChunksFor((IPathFindingVehicle) harvester);
      TreeId? treeForHarvestFor;
      if (harvester.ForestryTower.HasValue)
      {
        treeForHarvestFor = harvester.ForestryTower.Value.FindClosestTreeForHarvestFor((Vehicle) harvester, IdsCore.Products.Wood, unreachableTreesFor);
      }
      else
      {
        treeForHarvestFor = this.m_treeManager.FindClosestNonTowerTreeForHarvestFor((Vehicle) harvester, IdsCore.Products.Wood, unreachableTreesFor, unreachableTreeChunksFor);
        if (!treeForHarvestFor.HasValue)
          treeForHarvestFor = this.m_treeManager.FindClosestTowerTreeForHarvestFor((Vehicle) harvester, IdsCore.Products.Wood, unreachableTreesFor, out tower);
      }
      if (!treeForHarvestFor.HasValue)
        return false;
      this.m_treeHarvestingJobFactory.EnqueueJob(harvester, treeForHarvestFor.Value, tower);
      return true;
    }

    /// <summary>
    /// Creates a job unloading a cargo of the harvester onto a truck.
    /// </summary>
    private bool tryGetTruckLoadingJob(TreeHarvester harvester)
    {
      if (harvester.Cargo.IsEmpty)
        return false;
      this.m_treeHarvesterLoadTruckJobFactory.EnqueueJob(harvester);
      return true;
    }

    static TreeHarvesterJobProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreeHarvesterJobProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TreeHarvesterJobProvider) obj).SerializeData(writer));
      TreeHarvesterJobProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TreeHarvesterJobProvider) obj).DeserializeData(reader));
    }
  }
}
