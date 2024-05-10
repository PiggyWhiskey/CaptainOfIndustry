// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.UpgradeHullProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>Upgrades hull properties.</summary>
  public class UpgradeHullProto : FleetEntityPartProto
  {
    private readonly UpgradableIntProto m_hpUpgrade;
    private readonly UpgradableIntProto m_armorUpgrade;

    public UpgradeHullProto(
      FleetEntityPartProto.ID id,
      string name,
      AssetValue value,
      UpgradableIntProto hpUpgrade,
      UpgradableIntProto armorUpgrade,
      FleetEntityGfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.CreateStr((Proto.ID) id, name, "", "ship part upgrade"), value, 0, graphics);
      this.m_hpUpgrade = hpUpgrade;
      this.m_armorUpgrade = armorUpgrade;
    }

    public override void ApplyTo(FleetEntity entity)
    {
      base.ApplyTo(entity);
      entity.Armor.Apply(this.m_armorUpgrade);
      int num1 = entity.Hull.MaxHp.GetValue();
      entity.Hull.ApplyMaxHpUpgrade(this.m_hpUpgrade);
      int num2 = entity.Hull.MaxHp.GetValue() - num1;
      if (num2 <= 0)
        return;
      entity.Hull.SetHp(entity.Hull.CurrentHp + num2);
    }

    public override void RemoveFrom(FleetEntity entity)
    {
      base.RemoveFrom(entity);
      entity.Armor.Remove(this.m_armorUpgrade);
      int num1 = entity.Hull.MaxHp.GetValue();
      entity.Hull.RemoveMaxHpUpgrade(this.m_hpUpgrade);
      int num2 = num1 - entity.Hull.MaxHp.GetValue();
      if (num2 <= 0)
        return;
      entity.Hull.SetHp(entity.Hull.CurrentHp - num2);
    }

    public override void ApplyToStats(FleetEntityStats stats)
    {
      base.ApplyToStats(stats);
      stats.Armor += this.m_armorUpgrade.BonusValue;
      stats.HitPoints += this.m_hpUpgrade.BonusValue;
    }
  }
}
