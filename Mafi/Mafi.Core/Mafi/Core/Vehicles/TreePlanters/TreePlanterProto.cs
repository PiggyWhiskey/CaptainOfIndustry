// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.TreePlanters.TreePlanterProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.TreePlanters
{
  public class TreePlanterProto : DrivingEntityProto
  {
    public readonly TreePlanterProto.Timings PlantingTimings;
    public readonly Duration CargoPickupDuration;
    public readonly ProductProto ProductProto;
    public readonly Quantity Capacity;
    /// <summary>Parameters of the cabin driver.</summary>
    public readonly RotatingCabinDriverProto RotatingCabinDriverProto;
    public readonly RelTile1i TreePlantDistance;
    public readonly TreePlanterProto.Gfx Graphics;

    public override Type EntityType => typeof (TreePlanter);

    public TreePlanterProto(
      DynamicEntityProto.ID id,
      Proto.Str strings,
      RelTile3f entitySize,
      EntityCosts costs,
      int vehicleQuotaCost,
      DrivingData drivingData,
      RotatingCabinDriverProto rotatingCabinDriverProto,
      Option<Mafi.Core.Entities.Dynamic.FuelTankProto> fuelTankProto,
      RelTile1i treePlantDistance,
      TreePlanterProto.Timings timings,
      Duration cargoPickupDuration,
      ProductProto productProto,
      Quantity capacity,
      VehiclePathFindingParams pathFindingParams,
      ImmutableArray<ThicknessTilesF> disruptionByDistance,
      Duration durationToBuild,
      Option<DrivingEntityProto> nextTier,
      TreePlanterProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, entitySize, costs, vehicleQuotaCost, drivingData, fuelTankProto, pathFindingParams, disruptionByDistance, durationToBuild, nextTier, (DynamicGroundEntityProto.Gfx) graphics);
      this.TreePlantDistance = treePlantDistance;
      this.PlantingTimings = timings;
      this.CargoPickupDuration = cargoPickupDuration;
      this.ProductProto = productProto;
      this.Capacity = capacity;
      this.RotatingCabinDriverProto = rotatingCabinDriverProto;
      this.Graphics = graphics;
    }

    /// <summary>
    /// Timing parameters of tree planting, used in simulation.
    /// </summary>
    public class Timings
    {
      public readonly Duration PlantingDuration;
      public readonly Duration ReturningToIdleDuration;

      public Timings(Duration plantingDuration, Duration returningToIdleDuration)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.PlantingDuration = plantingDuration.CheckPositive();
        this.ReturningToIdleDuration = returningToIdleDuration.CheckPositive();
      }
    }

    public new class Gfx : DynamicGroundEntityProto.Gfx
    {
      public static readonly TreePlanterProto.Gfx Empty;
      public readonly string CabinObjectPath;
      public readonly string LeftTrackObjectPath;
      public readonly string RightTrackObjectPath;
      public readonly string TreesBaseObjectPath;
      public readonly int NumTrees;
      public readonly RelTile1f SpacingBetweenTracks;
      public readonly RelTile1f TrackTextureLength;
      public readonly string IdleAnimStateName;
      public readonly string PlantingAnimStateName;

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
        string treesBaseObjectPath,
        int numTrees,
        RelTile1f spacingBetweenTracks,
        RelTile1f trackTextureLength,
        string idleAnimStateName,
        string plantingAnimStateName)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, customIconPath, frontContactPtsOffset, rearContactPtsOffset, dustParticles, exhaustParticlesSpec, engineSoundPath, movementSoundPath);
        this.CabinObjectPath = cabinObjectPath.CheckNotNullOrEmpty();
        this.LeftTrackObjectPath = leftTrackObjectPath.CheckNotNullOrEmpty();
        this.RightTrackObjectPath = rightTrackObjectPath.CheckNotNullOrEmpty();
        this.TreesBaseObjectPath = treesBaseObjectPath.CheckNotNullOrEmpty();
        this.NumTrees = numTrees.CheckNotNegative();
        this.SpacingBetweenTracks = spacingBetweenTracks.CheckPositive();
        this.TrackTextureLength = trackTextureLength.CheckPositive();
        this.IdleAnimStateName = idleAnimStateName.CheckNotNullOrEmpty();
        this.PlantingAnimStateName = plantingAnimStateName.CheckNotNullOrEmpty();
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TreePlanterProto.Gfx.Empty = new TreePlanterProto.Gfx("EMPTY", (Option<string>) Option.None, RelTile2f.One, RelTile2f.One, ImmutableArray<DynamicEntityDustParticlesSpec>.Empty, Option<VehicleExhaustParticlesSpec>.None, "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", 0, 1.0.Tiles(), 1.0.Tiles(), "EMPTY", "EMPTY");
      }
    }
  }
}
