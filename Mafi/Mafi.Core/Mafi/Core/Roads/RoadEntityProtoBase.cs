// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadEntityProtoBase
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
  public abstract class RoadEntityProtoBase : LayoutEntityProto
  {
    public readonly ImmutableArray<RoadLaneSpec> LanesSpecs;
    public readonly ImmutableArray<RoadLaneMetadata> LanesData;
    public readonly ImmutableArray<RoadLaneTrajectory> LanesTrajectories;
    private readonly ImmutableArray<RoadLaneTrajectory>[] m_transformedTrajectoriesCache;

    public RoadEntityProtoBase.Gfx Graphics { get; }

    protected RoadEntityProtoBase(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ImmutableArray<RoadLaneSpec> lanesSpecs,
      ImmutableArray<RoadLaneMetadata> lanesData,
      ImmutableArray<RoadLaneTrajectory> lanesTrajectories,
      RoadEntityProtoBase.Gfx graphics,
      Duration? constructionDurationPerProduct = null,
      Upoints? boostCost = null,
      bool cannotBeBuiltByPlayer = false,
      bool isUnique = false,
      bool cannotBeReflected = false,
      bool doNotStartConstructionAutomatically = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics, constructionDurationPerProduct, boostCost, cannotBeBuiltByPlayer, isUnique, cannotBeReflected, doNotStartConstructionAutomatically);
      Assert.That<int>(lanesData.Length).IsEqualTo(lanesTrajectories.Length);
      Assert.That<int>(lanesData.Length).IsEqualTo(lanesSpecs.Length);
      this.LanesSpecs = lanesSpecs;
      this.LanesData = lanesData;
      this.LanesTrajectories = lanesTrajectories;
      this.Graphics = graphics;
      this.m_transformedTrajectoriesCache = new ImmutableArray<RoadLaneTrajectory>[8];
    }

    public RoadLaneTrajectory GetTransformedLane(TileTransform transform, int laneIndex)
    {
      return this.GetTransformedLanes(transform)[laneIndex];
    }

    public RoadGraphNodeKey GetTransformedRoadGraphNode(
      RelTile3f pos,
      RoadGraphNodeDirection dir,
      TileTransform transform)
    {
      return RoadGraphNodeKey.FromPosition(this.Layout.TransformPoint_RelToCenterTile(pos, transform), new RoadGraphNodeDirection(this.Layout.TransformDirection(dir.DirectionSigns, transform)));
    }

    public ImmutableArray<RoadLaneTrajectory> GetTransformedLanes(TileTransform transform)
    {
      int rawValue = (int) transform.Transform90RotFlip.RawValue;
      ImmutableArray<RoadLaneTrajectory> transformedTrajectory = this.m_transformedTrajectoriesCache[rawValue];
      if (transformedTrajectory.IsNotValid)
        this.m_transformedTrajectoriesCache[rawValue] = transformedTrajectory = this.ComputeTransformedTrajectory(transform);
      return transformedTrajectory;
    }

    protected ImmutableArray<RoadLaneTrajectory> ComputeTransformedTrajectory(
      TileTransform transform)
    {
      return this.LanesTrajectories.Map<RoadLaneTrajectory, TileTransform>(transform, (Func<RoadLaneTrajectory, TileTransform, RoadLaneTrajectory>) ((x, traj) => new RoadLaneTrajectory(x.LaneCenterSamples.Map<RelTile3f, TileTransform>(traj, (Func<RelTile3f, TileTransform, RelTile3f>) ((s, t) => this.Layout.TransformRelativeF_Point_RelToCenterTile(s, t))), x.LaneDirectionSamples.Map<RelTile3f, TileTransform>(traj, (Func<RelTile3f, TileTransform, RelTile3f>) ((s, t) => this.Layout.TransformDirection(s, t))), x.SegmentLengthsPrefixSums)));
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public readonly int MeshSegmentsCount;
      public readonly string MaterialPath;

      public Gfx(
        int meshSegmentsCount,
        string materialPath,
        ImmutableArray<ToolbarCategoryProto>? categories)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector("NO_PREFAB", categories: categories);
        this.MeshSegmentsCount = meshSegmentsCount;
        this.MaterialPath = materialPath;
      }
    }
  }
}
