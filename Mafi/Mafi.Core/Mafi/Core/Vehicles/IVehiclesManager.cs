// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.IVehiclesManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Vehicles
{
  public interface IVehiclesManager
  {
    int VehiclesLimitLeft { get; }

    int MaxVehiclesLimit { get; }

    void IncreaseVehicleLimit(int diff);

    IEvent<Vehicle> OnVehicleDespawned { get; }

    IReadOnlySet<Vehicle> AllVehicles { get; }

    IReadOnlySet<Truck> Trucks { get; }

    IReadOnlySet<Excavator> Excavators { get; }

    IReadOnlySet<TreeHarvester> TreeHarvesters { get; }

    IReadOnlySet<TreePlanter> TreePlanters { get; }

    Option<T> GetFreeVehicle<T>(DynamicEntityProto proto, Tile2f entityPosition) where T : Vehicle;

    VehicleStats GetStats(DynamicEntityProto proto);

    void VehicleSpawned(Vehicle vehicle);

    bool CanScrapVehicle(Vehicle vehicle);

    bool CanUpgradeVehicle(DrivingEntityProto currentProto, DrivingEntityProto newProto);

    bool CanRecoverVehicle(Vehicle vehicle, out LocStrFormatted errorReason);

    /// <summary>
    /// Sends given vehicle to the closest depot and despawns it there.
    /// </summary>
    bool TryEnqueueScrapJob(Vehicle vehicle);

    /// <summary>
    /// Sends given vehicle to the closest depot and replaces it there.
    /// </summary>
    bool TryEnqueueReplaceJob(Vehicle vehicle, DrivingEntityProto newProto);

    bool TryEnqueueRecoveryJob(Vehicle vehicle);

    void TeleportVehicleToAnyValidPosition(Vehicle vehicle);

    bool TryGetClosestDepotForReplacement(
      Vehicle vehicle,
      DrivingEntityProto newProto,
      out VehicleDepotBase closestDepot);
  }
}
