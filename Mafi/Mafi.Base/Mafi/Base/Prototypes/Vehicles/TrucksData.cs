// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Vehicles.TrucksData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Factory;
using Mafi.Core.Mods;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Vehicles
{
  internal class TrucksData : IModData
  {
    public static readonly Percent DIESEL_POLLUTION_RATIO;
    public static readonly Percent HYDROGEN_DIESEL_ENERGY_RATIO;
    public const int CAPACITY_T1 = 20;
    public const int CAPACITY_T2 = 60;
    public const int CAPACITY_T3 = 180;
    private LocStr2 truckT1Desc;
    private LocStr2 truckT2Desc;

    public void RegisterData(ProtoRegistrator registrator)
    {
      TruckProto truckT2_1 = this.createTruckT2(registrator.TruckProtoBuilder, Ids.Vehicles.TruckT2.Id, Ids.Products.Diesel, Costs.Vehicles.TruckT2, "Assets/Base/Vehicles/ModularTruck/TruckBase.prefab");
      TruckProto truckT2_2 = this.createTruckT2(registrator.TruckProtoBuilder, Ids.Vehicles.TruckT2H.Id, Ids.Products.Hydrogen, Costs.Vehicles.TruckT2H, "Assets/Base/Vehicles/ModularTruck/TruckBaseHydrogen.prefab");
      TruckProto truckT1 = this.createTruckT1(registrator.TruckProtoBuilder, Ids.Vehicles.TruckT1.Id, Ids.Products.Diesel);
      ImmutableArray<TruckProto> trucksT3_1 = this.createTrucksT3(registrator.TruckProtoBuilder, Ids.Vehicles.TruckT3Fluid.Id, Ids.Vehicles.TruckT3Loose.Id, Ids.Products.Diesel, Costs.Vehicles.TruckT3, "Assets/Base/Vehicles/ModularTruckT3/TruckT3Base.prefab", "Assets/Base/Vehicles/ModularTruckT3/TruckT3Dump.prefab", "korba");
      ImmutableArray<TruckProto> trucksT3_2 = this.createTrucksT3(registrator.TruckProtoBuilder, Ids.Vehicles.TruckT3FluidH.Id, Ids.Vehicles.TruckT3LooseH.Id, Ids.Products.Hydrogen, Costs.Vehicles.TruckT3H, "Assets/Base/Vehicles/ModularTruckT3/TruckT3BaseHydrogen.prefab", "Assets/Base/Vehicles/ModularTruckT3/TruckT3DumpHydrogen.prefab", "dump");
      VehicleDepotProto orThrow1 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepot);
      VehicleDepotProto orThrow2 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepotT2);
      VehicleDepotProto orThrow3 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepotT3);
      orThrow1.AddBuildableEntity((DynamicGroundEntityProto) truckT1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) truckT1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) truckT2_1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) truckT2_2);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) truckT1);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) truckT2_1);
      foreach (TruckProto entity in trucksT3_1)
        orThrow3.AddBuildableEntity((DynamicGroundEntityProto) entity);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) truckT2_2);
      foreach (TruckProto entity in trucksT3_2)
        orThrow3.AddBuildableEntity((DynamicGroundEntityProto) entity);
    }

    public static Func<FuelTankProtoBuilder, FuelTankProto> createFuelTank(
      DynamicEntityProto.ID vehicleId,
      Duration reserve,
      ProductProto.ID fuelProductId,
      Quantity tankSize,
      Duration baseDuration)
    {
      if (fuelProductId == Ids.Products.Diesel)
        return (Func<FuelTankProtoBuilder, FuelTankProto>) (tb => tb.Start((Proto.ID) vehicleId).SetReserve(reserve).SetProduct((Proto.ID) fuelProductId, tankSize, baseDuration).SetWasteProduct((Proto.ID) Ids.Products.PollutedAir, TrucksData.DIESEL_POLLUTION_RATIO).BuildTank());
      if (fuelProductId == Ids.Products.Hydrogen)
        return (Func<FuelTankProtoBuilder, FuelTankProto>) (tb => tb.Start((Proto.ID) vehicleId).SetReserve(reserve).SetProduct((Proto.ID) fuelProductId, tankSize.ScaledBy(TrucksData.HYDROGEN_DIESEL_ENERGY_RATIO), baseDuration).BuildTank());
      throw new ProtoBuilderException(string.Format("Unknown fuel '{0}'", (object) fuelProductId));
    }

    private TruckProto createTruckT1(
      TruckProtoBuilder builder,
      DynamicEntityProto.ID vehicleId,
      ProductProto.ID fuelProductId)
    {
      Func<FuelTankProtoBuilder, FuelTankProto> fuelTank = TrucksData.createFuelTank(vehicleId, 2.Minutes(), fuelProductId, new Quantity(5), 11.4.Minutes());
      Option<VehicleExhaustParticlesSpec> particlesSpec;
      if (fuelProductId == Ids.Products.Diesel)
        particlesSpec = (Option<VehicleExhaustParticlesSpec>) new VehicleExhaustParticlesSpec(new string[2]
        {
          "Exhaust_left",
          "Exhaust_right"
        }, 10f, 28f, 80f, 3f, 5f);
      else
        particlesSpec = Option<VehicleExhaustParticlesSpec>.None;
      TruckProtoBuilder.TruckProtoBuilderState protoBuilderState = builder.Start("Pickup", vehicleId);
      string id = vehicleId.ToString() + "_formatted";
      ref LocStr2 local = ref this.truckT1Desc;
      int num = 20;
      string str1 = num.ToString();
      num = 2;
      string str2 = num.ToString();
      string enUs = local.Format(str1, str2).Value;
      LocStr alreadyLocalizedStr = LocalizationManager.CreateAlreadyLocalizedStr(id, enUs);
      return protoBuilderState.Description(alreadyLocalizedStr).SetCosts(Costs.Vehicles.TruckT1).SetDurationToBuild(60.Seconds()).SetCapacity(20).SetDumpingDistance(2.Tiles(), 7.Tiles()).SetSizeInMeters(10.0, 2.5, 2.0).SetWheelDiameter(0.8.Meters()).SetDrivingData(new DrivingData(1.0.Tiles(), 0.6.Tiles(), 50.Percent(), 0.06.Tiles(), 0.09.Tiles(), 60.Degrees(), 20.Degrees(), 2.5.ToFix32(), 1.25.Tiles(), 1.25.Tiles())).SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.CanPassUnder, 100.Percent())).SetDisruptionByDistance((byte) 16, (byte) 8).SetPrefabPath("Assets/Base/Vehicles/ModularTruckSmall/TruckS_Base.prefab").SetTerrainContactPointsOffsets(new RelTile2f(1.75.ToFix32(), 0.75.ToFix32()), new RelTile2f(1.75.ToFix32(), 0.75.ToFix32())).SetSteeringWheelsSubmodelPaths("Truck-low-front-left", "Truck-low-front-right").SetStaticWheelsSubmodelPaths("Truck-low-back-left", "Truck-low-back-right").SetDumpedThicknessByDistanceMeters(1f, 0.8f, 0.5f).SetFuelTank(fuelTank).AddDustSource(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.4f, new RelTile3f((Fix32) -1, (Fix32) 0, (Fix32) 0), 50f, 0.3.Tiles())).AddExhaustSources(particlesSpec).SetEngineSound("Assets/Base/Vehicles/ModularTruckSmall/Audio/Engine.prefab").AddAttachment((AttachmentProto) new TankAttachmentProto(new Proto.ID(vehicleId.ToString() + "_AttachmentTank"), (Predicate<ProductProto>) (x => x is FluidProductProto), new TankAttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruckSmall/T1-tank.prefab", "icons", "T1-tank", ColorRgba.Gray, ColorRgba.DarkGray), false)).AddAttachment((AttachmentProto) new FlatBedAttachmentProto(new Proto.ID(vehicleId.ToString() + "_AttachmentFlatBed"), (Predicate<ProductProto>) (x => x is CountableProductProto), new FlatBedAttachmentProto.Gfx(1, "Assets/Base/Vehicles/ModularTruckSmall/TruckS_Flat.prefab"), true)).AddAttachment((AttachmentProto) new DumpAttachmentProto(new Proto.ID(vehicleId.ToString() + "_AttachmentDump"), new DumpAttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruckSmall/TruckS_Dump.prefab", "Box170/PileSmooth", "Box170/PileRough", new LoosePileTextureParams(0.8f), new Vector3f((Fix32) 2, 0.05.ToFix32(), (Fix32) 0), new Vector3f((Fix32) 2, 1.2.ToFix32(), (Fix32) 0)))).SetNextTier(builder.ProtosDb.GetOrThrow<DrivingEntityProto>((Proto.ID) Ids.Vehicles.TruckT2.Id)).BuildAndAdd();
    }

    private TruckProto createTruckT2(
      TruckProtoBuilder builder,
      DynamicEntityProto.ID vehicleId,
      ProductProto.ID fuelProductId,
      EntityCostsTpl costs,
      string prefabPath)
    {
      Func<FuelTankProtoBuilder, FuelTankProto> fuelTank = TrucksData.createFuelTank(vehicleId, 2.Minutes(), fuelProductId, new Quantity(15), 14.Minutes());
      Option<VehicleExhaustParticlesSpec> particlesSpec;
      if (fuelProductId == Ids.Products.Diesel)
        particlesSpec = (Option<VehicleExhaustParticlesSpec>) new VehicleExhaustParticlesSpec(new string[2]
        {
          "Exhaust_left",
          "Exhaust_right"
        }, 12f, 32f, 100f, 3f, 5f);
      else
        particlesSpec = Option<VehicleExhaustParticlesSpec>.None;
      TruckProtoBuilder.TruckProtoBuilderState protoBuilderState = builder.Start("Truck", vehicleId);
      string id = vehicleId.ToString() + "_formatted";
      ref LocStr2 local = ref this.truckT2Desc;
      int num = 60;
      string str1 = num.ToString();
      num = 2;
      string str2 = num.ToString();
      string enUs = local.Format(str1, str2).Value;
      LocStr alreadyLocalizedStr = LocalizationManager.CreateAlreadyLocalizedStr(id, enUs);
      return protoBuilderState.Description(alreadyLocalizedStr).SetCosts(costs).SetDurationToBuild(120.Seconds()).SetCapacity(60).SetDumpingDistance(3.Tiles(), 8.Tiles()).SetSizeInMeters(10.0, 2.5, 2.0).SetWheelDiameter(1.2.Meters()).SetDrivingData(new DrivingData(1.1.Tiles(), 0.6.Tiles(), 50.Percent(), 0.06.Tiles(), 0.09.Tiles(), 60.Degrees(), 20.Degrees(), 2.5.ToFix32(), 1.5.Tiles(), 1.5.Tiles())).SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.CanPassUnder, 100.Percent())).SetDisruptionByDistance((byte) 22, (byte) 10).SetPrefabPath(prefabPath).SetTerrainContactPointsOffsets(new RelTile2f((Fix32) 2, 0.75.ToFix32()), new RelTile2f((Fix32) 2, 0.75.ToFix32())).SetSteeringWheelsSubmodelPaths("wheel_front_left", "wheel_front_right").SetStaticWheelsSubmodelPaths("wheel_middle_left", "wheel_middle_right", "wheel_back1_left", "wheel_back1_right", "wheel_back2_left", "wheel_back2_right").SetDumpedThicknessByDistanceMeters(1.5f, 1.2f, 0.6f, 0.4f).SetFuelTank(fuelTank).AddDustSource(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.8f, new RelTile3f((Fix32) 0, (Fix32) 0, (Fix32) 0), 50f, 0.3.Tiles())).AddExhaustSources(particlesSpec).SetEngineSound("Assets/Base/Vehicles/ModularTruck/Audio/Engine.prefab").AddAttachment((AttachmentProto) new TankAttachmentProto(new Proto.ID(vehicleId.ToString() + "_AttachmentTank"), (Predicate<ProductProto>) (x => x is FluidProductProto), new TankAttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruck/T2-tank.prefab", "icons", "T2-tank", ColorRgba.Gray, ColorRgba.DarkGray), false)).AddAttachment((AttachmentProto) new FlatBedAttachmentProto(new Proto.ID(vehicleId.ToString() + "_AttachmentFlatBed"), (Predicate<ProductProto>) (x => x is CountableProductProto), new FlatBedAttachmentProto.Gfx(2, "Assets/Base/Vehicles/ModularTruck/Truck_Flat.prefab"), true)).AddAttachment((AttachmentProto) new DumpAttachmentProto(new Proto.ID(vehicleId.ToString() + "_AttachmentDump"), new DumpAttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruck/Truck_Dump.prefab", "Object010/PileSmooth", "Object010/PileRough", LoosePileTextureParams.Default, new Vector3f(2.6.ToFix32(), 0.2.ToFix32(), (Fix32) 0), new Vector3f(2.6.ToFix32(), 1.9.ToFix32(), (Fix32) 0)))).BuildAndAdd();
    }

    private ImmutableArray<TruckProto> createTrucksT3(
      TruckProtoBuilder builder,
      DynamicEntityProto.ID fluidVehicleId,
      DynamicEntityProto.ID looseVehicleId,
      ProductProto.ID fuelProductId,
      EntityCostsTpl costs,
      string prefabPath,
      string dumpPrefabPath,
      string pileRootName)
    {
      Option<VehicleExhaustParticlesSpec> exhaustParticleSpec;
      if (fuelProductId == Ids.Products.Diesel)
        exhaustParticleSpec = (Option<VehicleExhaustParticlesSpec>) new VehicleExhaustParticlesSpec(new string[1]
        {
          "Exhaust"
        }, 18f, 50f, 150f, 4f, 6f);
      else
        exhaustParticleSpec = Option<VehicleExhaustParticlesSpec>.None;
      LocStr1 desc1 = Loc.Str1(looseVehicleId.ToString() + "__desc", "Large hauling truck with max capacity of {0}. This type can transport only loose products (coal for instance). It cannot go under transports.", "vehicle description, for instance {0}=150");
      TruckProto truckProto1 = setupTruckBuilder(looseVehicleId, "Haul truck (dump)", desc1).SetFixedProductType(LooseProductProto.ProductType).AddAttachment((AttachmentProto) new DumpAttachmentProto(new Proto.ID(looseVehicleId.ToString() + "_AttachmentDump"), new DumpAttachmentProto.Gfx(dumpPrefabPath, pileRootName + "/PileSmooth", pileRootName + "/PileSmooth", LoosePileTextureParams.Default, "Main"))).BuildAndAdd();
      LocStr1 desc2 = Loc.Str1(fluidVehicleId.ToString() + "__desc", "Large hauling truck with max capacity of {0}. This type can transport only liquid or gas products. It cannot go under transports.", "vehicle description, for instance {0}=150");
      TruckProto truckProto2 = setupTruckBuilder(fluidVehicleId, "Haul truck (tank)", desc2).SetFixedProductType(FluidProductProto.ProductType).AddAttachment((AttachmentProto) new TankAttachmentProto(new Proto.ID(fluidVehicleId.ToString() + "_AttachmentTank"), (Predicate<ProductProto>) (x => x is FluidProductProto), new TankAttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruckT3/T3-tank.prefab", "icons", "T3-tank", new ColorRgba(0.63f, 0.51f, 0.24f), ColorRgba.DarkDarkGray), true)).BuildAndAdd();
      return ImmutableArray.Create<TruckProto>(truckProto1, truckProto2);

      TruckProtoBuilder.TruckProtoBuilderState setupTruckBuilder(
        DynamicEntityProto.ID id,
        string title,
        LocStr1 desc)
      {
        Func<FuelTankProtoBuilder, FuelTankProto> fuelTank = TrucksData.createFuelTank(id, 2.Minutes(), fuelProductId, new Quantity(36), 16.Minutes());
        return builder.Start(title, id).Description(LocalizationManager.CreateAlreadyLocalizedStr(id.ToString() + "_formatted", desc.Format(180.ToString()).Value)).SetCosts(costs).SetDurationToBuild(240.Seconds()).SetCapacity(180).SetDumpingDistance(4.Tiles(), 9.Tiles()).SetSizeInMeters(12.0, 7.0, 6.0).SetWheelDiameter(3.9.Meters()).SetDrivingData(new DrivingData(0.7.Tiles(), 0.5.Tiles(), 50.Percent(), 0.02.Tiles(), 0.06.Tiles(), 60.Degrees(), 15.Degrees(), 3.0.ToFix32(), 1.5.Tiles(), 1.5.Tiles())).SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(5), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 100.Percent())).SetDisruptionByDistance((byte) 0, (byte) 32, (byte) 32).SetPrefabPath(prefabPath).SetTerrainContactPointsOffsets(new RelTile2f(1.5.ToFix32(), 1.5.ToFix32()), new RelTile2f(1.5.ToFix32(), 1.5.ToFix32())).SetSteeringWheelsSubmodelPaths("wheel_front_left", "wheel_front_right").SetStaticWheelsSubmodelPaths("wheel_back").SetDumpedThicknessByDistanceMeters(1.5f, 1.5f, 1.2f, 0.8f).SetFuelTank(fuelTank).AddDustSource(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f((Fix32) 0, -1.5.ToFix32(), (Fix32) 0), 50f, 0.2.Tiles())).AddDustSource(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f((Fix32) 0, 1.5.ToFix32(), (Fix32) 0), 50f, 0.2.Tiles())).AddExhaustSources(exhaustParticleSpec).SetEngineSound("Assets/Base/Vehicles/ModularTruckT3/Audio/Engine.prefab");
      }
    }

    public TrucksData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.truckT1Desc = Loc.Str2(Ids.Vehicles.TruckT1.Id.ToString() + "__desc", "Heavy duty pickup truck with max capacity of {0}. It can go under transports that are at height {1} or higher.", "truck description, for instance {0}=20,{1}=2");
      this.truckT2Desc = Loc.Str2(Ids.Vehicles.TruckT2.Id.ToString() + "__desc", "Large industrial truck with max capacity of {0}. It can go under transports if they are at height {1} or higher.", "vehicle description, for instance {0}=20,{1}=2");
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TrucksData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TrucksData.DIESEL_POLLUTION_RATIO = 100.Percent();
      TrucksData.HYDROGEN_DIESEL_ENERGY_RATIO = 120.Percent();
    }
  }
}
