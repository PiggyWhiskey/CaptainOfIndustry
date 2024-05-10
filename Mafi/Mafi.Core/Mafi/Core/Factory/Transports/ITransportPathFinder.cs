// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.ITransportPathFinder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.PathFinding;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public interface ITransportPathFinder
  {
    Tile3i CurrentStart { get; }

    Tile3i CurrentGoal { get; }

    Tile3i OriginalGoal { get; }

    Option<TransportProto> CurrentTransportProto { get; }

    TransportPathFinderOptions Options { get; }

    void InitPathFinding(
      TransportProto proto,
      Tile3i start,
      Tile3i goal,
      TransportPathFinderOptions options,
      IEnumerable<Tile3i> bannedTiles = null);

    PathFinderResult ContinuePathFinding(ref int iterations, out ImmutableArray<Tile3i> outPivots);

    void SetUndirected();

    void ChangeGoal(Tile3i goal);

    void GetExploredTiles(Lyst<TransportPfExploredTile> exploredTiles);
  }
}
