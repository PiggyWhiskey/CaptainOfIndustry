// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IStaticEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Serialization;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public interface IStaticEntity : 
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    StaticEntityProto Prototype { get; }

    Tile3i CenterTile { get; }

    /// <summary>
    /// Relative tiles occupied by the entity (relative to <see cref="P:Mafi.Core.Entities.Static.IStaticEntity.CenterTile" />).
    /// </summary>
    ImmutableArray<OccupiedTileRelative> OccupiedTiles { get; }

    /// <summary>
    /// Occupied vertices by the entity (relative to <see cref="P:Mafi.Core.Entities.Static.IStaticEntity.CenterTile" />).
    /// </summary>
    ImmutableArray<OccupiedVertexRelative> OccupiedVertices { get; }

    /// <summary>
    /// Combined constraint of all <see cref="P:Mafi.Core.Entities.Static.IStaticEntity.OccupiedVertices" /> (using logical or).
    /// </summary>
    LayoutTileConstraint OccupiedVerticesCombinedConstraint { get; }

    /// <summary>
    /// Tile vertices that can be accessed by vehicles, and the height of the vehicle surface. Note that only tiles
    /// with height in all four corners are navigable.
    /// 
    /// All occupied tiles at the same 2D coordinate as vehicle tiles will be ignored for the purpose of vehicle
    /// collision for this entity. This allows entity to occupy tiles but allow vehicle access.
    /// </summary>
    ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>> VehicleSurfaceHeights { get; }

    /// <summary>
    /// Total value of this entity, including virtual products (amount payed when built). This is used to determine sell value. Entities that
    /// were obtained for free may have value of zero.
    /// </summary>
    AssetValue Value { get; }

    /// <summary>
    /// Amount of products needed to construct this entity  (virtual products are filtered out). This may be lower than
    /// <see cref="P:Mafi.Core.Entities.Static.IStaticEntity.Value" /> because some of the cost may be already payed from other entities (e.g. upgrade of entity, or extension
    /// of transport).
    /// </summary>
    AssetValue ConstructionCost { get; }

    ConstructionState ConstructionState { get; }

    Option<IEntityConstructionProgress> ConstructionProgress { get; }

    bool IsConstructed { get; }

    StaticEntityPfTargetTiles PfTargetTiles { get; }

    bool GetCustomPfTargetTiles(int retryNumber, Lyst<Tile2i> outTiles);

    ImmutableArray<IProductBufferReadOnly> GetConstructionBuffers();

    EntityValidationResult CanStartDeconstruction();

    /// <summary>
    /// In case deconstruction is pending, this is queried by the construction manager every sim to
    /// make sure the entity is working toward its deconstruction. Used in storage.
    /// </summary>
    bool CanMoveFromPendingDeconstruction();

    /// <summary>
    /// Requests entity to start a deconstruction. This can be postponed in case
    /// there is some clearing needed first like in case of storage.
    /// </summary>
    void StartDeconstructionIfCan();

    void SetConstructionState(ConstructionState state);

    bool AreConstructionCubesDisabled { get; }

    ImmutableArray<ConstrCubeSpec> GetConstructionCubesSpec(out int totalCubesVolume);

    bool DoNotAdjustTerrainDuringConstruction { get; }

    /// <summary>
    /// Notifies entity about uneven terrain and if the <paramref name="canCollapse" /> is true,
    /// <see cref="M:Mafi.Core.Entities.Static.IStaticEntity.TryCollapseOnUnevenTerrain(Mafi.Collections.IReadOnlySet{System.Int32},Mafi.Core.Entities.Static.EntityCollapseHelper)" /> will be called soon in the future.
    /// </summary>
    void NotifyUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      int newIndex,
      bool wasAdded,
      out bool canCollapse);

    /// <summary>
    /// Called when some terrain under entity constrains are violated. It is up to this method to decide whether
    /// this entity should collapse or not. Returns whether the entity was destroyed.
    /// </summary>
    /// <param name="groundVerticesViolatingConstraints">Indices of ground vertices that are violating terrain
    /// constraints.</param>
    /// <param name="collapseHelper">Use this helper to make the collapse operation.</param>
    bool TryCollapseOnUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      EntityCollapseHelper collapseHelper);
  }
}
