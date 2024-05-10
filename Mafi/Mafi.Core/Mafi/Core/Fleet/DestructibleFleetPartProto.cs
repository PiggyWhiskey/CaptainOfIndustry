// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.DestructibleFleetPartProto
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
  /// <summary>Represents fleet entity with HP and armor.</summary>
  public abstract class DestructibleFleetPartProto : FleetEntityPartProto
  {
    public readonly int MaxHp;
    public readonly int HitWeight;

    protected DestructibleFleetPartProto(
      FleetEntityPartProto.ID id,
      Proto.Str strings,
      AssetValue value,
      int maxHp,
      int hitWeight,
      int extraCrewNeeded,
      FleetEntityGfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, value, extraCrewNeeded, graphics);
      this.MaxHp = maxHp;
      this.HitWeight = hitWeight.CheckNotNegative();
    }
  }
}
