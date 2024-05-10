// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.VehicleQueueJobFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class VehicleQueueJobFactory
  {
    private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
    public readonly IVehiclePathFindingManager PathFindingManager;
    public readonly StaticEntityVehicleGoal.Factory StaticEntityGoalFactory;
    public readonly DynamicEntityVehicleGoal.Factory DynamicEntityGoalFactory;

    public VehicleQueueJobFactory(
      VehicleJobId.Factory vehicleJobIdFactory,
      IVehiclePathFindingManager pathFindingManager,
      StaticEntityVehicleGoal.Factory staticEntityGoalFactory,
      DynamicEntityVehicleGoal.Factory dynamicEntityGoalFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_vehicleJobIdFactory = vehicleJobIdFactory;
      this.PathFindingManager = pathFindingManager;
      this.StaticEntityGoalFactory = staticEntityGoalFactory;
      this.DynamicEntityGoalFactory = dynamicEntityGoalFactory;
    }

    public VehicleQueueJob<TVehicle> CreateJobForStaticOwnedQueue<TOwner, TVehicle>(
      TVehicle vehicle,
      VehicleQueue<TVehicle, TOwner> queue,
      bool isTrueJob = true,
      bool navigateClosebyIsSufficient = false,
      bool useCustomTarget = false,
      bool doNotCancelAllJobsOnNavFailure = false,
      Option<QueueJobResultRef> resultRef = default (Option<QueueJobResultRef>))
      where TOwner : class, IStaticEntity
      where TVehicle : Vehicle
    {
      VehicleQueueJob<TVehicle> staticOwnedQueue = new VehicleQueueJob<TVehicle>(this.m_vehicleJobIdFactory.GetNextId(), this);
      staticOwnedQueue.InitializeWithStaticOwner<TOwner>(vehicle, queue, isTrueJob, navigateClosebyIsSufficient, useCustomTarget, doNotCancelAllJobsOnNavFailure, resultRef);
      return staticOwnedQueue;
    }

    public VehicleQueueJob<TVehicle> CreateJobForVehicleOwnedQueue<TOwner, TVehicle>(
      TVehicle vehicle,
      VehicleQueue<TVehicle, TOwner> queue,
      bool isTrueJob = true,
      bool navigateClosebyIsSufficient = false)
      where TOwner : Vehicle
      where TVehicle : Vehicle
    {
      VehicleQueueJob<TVehicle> vehicleOwnedQueue = new VehicleQueueJob<TVehicle>(this.m_vehicleJobIdFactory.GetNextId(), this);
      vehicleOwnedQueue.InitializeWithVehicleOwner<TOwner>(vehicle, queue, isTrueJob, navigateClosebyIsSufficient);
      return vehicleOwnedQueue;
    }
  }
}
