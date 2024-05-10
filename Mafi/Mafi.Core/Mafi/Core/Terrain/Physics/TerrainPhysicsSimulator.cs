// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Physics.TerrainPhysicsSimulator
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
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Physics
{
  [GenerateSerializer(false, null, 0)]
  internal class TerrainPhysicsSimulator : ITerrainPhysicsSimulator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MIN_HEIGHT_DIFF_FOR_COLLAPSE;
    private static readonly HeightTilesF MAX_HEIGHT_AT_BOUNDARY;
    private readonly IRandom m_random;
    private readonly Queueue<Tile2iAndIndex> m_tilesToUpdate;
    private TerrainManager m_terrainManager;
    private TileFlagReporter m_disabledPhysicsReporter;
    private TileFlagReporter m_isInQueueReporter;
    private uint m_tileMaskIgnore;
    private uint m_tileMaskIgnoreOrAlreadyQueued;
    [NewInSaveVersion(140, null, null, null, null)]
    private uint m_tileMaskAlreadyQueued;
    [NewInSaveVersion(130, null, null, null, null)]
    private int m_cappedAtMaxStreak;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private Set<Tile2iAndIndex> m_uphillTilesToUpdateRead;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private Set<Tile2iAndIndex> m_uphillTilesToUpdateWrite;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<Tile2i, int> m_disabledPhysicsTiles;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<IStaticEntity> m_entitiesDisablingTerrainPhysics;
    [DoNotSave(0, null)]
    private ImmutableArray<MinMaxPair<ThicknessTilesF>> m_collapseHeightDiffsCache;
    [DoNotSave(0, null)]
    private ImmutableArray<bool> m_disruptAfterCollapseCache;

    public static void Serialize(TerrainPhysicsSimulator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainPhysicsSimulator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainPhysicsSimulator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsDisabled);
      writer.WriteInt(this.m_cappedAtMaxStreak);
      TileFlagReporter.Serialize(this.m_disabledPhysicsReporter, writer);
      TileFlagReporter.Serialize(this.m_isInQueueReporter, writer);
      writer.WriteGeneric<IRandom>(this.m_random);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      writer.WriteUInt(this.m_tileMaskAlreadyQueued);
      writer.WriteUInt(this.m_tileMaskIgnore);
      writer.WriteUInt(this.m_tileMaskIgnoreOrAlreadyQueued);
      Queueue<Tile2iAndIndex>.Serialize(this.m_tilesToUpdate, writer);
      Set<Tile2iAndIndex>.Serialize(this.m_uphillTilesToUpdateRead, writer);
      Set<Tile2iAndIndex>.Serialize(this.m_uphillTilesToUpdateWrite, writer);
    }

    public static TerrainPhysicsSimulator Deserialize(BlobReader reader)
    {
      TerrainPhysicsSimulator physicsSimulator;
      if (reader.TryStartClassDeserialization<TerrainPhysicsSimulator>(out physicsSimulator))
        reader.EnqueueDataDeserialization((object) physicsSimulator, TerrainPhysicsSimulator.s_deserializeDataDelayedAction);
      return physicsSimulator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsDisabled = reader.LoadedSaveVersion >= 140 && reader.ReadBool();
      this.m_cappedAtMaxStreak = reader.LoadedSaveVersion >= 130 ? reader.ReadInt() : 0;
      this.m_disabledPhysicsReporter = TileFlagReporter.Deserialize(reader);
      reader.SetField<TerrainPhysicsSimulator>(this, "m_disabledPhysicsTiles", (object) new Dict<Tile2i, int>());
      reader.SetField<TerrainPhysicsSimulator>(this, "m_entitiesDisablingTerrainPhysics", (object) new Set<IStaticEntity>());
      this.m_isInQueueReporter = TileFlagReporter.Deserialize(reader);
      reader.SetField<TerrainPhysicsSimulator>(this, "m_random", (object) reader.ReadGenericAs<IRandom>());
      this.m_terrainManager = TerrainManager.Deserialize(reader);
      this.m_tileMaskAlreadyQueued = reader.LoadedSaveVersion >= 140 ? reader.ReadUInt() : 0U;
      this.m_tileMaskIgnore = reader.ReadUInt();
      this.m_tileMaskIgnoreOrAlreadyQueued = reader.ReadUInt();
      reader.SetField<TerrainPhysicsSimulator>(this, "m_tilesToUpdate", (object) Queueue<Tile2iAndIndex>.Deserialize(reader));
      this.m_uphillTilesToUpdateRead = reader.LoadedSaveVersion >= 140 ? Set<Tile2iAndIndex>.Deserialize(reader) : new Set<Tile2iAndIndex>();
      this.m_uphillTilesToUpdateWrite = reader.LoadedSaveVersion >= 140 ? Set<Tile2iAndIndex>.Deserialize(reader) : new Set<Tile2iAndIndex>();
      reader.RegisterInitAfterLoad<TerrainPhysicsSimulator>(this, "initAfterLoad", InitPriority.Normal);
    }

    [NewInSaveVersion(140, null, null, null, null)]
    public bool IsDisabled { get; private set; }

    public bool IsProcessingTiles => this.m_tilesToUpdate.IsNotEmpty;

    public TerrainPhysicsSimulator(
      RandomProvider randomProvider,
      IEntitiesManager entitiesManager,
      IConstructionManager constructionManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_tilesToUpdate = new Queueue<Tile2iAndIndex>(true);
      this.m_uphillTilesToUpdateRead = new Set<Tile2iAndIndex>();
      this.m_uphillTilesToUpdateWrite = new Set<Tile2iAndIndex>();
      this.m_disabledPhysicsTiles = new Dict<Tile2i, int>();
      this.m_entitiesDisablingTerrainPhysics = new Set<IStaticEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_random = randomProvider.GetSimRandomFor((object) this);
      constructionManager.EntityConstructed.Add<TerrainPhysicsSimulator>(this, new Action<IStaticEntity>(this.registerStaticEntityPhysicsConstraints));
      entitiesManager.StaticEntityRemoved.Add<TerrainPhysicsSimulator>(this, new Action<IStaticEntity>(this.unregisterStaticEntityPhysicsConstraints));
      entitiesManager.OnUpgradeToBePerformed.Add<TerrainPhysicsSimulator>(this, new Action<IUpgradableEntity>(this.unregisterStaticEntityPhysicsConstraints));
      entitiesManager.OnUpgradeJustPerformed.Add<TerrainPhysicsSimulator>(this, new Action<IUpgradableEntity, IEntityProto>(this.registerStaticEntityPhysicsConstraintsForUpgrade));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad(DependencyResolver resolver)
    {
      Assert.That<Dict<Tile2i, int>>(this.m_disabledPhysicsTiles).IsEmpty<Tile2i, int>();
      this.initializeCachedValues();
      foreach (Tile2iAndIndex tile2iAndIndex in this.m_tilesToUpdate)
        this.m_isInQueueReporter.SetFlagNoEvents(tile2iAndIndex.Index);
      this.m_tileMaskAlreadyQueued = this.m_isInQueueReporter.FlagMask;
      foreach (IEntity entity1 in resolver.Resolve<IEntitiesManager>().Entities)
      {
        if (entity1 is IStaticEntity entity2 && entity2.IsConstructed)
          this.registerStaticEntityPhysicsConstraints(entity2);
      }
    }

    public void Initialize(TerrainManager terrainManager)
    {
      this.m_terrainManager = terrainManager;
      this.initializeCachedValues();
      this.m_isInQueueReporter = terrainManager.CreateNewTileFlagReporter("Physics: in queue", false, false, false);
      this.m_disabledPhysicsReporter = terrainManager.CreateNewTileFlagReporter("Physics: disabled", false, false, false);
      this.m_tileMaskIgnore = 1U | this.m_disabledPhysicsReporter.FlagMask;
      this.m_tileMaskAlreadyQueued = this.m_isInQueueReporter.FlagMask;
      this.m_tileMaskIgnoreOrAlreadyQueued = this.m_tileMaskIgnore | this.m_tileMaskAlreadyQueued;
      this.m_terrainManager.HeightChanged.Add<TerrainPhysicsSimulator>(this, new Action<Tile2iAndIndex>(this.tryQueueNewTile));
    }

    public void SetDisabled(bool isDisabled)
    {
      if (this.m_terrainManager == null)
      {
        Log.Error("Not initialized yet.");
      }
      else
      {
        if (isDisabled == this.IsDisabled)
          return;
        this.IsDisabled = isDisabled;
        if (isDisabled)
          this.m_terrainManager.HeightChanged.Remove<TerrainPhysicsSimulator>(this, new Action<Tile2iAndIndex>(this.tryQueueNewTile));
        else
          this.m_terrainManager.HeightChanged.Add<TerrainPhysicsSimulator>(this, new Action<Tile2iAndIndex>(this.tryQueueNewTile));
      }
    }

    private void initializeCachedValues()
    {
      ImmutableArray<TerrainMaterialProto> terrainMaterials = this.m_terrainManager.TerrainMaterials;
      this.m_collapseHeightDiffsCache = terrainMaterials.Map<MinMaxPair<ThicknessTilesF>>((Func<TerrainMaterialProto, MinMaxPair<ThicknessTilesF>>) (x =>
      {
        ThicknessTilesF min = x.MinCollapseHeightDiff;
        ThicknessTilesF max = x.MaxCollapseHeightDiff;
        if (min < TerrainPhysicsSimulator.MIN_HEIGHT_DIFF_FOR_COLLAPSE)
        {
          Log.Warning(string.Format("Material '{0}' has too small min collapse diff {1}, ", (object) x.Id, (object) min) + string.Format("setting to {0}.", (object) TerrainPhysicsSimulator.MIN_HEIGHT_DIFF_FOR_COLLAPSE));
          min = TerrainPhysicsSimulator.MIN_HEIGHT_DIFF_FOR_COLLAPSE;
        }
        if (max < min)
        {
          Log.Warning(string.Format("Material '{0}' has max collapse height diff {1} smaller than ", (object) x.Id, (object) max) + string.Format("min {0}. Setting to {1}", (object) min, (object) min));
          max = min;
        }
        return new MinMaxPair<ThicknessTilesF>(min, max);
      }));
      terrainMaterials = this.m_terrainManager.TerrainMaterials;
      this.m_disruptAfterCollapseCache = terrainMaterials.Map<bool>((Func<TerrainMaterialProto, bool>) (x => x.DisruptWhenCollapsing));
    }

    private void tryQueueNewTile(Tile2iAndIndex tileAndIndex)
    {
      if (this.m_terrainManager.HasAnyTileFlagSet(tileAndIndex.Index, this.m_tileMaskIgnoreOrAlreadyQueued))
        return;
      this.m_tilesToUpdate.Enqueue(tileAndIndex);
      this.m_isInQueueReporter.SetFlagNoEvents(tileAndIndex.Index);
    }

    private void tryQueueNewUphillTile(Tile2iAndIndex tileAndIndex)
    {
      if (this.m_terrainManager.HasAnyTileFlagSet(tileAndIndex.Index, this.m_tileMaskIgnore))
        return;
      this.m_uphillTilesToUpdateWrite.Add(tileAndIndex);
      this.m_isInQueueReporter.SetFlagNoEvents(tileAndIndex.Index);
    }

    public void StartPhysicsSimulationAt(Tile2iAndIndex tileAndIndex)
    {
      this.tryQueueNewTile(tileAndIndex);
    }

    public void StopPhysicsSimulationAt(Tile2iAndIndex tileAndIndex)
    {
      if (!this.m_isInQueueReporter.ClearFlagNoEventsReportChanged(tileAndIndex.Index))
        return;
      this.m_tilesToUpdate.TryRemove(tileAndIndex).AssertTrue();
    }

    public int GetQueueSize() => this.m_tilesToUpdate.Count;

    private void registerStaticEntityPhysicsConstraintsForUpgrade(
      IStaticEntity entity,
      IEntityProto previousProto)
    {
      this.registerStaticEntityPhysicsConstraints(entity);
    }

    private void registerStaticEntityPhysicsConstraints(IStaticEntity entity)
    {
      Assert.That<TileFlagReporter>(this.m_disabledPhysicsReporter).IsNotNull<TileFlagReporter>();
      if (!entity.OccupiedVerticesCombinedConstraint.HasAnyConstraints(LayoutTileConstraint.DisableTerrainPhysics) || !this.m_entitiesDisablingTerrainPhysics.Add(entity))
        return;
      Tile2i xy = entity.CenterTile.Xy;
      foreach (OccupiedVertexRelative occupiedVertex in entity.OccupiedVertices)
      {
        if (occupiedVertex.Constraint.HasAnyConstraints(LayoutTileConstraint.DisableTerrainPhysics))
        {
          Tile2i tile2i = xy + occupiedVertex.RelCoord;
          if (this.m_disabledPhysicsTiles.IncOrInsert1<Tile2i>(tile2i) == 1)
            this.m_disabledPhysicsReporter.SetFlag(this.m_terrainManager.ExtendTileIndex(tile2i));
        }
      }
    }

    private void unregisterStaticEntityPhysicsConstraints(IStaticEntity entity)
    {
      if (!entity.OccupiedVerticesCombinedConstraint.HasAnyConstraints(LayoutTileConstraint.DisableTerrainPhysics) || !this.m_entitiesDisablingTerrainPhysics.Remove(entity))
        return;
      Tile2i xy = entity.CenterTile.Xy;
      foreach (OccupiedVertexRelative occupiedVertex in entity.OccupiedVertices)
      {
        if (occupiedVertex.Constraint.HasAnyConstraints(LayoutTileConstraint.DisableTerrainPhysics))
        {
          Tile2i tile2i = xy + occupiedVertex.RelCoord;
          if (this.m_disabledPhysicsTiles.DecAndRemoveAtZero<Tile2i>(tile2i) == 0)
          {
            Tile2iAndIndex tileAndIndex = this.m_disabledPhysicsReporter.TerrainManager.ExtendTileIndex(tile2i);
            this.m_disabledPhysicsReporter.ClearFlag(tileAndIndex);
            this.tryQueueNewTile(tileAndIndex);
          }
        }
      }
    }

    public void Update()
    {
      if (this.m_cappedAtMaxStreak < 0)
      {
        Log.Error(string.Format("Capped at max streak somehow negative '{0}'.", (object) this.m_cappedAtMaxStreak));
        this.m_cappedAtMaxStreak = 0;
      }
      int num1 = (3000 + this.m_cappedAtMaxStreak * 10).Min(8000);
      int num2 = this.m_tilesToUpdate.Count.Min(num1);
      if (num2 >= num1)
        ++this.m_cappedAtMaxStreak;
      else if (this.m_cappedAtMaxStreak > 0)
        --this.m_cappedAtMaxStreak;
      for (int index = 0; index < num2; ++index)
      {
        Tile2iAndIndex tileAndIndex = this.m_tilesToUpdate.Dequeue();
        if (this.m_terrainManager.HasAnyTileFlagSet(tileAndIndex.Index, this.m_tileMaskAlreadyQueued))
        {
          this.m_isInQueueReporter.ClearFlagNoEvents(tileAndIndex.Index);
          if (!this.m_terrainManager.HasAnyTileFlagSet(tileAndIndex.Index, this.m_tileMaskIgnore))
            processTile(tileAndIndex);
        }
      }
      for (int index = 0; index < 10; ++index)
      {
        Swap.Them<Set<Tile2iAndIndex>>(ref this.m_uphillTilesToUpdateRead, ref this.m_uphillTilesToUpdateWrite);
        foreach (Tile2iAndIndex tileAndIndex in this.m_uphillTilesToUpdateRead)
        {
          this.m_isInQueueReporter.ClearFlagNoEvents(tileAndIndex.Index);
          processTile(tileAndIndex);
        }
        this.m_uphillTilesToUpdateRead.Clear();
      }
      Assert.That<int>(this.m_uphillTilesToUpdateRead.Count).IsZero();
      Assert.That<int>(this.m_uphillTilesToUpdateWrite.Count).IsZero();

      void processTile(Tile2iAndIndex tileAndIndex, int uphillStep = -1)
      {
        if (!this.processSolidHeightChange(tileAndIndex) || !this.m_random.TestProbability(0.05f))
          return;
        HeightTilesF height = this.m_terrainManager.GetHeight(tileAndIndex.Index);
        foreach (Tile2iAndIndexRel eightNeighborsDelta in this.m_terrainManager.EightNeighborsDeltas)
        {
          Tile2iAndIndex tileAndIndex1 = tileAndIndex + eightNeighborsDelta;
          if (this.m_terrainManager.GetHeight(tileAndIndex1.Index) > height)
          {
            if (uphillStep < 9)
              this.tryQueueNewUphillTile(tileAndIndex1);
            else
              this.tryQueueNewTile(tileAndIndex1);
          }
          else
            this.tryQueueNewTile(tileAndIndex1);
        }
      }
    }

    public void Clear()
    {
      foreach (Tile2iAndIndex tile2iAndIndex in this.m_tilesToUpdate)
        this.m_isInQueueReporter.ClearFlagNoEvents(tile2iAndIndex.Index);
      this.m_tilesToUpdate.Clear();
    }

    private bool processSolidHeightChange(Tile2iAndIndex tileAndIndex)
    {
      bool flag = false;
      HeightTilesF height = this.m_terrainManager.GetHeight(tileAndIndex.Index);
      Tile2iAndIndex tileLo = tileAndIndex;
      HeightTilesF heightLo = height;
      Tile2iAndIndex tileHi = tileAndIndex;
      HeightTilesF heightHi = height;
      int terrainWidth = this.m_terrainManager.TerrainWidth;
      processTile(tileAndIndex.MinusXNeighborUnchecked);
      processTile(tileAndIndex.PlusXNeighborUnchecked);
      processTile(tileAndIndex.MinusYNeighborUnchecked(terrainWidth));
      processTile(tileAndIndex.PlusYNeighborUnchecked(terrainWidth));
      if (this.m_random.TestProbability(0.3f))
      {
        processTile(tileAndIndex.MinusXMinusYNeighborUnchecked(terrainWidth));
        processTile(tileAndIndex.MinusXPlusYNeighborUnchecked(terrainWidth));
        processTile(tileAndIndex.PlusXMinusYNeighborUnchecked(terrainWidth));
        processTile(tileAndIndex.PlusXPlusYNeighborUnchecked(terrainWidth));
      }
      if (height > heightLo)
        flag |= this.simulateTerrainFall(tileAndIndex, tileLo, height - heightLo);
      if (heightHi > height)
        flag |= this.simulateTerrainFall(tileHi, tileAndIndex, heightHi - height);
      return flag;

      void processTile(Tile2iAndIndex t)
      {
        if (this.m_terrainManager.HasAnyTileFlagSet(t.Index, this.m_tileMaskIgnore))
        {
          if (!this.m_terrainManager.IsOnMapBoundary(t.Index))
            return;
          this.enforceMaxHeight(tileAndIndex, TerrainPhysicsSimulator.MAX_HEIGHT_AT_BOUNDARY);
        }
        else
        {
          HeightTilesF height = this.m_terrainManager.GetHeight(t.Index);
          if (height < heightLo)
          {
            tileLo = t;
            heightLo = height;
          }
          else
          {
            if (!(height > heightHi))
              return;
            tileHi = t;
            heightHi = height;
          }
        }
      }
    }

    private bool simulateTerrainFall(
      Tile2iAndIndex tileHi,
      Tile2iAndIndex tileLo,
      ThicknessTilesF heightDiff)
    {
      TerrainMaterialThicknessSlim firstLayerSlim = this.m_terrainManager.GetFirstLayerSlim(tileHi.Index);
      MinMaxPair<ThicknessTilesF> minMaxPair = this.m_collapseHeightDiffsCache[firstLayerSlim.SlimIdRaw];
      ThicknessTilesF thicknessTilesF;
      if (firstLayerSlim.Thickness < ThicknessTilesF.Two && (Proto) this.m_terrainManager.MinedProducts[firstLayerSlim.SlimIdRaw] != (Proto) this.m_terrainManager.MinedProducts[this.m_terrainManager.GetSecondLayerSlimOrNoneNoBedrock(tileHi.Index).SlimIdRaw])
      {
        ThicknessTilesF halfFast = (ThicknessTilesF.Two - firstLayerSlim.Thickness).HalfFast;
        thicknessTilesF = heightDiff - halfFast;
      }
      else
        thicknessTilesF = heightDiff;
      if (thicknessTilesF <= minMaxPair.Min || thicknessTilesF < minMaxPair.Max && !this.m_random.TestProbability(Percent.FromRatio((thicknessTilesF - minMaxPair.Min).Value, (minMaxPair.Max - minMaxPair.Min).Value)))
        return false;
      TerrainMaterialThicknessSlim newLayer = this.m_terrainManager.MineMaterial(tileHi, heightDiff.HalfFast);
      if (this.m_disruptAfterCollapseCache[this.m_terrainManager.GetFirstLayerSlim(tileHi.Index).SlimIdRaw])
        this.m_terrainManager.DisruptTopLayer(tileHi);
      TerrainMaterialSlimId disruptedMaterialId = this.m_terrainManager.DisruptedMaterialIds[(int) newLayer.SlimId.Value];
      if (disruptedMaterialId.IsNotPhantom)
        newLayer = newLayer.WithNewId(disruptedMaterialId);
      if (this.m_disruptAfterCollapseCache[this.m_terrainManager.GetFirstLayerSlim(tileLo.Index).SlimIdRaw])
        this.m_terrainManager.DisruptTopLayer(tileLo);
      this.m_terrainManager.DumpMaterial(tileLo, newLayer);
      return true;
    }

    private void enforceMaxHeight(Tile2iAndIndex tile, HeightTilesF maxHeight)
    {
      HeightTilesF height = this.m_terrainManager.GetHeight(tile.Index);
      if (height >= -HeightTilesF.One || height <= maxHeight)
        return;
      ThicknessTilesF maxThickness = height - maxHeight;
      this.m_terrainManager.MineMaterial(tile, maxThickness);
    }

    static TerrainPhysicsSimulator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainPhysicsSimulator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainPhysicsSimulator) obj).SerializeData(writer));
      TerrainPhysicsSimulator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainPhysicsSimulator) obj).DeserializeData(reader));
      TerrainPhysicsSimulator.MIN_HEIGHT_DIFF_FOR_COLLAPSE = 0.125.TilesThick();
      TerrainPhysicsSimulator.MAX_HEIGHT_AT_BOUNDARY = new HeightTilesF(-5);
    }
  }
}
