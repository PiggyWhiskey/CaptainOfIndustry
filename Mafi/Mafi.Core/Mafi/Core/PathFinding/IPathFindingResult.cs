// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.IPathFindingResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;

#nullable disable
namespace Mafi.Core.PathFinding
{
  /// <summary>Read-only interface for path finding result.</summary>
  public interface IPathFindingResult
  {
    IVehiclePathFindingTask Task { get; }

    VehiclePfResultStatus ResultStatus { get; }

    Tile2i GoalRawTile { get; }

    Option<IVehiclePathSegment> NextPathSegment { get; }

    /// <summary>
    /// All explored tiles it they were requested. Otherwise, it's empty. Note that this is for debugging and is not
    /// saved.
    /// </summary>
    IIndexable<ExploredPfNode> ExploredTiles { get; }
  }
}
