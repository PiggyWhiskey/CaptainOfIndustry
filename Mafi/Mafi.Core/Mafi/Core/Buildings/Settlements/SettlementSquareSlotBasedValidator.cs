// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementSquareSlotBasedValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  [GenerateSerializer(false, null, 0)]
  internal class SettlementSquareSlotBasedValidator : 
    LayoutEntitySlotBasedValidatorBase<ISettlementSquareModuleProto>,
    IEntityRemovalValidator<ISettlementSquareModule>,
    IEntityRemovalValidator
  {
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly SettlementsManager m_settlementsManager;
    private readonly DependencyResolver m_resolver;
    private readonly Dict<Tile2i, ISettlementSquareModule> m_modules;
    private readonly Dict<Tile2i, Lyst<Tile2i>> m_modulesGraph;
    private readonly Dict<ISettlementSquareModule, int> m_attachedSlots;
    [DoNotSave(0, null)]
    private Stak<Tile2i> m_dfsToVisit;
    [DoNotSave(0, null)]
    private Set<Tile2i> m_dfsVisited;
    [DoNotSave(0, null)]
    private ImmutableArray<LayoutEntitySlot> m_availableSlots;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SettlementSquareSlotBasedValidator(
      IEntitiesManager entitiesManager,
      TerrainOccupancyManager occupancyManager,
      SettlementsManager settlementsManager,
      DependencyResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_modules = new Dict<Tile2i, ISettlementSquareModule>();
      this.m_modulesGraph = new Dict<Tile2i, Lyst<Tile2i>>();
      this.m_attachedSlots = new Dict<ISettlementSquareModule, int>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_occupancyManager = occupancyManager;
      this.m_settlementsManager = settlementsManager;
      this.m_resolver = resolver;
      entitiesManager.StaticEntityAdded.Add<SettlementSquareSlotBasedValidator>(this, new Action<IStaticEntity>(this.staticEntityAdded));
      entitiesManager.StaticEntityRemoved.Add<SettlementSquareSlotBasedValidator>(this, new Action<IStaticEntity>(this.staticEntityRemoved));
    }

    private void staticEntityAdded(IStaticEntity entity)
    {
      if (entity is ISettlementSquareModule settlementSquareModule)
      {
        this.m_modules.AddAndAssertNew(settlementSquareModule.Transform.Position.Tile2i, settlementSquareModule);
        this.connectToGraph(settlementSquareModule);
        this.m_attachedSlots[settlementSquareModule] = 0;
        this.m_availableSlots = new ImmutableArray<LayoutEntitySlot>();
      }
      else
      {
        ISettlementSquareModule nbrHm;
        if (!(entity.Prototype is ISettlementModuleProto) || !this.getAttachedSquare(entity, out nbrHm))
          return;
        nbrHm.Settlement.Value.AddSlotModule(entity);
        ++this.m_attachedSlots[nbrHm];
      }
    }

    private bool getAttachedSquare(IStaticEntity entity, out ISettlementSquareModule nbrHm)
    {
      Assert.That<bool>(entity.Prototype is ISettlementModuleProto).IsTrue();
      ILayoutEntity layoutEntity = (ILayoutEntity) entity;
      Tile3i position = layoutEntity.Transform.Position + new RelTile2i(-layoutEntity.Transform.Rotation.DirectionVector * layoutEntity.Prototype.Layout.LayoutSize.X).ExtendZ(0);
      IStaticEntity entity1;
      if (!this.m_occupancyManager.TryGetOccupyingEntityAt<IStaticEntity>(position, out entity1))
      {
        Log.Error(string.Format("Settlement next to slot module '{0}' {1} ", (object) entity, (object) layoutEntity.Transform.Position) + string.Format("was not found at {0}.", (object) position));
        nbrHm = (ISettlementSquareModule) null;
        return false;
      }
      if (!(entity1 is ISettlementSquareModule settlementSquareModule))
      {
        Log.Error(string.Format("Entity next to slot module '{0}' is not settlement square but '{1}'", (object) entity, (object) entity1.Value));
        nbrHm = (ISettlementSquareModule) null;
        return false;
      }
      nbrHm = settlementSquareModule;
      return true;
    }

    private void connectToGraph(ISettlementSquareModule hm)
    {
      Lyst<Tile2i> neighbors = new Lyst<Tile2i>();
      this.getNeighbors(hm, neighbors);
      Tile2i xy = hm.Transform.Position.Xy;
      this.m_modulesGraph[xy] = neighbors;
      foreach (Tile2i key in neighbors)
      {
        Assert.That<Tile2i>(key).IsNotEqualTo<Tile2i>(xy);
        this.m_modulesGraph[key].Add(xy);
      }
      if (neighbors.IsEmpty)
      {
        this.m_resolver.Instantiate<Settlement>().AddSquareModule(hm);
      }
      else
      {
        Lyst<Settlement> lyst = ((IEnumerable<Settlement>) neighbors.Select<Settlement>(new Func<Tile2i, Settlement>(this.findSettlementFor))).Distinct<Settlement>().ToLyst<Settlement>();
        Settlement first;
        if (lyst.Count > 1)
        {
          first = lyst.First;
          foreach (Settlement other in lyst)
          {
            if (first != other)
              first.MergeIn(other);
          }
        }
        else
          first = lyst.First;
        first.AddSquareModule(hm);
      }
      this.updateDecorations();
    }

    private Settlement findSettlementFor(Tile2i key)
    {
      return this.m_settlementsManager.Settlements.First<Settlement>((Func<Settlement, bool>) (x => x.SquareModules.Any<ISettlementSquareModule>((Func<ISettlementSquareModule, bool>) (hm => key == hm.Transform.Position.Xy))));
    }

    private void getNeighbors(ISettlementSquareModule hm, Lyst<Tile2i> neighbors)
    {
      RelTile3i layoutSize = hm.Prototype.Layout.LayoutSize;
      Assert.That<int>(layoutSize.X).IsEqualTo(layoutSize.Y, "Square module is not square :).");
      foreach (Direction90 allFourDirection in Direction90.AllFourDirections)
      {
        Tile2i key = hm.Transform.Position.Xy + allFourDirection.ToTileDirection() * layoutSize.X;
        if (this.m_modulesGraph.ContainsKey(key))
          neighbors.AddAssertNew(key);
      }
    }

    private void getNeighborsInRange(ISettlementSquareModule hm, int range, Lyst<Tile2i> neighbors)
    {
      RelTile3i layoutSize = hm.Prototype.Layout.LayoutSize;
      Assert.That<int>(layoutSize.X).IsEqualTo(layoutSize.Y, "Square module is not square :).");
      for (int x = -range; x <= range; ++x)
      {
        for (int y = -range; y <= range; ++y)
        {
          if (x != 0 || y != 0)
          {
            Tile2i key = hm.Transform.Position.Xy + new RelTile2i(x, y) * layoutSize.X;
            if (this.m_modulesGraph.ContainsKey(key))
              neighbors.AddAssertNew(key);
          }
        }
      }
    }

    private void staticEntityRemoved(IStaticEntity entity)
    {
      if (entity is ISettlementSquareModule settlementSquareModule)
      {
        this.removeFromGraph(settlementSquareModule);
        this.m_modules.RemoveAndAssert(settlementSquareModule.Transform.Position.Tile2i);
        Assert.That<int>(this.m_attachedSlots[settlementSquareModule]).IsEqualTo(0, "Removing settlement square that has attached modules?");
        this.m_attachedSlots.RemoveAndAssert(settlementSquareModule);
        this.m_availableSlots = new ImmutableArray<LayoutEntitySlot>();
      }
      else
      {
        ISettlementSquareModule nbrHm;
        if (!(entity.Prototype is ISettlementModuleProto) || !this.getAttachedSquare(entity, out nbrHm))
          return;
        --this.m_attachedSlots[nbrHm];
      }
    }

    private void removeFromGraph(ISettlementSquareModule hm)
    {
      Tile2i xy = hm.Transform.Position.Xy;
      Lyst<Tile2i> lyst;
      this.m_modulesGraph.TryRemove(xy, out lyst).AssertTrue();
      foreach (Tile2i key in lyst)
        this.m_modulesGraph[key].RemoveAndAssert(xy);
      this.updateDecorations();
    }

    public override EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      return EntityValidationResult.Success;
    }

    public EntityValidationResult CanRemove(ISettlementSquareModule hm, EntityRemoveReason reason)
    {
      int num;
      if (this.m_attachedSlots.TryGetValue(hm, out num))
      {
        if (num > 0)
          return EntityValidationResult.CreateError((LocStrFormatted) TrCore.RemovalError__HousingHasModuleAttached);
      }
      else
        Log.Error("Unknown settlement.");
      if (this.m_dfsToVisit == null)
      {
        this.m_dfsToVisit = new Stak<Tile2i>(2 * this.m_modules.Count);
        this.m_dfsVisited = new Set<Tile2i>(2 * this.m_modules.Count);
      }
      else
      {
        this.m_dfsToVisit.Clear();
        this.m_dfsVisited.Clear();
      }
      Tile2i xy = hm.Transform.Position.Xy;
      foreach (Tile2i key in this.m_modulesGraph[xy])
      {
        ISettlementSquareModule module = this.m_modules[key];
        if (module.ConstructionState != ConstructionState.PendingDeconstruction && module.ConstructionState != ConstructionState.InDeconstruction)
        {
          this.m_dfsToVisit.Push(key);
          break;
        }
      }
      while (this.m_dfsToVisit.IsNotEmpty)
      {
        Tile2i tile2i1 = this.m_dfsToVisit.Pop();
        this.m_dfsVisited.Add(tile2i1);
        ISettlementSquareModule module = this.m_modules[tile2i1];
        if (module.ConstructionState != ConstructionState.PendingDeconstruction && module.ConstructionState != ConstructionState.InDeconstruction)
        {
          foreach (Tile2i tile2i2 in this.m_modulesGraph[tile2i1])
          {
            Assert.That<Tile2i>(tile2i2).IsNotEqualTo<Tile2i>(tile2i1, "Square neighboring with itself?");
            if (tile2i2 != xy && !this.m_dfsVisited.Contains(tile2i2))
              this.m_dfsToVisit.Push(tile2i2);
          }
        }
      }
      Assert.That<Set<Tile2i>>(this.m_dfsVisited).NotContains<Tile2i>(xy);
      return this.m_dfsVisited.Count != hm.Settlement.Value.SquareModules.Count - 1 ? EntityValidationResult.CreateError((LocStrFormatted) TrCore.RemovalError__NotContiguous) : EntityValidationResult.Success;
    }

    private void updateDecorations()
    {
      foreach (ISettlementSquareModule settlementSquareModule in this.m_modules.Values)
      {
        if (settlementSquareModule is SettlementHousingModule settlementHousingModule)
          settlementHousingModule.ClearDecorations();
      }
      Lyst<Tile2i> neighbors = new Lyst<Tile2i>();
      foreach (ISettlementSquareModule settlementSquareModule in this.m_modules.Values)
      {
        if (settlementSquareModule is SettlementDecorationModule decorationModule)
        {
          neighbors.Clear();
          this.getNeighborsInRange((ISettlementSquareModule) decorationModule, decorationModule.Prototype.BonusRange, neighbors);
          foreach (Tile2i key in neighbors)
          {
            if (this.m_modules[key] is SettlementHousingModule module)
              module.AddDecoration(decorationModule);
          }
        }
      }
    }

    public override ImmutableArray<LayoutEntitySlot> GetAvailableSlots(
      ISettlementSquareModuleProto proto)
    {
      if (this.m_availableSlots.IsValid)
        return this.m_availableSlots;
      Dict<Tile3i, LayoutEntitySlot> dict = new Dict<Tile3i, LayoutEntitySlot>();
      RelTile3i layoutSize = proto.Layout.LayoutSize;
      RelTile2i xy = layoutSize.Xy;
      Assert.That<int>(xy.X).IsEqualTo(xy.Y, "Square module is not square :)");
      foreach (ISettlementSquareModule settlementSquareModule in this.m_modules.Values)
      {
        layoutSize = settlementSquareModule.Prototype.Layout.LayoutSize;
        Assert.That<RelTile2i>(layoutSize.Xy).IsEqualTo<RelTile2i>(xy);
        foreach (Direction90 allFourDirection in Direction90.AllFourDirections)
        {
          Tile3i tile3i = settlementSquareModule.Transform.Position + (allFourDirection.ToTileDirection() * xy.X).ExtendZ(0);
          dict[tile3i] = new LayoutEntitySlot(new TileTransform(tile3i), true);
        }
      }
      foreach (ISettlementSquareModule settlementSquareModule in this.m_modules.Values)
        dict.Remove(settlementSquareModule.Transform.Position);
      this.m_availableSlots = dict.Values.ToImmutableArray<LayoutEntitySlot>();
      return this.m_availableSlots;
    }

    public static void Serialize(SettlementSquareSlotBasedValidator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementSquareSlotBasedValidator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementSquareSlotBasedValidator.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Dict<ISettlementSquareModule, int>.Serialize(this.m_attachedSlots, writer);
      Dict<Tile2i, ISettlementSquareModule>.Serialize(this.m_modules, writer);
      Dict<Tile2i, Lyst<Tile2i>>.Serialize(this.m_modulesGraph, writer);
      TerrainOccupancyManager.Serialize(this.m_occupancyManager, writer);
      DependencyResolver.Serialize(this.m_resolver, writer);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
    }

    public static SettlementSquareSlotBasedValidator Deserialize(BlobReader reader)
    {
      SettlementSquareSlotBasedValidator slotBasedValidator;
      if (reader.TryStartClassDeserialization<SettlementSquareSlotBasedValidator>(out slotBasedValidator))
        reader.EnqueueDataDeserialization((object) slotBasedValidator, SettlementSquareSlotBasedValidator.s_deserializeDataDelayedAction);
      return slotBasedValidator;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SettlementSquareSlotBasedValidator>(this, "m_attachedSlots", (object) Dict<ISettlementSquareModule, int>.Deserialize(reader));
      reader.SetField<SettlementSquareSlotBasedValidator>(this, "m_modules", (object) Dict<Tile2i, ISettlementSquareModule>.Deserialize(reader));
      reader.SetField<SettlementSquareSlotBasedValidator>(this, "m_modulesGraph", (object) Dict<Tile2i, Lyst<Tile2i>>.Deserialize(reader));
      reader.SetField<SettlementSquareSlotBasedValidator>(this, "m_occupancyManager", (object) TerrainOccupancyManager.Deserialize(reader));
      reader.SetField<SettlementSquareSlotBasedValidator>(this, "m_resolver", (object) DependencyResolver.Deserialize(reader));
      reader.SetField<SettlementSquareSlotBasedValidator>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
    }

    static SettlementSquareSlotBasedValidator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementSquareSlotBasedValidator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LayoutEntitySlotBasedValidatorBase<ISettlementSquareModuleProto>) obj).SerializeData(writer));
      SettlementSquareSlotBasedValidator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LayoutEntitySlotBasedValidatorBase<ISettlementSquareModuleProto>) obj).DeserializeData(reader));
    }
  }
}
