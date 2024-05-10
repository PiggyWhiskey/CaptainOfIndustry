// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.DummyRoadsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Roads
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class DummyRoadsManager : IRoadsManager
  {
    public IReadOnlyDictionary<RoadGraphNodeKey, int> RoadGraphNodes { get; }

    public IReadOnlyDictionary<Tile2iSlim, GraphTerrainConnection> TerrainGraphConnections { get; }

    public IReadOnlyDictionary<int, GraphTerrainConnection> GraphTerrainConnections { get; }

    public event Action<GraphTerrainConnection> RoadConnectionAdded;

    public event Action<GraphTerrainConnection> RoadConnectionRemoved;

    public RoadNetworkSearchStatus TrySearchForAllEntrances(
      int nodeId,
      ref int nonShortcutSteps,
      out ImmutableArray<RoadGraphPath> resultPaths)
    {
      resultPaths = ImmutableArray<RoadGraphPath>.Empty;
      return RoadNetworkSearchStatus.Success;
    }

    public RoadNetworkSearchStatus TrySearchForAllExits(
      int nodeId,
      ref int nonShortcutSteps,
      out ImmutableArray<RoadGraphPath> resultPaths)
    {
      resultPaths = ImmutableArray<RoadGraphPath>.Empty;
      return RoadNetworkSearchStatus.Success;
    }

    public DummyRoadsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RoadGraphNodes = (IReadOnlyDictionary<RoadGraphNodeKey, int>) new Dict<RoadGraphNodeKey, int>();
      this.TerrainGraphConnections = (IReadOnlyDictionary<Tile2iSlim, GraphTerrainConnection>) new Dict<Tile2iSlim, GraphTerrainConnection>();
      this.GraphTerrainConnections = (IReadOnlyDictionary<int, GraphTerrainConnection>) new Dict<int, GraphTerrainConnection>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
