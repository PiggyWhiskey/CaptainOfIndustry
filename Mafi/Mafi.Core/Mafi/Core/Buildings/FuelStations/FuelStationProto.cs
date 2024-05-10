// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.FuelStations.FuelStationProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.FuelStations
{
  public class FuelStationProto : 
    StorageBaseProto,
    IProtoWithUpgrade<FuelStationProto>,
    IProtoWithUpgrade,
    IProto
  {
    /// <summary>Type of the fuel.</summary>
    public readonly ProductProto FuelProto;
    /// <summary>
    /// Number of queues that vehicles wait in before getting fuel. Also means number of vehicles that can be service
    /// simultaneously. Only vehicles that are coming for to refuel their tanks wait in queues, vehicles bringing
    /// fuel to put it into the station or getting fuel to refuel other vehicles do not wait in queues.
    /// </summary>
    public readonly int VehicleQueuesCount;
    public readonly Quantity MaxTransferQuantityPerVehicle;

    public override Type EntityType => typeof (FuelStation);

    public UpgradeData<FuelStationProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public FuelStationProto(
      StaticEntityProto.ID id,
      ProductProto fuelProto,
      Quantity capacity,
      Quantity maxTransferQuantityPerVehicle,
      int vehicleQueuesCount,
      Proto.Str strings,
      EntityLayout layout,
      Option<FuelStationProto> nextTier,
      EntityCosts costs,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, capacity, costs, graphics);
      Assert.That<ImmutableArray<IoPortTemplate>>(this.OutputPorts).IsEmpty<IoPortTemplate>();
      this.FuelProto = fuelProto.CheckNotNull<ProductProto>();
      this.MaxTransferQuantityPerVehicle = maxTransferQuantityPerVehicle.CheckPositive();
      this.VehicleQueuesCount = vehicleQueuesCount.CheckPositive();
      this.Upgrade = new UpgradeData<FuelStationProto>(this, nextTier);
    }
  }
}
