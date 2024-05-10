// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.IRoadGraphEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Roads
{
  public interface IRoadGraphEntity : 
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    int RoadLanesCount { get; }

    RoadEntityProtoBase RoadProto { get; }

    /// <summary>
    /// Returns untransformed lane metadata (entity layout transform is not applied).
    /// </summary>
    RoadLaneMetadata GetRawRoadLaneMetadata(int laneIndex);

    RoadLaneTrajectory GetTransformedRoadLane(int laneIndex);

    /// <summary>Returns transformed start/end nodes for a given lane.</summary>
    void GetLaneNodes(
      int laneIndex,
      out RoadGraphNodeKey startNodeKey,
      out RoadGraphNodeKey endNodeKey);
  }
}
