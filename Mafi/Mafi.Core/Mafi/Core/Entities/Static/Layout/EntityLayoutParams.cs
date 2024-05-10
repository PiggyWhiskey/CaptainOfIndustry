// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.EntityLayoutParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  /// <summary>
  /// Extra parameters that are needed for layout construction or validation.
  /// </summary>
  public class EntityLayoutParams
  {
    public static readonly EntityLayoutParams DEFAULT;
    /// <summary>
    /// If true, it is not allowed to place a custom surface for empty tiles under this entity.
    /// This for instance protects farms and settlements.
    /// </summary>
    public readonly bool EnforceEmptySurface;
    public readonly Option<Predicate<LayoutTile>> IgnoreTilesForCore;
    public readonly ImmutableArray<CustomLayoutToken> CustomTokens;
    public readonly bool PortsCanOnlyConnectToTransports;
    public readonly Proto.ID HardenedFloorSurfaceId;
    public readonly Option<string[]> CustomVertexDataLayout;
    public readonly Option<Func<TerrainVertexRel, char, TerrainVertexRel>> CustomVertexTransformFn;
    /// <inheritdoc cref="F:Mafi.Core.Entities.Static.Layout.EntityLayout.CollapseVerticesThreshold" />
    public readonly int? CustomCollapseVerticesThreshold;
    public readonly ThicknessIRange? CustomPlacementRange;
    public readonly ImmutableArray<KeyValuePair<char, int>> CustomPortHeights;

    /// <param name="customCollapseVerticesThreshold">Entity will collapse if the number of vertices violating
    /// height constraints IS GREATER than this threshold.</param>
    public EntityLayoutParams(
      Predicate<LayoutTile> ignoreTilesForCore = null,
      IEnumerable<CustomLayoutToken> customTokens = null,
      bool portsCanOnlyConnectToTransports = false,
      Proto.ID? hardenedFloorSurfaceId = null,
      string[] customVertexDataLayout = null,
      Func<TerrainVertexRel, char, TerrainVertexRel> customVertexTransformFn = null,
      int? customCollapseVerticesThreshold = null,
      ThicknessIRange? customPlacementRange = null,
      Option<IEnumerable<KeyValuePair<char, int>>> customPortHeights = default (Option<IEnumerable<KeyValuePair<char, int>>>),
      bool enforceEmptySurface = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(customVertexDataLayout == null).IsEqualTo<bool>(customVertexTransformFn == null, "Both or neither `customVertexDataLayout` and `customVertexTransformFn` should be provided.");
      this.HardenedFloorSurfaceId = hardenedFloorSurfaceId ?? EntityLayoutParser.DEFAULT_HARDENED_SURFACE;
      this.IgnoreTilesForCore = (Option<Predicate<LayoutTile>>) ignoreTilesForCore;
      this.CustomTokens = customTokens != null ? customTokens.ToImmutableArray<CustomLayoutToken>() : (ImmutableArray<CustomLayoutToken>) ImmutableArray.Empty;
      this.PortsCanOnlyConnectToTransports = portsCanOnlyConnectToTransports;
      this.CustomVertexDataLayout = (Option<string[]>) customVertexDataLayout;
      this.CustomVertexTransformFn = (Option<Func<TerrainVertexRel, char, TerrainVertexRel>>) customVertexTransformFn;
      this.CustomCollapseVerticesThreshold = customCollapseVerticesThreshold;
      this.CustomPlacementRange = customPlacementRange;
      IEnumerable<KeyValuePair<char, int>> valueOrNull = customPortHeights.ValueOrNull;
      this.CustomPortHeights = valueOrNull != null ? valueOrNull.ToImmutableArray<KeyValuePair<char, int>>() : (ImmutableArray<KeyValuePair<char, int>>) ImmutableArray.Empty;
      this.EnforceEmptySurface = enforceEmptySurface;
    }

    static EntityLayoutParams()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityLayoutParams.DEFAULT = new EntityLayoutParams();
    }
  }
}
