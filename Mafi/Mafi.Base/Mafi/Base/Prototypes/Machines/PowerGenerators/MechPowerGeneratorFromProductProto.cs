// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.MechPowerGeneratorFromProductProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  public class MechPowerGeneratorFromProductProto : 
    LayoutEntityProto,
    IProtoWithUiRecipe,
    IRecipeForUi,
    IProtoWithUpgrade<MechPowerGeneratorFromProductProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithAnimation
  {
    public readonly Duration RecipeDuration;
    public readonly ProductQuantity ConsumedProduct;
    public readonly ProductQuantity? ProducedProduct;
    public readonly MechPower MechPowerOutput;
    /// <summary>
    /// How much efficiency per tick is gained when the generator has input. This also consumes extra resources.
    /// </summary>
    public readonly Percent EfficiencyIncPerTick;
    /// <summary>
    /// How much efficiency is lost when the generator has no input or is turned off.
    /// </summary>
    public readonly Percent EfficiencyDecPerTick;
    public readonly MechPowerGeneratorFromProductProto.Gfx Graphics;

    public override Type EntityType => typeof (MechPowerGeneratorFromProduct);

    public UpgradeData<MechPowerGeneratorFromProductProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public IRecipeForUi Recipe => (IRecipeForUi) this;

    Proto.ID IRecipeForUi.Id => (Proto.ID) this.Id;

    public ImmutableArray<RecipeInput> AllUserVisibleInputs { get; }

    public ImmutableArray<RecipeOutput> AllUserVisibleOutputs { get; }

    Duration IRecipeForUi.Duration => this.RecipeDuration;

    public MechPowerGeneratorFromProductProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Duration recipeDuration,
      ProductQuantity consumedProduct,
      ProductQuantity? producedProduct,
      ProductProto mechPowerProto,
      MechPower mechPowerOutput,
      Percent efficiencyIncPerTick,
      Percent efficiencyDecPerTick,
      Option<MechPowerGeneratorFromProductProto> nextTier,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      MechPowerGeneratorFromProductProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      this.Upgrade = new UpgradeData<MechPowerGeneratorFromProductProto>(this, nextTier);
      this.RecipeDuration = recipeDuration;
      this.ConsumedProduct = consumedProduct;
      this.ProducedProduct = producedProduct;
      this.MechPowerOutput = mechPowerOutput;
      this.EfficiencyIncPerTick = efficiencyIncPerTick;
      this.EfficiencyDecPerTick = efficiencyDecPerTick;
      this.AnimationParams = animationParams;
      this.Graphics = graphics;
      this.AllUserVisibleInputs = ImmutableArray.Create<RecipeInput>(new RecipeInput(consumedProduct.Product, consumedProduct.Quantity));
      this.AllUserVisibleOutputs = producedProduct.HasValue ? ImmutableArray.Create<RecipeOutput>(new RecipeOutput(mechPowerProto, this.MechPowerOutput.Quantity), new RecipeOutput(producedProduct.Value.Product, producedProduct.Value.Quantity)) : ImmutableArray.Create<RecipeOutput>(new RecipeOutput(mechPowerProto, this.MechPowerOutput.Quantity));
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly MechPowerGeneratorFromProductProto.Gfx Empty;
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
        MechPowerGeneratorFromProductProto.Gfx.Empty = new MechPowerGeneratorFromProductProto.Gfx("EMPTY", (Option<string>) Option.None, ImmutableArray<ToolbarCategoryProto>.Empty, customIconPath);
      }
    }
  }
}
