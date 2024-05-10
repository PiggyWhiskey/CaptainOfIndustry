// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementTransformerProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  public class SettlementTransformerProto : 
    LayoutEntityProto,
    ISettlementModuleProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto
  {
    public readonly PopNeedProto PopsNeed;
    private readonly Fix32 PowerRequiredPerPop;

    public override Type EntityType => typeof (SettlementTransformer);

    public SettlementTransformerProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      PopNeedProto need,
      Fix32 powerRequiredPerPop,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.PopsNeed = need;
      this.PowerRequiredPerPop = powerRequiredPerPop;
    }

    public Fix32 GetPowerRequired(int pops, Percent consumptionMultiplier)
    {
      return (this.PowerRequiredPerPop * pops).ScaledBy(consumptionMultiplier);
    }
  }
}
