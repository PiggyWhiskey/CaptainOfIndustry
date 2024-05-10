// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutEntityBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  public abstract class LayoutEntityBase : 
    StaticEntity,
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    private LayoutEntityProto m_proto;
    [DoNotSave(0, null)]
    private ImmutableArray<OccupiedTileRelative> m_occupiedTilesCache;
    [DoNotSave(0, null)]
    private ImmutableArray<OccupiedVertexRelative> m_occupiedVerticesCache;
    [DoNotSave(0, null)]
    private LayoutTileConstraint m_occVertsCombinedConstraintCache;
    [DoNotSave(0, null)]
    private ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>> m_vehicleSurfaceHeightsCache;
    [DoNotSave(0, null)]
    private StaticEntityPfTargetTiles m_pfTargetTilesCache;

    public override AssetValue Value => this.Prototype.Costs.Price;

    public override AssetValue ConstructionCost => this.Value;

    [DoNotSave(0, null)]
    public LayoutEntityProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        bool flag = (Proto) this.m_proto != (Proto) null;
        this.m_proto = value;
        this.Prototype = (StaticEntityProto) value;
        if (flag)
          this.SetTransform(this.Transform);
        this.m_occupiedTilesCache = new ImmutableArray<OccupiedTileRelative>();
        this.m_occupiedVerticesCache = new ImmutableArray<OccupiedVertexRelative>();
      }
    }

    public TileTransform Transform { get; private set; }

    public override ImmutableArray<OccupiedTileRelative> OccupiedTiles
    {
      get
      {
        if (this.m_occupiedTilesCache.IsNotValid)
          this.m_occupiedTilesCache = this.m_proto.Layout.GetOccupiedTilesRelative(this.Transform);
        return this.m_occupiedTilesCache;
      }
    }

    public override ImmutableArray<OccupiedVertexRelative> OccupiedVertices
    {
      get
      {
        if (this.m_occupiedVerticesCache.IsNotValid)
          this.computeOccVerts();
        return this.m_occupiedVerticesCache;
      }
    }

    public override LayoutTileConstraint OccupiedVerticesCombinedConstraint
    {
      get
      {
        if (this.m_occupiedVerticesCache.IsNotValid)
          this.computeOccVerts();
        return this.m_occVertsCombinedConstraintCache;
      }
    }

    private void computeOccVerts()
    {
      this.m_occupiedVerticesCache = this.m_proto.Layout.GetOccupiedVerticesRelative(this.Transform);
      this.m_occVertsCombinedConstraintCache = this.m_occupiedVerticesCache.CombineConstraint();
    }

    public override ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>> VehicleSurfaceHeights
    {
      get
      {
        if (this.m_vehicleSurfaceHeightsCache.IsNotValid)
          this.m_vehicleSurfaceHeightsCache = this.m_proto.Layout.GetVehicleSurfaceHeights(this.Transform);
        return this.m_vehicleSurfaceHeightsCache;
      }
    }

    public override StaticEntityPfTargetTiles PfTargetTiles
    {
      get
      {
        if (this.m_pfTargetTilesCache == null)
        {
          this.m_pfTargetTilesCache = StaticEntityPfTargetTiles.FromGroundTiles(this.OccupiedTiles.Filter((Predicate<OccupiedTileRelative>) (x => x.RelativeFrom <= (short) 0)).Map<Tile2i>((Func<OccupiedTileRelative, Tile2i>) (x => this.CenterTile.Xy + x.RelCoord)));
          Assert.That<int>(this.m_pfTargetTilesCache.TilesCount).IsPositive<LayoutEntityBase>("No PF tiles for entity '{0}'", this);
        }
        return this.m_pfTargetTilesCache;
      }
    }

    /// <summary>Creates a stub instance (for loading).</summary>
    protected LayoutEntityBase(
      EntityId id,
      LayoutEntityProto proto,
      TileTransform transform,
      EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (StaticEntityProto) proto, context, transform.Position);
      Assert.That<bool>(proto.Costs.Maintenance.MaintenancePerMonth.IsZero || this.GetType().IsAssignableTo<IMaintainedEntity>()).IsTrue<LayoutEntityBase>("Entity '{0}' with non-zero maintenance cost is not 'IMaintainedEntity'.", this);
      Assert.That<bool>(proto.Costs.Workers == 0 || this.GetType().IsAssignableTo<IEntityWithWorkers>()).IsTrue<LayoutEntityBase>("Entity '{0}' with non-zero workers requirement is not 'IWorkersAssignableEntity'.", this);
      this.m_proto = proto.CheckNotNull<LayoutEntityProto>();
      this.Transform = transform;
    }

    public Tile3f GetCenter() => this.Prototype.Layout.GetCenter(this.Transform);

    protected virtual void SetTransform(TileTransform transform)
    {
      this.Transform = transform;
      this.m_vehicleSurfaceHeightsCache = new ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>>();
      this.m_pfTargetTilesCache = (StaticEntityPfTargetTiles) null;
    }

    public override ImmutableArray<ConstrCubeSpec> GetConstructionCubesSpec(out int totalCubesVolume)
    {
      switch (this.ConstructionState)
      {
        case ConstructionState.InConstruction:
        case ConstructionState.InDeconstruction:
          return base.GetConstructionCubesSpec(out totalCubesVolume);
        case ConstructionState.BeingUpgraded:
          ImmutableArray<OccupiedTileRelative> occupiedTiles;
          if (this is IUpgradableEntity upgradableEntity)
          {
            if (upgradableEntity.Upgrader.NextTier.HasValue && upgradableEntity.Upgrader.NextTier.Value is LayoutEntityProto layoutEntityProto)
            {
              occupiedTiles = layoutEntityProto.Layout.GetOccupiedTilesRelative(this.Transform);
            }
            else
            {
              Log.Error("Entity that is being upgraded does not have next tier or the next tier is not layout entity.");
              occupiedTiles = this.OccupiedTiles;
            }
          }
          else
          {
            Log.Error("Non upgradable entity is being upgraded.");
            occupiedTiles = this.OccupiedTiles;
          }
          return ConstructionCubesHelper.ConvertColumnsToCubes(this.CenterTile, ConstructionCubesHelper.ComputeOptimizedConstructionCubeColumns(occupiedTiles), true, out totalCubesVolume);
        default:
          Log.Warning(string.Format("Getting construction cubes in non-constructing state: {0}", (object) this.ConstructionState));
          return base.GetConstructionCubesSpec(out totalCubesVolume);
      }
    }

    public override void NotifyUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      int newIndex,
      bool wasAdded,
      out bool canCollapse)
    {
      canCollapse = groundVerticesViolatingConstraints.Count > this.m_proto.Layout.CollapseVerticesThreshold;
    }

    public override bool TryCollapseOnUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      EntityCollapseHelper collapseHelper)
    {
      return collapseHelper.TryDestroyEntityAndAddRubble((IStaticEntity) this);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<LayoutEntityProto>(this.m_proto);
      TileTransform.Serialize(this.Transform, writer);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_proto = reader.ReadGenericAs<LayoutEntityProto>();
      this.Transform = TileTransform.Deserialize(reader);
    }
  }
}
