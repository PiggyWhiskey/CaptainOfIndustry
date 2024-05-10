// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementModuleProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  public class SettlementModuleProto : 
    LayoutEntityProto,
    ISettlementModuleProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto,
    IProtoWithUiRecipe,
    IProtoWithAnimation
  {
    public readonly PopNeedProto PopsNeed;
    public readonly ProductProto InputProduct;
    private readonly Fix32 ConsumedPerPopPerMonth;
    public readonly Option<ProductProto> OutputProduct;
    private readonly Fix32 ProducedPerPopPerMonth;
    public readonly Quantity InputBufferCapacity;
    public readonly Quantity OutputBufferCapacity;
    public readonly bool AnimateOnlyWhenServicingPops;
    public readonly bool StayConnectedToLogisticsByDefault;
    public readonly int? EmissionIntensity;

    public override Type EntityType => typeof (SettlementServiceModule);

    public Electricity ElectricityConsumed { get; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public IRecipeForUi Recipe { get; }

    public SettlementModuleProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ProductProto inputProduct,
      Fix32 consumedPerPopPerMonth,
      Option<ProductProto> outputProduct,
      Fix32? producedPerPopPerMonth,
      Quantity inputBufferCapacity,
      Quantity outputBufferCapacity,
      bool animateOnlyWhenServicingPops,
      bool stayConnectedToLogisticsByDefault,
      PopNeedProto need,
      Electricity electricityConsumed,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      int? emissionIntensity,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.PopsNeed = need;
      this.InputProduct = inputProduct;
      this.OutputProduct = outputProduct;
      this.ConsumedPerPopPerMonth = consumedPerPopPerMonth.CheckPositive();
      this.ProducedPerPopPerMonth = producedPerPopPerMonth ?? (Fix32) -1;
      this.StayConnectedToLogisticsByDefault = stayConnectedToLogisticsByDefault;
      this.InputBufferCapacity = inputBufferCapacity;
      this.OutputBufferCapacity = outputBufferCapacity;
      this.ElectricityConsumed = electricityConsumed;
      this.AnimationParams = animationParams;
      this.EmissionIntensity = emissionIntensity;
      this.AnimateOnlyWhenServicingPops = animateOnlyWhenServicingPops;
      this.Recipe = (IRecipeForUi) new RecipeForUiData(60.Seconds(), ImmutableArray.Create<RecipeInput>(new RecipeInput(this.InputProduct, 0.Quantity())), this.OutputProduct.HasValue ? ImmutableArray.Create<RecipeOutput>(new RecipeOutput(this.OutputProduct.Value, 0.Quantity())) : (ImmutableArray<RecipeOutput>) ImmutableArray.Empty);
      if (this.InputBufferCapacity.IsNotPositive)
        throw new ProtoBuilderException(string.Format("Input capacity for {0} is not positive!", (object) id));
      if (this.OutputProduct.HasValue && this.OutputBufferCapacity.IsNotPositive)
        throw new ProtoBuilderException(string.Format("Input capacity for {0} is not positive!", (object) id));
    }

    public PartialQuantity GetConsumedPerPopPerMonth(int pops, Percent consumptionMultiplier)
    {
      return new PartialQuantity((this.ConsumedPerPopPerMonth * pops).ScaledBy(consumptionMultiplier));
    }

    public PartialQuantity GetProducedPerPopPerMonth(int pops, Percent consumptionMultiplier)
    {
      return new PartialQuantity((this.ProducedPerPopPerMonth * pops).ScaledBy(consumptionMultiplier));
    }
  }
}
