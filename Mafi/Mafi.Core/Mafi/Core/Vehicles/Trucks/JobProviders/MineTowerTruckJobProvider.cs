// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.JobProviders.MineTowerTruckJobProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks.JobProviders
{
  /// <summary>
  /// Truck job provider providing jobs to trucks assigned to a <see cref="T:Mafi.Core.Buildings.Mine.MineTower" />.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class MineTowerTruckJobProvider : TruckJobProviderBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly MineTower m_mineTower;
    private readonly AssignedVehicles<Excavator, ExcavatorProto> m_excavators;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private LystStruct<MineTower> m_towersCache;
    private readonly VehicleQueueJobFactory m_queueJobFactory;
    [DoNotSave(0, null)]
    private Predicate<TerrainDesignation> m_dumpAnywherePredicate;
    [DoNotSave(0, null)]
    private Predicate<TerrainDesignation> m_dumpInAssignedTowersPredicate;
    [DoNotSave(0, null)]
    private Dict<ProductProto.ID, Option<RegisteredInputBuffer>> m_buffersForProduct;

    public static void Serialize(MineTowerTruckJobProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MineTowerTruckJobProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MineTowerTruckJobProvider.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AssignedVehicles<Excavator, ExcavatorProto>.Serialize(this.m_excavators, writer);
      MineTower.Serialize(this.m_mineTower, writer);
    }

    public static MineTowerTruckJobProvider Deserialize(BlobReader reader)
    {
      MineTowerTruckJobProvider truckJobProvider;
      if (reader.TryStartClassDeserialization<MineTowerTruckJobProvider>(out truckJobProvider))
        reader.EnqueueDataDeserialization((object) truckJobProvider, MineTowerTruckJobProvider.s_deserializeDataDelayedAction);
      return truckJobProvider;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MineTowerTruckJobProvider>(this, "m_excavators", (object) AssignedVehicles<Excavator, ExcavatorProto>.Deserialize(reader));
      reader.SetField<MineTowerTruckJobProvider>(this, "m_mineTower", (object) MineTower.Deserialize(reader));
      reader.RegisterResolvedMember<MineTowerTruckJobProvider>(this, "m_queueJobFactory", typeof (VehicleQueueJobFactory), true);
      this.m_towersCache = new LystStruct<MineTower>();
      reader.RegisterInitAfterLoad<MineTowerTruckJobProvider>(this, "initialize", InitPriority.Normal);
    }

    public MineTowerTruckJobProvider(
      MineTower mineTower,
      AssignedVehicles<Excavator, ExcavatorProto> excavators,
      TruckJobProviderContext context,
      VehicleQueueJobFactory queueJobFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(context);
      this.m_mineTower = mineTower;
      this.m_excavators = excavators;
      this.m_queueJobFactory = queueJobFactory;
      this.initialize();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initialize()
    {
      this.m_buffersForProduct = new Dict<ProductProto.ID, Option<RegisteredInputBuffer>>();
      this.m_dumpAnywherePredicate = (Predicate<TerrainDesignation>) (d =>
      {
        if (d.ManagedByTowers.IsEmpty())
          return true;
        foreach (IAreaManagingTower managedByTower in d.ManagedByTowers)
        {
          if (managedByTower is MineTower mineTower2 && (mineTower2 == this.m_mineTower || mineTower2.AllowNonAssignedOutput || !mineTower2.HasOutputStorageOrTowerAssigned || mineTower2.AssignedOutputTowers.Contains<MineTower>(this.m_mineTower)))
            return true;
        }
        return false;
      });
      this.m_dumpInAssignedTowersPredicate = (Predicate<TerrainDesignation>) (d => this.m_towersCache.Any((Predicate<MineTower>) (t => t.ManagedDesignations.Contains(d))));
    }

    public void OnExcavatorAssigned(Excavator excavator)
    {
      excavator.QueueEnabledChanged.Add<MineTowerTruckJobProvider>(this, new Action<TruckQueue>(this.queueEnableChanged));
    }

    public void OnExcavatorUnassigned(Excavator excavator)
    {
      excavator.QueueEnabledChanged.Remove<MineTowerTruckJobProvider>(this, new Action<TruckQueue>(this.queueEnableChanged));
    }

    private void queueEnableChanged(TruckQueue queue)
    {
      if (!queue.IsEnabled)
        return;
      this.balanceTruckExcavatorQueues();
    }

    /// <summary>
    /// Simple balancing of trucks in excavator queues. Computes the number of trucks that should be in queues to
    /// have balance queue sizes. Then releases trucks that are over this limit from their queues. The trucks will
    /// then go for the queue with lowest number of trucks.
    /// </summary>
    private void balanceTruckExcavatorQueues()
    {
      int num1 = 0;
      int num2 = 0;
      foreach (Excavator excavator in this.m_excavators.All)
      {
        TruckQueue truckQueue = excavator.TruckQueue;
        if (truckQueue.IsEnabled)
        {
          num1 += truckQueue.TrucksCount;
          ++num2;
        }
      }
      if (num2 == 0)
        return;
      int truckCountLimit = 1.Max(num1 / num2);
      foreach (Excavator excavator in this.m_excavators.All)
      {
        TruckQueue truckQueue = excavator.TruckQueue;
        if (truckQueue.IsEnabled)
          truckQueue.ReleaseVehiclesOverLimit(truckCountLimit);
      }
    }

    public override bool TryGetJobFor(Truck truck)
    {
      if (truck.HasJobs)
      {
        Assert.Fail("The truck already has a job assigned!");
        return false;
      }
      bool hasJob;
      if (this.TryGetVehicleRefuelingJob((Vehicle) truck, out hasJob))
        return hasJob;
      if (truck.Cargo.IsNotEmpty)
      {
        if (truck.Cargo.FirstOrPhantom.Product.Type == LooseProductProto.ProductType)
        {
          if (truck.DumpingOfAllCargoPending && this.tryGetExcavatorDeliveryJob(truck))
          {
            truck.DeactivateCannotDeliver();
            return true;
          }
          if (this.tryGetExcavatorDeliveryJob(truck))
          {
            truck.DeactivateCannotDeliver();
            return true;
          }
        }
        else
        {
          bool deliveryIsGuaranteed;
          if (this.TryGetRidOfCargo(truck, true, out deliveryIsGuaranteed))
          {
            if (deliveryIsGuaranteed)
              truck.DeactivateCannotDeliver();
            return true;
          }
        }
        truck.NotifyIffCannotDeliver(true, truck.Cargo.FirstOrPhantom.Product);
        return this.ParkAndWaitJobFactory.TryEnqueueParkingJobIfNeeded((Vehicle) truck, (ILayoutEntity) this.m_mineTower);
      }
      truck.DumpingOfAllCargoPending = false;
      truck.DeactivateCannotDeliver();
      return this.tryGetQueuedAtExcavatorJob(truck, 100.Tiles()) || this.ParkAndWaitJobFactory.TryEnqueueParkingJobIfNeeded((Vehicle) truck, (ILayoutEntity) this.m_mineTower);
    }

    /// <summary>
    /// Tries to get the truck queued at an Excavator that is currently working (it has its TruckQueue enabled). It
    /// searches among Excavators assigned to our MiningTower and looks for the Excavator with the shortest queue. If
    /// more queues have the same length, it chooses the closest Excavator with the shortest queue.
    /// </summary>
    private bool tryGetQueuedAtExcavatorJob(Truck truck, RelTile1i maxDistance)
    {
      Excavator excavator1 = (Excavator) null;
      Fix64 fix64_1 = (Fix64) 0L;
      int num = 0;
      bool flag1 = false;
      IReadOnlySet<IEntity> unreachableEntitiesFor = this.Context.UnreachablesManager.GetUnreachableEntitiesFor((IPathFindingVehicle) truck);
      foreach (Excavator excavator2 in this.m_excavators.All)
      {
        if (excavator2.TruckQueue.IsEnabled && excavator2.Prototype.IsTruckSupported(truck.Prototype) && !unreachableEntitiesFor.Contains((IEntity) excavator2))
        {
          bool flag2 = truck.IsEmpty;
          if (!flag2)
          {
            foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in excavator2.Cargo)
            {
              if (truck.Cargo.CanAdd(keyValuePair.Key))
              {
                flag2 = true;
                break;
              }
            }
          }
          if (flag2)
          {
            Fix64 fix64_2 = truck.Position2f.DistanceSqrTo(excavator2.Position2f);
            int trucksCount = excavator2.TruckQueue.TrucksCount;
            flag1 |= excavator2.TruckQueue.WaitingTrucksCount == 0;
            if (excavator1 == null || trucksCount < num || trucksCount == num && fix64_2 < fix64_1)
            {
              excavator1 = excavator2;
              fix64_1 = fix64_2;
              num = trucksCount;
            }
          }
        }
      }
      if (excavator1 == null || truck.Cargo.IsNotEmpty && (((excavator1.TruckQueue.TrucksCount >= 2 ? 1 : (excavator1.TruckQueue.WaitingTrucksCount >= 1 ? 1 : 0)) & (flag1 ? 1 : 0)) != 0 || fix64_1 > maxDistance.Squared))
        return false;
      truck.EnqueueJob((VehicleJob) this.m_queueJobFactory.CreateJobForVehicleOwnedQueue<Vehicle, Truck>(truck, (VehicleQueue<Truck, Vehicle>) excavator1.TruckQueue), false);
      return true;
    }

    private bool tryGetExcavatorDeliveryJob(Truck truck)
    {
      if (this.Context.OreSortingPlantsManager.IsSortingRequiredFor(truck, true))
      {
        bool hadEligibleAssignedEntity = false;
        if (truck.Cargo.Count == 1 && this.m_mineTower.AssignedInputStorages.IsNotEmpty<IEntityAssignedAsInput>())
        {
          ProductQuantity firstOrPhantom = truck.Cargo.FirstOrPhantom;
          Option<RegisteredInputBuffer> productInputForVehicle = this.VehicleBuffersRegistry.TryGetProductInputForVehicle((Vehicle) truck, firstOrPhantom, this.m_mineTower.AssignedInputStorages.SomeOption<IReadOnlySet<IEntityAssignedAsInput>>(), out hadEligibleAssignedEntity);
          if (productInputForVehicle.HasValue)
          {
            this.DeliveryJobFactory.EnqueueJob(truck, firstOrPhantom, productInputForVehicle.Value);
            return true;
          }
        }
        if (tryDumpAllCargoInAssignedTowersOrSelf())
        {
          truck.DeactivateCannotDeliver();
          return true;
        }
        bool hasMatchingPlant;
        bool isAssignedToPlant;
        if (this.Context.OreSortingPlantsManager.TryGetMixedDeliveryJobFor(truck, (Option<MineTower>) this.m_mineTower, out hasMatchingPlant, out isAssignedToPlant))
        {
          truck.DeactivateCannotDeliver();
          return true;
        }
        bool flag = isAssignedToPlant & hasMatchingPlant;
        if (truck.Cargo.Count > 1 && !flag && this.DumpJobFactory.TryCreateAndEnqueueJob(truck, (Option<ProductProto>) Option.None, predicate: this.m_dumpAnywherePredicate))
        {
          truck.DeactivateCannotDeliver();
          return true;
        }
        if (truck.Cargo.Count > 1 | flag | hadEligibleAssignedEntity)
        {
          if (truck.Cargo.Count > 1)
            truck.NotifyIffCannotDeliverMixed(!hasMatchingPlant);
          return false;
        }
      }
      this.m_buffersForProduct.Clear();
      bool hadEligibleAssignedEntity1 = false;
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        ProductQuantity productQuantity = new ProductQuantity(keyValuePair.Key, keyValuePair.Value);
        Option<RegisteredInputBuffer> productInputForVehicle = this.VehicleBuffersRegistry.TryGetProductInputForVehicle((Vehicle) truck, productQuantity, this.m_mineTower.AssignedInputStorages.SomeOption<IReadOnlySet<IEntityAssignedAsInput>>(), out hadEligibleAssignedEntity1);
        this.m_buffersForProduct.Add(productQuantity.Product.Id, productInputForVehicle);
      }
      if (this.m_mineTower.HasInputStorageOrTowerAssigned)
      {
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
        {
          ProductQuantity toDeliver = new ProductQuantity(keyValuePair.Key, keyValuePair.Value);
          Option<RegisteredInputBuffer> bufferMaybe = this.m_buffersForProduct[toDeliver.Product.Id];
          if (bufferMaybe.HasValue && this.m_mineTower.AssignedInputStorages.Any<IEntityAssignedAsInput>((Func<IEntityAssignedAsInput, bool>) (x => x == bufferMaybe.Value.Entity)))
          {
            this.DeliveryJobFactory.EnqueueJob(truck, toDeliver, bufferMaybe.Value);
            return true;
          }
        }
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
        {
          if (tryGetDumpingInAssignedTowers(keyValuePair.Key))
            return true;
        }
        if (truck.Cargo.Count == 1 & hadEligibleAssignedEntity1)
          return false;
      }
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        ProductQuantity toDeliver = new ProductQuantity(keyValuePair.Key, keyValuePair.Value);
        Option<RegisteredInputBuffer> option = this.m_buffersForProduct[toDeliver.Product.Id];
        if (option.HasValue)
        {
          this.DeliveryJobFactory.EnqueueJob(truck, toDeliver, option.Value);
          return true;
        }
      }
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        if (this.DumpJobFactory.TryCreateAndEnqueueJob(truck, (Option<ProductProto>) keyValuePair.Key, predicate: this.m_dumpAnywherePredicate))
          return true;
      }
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        if (this.SurfaceJobFactory.TryCreateAndEnqueuePlacementJob(keyValuePair.Key, truck))
          return true;
      }
      return false;

      bool tryGetDumpingInAssignedTowers(ProductProto productProto)
      {
        if (productProto.DumpableProduct.IsNone)
          return false;
        LooseProductProto product = productProto.DumpableProduct.Value;
        this.m_towersCache.Clear();
        if (this.m_mineTower.CanAcceptDumpOf((ProductProto) product) && this.m_mineTower.ManagedDesignations.Count > 0)
          this.m_towersCache.Add(this.m_mineTower);
        foreach (MineTower assignedInputTower in this.m_mineTower.AssignedInputTowers)
        {
          if (assignedInputTower.CanAcceptDumpOf((ProductProto) product) && assignedInputTower.ManagedDesignations.Count > 0)
            this.m_towersCache.Add(assignedInputTower);
        }
        if (this.m_towersCache.IsEmpty)
          return false;
        bool andEnqueueJob = this.DumpJobFactory.TryCreateAndEnqueueJob(truck, (Option<ProductProto>) productProto, predicate: this.m_dumpInAssignedTowersPredicate);
        this.m_towersCache.Clear();
        return andEnqueueJob;
      }

      bool tryDumpAllCargoInAssignedTowersOrSelf()
      {
        this.m_towersCache.Clear();
        if (canTowerAcceptDump(this.m_mineTower))
          this.m_towersCache.Add(this.m_mineTower);
        foreach (MineTower assignedInputTower in this.m_mineTower.AssignedInputTowers)
        {
          if (canTowerAcceptDump(assignedInputTower))
            this.m_towersCache.Add(assignedInputTower);
        }
        if (this.m_towersCache.IsEmpty)
          return false;
        bool andEnqueueJob = this.DumpJobFactory.TryCreateAndEnqueueJob(truck, (Option<ProductProto>) Option.None, predicate: this.m_dumpInAssignedTowersPredicate);
        this.m_towersCache.Clear();
        return andEnqueueJob;
      }

      bool canTowerAcceptDump(MineTower tower)
      {
        if (tower.ManagedDesignations.Count <= 0)
          return false;
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
        {
          if (!tower.CanAcceptDumpOf(keyValuePair.Key))
            return false;
        }
        return true;
      }
    }

    static MineTowerTruckJobProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MineTowerTruckJobProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TruckJobProviderBase) obj).SerializeData(writer));
      MineTowerTruckJobProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TruckJobProviderBase) obj).DeserializeData(reader));
    }
  }
}
