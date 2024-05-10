// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Waste.WasteSortingPlantProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Waste
{
  public class WasteSortingPlantProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<WasteSortingPlantProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithUiRecipes,
    IProtoWithPowerConsumption,
    IProtoWithAnimation
  {
    public readonly ImmutableArray<ProductQuantity> SupportedInputs;
    public readonly Quantity InputBuffersCapacity;
    public readonly Quantity OutputBuffersCapacity;
    public readonly WasteSortingPlantProto.Gfx Graphics;

    public override Type EntityType => typeof (WasteSortingPlant);

    public Duration Duration { get; }

    public UpgradeData<WasteSortingPlantProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public Electricity ElectricityConsumed { get; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public IIndexable<IRecipeForUi> Recipes { get; private set; }

    public WasteSortingPlantProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ImmutableArray<ProductQuantity> supportedInputs,
      Quantity inputBuffersCapacity,
      Quantity outputBuffersCapacity,
      Duration duration,
      Electricity electricityConsumed,
      Option<WasteSortingPlantProto> nextTier,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      WasteSortingPlantProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      this.SupportedInputs = supportedInputs;
      this.InputBuffersCapacity = inputBuffersCapacity;
      this.OutputBuffersCapacity = outputBuffersCapacity;
      this.Duration = duration;
      this.Upgrade = new UpgradeData<WasteSortingPlantProto>(this, nextTier);
      this.ElectricityConsumed = electricityConsumed;
      this.AnimationParams = animationParams;
      this.Graphics = graphics;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      ImmutableArray<RecipeOutput> immutableArray = protosDb.All<ProductProto>().Where<ProductProto>((Func<ProductProto, bool>) (x => x.IsRecyclable)).Select<ProductProto, RecipeOutput>((Func<ProductProto, RecipeOutput>) (x => new RecipeOutput(x, Quantity.Zero))).ToImmutableArray<RecipeOutput>();
      Lyst<IRecipeForUi> lyst = new Lyst<IRecipeForUi>();
      foreach (ProductQuantity supportedInput in this.SupportedInputs)
      {
        RecipeForUiData recipeForUiData = new RecipeForUiData(this.Duration, ImmutableArray.Create<RecipeInput>(new RecipeInput(supportedInput.Product, supportedInput.Quantity)), immutableArray);
        lyst.Add((IRecipeForUi) recipeForUiData);
      }
      this.Recipes = (IIndexable<IRecipeForUi>) lyst;
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public Gfx(
        string prefabPath,
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
      }
    }
  }
}
