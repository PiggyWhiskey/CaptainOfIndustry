// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.IMaintainedEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Maintenance
{
  /// <summary>
  /// Makes derived entity need maintenance to function.
  /// 
  /// To correctly implement this interface:
  ///  1) Use <see cref="T:Mafi.Core.Maintenance.IEntityMaintenanceProvidersFactory" /> to get a <see cref="T:Mafi.Core.Maintenance.IEntityMaintenanceProvider" />
  /// 	and assign it to <see cref="P:Mafi.Core.Maintenance.IMaintainedEntity.Maintenance" /> property.
  ///  2) Use <see cref="P:Mafi.Core.Maintenance.IEntityMaintenanceProvider.Status" /> and <see cref="F:Mafi.Core.Maintenance.MaintenanceStatus.IsBroken" />
  ///     as part of entities' <see cref="!:IEntityGeneral.IsEnabled" /> state.
  /// </summary>
  public interface IMaintainedEntity : IEntityWithGeneralPriority, IEntity, IIsSafeAsHashKey
  {
    MaintenanceCosts MaintenanceCosts { get; }

    IEntityMaintenanceProvider Maintenance { get; }

    bool IsIdleForMaintenance { get; }
  }
}
