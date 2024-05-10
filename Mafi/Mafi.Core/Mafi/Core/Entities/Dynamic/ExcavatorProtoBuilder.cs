// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.ExcavatorProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;
using Mafi.Core.PathFinding;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public class ExcavatorProtoBuilder : IProtoBuilder
  {
    private readonly FuelTankProtoBuilder m_tankBuilder;

    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public ExcavatorProtoBuilder(ProtoRegistrator registrator, FuelTankProtoBuilder tankBuilder)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
      this.m_tankBuilder = tankBuilder;
    }

    public ExcavatorProtoBuilder.ExcavatorProtoBuilderState Start(
      string name,
      DynamicEntityProto.ID id)
    {
      return new ExcavatorProtoBuilder.ExcavatorProtoBuilderState((IProtoBuilder) this, id, name, this.m_tankBuilder);
    }

    public class ExcavatorProtoBuilderState : 
      DrivingEntityProtoBuilderState<ExcavatorProtoBuilder.ExcavatorProtoBuilderState>
    {
      protected readonly DynamicEntityProto.ID ProtoId;
      private Quantity? m_capacity;
      private Option<RotatingCabinDriverProto> m_rotatingCabinDriverProto;
      private RelTile1i? m_minMiningDistance;
      private RelTile1i? m_maxMiningDistance;
      private ImmutableArray<ThicknessTilesF> m_minedThickByDist;
      private ExcavatorProto.Timings? m_mineTimings;
      private Func<TruckProto, bool> m_isTruckSupportedFunc;
      private Option<DrivingEntityProto> m_nextTier;
      private string m_cabinModelName;
      private string m_leftTrackModelName;
      private string m_rightTrackModelName;
      private string m_pileParentPath;
      private string m_smoothPileHalf;
      private string m_smoothPileFull;
      private string m_roughPileHalf;
      private string m_roughPileFull;
      private RelTile1f? m_spacingBetweenTracks;
      private RelTile1f? m_trackTextureLength;
      private string m_idleStateName;
      private string m_prepareToMineStateName;
      private string m_mineStateName;
      private string m_prepareToDumpStateName;
      private string m_dumpingStateName;
      private string m_engineSoundPath;
      private string m_movementSoundPath;
      private AngleDegrees1f m_cabinAngleCompensationDeg;
      private ImmutableArray<string> m_digSoundsPaths;
      private ImmutableArray<string> m_dumpSoundsPaths;

      public ExcavatorProtoBuilderState(
        IProtoBuilder builder,
        DynamicEntityProto.ID id,
        string name,
        FuelTankProtoBuilder tankBuilder)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_isTruckSupportedFunc = (Func<TruckProto, bool>) (_ => true);
        this.m_nextTier = Option<DrivingEntityProto>.None;
        this.m_digSoundsPaths = ImmutableArray<string>.Empty;
        this.m_dumpSoundsPaths = ImmutableArray<string>.Empty;
        // ISSUE: explicit constructor call
        base.\u002Ector(builder, (Proto.ID) id, name, tankBuilder);
        this.ProtoId = id;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetCapacity(int quantity)
      {
        this.m_capacity = new Quantity?(new Quantity(quantity).CheckPositive());
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetMinedThicknessByDistanceMeters(
        params float[] thicknessMeters)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_minedThickByDist = ((IEnumerable<float>) thicknessMeters).Select<float, ThicknessTilesF>(ExcavatorProtoBuilder.ExcavatorProtoBuilderState.\u003C\u003EO.\u003C0\u003E__FromMeters ?? (ExcavatorProtoBuilder.ExcavatorProtoBuilderState.\u003C\u003EO.\u003C0\u003E__FromMeters = new Func<float, ThicknessTilesF>(ThicknessTilesF.FromMeters))).ToImmutableArray<ThicknessTilesF>();
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetCabinDriver(
        RotatingCabinDriverProto rotatingCabinDriverProto)
      {
        this.m_rotatingCabinDriverProto = (Option<RotatingCabinDriverProto>) rotatingCabinDriverProto.CheckNotNull<RotatingCabinDriverProto>();
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetMaxMiningDistance(
        RelTile1i minMiningDistance,
        RelTile1i maxMiningDistance)
      {
        this.m_minMiningDistance = new RelTile1i?(minMiningDistance.CheckPositive());
        this.m_maxMiningDistance = new RelTile1i?(maxMiningDistance.CheckPositive());
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetIsTruckSupportedFunc(
        Func<TruckProto, bool> isTruckSupportedFunc)
      {
        this.m_isTruckSupportedFunc = isTruckSupportedFunc;
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetNextTier(
        DrivingEntityProto nextTier)
      {
        this.m_nextTier = (Option<DrivingEntityProto>) nextTier;
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetSubmodelNames(
        string cabin,
        string leftTrack,
        string rightTrack,
        string pileParent,
        string smoothPileHalf,
        string smoothPileFull,
        string roughPileHalf,
        string roughPileFull)
      {
        this.m_cabinModelName = cabin;
        this.m_leftTrackModelName = leftTrack;
        this.m_rightTrackModelName = rightTrack;
        this.m_pileParentPath = pileParent;
        this.m_smoothPileHalf = smoothPileHalf;
        this.m_smoothPileFull = smoothPileFull;
        this.m_roughPileHalf = roughPileHalf;
        this.m_roughPileFull = roughPileFull;
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetAnimationTimings(
        Duration prepareToMineDuration,
        Duration mineDuration,
        int mineIterations,
        Duration mineIterationDuration,
        Duration prepareToDumpDuration,
        Duration dumpDuration,
        Duration dumpDelay)
      {
        this.m_mineTimings = new ExcavatorProto.Timings?(new ExcavatorProto.Timings(prepareToMineDuration, mineDuration, prepareToDumpDuration, dumpDuration, dumpDelay, mineIterations, mineIterationDuration));
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetAnimationNames(
        string idleStateName,
        string prepareToMineStateName,
        string mineStateName,
        string prepareToDumpStateName,
        string dumpingStateName)
      {
        this.m_idleStateName = idleStateName;
        this.m_prepareToMineStateName = prepareToMineStateName;
        this.m_mineStateName = mineStateName;
        this.m_prepareToDumpStateName = prepareToDumpStateName;
        this.m_dumpingStateName = dumpingStateName;
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetCabinAngleCompensationDegrees(
        AngleDegrees1f degrees)
      {
        this.m_cabinAngleCompensationDeg = degrees;
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetTracksParameters(
        RelTile1f spacingBetweenTracks,
        RelTile1f trackTextureLength)
      {
        this.m_spacingBetweenTracks = new RelTile1f?(spacingBetweenTracks);
        this.m_trackTextureLength = new RelTile1f?(trackTextureLength);
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetEngineSound(string engineSoundPath)
      {
        this.m_engineSoundPath = engineSoundPath;
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetMovementSound(
        string movementSoundPath)
      {
        this.m_movementSoundPath = movementSoundPath;
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetDigSounds(
        ImmutableArray<string> digSoundsPaths)
      {
        this.m_digSoundsPaths = digSoundsPaths;
        return this;
      }

      [MustUseReturnValue]
      public ExcavatorProtoBuilder.ExcavatorProtoBuilderState SetDumpSounds(
        ImmutableArray<string> dumpSoundsPaths)
      {
        this.m_dumpSoundsPaths = dumpSoundsPaths;
        return this;
      }

      private ExcavatorProto.Gfx createGfx()
      {
        return this.PrefabPath.IsNone ? ExcavatorProto.Gfx.Empty : new ExcavatorProto.Gfx(this.PrefabPath.Value, this.CustomIconPath, this.ValueOrThrow<RelTile2f>(this.FrontContactPtsOffset, "FrontContactPtsOffset"), this.ValueOrThrow<RelTile2f>(this.RearContactPtsOffset, "RearContactPtsOffset"), this.DustParticlesSpecs.ToImmutableArray(), this.ExhaustParticlesSpec, this.m_engineSoundPath ?? "EMPTY", this.m_movementSoundPath ?? "EMPTY", this.ValueOrThrow(this.m_cabinModelName, "cabinModelName"), this.ValueOrThrow(this.m_leftTrackModelName, "leftTrackModelName"), this.ValueOrThrow(this.m_rightTrackModelName, "rightTrackModelName"), this.ValueOrThrow<RelTile1f>(this.m_spacingBetweenTracks, "SpacingBetweenTracks"), this.ValueOrThrow<RelTile1f>(this.m_trackTextureLength, "TrackTextureLength"), this.ValueOrThrow(this.m_idleStateName, "idleStateName"), this.ValueOrThrow(this.m_prepareToMineStateName, "prepareToMineStateName"), this.ValueOrThrow(this.m_mineStateName, "mineStateName"), this.ValueOrThrow(this.m_prepareToDumpStateName, "prepareToDumpStateName"), this.ValueOrThrow(this.m_dumpingStateName, "dumpingStateName"), this.ValueOrThrow(this.m_pileParentPath, "pileParentModelName"), this.ValueOrThrow(this.m_smoothPileHalf, "smoothPileHalf"), this.ValueOrThrow(this.m_smoothPileFull, "smoothPileFull"), this.ValueOrThrow(this.m_roughPileHalf, "roughPileHalf"), this.ValueOrThrow(this.m_roughPileFull, "roughPileFull"), this.m_digSoundsPaths, this.m_dumpSoundsPaths, this.m_cabinAngleCompensationDeg);
      }

      public ExcavatorProto BuildAndAdd()
      {
        DynamicEntityProto.ID protoId = this.ProtoId;
        Proto.Str strings = this.Strings;
        RelTile3f entitySize = this.ValueOrThrow<RelTile3f>(this.Size, "Entity size");
        DrivingData drivingData1 = this.ValueOrThrow<DrivingData>(this.DrivingData, "DrivingData");
        EntityCosts costs = this.Costs;
        DrivingData drivingData2 = drivingData1;
        RotatingCabinDriverProto rotatingCabinDriverProto = this.ValueOrThrow<RotatingCabinDriverProto>(this.m_rotatingCabinDriverProto, "RotatingCabinDriverProto");
        Quantity capacity = this.ValueOrThrow<Quantity>(this.m_capacity, "Capacity");
        RelTile1i minMiningDistance = this.ValueOrThrow<RelTile1i>(this.m_minMiningDistance, "minMiningDistance");
        RelTile1i maxMiningDistance = this.ValueOrThrow<RelTile1i>(this.m_maxMiningDistance, "maxMiningDistance");
        ImmutableArray<ThicknessTilesF> minedThicknessByDistance = this.NotEmptyOrThrow<ThicknessTilesF>(this.m_minedThickByDist, "MinedThicknessByDistance");
        ExcavatorProto.Timings mineTimings = this.ValueOrThrow<ExcavatorProto.Timings>(this.m_mineTimings, "mineTimings");
        Option<FuelTankProto> fuelTank = this.FuelTank;
        VehiclePathFindingParams pathFindingParams = this.ValueOrThrow<VehiclePathFindingParams>(this.PathFindingParams, "PathFindingParams");
        ImmutableArray<ThicknessTilesF> disruptionByDistance = this.NotEmptyOrThrow<ThicknessTilesF>(this.DisruptionByDistance, "DisruptionByDistance");
        Duration durationToBuild = this.ValueOrThrow<Duration>(this.DurationToBuild, "DurationToBuild");
        Func<TruckProto, bool> truckSupportedFunc = this.m_isTruckSupportedFunc;
        Option<DrivingEntityProto> nextTier = this.m_nextTier;
        ExcavatorProto.Gfx gfx = this.createGfx();
        return this.AddToDb<ExcavatorProto>(new ExcavatorProto(protoId, strings, entitySize, costs, 1, drivingData2, rotatingCabinDriverProto, capacity, minMiningDistance, maxMiningDistance, minedThicknessByDistance, mineTimings, fuelTank, pathFindingParams, disruptionByDistance, durationToBuild, truckSupportedFunc, nextTier, gfx));
      }
    }
  }
}
