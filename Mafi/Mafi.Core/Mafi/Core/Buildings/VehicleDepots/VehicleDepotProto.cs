// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.VehicleDepots.VehicleDepotProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.VehicleDepots
{
  public class VehicleDepotProto : VehicleDepotBaseProto
  {
    public override Type EntityType => typeof (VehicleDepot);

    public VehicleDepotProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity consumedPowerPerTick,
      Computing consumedComputingPerTick,
      Duration spawnInterval,
      Duration doorOpenCloseDuration,
      RelTile2f spawnTile,
      RelTile2f spawnDriveTile,
      RelTile2f despawnTile,
      RelTile2f despawnDriveTile,
      Option<VehicleDepotProto> nextTier,
      VehicleDepotBaseProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, consumedPowerPerTick, consumedComputingPerTick, spawnInterval, doorOpenCloseDuration, spawnTile, spawnDriveTile, despawnTile, despawnDriveTile, nextTier.As<VehicleDepotBaseProto>(), graphics);
    }
  }
}
