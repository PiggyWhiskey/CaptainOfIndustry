// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.FlyWheelEntityProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  public class FlyWheelEntityProto : LayoutEntityProto, IProtoWithAnimation
  {
    public readonly FlyWheelEntityProto.Gfx Graphics;

    public override Type EntityType => typeof (FlyWheelEntity);

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public FlyWheelEntityProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      FlyWheelEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      this.AnimationParams = animationParams;
      this.Graphics = graphics;
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly FlyWheelEntityProto.Gfx Empty;
      /// <summary>The sound the generator makes while it is operating.</summary>
      public readonly Option<string> SoundPrefabPath;

      public Gfx(
        string prefabPath,
        Option<string> soundPrefabPath,
        ImmutableArray<ToolbarCategoryProto> categories,
        Option<string> customIconPath = default (Option<string>),
        bool useSemiInstancedRendering = false)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        string prefabPath1 = prefabPath;
        Option<string> option = customIconPath;
        ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(categories);
        bool flag = useSemiInstancedRendering;
        RelTile3f prefabOrigin = new RelTile3f();
        Option<string> customIconPath1 = option;
        ColorRgba color = new ColorRgba();
        LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
        ImmutableArray<ToolbarCategoryProto>? categories1 = nullable;
        int num = flag ? 1 : 0;
        ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath1, prefabOrigin, customIconPath1, color, visualizedLayers: visualizedLayers, categories: categories1, useSemiInstancedRendering: num != 0, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
        this.SoundPrefabPath = soundPrefabPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Option<string> customIconPath = (Option<string>) "EMPTY";
        FlyWheelEntityProto.Gfx.Empty = new FlyWheelEntityProto.Gfx("EMPTY", (Option<string>) Option.None, ImmutableArray<ToolbarCategoryProto>.Empty, customIconPath);
      }
    }
  }
}
