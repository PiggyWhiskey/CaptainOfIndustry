// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.UnlockingConditionGlobalStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Research
{
  public class UnlockingConditionGlobalStats : IResearchNodeUnlockingCondition
  {
    public static readonly LocStr LIFETIME_PRODUCTION;
    public readonly ProductProto ProductToTrack;
    public readonly QuantityLarge QuantityRequired;
    public readonly LocStr Tooltip;
    public QuantityLarge CurrentQuantity;
    private readonly Func<ProductStats, QuantityLarge> m_statsSelector;
    private ProductStats m_productStats;

    public UnlockingConditionGlobalStats(
      ProductProto productToTrack,
      int quantityRequired,
      Func<ProductStats, QuantityLarge> statsSelector,
      LocStr tooltip)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProductToTrack = productToTrack;
      this.QuantityRequired = new QuantityLarge((long) quantityRequired);
      this.m_statsSelector = statsSelector;
      this.Tooltip = tooltip;
    }

    private void initialize(ProductsManager productsManager)
    {
      this.m_productStats = productsManager.GetStatsFor(this.ProductToTrack);
    }

    private void update()
    {
      this.CurrentQuantity = this.m_statsSelector(this.m_productStats).Min(this.QuantityRequired);
    }

    static UnlockingConditionGlobalStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnlockingConditionGlobalStats.LIFETIME_PRODUCTION = Loc.Str("ResearchLocked_LifetimeProduction", "Shows the total accumulated lifetime production you need to achieve in order to get enough experience to perform this research.", "tooltip for a product & quantity that player needs to produce per lifetime");
    }

    [GlobalDependency(RegistrationMode.AsEverything, false, false)]
    public class Manager : UnlockingManagerBase<UnlockingConditionGlobalStats>
    {
      private readonly ISimLoopEvents m_simLoopEvents;
      private readonly ProductsManager m_productsManager;

      public Manager(ISimLoopEvents simLoopEvents, ProductsManager productsManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_simLoopEvents = simLoopEvents;
        this.m_productsManager = productsManager;
        simLoopEvents.Update.AddNonSaveable<UnlockingConditionGlobalStats.Manager>(this, new Action(this.simUpdate));
      }

      private void simUpdate()
      {
        if (this.m_simLoopEvents.CurrentStep.Value % 11 == 0)
          return;
        this.UpdateAllConditions();
      }

      protected override void InitializeCondition(UnlockingConditionGlobalStats condition)
      {
        condition.initialize(this.m_productsManager);
      }

      protected override bool IsConditionSatisfied(UnlockingConditionGlobalStats condition)
      {
        condition.update();
        return condition.CurrentQuantity >= condition.QuantityRequired;
      }
    }
  }
}
