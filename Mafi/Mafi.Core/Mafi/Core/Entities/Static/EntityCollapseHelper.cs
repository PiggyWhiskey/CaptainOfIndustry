// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.EntityCollapseHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Utils;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class EntityCollapseHelper
  {
    public const int SIM_UPDATE_FREQ = 4;
    public static readonly Percent RUBBLE_ONLY_AFTER_CONSTR_PERCENT;
    public static readonly ThicknessTilesF RUBBLE_PER_TILE_MIN;
    public static readonly ThicknessTilesF RUBBLE_PER_TILE_MAX;
    public readonly TerrainMaterialProto RubbleMaterialProto;
    private readonly TerrainManager m_terrainManager;
    private readonly EntitiesManager m_entitiesManager;
    private readonly IConstructionManager m_constructionManager;
    [DoNotSaveCreateNewOnLoad("new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 0, 1)", 0)]
    private readonly XorRsr128PlusGenerator m_random;
    private readonly Lyst<EntityCollapseHelper.RemainingRubbleData> m_remainingRubbleData;
    private int m_simUpdateCounter;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool HasRemainingRubble => this.m_remainingRubbleData.IsNotEmpty;

    public EntityCollapseHelper(
      ISimLoopEvents simLoopEvents,
      TerrainManager terrainManager,
      EntitiesManager entitiesManager,
      ProtosDb protosDb,
      IConstructionManager constructionManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_random = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 0UL, 1UL);
      this.m_remainingRubbleData = new Lyst<EntityCollapseHelper.RemainingRubbleData>(true);
      this.m_simUpdateCounter = 4;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_entitiesManager = entitiesManager;
      this.m_constructionManager = constructionManager;
      this.RubbleMaterialProto = protosDb.GetOrThrow<TerrainMaterialProto>(IdsCore.TerrainMaterials.Landfill);
      simLoopEvents.Update.Add<EntityCollapseHelper>(this, new Action(this.simUpdate));
    }

    private void simUpdate()
    {
      if (this.m_remainingRubbleData.IsEmpty)
        return;
      if (this.m_simUpdateCounter > 0)
      {
        --this.m_simUpdateCounter;
      }
      else
      {
        this.m_simUpdateCounter = 4;
        for (int index = 0; index < this.m_remainingRubbleData.Count; ++index)
        {
          EntityCollapseHelper.RemainingRubbleData remainingRubbleData = this.m_remainingRubbleData[index];
          this.m_terrainManager.DumpMaterial(this.m_terrainManager.ExtendTileIndex(remainingRubbleData.VertexCoord), new TerrainMaterialThicknessSlim(this.RubbleMaterialProto, remainingRubbleData.ThicknessPerStep));
          this.m_remainingRubbleData[index] = remainingRubbleData.DecrementRemainingSteps();
        }
        this.m_remainingRubbleData.RemoveWhere((Predicate<EntityCollapseHelper.RemainingRubbleData>) (x => x.RemainingSteps <= 0));
      }
    }

    public void AddRubbleToTerrainVertex(
      Tile2i tileCoord,
      ThicknessTilesI height,
      Percent rubblePerc)
    {
      if (height.IsNotPositive || rubblePerc.IsNotPositive)
        return;
      this.m_random.SeedFast(tileCoord.X, tileCoord.Y, height.Value, 7);
      ThicknessTilesF thicknessTilesF = (height.ThicknessTilesF * this.m_random.NextFix32(EntityCollapseHelper.RUBBLE_PER_TILE_MIN.Value, EntityCollapseHelper.RUBBLE_PER_TILE_MAX.Value)).ScaledBy(rubblePerc);
      int remainingSteps = 1 + this.m_random.NextInt(0, height.Value / 2 + 1);
      this.m_remainingRubbleData.Add(new EntityCollapseHelper.RemainingRubbleData(tileCoord, thicknessTilesF / remainingSteps, remainingSteps));
    }

    public bool TryDestroyEntityAndAddRubble(IStaticEntity entity)
    {
      if (this.m_entitiesManager.CanRemoveEntity((IEntity) entity, EntityRemoveReason.Collapse).IsError)
        return false;
      Percent rubblePerc = entity.ConstructionProgress.HasValue ? (entity.ConstructionProgress.Value.Progress < 10.Percent() ? Percent.Zero : entity.ConstructionProgress.Value.Progress) : (entity.IsConstructed ? Percent.Hundred : Percent.Zero);
      if (entity.ConstructionState != ConstructionState.InDeconstruction)
        this.m_constructionManager.StartDeconstruction(entity, true, EntityRemoveReason.Collapse);
      if (entity.ConstructionState == ConstructionState.InDeconstruction)
        this.m_constructionManager.MarkDeconstructed(entity, doNotRecoverTerrainHeight: true, entityRemoveReason: EntityRemoveReason.Collapse);
      bool flag;
      switch (entity.ConstructionState)
      {
        case ConstructionState.InDeconstruction:
        case ConstructionState.Deconstructed:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      Assert.That<bool>(flag).IsTrue("Failed to start deconstruction during collapse");
      if (!entity.IsDestroyed)
        this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) entity, EntityRemoveReason.Collapse);
      if (rubblePerc > EntityCollapseHelper.RUBBLE_ONLY_AFTER_CONSTR_PERCENT)
      {
        Tile2i xy = entity.CenterTile.Xy;
        foreach (OccupiedVertexRelative occupiedVertex in entity.OccupiedVertices)
        {
          if (!occupiedVertex.Constraint.HasAnyConstraints(LayoutTileConstraint.NoRubbleAfterCollapse))
            this.AddRubbleToTerrainVertex(xy + occupiedVertex.RelCoord, occupiedVertex.VerticalSize, rubblePerc);
        }
      }
      return true;
    }

    public static void Serialize(EntityCollapseHelper value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<EntityCollapseHelper>(value))
        return;
      writer.EnqueueDataSerialization((object) value, EntityCollapseHelper.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IConstructionManager>(this.m_constructionManager);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      Lyst<EntityCollapseHelper.RemainingRubbleData>.Serialize(this.m_remainingRubbleData, writer);
      writer.WriteInt(this.m_simUpdateCounter);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      writer.WriteGeneric<TerrainMaterialProto>(this.RubbleMaterialProto);
    }

    public static EntityCollapseHelper Deserialize(BlobReader reader)
    {
      EntityCollapseHelper entityCollapseHelper;
      if (reader.TryStartClassDeserialization<EntityCollapseHelper>(out entityCollapseHelper))
        reader.EnqueueDataDeserialization((object) entityCollapseHelper, EntityCollapseHelper.s_deserializeDataDelayedAction);
      return entityCollapseHelper;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<EntityCollapseHelper>(this, "m_constructionManager", (object) reader.ReadGenericAs<IConstructionManager>());
      reader.SetField<EntityCollapseHelper>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<EntityCollapseHelper>(this, "m_random", (object) new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 0UL, 1UL));
      reader.SetField<EntityCollapseHelper>(this, "m_remainingRubbleData", (object) Lyst<EntityCollapseHelper.RemainingRubbleData>.Deserialize(reader));
      this.m_simUpdateCounter = reader.ReadInt();
      reader.SetField<EntityCollapseHelper>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      reader.SetField<EntityCollapseHelper>(this, "RubbleMaterialProto", (object) reader.ReadGenericAs<TerrainMaterialProto>());
    }

    static EntityCollapseHelper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityCollapseHelper.RUBBLE_ONLY_AFTER_CONSTR_PERCENT = 10.Percent();
      EntityCollapseHelper.RUBBLE_PER_TILE_MIN = 0.05.TilesThick();
      EntityCollapseHelper.RUBBLE_PER_TILE_MAX = 0.3.TilesThick();
      EntityCollapseHelper.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((EntityCollapseHelper) obj).SerializeData(writer));
      EntityCollapseHelper.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((EntityCollapseHelper) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private readonly struct RemainingRubbleData
    {
      public readonly Tile2i VertexCoord;
      public readonly ThicknessTilesF ThicknessPerStep;
      public readonly int RemainingSteps;

      public RemainingRubbleData(
        Tile2i vertexCoord,
        ThicknessTilesF thicknessPerStep,
        int remainingSteps)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.VertexCoord = vertexCoord;
        this.ThicknessPerStep = thicknessPerStep;
        this.RemainingSteps = remainingSteps;
      }

      public EntityCollapseHelper.RemainingRubbleData DecrementRemainingSteps()
      {
        return new EntityCollapseHelper.RemainingRubbleData(this.VertexCoord, this.ThicknessPerStep, this.RemainingSteps - 1);
      }

      public static void Serialize(
        EntityCollapseHelper.RemainingRubbleData value,
        BlobWriter writer)
      {
        Tile2i.Serialize(value.VertexCoord, writer);
        ThicknessTilesF.Serialize(value.ThicknessPerStep, writer);
        writer.WriteInt(value.RemainingSteps);
      }

      public static EntityCollapseHelper.RemainingRubbleData Deserialize(BlobReader reader)
      {
        return new EntityCollapseHelper.RemainingRubbleData(Tile2i.Deserialize(reader), ThicknessTilesF.Deserialize(reader), reader.ReadInt());
      }
    }
  }
}
