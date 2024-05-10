// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.ResearchLab.ResearchLabProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.ResearchLab
{
  public sealed class ResearchLabProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public ResearchLabProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public ResearchLabProtoBuilder.State Start(
      string name,
      StaticEntityProto.ID labId,
      int tierIndex)
    {
      return new ResearchLabProtoBuilder.State(this, labId, name, tierIndex);
    }

    public class State : LayoutEntityBuilderState<ResearchLabProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_id;
      private Duration? m_durationPerStep;
      private Fix32? m_pointsPerDuration;
      private int? m_emissionIntensity;
      private int m_tierIndex;
      private ProductQuantity m_consumedPerRecipe;
      private ProductQuantity m_producedPerRecipe;
      private Lyst<AnimationParams> m_animationParams;
      private Upoints m_unityMonthlyCost;
      private Option<ResearchLabProto> m_nextTier;
      private Option<UpointsCategoryProto> m_upointsCategory;
      private Electricity m_electricityConsumed;
      private Computing m_computingConsumed;

      public State(
        ResearchLabProtoBuilder builder,
        StaticEntityProto.ID id,
        string name,
        int tierIndex)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_animationParams = new Lyst<AnimationParams>();
        this.m_unityMonthlyCost = Upoints.Zero;
        this.m_electricityConsumed = Electricity.Zero;
        this.m_computingConsumed = Computing.Zero;
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, id, name);
        this.m_id = id;
        this.m_tierIndex = tierIndex;
      }

      public ResearchLabProtoBuilder.State SetResearchSpeed(
        Duration duration,
        Fix32 stepsPerDuration)
      {
        this.m_durationPerStep = new Duration?(duration.CheckPositive());
        this.m_pointsPerDuration = new Fix32?(stepsPerDuration.CheckPositive());
        return this;
      }

      public ResearchLabProtoBuilder.State SetConsumedPerDuration(
        ProductQuantity consumedPerDuration)
      {
        this.m_consumedPerRecipe = consumedPerDuration;
        return this;
      }

      public ResearchLabProtoBuilder.State SetProducedPerDuration(
        ProductQuantity producedPerDuration)
      {
        this.m_producedPerRecipe = producedPerDuration;
        return this;
      }

      [MustUseReturnValue]
      public ResearchLabProtoBuilder.State SetNextTier(ResearchLabProto nextTier)
      {
        this.m_nextTier = (Option<ResearchLabProto>) nextTier;
        return this;
      }

      [MustUseReturnValue]
      public ResearchLabProtoBuilder.State SetElectricityConsumed(Electricity electricityConsumed)
      {
        this.m_electricityConsumed = electricityConsumed;
        return this;
      }

      [MustUseReturnValue]
      public ResearchLabProtoBuilder.State SetComputingConsumed(Computing computingConsumed)
      {
        this.m_computingConsumed = computingConsumed;
        return this;
      }

      [MustUseReturnValue]
      public ResearchLabProtoBuilder.State SetUnityMonthlyCost(
        Upoints unityMonthlyCost,
        UpointsCategoryProto upointsCategory)
      {
        this.m_unityMonthlyCost = unityMonthlyCost;
        this.m_upointsCategory = (Option<UpointsCategoryProto>) upointsCategory;
        return this;
      }

      [MustUseReturnValue]
      public ResearchLabProtoBuilder.State SetAnimationParams(AnimationParams animationParams)
      {
        this.m_animationParams.Add(animationParams);
        return this;
      }

      [MustUseReturnValue]
      public ResearchLabProtoBuilder.State SetEmissionIntensity(int intensity)
      {
        this.m_emissionIntensity = new int?(intensity);
        return this;
      }

      public ResearchLabProto BuildAndAdd()
      {
        StaticEntityProto.ID id = this.m_id;
        Proto.Str strings = this.Strings;
        EntityLayout layoutOrThrow = this.LayoutOrThrow;
        EntityCosts costs = this.Costs;
        Electricity electricityConsumed = this.m_electricityConsumed;
        Computing computingConsumed = this.m_computingConsumed;
        Upoints unityMonthlyCost = this.m_unityMonthlyCost;
        UpointsCategoryProto upointsCategory = this.ValueOrThrow<UpointsCategoryProto>(this.m_upointsCategory, "upointsCategory not set");
        Option<ResearchLabProto> nextTier1 = this.m_nextTier;
        int tierIndex1 = this.m_tierIndex;
        Duration durationForRecipe = this.ValueOrThrow<Duration>(this.m_durationPerStep, "durationForRecipe");
        Fix32 stepsPerRecipe = this.ValueOrThrow<Fix32>(this.m_pointsPerDuration, "stepsPerRecipe");
        ProductQuantity consumedPerRecipe = this.m_consumedPerRecipe;
        ProductQuantity producedPerRecipe = this.m_producedPerRecipe;
        Quantity inputBufferCapacity = 40.Quantity();
        Quantity outputBufferCapacity = 160.Quantity();
        ImmutableArray<AnimationParams> immutableArray = this.m_animationParams.ToImmutableArray();
        int? emissionIntensity = this.m_emissionIntensity;
        Option<ResearchLabProto> nextTier2 = nextTier1;
        int tierIndex2 = tierIndex1;
        LayoutEntityProto.Gfx graphics = this.Graphics;
        return this.AddToDb<ResearchLabProto>(new ResearchLabProto(id, strings, layoutOrThrow, costs, electricityConsumed, computingConsumed, unityMonthlyCost, upointsCategory, durationForRecipe, stepsPerRecipe, consumedPerRecipe, producedPerRecipe, inputBufferCapacity, outputBufferCapacity, immutableArray, emissionIntensity, nextTier2, tierIndex2, graphics));
      }
    }
  }
}
