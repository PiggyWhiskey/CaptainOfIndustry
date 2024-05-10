// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.TreeHarvesters.TreeHarvesterProto
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
namespace Mafi.Core.Vehicles.TreeHarvesters
{
  public class TreeHarvesterProto : DrivingEntityProto
  {
    public readonly TreeHarvesterProto.Timings HarvestTimings;
    /// <summary>Parameters of the cabin driver.</summary>
    public readonly RotatingCabinDriverProto RotatingCabinDriverProto;
    public readonly RelTile1i TreeHarvestDistance;
    public readonly TreeHarvesterProto.Gfx Graphics;
    private readonly Func<TruckProto, bool> m_isTruckSupportedFunc;

    public override Type EntityType => typeof (TreeHarvester);

    public TreeHarvesterProto(
      DynamicEntityProto.ID id,
      Proto.Str strings,
      RelTile3f entitySize,
      EntityCosts costs,
      int vehicleQuotaCost,
      DrivingData drivingData,
      RotatingCabinDriverProto rotatingCabinDriverProto,
      Option<Mafi.Core.Entities.Dynamic.FuelTankProto> fuelTankProto,
      RelTile1i treeHarvestDistance,
      TreeHarvesterProto.Timings timings,
      VehiclePathFindingParams pathFindingParams,
      ImmutableArray<ThicknessTilesF> disruptionByDistance,
      Duration durationToBuild,
      Func<TruckProto, bool> isTruckSupportedFunc,
      Option<DrivingEntityProto> nextTier,
      TreeHarvesterProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, entitySize, costs, vehicleQuotaCost, drivingData, fuelTankProto, pathFindingParams, disruptionByDistance, durationToBuild, nextTier, (DynamicGroundEntityProto.Gfx) graphics);
      this.TreeHarvestDistance = treeHarvestDistance;
      this.HarvestTimings = timings;
      this.RotatingCabinDriverProto = rotatingCabinDriverProto;
      this.m_isTruckSupportedFunc = isTruckSupportedFunc;
      this.Graphics = graphics;
    }

    public bool IsTruckSupported(TruckProto truck) => this.m_isTruckSupportedFunc(truck);

    /// <summary>
    /// Timing parameters of tree harvesting, used in simulation.
    /// </summary>
    public class Timings
    {
      public readonly Duration ToPrepareForHarvestDuration;
      public readonly Duration ToTreeLayingDownDuration;
      public readonly Duration ToTreeAboveTruckDuration;
      public readonly Duration ToTreeOnTruckDuration;
      public readonly Duration ToArmUpDuration;
      public readonly Duration MoveToNextSectionDuration;
      public readonly Duration CutNextSectionDuration;
      public readonly Duration ToFoldedDuration;
      /// <summary>
      /// Time it takes for the harvester's saw to saw off a tree.
      /// </summary>
      public readonly Duration CuttingDuration;
      /// <summary>
      /// The time it takes for the harvester to cut off branches of a tree. This is done after a tree is cut and
      /// layed down.
      /// </summary>
      public readonly Duration TrimmingDuration;

