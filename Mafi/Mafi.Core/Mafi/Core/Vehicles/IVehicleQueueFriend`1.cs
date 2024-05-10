// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.IVehicleQueueFriend`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles.Jobs;

#nullable disable
namespace Mafi.Core.Vehicles
{
  public interface IVehicleQueueFriend<TVehicle> where TVehicle : Vehicle
  {
    IEntity Owner { get; }

    bool IsEnabled { get; }

    bool IsFirstVehicle(TVehicle vehicle);

    void VehicleArrivedAndWaiting(VehicleQueueJob<TVehicle> job);

    void RemoveWaitingJob(VehicleQueueJob<TVehicle> job);

    void VehicleArriving(VehicleQueueJob<TVehicle> job);

    void RemoveArrivingVehicle(VehicleQueueJob<TVehicle> job);

    Option<Vehicle> GetWaitTargetFor(VehicleQueueJob<TVehicle> job);

    void ReplaceFirstJobWith(IQueueTipJob queueTipJob);

    bool Contains(VehicleQueueJob<TVehicle> job);
  }
}
