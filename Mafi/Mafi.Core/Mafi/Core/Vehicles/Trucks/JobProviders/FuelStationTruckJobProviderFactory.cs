// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.JobProviders.FuelStationTruckJobProviderFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.FuelStations;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks.JobProviders
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class FuelStationTruckJobProviderFactory : IFuelStationTruckJobProviderFactory
  {
    private readonly FuelStationsManager m_fuelStationsManager;
    private readonly TruckJobProviderContext m_context;

    public FuelStationTruckJobProviderFactory(
      FuelStationsManager fuelStationsManager,
      TruckJobProviderContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_fuelStationsManager = fuelStationsManager;
      this.m_context = context;
    }

    public FuelStationTruckJobProvider CreateJobProvider(FuelStation assignee)
    {
      return new FuelStationTruckJobProvider(assignee, this.m_context, this.m_fuelStationsManager);
    }
  }
}
