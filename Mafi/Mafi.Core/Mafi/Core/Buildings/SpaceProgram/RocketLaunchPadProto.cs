// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.SpaceProgram.RocketLaunchPadProto
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
namespace Mafi.Core.Buildings.SpaceProgram
{
  public class RocketLaunchPadProto : LayoutEntityProto
  {
    public readonly RelTile1i RocketTransporterNavGoalDeltaX;
    public readonly RelTile1i RocketTransporterArriveDeltaY;
    public readonly RelTile1i RocketTransporterExitDeltaY;
    public readonly Duration RocketAttachDuration;
    public readonly Duration RocketCountdownDuration;
    public readonly Duration RocketLaunchDuration;
    public readonly ImmutableArray<char> WaterPortNames;
    public readonly ProductQuantity WaterPerLaunch;
    public readonly Duration WaterSprinklingDuration;
    public readonly Quantity WaterPerTick;
    public readonly RocketLaunchPadProto.Gfx Graphics;

    public override Type EntityType => typeof (RocketLaunchPad);

    public RocketLaunchPadProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityCosts costs,
      RelTile1i rocketTransporterNavGoalDeltaX,
      RelTile1i rocketTransporterArriveDeltaY,
      RelTile1i rocketTransporterExitDeltaY,
      Duration rocketAttachDuration,
      Duration rocketCountdownDuration,
      Duration rocketLaunchDuration,
      ImmutableArray<char> waterPortNames,
      ProductQuantity waterPerLaunch,
      Duration waterSprinklingDuration,
      EntityLayout layout,
      RocketLaunchPadProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics, cannotBeReflected: true);
      Assert.That<int>(waterPerLaunch.Quantity.Value % waterSprinklingDuration.Ticks).IsZero("Water quantity should be divisible by duration.");
      this.RocketTransporterNavGoalDeltaX = rocketTransporterNavGoalDeltaX;
      this.RocketTransporterArriveDeltaY = rocketTransporterArriveDeltaY;
      this.RocketTransporterExitDeltaY = rocketTransporterExitDeltaY;
      this.RocketAttachDuration = rocketAttachDuration.CheckPositive();
      this.RocketCountdownDuration = rocketCountdownDuration.CheckPositive();
      this.RocketLaunchDuration = rocketLaunchDuration.CheckPositive();
      this.WaterPortNames = waterPortNames;
      this.WaterPerLaunch = waterPerLaunch;
      this.WaterSprinklingDuration = waterSprinklingDuration.CheckPositive();
      this.WaterPerTick = waterPerLaunch.Quantity.CeilDiv(waterSprinklingDuration.Ticks);
      this.Graphics = graphics;
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public Gfx(string prefabPath, ImmutableArray<ToolbarCategoryProto> categories)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        string prefabPath1 = prefabPath;
        ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(categories);
        RelTile3f prefabOrigin = new RelTile3f();
        Option<string> customIconPath = new Option<string>();
        ColorRgba color = new ColorRgba();
        LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
        ImmutableArray<ToolbarCategoryProto>? categories1 = nullable;
        ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath1, prefabOrigin, customIconPath, color, visualizedLayers: visualizedLayers, categories: categories1, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
      }
    }
  }
}
