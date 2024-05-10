// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Rockets.RocketsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Mods;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Rockets
{
  internal class RocketsData : IModData
  {
    public static readonly Quantity SMALL_ROCKET_CAPACITY;
    public static readonly Quantity LARGE_ROCKET_CAPACITY;
    private static readonly DrivingData TRANSPORTER_DRIVING_DATA;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb db = registrator.PrototypesDb;
      ProductProto orThrow = db.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Hydrogen);
      RocketTransporterProto transporterFor = createTransporterFor(db.Add<RocketProto>(new RocketProto(Ids.Rockets.TestingRocketT0, Proto.CreateStrFormatDesc1((Proto.ID) Ids.Rockets.TestingRocketT0, "Testing rocket", "Testing rocket that demonstrates capability of reaching space! It uses {0} as fuel.", (LocStrFormatted) orThrow.Strings.Name, "rocket description, {0} is fuel like 'hydrogen'"), Costs.Rockets.TestingRocketT0.MapToEntityCosts(registrator), 8.Minutes(), Quantity.Zero, 80.Of(orThrow), 100, (Fix32) 100, (Fix32) 100, 0.015.Tiles(), 30.Seconds(), 11.5.Tiles(), new RocketProto.Gfx("Assets/Base/Vehicles/RocketT2.prefab", "Assets/Base/Vehicles/Rockets/RocketSound.prefab"))));
      db.GetOrThrow<RocketAssemblyBuildingProto>((Proto.ID) Ids.Buildings.RocketAssemblyDepot).AddBuildableEntity((DynamicGroundEntityProto) transporterFor);

      RocketTransporterProto createTransporterFor(RocketProto rocketProto)
      {
        return db.Add<RocketTransporterProto>(new RocketTransporterProto(Ids.Rockets.GetRocketTransporterId(rocketProto.Id), (TransportedRocketBaseProto) rocketProto, RelTile3f.FromDimensionsInMeters(7.0, 7.0, 2.0), RocketsData.TRANSPORTER_DRIVING_DATA, new VehiclePathFindingParams(new RelTile1i(5), SteepnessPathability.NoSlopeAllowed, HeightClearancePathability.NoPassingUnder, 0.Percent()), ((IReadOnlyCollection<double>) new double[3]
        {
          0.0,
          0.0,
          0.35
        }).ToImmutableArray<double, ThicknessTilesF>((Func<double, ThicknessTilesF>) (x => new ThicknessTilesF(x.ToFix32()))), Duration.FromKeyframes(300), Option<DrivingEntityProto>.None, new RocketTransporterProto.Gfx("Assets/Base/Vehicles/RocketTransporter.prefab", Option<string>.None, new RelTile2f((Fix32) 3, (Fix32) 3), new RelTile2f((Fix32) 3, (Fix32) 3), ImmutableArray.Create<DynamicEntityDustParticlesSpec>(new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 3f, new RelTile3f((Fix32) -2, (Fix32) -2, (Fix32) 0), 120f, 0.02.Tiles()), new DynamicEntityDustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 3f, new RelTile3f((Fix32) -2, (Fix32) 2, (Fix32) 0), 120f, 0.02.Tiles())), Option<VehicleExhaustParticlesSpec>.None, "EMPTY", "Assets/Base/Vehicles/RocketTransporter/Audio/EngineAndTreads.prefab", "LeftTrack", "RightTrack", 7.2.Tiles(), 2.35.Meters())));
      }
    }

    public RocketsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static RocketsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      RocketsData.SMALL_ROCKET_CAPACITY = 100.Quantity();
      RocketsData.LARGE_ROCKET_CAPACITY = 200.Quantity();
      RocketsData.TRANSPORTER_DRIVING_DATA = new DrivingData(0.1.Tiles(), 0.08.Tiles(), 20.Percent(), 0.004.Tiles(), 0.02.Tiles(), 2.Degrees(), 0.4.Degrees(), (Fix32) 4, RelTile1f.Zero, RelTile1f.Zero);
    }
  }
}
