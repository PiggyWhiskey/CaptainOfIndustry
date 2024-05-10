// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Validators.IEntityWithOccupiedTilesAddRequest
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static.Layout;
using System;

#nullable disable
namespace Mafi.Core.Entities.Validators
{
  /// <summary>
  /// Extends the request with the info that this entity occupies tiles in the world. That will allow validators to
  /// check for collisions.
  /// </summary>
  public interface IEntityWithOccupiedTilesAddRequest : IEntityAddRequest
  {
    /// <summary>Origin (center tile) of the entity.</summary>
    Tile3i Origin { get; }

    /// <summary>
    /// Occupied tiles of this entity to be checked for collisions.
    /// </summary>
    ReadOnlyArray<OccupiedTileRelative> OccupiedTiles { get; }

    ReadOnlyArray<OccupiedVertexRelative> OccupiedVertices { get; }

    /// <summary>
    /// Optional predicate that specifies whether to ignore certain entities. By default no entities are ignored.
    /// </summary>
    Option<Predicate<EntityId>> IgnoreForCollisions { get; }

    bool RecordTileErrorsAndMetadata { get; }

    bool GetTileError(int occupiedTileIndex);

    void SetTileError(int occupiedTileIndex);

    void AddMetadata(IAddRequestMetadata metadata);
  }
}
