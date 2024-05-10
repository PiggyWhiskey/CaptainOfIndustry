// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.ParkAndWaitJobFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Terrain.Designation;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Factory for a job(s) that navigate a vehicle to given static entity and wait there. Doesn't represent a factory
  /// for a new specific job as other factories in the same namespace usually do.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ParkAndWaitJobFactory
  {
    private static readonly RelTile1f DEFAULT_PARKED_VEHICLE_DISTANCE;
    private readonly UnreachableTerrainDesignationsManager m_unreachablesManager;
    private readonly NavigateToJob.Factory m_navigateToJobFactory;
    private readonly StaticEntityVehicleGoal.Factory m_staticEntityGoalFactory;

    public ParkAndWaitJobFactory(
      UnreachableTerrainDesignationsManager unreachablesManager,
      NavigateToJob.Factory navigateToJobFactory,
      StaticEntityVehicleGoal.Factory staticEntityGoalFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unreachablesManager = unreachablesManager;
      this.m_navigateToJobFactory = navigateToJobFactory;
      this.m_staticEntityGoalFactory = staticEntityGoalFactory;
    }

    /// <summary>
    /// Use to park vehicle at given static entity. Either enqueues navigation to the entity or waiting at the
    /// entity.
    /// </summary>
    public bool TryEnqueueParkingJobIfNeeded(
      Vehicle vehicle,
      ILayoutEntity staticEntity,
      RelTile1f? parkingDistance = null)
    {
      if (this.IsConsideredParkedAt(vehicle, staticEntity, parkingDistance) || this.m_unreachablesManager.HasUnreachableEntity((IPathFindingVehicle) vehicle, (IEntity) staticEntity))
        return false;
      this.m_navigateToJobFactory.EnqueueJob(vehicle, (IVehicleGoalFull) this.m_staticEntityGoalFactory.Create((IStaticEntity) staticEntity), asTrueJob: false);
      return true;
    }

    internal bool IsConsideredParkedAt(
      Vehicle vehicle,
      ILayoutEntity staticEntity,
      RelTile1f? parkingDistance = null)
    {
      RelTile1f relTile1f = (new RelTile1f(staticEntity.Prototype.Layout.LayoutSize.Xy.MaxComponent()) + new RelTile1f(vehicle.Prototype.EntitySize.Xy.MaxComponent())) / 2 + (parkingDistance ?? ParkAndWaitJobFactory.DEFAULT_PARKED_VEHICLE_DISTANCE);
      return vehicle.Position2f.DistanceSqrTo(staticEntity.Position2f) <= relTile1f.Squared;
    }

    static ParkAndWaitJobFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ParkAndWaitJobFactory.DEFAULT_PARKED_VEHICLE_DISTANCE = 5.0.Tiles();
    }
  }
}
