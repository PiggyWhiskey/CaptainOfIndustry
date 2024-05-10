// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapPathFinder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.PathFinding;
using System;

#nullable disable
namespace Mafi.Core.World
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class WorldMapPathFinder : AStarPathFinderBase<WorldMapPathFinder.Node>, IWorldMapPathFinder
  {
    private readonly WorldMapManager m_mapManager;
    private Option<WorldMap> m_map;
    private Dict<WorldMapLocId, WorldMapPathFinder.Node> m_nodes;
    private bool m_allowOnlyExplored;
    private readonly Lyst<WorldMapLocId> m_startsLocTmp;
    private readonly Lyst<WorldMapPathFinder.Node> m_startsTmp;

    public WorldMapPathFinder(WorldMapManager mapManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_startsLocTmp = new Lyst<WorldMapLocId>(true);
      this.m_startsTmp = new Lyst<WorldMapPathFinder.Node>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      WorldMapPathFinder worldMapPathFinder = this;
      this.m_mapManager = mapManager;
      mapManager.MapReplaced += new Action<WorldMap>(this.resetMap);
      mapManager.LocationAdded += (Action<WorldMapLocation>) (x => worldMapPathFinder.resetMap(mapManager.Map));
      mapManager.LocationRemoved += (Action<WorldMapLocation>) (x => worldMapPathFinder.resetMap(mapManager.Map));
    }

    /// <summary>
    /// Does not track map changes. Great for unit tests since instantiation of <see cref="T:Mafi.Core.World.WorldMapManager" /> is
    /// pain.
    /// </summary>
    internal WorldMapPathFinder(WorldMap map)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_startsLocTmp = new Lyst<WorldMapLocId>(true);
      this.m_startsTmp = new Lyst<WorldMapPathFinder.Node>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_map = (Option<WorldMap>) map;
    }

    protected override bool KeepSearchingToFindOptimalPath => true;

    private void resetMap(WorldMap map)
    {
      this.m_map = (Option<WorldMap>) map;
      this.m_nodes = (Dict<WorldMapLocId, WorldMapPathFinder.Node>) null;
    }

    private Dict<WorldMapLocId, WorldMapPathFinder.Node> recomputeNodes()
    {
      if (this.m_map == (WorldMap) null)
        this.m_map = (Option<WorldMap>) this.m_mapManager.Map;
      return this.m_map.Value.Locations.ToDict<WorldMapLocation, WorldMapLocId, WorldMapPathFinder.Node>((Func<WorldMapLocation, WorldMapLocId>) (x => x.Id), (Func<WorldMapLocation, WorldMapPathFinder.Node>) (x => new WorldMapPathFinder.Node(x)));
    }

    public bool FindPath(
      WorldMapLocId start,
      WorldMapLocId goal,
      bool allowOnlyExplored,
      Lyst<WorldMapLocId> foundPath)
    {
      this.m_startsLocTmp.Clear();
      this.m_startsLocTmp.Add(start);
      return this.FindPath(this.m_startsLocTmp, goal, allowOnlyExplored, foundPath);
    }

    public bool FindPath(
      Lyst<WorldMapLocId> starts,
      WorldMapLocId goal,
      bool allowOnlyExplored,
      Lyst<WorldMapLocId> foundPath)
    {
      if (this.m_nodes == null)
        this.m_nodes = this.recomputeNodes();
      this.ResetState();
      this.m_startsTmp.Clear();
      foreach (WorldMapLocId start in starts)
      {
        WorldMapPathFinder.Node node;
        if (this.m_nodes.TryGetValue(start, out node))
        {
          this.m_startsTmp.Add(node);
        }
        else
        {
          Log.Error(string.Format("Invalid start node: {0}", (object) start));
          return false;
        }
      }
      WorldMapPathFinder.Node startNode;
      if (!this.m_nodes.TryGetValue(goal, out startNode))
      {
        Log.Error(string.Format("Invalid goal node: {0}", (object) goal));
        return false;
      }
      this.m_allowOnlyExplored = allowOnlyExplored;
      this.InitPathFinding(startNode, this.m_startsTmp, this.m_startsTmp.First, false);
      int iterations = 1024;
      PathFinderResult pathFinderResult = this.ContinuePathFinding(ref iterations);
      if (pathFinderResult == PathFinderResult.StillSearching)
      {
        Log.Warning(string.Format("WorldMap PathFinder needs more iterations to find path. Current max: {0}", (object) 1024));
        if (!this.SomePathAlreadyFound)
          return false;
        pathFinderResult = PathFinderResult.PathFound;
      }
      if (pathFinderResult != PathFinderResult.PathFound)
        return false;
      int count = foundPath.Count;
      this.ReconstructPath<WorldMapLocId>(foundPath, (Func<WorldMapPathFinder.Node, WorldMapLocId>) (n => n.Location.Id));
      foundPath.Reverse(count, foundPath.Count - count);
      return true;
    }

    protected override void GetNeighbors(
      WorldMapPathFinder.Node node,
      Lyst<WorldMapPathFinder.Node> outNeighbors)
    {
      Assert.That<Lyst<WorldMapPathFinder.Node>>(outNeighbors).IsEmpty<WorldMapPathFinder.Node>();
      foreach (WorldMapLocation worldMapLocation in this.m_map.Value.NeighborsOf(node.Location))
      {
        if (!this.m_allowOnlyExplored || worldMapLocation.State != WorldMapLocationState.Hidden)
          outNeighbors.Add(this.m_nodes[worldMapLocation.Id]);
      }
    }

    protected override Fix32 GetStepCost(
      WorldMapPathFinder.Node node,
      WorldMapPathFinder.Node neighbor)
    {
      return node.Location.Position.DistanceTo(neighbor.Location.Position);
    }

    protected override Fix32 EstPathCost(WorldMapPathFinder.Node from, WorldMapPathFinder.Node to)
    {
      return from.Location.Position.DistanceTo(to.Location.Position);
    }

    protected override int GetMaxPathSteps(
      WorldMapPathFinder.Node start,
      WorldMapPathFinder.Node goal)
    {
      return int.MaxValue;
    }

    public class Node : AStarNode<WorldMapPathFinder.Node>
    {
      public readonly WorldMapLocation Location;

      public Node(WorldMapLocation loc)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Location = loc.CheckNotNull<WorldMapLocation>();
      }
    }
  }
}