      public Timings(
        Duration toPrepareForHarvestDuration,
        Duration toTreeLayingDownDuration,
        Duration toTreeAboveTruckDuration,
        Duration toTreeOnTruckDuration,
        Duration toArmUpDuration,
        Duration toFoldedDuration,
        Duration cuttingDuration,
        Duration trimmingDuration,
        Duration moveToNextSectionDuration,
        Duration cutNextSectionDuration)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.ToPrepareForHarvestDuration = toPrepareForHarvestDuration.CheckPositive();
        this.ToTreeLayingDownDuration = toTreeLayingDownDuration.CheckPositive();
        this.ToTreeAboveTruckDuration = toTreeAboveTruckDuration.CheckPositive();
        this.ToTreeOnTruckDuration = toTreeOnTruckDuration.CheckPositive();
        this.ToArmUpDuration = toArmUpDuration.CheckPositive();
        this.MoveToNextSectionDuration = moveToNextSectionDuration.CheckPositive();
        this.CutNextSectionDuration = cutNextSectionDuration.CheckPositive();
        this.ToFoldedDuration = toFoldedDuration.CheckPositive();
        this.CuttingDuration = cuttingDuration.CheckPositive();
        this.TrimmingDuration = trimmingDuration.CheckPositive();
      }
    }

    public new class Gfx : DynamicGroundEntityProto.Gfx
    {
      public static readonly TreeHarvesterProto.Gfx Empty;
      public readonly string CabinObjectPath;
      public readonly string LeftTrackObjectPath;
      public readonly string RightTrackObjectPath;
      public readonly RelTile1f SpacingBetweenTracks;
      public readonly RelTile1f TrackTextureLength;
      public readonly float TreeHolderOffset;
      public readonly float GripperWidth;
      public readonly string IdleAnimStateName;
      public readonly string PreparedForHarvestAnimStateName;
      public readonly string TreeLayingDownAnimStateName;
      public readonly string TreeAboveTruckAnimStateName;
      public readonly string TreeOnTruckAnimStateName;
      public readonly string TreeFromTruckAnimStateName;
      public readonly string FoldedAnimStateName;
      public readonly string HarvestedTreeParentObjectPath;
      public readonly string RotatingHandObjectPath;

      public Gfx(
        string prefabPath,
        Option<string> customIconPath,
        RelTile2f frontContactPtsOffset,
        RelTile2f rearContactPtsOffset,
        ImmutableArray<DynamicEntityDustParticlesSpec> dustParticles,
        Option<VehicleExhaustParticlesSpec> exhaustParticlesSpec,
        string engineSoundPath,
        string movementSoundPath,
        string cabinObjectPath,
        string leftTrackObjectPath,
        string rightTrackObjectPath,
        RelTile1f spacingBetweenTracks,
        RelTile1f trackTextureLength,
        float treeHolderOffset,
        float gripperWidth,
        string idleAnimStateName,
        string preparedForHarvestAnimStateName,
        string treeLayingDownAnimStateName,
        string treeAboveTruckAnimStateName,
        string treeOnTruckAnimStateName,
        string treeFromTruckAnimStateName,
        string foldedAnimStateName,
        string harvestedTreeParentObjectPath,
        string rotatingHandObjectPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, customIconPath, frontContactPtsOffset, rearContactPtsOffset, dustParticles, exhaustParticlesSpec, engineSoundPath, movementSoundPath);
        this.CabinObjectPath = cabinObjectPath.CheckNotNullOrEmpty();
        this.LeftTrackObjectPath = leftTrackObjectPath.CheckNotNullOrEmpty();
        this.RightTrackObjectPath = rightTrackObjectPath.CheckNotNullOrEmpty();
        this.SpacingBetweenTracks = spacingBetweenTracks.CheckPositive();
        this.TrackTextureLength = trackTextureLength.CheckPositive();
        this.IdleAnimStateName = idleAnimStateName.CheckNotNullOrEmpty();
        this.PreparedForHarvestAnimStateName = preparedForHarvestAnimStateName.CheckNotNullOrEmpty();
        this.TreeLayingDownAnimStateName = treeLayingDownAnimStateName.CheckNotNullOrEmpty();
        this.TreeAboveTruckAnimStateName = treeAboveTruckAnimStateName.CheckNotNullOrEmpty();
        this.TreeOnTruckAnimStateName = treeOnTruckAnimStateName.CheckNotNullOrEmpty();
        this.TreeFromTruckAnimStateName = treeFromTruckAnimStateName.CheckNotNullOrEmpty();
        this.FoldedAnimStateName = foldedAnimStateName.CheckNotNullOrEmpty();
        this.TreeHolderOffset = treeHolderOffset;
        this.GripperWidth = gripperWidth.CheckPositive();
        this.HarvestedTreeParentObjectPath = harvestedTreeParentObjectPath;
        this.RotatingHandObjectPath = rotatingHandObjectPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TreeHarvesterProto.Gfx.Empty = new TreeHarvesterProto.Gfx("EMPTY", (Option<string>) Option.None, RelTile2f.One, RelTile2f.One, ImmutableArray<DynamicEntityDustParticlesSpec>.Empty, Option<VehicleExhaustParticlesSpec>.None, "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", 1.0.Tiles(), 1.0.Tiles(), -1.8f, 1.8f, "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY");
      }
    }
  }
}
