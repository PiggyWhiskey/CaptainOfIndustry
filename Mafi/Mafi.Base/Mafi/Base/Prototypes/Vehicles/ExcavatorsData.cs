// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Vehicles.ExcavatorsData
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
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Vehicles
{
  internal class ExcavatorsData : IModData
  {
    private LocStr1 excavatorT2Desc;
    private LocStr1 excavatorT3Desc;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ExcavatorProto excavatorT3_1 = this.createExcavatorT3(registrator.ExcavatorProtoBuilder, Ids.Vehicles.ExcavatorT3, Ids.Products.Diesel, Costs.Vehicles.ExcavatorT3, "Assets/Base/Vehicles/ExcavatorT3.prefab", "rotate_base/base/ROtate_rameno/_bone-base/Dummy002/_bone-ramenoA/_bone-ramenoB/_ramenoB/_radlice", "");
      ExcavatorProto excavatorT3_2 = this.createExcavatorT3(registrator.ExcavatorProtoBuilder, Ids.Vehicles.ExcavatorT3H, Ids.Products.Hydrogen, Costs.Vehicles.ExcavatorT3H, "Assets/Base/Vehicles/ExcavatorT3Hydrogen.prefab", "rotate_base/base/_bone-base/Dummy002/_bone-ramenoA/_bone-ramenoB/_ramenoB/_radlice", "move/");
      ExcavatorProto excavatorT2_1 = this.createExcavatorT2(registrator.ExcavatorProtoBuilder, Ids.Vehicles.ExcavatorT2, Ids.Products.Diesel, Costs.Vehicles.ExcavatorT2, (Proto.ID) Ids.Vehicles.ExcavatorT3, "Assets/Base/Vehicles/ExcavatorT2.prefab");
      ExcavatorProto excavatorT2_2 = this.createExcavatorT2(registrator.ExcavatorProtoBuilder, Ids.Vehicles.ExcavatorT2H, Ids.Products.Hydrogen, Costs.Vehicles.ExcavatorT2H, (Proto.ID) Ids.Vehicles.ExcavatorT3H, "Assets/Base/Vehicles/ExcavatorT2Hydrogen.prefab");
      ExcavatorProto excavatorT1 = this.createExcavatorT1(registrator.ExcavatorProtoBuilder);
      VehicleDepotProto orThrow1 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepot);
      VehicleDepotProto orThrow2 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepotT2);
      VehicleDepotProto orThrow3 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepotT3);
      orThrow1.AddBuildableEntity((DynamicGroundEntityProto) excavatorT1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) excavatorT1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) excavatorT2_1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) excavatorT2_2);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) excavatorT1);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) excavatorT2_1);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) excavatorT2_2);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) excavatorT3_1);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) excavatorT3_2);
    }

    private ExcavatorProto createExcavatorT1(ExcavatorProtoBuilder builder)
    {
      LocStr1 locStr1 = Loc.Str1(Ids.Vehicles.ExcavatorT1.ToString() + "__desc", "Suitable for mining any terrain with max bucket capacity of {0}. It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=6");
      return builder.Start("Small excavator", Ids.Vehicles.ExcavatorT1).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Vehicles.ExcavatorT1.ToString() + "_formatted", locStr1.Format(6.ToString()).Value)).SetCosts(Costs.Vehicles.ExcavatorT1).SetDurationToBuild(100.Seconds()).SetCapacity(6).SetSizeInMeters(9.0, 4.5, 3.5).SetMaxMiningDistance(new RelTile1i(2), new RelTile1i(5)).SetMinedThicknessByDistanceMeters(1.5f, 1f, 0.5f).SetDrivingData(new DrivingData(0.6.Tiles(), 0.4.Tiles(), 50.Percent(), 0.04.Tiles(), 0.06.Tiles(), 10.Degrees(), 2.Degrees(), (Fix32) 2, RelTile1f.Zero, RelTile1f.Zero)).SetCabinDriver(new RotatingCabinDriverProto(10.Degrees(), 1.5.Degrees(), 2.Degrees(), 2.5.ToFix32())).SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 40.Percent())).SetDisruptionByDistance((byte) 0, (byte) 48).SetTerrainContactPointsOffsets(new RelTile2f(1.075.ToFix32(), (Fix32) 1), new RelTile2f(1.075.ToFix32(), (Fix32) 1)).SetAnimationTimings(Duration.FromKeyframes(33), Duration.FromKeyframes(69), 3, Duration.FromKeyframes(15), Duration.FromKeyframes(60), Duration.FromKeyframes(73), Duration.FromKeyframes(23)).SetFuelTank((Func<FuelTankProtoBuilder, FuelTankProto>) (tb => tb.Start((Proto.ID) Ids.Vehicles.ExcavatorT1).SetReserve(2.Minutes()).SetProduct((Proto.ID) Ids.Products.Diesel, new Quantity(10), 10.Minutes()).SetWasteProduct((Proto.ID) Ids.Products.PollutedAir, TrucksData.DIESEL_POLLUTION_RATIO).BuildTank())).SetTracksParameters(2.6.Meters(), 3.3.Meters()).SetPrefabPath("Assets/Base/Vehicles/ExcavatorT1.prefab").AddDustSource(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.8f, new RelTile3f((Fix32) 0, (Fix32) 0, (Fix32) 0), 60f, 0.1.Tiles(), 0.5f)).SetEngineSound("Assets/Base/Vehicles/ExcavatorT1/Audio/Engine.prefab").SetMovementSound("Assets/Base/Vehicles/ExcavatorT1/Audio/Treads.prefab").SetDigSounds(ImmutableArray.Create<string>("Assets/Base/Vehicles/ExcavatorT1/Audio/Dig1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dig2.prefab")).SetDumpSounds(ImmutableArray.Create<string>("Assets/Base/Vehicles/ExcavatorT1/Audio/Dump1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump2.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump3.prefab")).SetSubmodelNames("excavator-T1_body", "Base/TrackLeft", "Base/TrackRight", "excavator-T1_body/excavator-T1_rameno/Dummy_radlice/bagr_radlice", "SmoothHalf", "SmoothFull", "RoughHalf", "RoughFull").SetAnimationNames("Idle", "PrepareMine", "Mine", "PrepareDump", "Dump").SetIsTruckSupportedFunc((Func<TruckProto, bool>) (truckProto => (!truckProto.ProductType.HasValue || truckProto.ProductType.Value == LooseProductProto.ProductType) && truckProto.Id != Ids.Vehicles.TruckT3Loose.Id && truckProto.Id != Ids.Vehicles.TruckT3LooseH.Id)).SetNextTier(builder.ProtosDb.GetOrThrow<DrivingEntityProto>((Proto.ID) Ids.Vehicles.ExcavatorT2)).BuildAndAdd();
    }

    private ExcavatorProto createExcavatorT2(
      ExcavatorProtoBuilder builder,
      DynamicEntityProto.ID vehicleId,
      ProductProto.ID fuelProductId,
      EntityCostsTpl costs,
      Proto.ID nextTier,
      string prefabPath)
    {
      Func<FuelTankProtoBuilder, FuelTankProto> fuelTank = TrucksData.createFuelTank(vehicleId, 2.Minutes(), fuelProductId, new Quantity(27), 12.Minutes());
      return builder.Start("Excavator", vehicleId).Description(LocalizationManager.CreateAlreadyLocalizedStr(vehicleId.ToString() + "_formatted", this.excavatorT2Desc.Format(20.ToString()).Value)).SetCosts(costs).SetDurationToBuild(200.Seconds()).SetCapacity(20).SetSizeInMeters(10.5, 5.5, 5.5).SetMaxMiningDistance(new RelTile1i(3), new RelTile1i(7)).SetMinedThicknessByDistanceMeters(2f, 1.5f, 1f, 0.5f).SetDrivingData(new DrivingData(0.4.Tiles(), 0.4.Tiles(), 50.Percent(), 0.025.Tiles(), 0.05.Tiles(), 8.Degrees(), 1.Degrees(), 2.5.ToFix32(), RelTile1f.Zero, RelTile1f.Zero)).SetCabinDriver(new RotatingCabinDriverProto(8.Degrees(), 1.Degrees(), 2.Degrees(), 2.5.ToFix32())).SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 30.Percent())).SetDisruptionByDistance((byte) 0, (byte) 64).SetTerrainContactPointsOffsets(new RelTile2f(1.5.ToFix32(), 1.25.ToFix32()), new RelTile2f(1.5.ToFix32(), 1.25.ToFix32())).SetAnimationTimings(Duration.FromKeyframes(30), Duration.FromKeyframes(70), 4, Duration.FromKeyframes(11), Duration.FromKeyframes(57), Duration.FromKeyframes(73), Duration.FromKeyframes(23)).SetFuelTank(fuelTank).SetTracksParameters(3.6.Meters(), 3.55.Meters()).SetPrefabPath(prefabPath).AddDustSource(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2f, new RelTile3f((Fix32) 0, (Fix32) -1, (Fix32) 0), 55f, 0.1.Tiles(), 1f)).AddDustSource(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2f, new RelTile3f((Fix32) 0, (Fix32) 1, (Fix32) 0), 55f, 0.1.Tiles(), 1f)).SetEngineSound("Assets/Base/Vehicles/ExcavatorT2/Audio/Engine.prefab").SetMovementSound("Assets/Base/Vehicles/ExcavatorT2/Audio/Treads.prefab").SetDigSounds(ImmutableArray.Create<string>("Assets/Base/Vehicles/ExcavatorT1/Audio/Dig1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dig2.prefab")).SetDumpSounds(ImmutableArray.Create<string>("Assets/Base/Vehicles/ExcavatorT1/Audio/Dump1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump2.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump3.prefab")).SetSubmodelNames("excavator-T2_body", "TrackLeft", "TrackRight", "excavator-T2_body/excavator-T1_rameno/Dummy_radlice005/ShovelPileParent", "SmoothHalf", "SmoothFull", "RoughHalf", "RoughFull").SetAnimationNames("Idle", "PrepareMine", "Mine", "PrepareDump", "Dump").SetIsTruckSupportedFunc((Func<TruckProto, bool>) (truckProto => (!truckProto.ProductType.HasValue || truckProto.ProductType.Value == LooseProductProto.ProductType) && truckProto.Id != Ids.Vehicles.TruckT3Loose.Id && truckProto.Id != Ids.Vehicles.TruckT3LooseH.Id)).SetNextTier(builder.ProtosDb.GetOrThrow<DrivingEntityProto>(nextTier)).BuildAndAdd();
    }

    private ExcavatorProto createExcavatorT3(
      ExcavatorProtoBuilder builder,
      DynamicEntityProto.ID vehicleId,
      ProductProto.ID fuelProductId,
      EntityCostsTpl costs,
      string prefabPath,
      string pathToPilesParent,
      string tracksPrefix)
    {
      Func<FuelTankProtoBuilder, FuelTankProto> fuelTank = TrucksData.createFuelTank(vehicleId, 3.Minutes(), fuelProductId, new Quantity(70), 14.Minutes());
      return builder.Start("Mega excavator", vehicleId).Description(LocalizationManager.CreateAlreadyLocalizedStr(vehicleId.ToString() + "_formatted", this.excavatorT3Desc.Format(60.ToString()).Value)).SetCosts(costs).SetDurationToBuild(300.Seconds()).SetCapacity(60).SetSizeInMeters(13.0, 8.0, 8.0).SetMaxMiningDistance(new RelTile1i(4), new RelTile1i(9)).SetMinedThicknessByDistanceMeters(2f, 2f, 1.5f, 1f, 0.5f).SetDrivingData(new DrivingData(0.25.Tiles(), 0.2.Tiles(), 40.Percent(), 0.015.Tiles(), 0.03.Tiles(), 4.0.Degrees(), 0.4.Degrees(), 3.ToFix32(), RelTile1f.Zero, RelTile1f.Zero)).SetCabinDriver(new RotatingCabinDriverProto(6.Degrees(), 0.8.Degrees(), 1.5.Degrees(), 3.0.ToFix32())).SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(5), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 20.Percent())).SetDisruptionByDistance((byte) 0, (byte) 0, (byte) 64).SetTerrainContactPointsOffsets(new RelTile2f(1.75.ToFix32(), (Fix32) 2), new RelTile2f(1.75.ToFix32(), (Fix32) 2)).SetAnimationTimings(Duration.FromKeyframes(30), Duration.FromKeyframes(85), 5, Duration.FromKeyframes(11), Duration.FromKeyframes(50), Duration.FromKeyframes(120), Duration.FromKeyframes(15)).SetFuelTank(fuelTank).SetTracksParameters(7.0.Meters(), 3.04.Meters()).SetPrefabPath(prefabPath).AddDustSource(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f((Fix32) 0, -1.5.ToFix32(), (Fix32) 0), 60f, 0.05.Tiles(), 3f)).AddDustSource(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f((Fix32) 0, 1.5.ToFix32(), (Fix32) 0), 60f, 0.05.Tiles(), 2f)).SetEngineSound("Assets/Base/Vehicles/ExcavatorT3/Audio/Engine.prefab").SetMovementSound("Assets/Base/Vehicles/ExcavatorT3/Audio/Treads.prefab").SetDigSounds(ImmutableArray.Create<string>("Assets/Base/Vehicles/ExcavatorT1/Audio/Dig1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dig2.prefab")).SetDumpSounds(ImmutableArray.Create<string>("Assets/Base/Vehicles/ExcavatorT1/Audio/Dump1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump2.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump3.prefab")).SetSubmodelNames("rotate_base", tracksPrefix + "TrackLeft", tracksPrefix + "TrackRight", pathToPilesParent, "SmoothHalf", "SmoothFull", "RoughHalf", "RoughFull").SetAnimationNames("Idle", "PrepareMine", "Mine", "PrepareDump", "Dump").SetIsTruckSupportedFunc((Func<TruckProto, bool>) (truckProto => (!truckProto.ProductType.HasValue || truckProto.ProductType.Value == LooseProductProto.ProductType) && truckProto.Id != Ids.Vehicles.TruckT1.Id)).BuildAndAdd();
    }

    public ExcavatorsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.excavatorT2Desc = Loc.Str1(Ids.Vehicles.ExcavatorT2.ToString() + "__desc", "This is a serious mining machine with max bucket capacity of {0}! It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=18");
      this.excavatorT3Desc = Loc.Str1(Ids.Vehicles.ExcavatorT3.ToString() + "__desc", "Extremely large excavator that can mine any terrain with ease. It has bucket capacity of {0}. It cannot go under transports due to its size, use ramps to cross them.", "vehicle description, for instance {0}=60");
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
