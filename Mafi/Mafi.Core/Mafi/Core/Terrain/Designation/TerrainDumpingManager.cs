// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.TerrainDumpingManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TerrainDumpingManager : ITerrainDumpingManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly ProtosDb m_protosDb;
    private readonly ITerrainDesignationsManager m_terrainDesignationManager;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<Tile2i, TerrainDesignation> m_dumpingDesignations;
    private readonly UnreachableTerrainDesignationsManager m_unreachableDesignationsManager;
    private readonly Set<ProductProto> m_productsAllowedToDump;
    /// <summary>
    /// These caches can bring tremendous savings when searching for dumping designations frequently.
    /// 
    /// For instance VehicleBuffersRegistry can search a lot. Because scanning large designated areas
    /// per several output buffers per 30+ vehicles adds up a lot. Even if a product was not globally
    /// dumpable we were scanning every-time all the designations if there is a tower that allows it.
    /// 
    /// Some results on larger save (30+ balanced vehicles at the same time, large designated areas):
    /// No caching: ~1000ms / single jobs balancing of 30+ trucks
    /// Caching eligible designations: ~50ms / -||-
    /// Caching eligible designation per product: ~2-4ms / -||-
    /// 
    /// NOTE: Cache is only valid if product is in m_validCaches
    /// NOTE: Phantom product is used as a key to cache designations without a specific product (all)
    /// </summary>
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<LooseProductProto, Lyst<TerrainDesignation>> m_eligibleDesignationsCached;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<LooseProductProto> m_validCaches;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<TerrainDesignation> m_designationsCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<IEntityAssignedAsInput> m_assignedEntitiesCached;
    [DoNotSave(0, null)]
    private Predicate<TerrainDesignation> m_nonAssignedBufferPredicateCached;
    [DoNotSave(0, null)]
    private Predicate<TerrainDesignation> m_assignedBufferPredicateCached;
    [DoNotSave(0, null)]
    private LystStruct<LooseProductProto> m_cargoCache;

    public static void Serialize(TerrainDumpingManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainDumpingManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainDumpingManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Set<ProductProto>.Serialize(this.m_productsAllowedToDump, writer);
      writer.WriteGeneric<ITerrainDesignationsManager>(this.m_terrainDesignationManager);
      UnreachableTerrainDesignationsManager.Serialize(this.m_unreachableDesignationsManager, writer);
    }

    public static TerrainDumpingManager Deserialize(BlobReader reader)
    {
      TerrainDumpingManager terrainDumpingManager;
      if (reader.TryStartClassDeserialization<TerrainDumpingManager>(out terrainDumpingManager))
        reader.EnqueueDataDeserialization((object) terrainDumpingManager, TerrainDumpingManager.s_deserializeDataDelayedAction);
      return terrainDumpingManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TerrainDumpingManager>(this, "m_assignedEntitiesCached", (object) new Set<IEntityAssignedAsInput>());
      reader.SetField<TerrainDumpingManager>(this, "m_designationsCache", (object) new Lyst<TerrainDesignation>());
      reader.SetField<TerrainDumpingManager>(this, "m_dumpingDesignations", (object) new Dict<Tile2i, TerrainDesignation>());
      reader.SetField<TerrainDumpingManager>(this, "m_eligibleDesignationsCached", (object) new Dict<LooseProductProto, Lyst<TerrainDesignation>>());
      reader.SetField<TerrainDumpingManager>(this, "m_productsAllowedToDump", (object) Set<ProductProto>.Deserialize(reader));
      reader.RegisterResolvedMember<TerrainDumpingManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<TerrainDumpingManager>(this, "m_terrainDesignationManager", (object) reader.ReadGenericAs<ITerrainDesignationsManager>());
      reader.SetField<TerrainDumpingManager>(this, "m_unreachableDesignationsManager", (object) UnreachableTerrainDesignationsManager.Deserialize(reader));
      reader.SetField<TerrainDumpingManager>(this, "m_validCaches", (object) new Set<LooseProductProto>());
      reader.RegisterInitAfterLoad<TerrainDumpingManager>(this, "initAfterLoad", InitPriority.Normal);
    }

    public IEnumerable<TerrainDesignation> DumpingDesignations
    {
      get => (IEnumerable<TerrainDesignation>) this.m_dumpingDesignations.Values;
    }

    public int Count => this.m_dumpingDesignations.Count;

    /// <summary>Products that are allowed to be dumped globally.</summary>
    public IReadOnlySet<ProductProto> ProductsAllowedToDump
    {
      get => (IReadOnlySet<ProductProto>) this.m_productsAllowedToDump;
    }

    [DoNotSave(0, null)]
    public ImmutableArray<LooseProductProto> AllDumpableProducts { get; private set; }

    public TerrainDumpingManager(
      ProtosDb protosDb,
      ITerrainDesignationsManager terrainDesignationManager,
      UnreachableTerrainDesignationsManager unreachableDesignationsManager,
      ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_dumpingDesignations = new Dict<Tile2i, TerrainDesignation>();
      this.m_productsAllowedToDump = new Set<ProductProto>();
      this.m_eligibleDesignationsCached = new Dict<LooseProductProto, Lyst<TerrainDesignation>>();
      this.m_validCaches = new Set<LooseProductProto>();
      this.m_designationsCache = new Lyst<TerrainDesignation>();
      this.m_assignedEntitiesCached = new Set<IEntityAssignedAsInput>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_terrainDesignationManager = terrainDesignationManager;
      this.m_unreachableDesignationsManager = unreachableDesignationsManager;
      foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>) terrainDesignationManager.Designations)
        this.designationAdded(designation);
      this.AllDumpableProducts = protosDb.All<LooseProductProto>().Where<LooseProductProto>((Func<LooseProductProto, bool>) (x => x.CanBeOnTerrain)).ToImmutableArray<LooseProductProto>();
      this.m_productsAllowedToDump.AddRange((IEnumerable<ProductProto>) this.AllDumpableProducts.Where((Func<LooseProductProto, bool>) (x => x.IsDumpedOnTerrainByDefault)));
      this.initPredicatesCache();
      terrainDesignationManager.DesignationAdded.Add<TerrainDumpingManager>(this, new Action<TerrainDesignation>(this.designationAdded));
      terrainDesignationManager.DesignationRemoved.Add<TerrainDumpingManager>(this, new Action<TerrainDesignation>(this.designationRemoved));
      simLoopEvents.UpdateStart.Add<TerrainDumpingManager>(this, new Action(this.simUpdateStart));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad(int saveVersion, DependencyResolver resolver)
    {
      this.AllDumpableProducts = this.m_protosDb.All<LooseProductProto>().Where<LooseProductProto>((Func<LooseProductProto, bool>) (x => x.CanBeOnTerrain)).ToImmutableArray<LooseProductProto>();
      foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>) this.m_terrainDesignationManager.Designations)
        this.designationAdded(designation);
      this.initPredicatesCache();
      if (saveVersion >= 114)
        return;
      resolver.Resolve<ISimLoopEvents>().UpdateStart.Add<TerrainDumpingManager>(this, new Action(this.simUpdateStart));
    }

    private void initPredicatesCache()
    {
      this.m_assignedBufferPredicateCached = new Predicate<TerrainDesignation>(this.isEligibleForAssignedBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_nonAssignedBufferPredicateCached = TerrainDumpingManager.\u003C\u003EO.\u003C0\u003E__isEligibleForNonAssignedBuffer ?? (TerrainDumpingManager.\u003C\u003EO.\u003C0\u003E__isEligibleForNonAssignedBuffer = new Predicate<TerrainDesignation>(TerrainDumpingManager.isEligibleForNonAssignedBuffer));
    }

    private void simUpdateStart() => this.m_validCaches.Clear();

    private void designationAdded(TerrainDesignation d)
    {
      if (!(d.Prototype.Id == IdsCore.TerrainDesignators.DumpingDesignator) && !(d.Prototype.Id == IdsCore.TerrainDesignators.LevelDesignator))
        return;
      this.m_dumpingDesignations.AddAndAssertNew(d.Data.OriginTile, d);
    }

    private void designationRemoved(TerrainDesignation d)
    {
      if (!(d.Prototype.Id == IdsCore.TerrainDesignators.DumpingDesignator) && !(d.Prototype.Id == IdsCore.TerrainDesignators.LevelDesignator))
        return;
      this.m_dumpingDesignations.RemoveAndAssert(d.Data.OriginTile);
    }

    public bool HasEligibleDumpingDesignationsFor(LooseProductProto product)
    {
      return this.getAllEligibleCachedFor(product).IsNotEmpty;
    }

    public Option<TerrainDesignation> GetDumpingDesignationAt(Tile2i coord)
    {
      TerrainDesignation terrainDesignation;
      return !this.m_dumpingDesignations.TryGetValue(TerrainDesignation.GetOrigin(coord), out terrainDesignation) ? Option<TerrainDesignation>.None : (Option<TerrainDesignation>) terrainDesignation;
    }

    public bool TryFindClosestReadyToDump(
      Tile2i position,
      Option<LooseProductProto> product,
      Truck servicingVehicle,
      out TerrainDesignation bestDesignation,
      bool tryIgnoreReservations = false,
      Predicate<TerrainDesignation> predicate = null,
      Lyst<TerrainDesignation> additionalNearbyDesignations = null)
    {
      bool canDumpAllGlobally = true;
      bool canDumpProductGlobally = product.HasValue && this.m_productsAllowedToDump.Contains((ProductProto) product.Value);
      this.m_designationsCache.Clear();
      IReadOnlySet<IDesignation> unreachableDesignationsFor = this.m_unreachableDesignationsManager.GetUnreachableDesignationsFor((IPathFindingVehicle) servicingVehicle);
      if (product.HasValue)
      {
        if (!product.Value.CanBeOnTerrain)
        {
          bestDesignation = (TerrainDesignation) null;
          return false;
        }
        Lyst<TerrainDesignation> eligibleCachedFor = this.getAllEligibleCachedFor(product.Value);
        if (eligibleCachedFor.IsEmpty)
        {
          bestDesignation = (TerrainDesignation) null;
          return false;
        }
        foreach (TerrainDesignation terrainDesignation in eligibleCachedFor)
        {
          if ((!unreachableDesignationsFor.IsNotEmpty<IDesignation>() || !unreachableDesignationsFor.Contains((IDesignation) terrainDesignation)) && (predicate == null || predicate(terrainDesignation)))
            this.m_designationsCache.Add(terrainDesignation);
        }
      }
      else
      {
        this.m_cargoCache.Clear();
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in servicingVehicle.Cargo)
        {
          if (keyValuePair.Key.DumpableProduct.IsNone)
          {
            bestDesignation = (TerrainDesignation) null;
            return false;
          }
          canDumpAllGlobally &= this.m_productsAllowedToDump.Contains(keyValuePair.Key);
          this.m_cargoCache.Add(keyValuePair.Key.DumpableProduct.Value);
        }
        foreach (TerrainDesignation designation in this.getAllEligibleCached())
        {
          if (this.canDumpProductsAtEligibleDesignation(designation, this.m_cargoCache, canDumpAllGlobally) && (!unreachableDesignationsFor.IsNotEmpty<IDesignation>() || !unreachableDesignationsFor.Contains((IDesignation) designation)) && (predicate == null || predicate == null || predicate(designation)))
            this.m_designationsCache.Add(designation);
        }
      }
      ITerrainDesignationsManager designationManager = this.m_terrainDesignationManager;
      Lyst<TerrainDesignation> designationsCache = this.m_designationsCache;
      Tile2i position1 = position;
      Truck servicingVehicle1 = servicingVehicle;
      ref TerrainDesignation local = ref bestDesignation;
      bool flag = tryIgnoreReservations;
      Option<LooseProductProto> productToPrefer = new Option<LooseProductProto>();
      int num = flag ? 1 : 0;
      if (!designationManager.TryFindBestReadyToFulfill((IEnumerable<TerrainDesignation>) designationsCache, position1, (Vehicle) servicingVehicle1, out local, productToPrefer, num != 0, isDumping: true))
        return false;
      if (additionalNearbyDesignations != null)
      {
        foreach (TerrainDesignation designation in this.m_dumpingDesignations.Values)
        {
          if (!designation.IsFulfilled && designation != bestDesignation && designation.OriginTileCoord.DistanceToOrtho(bestDesignation.OriginTileCoord).Value <= 12)
          {
            if (product.HasValue)
            {
              if (this.isDesignationEligible(designation, false) && !this.canDumpProductAtEligibleDesignation(designation, product.Value, canDumpProductGlobally))
                continue;
            }
            else if (!this.canDumpProductsAtEligibleDesignation(designation, this.m_cargoCache, canDumpAllGlobally))
              continue;
            if (predicate == null || predicate(designation))
              additionalNearbyDesignations.Add(designation);
          }
        }
      }
      return true;
    }

    public bool TryFindClosestReadyToDump(
      Tile2i position,
      RegisteredOutputBuffer outputBuffer,
      Option<LooseProductProto> product,
      Truck servicingVehicle,
      out TerrainDesignation bestDesignation,
      Lyst<TerrainDesignation> additionalNearbyDesignations = null)
    {
      Predicate<TerrainDesignation> bufferPredicateCached;
      if (outputBuffer.HasAssignedInputEntities)
      {
        this.m_assignedEntitiesCached.Clear();
        this.m_assignedEntitiesCached.AddRange((IEnumerable<IEntityAssignedAsInput>) outputBuffer.EntityAsAssignee.Value.AssignedInputs);
        bufferPredicateCached = this.m_assignedBufferPredicateCached;
      }
      else
        bufferPredicateCached = this.m_nonAssignedBufferPredicateCached;
      return this.TryFindClosestReadyToDump(outputBuffer.Entity.CenterTile.Tile2i, product, servicingVehicle, out bestDesignation, false, bufferPredicateCached, additionalNearbyDesignations);
    }

    private bool isEligibleForAssignedBuffer(TerrainDesignation designation)
    {
      foreach (IAreaManagingTower managedByTower in designation.ManagedByTowers)
      {
        if (managedByTower is MineTower mineTower && this.m_assignedEntitiesCached.Contains((IEntityAssignedAsInput) mineTower))
          return true;
      }
      return false;
    }

    private static bool isEligibleForNonAssignedBuffer(TerrainDesignation designation)
    {
      if (designation.ManagedByTowers.IsEmpty())
        return true;
      bool flag = false;
      foreach (IAreaManagingTower managedByTower in designation.ManagedByTowers)
      {
        if (managedByTower is MineTower mineTower)
        {
          flag = true;
          if (!mineTower.HasOutputStorageOrTowerAssigned || mineTower.AllowNonAssignedOutput)
            return true;
        }
      }
      return !flag;
    }

    /// <summary>Makes the given product to be allowed to be dumped.</summary>
    public void AddProductToDump(LooseProductProto product)
    {
      Assert.That<LooseProductProto>(product).IsNotNullOrPhantom<LooseProductProto>();
      if (!product.CanBeOnTerrain)
        Log.Warning(string.Format("The given product '{0}' cannot be on terrain!", (object) product.Id));
      else
        this.m_productsAllowedToDump.AddAndAssertNew((ProductProto) product);
    }

    /// <summary>
    /// Removes the given product from the allowed dumpable products.
    /// </summary>
    public void RemoveProductToDump(LooseProductProto product)
    {
      Assert.That<LooseProductProto>(product).IsNotNullOrPhantom<LooseProductProto>();
      this.m_productsAllowedToDump.RemoveAndAssert((ProductProto) product);
    }

    private Lyst<TerrainDesignation> getAllEligibleCached()
    {
      Lyst<TerrainDesignation> allEligibleCached;
      if (!this.m_eligibleDesignationsCached.TryGetValue(LooseProductProto.Phantom, out allEligibleCached))
      {
        allEligibleCached = new Lyst<TerrainDesignation>();
        this.m_eligibleDesignationsCached.Add(LooseProductProto.Phantom, allEligibleCached);
      }
      if (this.m_validCaches.Contains(LooseProductProto.Phantom))
        return allEligibleCached;
      allEligibleCached.Clear();
      foreach (TerrainDesignation designation in this.m_dumpingDesignations.Values)
      {
        if (this.isDesignationEligible(designation, true))
          allEligibleCached.Add(designation);
      }
      this.m_validCaches.Add(LooseProductProto.Phantom);
      return allEligibleCached;
    }

    private Lyst<TerrainDesignation> getAllEligibleCachedFor(LooseProductProto product)
    {
      Lyst<TerrainDesignation> eligibleCachedFor;
      if (!this.m_eligibleDesignationsCached.TryGetValue(product, out eligibleCachedFor))
      {
        eligibleCachedFor = new Lyst<TerrainDesignation>();
        this.m_eligibleDesignationsCached.Add(product, eligibleCachedFor);
      }
      if (this.m_validCaches.Contains(product))
        return eligibleCachedFor;
      eligibleCachedFor.Clear();
      bool canDumpProductGlobally = this.m_productsAllowedToDump.Contains((ProductProto) product);
      foreach (TerrainDesignation designation in this.getAllEligibleCached())
      {
        if (this.canDumpProductAtEligibleDesignation(designation, product, canDumpProductGlobally))
          eligibleCachedFor.Add(designation);
      }
      this.m_validCaches.Add(product);
      return eligibleCachedFor;
    }

    private bool isDesignationEligible(TerrainDesignation designation, bool requiredReady)
    {
      return !designation.IsFulfilled && !(!designation.IsDumpingReadyToBeFulfilled & requiredReady) && (!(designation.Prototype.Id == IdsCore.TerrainDesignators.LevelDesignator) || !designation.IsDumpingFulfilled);
    }

    private bool canDumpProductAtEligibleDesignation(
      TerrainDesignation designation,
      LooseProductProto product,
      bool canDumpProductGlobally)
    {
      if (designation.ManagedByTowers.IsEmpty())
        return canDumpProductGlobally;
      bool flag = false;
      foreach (IAreaManagingTower managedByTower in designation.ManagedByTowers)
      {
        if (managedByTower is MineTower mineTower && mineTower.IsEnabled)
        {
          flag = true;
          if (mineTower.DumpableProducts.Contains((ProductProto) product))
            return true;
        }
      }
      return !flag & canDumpProductGlobally;
    }

    private bool canDumpProductsAtEligibleDesignation(
      TerrainDesignation designation,
      LystStruct<LooseProductProto> products,
      bool canDumpAllGlobally)
    {
      if (designation.ManagedByTowers.IsEmpty())
        return canDumpAllGlobally;
      bool flag1 = false;
      foreach (IAreaManagingTower managedByTower in designation.ManagedByTowers)
      {
        if (managedByTower is MineTower mineTower && mineTower.IsEnabled)
        {
          flag1 = true;
          bool flag2 = true;
          foreach (LooseProductProto product in products)
          {
            if (!mineTower.DumpableProducts.Contains((ProductProto) product))
              flag2 = false;
          }
          if (flag2)
            return true;
        }
      }
      return !flag1 & canDumpAllGlobally;
    }

    static TerrainDumpingManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainDumpingManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainDumpingManager) obj).SerializeData(writer));
      TerrainDumpingManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainDumpingManager) obj).DeserializeData(reader));
    }
  }
}
