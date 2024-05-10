// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.QuickTrade.QuickTradePairProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.World.QuickTrade
{
  public class QuickTradePairProto : Proto
  {
    public readonly ProductQuantity ProductToBuy;
    public readonly ProductQuantity ProductToPayWith;
    /// <summary>Unity to pay per each trade / step.</summary>
    public readonly Upoints UpointsPerTrade;
    /// <summary>Max steps until sold out.</summary>
    public readonly int MaxSteps;
    /// <summary>Period to wait for each step to go back.</summary>
    public readonly Duration CooldownPerStep;
    /// <summary>
    /// How frequently we step up the price. For 2, we increase after every 2 trades.
    /// </summary>
    public readonly int TradesPerStep;
    /// <summary>Cost multiplier per each step.</summary>
    public readonly Percent CostMultiplierPerStep;
    public readonly Percent UnityMultiplierPerStep;
    public readonly int MinReputationRequired;
    public readonly bool IgnoreTradeMultipliers;

    public QuickTradePairProto(
      Proto.ID id,
      ProductQuantity productToBuy,
      ProductQuantity productToPayWith,
      Upoints upointsPerTrade,
      int maxSteps,
      int minReputationRequired,
      int tradesPerStep,
      Duration cooldownPerStep,
      Percent costMultiplierPerStep,
      Percent unityMultiplierPerStep,
      bool ignoreTradeMultipliers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.ProductToBuy = productToBuy;
      this.ProductToPayWith = productToPayWith;
      this.UpointsPerTrade = upointsPerTrade;
      this.MaxSteps = maxSteps.CheckPositive();
      this.MinReputationRequired = minReputationRequired.CheckNotNegative();
      this.TradesPerStep = tradesPerStep.CheckPositive();
      this.CooldownPerStep = cooldownPerStep;
      this.CostMultiplierPerStep = costMultiplierPerStep.CheckPositive();
      this.UnityMultiplierPerStep = unityMultiplierPerStep.CheckPositive();
      this.IgnoreTradeMultipliers = ignoreTradeMultipliers;
    }
  }
}
