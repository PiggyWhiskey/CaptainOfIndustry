// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.HospitalProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  public class HospitalProto : 
    LayoutEntityProto,
    ISettlementModuleProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto,
    ISettlementModuleForNeedProto,
    IProtoWithUpgrade<HospitalProto>,
    IProtoWithUpgrade,
    IProtoWithAnimation
  {
    public readonly Electricity PowerRequired;
    public readonly int BuffersCount;
    public readonly Quantity CapacityPerBuffer;
    private readonly Fix32 SuppliesConsumedPerHundredPopsPerMonth;
    public readonly int? EmissionIntensity;

    public override Type EntityType => typeof (Hospital);

    public PopNeedProto PopsNeed { get; }

    public UpgradeData<HospitalProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public HospitalProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      PopNeedProto need,
      Electricity powerRequired,
      int buffersCount,
      Quantity capacityPerBuffer,
      Fix32 suppliesConsumedPerHundredPopsPerMonth,
      Option<HospitalProto> nextTier,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      int? emissionIntensity,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.PowerRequired = powerRequired;
      this.PopsNeed = need;
      this.BuffersCount = buffersCount;
      this.CapacityPerBuffer = capacityPerBuffer;
      this.SuppliesConsumedPerHundredPopsPerMonth = suppliesConsumedPerHundredPopsPerMonth;
      this.Upgrade = new UpgradeData<HospitalProto>(this, nextTier);
      this.AnimationParams = animationParams;
      this.EmissionIntensity = emissionIntensity;
    }

    public PartialQuantity GetSuppliesConsumedQuantityFromPopDays(int popDays, Percent multiplier)
    {
      Fix32 fix32 = this.SuppliesConsumedPerHundredPopsPerMonth.ScaledBy(multiplier);
      return popDays >= 10000 ? new PartialQuantity(popDays / 30 / 100 * fix32) : new PartialQuantity(popDays * fix32 / 30 / 100);
    }
  }
}
