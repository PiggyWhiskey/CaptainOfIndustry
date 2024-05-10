// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportPillar
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [GenerateSerializer(false, null, 0)]
  public class TransportPillar : StaticEntity, IEntityWithAdditionRequest, IEntity, IIsSafeAsHashKey
  {
    public readonly TransportPillarProto Prototype;
    [DoNotSave(0, null)]
    private StaticEntityPfTargetTiles m_pfTargetTilesCache;
    [DoNotSave(0, null)]
    private ImmutableArray<OccupiedTileRelative> m_occupiedTilesCache;
    [DoNotSave(0, null)]
    private ImmutableArray<OccupiedVertexRelative> m_occupiedVerticesCache;
    [DoNotSave(0, null)]
    private LayoutTileConstraint m_occVertsCombinedConstraintCache;
    /// <summary>Only for pillars renderer use, do not touch!</summary>
    [DoNotSave(0, null)]
    public TransportPillarRendererData RendererData;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => false;

    public override ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>> VehicleSurfaceHeights
    {
      get => ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>>.Empty;
    }

    public override StaticEntityPfTargetTiles PfTargetTiles
    {
      get
      {
        if (this.m_pfTargetTilesCache == null)
          this.m_pfTargetTilesCache = StaticEntityPfTargetTiles.FromGroundTiles(ImmutableArray.Create<Tile2i>(this.CenterTile.Xy));
        return this.m_pfTargetTilesCache;
      }
    }

    public override AssetValue Value => AssetValue.Empty;

    public override AssetValue ConstructionCost => AssetValue.Empty;

    public override ImmutableArray<OccupiedTileRelative> OccupiedTiles
    {
      get
      {
        if (this.m_occupiedTilesCache.IsNotValid)
          this.m_occupiedTilesCache = this.computeOccupiedTiles();
        return this.m_occupiedTilesCache;
      }
    }

    public override ImmutableArray<OccupiedVertexRelative> OccupiedVertices
    {
      get
      {
        if (this.m_occupiedVerticesCache.IsNotValid)
          this.m_occupiedVerticesCache = this.computeOccupiedVertices();
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
      this.m_occupiedVerticesCache = this.computeOccupiedVertices();
      this.m_occVertsCombinedConstraintCache = this.m_occupiedVerticesCache.CombineConstraint();
    }

    /// <summary>
    /// Height of the pillar. This does include the transport attachment on the top-most tile!
    /// </summary>
    public ThicknessTilesI Height { get; private set; }

    public HeightTilesI TopTileHeight => this.CenterTile.Height + this.Height - ThicknessTilesI.One;

    public override ImmutableArray<ConstrCubeSpec> GetConstructionCubesSpec(out int totalCubesVolume)
    {
      totalCubesVolume = 0;
      return ImmutableArray<ConstrCubeSpec>.Empty;
    }

    public override bool AreConstructionCubesDisabled => true;

    public TransportPillar(
      EntityId id,
      TransportPillarProto prototype,
      EntityContext context,
      Tile3i basePosition,
      ThicknessTilesI height)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (StaticEntityProto) prototype, context, basePosition);
      Assert.That<ThicknessTilesI>(height).IsLessOrEqual(TransportPillarProto.MAX_PILLAR_HEIGHT);
      this.Prototype = prototype;
      this.Height = height.CheckPositive();
    }

    private ImmutableArray<OccupiedTileRelative> computeOccupiedTiles()
    {
      return ImmutableArray.Create<OccupiedTileRelative>(new OccupiedTileRelative(RelTile2i.Zero, ThicknessTilesI.Zero, this.Height, LayoutTileConstraint.Ground, TileSurfaceSlimId.PhantomId, ThicknessTilesI.Zero));
    }

    private ImmutableArray<OccupiedVertexRelative> computeOccupiedVertices()
    {
      return ImmutableArray.Create<OccupiedVertexRelative>(new OccupiedVertexRelative(new RelTile2i(0, 0), ThicknessTilesI.Zero, this.Height, LayoutTileConstraint.Ground, TerrainMaterialSlimIdOption.Phantom, new ThicknessTilesI?(), new ThicknessTilesI?(), new ThicknessTilesI?(ThicknessTilesI.Zero), new ThicknessTilesI?(), 0), new OccupiedVertexRelative(new RelTile2i(1, 0), ThicknessTilesI.Zero, this.Height, LayoutTileConstraint.Ground, TerrainMaterialSlimIdOption.Phantom, new ThicknessTilesI?(), new ThicknessTilesI?(), new ThicknessTilesI?(ThicknessTilesI.Zero), new ThicknessTilesI?(), 0), new OccupiedVertexRelative(new RelTile2i(0, 1), ThicknessTilesI.Zero, this.Height, LayoutTileConstraint.Ground, TerrainMaterialSlimIdOption.Phantom, new ThicknessTilesI?(), new ThicknessTilesI?(), new ThicknessTilesI?(ThicknessTilesI.Zero), new ThicknessTilesI?(), 0), new OccupiedVertexRelative(new RelTile2i(1, 1), ThicknessTilesI.Zero, this.Height, LayoutTileConstraint.Ground, TerrainMaterialSlimIdOption.Phantom, new ThicknessTilesI?(), new ThicknessTilesI?(), new ThicknessTilesI?(ThicknessTilesI.Zero), new ThicknessTilesI?(), 0));
    }

    public IEntityAddRequest GetAddRequest(EntityAddReason reasonToAdd)
    {
      return (IEntityAddRequest) TransportPillarAddRequest.Instance;
    }

    public override void NotifyUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      int newIndex,
      bool wasAdded,
      out bool canCollapse)
    {
      canCollapse = groundVerticesViolatingConstraints.Count > 2;
    }

    public override bool TryCollapseOnUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      EntityCollapseHelper collapseHelper)
    {
      return collapseHelper.TryDestroyEntityAndAddRubble((IStaticEntity) this);
    }

    public static void Serialize(TransportPillar value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TransportPillar>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TransportPillar.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ThicknessTilesI.Serialize(this.Height, writer);
      writer.WriteGeneric<TransportPillarProto>(this.Prototype);
    }

    public static TransportPillar Deserialize(BlobReader reader)
    {
      TransportPillar transportPillar;
      if (reader.TryStartClassDeserialization<TransportPillar>(out transportPillar))
        reader.EnqueueDataDeserialization((object) transportPillar, TransportPillar.s_deserializeDataDelayedAction);
      return transportPillar;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Height = ThicknessTilesI.Deserialize(reader);
      reader.SetField<TransportPillar>(this, "Prototype", (object) reader.ReadGenericAs<TransportPillarProto>());
    }

    static TransportPillar()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TransportPillar.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      TransportPillar.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
