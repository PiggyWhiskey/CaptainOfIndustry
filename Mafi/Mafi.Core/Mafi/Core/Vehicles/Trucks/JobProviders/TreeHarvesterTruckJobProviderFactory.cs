// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.JobProviders.TreeHarvesterTruckJobProviderFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.TreeHarvesters;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks.JobProviders
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TreeHarvesterTruckJobProviderFactory
  {
    private readonly TruckJobProviderContext m_context;
    private readonly VehicleQueueJobFactory m_queueJobFactory;

    public TreeHarvesterTruckJobProviderFactory(
      TruckJobProviderContext context,
      VehicleQueueJobFactory queueJobFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_context = context;
      this.m_queueJobFactory = queueJobFactory;
    }

    public TreeHarvesterTruckJobProvider CreateJobProvider(TreeHarvester assignee)
    {
      return new TreeHarvesterTruckJobProvider(assignee, this.m_context, this.m_queueJobFactory);
    }
  }
}
