// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.ITreesManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  public interface ITreesManager : ITreeHarvestingManager, ITreePlantingManager
  {
    /// <summary>Total number of trees in the game.</summary>
    int TreesCount { get; }

    IReadOnlyDictionary<TreeId, TreeData> Trees { get; }

    IReadOnlyDictionary<TreeId, TreeData> PreviewTrees { get; }

    IReadOnlyDictionary<TreeId, TreeStumpData> Stumps { get; }

    IEnumerable<TreeId> EnumerateSelectedTrees();

    IEnumerable<TreeId> EnumerateReservedTrees();

    IEnumerable<TreeId> EnumerateTreesInArea(
      RectangleTerrainArea2i area,
      ProductProto.ID? productIdToSelect = null,
      bool? selectedToHarvest = null);

    IEnumerable<TreeId> EnumerateStumpsInArea(RectangleTerrainArea2i area);

    /// <summary>
    /// Invoked immediately when a forestry control tower is built.
    /// </summary>
    void AddControlTower(ForestryTower tower, EntityAddReason addReason);

    void RemoveControlTower(ForestryTower tower, EntityRemoveReason removeReason);

    /// <summary>Invoked immediately when a stump is added.</summary>
    IEvent<TreeStumpData> StumpAdded { get; }

    /// <summary>Invoked immediately when a stump is removed.</summary>
    IEvent<TreeStumpData> StumpRemoved { get; }

    /// <summary>
    /// Invoked immediately when a tree is added. This event not invoked for trees generated on terrain with
    /// terrain generators.
    /// </summary>
    IEvent<TreeData> TreeAdded { get; }

    /// <summary>Invoked immediately when a tree is removed.</summary>
    IEvent<TreeData> TreeRemoved { get; }

    /// <summary>Invoked after a tree is selected to harvest.</summary>
    IEvent<TreeId> TreeAddedToHarvest { get; }

    /// <summary>
    /// Invoked after a tree is removed from trees to harvest.
    /// </summary>
    IEvent<TreeId> TreeRemovedFromHarvest { get; }

    /// <summary>
    /// Invoked after a tree is manually placed in the world (but not yet planted).
    /// </summary>
    IEvent<TreeId> ManualTreePlaced { get; }

    /// <summary>
    /// Adds a new tree to the world. The target tile should have no trees.
    /// </summary>
    bool TryAddTree(TreeData tree);

    /// <summary>Removes an existing tree from the world.</summary>
    bool TryRemoveTree(TreeId tree);

    /// <summary>Should a stump exist at a tile, removes it.</summary>
    void RemoveStumpAtTile(Tile2i tile);

    /// <summary>Number of selected trees globally.</summary>
    int SelectedToHarvestCount { get; }

    /// <summary>Invoked when a tree should collapse.</summary>
    event Action<TreeId> TreeCollapseTriggered;
  }
}
