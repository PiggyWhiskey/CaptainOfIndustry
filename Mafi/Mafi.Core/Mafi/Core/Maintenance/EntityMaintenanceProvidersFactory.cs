// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.EntityMaintenanceProvidersFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Notifications;

#nullable disable
namespace Mafi.Core.Maintenance
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class EntityMaintenanceProvidersFactory : IEntityMaintenanceProvidersFactory
  {
    private readonly MaintenanceManager m_maintenanceManager;
    private readonly INotificationsManager m_notificationsManager;

    public EntityMaintenanceProvidersFactory(
      MaintenanceManager maintenanceManager,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_maintenanceManager = maintenanceManager;
      this.m_notificationsManager = notificationsManager;
    }

    public IEntityMaintenanceProvider CreateFor(IMaintainedEntity entity)
    {
      return (IEntityMaintenanceProvider) new EntityMaintenanceProvider(entity, this.m_maintenanceManager, this.m_notificationsManager);
    }
  }
}
