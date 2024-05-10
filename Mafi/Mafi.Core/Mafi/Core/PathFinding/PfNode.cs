// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.PfNode
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Roads;
using Mafi.Core.Terrain;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding
{
  public class PfNode : IEquatable<PfNode>, IIsSafeAsHashKey
  {
    public readonly RectangleTerrainArea2i Area;
    public readonly ClearancePathabilityProvider.CapabilityChunkData ParentChunk;
    private Option<Lyst<PfNode.Edge>> m_neighbors;
    private int m_pathLength;
    private bool m_isVisitedFromStart;

    /// <summary>
    /// Current neighbors that might not include all available due to non-existing or dirty neighbors.
    /// </summary>
    public IIndexable<PfNode.Edge> CurrentNeighbors
    {
      get => (IIndexable<PfNode.Edge>) this.m_neighbors.ValueOrNull ?? Indexable<PfNode.Edge>.Empty;
    }

    public bool IsDestroyed => this.m_neighbors.IsNone;

    public Fix32 CurrentCost { get; private set; }

    public Option<PfNode> ParentNodeOnPath { get; private set; }

    public Option<RoadGraphPath> RoadConnectionToParent { get; private set; }

    public int PathLength
    {
      get
      {
        Assert.That<Option<PfNode>>(this.ParentNodeOnPath).HasValue<PfNode>("Querying `PathLength` on node that is not visited.");
        return this.m_pathLength;
      }
    }

    public bool IsVisitedFromStart
    {
      get
      {
        Assert.That<Option<PfNode>>(this.ParentNodeOnPath).HasValue<PfNode>("Querying `IsVisitedFromStart` on node that is not visited.");
        return this.m_isVisitedFromStart;
      }
    }

    /// <summary>
    /// Whether this node was visited but its neighbors were not explored yet.
    /// </summary>
    public bool IsVisited { get; private set; }

    /// <summary>
    /// Whether this node is processed all its neighbors have been explored.
    /// </summary>
    public bool IsProcessed { get; private set; }

    /// <summary>
    /// Whether this node has parent that is not equal to itself. By our convention, start/goal nodes have parents
    /// set to themselves.
    /// </summary>
    public bool HasParent
    {
      get
      {
        Assert.That<Option<PfNode>>(this.ParentNodeOnPath).HasValue<PfNode>("Querying `HasParent` on non-initialized node.");
        return this.ParentNodeOnPath != this;
      }
    }

    public bool IsDirty => this.ParentChunk.IsDirty;

    public PfNode(
      ClearancePathabilityProvider.CapabilityChunkData parentChunk,
      RectangleTerrainArea2i area)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_neighbors = (Option<Lyst<PfNode.Edge>>) new Lyst<PfNode.Edge>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Tile2i>(area.Origin).IsGreaterOrEqual<Tile2i>((Tile2i) parentChunk.Parent.OriginTile.TileCoordSlim, "PF Node origin is not inside of its parent.");
      Assert.That<Tile2i>(area.Origin).IsLess<Tile2i>((Tile2i) parentChunk.Parent.OriginTile.TileCoordSlim.AddXy(8), "PF Node origin is not inside of its parent.");
      Assert.That<Tile2i>(area.PlusXyCoordExcl).IsLessOrEqual<Tile2i>((Tile2i) parentChunk.Parent.OriginTile.TileCoordSlim.AddXy(8), "PF Node is not contained inside of its parent.");
      this.ParentChunk = parentChunk;
      this.Area = area;
    }

    public PfNode.Edge GetEdgeTo(PfNode other)
    {
      if (this.m_neighbors.HasValue)
      {
        int index = this.m_neighbors.Value.IndexOf<PfNode.Edge, PfNode>(other, (Func<PfNode.Edge, PfNode>) (e => e.Node));
        if (index >= 0)
          return this.m_neighbors.Value[index];
      }
      Assert.Fail("Connection neighbor not found.");
      return new PfNode.Edge();
    }

    public PfNode GetParentSafe() => this.ParentNodeOnPath.ValueOrNull ?? this;

    public virtual void Initialize(bool isStartNode)
    {
      Assert.That<Option<PfNode>>(this.ParentNodeOnPath).IsNone<PfNode>("Node is already initialized.");
      this.CurrentCost = (Fix32) 0;
      this.m_pathLength = 0;
      this.m_isVisitedFromStart = isStartNode;
      this.ParentNodeOnPath = (Option<PfNode>) this;
    }

    public bool TrySetParent(
      PfNode parentOnPath,
      Fix32 newTotalCost,
      Option<RoadGraphPath> roadConnectionToParent)
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse();
      Assert.That<bool>(parentOnPath.IsVisited).IsTrue();
      Assert.That<bool>(parentOnPath.IsDestroyed).IsFalse();
      Assert.That<Fix32>(newTotalCost).IsGreaterOrEqual(parentOnPath.CurrentCost);
      this.ParentNodeOnPath = (Option<PfNode>) parentOnPath;
      this.RoadConnectionToParent = roadConnectionToParent;
      this.CurrentCost = newTotalCost;
      this.m_isVisitedFromStart = parentOnPath.IsVisitedFromStart;
      this.m_pathLength = parentOnPath.PathLength + 1;
      return true;
    }

    public void Clear()
    {
      Assert.That<Option<PfNode>>(this.ParentNodeOnPath).HasValue<PfNode>("Node is not initialized.");
      this.ParentNodeOnPath = Option<PfNode>.None;
      this.RoadConnectionToParent = Option<RoadGraphPath>.None;
      this.IsVisited = false;
      this.IsProcessed = false;
    }

    public void SetIsVisited()
    {
      Assert.That<Option<PfNode>>(this.ParentNodeOnPath).HasValue<PfNode>("Node is not initialized.");
      Assert.That<bool>(this.IsVisited).IsFalse("Node was already visited.");
      this.IsVisited = true;
    }

    public void SetIsProcessed()
    {
      Assert.That<Option<PfNode>>(this.ParentNodeOnPath).HasValue<PfNode>("Node is not initialized.");
      Assert.That<bool>(this.IsVisited).IsTrue("Node was not visited.");
      Assert.That<bool>(this.IsProcessed).IsFalse("Node was already processed.");
      this.IsProcessed = true;
    }

    /// <summary>
    /// Returns edges to all neighbors. This ensures that all surrounding chunks have valid non-dirty data.
    /// </summary>
    public IIndexable<PfNode.Edge> GetAllValidNeighbors()
    {
      if (this.m_neighbors.IsNone)
      {
        Log.Error("Attempting to get neighbors of destroyed node.");
        return Indexable<PfNode.Edge>.Empty;
      }
      this.EnsureAllPfNeighbors();
      return (IIndexable<PfNode.Edge>) this.m_neighbors.Value;
    }

    /// <summary>
    /// Ensures neighbors that this node potentially touches are not dirty if no nodes are visited.
    /// </summary>
    public void EnsureAllPfNeighbors()
    {
      this.ParentChunk.Parent.EnsureAllNeighbors();
      int capabilityIndex = this.ParentChunk.CapabilityIndex;
      ClearancePathabilityProvider.DataChunk parent = this.ParentChunk.Parent;
      Tile2i tile2i1 = this.Area.Origin & 7;
      if (tile2i1.X == 0)
        parent.MinusXNeighbor.Value.GetOrCreatePfData(capabilityIndex).EnsureNotDirtyIfNoNodesVisited();
      if (tile2i1.Y == 0)
        parent.MinusYNeighbor.Value.GetOrCreatePfData(capabilityIndex).EnsureNotDirtyIfNoNodesVisited();
      Tile2i tile2i2 = this.Area.PlusXyCoordExcl & 7;
      if (tile2i2.X == 0)
        parent.PlusXNeighbor.Value.GetOrCreatePfData(capabilityIndex).EnsureNotDirtyIfNoNodesVisited();
      if (tile2i2.Y != 0)
        return;
      parent.PlusYNeighbor.Value.GetOrCreatePfData(capabilityIndex).EnsureNotDirtyIfNoNodesVisited();
    }

    public void ConnectTo(PfNode neighbor, Line2i connLine, Direction90 neighborDirection)
    {
      Assert.That<bool>(this.IsProcessed).IsFalse<PfNode, PfNode>("Connecting node {0}, that was already processed, to node {1}.", this, neighbor);
      Assert.That<bool>(neighbor.IsProcessed).IsFalse<PfNode, PfNode>("Connecting node {0}, that was already processed, to node {1}.", neighbor, this);
      if (this.m_neighbors.IsNone || neighbor.m_neighbors.IsNone)
      {
        Log.Error("Attempting to connect destroyed node.");
      }
      else
      {
        Fix32 distance = this.Area.CenterCoord.DistanceTo(neighbor.Area.CenterCoord);
        this.m_neighbors.Value.Add(new PfNode.Edge(neighbor, distance, connLine, neighborDirection));
        Vector2i directionVector = neighborDirection.DirectionVector;
        Line2i connectionLine = new Line2i(connLine.P0 + directionVector, connLine.P1 + directionVector);
        neighbor.m_neighbors.Value.Add(new PfNode.Edge(this, distance, connectionLine, neighborDirection.Rotated180));
      }
    }

    public void DisconnectAndDestroy()
    {
      Assert.That<bool>(this.IsVisited).IsFalse("Destroying node that is currently in PF queue.");
      if (this.m_neighbors.IsNone)
      {
        Log.Error("Attempting to destroy already destroyed node.");
      }
      else
      {
        foreach (PfNode.Edge edge in this.m_neighbors.Value)
          edge.Node.removeNeighbor(this);
        this.m_neighbors = Option<Lyst<PfNode.Edge>>.None;
        this.ParentNodeOnPath = Option<PfNode>.None;
      }
    }

    private void removeNeighbor(PfNode pfNode)
    {
      Assert.That<bool>(this.IsProcessed).IsFalse("Removing neighbor from processed node.");
      if (this.m_neighbors.IsNone)
      {
        Log.Error(string.Format("No neighbors of PF node {0}!", (object) pfNode));
      }
      else
      {
        int index = this.m_neighbors.Value.IndexOf<PfNode.Edge, PfNode>(pfNode, (Func<PfNode.Edge, PfNode>) (x => x.Node));
        if (index < 0)
          Assert.Fail(string.Format("Failed to remove neighbor {0} from {1}.", (object) pfNode, (object) this));
        else
          this.m_neighbors.Value.RemoveAtReplaceWithLast(index);
      }
    }

    public override string ToString()
    {
      return string.Format("{0}+{1}, from {2}", (object) this.Area.Origin, (object) this.Area.Size, this.IsVisitedFromStart ? (object) "start" : (object) "end");
    }

    public bool Equals(PfNode other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      if (!this.Area.Equals(other.Area))
        return false;
      Log.Error(string.Format("PfNode instances that are not equal references have equal areas: {0}", (object) this.Area));
      return true;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      if (!(obj is PfNode pfNode) || !this.Area.Equals(pfNode.Area))
        return false;
      Log.Error(string.Format("PfNode instances that are not equal references have equal areas: {0}", (object) this.Area));
      return true;
    }

    public override int GetHashCode() => this.Area.GetHashCode();

    public readonly struct Edge
    {
      public readonly PfNode Node;
      public readonly Fix32 Distance;
      public readonly Line2i ConnectionLine;
      public readonly Direction90 NeighborDirection;

      public Line2i OtherConnectionLine
      {
        get
        {
          Vector2i directionVector = this.NeighborDirection.DirectionVector;
          return new Line2i(this.ConnectionLine.P0 + directionVector, this.ConnectionLine.P1 + directionVector);
        }
      }

      public Edge(
        PfNode node,
        Fix32 distance,
        Line2i connectionLine,
        Direction90 neighborDirection)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Node = node;
        this.Distance = distance;
        this.ConnectionLine = connectionLine;
        this.NeighborDirection = neighborDirection;
      }

      public override string ToString()
      {
        return string.Format("Edge to {0} ({1})", (object) this.Node, (object) this.NeighborDirection);
      }
    }
  }
}
