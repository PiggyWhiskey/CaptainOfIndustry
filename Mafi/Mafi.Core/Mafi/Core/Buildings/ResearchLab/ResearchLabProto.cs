// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.ResearchLab.ResearchLabProto
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
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.ResearchLab
{
  [DebuggerDisplay("ResearchLabProto: {Id}")]
  public class ResearchLabProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<ResearchLabProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithPowerConsumption,
    IProtoWithUnityConsumption,
    IProtoWithComputingConsumption,
    IProtoWithUiRecipes,
    IProtoWithAnimation
  {
    public int TierIndex;
    /// <summary>Duration of a single research recipe.</summary>
    public readonly Duration DurationForRecipe;
    /// <summary>
    /// Amount of steps done per each time a recipe is completed. Each research defines the required amount of steps.
    /// </summary>
    public readonly Fix32 StepsPerRecipe;
    /// <summary>Can be None</summary>
    public ProductQuantity ConsumedPerRecipe;
    /// <summary>Can be None</summary>
    public ProductQuantity ProducedPerRecipe;
    public Quantity InputBufferCapacity;
    public Quantity OutputBufferCapacity;
    public readonly int? EmissionIntensity;

    public override Type EntityType => typeof (Mafi.Core.Buildings.ResearchLab.ResearchLab);

    public UpgradeData<ResearchLabProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public Electricity ElectricityConsumed { get; }

    public Computing ComputingConsumed { get; }

    public Upoints UnityMonthlyCost { get; }

    public UpointsCategoryProto UpointsCategory { get; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public IIndexable<IRecipeForUi> Recipes { get; }

    public ResearchLabProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity electricityConsumed,
      Computing computingConsumed,
      Upoints unityMonthlyCost,
      UpointsCategoryProto upointsCategory,
      Duration durationForRecipe,
      Fix32 stepsPerRecipe,
      ProductQuantity consumedPerRecipe,
      ProductQuantity producedPerRecipe,
      Quantity inputBufferCapacity,
      Quantity outputBufferCapacity,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      int? emissionIntensity,
      Option<ResearchLabProto> nextTier,
      int tierIndex,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.ElectricityConsumed = electricityConsumed;
      this.AnimationParams = animationParams;
      this.ComputingConsumed = computingConsumed;
      this.UnityMonthlyCost = unityMonthlyCost;
      this.UpointsCategory = upointsCategory;
      this.Upgrade = new UpgradeData<ResearchLabProto>(this, nextTier);
      this.TierIndex = tierIndex;
      this.DurationForRecipe = durationForRecipe.CheckPositive();
      this.StepsPerRecipe = stepsPerRecipe.CheckPositive();
      this.ConsumedPerRecipe = consumedPerRecipe;
      this.ProducedPerRecipe = producedPerRecipe;
      this.InputBufferCapacity = inputBufferCapacity;
      this.OutputBufferCapacity = outputBufferCapacity;
      this.EmissionIntensity = emissionIntensity;
      Lyst<IRecipeForUi> lyst = new Lyst<IRecipeForUi>();
      if (producedPerRecipe.IsNotEmpty)
        lyst.Add((IRecipeForUi) new RecipeForUiData(durationForRecipe, ImmutableArray.Create<RecipeInput>(new RecipeInput(consumedPerRecipe.Product, consumedPerRecipe.Quantity)), (ImmutableArray<RecipeOutput>) ImmutableArray.Empty));
      if (producedPerRecipe.IsNotEmpty && producedPerRecipe.IsNotEmpty)
        lyst.Add((IRecipeForUi) new RecipeForUiData(durationForRecipe, ImmutableArray.Create<RecipeInput>(new RecipeInput(consumedPerRecipe.Product, consumedPerRecipe.Quantity)), ImmutableArray.Create<RecipeOutput>(new RecipeOutput(producedPerRecipe.Product, producedPerRecipe.Quantity))));
      this.Recipes = lyst.ToImmutableArray().AsIndexable;
    }

    public bool CanResearchIfRequiredLabIs(Option<ResearchLabProto> requiredLab)
    {
      return requiredLab.IsNone || this.TierIndex >= requiredLab.Value.TierIndex;
    }
  }
}
