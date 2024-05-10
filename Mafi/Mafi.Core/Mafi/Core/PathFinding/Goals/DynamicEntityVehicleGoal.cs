// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.DynamicEntityVehicleGoal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  [GenerateSerializer(false, null, 0)]
  public sealed class DynamicEntityVehicleGoal : VehicleGoalBase
  {
    public static readonly RelTile1i GOAL_INVALID_RETRY_PF_RADIUS;
    [NewInSaveVersion(140, null, null, typeof (DynamicEntityVehicleGoal.Factory), null)]
    private readonly DynamicEntityVehicleGoal.Factory m_factory;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<Vehicle> GoalVehicle { get; private set; }

    public override LocStrFormatted GoalName
    {
      get
      {
        Vehicle valueOrNull = this.GoalVehicle.ValueOrNull;
        return valueOrNull == null ? LocStrFormatted.Empty : valueOrNull.DefaultTitle;
      }
    }

    public DynamicEntityVehicleGoal(DynamicEntityVehicleGoal.Factory factory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(factory.m_vehicleSurfaceProvider);
      this.m_factory = factory;
    }

    public void Initialize(Vehicle vehicle)
    {
      Assert.That<bool>(vehicle.IsDestroyed).IsFalse("Creating nav goal to a destroyed vehicle.");
      Assert.That<bool>(vehicle.IsSpawned).IsTrue("Creating nav goal to not spawned vehicle.");
      this.Initialize();
      this.GoalVehicle = (Option<Vehicle>) vehicle;
    }

    public override Tile3f GetGoalPosition()
    {
      return !this.GoalVehicle.HasValue ? new Tile3f() : this.GoalVehicle.Value.Position3f;
    }

    protected override bool ShouldCheckGoalHeights(
      int retryNumber,
      out HeightTilesF goalHeightLow,
      out HeightTilesF goalHeightHigh)
    {
      goalHeightLow = new HeightTilesF(this.GoalVehicle.Value.Position3f.Z - Fix32.One);
      goalHeightHigh = new HeightTilesF(this.GoalVehicle.Value.Position3f.Z + Fix32.One);
      return true;
    }

    protected override bool GetGoalTilesInternal(
      Tile2i startTile,
      VehiclePathFindingParams pfParams,
      Lyst<Tile2i> goalTiles,
      out Tile2i distanceEstimationGoalTile,
      int retryNumber,
      RelTile1i extraTolerancePerRetry)
    {
      Assert.That<Option<Vehicle>>(this.GoalVehicle).HasValue<Vehicle>();
      Assert.That<Lyst<Tile2i>>(goalTiles).IsEmpty<Tile2i>();
      DrivingEntity drivingEntity = (DrivingEntity) this.GoalVehicle.Value;
      if (drivingEntity.IsDestroyed)
      {
        distanceEstimationGoalTile = startTile;
        return false;
      }
      if (drivingEntity.IsMoving)
      {
        ref Tile2i local1 = ref distanceEstimationGoalTile;
        Tile2f? target = drivingEntity.Target;
        ref Tile2f? local2 = ref target;
        Tile2i tile2i = local2.HasValue ? local2.GetValueOrDefault().Tile2i : drivingEntity.GroundPositionTile2i;
        local1 = tile2i;
      }
      else
        distanceEstimationGoalTile = drivingEntity.GroundPositionTile2i;
      RelTile1i relTile1i = this.GoalVehicle.Value.Prototype.NavTolerance + extraTolerancePerRetry * retryNumber;
      if ((startTile - drivingEntity.GroundPositionTile2i).LengthSqr <= relTile1i.Squared)
      {
        goalTiles.Add(drivingEntity.GroundPositionTile2i);
        return true;
      }
      Tile2i goal = distanceEstimationGoalTile;
      MafiMath.IterateCirclePoints(relTile1i.Value, (Action<int, int>) ((dx, dy) => goalTiles.Add(goal + new RelTile2i(dx, dy))));
      return false;
    }

    public override bool IsGoalValid(PathFindingEntity vehicle, out bool retryPf)
    {
      retryPf = false;
      if (this.GoalVehicle.IsNone || this.GoalVehicle.Value.IsDestroyed || !this.GoalVehicle.Value.IsSpawned)
        return false;
      IPathFindingResult pathFindingResult = vehicle.PathFindingResult;
      if (pathFindingResult.ResultStatus != VehiclePfResultStatus.PathFound)
        return true;
      Tile2f centerTileSpace = pathFindingResult.Task.PathFindingParams.ConvertToCenterTileSpace(pathFindingResult.GoalRawTile);
      long squared = (vehicle.Prototype.NavTolerance + DynamicEntityVehicleGoal.GOAL_INVALID_RETRY_PF_RADIUS).Squared;
      Tile2f position2f;
      if (this.GoalVehicle.Value is IEntityWithMaxServiceRadius maxServiceRadius && vehicle.Position2f.DistanceSqrTo(centerTileSpace) < squared)
      {
        position2f = this.GoalVehicle.Value.Position2f;
        if (position2f.DistanceSqrTo(centerTileSpace) >= maxServiceRadius.MaxServiceRadius.Value.Squared())
        {
          retryPf = true;
          return false;
        }
      }
      position2f = this.GoalVehicle.Value.Position2f;
      if (position2f.DistanceSqrTo(centerTileSpace) < squared)
        return true;
      Tile2f? target = this.GoalVehicle.Value.Target;
      if (target.HasValue)
      {
        target = this.GoalVehicle.Value.Target;
        position2f = target.Value;
        if (position2f.DistanceSqrTo(centerTileSpace) < squared)
          return true;
      }
      position2f = vehicle.Position2f;
      Fix64 fix64_1 = position2f.DistanceSqrTo(centerTileSpace);
      if (fix64_1 > 4L * squared)
      {
        Fix64 fix64_2 = centerTileSpace.DistanceSqrTo(this.GoalVehicle.Value.Position2f);
        if (fix64_1 > 16 * fix64_2)
          return true;
      }
      retryPf = true;
      return false;
    }

    public override void OnNavigationResult(bool isSuccess, IPathFindingVehicle vehicle)
    {
      if (this.GoalVehicle.IsNone)
        return;
      if (isSuccess)
        this.m_factory.UnreachablesManager.MarkReachableFor((IEntity) this.GoalVehicle.Value, vehicle);
      else
        this.m_factory.UnreachablesManager.MarkUnreachableFor((IEntity) this.GoalVehicle.Value, vehicle);
    }

    public override string ToString()
    {
      return string.Format("{0} +- {1}", (object) this.GoalVehicle.Value, (object) DynamicEntityVehicleGoal.GOAL_INVALID_RETRY_PF_RADIUS);
    }

    public static void Serialize(DynamicEntityVehicleGoal value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DynamicEntityVehicleGoal>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DynamicEntityVehicleGoal.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<Vehicle>.Serialize(this.GoalVehicle, writer);
      DynamicEntityVehicleGoal.Factory.Serialize(this.m_factory, writer);
    }

    public static DynamicEntityVehicleGoal Deserialize(BlobReader reader)
    {
      DynamicEntityVehicleGoal entityVehicleGoal;
      if (reader.TryStartClassDeserialization<DynamicEntityVehicleGoal>(out entityVehicleGoal))
        reader.EnqueueDataDeserialization((object) entityVehicleGoal, DynamicEntityVehicleGoal.s_deserializeDataDelayedAction);
      return entityVehicleGoal;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.GoalVehicle = Option<Vehicle>.Deserialize(reader);
      reader.SetField<DynamicEntityVehicleGoal>(this, "m_factory", reader.LoadedSaveVersion >= 140 ? (object) DynamicEntityVehicleGoal.Factory.Deserialize(reader) : (object) (DynamicEntityVehicleGoal.Factory) null);
      if (reader.LoadedSaveVersion >= 140)
        return;
      reader.RegisterResolvedMember<DynamicEntityVehicleGoal>(this, "m_factory", typeof (DynamicEntityVehicleGoal.Factory), true);
    }

    static DynamicEntityVehicleGoal()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DynamicEntityVehicleGoal.GOAL_INVALID_RETRY_PF_RADIUS = new RelTile1i(2);
      DynamicEntityVehicleGoal.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleGoalBase) obj).SerializeData(writer));
      DynamicEntityVehicleGoal.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleGoalBase) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [OnlyForSaveCompatibility("This does not need a serializer!")]
    public sealed class Factory
    {
      public readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
      [NewInSaveVersion(140, null, null, typeof (UnreachableTerrainDesignationsManager), null)]
      internal readonly UnreachableTerrainDesignationsManager UnreachablesManager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Factory(
        IVehicleSurfaceProvider vehicleSurfaceProvider,
        UnreachableTerrainDesignationsManager unreachablesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
        this.UnreachablesManager = unreachablesManager;
      }

      public DynamicEntityVehicleGoal Create(Vehicle vehicle)
      {
        DynamicEntityVehicleGoal entityVehicleGoal = new DynamicEntityVehicleGoal(this);
        entityVehicleGoal.Initialize(vehicle);
        return entityVehicleGoal;
      }

      public static void Serialize(DynamicEntityVehicleGoal.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<DynamicEntityVehicleGoal.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, DynamicEntityVehicleGoal.Factory.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        writer.WriteGeneric<IVehicleSurfaceProvider>(this.m_vehicleSurfaceProvider);
        UnreachableTerrainDesignationsManager.Serialize(this.UnreachablesManager, writer);
      }

      public static DynamicEntityVehicleGoal.Factory Deserialize(BlobReader reader)
      {
        DynamicEntityVehicleGoal.Factory factory;
        if (reader.TryStartClassDeserialization<DynamicEntityVehicleGoal.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, DynamicEntityVehicleGoal.Factory.s_deserializeDataDelayedAction);
        return factory;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<DynamicEntityVehicleGoal.Factory>(this, "m_vehicleSurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
        reader.SetField<DynamicEntityVehicleGoal.Factory>(this, "UnreachablesManager", reader.LoadedSaveVersion >= 140 ? (object) UnreachableTerrainDesignationsManager.Deserialize(reader) : (object) (UnreachableTerrainDesignationsManager) null);
        if (reader.LoadedSaveVersion >= 140)
          return;
        reader.RegisterResolvedMember<DynamicEntityVehicleGoal.Factory>(this, "UnreachablesManager", typeof (UnreachableTerrainDesignationsManager), true);
      }

      static Factory()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        DynamicEntityVehicleGoal.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((DynamicEntityVehicleGoal.Factory) obj).SerializeData(writer));
        DynamicEntityVehicleGoal.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((DynamicEntityVehicleGoal.Factory) obj).DeserializeData(reader));
      }
    }
  }
}
