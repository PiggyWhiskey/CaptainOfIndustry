// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.OreSortingPlantProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.OreSorting
{
  public class OreSortingPlantProto : 
    LayoutEntityProto,
    IProtoWithPowerConsumption,
    IProtoWithAnimation
  {
    public readonly Quantity InputBufferCapacity;
    public readonly Quantity OutputBuffersCapacity;
    public readonly OreSortingPlantProto.Gfx Graphics;

    public override Type EntityType => typeof (OreSortingPlant);

    public Percent ConversionLoss { get; }

    public Duration Duration { get; }

    public Quantity QuantityPerDuration { get; }

    public Electricity ElectricityConsumed { get; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public OreSortingPlantProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Quantity inputBufferCapacity,
      Quantity outputBuffersCapacity,
      Duration duration,
      Quantity quantityPerDuration,
      Percent conversionLoss,
      Electricity electricityConsumed,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      OreSortingPlantProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      this.InputBufferCapacity = inputBufferCapacity;
      this.OutputBuffersCapacity = outputBuffersCapacity;
      this.Duration = duration;
      this.QuantityPerDuration = quantityPerDuration;
      this.ConversionLoss = conversionLoss;
      this.ElectricityConsumed = electricityConsumed;
      this.AnimationParams = animationParams;
      this.Graphics = graphics;
      if (this.OutputPorts.Length != 4)
        throw new InvalidProtoException(string.Format("Sorter needs to have exactly {0} ports!", (object) 4));
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      protosDb.TrackProperty((IProtoWithPropertiesUpdate) this, IdsCore.PropertyIds.OreSortingEnabled.Value);
    }

    public override void OnPropertyUpdated(IProperty property)
    {
      base.OnPropertyUpdated(property);
      bool isAvailable;
      if (!property.TryGetValueAs<bool>(IdsCore.PropertyIds.OreSortingEnabled, out isAvailable))
        return;
      this.SetAvailabilityRuntime(isAvailable);
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public readonly string SmoothPileObjectPath;
      public readonly LoosePileTextureParams PileTextureParams;

      public Gfx(
        string prefabPath,
        string smoothPileObjectPath,
        LoosePileTextureParams loosePileTextureParams,
        RelTile3f prefabOrigin = default (RelTile3f),
        Option<string> customIconPath = default (Option<string>),
        ColorRgba color = default (ColorRgba),
        bool hideBlockedPortsIcon = false,
        LayoutEntityProto.VisualizedLayers? visualizedLayers = null,
        ImmutableArray<ToolbarCategoryProto>? categories = null,
        bool useSemiInstancedRendering = false)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, prefabOrigin, customIconPath, color, hideBlockedPortsIcon, visualizedLayers, categories, useSemiInstancedRendering: useSemiInstancedRendering);
        this.SmoothPileObjectPath = smoothPileObjectPath;
        this.PileTextureParams = loosePileTextureParams;
      }
    }
  }
}
