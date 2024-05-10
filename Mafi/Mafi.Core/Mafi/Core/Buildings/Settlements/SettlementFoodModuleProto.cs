// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementFoodModuleProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  public class SettlementFoodModuleProto : 
    LayoutEntityProto,
    ISettlementModuleProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto,
    IProtoWithUpgrade<SettlementFoodModuleProto>,
    IProtoWithUpgrade
  {
    public readonly int BuffersCount;
    public readonly Quantity CapacityPerBuffer;

    public override Type EntityType => typeof (SettlementFoodModule);

    public bool StayConnectedToLogisticsByDefault => true;

    public UpgradeData<SettlementFoodModuleProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public SettlementFoodModuleProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      int buffersCount,
      Quantity capacityPerBuffer,
      Option<SettlementFoodModuleProto> nextTier,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.BuffersCount = buffersCount;
      this.CapacityPerBuffer = capacityPerBuffer;
      this.Upgrade = new UpgradeData<SettlementFoodModuleProto>(this, nextTier);
    }
  }
}
