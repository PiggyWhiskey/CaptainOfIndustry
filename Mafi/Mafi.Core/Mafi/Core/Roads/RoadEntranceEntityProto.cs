// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadEntranceEntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Roads
{
  public class RoadEntranceEntityProto : RoadEntityProtoBase
  {
    public readonly ImmutableArray<LaneTerrainConnectionSpec> TerrainConnections;

    public override Type EntityType => typeof (RoadEntranceEntity);

    public RoadEntranceEntityProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ImmutableArray<RoadLaneSpec> lanesSpecs,
      ImmutableArray<RoadLaneMetadata> lanesData,
      ImmutableArray<RoadLaneTrajectory> lanesTrajectories,
      ImmutableArray<LaneTerrainConnectionSpec> terrainConnections,
      RoadEntityProtoBase.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, lanesSpecs, lanesData, lanesTrajectories, graphics);
      this.TerrainConnections = terrainConnections;
    }
  }
}
