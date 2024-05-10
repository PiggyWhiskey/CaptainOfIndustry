// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.TilePositionVehicleGoal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  [GenerateSerializer(false, null, 0)]
  public sealed class TilePositionVehicleGoal : VehicleGoalBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Tile2i? GoalTile { get; private set; }

    public RelTile1i ToleranceRadius { get; private set; }

    public override LocStrFormatted GoalName => "Terrain position".ToDoTranslate();

    public TilePositionVehicleGoal(IVehicleSurfaceProvider vehicleSurfaceProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(vehicleSurfaceProvider);
    }

    public void Initialize(Tile2i goal, RelTile1i tolerance)
    {
      this.Initialize();
      this.GoalTile = new Tile2i?(goal);
      this.ToleranceRadius = tolerance.CheckPositive();
    }

    public override Tile3f GetGoalPosition()
    {
      return !this.GoalTile.HasValue ? new Tile3f() : this.GoalTile.Value.CenterTile2f.ExtendHeight(this.VehicleSurfaceProvider.GetVehicleSurfaceAt(this.GoalTile.Value, out bool _));
    }

    protected override bool ShouldCheckGoalHeights(
      int retryNumber,
      out HeightTilesF goalHeightLow,
      out HeightTilesF goalHeightHigh)
    {
      goalHeightLow = goalHeightHigh = HeightTilesF.Zero;
      return false;
    }

    protected override bool GetGoalTilesInternal(
      Tile2i startTile,
      VehiclePathFindingParams pfParams,
      Lyst<Tile2i> goalTiles,
      out Tile2i distanceEstimationGoalTile,
      int retryNumber,
      RelTile1i extraTolerancePerRetry)
    {
      Tile2i goal = this.GoalTile.Value;
      distanceEstimationGoalTile = goal;
      RelTile1i relTile1i = this.ToleranceRadius + extraTolerancePerRetry * retryNumber;
      if (relTile1i <= RelTile1i.Zero)
      {
        if (startTile == goal)
        {
          goalTiles.Clear();
          goalTiles.Add(goal);
          return true;
        }
        goalTiles.Add(goal);
        return false;
      }
      if ((startTile - goal).LengthSqr <= relTile1i.Squared)
      {
        goalTiles.Clear();
        goalTiles.Add(goal);
        return true;
      }
      MafiMath.IterateCirclePoints(relTile1i.Value, (Action<int, int>) ((dx, dy) => goalTiles.Add(goal + new RelTile2i(dx, dy))));
      return false;
    }

    public override bool IsGoalValid(PathFindingEntity vehicle, out bool retryPf)
    {
      retryPf = false;
      return true;
    }

    public override void OnNavigationResult(bool isSuccess, IPathFindingVehicle vehicle)
    {
    }

    public override string ToString()
    {
      return string.Format("Tile {0} +- {1}", (object) this.GoalTile, (object) this.ToleranceRadius);
    }

    public static void Serialize(TilePositionVehicleGoal value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TilePositionVehicleGoal>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TilePositionVehicleGoal.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteNullableStruct<Tile2i>(this.GoalTile);
      RelTile1i.Serialize(this.ToleranceRadius, writer);
    }

    public static TilePositionVehicleGoal Deserialize(BlobReader reader)
    {
      TilePositionVehicleGoal positionVehicleGoal;
      if (reader.TryStartClassDeserialization<TilePositionVehicleGoal>(out positionVehicleGoal))
        reader.EnqueueDataDeserialization((object) positionVehicleGoal, TilePositionVehicleGoal.s_deserializeDataDelayedAction);
      return positionVehicleGoal;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.GoalTile = reader.ReadNullableStruct<Tile2i>();
      this.ToleranceRadius = RelTile1i.Deserialize(reader);
    }

    static TilePositionVehicleGoal()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TilePositionVehicleGoal.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleGoalBase) obj).SerializeData(writer));
      TilePositionVehicleGoal.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleGoalBase) obj).DeserializeData(reader));
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [GenerateSerializer(false, null, 0)]
    public sealed class Factory
    {
      private readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Factory(IVehicleSurfaceProvider vehicleSurfaceProvider)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
      }

      public TilePositionVehicleGoal Create(Tile2i goalTile, RelTile1i? tolerance = null)
      {
        TilePositionVehicleGoal positionVehicleGoal = new TilePositionVehicleGoal(this.m_vehicleSurfaceProvider);
        positionVehicleGoal.Initialize(goalTile, tolerance ?? RobustNavHelper.DEFAULT_EXTRA_TOLERANCE_PER_RETRY);
        return positionVehicleGoal;
      }

      public static void Serialize(TilePositionVehicleGoal.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<TilePositionVehicleGoal.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, TilePositionVehicleGoal.Factory.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        writer.WriteGeneric<IVehicleSurfaceProvider>(this.m_vehicleSurfaceProvider);
      }

      public static TilePositionVehicleGoal.Factory Deserialize(BlobReader reader)
      {
        TilePositionVehicleGoal.Factory factory;
        if (reader.TryStartClassDeserialization<TilePositionVehicleGoal.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, TilePositionVehicleGoal.Factory.s_deserializeDataDelayedAction);
        return factory;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<TilePositionVehicleGoal.Factory>(this, "m_vehicleSurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
      }

      static Factory()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TilePositionVehicleGoal.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TilePositionVehicleGoal.Factory) obj).SerializeData(writer));
        TilePositionVehicleGoal.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TilePositionVehicleGoal.Factory) obj).DeserializeData(reader));
      }
    }
  }
}
