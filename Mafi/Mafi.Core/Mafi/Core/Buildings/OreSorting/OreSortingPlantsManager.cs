// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.OreSortingPlantsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.OreSorting
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class OreSortingPlantsManager : 
    ICommandProcessor<AddProductToSortCmd>,
    IAction<AddProductToSortCmd>,
    ICommandProcessor<RemoveProductToSortCmd>,
    IAction<RemoveProductToSortCmd>,
    ICommandProcessor<SetProductPortCmd>,
    IAction<SetProductPortCmd>,
    ICommandProcessor<SortingPlantNoSingleProductCmd>,
    IAction<SortingPlantNoSingleProductCmd>,
    ICommandProcessor<SortingPlantSetBlockedProductAlertCmd>,
    IAction<SortingPlantSetBlockedProductAlertCmd>
  {
    private readonly Lyst<OreSortingPlant> m_sortingPlants;
    private readonly ProtosDb m_protosDb;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly IUnreachablesManager m_unreachablesManager;
    private readonly MixedCargoDeliveryJob.Factory m_mixedDeliveryJobFactory;
    [DoNotSave(0, null)]
    private LystStruct<OreSortingPlant> m_plantsCache;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool IsSortingEnabled { get; private set; }

    public OreSortingPlantsManager(
      ProtosDb protosDb,
      IEntitiesManager entitiesManager,
      IUnreachablesManager unreachablesManager,
      IConstructionManager constructionManager,
      IPropertiesDb propsDb,
      MixedCargoDeliveryJob.Factory mixedDeliveryJobFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_sortingPlants = new Lyst<OreSortingPlant>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_entitiesManager = entitiesManager;
      this.m_unreachablesManager = unreachablesManager;
      this.m_mixedDeliveryJobFactory = mixedDeliveryJobFactory;
      constructionManager.EntityConstructed.Add<OreSortingPlantsManager>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<OreSortingPlantsManager>(this, new Action<IStaticEntity>(this.onEntityDeconstructionStarted));
      IProperty<bool> property = propsDb.GetProperty<bool>(IdsCore.PropertyIds.OreSortingEnabled);
      property.OnChange.Add<OreSortingPlantsManager>(this, new Action<bool>(this.onSortingEnabledChange));
      this.IsSortingEnabled = property.Value;
    }

    private void onSortingEnabledChange(bool value) => this.IsSortingEnabled = value;

    private void onEntityConstructed(IStaticEntity entity)
    {
      if (!(entity is OreSortingPlant oreSortingPlant))
        return;
      this.m_sortingPlants.Add(oreSortingPlant);
    }

    private void onEntityDeconstructionStarted(IStaticEntity entity)
    {
      if (!(entity is OreSortingPlant oreSortingPlant))
        return;
      this.m_sortingPlants.TryRemoveReplaceLast(oreSortingPlant);
    }

    public bool IsSortingRequiredFor(Truck truck, bool isMineTruck)
    {
      if (!this.IsSortingEnabled || truck.Cargo.Count <= 0 || truck.Cargo.Count == 1 && !isMineTruck)
        return false;
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        if (keyValuePair.Key.DumpableProduct.IsNone)
          return false;
      }
      return true;
    }

    public bool TryGetMixedDeliveryJobFor(
      Truck truck,
      Option<MineTower> tower,
      out bool hasMatchingPlant,
      out bool isAssignedToPlant)
    {
      this.m_plantsCache.Clear();
      IReadOnlySet<IEntity> unreachableEntitiesFor = this.m_unreachablesManager.GetUnreachableEntitiesFor((IPathFindingVehicle) truck);
      hasMatchingPlant = false;
      isAssignedToPlant = tower.HasValue && tower.Value.AssignedInputOreSorters.IsNotEmpty<OreSortingPlant>();
      if (isAssignedToPlant)
      {
        foreach (OreSortingPlant assignedInputOreSorter in (IEnumerable<OreSortingPlant>) tower.Value.AssignedInputOreSorters)
        {
          bool hadMatchingProducts;
          bool flag = assignedInputOreSorter.CanAcceptTruck(truck, out hadMatchingProducts);
          hasMatchingPlant |= hadMatchingProducts;
          if (flag && !unreachableEntitiesFor.Contains((IEntity) assignedInputOreSorter))
            this.m_plantsCache.Add(assignedInputOreSorter);
        }
      }
      else
      {
        foreach (OreSortingPlant sortingPlant in this.m_sortingPlants)
        {
          if (sortingPlant.AllowNonAssignedOutput || !sortingPlant.AssignedOutputs.IsNotEmpty<IEntityAssignedAsOutput>())
          {
            bool hadMatchingProducts;
            bool flag = sortingPlant.CanAcceptTruck(truck, out hadMatchingProducts);
            hasMatchingPlant |= hadMatchingProducts;
            if (flag && !unreachableEntitiesFor.Contains((IEntity) sortingPlant))
              this.m_plantsCache.Add(sortingPlant);
          }
        }
      }
      if (this.m_plantsCache.IsEmpty)
        return false;
      if (this.m_plantsCache.Count == 1)
      {
        this.m_mixedDeliveryJobFactory.EnqueueJob(truck, this.m_plantsCache.First);
        return true;
      }
      Fix64 fix64_1 = Fix64.MaxIntValue;
      OreSortingPlant targetPlant = this.m_plantsCache.First;
      foreach (OreSortingPlant oreSortingPlant in this.m_plantsCache)
      {
        Fix64 fix64_2 = oreSortingPlant.Position2f.DistanceSqrTo(truck.Position2f);
        if (fix64_2 < fix64_1)
        {
          targetPlant = oreSortingPlant;
          fix64_1 = fix64_2;
        }
      }
      this.m_mixedDeliveryJobFactory.EnqueueJob(truck, targetPlant);
      return true;
    }

    public void Invoke(AddProductToSortCmd cmd)
    {
      OreSortingPlant entity;
      if (!this.m_entitiesManager.TryGetEntity<OreSortingPlant>(cmd.SortingPlantId, out entity))
      {
        cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", (object) cmd.SortingPlantId));
      }
      else
      {
        ProductProto proto;
        if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductId, out proto))
        {
          cmd.SetResultError(string.Format("Product with id {0} not found.", (object) cmd.ProductId));
        }
        else
        {
          entity.AddProductToSort(proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(RemoveProductToSortCmd cmd)
    {
      OreSortingPlant entity;
      if (!this.m_entitiesManager.TryGetEntity<OreSortingPlant>(cmd.SortingPlantId, out entity))
      {
        cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", (object) cmd.SortingPlantId));
      }
      else
      {
        ProductProto proto;
        if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductId, out proto))
        {
          cmd.SetResultError(string.Format("Product with id {0} not found.", (object) cmd.ProductId));
        }
        else
        {
          entity.RemoveProductToSort(proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(SetProductPortCmd cmd)
    {
      OreSortingPlant entity;
      if (!this.m_entitiesManager.TryGetEntity<OreSortingPlant>(cmd.SortingPlantId, out entity))
      {
        cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", (object) cmd.SortingPlantId));
      }
      else
      {
        ProductProto proto;
        if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductId, out proto))
        {
          cmd.SetResultError(string.Format("Product with id {0} not found.", (object) cmd.ProductId));
        }
        else
        {
          entity.SetProductPortIndex(proto, cmd.PortIndex);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(SortingPlantNoSingleProductCmd cmd)
    {
      OreSortingPlant entity;
      if (!this.m_entitiesManager.TryGetEntity<OreSortingPlant>(cmd.SortingPlantId, out entity))
      {
        cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", (object) cmd.SortingPlantId));
      }
      else
      {
        entity.DoNotAcceptSingleProduct = cmd.DoNotAcceptSingleProduct;
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(SortingPlantSetBlockedProductAlertCmd cmd)
    {
      OreSortingPlant entity;
      if (!this.m_entitiesManager.TryGetEntity<OreSortingPlant>(cmd.SortingPlantId, out entity))
      {
        cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", (object) cmd.SortingPlantId));
      }
      else
      {
        ProductProto proto;
        if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductId, out proto))
        {
          cmd.SetResultError(string.Format("Product with id {0} not found.", (object) cmd.ProductId));
        }
        else
        {
          entity.SetProductBlockedAlert(proto, cmd.IsAlertEnabled);
          cmd.SetResultSuccess();
        }
      }
    }

    public static void Serialize(OreSortingPlantsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<OreSortingPlantsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, OreSortingPlantsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsSortingEnabled);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      Lyst<OreSortingPlant>.Serialize(this.m_sortingPlants, writer);
      writer.WriteGeneric<IUnreachablesManager>(this.m_unreachablesManager);
    }

    public static OreSortingPlantsManager Deserialize(BlobReader reader)
    {
      OreSortingPlantsManager sortingPlantsManager;
      if (reader.TryStartClassDeserialization<OreSortingPlantsManager>(out sortingPlantsManager))
        reader.EnqueueDataDeserialization((object) sortingPlantsManager, OreSortingPlantsManager.s_deserializeDataDelayedAction);
      return sortingPlantsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsSortingEnabled = reader.ReadBool();
      reader.SetField<OreSortingPlantsManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.RegisterResolvedMember<OreSortingPlantsManager>(this, "m_mixedDeliveryJobFactory", typeof (MixedCargoDeliveryJob.Factory), true);
      reader.RegisterResolvedMember<OreSortingPlantsManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<OreSortingPlantsManager>(this, "m_sortingPlants", (object) Lyst<OreSortingPlant>.Deserialize(reader));
      reader.SetField<OreSortingPlantsManager>(this, "m_unreachablesManager", (object) reader.ReadGenericAs<IUnreachablesManager>());
    }

    static OreSortingPlantsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      OreSortingPlantsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((OreSortingPlantsManager) obj).SerializeData(writer));
      OreSortingPlantsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((OreSortingPlantsManager) obj).DeserializeData(reader));
    }
  }
}
