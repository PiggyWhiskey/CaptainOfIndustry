// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.JobProviders.MineTowerTruckJobProviderFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Jobs;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks.JobProviders
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class MineTowerTruckJobProviderFactory : IMineTowerTruckJobProviderFactory
  {
    private readonly TruckJobProviderContext m_context;
    private readonly VehicleQueueJobFactory m_queueJobFactory;

    public MineTowerTruckJobProviderFactory(
      TruckJobProviderContext context,
      VehicleQueueJobFactory queueJobFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_context = context;
      this.m_queueJobFactory = queueJobFactory;
    }

    public MineTowerTruckJobProvider CreateJobProvider(
      MineTower entity,
      AssignedVehicles<Excavator, ExcavatorProto> excavators)
    {
      return new MineTowerTruckJobProvider(entity, excavators, this.m_context, this.m_queueJobFactory);
    }
  }
}
