// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementDecorationModuleProto
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
  public class SettlementDecorationModuleProto : 
    LayoutEntityProto,
    ISettlementSquareModuleProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto,
    IProtoWithUpgrade<SettlementDecorationModuleProto>,
    IProtoWithUpgrade
  {
    public readonly Upoints UpointsBonusToNearbyHousing;
    public readonly int BonusRange;

    public override Type EntityType => typeof (SettlementDecorationModule);

    /// <summary>Next tier of decoration (upgrade), if available.</summary>
    public UpgradeData<SettlementDecorationModuleProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public SettlementDecorationModuleProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Upoints upointsBonusToNearbyHousing,
      int bonusRange,
      Option<SettlementDecorationModuleProto> nextTier,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.UpointsBonusToNearbyHousing = upointsBonusToNearbyHousing.CheckNotNegative();
      this.BonusRange = bonusRange.CheckNotNegative();
      this.Upgrade = new UpgradeData<SettlementDecorationModuleProto>(this, nextTier);
    }
  }
}
