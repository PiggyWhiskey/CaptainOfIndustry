// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementModuleProtoBuilder
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
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  public sealed class SettlementModuleProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public SettlementModuleProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public SettlementModuleProtoBuilder.State Start(string name, StaticEntityProto.ID labId)
    {
      return new SettlementModuleProtoBuilder.State(this, labId, name);
    }

    public class State : LayoutEntityBuilderState<SettlementModuleProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_id;
      private Fix32? m_consumedPerPopPerMonth;
      private Fix32? m_producedPerPopPerMonth;
      private Option<PopNeedProto> m_need;
      private Option<ProductProto> m_inputProduct;
      private Option<ProductProto> m_outputProduct;
      private Quantity m_inputBufferCapacity;
      private Quantity m_outputBufferCapacity;
      private readonly Lyst<AnimationParams> m_animationParams;
      private bool m_stayConnectedToLogisticsByDefault;
      private bool m_animateOnlyWhenServicingPops;
      private Electricity m_powerConsumed;
      private int? m_emissionIntensity;

      public State(SettlementModuleProtoBuilder builder, StaticEntityProto.ID id, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_animationParams = new Lyst<AnimationParams>();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, id, name);
        this.m_id = id;
      }

      [MustUseReturnValue]
      public SettlementModuleProtoBuilder.State SetNeed(PopNeedProto needProto)
      {
        this.m_need = (Option<PopNeedProto>) needProto;
        return this;
      }

      [MustUseReturnValue]
      public SettlementModuleProtoBuilder.State SetAnimationParams(AnimationParams animationParams)
      {
        this.m_animationParams.Add(animationParams);
        return this;
      }

      [MustUseReturnValue]
      public SettlementModuleProtoBuilder.State SetStayConnectedToLogisticsByDefault()
      {
        this.m_stayConnectedToLogisticsByDefault = true;
        return this;
      }

      [MustUseReturnValue]
      public SettlementModuleProtoBuilder.State AnimateOnlyWhenServicingPops()
      {
        this.m_animateOnlyWhenServicingPops = true;
        return this;
      }

      [MustUseReturnValue]
      public SettlementModuleProtoBuilder.State SetEmissionIntensity(int intensity)
      {
        this.m_emissionIntensity = new int?(intensity);
        return this;
      }

      [MustUseReturnValue]
      public SettlementModuleProtoBuilder.State SetElectricityConsumed(Electricity consumed)
      {
        this.m_powerConsumed = consumed;
        return this;
      }

      [MustUseReturnValue]
      public SettlementModuleProtoBuilder.State SetInput(
        ProductProto product,
        Fix32 consumedPerPopPerMonth,
        int capacity)
      {
        Assert.That<Option<PopNeedProto>>(this.m_need).HasValue<PopNeedProto>();
        this.m_consumedPerPopPerMonth = new Fix32?(consumedPerPopPerMonth);
        this.m_inputProduct = (Option<ProductProto>) product;
        this.m_inputBufferCapacity = capacity.Quantity();
        return this;
      }

      [MustUseReturnValue]
      public SettlementModuleProtoBuilder.State SetOutput(
        ProductProto product,
        Fix32 producedPerPopPerMonth,
        int capacity)
      {
        Assert.That<Option<PopNeedProto>>(this.m_need).HasValue<PopNeedProto>();
        this.m_producedPerPopPerMonth = new Fix32?(producedPerPopPerMonth);
        this.m_outputProduct = (Option<ProductProto>) product;
        this.m_outputBufferCapacity = capacity.Quantity();
        return this;
      }

      public SettlementModuleProto BuildAndAdd()
      {
        StaticEntityProto.ID id = this.m_id;
        Proto.Str strings = this.Strings;
        EntityLayout layoutOrThrow = this.LayoutOrThrow;
        EntityCosts costs = this.Costs;
        ProductProto inputProduct = this.ValueOrThrow<ProductProto>(this.m_inputProduct, "No input set!");
        Fix32 consumedPerPopPerMonth = this.ValueOrThrow<Fix32>(this.m_consumedPerPopPerMonth, "Pops served per day not set!");
        Option<ProductProto> outputProduct = this.m_outputProduct;
        Fix32? producedPerPopPerMonth = this.m_producedPerPopPerMonth ?? this.m_consumedPerPopPerMonth;
        Quantity inputBufferCapacity = this.m_inputBufferCapacity;
        Quantity outputBufferCapacity = this.m_outputBufferCapacity;
        PopNeedProto popNeedProto = this.ValueOrThrow<PopNeedProto>(this.m_need, "No input set!");
        int num1 = this.m_animateOnlyWhenServicingPops ? 1 : 0;
        int num2 = this.m_stayConnectedToLogisticsByDefault ? 1 : 0;
        PopNeedProto need = popNeedProto;
        Electricity powerConsumed = this.m_powerConsumed;
        ImmutableArray<AnimationParams> immutableArray = this.m_animationParams.ToImmutableArray();
        int? emissionIntensity = this.m_emissionIntensity;
        LayoutEntityProto.Gfx graphics = this.Graphics;
        return this.AddToDb<SettlementModuleProto>(new SettlementModuleProto(id, strings, layoutOrThrow, costs, inputProduct, consumedPerPopPerMonth, outputProduct, producedPerPopPerMonth, inputBufferCapacity, outputBufferCapacity, num1 != 0, num2 != 0, need, powerConsumed, immutableArray, emissionIntensity, graphics));
      }
    }
  }
}
