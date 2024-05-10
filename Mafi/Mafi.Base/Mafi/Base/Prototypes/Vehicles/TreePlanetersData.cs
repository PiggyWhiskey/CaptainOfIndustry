// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Vehicles.TreePlanetersData
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
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Vehicles
{
  public class TreePlanetersData : IModData
  {
    private LocStr treePlanterDesc;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProductProto orThrow1 = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.TreeSapling);
      TreePlanterProto treePlanterT1_1 = this.createTreePlanterT1(registrator, orThrow1, Ids.Vehicles.TreePlanterT1, Ids.Products.Diesel, Costs.Vehicles.TreePlanterT1, "Assets/Base/Vehicles/TreePlanter.prefab");
      TreePlanterProto treePlanterT1_2 = this.createTreePlanterT1(registrator, orThrow1, Ids.Vehicles.TreePlanterT1H, Ids.Products.Hydrogen, Costs.Vehicles.TreePlanterT1H, "Assets/Base/Vehicles/TreePlanterHydrogen.prefab");
      VehicleDepotProto orThrow2 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepot);
      VehicleDepotProto orThrow3 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepotT2);
      VehicleDepotProto orThrow4 = registrator.PrototypesDb.GetOrThrow<VehicleDepotProto>((Proto.ID) Ids.Buildings.VehiclesDepotT3);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) treePlanterT1_1);
      orThrow2.AddBuildableEntity((DynamicGroundEntityProto) treePlanterT1_2);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) treePlanterT1_1);
      orThrow3.AddBuildableEntity((DynamicGroundEntityProto) treePlanterT1_2);
      orThrow4.AddBuildableEntity((DynamicGroundEntityProto) treePlanterT1_1);
      orThrow4.AddBuildableEntity((DynamicGroundEntityProto) treePlanterT1_2);
    }

    private TreePlanterProto createTreePlanterT1(
      ProtoRegistrator registrator,
      ProductProto saplingProto,
      DynamicEntityProto.ID vehicleId,
      ProductProto.ID fuelProductId,
      EntityCostsTpl costs,
      string prefabPath)
    {
      Func<FuelTankProtoBuilder, FuelTankProto> fuelTank = TrucksData.createFuelTank(vehicleId, 3.Minutes(), fuelProductId, new Quantity(12), 15.Minutes());
      return registrator.PrototypesDb.Add<TreePlanterProto>(new TreePlanterProto(vehicleId, Proto.CreateStr((Proto.ID) vehicleId, "Tree planter", this.treePlanterDesc), RelTile3f.FromDimensionsInMeters(6.0, 4.0, 5.0), costs.MapToEntityCosts(registrator), 1, new DrivingData(0.6.Tiles(), 0.4.Tiles(), 50.Percent(), 0.04.Tiles(), 0.06.Tiles(), 8.Degrees(), 1.5.Degrees(), (Fix32) 2, RelTile1f.Zero, RelTile1f.Zero), new RotatingCabinDriverProto(7.Degrees(), 0.8.Degrees(), 2.Degrees(), 2.5.ToFix32()), registrator.DisableVehicleFuelConsumption ? Option<FuelTankProto>.None : (Option<FuelTankProto>) fuelTank(registrator.FuelTankProtoBuilder), 5.Tiles(), new TreePlanterProto.Timings(Duration.FromKeyframes(440, 1.25f), Duration.FromKeyframes(50, 1.25f)), 3.Seconds(), saplingProto, new Quantity(8), new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 30.Percent()), ((IReadOnlyCollection<double>) new double[2]
      {
        0.0,
        0.2
      }).ToImmutableArray<double, ThicknessTilesF>((Func<double, ThicknessTilesF>) (x => new ThicknessTilesF(x.ToFix32()))), 80.Seconds(), Option<DrivingEntityProto>.None, new TreePlanterProto.Gfx(prefabPath, Option<string>.None, RelTile2f.FromMeters((Fix32) 3, (Fix32) 3), RelTile2f.FromMeters((Fix32) 3, (Fix32) 3), ImmutableArray.Create<DynamicEntityDustParticlesSpec>(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.5f, new RelTile3f((Fix32) 0, (Fix32) 0, (Fix32) 0), 50f, 0.1.Tiles(), 0.5f)), Option<VehicleExhaustParticlesSpec>.None, "Assets/Base/Vehicles/ExcavatorT1/Audio/Engine.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Treads.prefab", "base", "TrackLeft", "TrackRight", "base/hand-pivot/Bone001/Bone002/TH_hand2/TH-hand3/seeder/trees-", 8, 3.2.Tiles(), 2.5.Meters(), "Idle", "PlantingMachine")));
    }

    public TreePlanetersData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.treePlanterDesc = Loc.Str(Ids.Vehicles.TreePlanterT1.ToString() + "__desc", "A machine for planting trees. Make sure it has access to saplings.  It cannot pass under transports.", "");
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
