// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetFuelTankPartProto
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
  public class FleetFuelTankPartProto : FleetEntityPartProto
  {
    public readonly Quantity AddedFuelCapacity;

    public FleetFuelTankPartProto(
      FleetEntityPartProto.ID id,
      string name,
      AssetValue value,
      Quantity addedFuelCapacity,
      FleetEntityGfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.CreateStr((Proto.ID) id, name, "", "ship part upgrade"), value, 0, graphics);
      this.AddedFuelCapacity = addedFuelCapacity;
    }

    public override void ApplyTo(FleetEntity entity)
    {
      base.ApplyTo(entity);
      entity.AddFuelCapacity(this.AddedFuelCapacity);
    }

    public override void RemoveFrom(FleetEntity entity)
    {
      base.ApplyTo(entity);
      entity.AddFuelCapacity(-this.AddedFuelCapacity);
    }

    public override void ApplyToStats(FleetEntityStats stats)
    {
      base.ApplyToStats(stats);
      stats.FuelTankCapacity += this.AddedFuelCapacity;
    }
  }
}
