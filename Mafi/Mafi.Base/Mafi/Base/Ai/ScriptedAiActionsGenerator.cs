// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Ai.ScriptedAiActionsGenerator
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Ai.Scripted;
using Mafi.Core.Ai.Scripted.Actions;
using Mafi.Core.Buildings.RuinedBuildings;
using Mafi.Core.Buildings.Shipyard;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;

#nullable disable
namespace Mafi.Base.Ai
{
  public class ScriptedAiActionsGenerator
  {
    private int m_trucksForTreeHarvesting;
    private int m_housesCount;
    private int m_extraTrucksCalls;
    private readonly Tile2i m_oilDepositPosition;

    public static void CreateGamePlaythroughActions(Lyst<IScriptedAiPlayerAction> actions)
    {
      new ScriptedAiActionsGenerator().createGamePlaythroughActions(actions);
    }

    private void createGamePlaythroughActions(Lyst<IScriptedAiPlayerAction> actions)
    {
    }

    private void setupIronSmelting(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.CharcoalMaker, "burner_1"), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.SmokeStack, "smoke_2", new PlacementSpec(true)));
      actions.Add((IScriptedAiPlayerAction) new BuildTransportBetweenAction("burner_1", "smoke_2", Ids.Products.Exhaust), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(new string[2]
      {
        "burner_1",
        "smoke_2"
      }));
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.Caster, "cast_1.1"), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.SmeltingFurnaceT1, "smelt_1"), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.SmokeStack, "smoke_1", new PlacementSpec(true)), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.Caster, "cast_1.2"));
      actions.Add((IScriptedAiPlayerAction) new SetRecipeAction("smelt_1", Ids.Recipes.IronSmeltingT1Scrap, true), (IScriptedAiPlayerAction) new SetRecipeAction("cast_1.1", Ids.Recipes.IronCasting, true), (IScriptedAiPlayerAction) new SetRecipeAction("cast_1.2", Ids.Recipes.IronCasting, true));
      actions.Add((IScriptedAiPlayerAction) new BuildTransportBetweenAction("smelt_1", "cast_1.1", Ids.Products.MoltenIron), (IScriptedAiPlayerAction) new BuildTransportBetweenAction("smelt_1", "cast_1.2", Ids.Products.MoltenIron), (IScriptedAiPlayerAction) new BuildTransportBetweenAction("smelt_1", "smoke_1", Ids.Products.Exhaust), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(new string[4]
      {
        "smelt_1",
        "cast_1.1",
        "cast_1.2",
        "smoke_1"
      }));
      RectangleTerrainArea2i area = TestMapGenerator.GetTreesArea().ExtendBy(new RelTile2i(20, 20));
      actions.Add((IScriptedAiPlayerAction) new ScheduleCommandAction((IInputCommand) new RuinsScrapCmd(true)), (IScriptedAiPlayerAction) new ScheduleCommandAction((IInputCommand) new DesignateHarvestedTreesCmd(area, true, new ProductProto.ID?())));
      this.assignTruckToTreeHarvesting(actions);
      actions.Add((IScriptedAiPlayerAction) new WaitForNewGlobalProductsAction(Ids.Products.Wood, 10.Quantity(), 1.Minutes()), (IScriptedAiPlayerAction) new WaitForNewGlobalProductsAction(Ids.Products.Iron, 8.Quantity(), 5.Minutes()));
    }

    private void setupCopperSmelting(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.SmeltingFurnaceT1, "smelt_cop"), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.Caster, "cast_cop1"), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.SmokeStack, "smoke_cop", new PlacementSpec(true)));
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.CopperElectrolysis, "electrolysis"), (IScriptedAiPlayerAction) new BuildStaticEntityAction(Ids.Buildings.RainwaterHarvester, "harvester_cop"));
      actions.Add((IScriptedAiPlayerAction) new SetRecipeAction("smelt_cop", Ids.Recipes.CopperSmeltingT1, true), (IScriptedAiPlayerAction) new SetRecipeAction("electrolysis", Ids.Recipes.CopperElectrolysisWithWater, true), (IScriptedAiPlayerAction) new SetRecipeAction("cast_cop1", Ids.Recipes.CopperCasting, true));
      actions.Add((IScriptedAiPlayerAction) new BuildTransportBetweenAction("smelt_cop", "cast_cop1", Ids.Products.MoltenCopper), (IScriptedAiPlayerAction) new BuildTransportBetweenAction("smelt_cop", "smoke_cop", Ids.Products.Exhaust), (IScriptedAiPlayerAction) new BuildTransportBetweenAction("harvester_cop", "electrolysis", Ids.Products.Water));
      actions.Add((IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(20.Minutes(), new string[4]
      {
        "smelt_cop",
        "cast_cop1",
        "electrolysis",
        "harvester_cop"
      }));
    }

    private void setupBetterIronSmelting(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.SmeltingFurnaceT1, "smeltIron_1"), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.SmokeStack, "smokeIron_1", new PlacementSpec(true)), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.Caster, "cast_iron_1.1"), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.Caster, "cast_iron_1.2"));
      actions.Add((IScriptedAiPlayerAction) new SetRecipeAction("smeltIron_1", Ids.Recipes.IronSmeltingT1Coal, true), (IScriptedAiPlayerAction) new SetRecipeAction("cast_iron_1.1", Ids.Recipes.IronCasting, true), (IScriptedAiPlayerAction) new SetRecipeAction("cast_iron_1.2", Ids.Recipes.IronCasting, true));
      actions.Add((IScriptedAiPlayerAction) new BuildTransportBetweenAction("smeltIron_1", "cast_iron_1.1", Ids.Products.MoltenIron), (IScriptedAiPlayerAction) new BuildTransportBetweenAction("smeltIron_1", "cast_iron_1.2", Ids.Products.MoltenIron), (IScriptedAiPlayerAction) new BuildTransportBetweenAction("smeltIron_1", "smokeIron_1", Ids.Products.Exhaust), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(20.Minutes(), new string[4]
      {
        "smeltIron_1",
        "cast_iron_1.1",
        "cast_iron_1.2",
        "smokeIron_1"
      }));
    }

    private void setupCpProduction(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.AssemblyManual, "cp_assm"), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(new string[1]
      {
        "cp_assm"
      }), (IScriptedAiPlayerAction) new SetRecipeAction("cp_assm", Ids.Recipes.CpAssemblyT1, true));
      actions.Add((IScriptedAiPlayerAction) new WaitForNewGlobalProductsAction(Ids.Products.ConstructionParts, 4.Quantity(), 10.Minutes()));
    }

    private void assignTruckToTreeHarvesting(Lyst<IScriptedAiPlayerAction> actions)
    {
      ++this.m_trucksForTreeHarvesting;
    }

    private void buildVehicleDepot(Lyst<IScriptedAiPlayerAction> actions)
    {
      ScriptedAiPlayerConfig forDefaultTestMap = ScriptedAiConfigs.GetScriptedAiPlayerConfigForDefaultTestMap();
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction(Ids.Buildings.VehiclesDepot, "vehicles_depot", new PlacementSpec(rotation: Rotation90.Deg90, customPosition: new Tile2i?(forDefaultTestMap.VehicleDepotPosition))));
      actions.Add((IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(10.Minutes(), new string[1]
      {
        "vehicles_depot"
      }));
    }

    private void setupIronOreMining(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new SetupTerrainDesignationsAction(IdsCore.TerrainDesignators.DumpingDesignator, TestMapGenerator.GetDumpArea(), DesignationType.RampUp));
      this.setupMiningFor(actions, Ids.TerrainMaterials.IronOre, "iron");
      actions.Add((IScriptedAiPlayerAction) new SetRecipeAction("smelt_1", Ids.Recipes.IronSmeltingT1Coal, true));
    }

    private void buildFarm(Lyst<IScriptedAiPlayerAction> actions)
    {
      int count = actions.Count;
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction(Ids.Buildings.FarmT1, string.Format("farm_{0}", (object) count), new PlacementSpec(altLane: true, rotation: Rotation90.Deg270)));
      actions.Add((IScriptedAiPlayerAction) new SetupFarmScheduleAction(string.Format("farm_{0}", (object) count), new Proto.ID[2]
      {
        Ids.Crops.Potato,
        Ids.Crops.GreenManure
      }));
    }

    private void buildDieselGenerator(Lyst<IScriptedAiPlayerAction> actions)
    {
      int count = actions.Count;
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.DieselGenerator, string.Format("diesel_engine_{0}", (object) count)), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(10.Minutes(), new string[1]
      {
        string.Format("diesel_engine_{0}", (object) count)
      }));
    }

    private void buildHouse(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction(Ids.Buildings.Housing, string.Format("house_{0}", (object) this.m_housesCount), new PlacementSpec(customSpacing: new int?(0), relativeTo: string.Format("house_{0}", (object) (this.m_housesCount - 1)), relativeToDirection: this.m_housesCount == 1 ? Direction90.MinusX : Direction90.PlusY)), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(10.Minutes(), new string[1]
      {
        string.Format("house_{0}", (object) this.m_housesCount)
      }));
      ++this.m_housesCount;
    }

    private void setupBeacon(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction(Ids.Buildings.Beacon, "beacon"), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(5.Minutes(), new string[1]
      {
        "beacon"
      }));
    }

    private void buildDiesel(Lyst<IScriptedAiPlayerAction> actions)
    {
      int count = actions.Count;
      Lyst<IScriptedAiPlayerAction> lyst1 = actions;
      StaticEntityProto.ID oilPump1 = (StaticEntityProto.ID) Ids.Machines.OilPump;
      string name1 = string.Format("oil_pump_{0}", (object) count);
      Tile2i? customPosition = new Tile2i?(this.m_oilDepositPosition - new RelTile2i(6, 6));
      PlacementSpec placement1 = new PlacementSpec(customPosition: customPosition);
      BuildStaticEntityAction staticEntityAction1 = new BuildStaticEntityAction(oilPump1, name1, placement1);
      StaticEntityProto.ID oilPump2 = (StaticEntityProto.ID) Ids.Machines.OilPump;
      string name2 = string.Format("oil_pump_{0}", (object) (count + 1));
      customPosition = new Tile2i?(this.m_oilDepositPosition);
      PlacementSpec placement2 = new PlacementSpec(customPosition: customPosition);
      BuildStaticEntityAction staticEntityAction2 = new BuildStaticEntityAction(oilPump2, name2, placement2);
      StaticEntityProto.ID basicDieselDistiller = (StaticEntityProto.ID) Ids.Machines.BasicDieselDistiller;
      string name3 = string.Format("bio_diesel_{0}", (object) count);
      customPosition = new Tile2i?(this.m_oilDepositPosition - new RelTile2i(16, 16));
      PlacementSpec placement3 = new PlacementSpec(rotation: Rotation90.Deg270, customPosition: customPosition);
      BuildStaticEntityAction staticEntityAction3 = new BuildStaticEntityAction(basicDieselDistiller, name3, placement3);
      WaitForStaticEntitiesBuiltAction entitiesBuiltAction1 = new WaitForStaticEntitiesBuiltAction(10.Minutes(), new string[1]
      {
        string.Format("bio_diesel_{0}", (object) count)
      });
      lyst1.Add((IScriptedAiPlayerAction) staticEntityAction1, (IScriptedAiPlayerAction) staticEntityAction2, (IScriptedAiPlayerAction) staticEntityAction3, (IScriptedAiPlayerAction) entitiesBuiltAction1);
      Lyst<IScriptedAiPlayerAction> lyst2 = actions;
      StaticEntityProto.ID smokeStack = (StaticEntityProto.ID) Ids.Machines.SmokeStack;
      string name4 = string.Format("bio_smoke_{0}", (object) count);
      customPosition = new Tile2i?(this.m_oilDepositPosition - new RelTile2i(22, 16));
      PlacementSpec placement4 = new PlacementSpec(customPosition: customPosition);
      BuildStaticEntityAction staticEntityAction4 = new BuildStaticEntityAction(smokeStack, name4, placement4);
      WaitForStaticEntitiesBuiltAction entitiesBuiltAction2 = new WaitForStaticEntitiesBuiltAction(new string[1]
      {
        string.Format("bio_smoke_{0}", (object) count)
      });
      lyst2.Add((IScriptedAiPlayerAction) staticEntityAction4, (IScriptedAiPlayerAction) entitiesBuiltAction2);
      actions.Add((IScriptedAiPlayerAction) new BuildTransportBetweenAction(string.Format("bio_diesel_{0}", (object) count), string.Format("bio_smoke_{0}", (object) count), Ids.Products.Exhaust), (IScriptedAiPlayerAction) new BuildTransportBetweenAction(string.Format("oil_pump_{0}", (object) count), string.Format("bio_diesel_{0}", (object) count), Ids.Products.CrudeOil));
    }

    private void buildSmallTrucks(Lyst<IScriptedAiPlayerAction> actions, int count)
    {
      this.buildExtraTrucks(actions, count, Ids.Vehicles.TruckT1.Id);
    }

    private void buildExtraTrucks(
      Lyst<IScriptedAiPlayerAction> actions,
      int count,
      DynamicEntityProto.ID id)
    {
      ++this.m_extraTrucksCalls;
      for (int index = 0; index < count; ++index)
        actions.Add((IScriptedAiPlayerAction) new ScheduleVehicleConstructionAction(id));
      actions.Add((IScriptedAiPlayerAction) new WaitForNewVehiclesAction(id, count, 10.Minutes(), string.Format("Extra trucks #{0}", (object) this.m_extraTrucksCalls)));
    }

    private void setupDiesel(Lyst<IScriptedAiPlayerAction> actions)
    {
      this.buildDiesel(actions);
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.WasteDump, "waste_pump", new PlacementSpec(customPosition: new Tile2i?(new Tile2i(-32, 58)))), (IScriptedAiPlayerAction) new SetRecipeAction("waste_pump", Ids.Recipes.WasteWaterDumping, true), (IScriptedAiPlayerAction) new ToggleLogisticsAction("waste_pump", true, EntityLogisticsMode.On));
    }

    private void buildMaintenanceDepotT0(Lyst<IScriptedAiPlayerAction> actions)
    {
      int count = actions.Count;
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Buildings.MaintenanceDepotT0, string.Format("mttn_depot_{0}", (object) count)), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(15.Minutes(), new string[1]
      {
        string.Format("mttn_depot_{0}", (object) count)
      }), (IScriptedAiPlayerAction) new SetRecipeAction(string.Format("mttn_depot_{0}", (object) count), Ids.Recipes.MaintenanceT0, true));
    }

    private void setupSettlementWater(Lyst<IScriptedAiPlayerAction> actions)
    {
      int count = actions.Count;
      Lyst<IScriptedAiPlayerAction> lyst = actions;
      StaticEntityProto.ID settlementWaterModule = Ids.Buildings.SettlementWaterModule;
      Rotation90 deg180_1 = Rotation90.Deg180;
      Direction90 minusX = Direction90.MinusX;
      RelTile2i relTile2i = new RelTile2i(4, -4);
      int? customSpacing = new int?();
      Rotation90 rotation = deg180_1;
      Direction90 relativeToDirection = minusX;
      RelTile2i relativeOffset = relTile2i;
      Tile2i? customPosition1 = new Tile2i?();
      Tile2i? customPosition2 = customPosition1;
      PlacementSpec placement1 = new PlacementSpec(customSpacing: customSpacing, rotation: rotation, relativeTo: "house_1", relativeToDirection: relativeToDirection, relativeOffset: relativeOffset, customPosition: customPosition2);
      BuildStaticEntityAction staticEntityAction1 = new BuildStaticEntityAction(settlementWaterModule, "settlement_water", placement1);
      StaticEntityProto.ID rainwaterHarvester = Ids.Buildings.RainwaterHarvester;
      string name1 = string.Format("water_collector_{0}", (object) count);
      Rotation90 deg180_2 = Rotation90.Deg180;
      customPosition1 = new Tile2i?(new Tile2i(-10, 74));
      PlacementSpec placement2 = new PlacementSpec(rotation: deg180_2, customPosition: customPosition1);
      BuildStaticEntityAction staticEntityAction2 = new BuildStaticEntityAction(rainwaterHarvester, name1, placement2);
      StaticEntityProto.ID wasteDump = (StaticEntityProto.ID) Ids.Machines.WasteDump;
      string name2 = string.Format("waste_pump_{0}", (object) count);
      customPosition1 = new Tile2i?(new Tile2i(-32, 64));
      PlacementSpec placement3 = new PlacementSpec(customPosition: customPosition1);
      BuildStaticEntityAction staticEntityAction3 = new BuildStaticEntityAction(wasteDump, name2, placement3);
      SetRecipeAction setRecipeAction = new SetRecipeAction(string.Format("waste_pump_{0}", (object) count), Ids.Recipes.WasteWaterDumping, true);
      lyst.Add((IScriptedAiPlayerAction) staticEntityAction1, (IScriptedAiPlayerAction) staticEntityAction2, (IScriptedAiPlayerAction) staticEntityAction3, (IScriptedAiPlayerAction) setRecipeAction);
      actions.Add((IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(10.Minutes(), new string[3]
      {
        "settlement_water",
        string.Format("water_collector_{0}", (object) count),
        string.Format("waste_pump_{0}", (object) count)
      }));
      actions.Add((IScriptedAiPlayerAction) new BuildTransportBetweenAction(string.Format("water_collector_{0}", (object) count), "settlement_water", Ids.Products.Water, name: "t_settlementWater1"), (IScriptedAiPlayerAction) new BuildTransportBetweenAction("settlement_water", string.Format("waste_pump_{0}", (object) count), Ids.Products.WasteWater, name: "t_settlementWater2"), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(new string[2]
      {
        "t_settlementWater1",
        "t_settlementWater2"
      }));
    }

    private void setupCoalMiningAndUsage(Lyst<IScriptedAiPlayerAction> actions)
    {
      this.setupMiningFor(actions, Ids.TerrainMaterials.Coal, "coal");
    }

    private void setupMiningFor(
      Lyst<IScriptedAiPlayerAction> actions,
      Proto.ID materialId,
      string name)
    {
      actions.Add((IScriptedAiPlayerAction) new SetupTerrainDesignationsAction(Ids.Buildings.MineTower, (Option<string>) (name + "_tower"), IdsCore.TerrainDesignators.MiningDesignator, DesignationType.Flat, materialId), (IScriptedAiPlayerAction) new ScheduleVehicleConstructionAction(Ids.Vehicles.ExcavatorT1));
      actions.Add((IScriptedAiPlayerAction) new WaitForNewVehiclesAction(Ids.Vehicles.ExcavatorT1, 1, 30.Minutes(), name + " setup"), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(12.Minutes(), new string[1]
      {
        name + "_tower"
      }));
      actions.Add((IScriptedAiPlayerAction) new AssignVehicleToEntityAction(Ids.Vehicles.ExcavatorT1, 1, name + "_tower"), (IScriptedAiPlayerAction) new AssignVehicleToEntityAction(Ids.Vehicles.TruckT1.Id, 2, name + "_tower"), (IScriptedAiPlayerAction) new ScheduleVehicleConstructionAction(Ids.Vehicles.TruckT1.Id), (IScriptedAiPlayerAction) new ScheduleVehicleConstructionAction(Ids.Vehicles.TruckT1.Id));
    }

    private void setupConcreteProduction(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.ConcreteMixer, "concrete"), (IScriptedAiPlayerAction) new BuildStaticEntityAction(Ids.Buildings.RainwaterHarvester, "concrete_water", new PlacementSpec(true)), (IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.Crusher, "crusher"));
      actions.Add((IScriptedAiPlayerAction) new BuildTransportBetweenAction("concrete_water", "concrete", Ids.Products.Water), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(20.Minutes(), new string[2]
      {
        "concrete",
        "concrete_water"
      }), (IScriptedAiPlayerAction) new SetRecipeAction("crusher", Ids.Recipes.SlagCrushing, true));
    }

    private void setupCp2(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction((StaticEntityProto.ID) Ids.Machines.AssemblyElectrified, "cp2_assm"), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(20.Minutes(), new string[1]
      {
        "cp2_assm"
      }), (IScriptedAiPlayerAction) new SetRecipeAction("cp2_assm", Ids.Recipes.Cp2AssemblyT2, true));
    }

    private void setupConveyorBelts(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildTransportBetweenAction("crusher", "concrete", Ids.Products.SlagCrushed, name: "t_crusher"), (IScriptedAiPlayerAction) new BuildTransportBetweenAction("cast_cop1", "electrolysis", Ids.Products.ImpureCopper, name: "t_copper"), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(new string[2]
      {
        "t_crusher",
        "t_copper"
      }));
    }

    private void setupSlagStorage(Lyst<IScriptedAiPlayerAction> actions, string furnaceName)
    {
      string toEntityName = furnaceName + "_slag_storage";
      Lyst<IScriptedAiPlayerAction> lyst = actions;
      StaticEntityProto.ID storageLoose = Ids.Buildings.StorageLoose;
      string str = toEntityName;
      ProductProto.ID slag = Ids.Products.Slag;
      string name = str;
      Percent? nullable = new Percent?(50.Percent());
      PlacementSpec placement = new PlacementSpec(customSpacing: new int?(10), rotation: Rotation90.Deg90, relativeTo: furnaceName, relativeToDirection: Direction90.PlusY);
      Percent? importUntil = new Percent?();
      Percent? exportFrom = nullable;
      BuildStorageAction buildStorageAction = new BuildStorageAction(storageLoose, slag, name, placement, importUntil, exportFrom);
      BuildTransportBetweenAction transportBetweenAction = new BuildTransportBetweenAction(furnaceName, toEntityName, Ids.Products.Slag);
      WaitForStaticEntitiesBuiltAction entitiesBuiltAction = new WaitForStaticEntitiesBuiltAction(10.Minutes(), new string[1]
      {
        toEntityName
      });
      lyst.Add((IScriptedAiPlayerAction) buildStorageAction, (IScriptedAiPlayerAction) transportBetweenAction, (IScriptedAiPlayerAction) entitiesBuiltAction);
    }

    private void setupFuelStation(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new BuildStaticEntityAction(Ids.Buildings.FuelStationT1, "fuel_station"), (IScriptedAiPlayerAction) new ScheduleVehicleConstructionAction(Ids.Vehicles.TruckT1.Id), (IScriptedAiPlayerAction) new AssignVehicleToEntityAction(Ids.Vehicles.TruckT1.Id, 1, "fuel_station"));
      Lyst<IScriptedAiPlayerAction> lyst = actions;
      StaticEntityProto.ID storageFluid = Ids.Buildings.StorageFluid;
      ProductProto.ID diesel = Ids.Products.Diesel;
      Percent? nullable = new Percent?(20.Percent());
      PlacementSpec placement = new PlacementSpec(true);
      Percent? importUntil = nullable;
      Percent? exportFrom = new Percent?();
      BuildStorageAction buildStorageAction = new BuildStorageAction(storageFluid, diesel, "diesel_storage", placement, importUntil, exportFrom);
      BuildTransportBetweenAction transportBetweenAction = new BuildTransportBetweenAction("diesel_storage", "fuel_station", Ids.Products.Diesel);
      lyst.Add((IScriptedAiPlayerAction) buildStorageAction, (IScriptedAiPlayerAction) transportBetweenAction);
      actions.Add((IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(10.Minutes(), new string[1]
      {
        "fuel_station"
      }), (IScriptedAiPlayerAction) new WaitForStaticEntitiesBuiltAction(10.Minutes(), new string[1]
      {
        "diesel_storage"
      }));
    }

    private void setupShipRepair(Lyst<IScriptedAiPlayerAction> actions)
    {
      actions.Add((IScriptedAiPlayerAction) new UpgradeStaticEntityAction("dock_entity", 20.Minutes()), (IScriptedAiPlayerAction) new ScheduleCommandAction((IInputCommand) new ShipyardSetRepairingCmd(true)));
    }

    public ScriptedAiActionsGenerator()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_housesCount = 1;
      this.m_oilDepositPosition = TestMapGenerator.OIL_DEPOSIT_POS.Xy;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
