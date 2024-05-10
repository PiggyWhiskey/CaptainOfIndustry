// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DynamicGroundEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  /// <summary>
  /// Vehicle that is able to drive around.
  /// TODO: Rename to DrivingVehicle
  /// </summary>
  public abstract class DynamicGroundEntity : 
    Entity,
    IEntityWithSimUpdate,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IRenderedEntity
  {
    public readonly TerrainManager Terrain;
    public readonly DynamicGroundEntityProto Prototype;
    public readonly IVehicleSurfaceProvider SurfaceProvider;
    private Tile2f m_groundPosition;
    private HeightTilesF m_height;
    private bool m_isHeightComputed;
    public Tile2i LastDisruptedTile;

    public Tile2f Position2f => this.m_groundPosition;

    /// <summary>Absolute position of this entity in the world.</summary>
    public Tile3f Position3f
    {
      get
      {
        if (!this.m_isHeightComputed)
        {
          this.m_height = this.SurfaceProvider.GetInterpolatedVehicleSurfaceAt(this.m_groundPosition);
          this.m_isHeightComputed = true;
        }
        return this.m_groundPosition.ExtendHeight(this.m_height);
      }
    }

    /// <summary>
    /// Tile that this entity occupies. Note that this is more effective than <see cref="P:Mafi.Core.Entities.Dynamic.DynamicGroundEntity.GroundPositionTile" />.
    /// </summary>
    public Tile2i GroundPositionTile2i => this.m_groundPosition.Tile2i;

    /// <summary>
    /// Terrain tile that this entity occupies. Note that this requires terrain tile lookup. Use <see cref="P:Mafi.Core.Entities.Dynamic.DynamicGroundEntity.GroundPositionTile2i" /> if you just care about position.
    /// </summary>
    public TerrainTile GroundPositionTile => this.Terrain[this.m_groundPosition.Tile2i];

    /// <summary>
    /// Direction of the vehicle. If direction is zero, vehicle points to positive x direction.
    /// </summary>
    public AngleDegrees1f Direction { get; protected set; }

    /// <summary>
    /// Whether this vehicle is spawned in the world and its models should be drawn.
    /// </summary>
    public bool IsSpawned { get; private set; }

    /// <summary>
    /// Current speed. Sign represents direction, negative value represents backwards motion.
    /// </summary>
    public abstract RelTile1f Speed { get; }

    public bool ForceFlatGround { get; set; }

    [DoNotSave(0, null)]
    ulong IRenderedEntity.RendererData { get; set; }

    protected DynamicGroundEntity(
      EntityId id,
      DynamicGroundEntityProto prototype,
      EntityContext context,
      Tile2f initialPosition,
      TerrainManager terrain,
      IVehicleSurfaceProvider surfaceProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (EntityProto) prototype, context);
      this.Prototype = prototype;
      this.SurfaceProvider = surfaceProvider;
      this.Terrain = terrain.CheckNotNull<TerrainManager>();
      this.SetGroundPosition(initialPosition);
      this.LastDisruptedTile = initialPosition.Tile2i;
    }

    /// <summary>
    /// Returns distance squared to given <paramref name="position" />.
    /// </summary>
    public Fix64 DistanceSqrTo(Tile2f position) => (this.m_groundPosition - position).LengthSqr;

    /// <summary>Spawns this vehicle at given position in the world.</summary>
    public virtual void Spawn(Tile2f position, AngleDegrees1f direction)
    {
      Assert.That<bool>(this.IsSpawned).IsFalse();
      this.IsSpawned = true;
      this.SetGroundPosition(position);
      this.LastDisruptedTile = position.Tile2i;
      this.Direction = direction.Normalized;
    }

    /// <summary>Removes this vehicle from the world.</summary>
    public virtual void Despawn()
    {
      Assert.That<bool>(this.IsSpawned).IsTrue();
      this.IsSpawned = false;
    }

    public void SetForceFlatGround(bool enabled) => this.ForceFlatGround = enabled;

    void IEntityWithSimUpdate.SimUpdate() => this.SimUpdateInternal();

    protected virtual void SimUpdateInternal()
    {
      if (!this.IsSpawned || !this.Prototype.DisruptsSurface)
        return;
      Tile2i tile2i = this.m_groundPosition.Tile2i;
      if (!(tile2i != this.LastDisruptedTile))
        return;
      this.disruptSurfaceAt(this.LastDisruptedTile);
      this.LastDisruptedTile = tile2i;
    }

    internal virtual void TeleportTo(Tile2f position, AngleDegrees1f? angle = null)
    {
      this.SetGroundPosition(position);
      this.LastDisruptedTile = position.Tile2i;
      if (!angle.HasValue)
        return;
      this.Direction = angle.Value.Normalized;
    }

    protected void SetGroundPosition(Tile2f position)
    {
      if (position == this.m_groundPosition)
        return;
      Tile2i tile2i = this.m_groundPosition.Tile2i;
      this.m_groundPosition = position;
      this.m_isHeightComputed = false;
      if (!(position.Tile2i != tile2i))
        return;
      this.OnNewTile2iVisited(tile2i);
    }

    protected void SetPosition(Tile3f position)
    {
      this.m_groundPosition = position.Xy;
      this.m_height = position.Height;
      this.m_isHeightComputed = true;
    }

    protected virtual void OnNewTile2iVisited(Tile2i oldPosition)
    {
    }

    private void disruptSurfaceAt(Tile2i position)
    {
      Tile2iAndIndex tileAndIndex = this.Terrain.ExtendTileIndex(position);
      if (this.Terrain.IsOffLimitsOrInvalid(position))
        return;
      if (this.Prototype.DisruptionByDistance[0].IsPositive)
        this.Terrain.Disrupt(tileAndIndex, this.Prototype.DisruptionByDistance[0]);
      int num = 1;
      for (int length = this.Prototype.DisruptionByDistance.Length; num < length; ++num)
      {
        ThicknessTilesF disruptionAmount = this.Prototype.DisruptionByDistance[num];
        if (!disruptionAmount.IsNotPositive)
        {
          for (int index = -num; index < num; ++index)
          {
            this.Terrain.Disrupt(this.Terrain.ExtendTileIndex(position + new RelTile2i(index, num)), disruptionAmount);
            this.Terrain.Disrupt(this.Terrain.ExtendTileIndex(position + new RelTile2i(num, -index)), disruptionAmount);
            this.Terrain.Disrupt(this.Terrain.ExtendTileIndex(position + new RelTile2i(-index, -num)), disruptionAmount);
            this.Terrain.Disrupt(this.Terrain.ExtendTileIndex(position + new RelTile2i(-num, index)), disruptionAmount);
          }
        }
      }
    }

    bool IAreaSelectableEntity.IsSelected(RectangleTerrainArea2i area)
    {
      return area.Contains(this.m_groundPosition);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AngleDegrees1f.Serialize(this.Direction, writer);
      writer.WriteBool(this.ForceFlatGround);
      writer.WriteBool(this.IsSpawned);
      Tile2i.Serialize(this.LastDisruptedTile, writer);
      Tile2f.Serialize(this.m_groundPosition, writer);
      HeightTilesF.Serialize(this.m_height, writer);
      writer.WriteBool(this.m_isHeightComputed);
      writer.WriteGeneric<DynamicGroundEntityProto>(this.Prototype);
      writer.WriteGeneric<IVehicleSurfaceProvider>(this.SurfaceProvider);
      TerrainManager.Serialize(this.Terrain, writer);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Direction = AngleDegrees1f.Deserialize(reader);
      this.ForceFlatGround = reader.ReadBool();
      this.IsSpawned = reader.ReadBool();
      this.LastDisruptedTile = Tile2i.Deserialize(reader);
      this.m_groundPosition = Tile2f.Deserialize(reader);
      this.m_height = HeightTilesF.Deserialize(reader);
      this.m_isHeightComputed = reader.ReadBool();
      reader.SetField<DynamicGroundEntity>(this, "Prototype", (object) reader.ReadGenericAs<DynamicGroundEntityProto>());
      reader.SetField<DynamicGroundEntity>(this, "SurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
      reader.SetField<DynamicGroundEntity>(this, "Terrain", (object) TerrainManager.Deserialize(reader));
    }
  }
}
