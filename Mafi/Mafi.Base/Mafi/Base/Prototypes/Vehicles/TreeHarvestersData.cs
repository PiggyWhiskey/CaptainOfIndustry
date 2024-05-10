// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Vehicles.TreeHarvestersData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Mods;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Vehicles
{
  public class TreeHarvestersData : IModData
  {
    private LocStr treeHarvesterT2Desc;

    public void RegisterData(ProtoRegistrator registrator)
    {
      TreeHarvesterProto treeHarvesterT2_1 = this.createTreeHarvesterT2(registrator, Ids.Vehicles.TreeHarvesterT2, Ids.Products.Diesel, Costs.Vehicles.TreeHarvesterT2, "Assets/Base/Vehicles/TreeHarvester.prefab");
      TreeHarvesterProto treeHarvesterT2_2 = this.createTreeHarvesterT2(registrator, Ids.Vehicles.TreeHarvesterT2H, Ids.Products.Hydrogen, Costs.Vehicles.TreeHarvesterT2H, "Assets/Base/Vehicles/TreeHarvesterT2Hydrogen.prefab");
      TreeHarvesterProto treeHarvesterT1 = this.createTreeHarvesterT1(registrator);
      VehicleDepotProto orThrow1 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepot);
      VehicleDepotProto orThrow2 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepotT2);
      VehicleDepotProto orThrow3 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepotT3);
      orThrow1.AddBuildableEntity((DynamicGroundEntityProto) treeHarvesterT1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) treeHarvesterT1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) treeHarvesterT2_1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) treeHarvesterT2_2);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) treeHarvesterT1);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) treeHarvesterT2_1);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) treeHarvesterT2_2);
    }

    private TreeHarvesterProto createTreeHarvesterT1(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      DynamicEntityProto.ID treeHarvesterT1 = Ids.Vehicles.TreeHarvesterT1;
      Proto.Str str = Proto.CreateStr((Proto.ID) Ids.Vehicles.TreeHarvesterT1, "Tree harvester", "A machine for harvesting trees. Make sure to assign service trucks to use it.  It cannot pass under transports.");
      RelTile3f entitySize = RelTile3f.FromDimensionsInMeters(6.0, 4.0, 5.0);
      EntityCosts entityCosts = Costs.Vehicles.TreeHarvesterT1.MapToEntityCosts(registrator);
      DrivingData drivingData = new DrivingData(0.6.Tiles(), 0.4.Tiles(), 50.Percent(), 0.04.Tiles(), 0.06.Tiles(), 8.Degrees(), 1.5.Degrees(), (Fix32) 2, RelTile1f.Zero, RelTile1f.Zero);
      RotatingCabinDriverProto rotatingCabinDriverProto = new RotatingCabinDriverProto(7.Degrees(), 0.8.Degrees(), 2.Degrees(), 2.5.ToFix32());
      Option<FuelTankProto> fuelTankProto = registrator.DisableVehicleFuelConsumption ? Option<FuelTankProto>.None : (Option<FuelTankProto>) registrator.FuelTankProtoBuilder.Start((Proto.ID) Ids.Vehicles.TreeHarvesterT1).SetReserve(3.Minutes()).SetProduct((Proto.ID) Ids.Products.Diesel, new Quantity(12), 15.Minutes()).SetWasteProduct((Proto.ID) Ids.Products.PollutedAir, TrucksData.DIESEL_POLLUTION_RATIO).BuildTank();
      RelTile1i treeHarvestDistance = 7.Tiles();
      Duration toPrepareForHarvestDuration = Duration.FromKeyframes(47, 0.8f);
      Duration toTreeLayingDownDuration = Duration.FromKeyframes(58, 0.8f);
      Duration toTreeAboveTruckDuration = Duration.FromKeyframes(32, 0.6f);
      Duration toTreeOnTruckDuration = Duration.FromKeyframes(27, 0.8f);
      Duration toArmUpDuration = Duration.FromKeyframes(16, 0.8f);
      Duration duration1 = 1.5.Seconds();
      Duration duration2 = 0.8.Seconds();
      Duration toFoldedDuration = Duration.FromKeyframes(30, 0.8f);
      Duration cuttingDuration = 2.Seconds();
      Duration trimmingDuration = 5.Seconds();
      Duration moveToNextSectionDuration = duration1;
      Duration cutNextSectionDuration = duration2;
      TreeHarvesterProto.Timings timings = new TreeHarvesterProto.Timings(toPrepareForHarvestDuration, toTreeLayingDownDuration, toTreeAboveTruckDuration, toTreeOnTruckDuration, toArmUpDuration, toFoldedDuration, cuttingDuration, trimmingDuration, moveToNextSectionDuration, cutNextSectionDuration);
      VehiclePathFindingParams pathFindingParams = new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 30.Percent());
      ImmutableArray<ThicknessTilesF> immutableArray = ((IReadOnlyCollection<double>) new double[2]
      {
        0.0,
        0.2
      }).ToImmutableArray<double, ThicknessTilesF>((Func<double, ThicknessTilesF>) (x => new ThicknessTilesF(x.ToFix32())));
      Duration durationToBuild = 80.Seconds();
      Option<DrivingEntityProto> orThrow = (Option<DrivingEntityProto>) registrator.PrototypesDb.GetOrThrow<DrivingEntityProto>((Proto.ID) Ids.Vehicles.TreeHarvesterT2);
      TreeHarvesterProto.Gfx graphics = new TreeHarvesterProto.Gfx("Assets/Base/Vehicles/TreeHarvesterT1.prefab", Option<string>.None, RelTile2f.FromMeters((Fix32) 3, (Fix32) 3), RelTile2f.FromMeters((Fix32) 3, (Fix32) 3), ImmutableArray.Create<DynamicEntityDustParticlesSpec>(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.5f, new RelTile3f((Fix32) 0, (Fix32) 0, (Fix32) 0), 50f, 0.1.Tiles(), 0.5f)), Option<VehicleExhaustParticlesSpec>.None, "Assets/Base/Vehicles/ExcavatorT1/Audio/Engine.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Treads.prefab", "rotate_base", "TrackLeft", "TrackRight", 3.3.Tiles(), 3.0.Meters(), -1.8f, 1.8f, "Idle", "GrabTree", "CutTree", "RaiseTree", "DropTree", "ArmUpFromDrop", "FoldArm", "rotate_base/rotate_rameno/TH_T1_base_helper1/TH_T1_handA/TH_T1_handB/TH_T1_handC/TH_T1_handD/TH_T1_hand_cutter/TreeHolder", "rotate_base/rotate_rameno/TH_T1_base_helper1/TH_T1_handA/TH_T1_handB/TH_T1_handC/TH_T1_handD/TH_T1_hand_cutter");
      TreeHarvesterProto proto = new TreeHarvesterProto(treeHarvesterT1, str, entitySize, entityCosts, 1, drivingData, rotatingCabinDriverProto, fuelTankProto, treeHarvestDistance, timings, pathFindingParams, immutableArray, durationToBuild, (Func<TruckProto, bool>) (truck => !truck.ProductType.HasValue || truck.ProductType.Value == CountableProductProto.ProductType), orThrow, graphics);
      return prototypesDb.Add<TreeHarvesterProto>(proto);
    }

    private TreeHarvesterProto createTreeHarvesterT2(
      ProtoRegistrator registrator,
      DynamicEntityProto.ID vehicleId,
      ProductProto.ID fuelProductId,
      EntityCostsTpl costs,
      string prefabPath)
    {
      Func<FuelTankProtoBuilder, FuelTankProto> fuelTank = TrucksData.createFuelTank(vehicleId, 3.Minutes(), fuelProductId, new Quantity(32), 17.8.Minutes());
      ProtosDb prototypesDb = registrator.PrototypesDb;
      DynamicEntityProto.ID id = vehicleId;
      Proto.Str str = Proto.CreateStr((Proto.ID) vehicleId, "Large tree harvester", this.treeHarvesterT2Desc);
      RelTile3f entitySize = RelTile3f.FromDimensionsInMeters(8.0, 5.0, 5.0);
      EntityCosts entityCosts = costs.MapToEntityCosts(registrator);
      DrivingData drivingData = new DrivingData(0.8.Tiles(), 0.6.Tiles(), 50.Percent(), 0.05.Tiles(), 0.06.Tiles(), 10.Degrees(), 2.Degrees(), (Fix32) 2, RelTile1f.Zero, RelTile1f.Zero);
      RotatingCabinDriverProto rotatingCabinDriverProto = new RotatingCabinDriverProto(8.Degrees(), 1.Degrees(), 2.Degrees(), 2.5.ToFix32());
      Option<FuelTankProto> fuelTankProto = registrator.DisableVehicleFuelConsumption ? Option<FuelTankProto>.None : (Option<FuelTankProto>) fuelTank(registrator.FuelTankProtoBuilder);
      RelTile1i treeHarvestDistance = 7.Tiles();
      Duration toPrepareForHarvestDuration = Duration.FromKeyframes(47);
      Duration toTreeLayingDownDuration = Duration.FromKeyframes(54);
      Duration toTreeAboveTruckDuration = Duration.FromKeyframes(36);
      Duration toTreeOnTruckDuration = Duration.FromKeyframes(27);
      Duration toArmUpDuration = Duration.FromKeyframes(16);
      Duration duration1 = 0.75.Seconds();
      Duration duration2 = 0.4.Seconds();
      Duration toFoldedDuration = Duration.FromKeyframes(30);
      Duration cuttingDuration = 0.8.Seconds();
      Duration trimmingDuration = 1.6.Seconds();
      Duration moveToNextSectionDuration = duration1;
      Duration cutNextSectionDuration = duration2;
      TreeHarvesterProto.Timings timings = new TreeHarvesterProto.Timings(toPrepareForHarvestDuration, toTreeLayingDownDuration, toTreeAboveTruckDuration, toTreeOnTruckDuration, toArmUpDuration, toFoldedDuration, cuttingDuration, trimmingDuration, moveToNextSectionDuration, cutNextSectionDuration);
      VehiclePathFindingParams pathFindingParams = new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 30.Percent());
      ImmutableArray<ThicknessTilesF> immutableArray = ((IReadOnlyCollection<double>) new double[2]
      {
        0.0,
        0.25
      }).ToImmutableArray<double, ThicknessTilesF>((Func<double, ThicknessTilesF>) (x => new ThicknessTilesF(x.ToFix32())));
      Duration durationToBuild = 100.Seconds();
      Option<DrivingEntityProto> none = Option<DrivingEntityProto>.None;
      TreeHarvesterProto.Gfx graphics = new TreeHarvesterProto.Gfx(prefabPath, Option<string>.None, RelTile2f.FromMeters((Fix32) 3, 2.5.ToFix32()), RelTile2f.FromMeters((Fix32) 3, 2.5.ToFix32()), ImmutableArray.Create<DynamicEntityDustParticlesSpec>(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.8f, new RelTile3f((Fix32) 0, (Fix32) -1, (Fix32) 0), 55f, 0.1.Tiles(), 1f), new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.8f, new RelTile3f((Fix32) 0, (Fix32) 1, (Fix32) 0), 55f, 0.1.Tiles(), 1f)), Option<VehicleExhaustParticlesSpec>.None, "Assets/Base/Vehicles/ExcavatorT2/Audio/Engine.prefab", "Assets/Base/Vehicles/ExcavatorT2/Audio/Treads.prefab", "TH_body_ROTATE", "TH_pasy_b", "TH_pasy_a", 3.5.Tiles(), 13.62.Meters(), -1.8f, 2.7f, "Idle", "LayArmToTree", "LayDownTree", "TakeUpTree", "PutTreeOnTruck", "TakeTreeFromTruck", "FoldArmFromTruck", "TH_body_ROTATE/TH_hand1/TH_hand2/TH_hand3/ROT/TH_hand4/TreeHolder", "TH_body_ROTATE/TH_hand1/TH_hand2/TH_hand3/ROT/TH_hand4");
      TreeHarvesterProto proto = new TreeHarvesterProto(id, str, entitySize, entityCosts, 1, drivingData, rotatingCabinDriverProto, fuelTankProto, treeHarvestDistance, timings, pathFindingParams, immutableArray, durationToBuild, (Func<TruckProto, bool>) (truck =>
      {
        if (!truck.ProductType.HasValue)
          return true;
        ProductType? productType1 = truck.ProductType;
        ProductType productType2 = CountableProductProto.ProductType;
        return productType1.HasValue && productType1.GetValueOrDefault() == productType2;
      }), none, graphics);
      return prototypesDb.Add<TreeHarvesterProto>(proto);
    }

    public TreeHarvestersData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.treeHarvesterT2Desc = Loc.Str(Ids.Vehicles.TreeHarvesterT2.ToString() + "__desc", "There is nothing that can harvest trees faster than this machine. Make sure you assign plenty of service trucks to it to keep up with the amount of harvested wood.", "");
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
