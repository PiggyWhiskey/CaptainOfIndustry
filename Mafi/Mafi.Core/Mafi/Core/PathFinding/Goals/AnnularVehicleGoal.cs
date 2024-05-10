// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.AnnularVehicleGoal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  /// <summary>
  /// A goal for tiles a set distance from the target, neither further nor closer.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class AnnularVehicleGoal : VehicleGoalBase
  {
    protected readonly TerrainManager m_terrainManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Tile2i? GoalPosition { get; private set; }

    public RelTile1i Distance { get; private set; }

    public override LocStrFormatted GoalName => "Terrain position".ToDoTranslate();

    internal AnnularVehicleGoal(
      IVehicleSurfaceProvider vehicleSurfaceProvider,
      TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(vehicleSurfaceProvider);
      this.m_terrainManager = terrainManager;
    }

    public void Initialize(Tile2i position, RelTile1i distance)
    {
      this.Initialize();
      this.GoalPosition = new Tile2i?(position);
      this.Distance = distance.CheckPositive();
    }

    public override Tile3f GetGoalPosition()
    {
      return !this.GoalPosition.HasValue ? new Tile3f() : this.GoalPosition.Value.CenterTile2f.ExtendHeight(this.m_terrainManager.GetHeight(this.GoalPosition.Value));
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
      distanceEstimationGoalTile = this.GoalPosition.Value;
      RelTile1i relTile1i = this.Distance + extraTolerancePerRetry * retryNumber;
      Tile2i goal = distanceEstimationGoalTile;
      MafiMath.IterateCirclePoints(relTile1i.Value, (Action<int, int>) ((dx, dy) => goalTiles.Add(goal + new RelTile2i(dx, dy))));
      if (!goalTiles.Contains(startTile))
        return false;
      goalTiles.Clear();
      goalTiles.Add(startTile);
      return true;
    }

    public override bool IsGoalValid(PathFindingEntity vehicle, out bool retryPf)
    {
      retryPf = false;
      return this.GoalPosition.HasValue;
    }

    public override void OnNavigationResult(bool isSuccess, IPathFindingVehicle vehicle)
    {
    }

    public override string ToString()
    {
      return string.Format("Annulus at {0} +- {1}", (object) this.GoalPosition, (object) this.Distance);
    }

    public static void Serialize(AnnularVehicleGoal value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AnnularVehicleGoal>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AnnularVehicleGoal.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RelTile1i.Serialize(this.Distance, writer);
      writer.WriteNullableStruct<Tile2i>(this.GoalPosition);
      TerrainManager.Serialize(this.m_terrainManager, writer);
    }

    public static AnnularVehicleGoal Deserialize(BlobReader reader)
    {
      AnnularVehicleGoal annularVehicleGoal;
      if (reader.TryStartClassDeserialization<AnnularVehicleGoal>(out annularVehicleGoal))
        reader.EnqueueDataDeserialization((object) annularVehicleGoal, AnnularVehicleGoal.s_deserializeDataDelayedAction);
      return annularVehicleGoal;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Distance = RelTile1i.Deserialize(reader);
      this.GoalPosition = reader.ReadNullableStruct<Tile2i>();
      reader.SetField<AnnularVehicleGoal>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
    }

    static AnnularVehicleGoal()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AnnularVehicleGoal.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleGoalBase) obj).SerializeData(writer));
      AnnularVehicleGoal.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleGoalBase) obj).DeserializeData(reader));
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [GenerateSerializer(false, null, 0)]
    public sealed class Factory
    {
      private readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
      private readonly TerrainManager m_terrainManager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Factory(IVehicleSurfaceProvider vehicleSurfaceProvider, TerrainManager terrainManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
        this.m_terrainManager = terrainManager;
      }

      public AnnularVehicleGoal Create(Tile2i position, RelTile1i tolerance)
      {
        AnnularVehicleGoal annularVehicleGoal = new AnnularVehicleGoal(this.m_vehicleSurfaceProvider, this.m_terrainManager);
        annularVehicleGoal.Initialize(position, tolerance);
        return annularVehicleGoal;
      }

      public static void Serialize(AnnularVehicleGoal.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<AnnularVehicleGoal.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, AnnularVehicleGoal.Factory.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        TerrainManager.Serialize(this.m_terrainManager, writer);
        writer.WriteGeneric<IVehicleSurfaceProvider>(this.m_vehicleSurfaceProvider);
      }

      public static AnnularVehicleGoal.Factory Deserialize(BlobReader reader)
      {
        AnnularVehicleGoal.Factory factory;
        if (reader.TryStartClassDeserialization<AnnularVehicleGoal.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, AnnularVehicleGoal.Factory.s_deserializeDataDelayedAction);
        return factory;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<AnnularVehicleGoal.Factory>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
        reader.SetField<AnnularVehicleGoal.Factory>(this, "m_vehicleSurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
      }

      static Factory()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        AnnularVehicleGoal.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AnnularVehicleGoal.Factory) obj).SerializeData(writer));
        AnnularVehicleGoal.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AnnularVehicleGoal.Factory) obj).DeserializeData(reader));
      }
    }
  }
}
