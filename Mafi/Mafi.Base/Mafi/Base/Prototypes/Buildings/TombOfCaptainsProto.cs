// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.TombOfCaptainsProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  public sealed class TombOfCaptainsProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<TombOfCaptainsProto>,
    IProtoWithUpgrade,
    IProto
  {
    public readonly PartialProductQuantity FuelConsumptionPerDay;
    public readonly PartialProductQuantity FlowersConsumptionPerDay;
    /// <summary>Unity gain (or loss) when flowers are at 0%.</summary>
    public readonly Upoints MinUnityForFirePerMonth;
    /// <summary>Unity gain when flowers are at 100%.</summary>
    public readonly Upoints MaxUnityForFirePerMonth;
    /// <summary>Unity gain (or loss) when fire is at 0%.</summary>
    public readonly Upoints MinUnityForFlowersPerMonth;
    /// <summary>Unity gain when fire is at 100%.</summary>
    public readonly Upoints MaxUnityForFlowersPerMonth;

    public override Type EntityType => typeof (TombOfCaptains);

    public UpgradeData<TombOfCaptainsProto> Upgrade { get; private set; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public TombOfCaptainsProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      LayoutEntityProto.Gfx graphics,
      Duration constructionDurationPerProduct,
      PartialProductQuantity fuelConsumptionPerDay,
      PartialProductQuantity flowersConsumptionPerDay,
      Upoints minUnityForFirePerMonth,
      Upoints maxUnityForFirePerMonth,
      Upoints minUnityForFlowersPerMonth,
      Upoints maxUnityForFlowersPerMonth)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics, new Duration?(constructionDurationPerProduct), isUnique: true);
      this.FuelConsumptionPerDay = fuelConsumptionPerDay;
      this.FlowersConsumptionPerDay = flowersConsumptionPerDay;
      this.MinUnityForFirePerMonth = minUnityForFirePerMonth;
      this.MaxUnityForFirePerMonth = maxUnityForFirePerMonth;
      this.MinUnityForFlowersPerMonth = minUnityForFlowersPerMonth;
      this.MaxUnityForFlowersPerMonth = maxUnityForFlowersPerMonth;
      this.Upgrade = new UpgradeData<TombOfCaptainsProto>(this, (Option<TombOfCaptainsProto>) Option.None);
    }

    public void SetNextTier(Option<TombOfCaptainsProto> nextTierProto)
    {
      if (this.IsInitialized)
        Log.Error(string.Format("Failed to set next tier of '{0}', protos are already initialized.", (object) this.Id));
      else
        this.Upgrade = new UpgradeData<TombOfCaptainsProto>(this, nextTierProto);
    }
  }
}
