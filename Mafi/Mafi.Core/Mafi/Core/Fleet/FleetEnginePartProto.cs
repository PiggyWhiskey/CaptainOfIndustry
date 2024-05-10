// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEnginePartProto
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
  public class FleetEnginePartProto : FleetEntityPartProto
  {
    public readonly Fix32 DistancePerStep;
    public readonly Fix32 DistancePerFuel;
    public readonly Quantity FuelCapacity;

    public FleetEnginePartProto(
      FleetEntityPartProto.ID id,
      Proto.Str strings,
      AssetValue value,
      Fix32 distancePerStep,
      Fix32 distancePerFuel,
      Quantity fuelCapacity,
      int extraCrewNeeded,
      FleetEntityGfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, value, extraCrewNeeded, graphics);
      this.DistancePerStep = distancePerStep;
      this.DistancePerFuel = distancePerFuel;
      this.FuelCapacity = fuelCapacity;
    }

    public override void ApplyTo(FleetEntity entity)
    {
      base.ApplyTo(entity);
      entity.DistancePerStep += this.DistancePerStep;
      entity.DistancePerFuel += this.DistancePerFuel;
      entity.AddFuelCapacity(this.FuelCapacity);
    }

    public override void RemoveFrom(FleetEntity entity)
    {
      base.RemoveFrom(entity);
      entity.DistancePerStep -= this.DistancePerStep;
      entity.DistancePerFuel -= this.DistancePerFuel;
      entity.AddFuelCapacity(-this.FuelCapacity);
    }

    public override void ApplyToStats(FleetEntityStats stats)
    {
      base.ApplyToStats(stats);
      stats.FuelTankCapacity += this.FuelCapacity;
    }

    public Quantity GetFuelCostFromDistance(Fix32 totalDist)
    {
      return FleetEntity.GetFuelCostFromDistance(totalDist, this.DistancePerFuel);
    }
  }
}
