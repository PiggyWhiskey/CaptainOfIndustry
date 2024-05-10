// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.SpaceProgram.RocketAssemblyBuildingProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.SpaceProgram
{
  public class RocketAssemblyBuildingProto : VehicleDepotBaseProto
  {
    public readonly Duration RoofOpenDuration;
    public readonly Duration RocketRaiseDuration;
    public readonly string RocketHolderObjectPath;

    public override Type EntityType => typeof (RocketAssemblyBuilding);

    public RocketAssemblyBuildingProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityCosts costs,
      EntityLayout layout,
      Electricity consumedPowerPerTick,
      Computing consumedComputingPerTick,
      Duration spawnInterval,
      Duration doorOpenCloseDuration,
      Duration roofOpenDuration,
      Duration rocketRaiseDuration,
      RelTile2f spawnTile,
      RelTile2f spawnDriveTile,
      RelTile2f despawnTile,
      RelTile2f despawnDriveTile,
      VehicleDepotBaseProto.Gfx graphics,
      string rocketHolderObjectPath)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, consumedPowerPerTick, consumedComputingPerTick, spawnInterval, doorOpenCloseDuration, spawnTile, spawnDriveTile, despawnTile, despawnDriveTile, Option<VehicleDepotBaseProto>.None, graphics, true);
      this.RoofOpenDuration = roofOpenDuration;
      this.RocketRaiseDuration = rocketRaiseDuration;
      this.RocketHolderObjectPath = rocketHolderObjectPath;
    }
  }
}
