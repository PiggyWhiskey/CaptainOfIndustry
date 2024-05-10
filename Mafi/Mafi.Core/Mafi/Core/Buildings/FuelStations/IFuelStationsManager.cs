// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.FuelStations.IFuelStationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles.Trucks;

#nullable disable
namespace Mafi.Core.Buildings.FuelStations
{
  public interface IFuelStationsManager
  {
    bool TryRefuelSelf(Vehicle vehicle);

    bool TryGetRefuelOtherVehicleJob(FuelStation fuelStation, Truck refuelingTruck);

    bool TryRequestTruckForRefueling(Vehicle vehicle);

    void VehicleRefueledByTruck(Vehicle vehicle);

    void OnRecoveryPerformed(Vehicle vehicle);
  }
}
