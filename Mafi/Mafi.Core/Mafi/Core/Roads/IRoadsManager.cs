// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.IRoadsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Roads
{
  public interface IRoadsManager
  {
    IReadOnlyDictionary<RoadGraphNodeKey, int> RoadGraphNodes { get; }

    IReadOnlyDictionary<Tile2iSlim, GraphTerrainConnection> TerrainGraphConnections { get; }

    IReadOnlyDictionary<int, GraphTerrainConnection> GraphTerrainConnections { get; }

    event Action<GraphTerrainConnection> RoadConnectionAdded;

    event Action<GraphTerrainConnection> RoadConnectionRemoved;

    RoadNetworkSearchStatus TrySearchForAllExits(
      int nodeId,
      ref int nonShortcutSteps,
      out ImmutableArray<RoadGraphPath> resultPaths);

    RoadNetworkSearchStatus TrySearchForAllEntrances(
      int nodeId,
      ref int nonShortcutSteps,
      out ImmutableArray<RoadGraphPath> resultPaths);
  }
}
