// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Lifts.LiftProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Factory.Lifts
{
  [DebuggerDisplay("LiftProto: {Id}")]
  public class LiftProto : 
    LayoutEntityProto,
    IProtoWithPowerConsumption,
    ILayoutEntityProtoWithElevation,
    ILiftProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto,
    IProtoWithAnimation
  {
    public override Type EntityType => typeof (Lift);

    /// <summary>Can be supported by pillars.</summary>
    public bool CanBeElevated => true;

    /// <summary>
    /// If true, pillars can pass through this entity to support a higher entity.
    /// </summary>
    public bool CanPillarsPassThrough => false;

    /// <summary>Electricity consumed by the sorter.</summary>
    public Electricity ElectricityConsumed { get; }

    public IoPortShapeProto PortsShape { get; }

    /// <summary>Parameters for animation.</summary>
    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    /// <summary>
    /// Change in height compared to initial placement position.
    /// </summary>
    public ThicknessTilesI HeightDelta { get; }

    /// <summary>
    /// The minimum height delta across all protos of this type.
    /// </summary>
    public ThicknessTilesI MinHeightDelta { get; }

    /// <summary>
    /// Maximum product that can pass through this lift per step.
    /// </summary>
    public PartialQuantity MaxThroughputPerStep { get; }

    /// <summary>
    /// If set, will automatically build miniZippers at ports.
    /// </summary>
    public override bool AutoBuildMiniZippers => true;

    /// <summary>3D model of this entity.</summary>
    public LiftProto.Gfx Graphics { get; }

    public LiftProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity requiredPower,
      IoPortShapeProto portsShape,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      ThicknessTilesI heightDelta,
      ThicknessTilesI minHeightDelta,
      PartialQuantity maxThroughputPerStep,
      LiftProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      this.ElectricityConsumed = requiredPower.CheckNotNegative();
      this.PortsShape = portsShape;
      this.AnimationParams = animationParams;
      this.MinHeightDelta = minHeightDelta;
      this.MaxThroughputPerStep = maxThroughputPerStep;
      this.HeightDelta = heightDelta;
      this.Graphics = graphics;
    }

    public bool TryGetHeightReversedProto(ProtosDb protosDb, out ILiftProto newProto)
    {
      StaticEntityProto.ID liftIdFor = IdsCore.Transports.GetLiftIdFor(this.PortsShape.Id, -this.HeightDelta.Value);
      LiftProto proto1;
      bool proto2 = protosDb.TryGetProto<LiftProto>((Proto.ID) liftIdFor, out proto1);
      newProto = (ILiftProto) proto1;
      return proto2;
    }

    public bool TryGetProtoForHeightDelta(
      ProtosDb protosDb,
      int heightChange,
      out ILiftProto newProto)
    {
      StaticEntityProto.ID liftIdFor = IdsCore.Transports.GetLiftIdFor(this.PortsShape.Id, heightChange);
      LiftProto proto1;
      bool proto2 = protosDb.TryGetProto<LiftProto>((Proto.ID) liftIdFor, out proto1);
      newProto = (ILiftProto) proto1;
      return proto2;
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly LiftProto.Gfx Empty;
      /// <summary>Disable this child object when ascending.</summary>
      public readonly string AscendingChildEnabled;
      /// <summary>Disable this child object when descending.</summary>
      public readonly string DescendingChildEnabled;

      public Gfx(
        string prefabPath,
        string ascendingChildEnabled,
        string descendingChildEnabled,
        RelTile3f prefabOrigin = default (RelTile3f),
        Option<string> customIconPath = default (Option<string>),
        ColorRgba color = default (ColorRgba),
        bool hideBlockedPortsIcon = false,
        LayoutEntityProto.VisualizedLayers? visualizedLayers = null,
        ImmutableArray<ToolbarCategoryProto>? categories = null,
        bool useInstancedRendering = false,
        bool useSemiInstancedRendering = false,
        string instancedRenderingAnimationProtoSwap = null,
        IReadOnlyDictionary<string, string> instancedRenderingAnimationMaterialSwap = null,
        ImmutableArray<string> instancedRenderingExcludedObjects = default (ImmutableArray<string>),
        int maxRenderedLod = 2147483647,
        bool disableEmptyChildrenStripping = false)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, prefabOrigin, customIconPath, color, hideBlockedPortsIcon, visualizedLayers, categories, useInstancedRendering, useSemiInstancedRendering, instancedRenderingAnimationProtoSwap, instancedRenderingAnimationMaterialSwap, instancedRenderingExcludedObjects, maxRenderedLod, disableEmptyChildrenStripping);
        this.AscendingChildEnabled = ascendingChildEnabled;
        this.DescendingChildEnabled = descendingChildEnabled;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        LiftProto.Gfx.Empty = new LiftProto.Gfx("EMPTY", "EMPTY", "EMPTY", RelTile3f.Zero, (Option<string>) "EMPTY", ColorRgba.Empty, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty), categories: new ImmutableArray<ToolbarCategoryProto>?(ImmutableArray<ToolbarCategoryProto>.Empty));
      }
    }
  }
}
