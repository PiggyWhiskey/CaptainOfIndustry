// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.JobProviders.TruckJobProviderBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks.JobProviders
{
  /// <summary>
  /// Base class for job providers, implements shared functionality.
  /// </summary>
  public abstract class TruckJobProviderBase : IJobProvider<Truck>
  {
    private static readonly RelTile1i PARKED_VEHICLE_MAX_DISTANCE;
    protected readonly TruckJobProviderContext Context;
    [ThreadStatic]
    private static Lyst<ProductProto> s_discardTmp;

    protected virtual void SerializeData(BlobWriter writer)
    {
      TruckJobProviderContext.Serialize(this.Context, writer);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TruckJobProviderBase>(this, "Context", (object) TruckJobProviderContext.Deserialize(reader));
    }

    public TruckJobProviderBase(TruckJobProviderContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Context = context;
    }

    protected VehicleJobStatsManager VehicleJobStatsManager => this.Context.VehicleJobStatsManager;

    protected IVehicleBuffersRegistry VehicleBuffersRegistry => this.Context.VehicleBuffersRegistry;

    protected IVehiclesManager VehiclesManager => this.Context.VehiclesManager;

    protected ITerrainDumpingManager TerrainDumpingManager => this.Context.TerrainDumpingManager;

    protected IFuelStationsManager FuelStationsManager => this.Context.FuelStationsManager;

    protected ITreesManager TreeManager => this.Context.TreeManager;

    protected CargoPickUpJob.Factory PickUpJobFactory => this.Context.PickUpJobFactory;

    protected CargoDeliveryJob.Factory DeliveryJobFactory => this.Context.DeliveryJobFactory;

    protected DumpingJob.Factory DumpJobFactory => this.Context.DumpJobFactory;

    protected SurfaceModificationJob.Factory SurfaceJobFactory => this.Context.SurfaceJobFactory;

    protected NavigateToJob.Factory NavigateToJobFactory => this.Context.NavigateToJobFactory;

    protected WaitingJob.Factory WaitingJobFactory => this.Context.WaitingJobFactory;

    protected ParkAndWaitJobFactory ParkAndWaitJobFactory => this.Context.ParkAndWaitJobFactory;

    protected TreeHarvestingJob.Factory TreeHarvestingJobFactory
    {
      get => this.Context.TreeHarvestingJobFactory;
    }

    public abstract bool TryGetJobFor(Truck truck);

    protected void GetWaitingJob(Vehicle vehicle, Duration duration)
    {
      this.WaitingJobFactory.EnqueueJob(vehicle, duration);
    }

    /// <summary>
    /// If the vehicle is running on a reserve, we try getting a refueling job.
    /// </summary>
    /// <param name="hasJob">if refueling job was assigned or not</param>
    /// <returns>true if the caller should not try to search for any other job.</returns>
    protected bool TryGetVehicleRefuelingJob(Vehicle vehicle, out bool hasJob)
    {
      hasJob = false;
      if (!vehicle.NeedsRefueling)
        return false;
      hasJob = this.FuelStationsManager.TryRefuelSelf(vehicle);
      return hasJob || vehicle.CannotWorkDueToLowFuel;
    }

    /// <summary>
    /// Tries to get rid of current cargo.
    /// 
    /// If the cargo is something we really need to get rid of mark it as <param name="isCargoUnexpected" />.
    /// This is anytime the cargo is in way. However don't use it for situations where it is expected such
    /// as mining truck as we don't want to fill shipyard with mined products.
    /// 
    /// Returns true if it found a job to remove cargo not if it succeeded removing cargo.
    /// </summary>
    public bool TryGetRidOfCargo(
      Truck truck,
      bool isCargoUnexpected,
      out bool deliveryIsGuaranteed)
    {
      deliveryIsGuaranteed = false;
      if (truck.Cargo.IsEmpty)
        return false;
      if (this.Context.OreSortingPlantsManager.IsSortingRequiredFor(truck, false))
      {
        bool hasMatchingPlant;
        if (this.Context.OreSortingPlantsManager.TryGetMixedDeliveryJobFor(truck, (Option<MineTower>) Option.None, out hasMatchingPlant, out bool _))
        {
          truck.DeactivateCannotDeliver();
          return true;
        }
        if (!hasMatchingPlant && this.DumpJobFactory.TryCreateAndEnqueueJob(truck, (Option<ProductProto>) Option.None))
        {
          truck.DeactivateCannotDeliver();
          return true;
        }
        truck.NotifyIffCannotDeliverMixed(!hasMatchingPlant);
        return false;
      }
      RegisteredOutputBuffer buffer;
      if (this.Context.VehicleLastOutputBufferManager.TryGetLastOutputBufferFor((Vehicle) truck, out buffer) && buffer.HasAssignedInputEntities)
      {
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
        {
          ProductQuantity productQuantity = new ProductQuantity(keyValuePair.Key, keyValuePair.Value);
          Option<RegisteredInputBuffer> productInputForVehicle = this.Context.VehicleBuffersRegistry.TryGetProductInputForVehicle((Vehicle) truck, productQuantity, buffer.EntityAsAssignee.Value.AssignedInputs.SomeOption<IReadOnlySet<IEntityAssignedAsInput>>(), out bool _, true);
          if (productInputForVehicle.HasValue)
          {
            this.DeliveryJobFactory.EnqueueJob(truck, productQuantity, productInputForVehicle.Value);
            deliveryIsGuaranteed = true;
            return true;
          }
        }
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
        {
          if (this.DumpJobFactory.TryCreateAndEnqueueJob(truck, keyValuePair.Key, buffer.EntityAsAssignee.Value.AssignedInputs))
            return true;
        }
        if (!isCargoUnexpected)
          return false;
      }
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        ProductQuantity productQuantity = new ProductQuantity(keyValuePair.Key, keyValuePair.Value);
        if (this.SurfaceJobFactory.TryCreateAndEnqueuePlacementJob(keyValuePair.Key, truck))
          return true;
        Option<RegisteredInputBuffer> productInputForVehicle = this.Context.VehicleBuffersRegistry.TryGetProductInputForVehicle((Vehicle) truck, productQuantity, (Option<IReadOnlySet<IEntityAssignedAsInput>>) Option.None, out bool _);
        if (productInputForVehicle.HasValue)
        {
          this.DeliveryJobFactory.EnqueueJob(truck, productQuantity, productInputForVehicle.Value);
          deliveryIsGuaranteed = true;
          return true;
        }
      }
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        if (this.DumpJobFactory.TryCreateAndEnqueueJob(truck, (Option<ProductProto>) keyValuePair.Key, isCargoUnexpected))
          return true;
      }
      if (TruckJobProviderBase.s_discardTmp == null)
        TruckJobProviderBase.s_discardTmp = new Lyst<ProductProto>();
      TruckJobProviderBase.s_discardTmp.Clear();
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        if (keyValuePair.Key.CanBeDiscarded)
          TruckJobProviderBase.s_discardTmp.Add(keyValuePair.Key);
      }
      foreach (ProductProto product in TruckJobProviderBase.s_discardTmp)
        truck.ClearCargoImmediately(product);
      if (truck.Cargo.IsEmpty || !isCargoUnexpected)
        return false;
      IReadOnlySet<IEntity> unreachableEntitiesFor = this.Context.UnreachablesManager.GetUnreachableEntitiesFor((IPathFindingVehicle) truck);
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        ProductQuantity toDeliver = new ProductQuantity(keyValuePair.Key, keyValuePair.Value);
        Option<RegisteredInputBuffer> fallbackInputForVehicle = this.Context.VehicleBuffersRegistry.TryGetFallbackInputForVehicle(toDeliver.Product);
        if (fallbackInputForVehicle.HasValue && !unreachableEntitiesFor.Contains((IEntity) fallbackInputForVehicle.Value.Entity))
        {
          this.DeliveryJobFactory.EnqueueJob(truck, toDeliver, fallbackInputForVehicle.Value);
          deliveryIsGuaranteed = true;
          return true;
        }
      }
      return false;
    }

    protected bool TryGetRidOfCargoAndUpdateCannotDeliverNotif(Truck truck, bool isCargoUnexpected)
    {
      if (truck.IsEmpty)
      {
        truck.DumpingOfAllCargoPending = false;
        truck.DeactivateCannotDeliver();
        return false;
      }
      bool deliveryIsGuaranteed;
      if (this.TryGetRidOfCargo(truck, isCargoUnexpected, out deliveryIsGuaranteed))
      {
        if (deliveryIsGuaranteed)
          truck.DeactivateCannotDeliver();
        return true;
      }
      truck.NotifyIffCannotDeliver(!truck.Cargo.IsEmpty, truck.Cargo.FirstOrPhantom.Product);
      return false;
    }

    /// <summary>
    /// If the vehicle is not parked close to the entity, enqueues a navigation job to given entity.
    /// </summary>
    protected bool TryGetNavigateToJob(Vehicle vehicle, ILayoutEntity layoutEntity, bool asTrueJob = true)
    {
      RelTile3i layoutSize = layoutEntity.Prototype.Layout.LayoutSize;
      long maxParkedVehicleDistanceSqr = (long) (layoutSize.X.Max(layoutSize.Y) + TruckJobProviderBase.PARKED_VEHICLE_MAX_DISTANCE.Value).Squared();
      return this.TryGetNavigateToJob(vehicle, layoutEntity, (Fix64) maxParkedVehicleDistanceSqr, asTrueJob);
    }

    protected bool TryGetNavigateToJob(
      Vehicle vehicle,
      ILayoutEntity layoutEntity,
      Fix64 maxParkedVehicleDistanceSqr,
      bool asTrueJob = true)
    {
      if (vehicle.Position2f.DistanceSqrTo(layoutEntity.Position2f) <= maxParkedVehicleDistanceSqr)
        return false;
      this.NavigateToJobFactory.EnqueueJob(vehicle, (IVehicleGoalFull) this.Context.StaticEntityGoalFactory.Create((IStaticEntity) layoutEntity), asTrueJob: asTrueJob);
      return true;
    }

    static TruckJobProviderBase()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TruckJobProviderBase.PARKED_VEHICLE_MAX_DISTANCE = new RelTile1i(5);
      TruckJobProviderBase.s_discardTmp = new Lyst<ProductProto>();
    }
  }
}
