// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutEntityAddRequest
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Validators;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  public class LayoutEntityAddRequest : 
    ILayoutEntityAddRequest,
    IEntityWithOccupiedTilesAddRequest,
    IEntityAddRequest
  {
    private static readonly ObjectPool<LayoutEntityAddRequest> s_pool;
    private EntityPlacementPhase m_placementPhase;
    private Option<Lyst<Tile2i>> m_additionalErrorTiles;
    private BitMap m_tileErrors;

    /// <summary>
    /// Returns an instance (either from pool or new one).
    /// NOTE: The caller should perform operation and then return the object to the pool. Instances obtained from the
    /// pool should have very short life-span. For long-living objects create an instance using new.
    /// </summary>
    public static LayoutEntityAddRequest GetPooledInstanceToCreateEntity(
      ILayoutEntityProto proto,
      EntityAddRequestData data,
      EntityAddReason reason = EntityAddReason.New)
    {
      return LayoutEntityAddRequest.getPooledInstance(proto, data.Transform, reason, data.IgnoreForCollisions.ValueOrNull, data.RecordTileErrors);
    }

    private static LayoutEntityAddRequest getPooledInstance(
      ILayoutEntityProto proto,
      TileTransform transform,
      EntityAddReason reasonToAdd,
      Predicate<EntityId> ignoreForCollisions = null,
      bool recordTileErrors = false)
    {
      LayoutEntityAddRequest instance = LayoutEntityAddRequest.s_pool.GetInstance();
      instance.initialize(proto, transform, reasonToAdd, ignoreForCollisions, recordTileErrors);
      return instance;
    }

    private static LayoutEntityAddRequest getPooledInstance(
      LayoutEntityProto proto,
      TileTransform transform,
      Option<ILayoutEntity> layoutEntity,
      EntityAddReason reasonToAdd)
    {
      EntityId id = layoutEntity.HasValue ? layoutEntity.Value.Id : EntityId.Invalid;
      Predicate<EntityId> ignoreForCollisions = layoutEntity.HasValue ? (Predicate<EntityId>) (x => x == id) : (Predicate<EntityId>) null;
      return LayoutEntityAddRequest.getPooledInstance((ILayoutEntityProto) proto, transform, reasonToAdd, ignoreForCollisions);
    }

    /// <summary>
    /// Returns this instance to the object pool.
    /// IMPORTANT: The caller is responsible to ensure that he is the only owner of this instance and no other object
    /// have reference to this object.
    /// </summary>
    public void ReturnToPool() => LayoutEntityAddRequest.s_pool.ReturnInstance(this);

    public ILayoutEntityProto Proto { get; private set; }

    public EntityLayout Layout => this.Proto.Layout;

    public bool RecordTileErrorsAndMetadata { get; private set; }

    public EntityPlacementPhase PlacementPhase => this.m_placementPhase;

    public void SetPlacementPhase(EntityPlacementPhase phase) => this.m_placementPhase = phase;

    public bool GetTileError(int occupiedTileIndex) => this.m_tileErrors.IsSet(occupiedTileIndex);

    public void SetTileError(int occupiedTileIndex) => this.m_tileErrors.SetBit(occupiedTileIndex);

    public void CopyTileErrorsTo(LayoutEntityAddRequest otherAddRequest)
    {
      if (otherAddRequest.m_tileErrors.BackingArray.Length < this.m_tileErrors.BackingArray.Length)
        Log.Error("Attempting to copy to undersized target.");
      else
        this.m_tileErrors.BackingArray.CopyTo((Array) otherAddRequest.m_tileErrors.BackingArray, 0);
    }

    public bool HasAdditionalErrorTiles
    {
      get => this.m_additionalErrorTiles.HasValue && this.m_additionalErrorTiles.Value.IsNotEmpty;
    }

    /// <summary>
    /// Additional error tiles may be outside of map (incl negative coordinates).
    /// </summary>
    public Lyst<Tile2i> GetAdditionalErrorTilesStorage()
    {
      Assert.That<bool>(this.RecordTileErrorsAndMetadata).IsTrue("Getting additional error tiles storage but error reporting was not requested.");
      if (this.m_additionalErrorTiles.IsNone)
        this.m_additionalErrorTiles = (Option<Lyst<Tile2i>>) new Lyst<Tile2i>(64);
      return this.m_additionalErrorTiles.Value;
    }

    public TileTransform Transform { get; private set; }

    public Tile3i Origin { get; private set; }

    public ReadOnlyArray<OccupiedTileRelative> OccupiedTiles { get; private set; }

    public ReadOnlyArray<OccupiedVertexRelative> OccupiedVertices { get; private set; }

    public Option<Predicate<EntityId>> IgnoreForCollisions { get; private set; }

    public EntityAddReason ReasonToAdd { get; private set; }

    public Option<Lyst<IAddRequestMetadata>> Metadata { get; private set; }

    private LayoutEntityAddRequest()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    internal LayoutEntityAddRequest(
      ILayoutEntityProto proto,
      TileTransform transform,
      EntityAddReason reasonToAdd,
      Predicate<EntityId> ignoreForCollisions = null,
      bool recordTileErrors = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.initialize(proto, transform, reasonToAdd, ignoreForCollisions, recordTileErrors);
    }

    private void initialize(
      ILayoutEntityProto proto,
      TileTransform transform,
      EntityAddReason reasonToAdd,
      Predicate<EntityId> ignoreForCollisions = null,
      bool recordTileErrors = false)
    {
      this.Proto = proto.CheckNotNull<ILayoutEntityProto>();
      this.Transform = transform;
      this.Origin = transform.Position;
      this.OccupiedTiles = proto.Layout.GetOccupiedTilesRelative(transform).AsReadOnlyArray;
      this.OccupiedVertices = proto.Layout.GetOccupiedVerticesRelative(transform).AsReadOnlyArray;
      this.IgnoreForCollisions = (Option<Predicate<EntityId>>) ignoreForCollisions;
      this.ReasonToAdd = reasonToAdd;
      this.RecordTileErrorsAndMetadata = recordTileErrors;
      if (this.m_tileErrors.Size < this.OccupiedTiles.Length)
        this.m_tileErrors = new BitMap(this.OccupiedTiles.Length + 32);
      else
        this.m_tileErrors.ClearAllBits();
      if (this.m_additionalErrorTiles.HasValue)
        this.m_additionalErrorTiles.Value.Clear();
      this.m_placementPhase = EntityPlacementPhase.FirstAndFinal;
    }

    public void AddMetadata(IAddRequestMetadata metadata)
    {
      Lyst<IAddRequestMetadata> lyst;
      if (this.Metadata.HasValue)
      {
        lyst = this.Metadata.Value;
      }
      else
      {
        lyst = new Lyst<IAddRequestMetadata>();
        this.Metadata = (Option<Lyst<IAddRequestMetadata>>) lyst;
      }
      lyst.Add(metadata);
    }

    private void recycle()
    {
      this.Proto = (ILayoutEntityProto) null;
      this.Transform = new TileTransform();
      this.m_placementPhase = EntityPlacementPhase.FirstAndFinal;
      if (!this.Metadata.HasValue)
        return;
      this.Metadata.Value.ForEachAndClear((Action<IAddRequestMetadata>) (x => x.Recycle()));
    }

    static LayoutEntityAddRequest()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LayoutEntityAddRequest.s_pool = new ObjectPool<LayoutEntityAddRequest>(4, (Func<LayoutEntityAddRequest>) (() => new LayoutEntityAddRequest()), (Action<LayoutEntityAddRequest>) (rr => rr.recycle()));
    }
  }
}
