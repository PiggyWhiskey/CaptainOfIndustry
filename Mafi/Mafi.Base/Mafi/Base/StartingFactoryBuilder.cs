// Decompiled with JetBrains decompiler
// Type: Mafi.Base.StartingFactoryBuilder
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.GameLoop;
using Mafi.Core.Map;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Core.World;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base
{
  [DependencyRegisteredManually("")]
  public class StartingFactoryBuilder : INeedsSimUpdatesForInit
  {
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly IStartingFactoryConfig m_config;
    private readonly IPopulationConfig m_populationConfig;
    private readonly TutorialsConfig m_tutorialsConfig;
    private readonly TerrainManager m_terrainManager;
    private readonly EntitiesBuilder m_entitiesBuilder;
    private readonly ProtosDb m_protosDb;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly ProductsManager m_productsManager;
    private readonly UpointsManager m_upointsManager;
    private readonly SettlementModulesSlotBasedValidator m_settlementModulesSlotValidator;
    private readonly SettlementsManager m_settlementsManager;
    private readonly IStartLocationProvider m_startLocationProvider;
    private readonly VehiclesManager m_vehiclesManager;
    private Tile2i m_basePosition;
    private Rotation90 m_baseRotation;
    private readonly StartingFactoryPlacer m_staringFactoryPlacer;

    public bool FailedInit { get; private set; }

    public string FailedInitMessage { get; private set; }

    public StartingFactoryBuilder(
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      IStartingFactoryConfig config,
      IPopulationConfig populationConfig,
      TutorialsConfig tutorialsConfig,
      TerrainManager terrainManager,
      EntitiesBuilder entitiesBuilder,
      ProtosDb protosDb,
      TravelingFleetManager fleetManager,
      ProductsManager productsManager,
      UpointsManager upointsManager,
      SettlementModulesSlotBasedValidator settlementModulesSlotValidator,
      SettlementsManager settlementsManager,
      IStartLocationProvider startLocationProvider,
      StartingFactoryPlacer startingFactoryPlacer,
      VehiclesManager vehiclesManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: reference to a compiler-generated field
      this.\u003CNeedsMoreSimUpdates\u003Ek__BackingField = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      StartingFactoryBuilder owner = this;
      this.m_simLoopEvents = simLoopEvents;
      this.m_config = config;
      this.m_populationConfig = populationConfig;
      this.m_tutorialsConfig = tutorialsConfig;
      this.m_terrainManager = terrainManager;
      this.m_entitiesBuilder = entitiesBuilder;
      this.m_protosDb = protosDb;
      this.m_fleetManager = fleetManager;
      this.m_productsManager = productsManager;
      this.m_upointsManager = upointsManager;
      this.m_settlementModulesSlotValidator = settlementModulesSlotValidator;
      this.m_settlementsManager = settlementsManager;
      this.m_startLocationProvider = startLocationProvider;
      this.m_staringFactoryPlacer = startingFactoryPlacer;
      this.m_vehiclesManager = vehiclesManager;
      gameLoopEvents.RegisterNewGameCreated((object) this, (Action) (() => simLoopEvents.Update.AddNonSaveable<StartingFactoryBuilder>(owner, new Action(owner.simUpdate))));
      gameLoopEvents.RegisterInitSimUpdate((INeedsSimUpdatesForInit) this);
    }

    public bool NeedsMoreSimUpdates { get; private set; }

    private void markFailed(string message)
    {
      this.FailedInit = true;
      this.FailedInitMessage = message + " This is likely due to a problem with the chosen starting location on this map.";
    }

    private void simUpdate()
    {
      this.NeedsMoreSimUpdates = false;
      StartingLocation startingLocation = this.m_startLocationProvider.StartingLocation;
      this.m_basePosition = startingLocation.Position;
      this.m_baseRotation = startingLocation.ShoreDirection.ToRotation().Rotated180;
      this.m_upointsManager.AddInitialUnity(5.Upoints());
      StartingFactoryPlan planShared;
      string error;
      if (!this.m_staringFactoryPlacer.TryCreatePlan(this.m_basePosition, this.m_baseRotation, out planShared, out error, (Option<Lyst<Pair<Tile3i, Option<string>>>>) Option.None))
      {
        this.markFailed("Failed to place starting buildings: " + error);
      }
      else
      {
        this.m_simLoopEvents.Update.RemoveNonSaveable<StartingFactoryBuilder>(this, new Action(this.simUpdate));
        this.buildShipyardAndDockFleet(planShared);
        this.placeRuinedRadioTower(planShared);
        this.buildVehicles(new RelTile2i(20, -5).Rotate(this.m_baseRotation));
        this.buildSettlement(Ids.Buildings.Housing, planShared);
        this.m_terrainManager.StabilizeTerrainDisruption();
      }
    }

    private void buildShipyardAndDockFleet(StartingFactoryPlan plan)
    {
      if (!plan.ShipyardPos.HasValue)
      {
        this.markFailed("No shipyard position set!");
      }
      else
      {
        Option<Mafi.Core.Buildings.Shipyard.Shipyard> option = this.m_entitiesBuilder.TryBuildLayoutEntity<Mafi.Core.Buildings.Shipyard.Shipyard>(Ids.Buildings.Shipyard, plan.ShipyardPos.Value, this.m_baseRotation, makeFullyConstructed: true);
        if (option.IsNone)
        {
          this.markFailed(string.Format("Failed to build shipyard at {0}.", (object) plan.ShipyardPos));
        }
        else
        {
          Mafi.Core.Buildings.Shipyard.Shipyard dock = option.Value;
          this.m_fleetManager.CreateAndAddShip(dock);
          ImmutableArray<KeyValuePair<ProductProto.ID, Quantity>> immutableArray = this.m_config.InitialProducts;
          foreach (KeyValuePair<ProductProto.ID, Quantity> keyValuePair in immutableArray)
          {
            ProductProto orThrow = this.m_protosDb.GetOrThrow<ProductProto>((Mafi.Core.Prototypes.Proto.ID) keyValuePair.Key);
            dock.StoreProduct(orThrow.WithQuantity(keyValuePair.Value));
            this.m_productsManager.ProductCreated(orThrow, keyValuePair.Value, CreateReason.InitialResource);
          }
          if (this.m_tutorialsConfig.AreTutorialsEnabled)
            return;
          immutableArray = this.m_config.ExtraInitialProductsIfGoalsSkipped;
          foreach (KeyValuePair<ProductProto.ID, Quantity> keyValuePair in immutableArray)
          {
            ProductProto orThrow = this.m_protosDb.GetOrThrow<ProductProto>((Mafi.Core.Prototypes.Proto.ID) keyValuePair.Key);
            dock.StoreProduct(orThrow.WithQuantity(keyValuePair.Value));
            this.m_productsManager.ProductCreated(orThrow, keyValuePair.Value, CreateReason.InitialResource);
          }
        }
      }
    }

    private void placeRuinedRadioTower(StartingFactoryPlan plan)
    {
      if (!plan.RadioTowerPos.HasValue)
      {
        this.markFailed("No radio tower position set!");
      }
      else
      {
        if (!this.m_entitiesBuilder.TryBuildLayoutEntity<Mafi.Core.Buildings.RuinedBuildings.Ruins>(Ids.Buildings.Ruins, plan.RadioTowerPos.Value, makeFullyConstructed: true).IsNone)
          return;
        this.markFailed(string.Format("Failed to build radio tower at {0}.", (object) plan.RadioTowerPos));
      }
    }

    private TerrainTile findFirstOceanTile(StartingLocation startingLocation)
    {
      TerrainTile firstOceanTile = this.m_terrainManager[startingLocation.Position];
      Assert.That<bool>(firstOceanTile.IsNotOcean).IsTrue("Inner tile of starting cell is already an ocean?");
      for (int index = 100; index >= 0; --index)
      {
        if (this.m_terrainManager.IsOffLimits(firstOceanTile.DataIndex))
          throw new StartingFactoryBuilderException(string.Format("Failed to find initial ocean. Tile {0} is off-limits.", (object) firstOceanTile));
        firstOceanTile = firstOceanTile[startingLocation.ShoreDirection];
        if (firstOceanTile.IsOcean)
          return firstOceanTile;
      }
      throw new StartingFactoryBuilderException("Ocean not found on -X side of starting cell!");
    }

    private void flattenArea(Tile2i from, Tile2i to, HeightTilesI height, bool onlyReduceHeight = false)
    {
      for (int y = from.Y; y < to.Y; ++y)
      {
        TerrainTile plusXneighbor = this.m_terrainManager[new Tile2i(from.X, y)];
        int x = from.X;
        while (x < to.X)
        {
          if (this.m_terrainManager.IsOffLimits(plusXneighbor.DataIndex))
            Log.Error(string.Format("Flattening of area failed. Tile at {0} is off-limits.", (object) new Tile2i(x, y)));
          if (plusXneighbor.Height == height.HeightTilesF)
            return;
          if (!onlyReduceHeight || !(plusXneighbor.Height < height.HeightTilesF))
          {
            this.m_terrainManager.SetHeight(plusXneighbor.CoordAndIndex, height.HeightTilesF);
            this.m_terrainManager.StopTerrainPhysicsSimulationAt(plusXneighbor.CoordAndIndex);
          }
          ++x;
          plusXneighbor = plusXneighbor.PlusXNeighbor;
        }
      }
    }

    private void buildVehicles(RelTile2i offset)
    {
      Tile2i position1 = this.m_basePosition + offset;
      RelTile2i increment = new RelTile2i(0, 3).Rotate(this.m_baseRotation);
      AngleDegrees1f angle = this.m_baseRotation.Angle;
      if (!this.m_entitiesBuilder.TryCreateVehicles<Truck>(Ids.Vehicles.TruckT1.Id, this.m_config.InitialTrucks, position1, angle, increment))
        this.markFailed(string.Format("Failed to build {0} initial trucks.", (object) this.m_config.InitialTrucks));
      Tile2i position2 = position1 + increment * this.m_config.InitialTrucks;
      if (!this.m_entitiesBuilder.TryCreateVehicles<Excavator>(Ids.Vehicles.ExcavatorT1, this.m_config.InitialExcavators, position2, angle, increment))
        this.markFailed(string.Format("Failed to build {0} initial excavators.", (object) this.m_config.InitialExcavators));
      Tile2i position3 = position2 + increment * this.m_config.InitialExcavators;
      if (!this.m_entitiesBuilder.TryCreateVehicles<Mafi.Core.Vehicles.TreeHarvesters.TreeHarvester>(Ids.Vehicles.TreeHarvesterT1, this.m_config.InitialTreeHarvesters, position3, angle, increment))
        this.markFailed(string.Format("Failed to build {0} initial tree harvesters.", (object) this.m_config.InitialTreeHarvesters));
      IRandom random = (IRandom) new XorRsr128PlusGenerator(RandomGeneratorType.SimOnly, this.m_basePosition.X.GetRngSeed(), this.m_basePosition.Y.GetRngSeed());
      foreach (Vehicle allVehicle in (IEnumerable<Vehicle>) this.m_vehiclesManager.AllVehicles)
      {
        Tile2f position2f = allVehicle.Position2f;
        RelTile2f relTile2f1 = new RelTile2f(random.NextFix32Between01Fast() - Fix32.Half, random.NextFix32Between01Fast() * Fix32.Half - Fix32.Quarter);
        RelTile2f relTile2f2 = relTile2f1.Rotate(this.m_baseRotation);
        Tile2f position4 = position2f + relTile2f2;
        AngleDegrees1f angleDegrees1f = allVehicle.Direction + random.NextAngle(-20.Degrees(), 20.Degrees());
        if (!allVehicle.IsPathable(position4.Tile2i))
        {
          for (int index = 0; index < 10; ++index)
          {
            Tile2f tile2f = position4;
            relTile2f1 = new RelTile2f(Fix32.One, Fix32.Zero);
            RelTile2f relTile2f3 = relTile2f1.Rotate(this.m_baseRotation);
            position4 = tile2f + relTile2f3;
            if (allVehicle.IsPathable(position4.Tile2i))
              break;
          }
        }
        allVehicle.TeleportTo(position4, new AngleDegrees1f?(angleDegrees1f));
      }
    }

    private void buildSettlement(StaticEntityProto.ID id, StartingFactoryPlan plan)
    {
      if (!plan.Housing1Pos.HasValue)
        this.markFailed("No housing 1 position set!");
      else if (!plan.Housing2Pos.HasValue)
        this.markFailed("No housing 2 position set!");
      else if (this.m_entitiesBuilder.TryBuildLayoutEntity<SettlementHousingModule>(id, plan.Housing1Pos.Value, makeFullyConstructed: true).IsNone)
        this.markFailed(string.Format("Failed to build settlement {0} at {1}!", (object) id, (object) plan.Housing1Pos.Value));
      else if (this.m_entitiesBuilder.TryBuildLayoutEntity<SettlementHousingModule>(id, plan.Housing2Pos.Value, Rotation90.Deg90, makeFullyConstructed: true).IsNone)
      {
        this.markFailed(string.Format("Failed to build second settlement {0} at {1}!", (object) id, (object) plan.Housing2Pos.Value));
      }
      else
      {
        ImmutableArray<LayoutEntitySlot> availableSlots = this.m_settlementModulesSlotValidator.GetAvailableSlots((ISettlementModuleProto) this.m_protosDb.GetOrThrow<SettlementFoodModuleProto>((Mafi.Core.Prototypes.Proto.ID) Ids.Buildings.SettlementFoodModule));
        Option<SettlementFoodModule> option = Option<SettlementFoodModule>.None;
        foreach (LayoutEntitySlot layoutEntitySlot in availableSlots)
        {
          if (this.m_entitiesBuilder.CanBuildLayoutEntity(Ids.Buildings.SettlementFoodModule, layoutEntitySlot.Transform.Position, layoutEntitySlot.Transform.Rotation, layoutEntitySlot.Transform.IsReflected, out string _))
          {
            option = this.m_entitiesBuilder.TryBuildLayoutEntity<SettlementFoodModule>(Ids.Buildings.SettlementFoodModule, layoutEntitySlot.Transform, true);
            break;
          }
        }
        if (option.IsNone)
        {
          this.markFailed(string.Format("Failed to build food module {0}!", (object) id));
        }
        else
        {
          FoodProto foodProto = this.m_protosDb.All<FoodProto>().First<FoodProto>((Func<FoodProto, bool>) (x => x.Product.Id == this.m_config.StartingFoodProto));
          option.Value.Cheat_FillBuffers(foodProto.Product, 60.Percent());
          this.m_settlementsManager.AddInitialPopulation(this.m_populationConfig.StartingPopulation);
        }
      }
    }
  }
}
