// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEntityStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>
  /// Stats about ship. Mostly used in UI. Allows to preview upgrades easily.
  /// </summary>
  public class FleetEntityStats
  {
    public int HitPoints;
    public int Armor;
    public int AvgDamage;
    public int MaxWeaponRange;
    public int Crew;
    public int RadarRange;
    public Quantity FuelTankCapacity;

    public int GetBattleScore()
    {
      return this.AvgDamage == 0 ? 0 : ((float) ((double) this.HitPoints / 20.0 + (double) (this.AvgDamage * this.MaxWeaponRange) / 10.0) + (float) this.Armor).RoundToMultipleOf(10);
    }

    public FleetEntityStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
