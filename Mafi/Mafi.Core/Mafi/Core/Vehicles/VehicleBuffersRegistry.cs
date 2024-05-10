// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleBuffersRegistry
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class VehicleBuffersRegistry : IVehicleBuffersRegistry, IEntityObserver
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Fix64 MAX_SECONDARY_CARGO_DISTANCE_SQR;
    private readonly Dict<ProductProto, VehicleBuffersRegistry.RegisteredBuffers> m_registeredBuffersPerProduct;
    private readonly Dict<IStaticEntity, VehicleBuffersRegistry.RegisteredBuffersPerEntity> m_registeredBuffersPerEntity;
    [DoNotSave(0, null)]
    private Dict<IProductBuffer, RegisteredInputBuffer> m_registeredInputBuffers;
    [DoNotSave(0, null)]
    private Dict<IProductBuffer, RegisteredOutputBuffer> m_registeredOutputBuffers;
    private readonly LazyResolve<ITerrainDumpingManager> m_dumpingManager;
    [NewInSaveVersion(140, null, null, typeof (LazyResolve<ISurfaceDesignationsManager>), null)]
    private readonly LazyResolve<ISurfaceDesignationsManager> m_surfaceManager;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly LazyResolve<IVehiclePathFindingManager> m_vehiclePathFindingManager;
    [NewInSaveVersion(140, null, null, typeof (LazyResolve<IUnreachablesManager>), null)]
    private readonly LazyResolve<IUnreachablesManager> m_unreachablesManager;
    private SimStep m_lastBalancingStep;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<BalancingJobSpec> m_availableJobsTmp;
    /// <summary>
    /// We are using list because we sort via Comparison instead of Comparator. Array.sort causes
    /// allocations when used with Comparator. Comparison cannot be used for Lyst because Array.sort
    /// with Comparison arg does not accept a range selection (it would be touching nulls in Lyst). However
    /// I still see some allocations from GetHashCode (but not coming from sort) and I'm not sure. Maybe
    /// we might need to implement our own Sort. Or there is some other work causing besides Sort.
    /// 
    /// Some perf tests are in SortPerformanceTest.cs but results are mixed.
    /// 
    /// TODO: Get rid of allocations entirely.
    /// </summary>
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly List<VehicleBuffersRegistry.RegisteredBuffers> m_buffersTmp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private Set<VehicleBuffersRegistry.RegisteredBuffers> m_buffersTmpSet;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private Lyst<RegisteredInputBuffer> m_buffersCache;
    private readonly IRandom m_random;
    private readonly Lyst<Truck> m_trucksWaitingForJobs;
    private int m_jobsFoundDuringLastBalancing;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<VehicleBuffersRegistry.TruckData> m_trucksMatchedCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<VehicleBuffersRegistry.TruckData> m_allTrucksPerProductCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<TerrainDesignation> m_terrainDesignationsCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<SurfaceDesignation> m_surfaceDesignationsCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<ProductProto> m_productsToIgnoreCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<ProductProto> m_productsToRefreshCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<VehicleBuffersRegistry.TruckData> m_trucksToBalanceCache;

    public static void Serialize(VehicleBuffersRegistry value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleBuffersRegistry>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleBuffersRegistry.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.AllowPartialTrucks);
      LazyResolve<ITerrainDumpingManager>.Serialize(this.m_dumpingManager, writer);
      writer.WriteInt(this.m_jobsFoundDuringLastBalancing);
      SimStep.Serialize(this.m_lastBalancingStep, writer);
      writer.WriteGeneric<IRandom>(this.m_random);
      Dict<IStaticEntity, VehicleBuffersRegistry.RegisteredBuffersPerEntity>.Serialize(this.m_registeredBuffersPerEntity, writer);
      Dict<ProductProto, VehicleBuffersRegistry.RegisteredBuffers>.Serialize(this.m_registeredBuffersPerProduct, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      LazyResolve<ISurfaceDesignationsManager>.Serialize(this.m_surfaceManager, writer);
      Lyst<Truck>.Serialize(this.m_trucksWaitingForJobs, writer);
      LazyResolve<IUnreachablesManager>.Serialize(this.m_unreachablesManager, writer);
      LazyResolve<IVehiclePathFindingManager>.Serialize(this.m_vehiclePathFindingManager, writer);
    }

    public static VehicleBuffersRegistry Deserialize(BlobReader reader)
    {
      VehicleBuffersRegistry vehicleBuffersRegistry;
      if (reader.TryStartClassDeserialization<VehicleBuffersRegistry>(out vehicleBuffersRegistry))
        reader.EnqueueDataDeserialization((object) vehicleBuffersRegistry, VehicleBuffersRegistry.s_deserializeDataDelayedAction);
      return vehicleBuffersRegistry;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AllowPartialTrucks = reader.ReadBool();
      reader.SetField<VehicleBuffersRegistry>(this, "m_allTrucksPerProductCache", (object) new Lyst<VehicleBuffersRegistry.TruckData>());
      reader.SetField<VehicleBuffersRegistry>(this, "m_availableJobsTmp", (object) new Lyst<BalancingJobSpec>());
      this.m_buffersCache = new Lyst<RegisteredInputBuffer>();
      reader.SetField<VehicleBuffersRegistry>(this, "m_buffersTmp", (object) new List<VehicleBuffersRegistry.RegisteredBuffers>());
      this.m_buffersTmpSet = new Set<VehicleBuffersRegistry.RegisteredBuffers>();
      reader.SetField<VehicleBuffersRegistry>(this, "m_dumpingManager", (object) LazyResolve<ITerrainDumpingManager>.Deserialize(reader));
      this.m_jobsFoundDuringLastBalancing = reader.ReadInt();
      this.m_lastBalancingStep = SimStep.Deserialize(reader);
      reader.SetField<VehicleBuffersRegistry>(this, "m_productsToIgnoreCache", (object) new Set<ProductProto>());
      reader.SetField<VehicleBuffersRegistry>(this, "m_productsToRefreshCache", (object) new Set<ProductProto>());
      reader.SetField<VehicleBuffersRegistry>(this, "m_random", (object) reader.ReadGenericAs<IRandom>());
      reader.SetField<VehicleBuffersRegistry>(this, "m_registeredBuffersPerEntity", (object) Dict<IStaticEntity, VehicleBuffersRegistry.RegisteredBuffersPerEntity>.Deserialize(reader));
      reader.SetField<VehicleBuffersRegistry>(this, "m_registeredBuffersPerProduct", (object) Dict<ProductProto, VehicleBuffersRegistry.RegisteredBuffers>.Deserialize(reader));
      reader.SetField<VehicleBuffersRegistry>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<VehicleBuffersRegistry>(this, "m_surfaceDesignationsCache", (object) new Lyst<SurfaceDesignation>());
      reader.SetField<VehicleBuffersRegistry>(this, "m_surfaceManager", reader.LoadedSaveVersion >= 140 ? (object) LazyResolve<ISurfaceDesignationsManager>.Deserialize(reader) : (object) (LazyResolve<ISurfaceDesignationsManager>) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<VehicleBuffersRegistry>(this, "m_surfaceManager", typeof (LazyResolve<ISurfaceDesignationsManager>), true);
      reader.SetField<VehicleBuffersRegistry>(this, "m_terrainDesignationsCache", (object) new Lyst<TerrainDesignation>());
      reader.SetField<VehicleBuffersRegistry>(this, "m_trucksMatchedCache", (object) new Lyst<VehicleBuffersRegistry.TruckData>());
      reader.SetField<VehicleBuffersRegistry>(this, "m_trucksToBalanceCache", (object) new Lyst<VehicleBuffersRegistry.TruckData>());
      reader.SetField<VehicleBuffersRegistry>(this, "m_trucksWaitingForJobs", (object) Lyst<Truck>.Deserialize(reader));
      reader.SetField<VehicleBuffersRegistry>(this, "m_unreachablesManager", reader.LoadedSaveVersion >= 140 ? (object) LazyResolve<IUnreachablesManager>.Deserialize(reader) : (object) (LazyResolve<IUnreachablesManager>) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<VehicleBuffersRegistry>(this, "m_unreachablesManager", typeof (LazyResolve<IUnreachablesManager>), true);
      reader.SetField<VehicleBuffersRegistry>(this, "m_vehiclePathFindingManager", (object) LazyResolve<IVehiclePathFindingManager>.Deserialize(reader));
      reader.RegisterInitAfterLoad<VehicleBuffersRegistry>(this, "onLoaded", InitPriority.Normal);
    }

    public bool AllowPartialTrucks { get; set; }

    public int NumberOfTrucksWaitingForJobs => this.m_trucksWaitingForJobs.Count;

    public VehicleBuffersRegistry(
      RandomProvider randomProvider,
      LazyResolve<ITerrainDumpingManager> dumpingManager,
      LazyResolve<ISurfaceDesignationsManager> surfaceManager,
      ISimLoopEvents simLoopEvents,
      LazyResolve<IVehiclePathFindingManager> vehiclePathFindingManager,
      LazyResolve<IUnreachablesManager> unreachablesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_registeredBuffersPerProduct = new Dict<ProductProto, VehicleBuffersRegistry.RegisteredBuffers>();
      this.m_registeredBuffersPerEntity = new Dict<IStaticEntity, VehicleBuffersRegistry.RegisteredBuffersPerEntity>();
      this.m_registeredInputBuffers = new Dict<IProductBuffer, RegisteredInputBuffer>();
      this.m_registeredOutputBuffers = new Dict<IProductBuffer, RegisteredOutputBuffer>();
      this.m_availableJobsTmp = new Lyst<BalancingJobSpec>();
      this.m_buffersTmp = new List<VehicleBuffersRegistry.RegisteredBuffers>();
      this.m_buffersTmpSet = new Set<VehicleBuffersRegistry.RegisteredBuffers>();
      this.m_buffersCache = new Lyst<RegisteredInputBuffer>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CAllowPartialTrucks\u003Ek__BackingField = true;
      this.m_trucksWaitingForJobs = new Lyst<Truck>();
      this.m_trucksMatchedCache = new Lyst<VehicleBuffersRegistry.TruckData>();
      this.m_allTrucksPerProductCache = new Lyst<VehicleBuffersRegistry.TruckData>();
      this.m_terrainDesignationsCache = new Lyst<TerrainDesignation>();
      this.m_surfaceDesignationsCache = new Lyst<SurfaceDesignation>();
      this.m_productsToIgnoreCache = new Set<ProductProto>();
      this.m_productsToRefreshCache = new Set<ProductProto>();
      this.m_trucksToBalanceCache = new Lyst<VehicleBuffersRegistry.TruckData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_dumpingManager = dumpingManager;
      this.m_surfaceManager = surfaceManager;
      this.m_simLoopEvents = simLoopEvents;
      this.m_vehiclePathFindingManager = vehiclePathFindingManager;
      this.m_unreachablesManager = unreachablesManager;
      this.m_random = randomProvider.GetSimRandomFor((object) this);
      this.m_simLoopEvents.Update.Add<VehicleBuffersRegistry>(this, new Action(this.simUpdate));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void onLoaded()
    {
      this.m_registeredInputBuffers = new Dict<IProductBuffer, RegisteredInputBuffer>();
      this.m_registeredOutputBuffers = new Dict<IProductBuffer, RegisteredOutputBuffer>();
      foreach (VehicleBuffersRegistry.RegisteredBuffers registeredBuffers in this.m_registeredBuffersPerProduct.Values)
      {
        foreach (RegisteredInputBuffer inputBuffer in registeredBuffers.InputBuffers)
          this.m_registeredInputBuffers.Add(inputBuffer.Buffer, inputBuffer);
        foreach (RegisteredOutputBuffer outputBuffer in registeredBuffers.OutputBuffers)
          this.m_registeredOutputBuffers.Add(outputBuffer.Buffer, outputBuffer);
      }
    }

    private void simUpdate()
    {
      if (this.m_trucksWaitingForJobs.Count <= 0 || this.m_simLoopEvents.CurrentStep.Value < this.m_lastBalancingStep.Value + (this.m_jobsFoundDuringLastBalancing > 0 ? 17 : 23))
        return;
      Duration balancingLatency = this.GetBalancingLatency();
      Percent minTruckCapUtilization = !this.AllowPartialTrucks ? 100.Percent() : (balancingLatency < 10.Seconds() ? 50.Percent() : 100.Percent());
      this.m_lastBalancingStep = this.m_simLoopEvents.CurrentStep;
      this.m_jobsFoundDuringLastBalancing = 0;
      this.balanceBuffers(minTruckCapUtilization);
      foreach (Vehicle trucksWaitingForJob in this.m_trucksWaitingForJobs)
        this.m_unreachablesManager.Value.ReportVehicleIdle(trucksWaitingForJob);
    }

    /// <summary>
    /// This can be called every update and it is no need to un-register.
    /// We will check <see cref="M:Mafi.Core.Vehicles.Trucks.Truck.IsAvailableToBalanceCargo" /> before balancing.
    /// </summary>
    /// <param name="truck"></param>
    public void RegisterTruckForBalancingJob(Truck truck)
    {
      Assert.That<bool>(truck.IsEmpty).IsTrue();
      this.m_trucksWaitingForJobs.AddIfNotPresent(truck);
    }

    /// <summary>
    /// Finds the most suitable input buffer. It always prioritizes entities with the highest priority. And then the
    /// closest ones. If you provide preferredEntities it will try these before fallbacking.
    /// </summary>
    public Option<RegisteredInputBuffer> TryGetProductInputForVehicle(
      Vehicle vehicle,
      ProductQuantity productQuantity,
      Option<IReadOnlySet<IEntityAssignedAsInput>> preferredEntities,
      out bool hadEligibleAssignedEntity,
      bool allowOnlyPreferredEntities = false,
      IEntityAssignedAsOutput customAssignedEntity = null)
    {
      return this.tryGetProductInputForVehicleInternal(vehicle, productQuantity, preferredEntities, out hadEligibleAssignedEntity, allowOnlyPreferredEntities, customAssignedEntity);
    }

    private Option<RegisteredInputBuffer> tryGetProductInputForVehicleInternal(
      Vehicle vehicle,
      ProductQuantity productQuantity,
      Option<IReadOnlySet<IEntityAssignedAsInput>> preferredEntities,
      out bool hadEligibleAssignedEntity,
      bool allowOnlyPreferredEntities,
      IEntityAssignedAsOutput customAssignedEntity = null)
    {
      hadEligibleAssignedEntity = false;
      Option<VehicleBuffersRegistry.RegisteredBuffers> option = this.m_registeredBuffersPerProduct.Get<ProductProto, VehicleBuffersRegistry.RegisteredBuffers>(productQuantity.Product);
      if (option.IsNone)
        return (Option<RegisteredInputBuffer>) Option.None;
      VehicleBuffersRegistry.InputBufferDistanceComparator distanceComparator = VehicleBuffersRegistry.InputBufferDistanceComparator.GetInstance(vehicle.GroundPositionTile2i);
      Option<IEntityAssignedAsOutput> staticAssignee = (customAssignedEntity ?? vehicle.AssignedTo.ValueOrNull as IEntityAssignedAsOutput).CreateOption<IEntityAssignedAsOutput>();
      IReadOnlySet<IEntity> unreachables = this.m_unreachablesManager.Value.GetUnreachableEntitiesFor((IPathFindingVehicle) vehicle);
      if (preferredEntities.HasValue && preferredEntities.Value.IsNotEmpty<IEntityAssignedAsInput>())
      {
        option.Value.InputBuffers.Where<RegisteredInputBuffer>((Func<RegisteredInputBuffer, bool>) (x => ((IEnumerable<IStaticEntity>) preferredEntities.Value).Contains<IStaticEntity>(x.Entity))).ToCleanLyst<RegisteredInputBuffer>(this.m_buffersCache);
        if (this.m_buffersCache.IsNotEmpty)
        {
          hadEligibleAssignedEntity = true;
          return filterEligibleBufferFromSet(this.m_buffersCache, true);
        }
      }
      return allowOnlyPreferredEntities && preferredEntities.HasValue && preferredEntities.Value.IsNotEmpty<IEntityAssignedAsInput>() ? Option<RegisteredInputBuffer>.None : filterEligibleBufferFromSet(option.Value.InputBuffers, false);

      bool verifyAssignment(RegisteredInputBuffer buffer)
      {
        if (!buffer.HasAssignedOutputEntities || buffer.EntityAsAssignee.Value.AllowNonAssignedOutput)
          return buffer.CanBeServedBy(vehicle, false);
        if (vehicle.AssignedTo.ValueOrNull == buffer.Entity)
          return true;
        return staticAssignee.HasValue && buffer.EntityAsAssignee.Value.AssignedOutputs.Contains(staticAssignee.Value);
      }

      bool canAcceptCargo(RegisteredInputBuffer buffer, bool ignoreAssignments)
      {
        buffer.RefreshPriorities();
        if (!buffer.IsAvailableCached || unreachables.Contains((IEntity) buffer.Entity) || !ignoreAssignments && !verifyAssignment(buffer))
          return false;
        Quantity quantity1 = buffer.RemainingCapacityCached.Min(productQuantity.Quantity);
        Quantity quantity2 = buffer.OptimalQuantityOrMaxCached.Min(productQuantity.Quantity);
        return quantity2.IsPositive && quantity2 <= quantity1;
      }

      Option<RegisteredInputBuffer> filterEligibleBufferFromSet(
        Lyst<RegisteredInputBuffer> buffersToFilter,
        bool ignoreAssignments)
      {
        return (Option<RegisteredInputBuffer>) buffersToFilter.Where<RegisteredInputBuffer>((Func<RegisteredInputBuffer, bool>) (x => canAcceptCargo(x, ignoreAssignments))).OrderBy<RegisteredInputBuffer, int>((Func<RegisteredInputBuffer, int>) (x => x.CombinedPriorityCached)).ThenBy<RegisteredInputBuffer, RegisteredInputBuffer>((Func<RegisteredInputBuffer, RegisteredInputBuffer>) (x => x), (IComparer<RegisteredInputBuffer>) distanceComparator).FirstOrDefault<RegisteredInputBuffer>();
      }
    }

    public Option<RegisteredInputBuffer> TryGetFallbackInputForVehicle(ProductProto product)
    {
      Option<VehicleBuffersRegistry.RegisteredBuffers> option = this.m_registeredBuffersPerProduct.Get<ProductProto, VehicleBuffersRegistry.RegisteredBuffers>(product);
      if (option.IsNone)
        return (Option<RegisteredInputBuffer>) Option.None;
      foreach (RegisteredInputBuffer inputBuffer in option.Value.InputBuffers)
      {
        if (inputBuffer.IsFallbackOnly)
          return (Option<RegisteredInputBuffer>) inputBuffer;
      }
      return (Option<RegisteredInputBuffer>) Option.None;
    }

    /// <summary>
    /// Finds the most suitable output buffer. It always prioritizes entities with the highest priority. And then the
    /// closest ones. If you provide preferredEntities it will try these before fallbacking. You can also define
    /// entity to ignore (useful to avoid delivering yourself).
    /// </summary>
    public Option<RegisteredOutputBuffer> TryGetProductOutputForVehicle(
      Vehicle vehicle,
      ProductProto product,
      IStaticEntity entityToIgnore,
      IReadOnlySet<IEntityAssignedAsOutput> preferredEntities = null,
      bool isForRefueling = false,
      bool forcePreferredEntity = false)
    {
      return this.tryGetProductOutputForVehicleInternal(vehicle, product, entityToIgnore, preferredEntities, isForRefueling, forcePreferredEntity);
    }

    private Option<RegisteredOutputBuffer> tryGetProductOutputForVehicleInternal(
      Vehicle vehicle,
      ProductProto product,
      IStaticEntity entityToIgnore,
      IReadOnlySet<IEntityAssignedAsOutput> preferredEntities,
      bool isForRefueling,
      bool forcePreferredEntity)
    {
      Option<VehicleBuffersRegistry.RegisteredBuffers> option = this.m_registeredBuffersPerProduct.Get<ProductProto, VehicleBuffersRegistry.RegisteredBuffers>(product);
      if (option.IsNone)
        return Option<RegisteredOutputBuffer>.None;
      IReadOnlySet<IEntity> unreachables = this.m_unreachablesManager.Value.GetUnreachableEntitiesFor((IPathFindingVehicle) vehicle);
      Lyst<RegisteredOutputBuffer> lyst = option.Value.OutputBuffers.Where<RegisteredOutputBuffer>((Func<RegisteredOutputBuffer, bool>) (x =>
      {
        if (forcePreferredEntity && !((IEnumerable<IStaticEntity>) preferredEntities).Contains<IStaticEntity>(x.Entity) || unreachables.Contains((IEntity) x.Entity))
          return false;
        x.RefreshPriorities(isForRefueling);
        return x.IsAvailableCached && x.Entity != entityToIgnore;
      })).OrderBy<RegisteredOutputBuffer, int>((Func<RegisteredOutputBuffer, int>) (x => 80 * x.RawPriorityCached + 10 * x.JobsCount + x.Entity.Position2f.DistanceTo(vehicle.Position2f).ToIntFloored())).ToLyst<RegisteredOutputBuffer>();
      if (!forcePreferredEntity && preferredEntities != null && preferredEntities.IsNotEmpty<IEntityAssignedAsOutput>())
      {
        RegisteredOutputBuffer forVehicleInternal = lyst.FirstOrDefault<RegisteredOutputBuffer>((Predicate<RegisteredOutputBuffer>) (x => ((IEnumerable<IStaticEntity>) preferredEntities).Contains<IStaticEntity>(x.Entity)));
        if (forVehicleInternal != null)
          return (Option<RegisteredOutputBuffer>) forVehicleInternal;
      }
      return (Option<RegisteredOutputBuffer>) lyst.FirstOrDefault<RegisteredOutputBuffer>();
    }

    /// <summary>
    /// TODO: No longer true. Called also from our balancing code.
    /// For UI. Don't call often.
    /// 
    /// Logistics scans balancing jobs by taking each product one by one. If there are some products that
    /// we did not manage to balance for some time already it means logistics is getting overloaded with work.
    /// Idle logistics will have latency close to 0 sec. While a busy one will reach 10's of seconds.
    /// </summary>
    public Duration GetBalancingLatency()
    {
      Duration balancingLatency = Duration.Zero;
      foreach (VehicleBuffersRegistry.RegisteredBuffers registeredBuffers in this.m_registeredBuffersPerProduct.Values)
      {
        if (registeredBuffers.LastProcessedSimStep.IsPositive)
          balancingLatency = balancingLatency.Max(this.m_lastBalancingStep - registeredBuffers.LastProcessedSimStep);
      }
      return balancingLatency;
    }

    private void updateTrucksToBalance(Percent minTruckCapUtilization)
    {
      this.m_trucksToBalanceCache.Clear();
      foreach (Truck truck in this.m_trucksWaitingForJobs.ToArray())
      {
        if (!truck.IsAvailableToBalanceCargo())
        {
          this.m_trucksWaitingForJobs.TryRemoveReplaceLast(truck);
        }
        else
        {
          IReadOnlySet<IEntity> unreachableEntitiesFor = this.m_unreachablesManager.Value.GetUnreachableEntitiesFor((IPathFindingVehicle) truck);
          this.m_trucksToBalanceCache.Add(new VehicleBuffersRegistry.TruckData(truck, minTruckCapUtilization, unreachableEntitiesFor));
        }
      }
    }

    private void assignJobToTruck(BalancingJobSpec spec, Percent minTruckCapUtilization)
    {
      this.m_trucksWaitingForJobs.TryRemoveReplaceLast(spec.Truck);
      spec.Truck.AssignBalancingJob(spec);
      this.updateTrucksToBalance(minTruckCapUtilization);
      ++this.m_jobsFoundDuringLastBalancing;
    }

    /// <summary>
    /// This method handles balancing for multiple trucks at once. It is actually optimized
    /// to run for multiple trucks at once and the cost is almost "constant" in number of trucks.
    /// This is achieved due to several tricks:
    /// 1) If job is found, we refresh only the buffers per product that were assigned a job, the rest
    /// stays as it was. We also skip pairing buffers for which we failed to pair in that run.
    /// 2) To make this even better, if there is a same priority level for multiple products, we try to pair
    /// all of them before even refreshing priorities and going again (as in step 1)).
    /// 3) We also reject buffers early on by checking min. truck capacity to avoid running checks for all
    /// trucks
    /// 4) Finally we also filter trucks that are not eligible for a particular product. So a tank truck
    /// will not participate in balancing of loose products.
    /// 
    /// Some measured observations:
    /// Observed durations to assign trucks in one go (in debug):
    /// - 6 trucks in 1.4ms
    /// - 3 trucks in 0.7-2.9ms
    /// - 1 truck in 0.5-2.3ms
    /// The reason for the variance is that it depends more on the buffers and designations instead on the
    /// number of trucks.
    /// 
    /// We also save a bit on CPU by not having all trucks call this individually.
    /// It went from ~40ms to ~30ms of CPU time spent in balancing code until it assigns 50 jobs
    /// in an end-game factory.
    /// Note that this was measured on a busy logistics. Players with idle vehicles will benefit more.
    /// 
    /// There is also a big benefit in running this for multiple trucks as we always assign the truck
    /// that is closest which saves on driving. So there is a trade-off on how long should we let trucks
    /// wait hoping they get a job nearby. In general, we don't let trucks wait too long.
    /// 
    /// TODO: We can consider assigning large trucks to large cargo jobs (using some weight + distance).
    /// </summary>
    private void balanceBuffers(Percent minTruckCapUtilization)
    {
      this.updateTrucksToBalance(minTruckCapUtilization);
      if (this.m_trucksToBalanceCache.IsEmpty)
        return;
      this.m_buffersTmp.Clear();
      Quantity minTruckCapacity1 = getMinTruckCapacity();
      foreach (KeyValuePair<ProductProto, VehicleBuffersRegistry.RegisteredBuffers> keyValuePair in this.m_registeredBuffersPerProduct)
        preprocessBuffersFor(keyValuePair.Value, minTruckCapacity1);
      this.m_productsToIgnoreCache.Clear();
      this.m_productsToRefreshCache.Clear();
      while (pairTmpBuffers() && this.m_trucksToBalanceCache.IsNotEmpty)
      {
        this.m_buffersTmp.Clear();
        Quantity minTruckCapacity2 = getMinTruckCapacity();
        foreach (VehicleBuffersRegistry.RegisteredBuffers registeredBuffers in this.m_registeredBuffersPerProduct.Values)
        {
          if (!this.m_productsToIgnoreCache.Contains(registeredBuffers.Product))
          {
            if (this.m_productsToRefreshCache.Contains(registeredBuffers.Product))
              preprocessBuffersFor(registeredBuffers, minTruckCapacity2);
            else if (registeredBuffers.OutputBuffersTmp.IsNotEmpty<RegisteredOutputBuffer>())
              this.m_buffersTmp.Add(registeredBuffers);
          }
        }
      }

      void preprocessBuffersFor(
        VehicleBuffersRegistry.RegisteredBuffers registeredBuffers,
        Quantity minActionableTruckCapacity)
      {
        registeredBuffers.InputBuffersTmp.Clear();
        registeredBuffers.OutputBuffersTmp.Clear();
        registeredBuffers.SortingPriorityTmp = int.MaxValue;
        int self1 = 16;
        foreach (RegisteredOutputBuffer outputBuffer in registeredBuffers.OutputBuffers)
        {
          outputBuffer.RefreshPriorities();
          if (outputBuffer.IsAvailableCached)
          {
            registeredBuffers.OutputBuffersTmp.Add(outputBuffer);
            Quantity quantity = outputBuffer.OptimalQuantityOrMaxCached.Min(minActionableTruckCapacity);
            bool flag = quantity.IsPositive && quantity <= outputBuffer.AvailableQuantityCached;
            self1 = self1.Min(flag ? outputBuffer.CombinedPriorityCached : outputBuffer.CombinedPriorityCached.Max(14));
          }
        }
        DesignationsPerProductCache data;
        if (registeredBuffers.OutputBuffersTmp.IsEmpty<RegisteredOutputBuffer>() && (!this.m_surfaceManager.Value.TryGetDataFor(registeredBuffers.Product, out data) || data.LeftToClear.IsNotPositive))
        {
          registeredBuffers.LastProcessedSimStep = this.m_simLoopEvents.CurrentStep;
        }
        else
        {
          int self2 = 16;
          foreach (RegisteredInputBuffer inputBuffer in registeredBuffers.InputBuffers)
          {
            inputBuffer.RefreshPriorities();
            if (inputBuffer.IsAvailableCached)
            {
              registeredBuffers.InputBuffersTmp.Add(inputBuffer);
              Quantity quantity = inputBuffer.OptimalQuantityOrMaxCached.Min(minActionableTruckCapacity);
              bool flag = quantity.IsPositive && quantity <= inputBuffer.RemainingCapacityCached;
              self2 = self2.Min(flag ? inputBuffer.CombinedPriorityCached : inputBuffer.CombinedPriorityCached.Max(14));
            }
          }
          registeredBuffers.SortingPriorityTmp = self1.Min(self2);
          this.m_buffersTmp.Add(registeredBuffers);
        }
      }

      Quantity getMinTruckCapacity()
      {
        Quantity quantity = Quantity.MaxValue;
        foreach (VehicleBuffersRegistry.TruckData truckData in this.m_trucksToBalanceCache)
          quantity = quantity.Min(truckData.Capacity);
        return quantity.ScaledBy(minTruckCapUtilization);
      }

      bool pairTmpBuffers()
      {
        if (this.m_buffersTmp.IsEmpty<VehicleBuffersRegistry.RegisteredBuffers>())
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_buffersTmp.Sort(VehicleBuffersRegistry.\u003C\u003EO.\u003C0\u003E__BuffersAscendingPriorityComparison ?? (VehicleBuffersRegistry.\u003C\u003EO.\u003C0\u003E__BuffersAscendingPriorityComparison = new Comparison<VehicleBuffersRegistry.RegisteredBuffers>(VehicleBuffersRegistry.BuffersAscendingPriorityComparison)));
        this.m_buffersTmpSet.Clear();
        int sortingPriorityTmp = this.m_buffersTmp[0].SortingPriorityTmp;
        foreach (VehicleBuffersRegistry.RegisteredBuffers registeredBuffers in this.m_buffersTmp)
        {
          if (registeredBuffers.SortingPriorityTmp == sortingPriorityTmp)
          {
            this.m_buffersTmpSet.Add(registeredBuffers);
          }
          else
          {
            if (pairCurrentPriorityGroupAtRandom())
            {
              this.m_buffersTmpSet.Clear();
              return true;
            }
            sortingPriorityTmp = registeredBuffers.SortingPriorityTmp;
            this.m_buffersTmpSet.Add(registeredBuffers);
          }
        }
        return pairCurrentPriorityGroupAtRandom();
      }

      bool pairCurrentPriorityGroupAtRandom()
      {
        bool flag = false;
        while (this.m_buffersTmpSet.IsNotEmpty && this.m_trucksToBalanceCache.IsNotEmpty)
        {
          VehicleBuffersRegistry.RegisteredBuffers buffers = this.m_buffersTmpSet.SampleRandomKeyOrDefault(this.m_random);
          this.m_buffersTmpSet.Remove(buffers);
          bool hadEligibleTruck;
          BalancingJobSpec? nullable = balanceBuffersForProduct(buffers, out hadEligibleTruck);
          if (nullable.HasValue)
          {
            this.m_productsToRefreshCache.Add(buffers.Product);
            this.assignJobToTruck(nullable.Value, minTruckCapUtilization);
            flag = true;
          }
          else
          {
            this.m_productsToIgnoreCache.Add(buffers.Product);
            if (hadEligibleTruck)
              buffers.LastProcessedSimStep = this.m_simLoopEvents.CurrentStep;
          }
        }
        return flag;
      }

      BalancingJobSpec? balanceBuffersForProduct(
        VehicleBuffersRegistry.RegisteredBuffers buffers,
        out bool hadEligibleTruck)
      {
        this.m_allTrucksPerProductCache.Clear();
        Quantity quantity1 = Quantity.MaxValue;
        foreach (VehicleBuffersRegistry.TruckData truckData in this.m_trucksToBalanceCache)
        {
          if ((!truckData.Truck.ProductType.HasValue || !(buffers.Product.Type != truckData.Truck.ProductType.Value)) && (!truckData.AssignedBuilding.HasValue || !(truckData.AssignedBuilding.Value is Storage storage) || !storage.StoredProduct.IsNone && !(storage.StoredProduct.Value.Type != buffers.Product.Type)))
          {
            this.m_allTrucksPerProductCache.Add(truckData);
            quantity1 = truckData.Capacity.Min(quantity1);
          }
        }
        if (this.m_allTrucksPerProductCache.IsEmpty)
        {
          hadEligibleTruck = false;
          return new BalancingJobSpec?();
        }
        hadEligibleTruck = true;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        buffers.InputBuffersTmp.Sort(VehicleBuffersRegistry.\u003C\u003EO.\u003C1\u003E__InputAscendingPriorityComparison ?? (VehicleBuffersRegistry.\u003C\u003EO.\u003C1\u003E__InputAscendingPriorityComparison = new Comparison<RegisteredInputBuffer>(VehicleBuffersRegistry.InputAscendingPriorityComparison)));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        buffers.OutputBuffersTmp.Sort(VehicleBuffersRegistry.\u003C\u003EO.\u003C2\u003E__OutputAscendingPriorityComparison ?? (VehicleBuffersRegistry.\u003C\u003EO.\u003C2\u003E__OutputAscendingPriorityComparison = new Comparison<RegisteredOutputBuffer>(VehicleBuffersRegistry.OutputAscendingPriorityComparison)));
        BalancingJobSpec? nullable = this.tryBalanceBuffers(buffers, this.m_allTrucksPerProductCache, quantity1.ScaledBy(minTruckCapUtilization));
        if (nullable.HasValue && nullable.Value.InputBuffer.HasValue && nullable.Value.OutputBuffer.HasValue)
        {
          BalancingJobSpec balancingJobSpec = nullable.Value;
          ProductProto product = balancingJobSpec.InputBuffer.Value.Product;
          Quantity remainingCapacityCached = balancingJobSpec.InputBuffer.Value.RemainingCapacityCached;
          Quantity quantity2 = balancingJobSpec.OutputBuffer.Value.AvailableQuantityCached.Min(remainingCapacityCached).Min(balancingJobSpec.Truck.Capacity);
          Option<Lyst<SecondaryInputBufferSpec>> secondaryInputBuffers;
          Option<Lyst<SecondaryOutputBufferSpec>> secondaryOutputBuffers;
          this.tryToAddMoreConstructionJobs(balancingJobSpec.Truck, quantity2, buffers, balancingJobSpec.InputBuffer.Value, balancingJobSpec.OutputBuffer.Value, out secondaryInputBuffers, out secondaryOutputBuffers);
          return new BalancingJobSpec?(new BalancingJobSpec(balancingJobSpec.Truck, balancingJobSpec.InputBuffer.Value, balancingJobSpec.OutputBuffer.Value, product.WithQuantity(quantity2), secondaryInputBuffers, secondaryOutputBuffers));
        }
        DesignationsPerProductCache data;
        if (this.m_surfaceManager.Value.TryGetDataFor(buffers.Product, out data))
        {
          BalancingJobSpec spec1;
          if (this.tryGetSurfaceClearingSpec(buffers, data, quantity1, out spec1))
            return new BalancingJobSpec?(spec1);
          BalancingJobSpec spec2;
          if (this.tryGetSurfacePlacementSpec(buffers, data, quantity1, out spec2))
            return new BalancingJobSpec?(spec2);
        }
        if (buffers.OutputBuffersTmp.Count <= 0)
          return new BalancingJobSpec?();
        Option<LooseProductProto> dumpableProduct = buffers.Product.DumpableProduct;
        if (dumpableProduct.IsNone)
          return new BalancingJobSpec?();
        foreach (RegisteredOutputBuffer outputBuffer in buffers.OutputBuffersTmp)
        {
          if (!outputBuffer.AvailableQuantityCached.IsZero && outputBuffer.RawPriorityCached <= 14 && !(outputBuffer.OptimalQuantityOrMaxCached.Min(quantity1) > outputBuffer.AvailableQuantityCached))
          {
            this.m_allTrucksPerProductCache.Sort((IComparer<VehicleBuffersRegistry.TruckData>) VehicleBuffersRegistry.TruckDistanceComparer.OrderByDistanceFrom(outputBuffer.Position2f));
            foreach (VehicleBuffersRegistry.TruckData truckData in this.m_allTrucksPerProductCache)
            {
              if (!(outputBuffer.OptimalQuantityOrMaxCached.Min(truckData.Capacity) > outputBuffer.AvailableQuantityCached) && outputBuffer.CanBeServedBy((Vehicle) truckData.Truck, true) && (!truckData.AssignedBuilding.HasValue || truckData.AssignedBuilding.Value == outputBuffer.Entity) && !truckData.UnreachableEntities.Contains((IEntity) outputBuffer.Entity))
              {
                this.m_terrainDesignationsCache.Clear();
                TerrainDesignation bestDesignation;
                if (this.m_dumpingManager.Value.TryFindClosestReadyToDump(outputBuffer.Entity.CenterTile.Tile2i, outputBuffer, (Option<LooseProductProto>) dumpableProduct.Value, truckData.Truck, out bestDesignation, this.m_terrainDesignationsCache))
                {
                  Quantity quantity3 = truckData.Capacity.Min(outputBuffer.AvailableQuantityCached);
                  return new BalancingJobSpec?(new BalancingJobSpec(truckData.Truck, bestDesignation, new Lyst<TerrainDesignation>((IEnumerable<TerrainDesignation>) this.m_terrainDesignationsCache), outputBuffer, new ProductQuantity((ProductProto) dumpableProduct.Value, quantity3)));
                }
              }
            }
          }
        }
        return new BalancingJobSpec?();
      }
    }

    private BalancingJobSpec? tryBalanceBuffers(
      VehicleBuffersRegistry.RegisteredBuffers buffers,
      Lyst<VehicleBuffersRegistry.TruckData> trucksAvailable,
      Quantity minTruckCapacity)
    {
      this.m_availableJobsTmp.Clear();
      int? currentPriority = new int?();
      foreach (RegisteredOutputBuffer registeredOutputBuffer in buffers.OutputBuffersTmp)
      {
        bool flag1 = false;
        int num1 = -1;
        foreach (RegisteredInputBuffer inputBuffer in buffers.InputBuffersTmp)
        {
          int num2 = (1 + inputBuffer.CombinedPriorityCached) * (1 + registeredOutputBuffer.CombinedPriorityCached);
          if (currentPriority.HasValue)
          {
            int? nullable = currentPriority;
            int num3 = num2;
            if (nullable.GetValueOrDefault() < num3 & nullable.HasValue)
              break;
          }
          if (inputBuffer.RawPriorityCached <= 14 || registeredOutputBuffer.RawPriorityCached <= 14)
          {
            bool assignedOutputEntities = inputBuffer.HasAssignedOutputEntities;
            bool assignedInputEntities = registeredOutputBuffer.HasAssignedInputEntities;
            bool flag2 = inputBuffer.NumberOfVehiclesAssigned > 0;
            bool flag3 = registeredOutputBuffer.NumberOfVehiclesAssigned > 0;
            bool flag4 = (inputBuffer.IsConstructionBuffer || !assignedOutputEntities && !flag2) && (registeredOutputBuffer.IsConstructionBuffer || !assignedInputEntities && !flag3);
            if (((num1 < 0 ? 0 : (num1 < num2 ? 1 : 0)) & (flag4 ? 1 : 0)) == 0 && (inputBuffer.IsConstructionBuffer || !(inputBuffer.Entity is Storage entity1) || !(registeredOutputBuffer.Entity is Storage entity2) || !entity1.IsChainedTo(entity2)))
            {
              Quantity quantity1 = registeredOutputBuffer.AvailableQuantityCached.Min(inputBuffer.RemainingCapacityCached);
              Quantity quantity2 = inputBuffer.OptimalQuantityOrMaxCached.Min(registeredOutputBuffer.OptimalQuantityOrMaxCached);
              Quantity quantity3 = quantity2.Min(minTruckCapacity);
              if (!quantity3.IsPositive || !(quantity3 <= quantity1))
              {
                if (num1 < 0 & flag4 && inputBuffer.OptimalQuantityOrMaxCached.IsPositive && inputBuffer.RemainingCapacityCached >= quantity3)
                  num1 = num2;
              }
              else if ((!assignedOutputEntities || inputBuffer.IgnoreAssignedEntities || inputBuffer.EntityAsAssignee.Value.AllowNonAssignedOutput) && (!assignedInputEntities || registeredOutputBuffer.IgnoreAssignedEntities) || assignedOutputEntities && assignedInputEntities && ((IEnumerable<IStaticEntity>) inputBuffer.EntityAsAssignee.Value.AssignedOutputs).Contains<IStaticEntity>(registeredOutputBuffer.Entity))
              {
                bool flag5 = flag2 & flag3;
                this.m_trucksMatchedCache.Clear();
                foreach (VehicleBuffersRegistry.TruckData truckData in trucksAvailable)
                {
                  Quantity quantity4 = quantity2.Min(truckData.CapacityWithMinUtilApplied);
                  if (quantity4.IsPositive && quantity4 <= quantity1 && (!truckData.AssignedBuilding.HasValue || inputBuffer.Entity == truckData.AssignedBuilding.Value || registeredOutputBuffer.Entity == truckData.AssignedBuilding.Value) && !truckData.UnreachableEntities.Contains((IEntity) inputBuffer.Entity) && !truckData.UnreachableEntities.Contains((IEntity) registeredOutputBuffer.Entity))
                  {
                    if (flag5)
                    {
                      if (!inputBuffer.VehiclesEnforcer.Value.HasAssignedVehicle((Vehicle) truckData.Truck) && !registeredOutputBuffer.VehiclesEnforcer.Value.HasAssignedVehicle((Vehicle) truckData.Truck))
                        continue;
                    }
                    else if (flag2 && !inputBuffer.CanBeServedBy((Vehicle) truckData.Truck, true) || flag3 && !registeredOutputBuffer.CanBeServedBy((Vehicle) truckData.Truck, true))
                      continue;
                    this.m_trucksMatchedCache.Add(truckData);
                  }
                }
                if (this.m_trucksMatchedCache.IsNotEmpty)
                {
                  VehicleBuffersRegistry.TruckData fromNonEmptyList = this.getClosestTruckFromNonEmptyList(this.m_trucksMatchedCache, registeredOutputBuffer);
                  if (currentPriority.HasValue)
                  {
                    int? nullable = currentPriority;
                    int num4 = num2;
                    if (!(nullable.GetValueOrDefault() > num4 & nullable.HasValue))
                      goto label_23;
                  }
                  currentPriority = new int?(num2);
                  this.m_availableJobsTmp.Clear();
label_23:
                  flag1 = true;
                  this.m_availableJobsTmp.Add(new BalancingJobSpec(fromNonEmptyList.Truck, inputBuffer, registeredOutputBuffer));
                }
              }
            }
          }
        }
        if (!flag1 && registeredOutputBuffer.RawPriorityCached <= 14 && registeredOutputBuffer.UseFallbackIfNeeded && this.m_availableJobsTmp.IsEmpty)
        {
          Option<RegisteredInputBuffer> fallbackInputForVehicle = this.TryGetFallbackInputForVehicle(registeredOutputBuffer.Product);
          if (fallbackInputForVehicle.HasValue && testPriorityForFallback(registeredOutputBuffer))
          {
            this.m_trucksMatchedCache.Clear();
            foreach (VehicleBuffersRegistry.TruckData truckData in trucksAvailable)
            {
              if (!truckData.UnreachableEntities.Contains((IEntity) fallbackInputForVehicle.Value.Entity))
                this.m_trucksMatchedCache.Add(truckData);
            }
            if (this.m_trucksMatchedCache.IsNotEmpty)
              this.m_availableJobsTmp.Add(new BalancingJobSpec(this.getClosestTruckFromNonEmptyList(this.m_trucksMatchedCache, registeredOutputBuffer).Truck, fallbackInputForVehicle.Value, registeredOutputBuffer));
          }
        }
      }
      if (this.m_availableJobsTmp.IsEmpty)
        return new BalancingJobSpec?();
      BalancingJobSpec elementOrDefault = this.m_random.GetRandomElementOrDefault<BalancingJobSpec>((IIndexable<BalancingJobSpec>) this.m_availableJobsTmp);
      this.m_availableJobsTmp.Clear();
      return new BalancingJobSpec?(elementOrDefault);

      bool testPriorityForFallback(RegisteredOutputBuffer outBuffer)
      {
        int num = (1 + outBuffer.CombinedPriorityCached) * (1 + outBuffer.CombinedPriorityCached);
        if (!currentPriority.HasValue)
          currentPriority = new int?(num);
        return num <= currentPriority.Value;
      }
    }

    private VehicleBuffersRegistry.TruckData getClosestTruckFromNonEmptyList(
      Lyst<VehicleBuffersRegistry.TruckData> trucks,
      RegisteredOutputBuffer outputBuffer)
    {
      Assert.That<Lyst<VehicleBuffersRegistry.TruckData>>(trucks).IsNotEmpty<VehicleBuffersRegistry.TruckData>();
      Fix64 fix64_1 = Fix64.MaxIntValue;
      VehicleBuffersRegistry.TruckData fromNonEmptyList = trucks.First;
      foreach (VehicleBuffersRegistry.TruckData truck in trucks)
      {
        Fix64 fix64_2 = outputBuffer.Position2f.DistanceSqrTo(truck.Position2f);
        if (fix64_2 < fix64_1)
        {
          fromNonEmptyList = truck;
          fix64_1 = fix64_2;
        }
      }
      return fromNonEmptyList;
    }

    private bool tryGetSurfacePlacementSpec(
      VehicleBuffersRegistry.RegisteredBuffers buffers,
      DesignationsPerProductCache data,
      Quantity minTruckCapacity,
      out BalancingJobSpec spec)
    {
      if (buffers.OutputBuffersTmp.Count <= 0 || data.LeftToPlace.IsNotPositive)
      {
        spec = new BalancingJobSpec();
        return false;
      }
      foreach (RegisteredOutputBuffer buffer in buffers.OutputBuffersTmp)
      {
        if (!buffer.AvailableQuantityCached.IsZero && buffer.RawPriorityCached < 16 && !(buffer.OptimalQuantityOrMaxCached.Min(minTruckCapacity).Min(data.LeftToPlace) > buffer.AvailableQuantityCached) && (!buffer.HasAssignedInputEntities || buffer.IgnoreAssignedEntities))
        {
          this.m_allTrucksPerProductCache.Sort((IComparer<VehicleBuffersRegistry.TruckData>) VehicleBuffersRegistry.TruckDistanceComparer.OrderByDistanceFrom(buffer.Position2f));
          foreach (VehicleBuffersRegistry.TruckData truckData in this.m_allTrucksPerProductCache)
          {
            if (!(buffer.OptimalQuantityOrMaxCached.Min(truckData.Capacity).Min(data.LeftToPlace) > buffer.AvailableQuantityCached) && buffer.CanBeServedBy((Vehicle) truckData.Truck, true) && (!truckData.AssignedBuilding.HasValue || truckData.AssignedBuilding.Value == buffer.Entity) && !truckData.UnreachableEntities.Contains((IEntity) buffer.Entity))
            {
              Quantity maxQuantity = truckData.Capacity.Min(buffer.AvailableQuantityCached);
              this.m_surfaceDesignationsCache.Clear();
              Quantity toExchange;
              if (this.m_surfaceManager.Value.TryFindClosestReadyToPlace(buffers.Product, buffer.Entity.CenterTile.Tile2i, truckData.Truck, maxQuantity, this.m_surfaceDesignationsCache, out toExchange))
              {
                spec = new BalancingJobSpec(truckData.Truck, new Lyst<SurfaceDesignation>((IEnumerable<SurfaceDesignation>) this.m_surfaceDesignationsCache), buffer, buffer.Product.WithQuantity(maxQuantity.Min(toExchange)));
                return true;
              }
            }
          }
        }
      }
      spec = new BalancingJobSpec();
      return false;
    }

    private bool tryGetSurfaceClearingSpec(
      VehicleBuffersRegistry.RegisteredBuffers buffers,
      DesignationsPerProductCache data,
      Quantity minTruckCapacity,
      out BalancingJobSpec spec)
    {
      if (data.LeftToClear.IsNotPositive)
      {
        spec = new BalancingJobSpec();
        return false;
      }
      Tile2f position2f;
      if (data.LeftToPlace.IsPositive)
      {
        foreach (VehicleBuffersRegistry.TruckData truckData in this.m_allTrucksPerProductCache)
        {
          if (!truckData.AssignedBuilding.HasValue)
          {
            Quantity quantity = truckData.Capacity;
            this.m_surfaceDesignationsCache.Clear();
            ISurfaceDesignationsManager designationsManager1 = this.m_surfaceManager.Value;
            ProductProto product1 = buffers.Product;
            position2f = truckData.Position2f;
            Tile2i tile2i1 = position2f.Tile2i;
            Truck truck1 = truckData.Truck;
            Quantity maxQuantity1 = quantity;
            Lyst<SurfaceDesignation> designationsCache1 = this.m_surfaceDesignationsCache;
            Quantity rhs1;
            ref Quantity local1 = ref rhs1;
            if (designationsManager1.TryFindClosestReadyToClear(product1, tile2i1, truck1, maxQuantity1, designationsCache1, out local1))
            {
              Lyst<SurfaceDesignation> lyst = this.m_surfaceDesignationsCache.ToLyst<SurfaceDesignation>();
              this.m_surfaceDesignationsCache.Clear();
              quantity = quantity.Min(rhs1);
              ISurfaceDesignationsManager designationsManager2 = this.m_surfaceManager.Value;
              ProductProto product2 = buffers.Product;
              position2f = truckData.Position2f;
              Tile2i tile2i2 = position2f.Tile2i;
              Truck truck2 = truckData.Truck;
              Quantity maxQuantity2 = quantity;
              Lyst<SurfaceDesignation> designationsCache2 = this.m_surfaceDesignationsCache;
              Quantity rhs2;
              ref Quantity local2 = ref rhs2;
              if (designationsManager2.TryFindClosestReadyToPlace(product2, tile2i2, truck2, maxQuantity2, designationsCache2, out local2))
              {
                spec = new BalancingJobSpec(truckData.Truck, lyst, this.m_surfaceDesignationsCache.ToLyst<SurfaceDesignation>(), buffers.Product.WithQuantity(quantity.Min(rhs2)));
                return true;
              }
            }
          }
        }
      }
      foreach (RegisteredInputBuffer buffer in buffers.InputBuffersTmp)
      {
        if (!buffer.RemainingCapacityCached.IsZero && buffer.RawPriorityCached < 16)
        {
          Quantity quantity1 = buffer.OptimalQuantityOrMaxCached.Min(minTruckCapacity);
          if (!(quantity1.Min(data.LeftToClear) > buffer.RemainingCapacityCached))
          {
            foreach (VehicleBuffersRegistry.TruckData truckData in this.m_allTrucksPerProductCache)
            {
              quantity1 = buffer.OptimalQuantityOrMaxCached.Min(truckData.Capacity);
              if (!(quantity1.Min(data.LeftToClear) > buffer.RemainingCapacityCached) && buffer.CanBeServedBy((Vehicle) truckData.Truck, true) && (!truckData.AssignedBuilding.HasValue || truckData.AssignedBuilding.Value == buffer.Entity) && !truckData.UnreachableEntities.Contains((IEntity) buffer.Entity))
              {
                Quantity quantity2 = truckData.Capacity.Min(buffer.RemainingCapacityCached);
                this.m_surfaceDesignationsCache.Clear();
                ISurfaceDesignationsManager designationsManager = this.m_surfaceManager.Value;
                ProductProto product = buffers.Product;
                position2f = truckData.Position2f;
                Tile2i tile2i = position2f.Tile2i;
                Truck truck = truckData.Truck;
                Quantity maxQuantity = quantity2;
                Lyst<SurfaceDesignation> designationsCache = this.m_surfaceDesignationsCache;
                Quantity rhs;
                ref Quantity local = ref rhs;
                if (designationsManager.TryFindClosestReadyToClear(product, tile2i, truck, maxQuantity, designationsCache, out local))
                {
                  spec = new BalancingJobSpec(truckData.Truck, new Lyst<SurfaceDesignation>((IEnumerable<SurfaceDesignation>) this.m_surfaceDesignationsCache), buffer, buffer.Product.WithQuantity(quantity2.Min(rhs)));
                  return true;
                }
              }
            }
          }
        }
      }
      RegisteredInputBuffer valueOrNull = this.TryGetFallbackInputForVehicle(buffers.Product).ValueOrNull;
      if (valueOrNull != null)
      {
        foreach (VehicleBuffersRegistry.TruckData truckData in this.m_allTrucksPerProductCache)
        {
          if (!truckData.UnreachableEntities.Contains((IEntity) valueOrNull.Entity) && (!truckData.AssignedBuilding.HasValue || truckData.AssignedBuilding.Value == valueOrNull.Entity))
          {
            Quantity maxQuantity = truckData.Capacity.Min(valueOrNull.RemainingCapacityCached);
            this.m_surfaceDesignationsCache.Clear();
            Quantity toExchange;
            if (this.m_surfaceManager.Value.TryFindClosestReadyToClear(buffers.Product, valueOrNull.Entity.CenterTile.Tile2i, truckData.Truck, maxQuantity, this.m_surfaceDesignationsCache, out toExchange))
            {
              spec = new BalancingJobSpec(truckData.Truck, new Lyst<SurfaceDesignation>((IEnumerable<SurfaceDesignation>) this.m_surfaceDesignationsCache), valueOrNull, valueOrNull.Product.WithQuantity(maxQuantity.Min(toExchange)));
              return true;
            }
          }
        }
      }
      spec = new BalancingJobSpec();
      return false;
    }

    /// <summary>
    /// Tries to add more nearby targets to an already existing scheduled job.
    /// So if we have a delivery target we try to find other delivery target nearby
    /// as long as we are able to increase reservation of cargo that we can pick up.
    /// We also do the same the opposite way, we try to add multiple pick up
    /// points.
    /// However, there are still strong limitations. The first limitation
    /// is that we only expand delivery or pick up targets, not both at the same time
    /// due to combinatorial costs. Second limitation is that we do this only for
    /// (de)construction / upgrade buffers. Otherwise we would have to go through
    /// the pain of handling assigned routes and all that stuff. Also performance
    /// wise this does not hurt much for constructions because we will definitely
    /// get savings on unnecessary jobs.
    /// </summary>
    private void tryToAddMoreConstructionJobs(
      Truck truck,
      Quantity balancedSoFar,
      VehicleBuffersRegistry.RegisteredBuffers registeredBuffers,
      RegisteredInputBuffer inputBuffer,
      RegisteredOutputBuffer outputBuffer,
      out Option<Lyst<SecondaryInputBufferSpec>> secondaryInputBuffers,
      out Option<Lyst<SecondaryOutputBufferSpec>> secondaryOutputBuffers)
    {
      Lyst<SecondaryInputBufferSpec> tempSecondaryInputBuffers = (Lyst<SecondaryInputBufferSpec>) null;
      Lyst<SecondaryOutputBufferSpec> tempSecondaryOutputBuffers = (Lyst<SecondaryOutputBufferSpec>) null;
      Quantity capacityLeft = truck.Capacity - balancedSoFar;
      if (capacityLeft.IsNotPositive)
      {
        secondaryInputBuffers = (Option<Lyst<SecondaryInputBufferSpec>>) Option.None;
        secondaryOutputBuffers = (Option<Lyst<SecondaryOutputBufferSpec>>) Option.None;
      }
      else
      {
        IReadOnlySet<IEntity> unreachableEntities = this.m_unreachablesManager.Value.GetUnreachableEntitiesFor((IPathFindingVehicle) truck);
        if (inputBuffer.IsConstructionBuffer && !outputBuffer.IsConstructionBuffer)
          tryToAddMoreInputConstructionJobs();
        else if (!inputBuffer.IsConstructionBuffer && outputBuffer.IsConstructionBuffer)
          tryToAddMoreOutputConstructionJobs();
        secondaryInputBuffers = (Option<Lyst<SecondaryInputBufferSpec>>) tempSecondaryInputBuffers;
        secondaryOutputBuffers = (Option<Lyst<SecondaryOutputBufferSpec>>) tempSecondaryOutputBuffers;

        void tryToAddMoreInputConstructionJobs()
        {
          Quantity rhs = outputBuffer.AvailableQuantity.Min(capacityLeft);
          if (rhs.IsNotPositive)
            return;
          foreach (RegisteredInputBuffer inputBuffer in registeredBuffers.InputBuffers)
          {
            if (inputBuffer != inputBuffer && inputBuffer.IsConstructionBuffer && inputBuffer.IsAvailableCached && inputBuffer.OptimalQuantityCached.HasValue && inputBuffer.Entity.Position2f.DistanceSqrTo(inputBuffer.Entity.Position2f) < VehicleBuffersRegistry.MAX_SECONDARY_CARGO_DISTANCE_SQR)
            {
              Quantity quantity = inputBuffer.RemainingCapacityCached.Min(rhs);
              if (!quantity.IsNotPositive && !unreachableEntities.Contains((IEntity) inputBuffer.Entity))
              {
                if (tempSecondaryInputBuffers == null)
                  tempSecondaryInputBuffers = new Lyst<SecondaryInputBufferSpec>();
                tempSecondaryInputBuffers.Add(new SecondaryInputBufferSpec(inputBuffer, quantity));
                rhs -= quantity;
                if (rhs.IsNotPositive)
                  break;
              }
            }
          }
          Assert.That<Quantity>(rhs).IsNotNegative();
        }

        void tryToAddMoreOutputConstructionJobs()
        {
          Quantity rhs = inputBuffer.RemainingCapacity.Min(capacityLeft);
          if (rhs.IsNotPositive)
            return;
          foreach (RegisteredOutputBuffer outputBuffer in registeredBuffers.OutputBuffers)
          {
            if (outputBuffer != outputBuffer && outputBuffer.IsConstructionBuffer && outputBuffer.IsAvailableCached && outputBuffer.OptimalQuantityCached.HasValue && outputBuffer.Entity.Position2f.DistanceSqrTo(outputBuffer.Entity.Position2f) < VehicleBuffersRegistry.MAX_SECONDARY_CARGO_DISTANCE_SQR)
            {
              Quantity quantity = outputBuffer.AvailableQuantityCached.Min(rhs);
              if (!quantity.IsNotPositive && !unreachableEntities.Contains((IEntity) outputBuffer.Entity))
              {
                if (tempSecondaryOutputBuffers == null)
                  tempSecondaryOutputBuffers = new Lyst<SecondaryOutputBufferSpec>();
                tempSecondaryOutputBuffers.Add(new SecondaryOutputBufferSpec(outputBuffer, quantity));
                rhs -= quantity;
                if (rhs.IsNotPositive)
                  break;
              }
            }
          }
          Assert.That<Quantity>(rhs).IsNotNegative();
        }
      }
    }

    public bool TryRegisterInputBuffer(
      IStaticEntity entity,
      IProductBuffer buffer,
      IInputBufferPriorityProvider priorityProvider,
      bool alwaysEnabled = false,
      bool isFallbackOnly = false,
      bool allowDeliveryAtDistanceWhenBlocked = false)
    {
      RegisteredInputBuffer registeredInputBuffer1;
      if (this.m_registeredInputBuffers.TryGetValue(buffer, out registeredInputBuffer1))
      {
        Assert.That<IStaticEntity>(registeredInputBuffer1.Entity).IsEqualTo<IStaticEntity>(entity);
        return false;
      }
      if (!buffer.Product.CanBeLoadedOnTruck)
        return false;
      VehicleBuffersRegistry.RegisteredBuffers buffersPerProduct = this.getOrCreateBuffersPerProduct(buffer.Product);
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity = this.getOrCreateBuffersPerEntity(entity);
      RegisteredInputBuffer registeredInputBuffer2 = new RegisteredInputBuffer(entity, buffer, priorityProvider, alwaysEnabled, isFallbackOnly, allowDeliveryAtDistanceWhenBlocked);
      buffersPerProduct.InputBuffers.AddAssertNew(registeredInputBuffer2);
      buffersPerEntity.InputBuffers.AddAssertNew(registeredInputBuffer2);
      this.m_registeredInputBuffers.Add(buffer, registeredInputBuffer2);
      return true;
    }

    public bool TryUnregisterInputBuffer(IProductBuffer buffer, bool keepCurrentReservations = false)
    {
      RegisteredInputBuffer buffer1;
      if (!this.m_registeredInputBuffers.TryGetValue(buffer, out buffer1))
        return false;
      this.unregisterBuffer(buffer1);
      return true;
    }

    public bool TryRegisterOutputBuffer(
      IStaticEntity entity,
      IProductBuffer buffer,
      IOutputBufferPriorityProvider priorityProvider,
      bool alwaysEnabled = false,
      bool useFallbackIfNeeded = false,
      bool allowPickupAtDistanceWhenBlocked = false)
    {
      RegisteredOutputBuffer registeredOutputBuffer1;
      if (this.m_registeredOutputBuffers.TryGetValue(buffer, out registeredOutputBuffer1))
      {
        Assert.That<IStaticEntity>(registeredOutputBuffer1.Entity).IsEqualTo<IStaticEntity>(entity);
        return false;
      }
      if (!buffer.Product.CanBeLoadedOnTruck)
        return false;
      VehicleBuffersRegistry.RegisteredBuffers buffersPerProduct = this.getOrCreateBuffersPerProduct(buffer.Product);
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity = this.getOrCreateBuffersPerEntity(entity);
      RegisteredOutputBuffer registeredOutputBuffer2 = new RegisteredOutputBuffer(entity, buffer, priorityProvider, alwaysEnabled, useFallbackIfNeeded, allowPickupAtDistanceWhenBlocked);
      buffersPerProduct.OutputBuffers.AddAssertNew(registeredOutputBuffer2);
      buffersPerEntity.OutputBuffers.AddAssertNew(registeredOutputBuffer2);
      this.m_registeredOutputBuffers.Add(buffer, registeredOutputBuffer2);
      return true;
    }

    public bool TryUnregisterOutputBuffer(IProductBuffer buffer)
    {
      RegisteredOutputBuffer buffer1;
      if (!this.m_registeredOutputBuffers.TryGetValue(buffer, out buffer1))
        return false;
      this.unregisterBuffer(buffer1);
      return true;
    }

    private VehicleBuffersRegistry.RegisteredBuffers getOrCreateBuffersPerProduct(
      ProductProto product)
    {
      VehicleBuffersRegistry.RegisteredBuffers buffersPerProduct1;
      if (this.m_registeredBuffersPerProduct.TryGetValue(product, out buffersPerProduct1))
        return buffersPerProduct1;
      VehicleBuffersRegistry.RegisteredBuffers buffersPerProduct2 = new VehicleBuffersRegistry.RegisteredBuffers(product);
      this.m_registeredBuffersPerProduct[product] = buffersPerProduct2;
      return buffersPerProduct2;
    }

    private VehicleBuffersRegistry.RegisteredBuffersPerEntity getOrCreateBuffersPerEntity(
      IStaticEntity entity)
    {
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity1;
      if (this.m_registeredBuffersPerEntity.TryGetValue(entity, out buffersPerEntity1))
        return buffersPerEntity1;
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity2 = new VehicleBuffersRegistry.RegisteredBuffersPerEntity();
      this.m_registeredBuffersPerEntity[entity] = buffersPerEntity2;
      entity.AddObserver((IEntityObserver) this);
      return buffersPerEntity2;
    }

    private void unregisterBuffer(RegisteredInputBuffer buffer, bool keepCurrentReservations = false)
    {
      this.m_registeredBuffersPerProduct[buffer.Product].InputBuffers.RemoveAndAssert(buffer);
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity = this.m_registeredBuffersPerEntity[buffer.Entity];
      buffersPerEntity.InputBuffers.RemoveAndAssert(buffer);
      Assert.That<bool>(this.m_registeredInputBuffers.Remove(buffer.Buffer)).IsTrue();
      if (!keepCurrentReservations)
        buffer.Destroy();
      if (!buffersPerEntity.InputBuffers.IsEmpty || !buffersPerEntity.OutputBuffers.IsEmpty)
        return;
      this.m_unreachablesManager.Value.TryClearUnreachableVehiclesFor((IEntity) buffer.Entity);
    }

    private void unregisterBuffer(RegisteredOutputBuffer buffer)
    {
      this.m_registeredBuffersPerProduct[buffer.Product].OutputBuffers.RemoveAndAssert(buffer);
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity = this.m_registeredBuffersPerEntity[buffer.Entity];
      buffersPerEntity.OutputBuffers.RemoveAndAssert(buffer);
      Assert.That<bool>(this.m_registeredOutputBuffers.Remove(buffer.Buffer)).IsTrue();
      buffer.Destroy();
      if (!buffersPerEntity.InputBuffers.IsEmpty || !buffersPerEntity.OutputBuffers.IsEmpty)
        return;
      this.m_unreachablesManager.Value.TryClearUnreachableVehiclesFor((IEntity) buffer.Entity);
    }

    public Option<RegisteredInputBuffer> TryGetInputBuffer(
      IStaticEntity entity,
      ProductProto product)
    {
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity;
      return !this.m_registeredBuffersPerEntity.TryGetValue(entity, out buffersPerEntity) ? (Option<RegisteredInputBuffer>) Option.None : (Option<RegisteredInputBuffer>) buffersPerEntity.InputBuffers.FirstOrDefault<RegisteredInputBuffer>((Predicate<RegisteredInputBuffer>) (x => (Proto) x.Product == (Proto) product));
    }

    public Option<RegisteredOutputBuffer> TryGetOutputBuffer(
      IStaticEntity entity,
      ProductProto product)
    {
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity;
      return !this.m_registeredBuffersPerEntity.TryGetValue(entity, out buffersPerEntity) ? (Option<RegisteredOutputBuffer>) Option.None : (Option<RegisteredOutputBuffer>) buffersPerEntity.OutputBuffers.FirstOrDefault<RegisteredOutputBuffer>((Predicate<RegisteredOutputBuffer>) (x => (Proto) x.Product == (Proto) product));
    }

    public void ClearAndCancelAllJobs(IStaticEntity entity)
    {
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity;
      if (!this.m_registeredBuffersPerEntity.TryGetValue(entity, out buffersPerEntity))
        return;
      foreach (RegisteredInputBuffer immutable in buffersPerEntity.InputBuffers.ToImmutableArray())
        immutable.ClearAndCancelAllJobs();
      foreach (RegisteredOutputBuffer immutable in buffersPerEntity.OutputBuffers.ToImmutableArray())
        immutable.ClearAndCancelAllJobs();
    }

    public void UnregisterAllBuffers(IStaticEntity entity)
    {
      VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity;
      if (!this.m_registeredBuffersPerEntity.TryGetValue(entity, out buffersPerEntity))
        return;
      foreach (RegisteredInputBuffer immutable in buffersPerEntity.InputBuffers.ToImmutableArray())
        this.unregisterBuffer(immutable);
      foreach (RegisteredOutputBuffer immutable in buffersPerEntity.OutputBuffers.ToImmutableArray())
        this.unregisterBuffer(immutable);
      this.m_registeredBuffersPerEntity.Remove(entity);
      entity.RemoveObserver((IEntityObserver) this);
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      entity.RemoveObserver((IEntityObserver) this);
      if (!(entity is IStaticEntity entity1))
        return;
      this.UnregisterAllBuffers(entity1);
    }

    private static int InputAscendingPriorityComparison(
      RegisteredInputBuffer x,
      RegisteredInputBuffer y)
    {
      return x.CombinedPriorityCached.CompareTo(y.CombinedPriorityCached);
    }

    private static int OutputAscendingPriorityComparison(
      RegisteredOutputBuffer x,
      RegisteredOutputBuffer y)
    {
      return x.CombinedPriorityCached.CompareTo(y.CombinedPriorityCached);
    }

    private static int BuffersAscendingPriorityComparison(
      VehicleBuffersRegistry.RegisteredBuffers x,
      VehicleBuffersRegistry.RegisteredBuffers y)
    {
      return x.SortingPriorityTmp.CompareTo(y.SortingPriorityTmp);
    }

    static VehicleBuffersRegistry()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleBuffersRegistry.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleBuffersRegistry) obj).SerializeData(writer));
      VehicleBuffersRegistry.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleBuffersRegistry) obj).DeserializeData(reader));
      VehicleBuffersRegistry.MAX_SECONDARY_CARGO_DISTANCE_SQR = 30.ToFix64().Squared();
    }

    [GenerateSerializer(false, null, 0)]
    private class RegisteredBuffers
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      public readonly ProductProto Product;
      public readonly Lyst<RegisteredInputBuffer> InputBuffers;
      public readonly Lyst<RegisteredOutputBuffer> OutputBuffers;
      /// <summary>
      /// The last sim step in which these buffers were being rebalanced by logistics.
      /// </summary>
      public SimStep LastProcessedSimStep;
      [DoNotSaveCreateNewOnLoad(null, 0)]
      public readonly List<RegisteredInputBuffer> InputBuffersTmp;
      [DoNotSaveCreateNewOnLoad(null, 0)]
      public readonly List<RegisteredOutputBuffer> OutputBuffersTmp;
      [DoNotSave(0, null)]
      public int SortingPriorityTmp;

      public static void Serialize(
        VehicleBuffersRegistry.RegisteredBuffers value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<VehicleBuffersRegistry.RegisteredBuffers>(value))
          return;
        writer.EnqueueDataSerialization((object) value, VehicleBuffersRegistry.RegisteredBuffers.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Lyst<RegisteredInputBuffer>.Serialize(this.InputBuffers, writer);
        SimStep.Serialize(this.LastProcessedSimStep, writer);
        Lyst<RegisteredOutputBuffer>.Serialize(this.OutputBuffers, writer);
        writer.WriteGeneric<ProductProto>(this.Product);
      }

      public static VehicleBuffersRegistry.RegisteredBuffers Deserialize(BlobReader reader)
      {
        VehicleBuffersRegistry.RegisteredBuffers registeredBuffers;
        if (reader.TryStartClassDeserialization<VehicleBuffersRegistry.RegisteredBuffers>(out registeredBuffers))
          reader.EnqueueDataDeserialization((object) registeredBuffers, VehicleBuffersRegistry.RegisteredBuffers.s_deserializeDataDelayedAction);
        return registeredBuffers;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<VehicleBuffersRegistry.RegisteredBuffers>(this, "InputBuffers", (object) Lyst<RegisteredInputBuffer>.Deserialize(reader));
        reader.SetField<VehicleBuffersRegistry.RegisteredBuffers>(this, "InputBuffersTmp", (object) new List<RegisteredInputBuffer>());
        this.LastProcessedSimStep = SimStep.Deserialize(reader);
        reader.SetField<VehicleBuffersRegistry.RegisteredBuffers>(this, "OutputBuffers", (object) Lyst<RegisteredOutputBuffer>.Deserialize(reader));
        reader.SetField<VehicleBuffersRegistry.RegisteredBuffers>(this, "OutputBuffersTmp", (object) new List<RegisteredOutputBuffer>());
        reader.SetField<VehicleBuffersRegistry.RegisteredBuffers>(this, "Product", (object) reader.ReadGenericAs<ProductProto>());
      }

      public RegisteredBuffers(ProductProto product)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.InputBuffers = new Lyst<RegisteredInputBuffer>();
        this.OutputBuffers = new Lyst<RegisteredOutputBuffer>();
        this.InputBuffersTmp = new List<RegisteredInputBuffer>();
        this.OutputBuffersTmp = new List<RegisteredOutputBuffer>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Product = product;
      }

      static RegisteredBuffers()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        VehicleBuffersRegistry.RegisteredBuffers.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleBuffersRegistry.RegisteredBuffers) obj).SerializeData(writer));
        VehicleBuffersRegistry.RegisteredBuffers.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleBuffersRegistry.RegisteredBuffers) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    private class RegisteredBuffersPerEntity
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      public readonly Lyst<RegisteredInputBuffer> InputBuffers;
      public readonly Lyst<RegisteredOutputBuffer> OutputBuffers;

      public static void Serialize(
        VehicleBuffersRegistry.RegisteredBuffersPerEntity value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<VehicleBuffersRegistry.RegisteredBuffersPerEntity>(value))
          return;
        writer.EnqueueDataSerialization((object) value, VehicleBuffersRegistry.RegisteredBuffersPerEntity.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Lyst<RegisteredInputBuffer>.Serialize(this.InputBuffers, writer);
        Lyst<RegisteredOutputBuffer>.Serialize(this.OutputBuffers, writer);
      }

      public static VehicleBuffersRegistry.RegisteredBuffersPerEntity Deserialize(BlobReader reader)
      {
        VehicleBuffersRegistry.RegisteredBuffersPerEntity buffersPerEntity;
        if (reader.TryStartClassDeserialization<VehicleBuffersRegistry.RegisteredBuffersPerEntity>(out buffersPerEntity))
          reader.EnqueueDataDeserialization((object) buffersPerEntity, VehicleBuffersRegistry.RegisteredBuffersPerEntity.s_deserializeDataDelayedAction);
        return buffersPerEntity;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<VehicleBuffersRegistry.RegisteredBuffersPerEntity>(this, "InputBuffers", (object) Lyst<RegisteredInputBuffer>.Deserialize(reader));
        reader.SetField<VehicleBuffersRegistry.RegisteredBuffersPerEntity>(this, "OutputBuffers", (object) Lyst<RegisteredOutputBuffer>.Deserialize(reader));
      }

      public RegisteredBuffersPerEntity()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.InputBuffers = new Lyst<RegisteredInputBuffer>();
        this.OutputBuffers = new Lyst<RegisteredOutputBuffer>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static RegisteredBuffersPerEntity()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        VehicleBuffersRegistry.RegisteredBuffersPerEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleBuffersRegistry.RegisteredBuffersPerEntity) obj).SerializeData(writer));
        VehicleBuffersRegistry.RegisteredBuffersPerEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleBuffersRegistry.RegisteredBuffersPerEntity) obj).DeserializeData(reader));
      }
    }

    private class TruckDistanceComparer : IComparer<VehicleBuffersRegistry.TruckData>
    {
      private static readonly VehicleBuffersRegistry.TruckDistanceComparer INSTANCE;
      public Tile2f Position;

      /// <summary>
      /// Sorts trucks in ascending order based on their distance to a position.
      /// </summary>
      public static VehicleBuffersRegistry.TruckDistanceComparer OrderByDistanceFrom(Tile2f position)
      {
        VehicleBuffersRegistry.TruckDistanceComparer.INSTANCE.Position = position;
        return VehicleBuffersRegistry.TruckDistanceComparer.INSTANCE;
      }

      public int Compare(VehicleBuffersRegistry.TruckData x, VehicleBuffersRegistry.TruckData y)
      {
        return x.Position2f.DistanceSqrTo(this.Position).CompareTo(y.Position2f.DistanceSqrTo(this.Position));
      }

      public TruckDistanceComparer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static TruckDistanceComparer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        VehicleBuffersRegistry.TruckDistanceComparer.INSTANCE = new VehicleBuffersRegistry.TruckDistanceComparer();
      }
    }

    private struct TruckData
    {
      public readonly Truck Truck;
      public readonly Option<IEntityAssignedWithVehicles> AssignedBuilding;
      public readonly Quantity CapacityWithMinUtilApplied;
      public readonly Quantity Capacity;
      public readonly Tile2f Position2f;
      public readonly IReadOnlySet<IEntity> UnreachableEntities;

      public TruckData(
        Truck truck,
        Percent minTruckCapUtilization,
        IReadOnlySet<IEntity> unreachableEntities)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Truck = truck;
        this.CapacityWithMinUtilApplied = truck.Capacity.ScaledBy(minTruckCapUtilization);
        this.Capacity = truck.Capacity;
        this.AssignedBuilding = (Option<IEntityAssignedWithVehicles>) Option.None;
        this.Position2f = truck.Position2f;
        this.UnreachableEntities = unreachableEntities;
        if (!truck.AssignedTo.HasValue)
          return;
        IEntityAssignedWithVehicles assignedWithVehicles = truck.AssignedTo.Value;
        if (!(assignedWithVehicles is IStaticEntity staticEntity) || staticEntity is MineTower)
          return;
        this.AssignedBuilding = assignedWithVehicles.SomeOption<IEntityAssignedWithVehicles>();
      }
    }

    private class InputBufferDistanceComparator : IComparer<RegisteredInputBuffer>
    {
      private static readonly VehicleBuffersRegistry.InputBufferDistanceComparator INSTANCE;
      private static readonly int PENALTY_FOR_NON_EMPTY_BUFFER;
      private Tile2i m_fromPoint;

      public static VehicleBuffersRegistry.InputBufferDistanceComparator GetInstance(
        Tile2i fromPoint)
      {
        VehicleBuffersRegistry.InputBufferDistanceComparator.INSTANCE.m_fromPoint = fromPoint;
        return VehicleBuffersRegistry.InputBufferDistanceComparator.INSTANCE;
      }

      private InputBufferDistanceComparator(Tile2i fromPoint)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_fromPoint = fromPoint;
      }

      public int Compare(RegisteredInputBuffer x, RegisteredInputBuffer y)
      {
        long lengthSqr1 = (x.Entity.CenterTile.Tile2i - this.m_fromPoint).LengthSqr;
        long lengthSqr2 = (y.Entity.CenterTile.Tile2i - this.m_fromPoint).LengthSqr;
        if (x.Buffer.IsNotEmpty())
          lengthSqr1 += (long) VehicleBuffersRegistry.InputBufferDistanceComparator.PENALTY_FOR_NON_EMPTY_BUFFER;
        if (y.Buffer.IsNotEmpty())
          lengthSqr2 += (long) VehicleBuffersRegistry.InputBufferDistanceComparator.PENALTY_FOR_NON_EMPTY_BUFFER;
        return lengthSqr1.CompareTo(lengthSqr2);
      }

      static InputBufferDistanceComparator()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        VehicleBuffersRegistry.InputBufferDistanceComparator.INSTANCE = new VehicleBuffersRegistry.InputBufferDistanceComparator(Tile2i.Zero);
        VehicleBuffersRegistry.InputBufferDistanceComparator.PENALTY_FOR_NON_EMPTY_BUFFER = 100.Squared();
      }
    }
  }
}
