// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.DirectVehiclePathFinder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Vehicles;
using Mafi.PathFinding;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding
{
  /// <summary>
  /// A path finder that always returns a straight path to the first goal location. This is for testing and debugging only.
  /// </summary>
  [DependencyRegisteredManually("")]
  [GenerateSerializer(false, null, 0)]
  internal class DirectVehiclePathFinder : IVehiclePathFinder, IPathabilityProvider
  {
    private static readonly RelTile1i MAX_SEGMENT_LENGTH;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int CurrentPfId { get; private set; }

    public Tile2i StartCoord { get; private set; }

    public Tile2i SomeGoalCoord { get; private set; }

    public Option<VehiclePathFindingParams> PfParams { get; private set; }

    public Tile2i DistanceEstimationStartCoord => this.StartCoord;

    public Tile2i DistanceEstimationGoalCoord => this.SomeGoalCoord;

    public IPathabilityProvider PathabilityProvider => (IPathabilityProvider) this;

    public int TotalStepsCount => 0;

    public VehiclePathFinderInitResult InitVehiclePathFinding(
      IVehiclePathFindingTask task,
      ref int stepsLeft)
    {
      Assert.That<Option<VehiclePathFindingParams>>(this.PfParams).IsNone<VehiclePathFindingParams>();
      ++this.CurrentPfId;
      this.PfParams = (Option<VehiclePathFindingParams>) task.PathFindingParams;
      if (task.InitializeStartAndGoals(0))
      {
        Assert.That<IIndexable<Tile2i>>(task.GoalTiles).IsNotEmpty<Tile2i>();
        this.StartCoord = this.SomeGoalCoord = task.StartTiles.FirstOrDefault<Tile2i>();
        return VehiclePathFinderInitResult.GoalAlreadyReached;
      }
      if (task.StartTiles.IsEmpty<Tile2i>())
        return VehiclePathFinderInitResult.NoStarts;
      if (task.GoalTiles.IsEmpty<Tile2i>())
        return VehiclePathFinderInitResult.NoGoals;
      this.StartCoord = task.StartTiles.First<Tile2i>();
      this.SomeGoalCoord = task.GoalTiles.AsEnumerable().MinElement<Tile2i, long>((Func<Tile2i, long>) (x => x.DistanceSqrTo(this.StartCoord)));
      return VehiclePathFinderInitResult.ReadyForPf;
    }

    public PathFinderResult ContinueVehiclePathFinding(
      ref int stepsLeft,
      bool newTaskStartedThisSimStep,
      IVehicleSurfaceProvider surfaceProvider,
      bool isFinalAttempt)
    {
      Assert.That<Option<VehiclePathFindingParams>>(this.PfParams).HasValue<VehiclePathFindingParams>();
      return PathFinderResult.PathFound;
    }

    public bool? TryExtendGoals() => new bool?();

    public bool TryReconstructFoundPath(
      out IVehiclePathSegment firstPathSegment,
      out Tile2i goalTileRaw)
    {
      Assert.That<Option<VehiclePathFindingParams>>(this.PfParams).HasValue<VehiclePathFindingParams>();
      Tile2i cornerTileSpace = this.PfParams.Value.ConvertToCornerTileSpace(this.StartCoord);
      goalTileRaw = this.PfParams.Value.ConvertToCornerTileSpace(this.SomeGoalCoord);
      VehicleTerrainPathSegment terrainPathSegment = new VehicleTerrainPathSegment();
      firstPathSegment = (IVehiclePathSegment) terrainPathSegment;
      terrainPathSegment.PathRawReversed.Add(goalTileRaw);
      if (cornerTileSpace == goalTileRaw)
        return true;
      int intFloored = (this.StartCoord.DistanceTo(this.SomeGoalCoord) / DirectVehiclePathFinder.MAX_SEGMENT_LENGTH.Value).ToIntFloored();
      for (int index = intFloored - 1; index >= 0; --index)
      {
        Percent t = Percent.FromRatio(index + 1, intFloored + 1);
        terrainPathSegment.PathRawReversed.Add(cornerTileSpace.Lerp(goalTileRaw, t));
      }
      return true;
    }

    public void GetExploredTiles(Lyst<ExploredPfNode> exploredTiles)
    {
    }

    public void ResetState() => this.PfParams = Option<VehiclePathFindingParams>.None;

    public Tile2i? FindClosestValidPosition(
      Tile2i coord,
      VehiclePathFindingParams pfParams,
      Predicate<PfNode> predicate = null)
    {
      return new Tile2i?();
    }

    public bool IsPathable(Tile2i centerTile, ulong pathabilityMask) => true;

    public bool IsPathableIgnoringTerrain(Tile2i centerTile, ulong pathabilityMask) => true;

    public bool IsPathableRaw(Tile2i rawTile, ulong pathabilityMask) => true;

    public bool IsTilePathable(Tile2i tileCoord, ulong pathabilityMask) => true;

    public bool ComputeIsPathableAtRaw(
      Tile2i rawTile,
      VehiclePathFindingParams pfParams,
      uint ignoredReportersMask)
    {
      return true;
    }

    public ulong GetPathabilityMask(VehiclePathFindingParams pfParams) => 0;

    public ulong GetPathabilityMaskSingleTile(VehiclePathFindingParams pfParams) => 0;

    public static void Serialize(DirectVehiclePathFinder value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DirectVehiclePathFinder>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DirectVehiclePathFinder.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.CurrentPfId);
      Option<VehiclePathFindingParams>.Serialize(this.PfParams, writer);
      Tile2i.Serialize(this.SomeGoalCoord, writer);
      Tile2i.Serialize(this.StartCoord, writer);
    }

    public static DirectVehiclePathFinder Deserialize(BlobReader reader)
    {
      DirectVehiclePathFinder vehiclePathFinder;
      if (reader.TryStartClassDeserialization<DirectVehiclePathFinder>(out vehiclePathFinder))
        reader.EnqueueDataDeserialization((object) vehiclePathFinder, DirectVehiclePathFinder.s_deserializeDataDelayedAction);
      return vehiclePathFinder;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CurrentPfId = reader.ReadInt();
      this.PfParams = Option<VehiclePathFindingParams>.Deserialize(reader);
      this.SomeGoalCoord = Tile2i.Deserialize(reader);
      this.StartCoord = Tile2i.Deserialize(reader);
    }

    public DirectVehiclePathFinder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static DirectVehiclePathFinder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DirectVehiclePathFinder.MAX_SEGMENT_LENGTH = new RelTile1i(5);
      DirectVehiclePathFinder.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((DirectVehiclePathFinder) obj).SerializeData(writer));
      DirectVehiclePathFinder.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((DirectVehiclePathFinder) obj).DeserializeData(reader));
    }
  }
}
