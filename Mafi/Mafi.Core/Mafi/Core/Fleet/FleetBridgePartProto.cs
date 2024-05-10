// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetBridgePartProto
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
  public class FleetBridgePartProto : FleetEntityPartProto
  {
    public readonly FleetBridgePartProto.Gfx Graphics;
    private readonly UpgradableIntProto m_hpUpgrade;
    private readonly UpgradableIntProto m_radarRange;

    public FleetBridgePartProto(
      FleetEntityPartProto.ID id,
      Proto.Str strings,
      AssetValue value,
      UpgradableIntProto hpUpgrade,
      UpgradableIntProto radarRange,
      int extraCrewNeeded,
      FleetBridgePartProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, value, extraCrewNeeded, (FleetEntityGfx) graphics);
      this.m_hpUpgrade = hpUpgrade;
      this.m_radarRange = radarRange;
      this.Graphics = graphics;
    }

    public override void ApplyTo(FleetEntity entity)
    {
      base.ApplyTo(entity);
      entity.Hull.RadarRange.Apply(this.m_radarRange);
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
      entity.Hull.RadarRange.Remove(this.m_radarRange);
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
      stats.RadarRange += this.m_radarRange.BonusValue;
      stats.HitPoints += this.m_hpUpgrade.BonusValue;
    }

    public new class Gfx : FleetEntityGfx
    {
      public static readonly FleetBridgePartProto.Gfx Empty;
      public readonly string PrefabPath;

      public Gfx(string prefabPath, string iconPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(iconPath);
        this.PrefabPath = prefabPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        FleetBridgePartProto.Gfx.Empty = new FleetBridgePartProto.Gfx("EMPTY", "EMPTY");
      }
    }
  }
}
