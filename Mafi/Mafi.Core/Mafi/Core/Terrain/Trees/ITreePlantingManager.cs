// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.ITreePlantingManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Entities.Dynamic;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  /// <summary>An interface for harvesting trees.</summary>
  public interface ITreePlantingManager
  {
    /// <summary>
    /// Invoked immediately when a manual tree planting preview is added.
    /// </summary>
    IEvent<TreeData> TreePreviewAdded { get; }

    /// <summary>
    /// Invoked immediately when a manual tree planting preview is removed.
    /// </summary>
    IEvent<TreeData> TreePreviewRemoved { get; }

    /// <summary>Tries to plant a tree.</summary>
    bool TryPlantTree(TreeProto proto, Tile2f position, AngleSlim rotation, Percent? baseScale = null);

    /// <summary>Gets a random rotation.</summary>
    AngleSlim GetRandomPlantingRotation();

    /// <summary>
    /// Finds the closest tower than can possibly plant a tree.
    /// </summary>
    Option<ForestryTower> FindClosestTowerForPlantingFor(Vehicle vehicle);

    bool IsBlockedOrOccupied(Tile2i position);

    bool HasEnoughSpacingToOtherTrees(Tile2i position, int spacing);

    bool IsGroundFertileAtPosition(Tile2i position);

    /// <summary>
    /// Checks if it is allowed to plant a tree at this location.
    /// </summary>
    bool IsValidTileForPlanting(Tile2i position, int spacing);

    /// <summary>Tries to manually plant a tree.</summary>
    bool TryAddManualTree(
      TreeProto proto,
      Tile2f position,
      AngleSlim rotation,
      Percent? baseScale = null);

    /// <summary>Tries to remove a previously manual placed tree.</summary>
    bool TryRemoveManualTree(Tile2i position, bool replacing);

    /// <summary>
    /// Tries to get a manually placed tree to plant, and reserves if successful.
    /// </summary>
    bool TryGetAndReserveManualTree(
      Vehicle vehicle,
      IReadOnlySet<Tile2iSlim> unreachableTiles,
      out TreeData? treeData);

    /// <summary>
    /// Tries to cancel a reservation on a manually placed tree.
    /// </summary>
    bool TryCancelReserveManualTree(Tile2i plantingPosition);

    /// <summary>Checks whether a manually placed tree is reserved.</summary>
    bool HasReservedManualTree(Tile2i plantingPosition);

    /// <summary>Enumerates manually planted trees in the given area.</summary>
    IEnumerable<TreeId> EnumerateManualTreesInArea(RectangleTerrainArea2i area);
  }
}
