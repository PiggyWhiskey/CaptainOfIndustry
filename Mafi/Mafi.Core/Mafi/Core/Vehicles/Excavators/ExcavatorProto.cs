// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Excavators.ExcavatorProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles.Trucks;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Excavators
{
  public class ExcavatorProto : DrivingEntityProto
  {
    /// <summary>Capacity of excavator's shovel.</summary>
    public readonly Quantity Capacity;
    /// <summary>Tolerance while navigating towards a designation.</summary>
    public readonly RelTile1i MinMiningDistance;
    /// <summary>
    /// Maximal distance to target that is considered as small enough to start mine/deposit. This represents reach of
    /// the excavator shovel.
    /// </summary>
    /// <remarks>
    /// Note that this distance is from origin of the entity (center) so the value should be greater then length of
    /// the vehicle.
    /// </remarks>
    public readonly RelTile1i MaxMiningDistance;
    /// <summary>
    /// Specifies how much of material can be mined at particular radius per shovel. Length of this array naturally
    /// specifies mining radius.
    /// </summary>
    /// <remarks>
    /// Excavator can over-mine by maximum of thickness specified in this array. The shovel capacity should be
    /// generally larger too minimize impact of this imprecision.
    /// </remarks>
    public readonly ImmutableArray<ThicknessTilesF> MinedThicknessByDistance;
    /// <summary>Timings for excavator animations.</summary>
    /// <remarks>
    /// The timings cannot be just UI detail since the actual duration is crucial for correct simulation of the
    /// delays.
    /// </remarks>
    public readonly ExcavatorProto.Timings MineTimings;
    /// <summary>Parameters of the cabin driver.</summary>
    public readonly RotatingCabinDriverProto RotatingCabinDriverProto;
    /// <summary>
    /// Graphics-only properties that does not affect game simulation and are not needed or accessed by the game
    /// simulation.
    /// </summary>
    public readonly ExcavatorProto.Gfx Graphics;
    private readonly Func<TruckProto, bool> m_isTruckSupportedFunc;

    public override Type EntityType => typeof (Excavator);

    public ExcavatorProto(
      DynamicEntityProto.ID id,
      Proto.Str strings,
      RelTile3f entitySize,
      EntityCosts costs,
      int vehicleQuotaCost,
      DrivingData drivingData,
      RotatingCabinDriverProto rotatingCabinDriverProto,
      Quantity capacity,
      RelTile1i minMiningDistance,
      RelTile1i maxMiningDistance,
      ImmutableArray<ThicknessTilesF> minedThicknessByDistance,
      ExcavatorProto.Timings mineTimings,
      Option<Mafi.Core.Entities.Dynamic.FuelTankProto> fuelTankProto,
      VehiclePathFindingParams pathFindingParams,
      ImmutableArray<ThicknessTilesF> disruptionByDistance,
      Duration durationToBuild,
      Func<TruckProto, bool> isTruckSupportedFunc,
      Option<DrivingEntityProto> nextTier,
      ExcavatorProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, entitySize, costs, vehicleQuotaCost, drivingData, fuelTankProto, pathFindingParams, disruptionByDistance, durationToBuild, nextTier, (DynamicGroundEntityProto.Gfx) graphics);
      if (entitySize.LengthTiles1f / 2 > maxMiningDistance.RelTile1f)
        throw new ProtoBuilderException(string.Format("Mining distance {0} of excavator '{1}' is too small", (object) maxMiningDistance, (object) id) + string.Format(" (it should not be lower then half-length of the excavator {0})!", (object) (entitySize.X / 2)));
      if ((pathFindingParams.RequiredClearance / 2).Value >= maxMiningDistance.Value)
        throw new ProtoBuilderException(string.Format("Mining distance {0} of excavator '{1}' is too small", (object) maxMiningDistance, (object) id) + string.Format(" (it should not be lower then half clearance of excavator {0})!", (object) (pathFindingParams.RequiredClearance / 2)));
      this.RotatingCabinDriverProto = rotatingCabinDriverProto;
      this.Capacity = capacity.CheckPositive();
      this.MinMiningDistance = minMiningDistance.CheckPositive();
      this.MaxMiningDistance = maxMiningDistance.CheckPositive();
      this.MinedThicknessByDistance = minedThicknessByDistance.CheckNotEmpty<ThicknessTilesF>();
      this.MineTimings = mineTimings.CheckNotDefaultStruct<ExcavatorProto.Timings>();
      this.m_isTruckSupportedFunc = isTruckSupportedFunc;
      this.Graphics = graphics.CheckNotNull<ExcavatorProto.Gfx>();
    }

    public bool IsTruckSupported(TruckProto truck) => this.m_isTruckSupportedFunc(truck);

    public struct Timings
    {
      public readonly Duration PrepareToMineDuration;
      public readonly Duration MineDuration;
      public readonly Duration PrepareToDumpDuration;
      public readonly Duration DumpDuration;
      /// <summary>
      /// Delay after start of dump animation to actual material transfer. Should be less than <see cref="F:Mafi.Core.Vehicles.Excavators.ExcavatorProto.Timings.DumpDuration" />.
      /// </summary>
      public readonly Duration DumpDelay;
      /// <summary>
      /// Number of actual mine operations on the terrain. This should be in sync with
      /// </summary>
      public readonly int MineTileIterations;
      /// <summary>
      /// A delay between mine operations. Keep in ming that <see cref="F:Mafi.Core.Vehicles.Excavators.ExcavatorProto.Timings.MineIterationDuration" /> times <see cref="F:Mafi.Core.Vehicles.Excavators.ExcavatorProto.Timings.MineTileIterations" /> should be less or equal to <see cref="F:Mafi.Core.Vehicles.Excavators.ExcavatorProto.Timings.MineDuration" />.
      /// </summary>
      public readonly Duration MineIterationDuration;

      public Timings(
        Duration prepareToMineDuration,
        Duration mineDuration,
        Duration prepareToDumpDuration,
        Duration dumpDuration,
        Duration dumpDelay,
        int mineTileIterations,
        Duration mineIterationDuration)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Assert.That<Duration>(mineDuration).IsGreaterOrEqual(mineTileIterations * mineIterationDuration);
        this.PrepareToMineDuration = prepareToMineDuration.CheckPositive();
        this.MineDuration = mineDuration.CheckPositive();
        this.PrepareToDumpDuration = prepareToDumpDuration.CheckPositive();
        this.DumpDuration = dumpDuration.CheckPositive();
        this.DumpDelay = dumpDelay.CheckWithinIncl(Duration.OneTick, dumpDuration);
        this.MineTileIterations = mineTileIterations.CheckPositive();
        this.MineIterationDuration = mineIterationDuration.CheckPositive();
      }

      [Pure]
      public Duration GetDuration(ExcavatorShovelState state)
      {
        switch (state)
        {
          case ExcavatorShovelState.PrepareToMine:
            return this.PrepareToMineDuration;
          case ExcavatorShovelState.Mine:
            return this.MineDuration;
          case ExcavatorShovelState.PrepareToDump:
            return this.PrepareToDumpDuration;
          case ExcavatorShovelState.Dump:
            return this.DumpDuration;
          default:
            return Duration.Zero;
        }
      }
    }

    public new class Gfx : DynamicGroundEntityProto.Gfx
    {
      public static readonly ExcavatorProto.Gfx Empty;
      public readonly string CabinModelName;
      public readonly string LeftTrackModelName;
      public readonly string RightTrackModelName;
      public readonly RelTile1f SpacingBetweenTracks;
      public readonly RelTile1f TrackTextureLength;
      public readonly string IdleStateName;
      public readonly string PrepareToMineStateName;
      public readonly string MiningStateName;
      public readonly string PrepareToDumpStateName;
      public readonly string DumpingStateName;
      public readonly string PileParentPath;
      public readonly string SmoothPileHalf;
      public readonly string SmoothPileFull;
      public readonly string RoughPileHalf;
      public readonly string RoughPileFull;
      public readonly ImmutableArray<string> DigSounds;
      public readonly ImmutableArray<string> DumpSounds;
      /// <summary>
      /// We have to compensate cabin angle because the models cabin incorrectly face +Y or -X instead of +X.
      /// Remove this when the cabins are fixed.
      /// </summary>
      public readonly AngleDegrees1f CabinAngleCompensation;

      public Gfx(
        string prefabPath,
        Option<string> customIconPath,
        RelTile2f frontContactPtsOffset,
        RelTile2f rearContactPtsOffset,
        ImmutableArray<DynamicEntityDustParticlesSpec> dustParticles,
        Option<VehicleExhaustParticlesSpec> exhaustParticlesSpec,
        string engineSoundPath,
        string movementSoundPath,
        string cabinModelName,
        string leftTrackModelName,
        string rightTrackModelName,
        RelTile1f spacingBetweenTracks,
        RelTile1f trackTextureLength,
        string idleStateName,
        string prepareToMineStateName,
        string miningStateName,
        string prepareToDumpStateName,
        string dumpingStateName,
        string pileParentPath,
        string smoothPileHalf,
        string smoothPileFull,
        string roughPileHalf,
        string roughPileFull,
        ImmutableArray<string> digSounds,
        ImmutableArray<string> dumpSounds,
        AngleDegrees1f cabinAngleCompensation = default (AngleDegrees1f))
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, customIconPath, frontContactPtsOffset, rearContactPtsOffset, dustParticles, exhaustParticlesSpec, engineSoundPath, movementSoundPath);
        this.CabinModelName = cabinModelName.CheckNotNullOrEmpty();
        this.LeftTrackModelName = leftTrackModelName.CheckNotNullOrEmpty();
        this.RightTrackModelName = rightTrackModelName.CheckNotNullOrEmpty();
        this.SpacingBetweenTracks = spacingBetweenTracks.CheckPositive();
        this.TrackTextureLength = trackTextureLength.CheckPositive();
        this.IdleStateName = idleStateName.CheckNotNullOrEmpty();
        this.PrepareToMineStateName = prepareToMineStateName.CheckNotNullOrEmpty();
        this.MiningStateName = miningStateName.CheckNotNullOrEmpty();
        this.PrepareToDumpStateName = prepareToDumpStateName.CheckNotNullOrEmpty();
        this.DumpingStateName = dumpingStateName.CheckNotNullOrEmpty();
        this.PileParentPath = pileParentPath.CheckNotNullOrEmpty();
        this.SmoothPileHalf = smoothPileHalf;
        this.SmoothPileFull = smoothPileFull;
        this.RoughPileHalf = roughPileHalf;
        this.RoughPileFull = roughPileFull;
        this.DigSounds = digSounds;
        this.DumpSounds = dumpSounds;
        this.CabinAngleCompensation = cabinAngleCompensation;
      }

      [Pure]
      public string GetAnimatorStateName(ExcavatorShovelState state)
      {
        switch (state)
        {
          case ExcavatorShovelState.PrepareToMine:
            return this.PrepareToMineStateName;
          case ExcavatorShovelState.Mine:
            return this.MiningStateName;
          case ExcavatorShovelState.PrepareToDump:
            return this.PrepareToDumpStateName;
          case ExcavatorShovelState.Dump:
            return this.DumpingStateName;
          default:
            return this.IdleStateName;
        }
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Option<string> customIconPath = (Option<string>) "EMPTY";
        RelTile2f frontContactPtsOffset = new RelTile2f((Fix32) 1, (Fix32) 1);
        RelTile2f rearContactPtsOffset = new RelTile2f((Fix32) 1, (Fix32) 1);
        ImmutableArray<DynamicEntityDustParticlesSpec> empty1 = ImmutableArray<DynamicEntityDustParticlesSpec>.Empty;
        Option<VehicleExhaustParticlesSpec> none = Option<VehicleExhaustParticlesSpec>.None;
        ImmutableArray<string> empty2 = ImmutableArray<string>.Empty;
        ImmutableArray<string> empty3 = ImmutableArray<string>.Empty;
        RelTile1f spacingBetweenTracks = 1.0.Tiles();
        RelTile1f trackTextureLength = 1.0.Tiles();
        ImmutableArray<string> digSounds = empty2;
        ImmutableArray<string> dumpSounds = empty3;
        AngleDegrees1f cabinAngleCompensation = new AngleDegrees1f();
        ExcavatorProto.Gfx.Empty = new ExcavatorProto.Gfx("EMPTY", customIconPath, frontContactPtsOffset, rearContactPtsOffset, empty1, none, "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", spacingBetweenTracks, trackTextureLength, "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", digSounds, dumpSounds, cabinAngleCompensation);
      }
    }
  }
}
