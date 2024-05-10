// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Priorities.EntityWithGeneralPriorityExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Factory.ElectricPower;

#nullable disable
namespace Mafi.Core.Entities.Priorities
{
  public static class EntityWithGeneralPriorityExtensions
  {
    public static void SetGeneralPriority(this IEntityWithGeneralPriority entity, int priority)
    {
      if (priority == entity.GeneralPriority || !GeneralPriorities.AssertAssignableRange(priority))
        return;
      if (!(entity is IEntityWithGeneralPriorityFriend generalPriorityFriend))
        Log.Error("Failed to set priority for " + entity.GetTitle());
      else
        generalPriorityFriend.SetGeneralPriorityInternal(priority);
    }

    public static bool IsPriorityVisibleByDefault(this IEntityWithGeneralPriority entity)
    {
      if (entity.Prototype.Costs.Workers > 0 || entity.Prototype.Costs.Maintenance.MaintenancePerMonth.IsPositive)
        return true;
      return entity is IElectricityConsumingEntity electricityConsumingEntity && electricityConsumingEntity.PowerRequired.IsPositive;
    }
  }
}
