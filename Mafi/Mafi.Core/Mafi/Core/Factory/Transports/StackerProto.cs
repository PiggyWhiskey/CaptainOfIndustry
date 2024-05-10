// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.StackerProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [DebuggerDisplay("StackerProto: {Id}")]
  public class StackerProto : LayoutEntityProto, IProtoWithPowerConsumption
  {
    public readonly ThicknessTilesI MinDumpOffset;
    public readonly ThicknessTilesI DefaultDumpOffset;
    /// <summary>
    /// The delay between accepting a product and it being dumped.
    /// </summary>
    public readonly Duration DumpDelay;
    /// <summary>Accepts (and dumps) one product every DumpPeriod.</summary>
    public readonly Duration DumpPeriod;
    /// <summary>
    /// Position of the dumping "head" relative to the centre.
    /// </summary>
    public readonly RelTile3i DumpHeadRelPos;
    /// <summary>
    /// The maximum number of products that should ever be in the queue to be dumped.
    /// </summary>
    public readonly int MaxProductsInQueue;
    public readonly StackerProto.Gfx Graphics;

    public override Type EntityType => typeof (Stacker);

    public Electricity ElectricityConsumed { get; }

    public StackerProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity consumedPowerPerTick,
      ThicknessTilesI minDumpOffset,
      RelTile3i dumpHeadRelPos,
      Duration dumpDelay,
      Duration dumpPeriod,
      StackerProto.Gfx graphics,
      ThicknessTilesI defaultDumpOffset,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntityProto.ID id1 = id;
      Proto.Str strings1 = strings;
      EntityLayout layout1 = layout;
      EntityCosts costs1 = costs;
      StackerProto.Gfx graphics1 = graphics;
      IEnumerable<Tag> tags1 = tags;
      Duration? constructionDurationPerProduct = new Duration?();
      Upoints? boostCost = new Upoints?();
      IEnumerable<Tag> tags2 = tags1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, layout1, costs1, (LayoutEntityProto.Gfx) graphics1, constructionDurationPerProduct, boostCost, tags: tags2);
      this.Graphics = graphics.CheckNotNull<StackerProto.Gfx>();
      this.MinDumpOffset = minDumpOffset.CheckPositive();
      this.DefaultDumpOffset = defaultDumpOffset.CheckGreater(this.MinDumpOffset);
      this.DumpHeadRelPos = dumpHeadRelPos;
      this.DumpDelay = dumpDelay;
      this.DumpPeriod = dumpPeriod;
      this.ElectricityConsumed = consumedPowerPerTick;
      this.MaxProductsInQueue = this.DumpDelay.Ticks.CeilDivPositive(this.DumpPeriod.Ticks) + 1;
    }

    public new class Gfx : MachineProto.Gfx
    {
      public Gfx(
        string prefabPath,
        RelTile3f prefabOffset,
        Option<string> customIconPath,
        ImmutableArray<ParticlesParams> particlesParams,
        ImmutableArray<EmissionParams> emissionsParams,
        Option<string> machineSoundPrefabPath,
        ColorRgba color,
        bool useInstancedRendering,
        bool useSemiInstancedRendering,
        LayoutEntityProto.VisualizedLayers visualizedLayers,
        ImmutableArray<ToolbarCategoryProto> categories)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        string prefabPath1 = prefabPath;
        ImmutableArray<ToolbarCategoryProto> categories1 = categories;
        RelTile3f prefabOffset1 = prefabOffset;
        Option<string> customIconPath1 = customIconPath;
        ImmutableArray<ParticlesParams> particlesParams1 = particlesParams;
        ImmutableArray<EmissionParams> emissionsParams1 = emissionsParams;
        Option<string> machineSoundPrefabPath1 = machineSoundPrefabPath;
        int num1 = useInstancedRendering ? 1 : 0;
        int num2 = useSemiInstancedRendering ? 1 : 0;
        ColorRgba colorRgba = color;
        LayoutEntityProto.VisualizedLayers? nullable = new LayoutEntityProto.VisualizedLayers?(visualizedLayers);
        ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
        ColorRgba color1 = colorRgba;
        LayoutEntityProto.VisualizedLayers? visualizedLayers1 = nullable;
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath1, categories1, prefabOffset1, customIconPath1, particlesParams1, emissionsParams1, machineSoundPrefabPath1, num1 != 0, num2 != 0, instancedRenderingExcludedObjects, color: color1, visualizedLayers: visualizedLayers1);
      }
    }
  }
}
