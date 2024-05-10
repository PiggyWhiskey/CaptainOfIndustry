// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.ITreeHarvestingManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  /// <summary>An interface for harvesting trees.</summary>
  public interface ITreeHarvestingManager
  {
    TreeData this[TreeId id] { get; }

    bool TryGetTree(TreeId tree, out TreeData treeData);

    bool TryGetManualTree(TreeId tree, out TreeData treeData);

    bool HasTree(TreeId id);

    bool IsTreeSelected(TreeId tree);

    bool IsTreeReserved(TreeId tree);

    bool TryGetTreeAndSelected(TreeId tree, out TreeData treeData, out bool selected);

    /// <summary>Selects an existing tree to harvest.</summary>
    void AddToHarvest(TreeId tree);

    /// <summary>Removes a selected tree from harvest.</summary>
    void RemoveFromHarvest(TreeId tree);

    /// <summary>Reserves a tree for harvesting.</summary>
    bool TryReserveTree(TreeId tree);

    /// <summary>Releases harvesting reservation for a tree.</summary>
    bool TryCancelTreeReservation(TreeId tree);

    /// <summary>
    /// Finds the closest selected tree which is not reserved.
    /// </summary>
    TreeId? FindClosestNonTowerTreeForHarvestFor(
      Vehicle vehicle,
      ProductProto.ID productId,
      IReadOnlySet<TreeId> unreachableTrees,
      IReadOnlySet<Chunk2i> unreachableTreeChunks);

    /// <summary>
    /// Finds the closest harvestable tree in a valid forestry tower area/designation which has no
    /// harvesters already.
    /// </summary>
    TreeId? FindClosestTowerTreeForHarvestFor(
      Vehicle vehicle,
      ProductProto.ID productId,
      IReadOnlySet<TreeId> unreachableTrees,
      out Option<ForestryTower> tower);

    /// <summary>Harvests a tree.</summary>
    ProductQuantity HarvestTree(TreeId tree);
  }
}
