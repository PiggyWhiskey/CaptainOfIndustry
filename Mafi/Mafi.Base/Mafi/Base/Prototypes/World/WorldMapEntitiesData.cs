// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.World.WorldMapEntitiesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Maintenance;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.World.Contracts;
using Mafi.Core.World.Entities;
using Mafi.Core.World.QuickTrade;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.World
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class WorldMapEntitiesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb protosDb = registrator.PrototypesDb;
      ProductProto oil = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.CrudeOil);
      ProductProto orThrow1 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Iron);
      ProductProto orThrow2 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Steel);
      ProductProto cp2 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.ConstructionParts2);
      ProductProto cp3 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.ConstructionParts3);
      ProductProto orThrow3 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Copper);
      VirtualProductProto maintenanceT1 = protosDb.GetOrThrow<VirtualProductProto>((Proto.ID) Ids.Products.MaintenanceT1);
      Proto.ID id = new Proto.ID("UpointsStatsCat_WorldMapMines");
      UpointsStatsCategoryProto statsCategory = registrator.PrototypesDb.Add<UpointsStatsCategoryProto>(new UpointsStatsCategoryProto(id, Proto.CreateStr(id, "World map mines", ""), new UpointsStatsCategoryProto.Gfx("Assets/Base/Icons/WorldMap/OilRig.svg")));
      UpointsCategoryProto oilRigUpointsCategory = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.World.OilRigCost1, "Assets/Base/Icons/WorldMap/OilRig.svg", (Option<UpointsStatsCategoryProto>) statsCategory));
      Proto.Str oilRigStr = Proto.CreateStr((Proto.ID) Ids.World.OilRigCost1, "Oil rig", "This station provides crude oil when assigned with workers.");
      addOilRig(Ids.World.OilRigCost1, new AssetValue(cp2.WithQuantity(120)), 1040000);
      addOilRig(Ids.World.OilRigCost2, new AssetValue(cp2.WithQuantity(240)), 1040000);
      addOilRig(Ids.World.OilRigCost3, new AssetValue(cp2.WithQuantity(240)), 780000);
      UpointsCategoryProto upointsCategory1 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.World.WaterWell, "Assets/Base/Icons/WorldMap/WaterWell.svg", (Option<UpointsStatsCategoryProto>) statsCategory));
      ProductProto orThrow4 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Water);
      protosDb.Add<WorldMapMineProto>(new WorldMapMineProto(Ids.World.WaterWell, Proto.CreateStr((Proto.ID) Ids.World.WaterWell, "Groundwater well", "This station can extract fresh water."), new ProductQuantity(orThrow4, 8.Quantity()), 10.Seconds(), 0.2.Upoints(), upointsCategory1, new EntityCosts(new AssetValue(cp3.WithQuantity(100)), 8), new Func<int, EntityCosts>(waterWellCostFunc), 8, new Quantity?(), new WorldMapEntityProto.Gfx("Assets/Unity/UserInterface/WorldMap/PumpjackBig.svg", "Assets/Base/Icons/WorldMap/WaterWell.svg")));
      LocStr1 locStr1 = Loc.Str1("WorldMine__Desc", "This site mines {0} when assigned with workers.", "description of a world mine, example use: This site mines Coal when assigned with workers.");
      UpointsCategoryProto upointsCategory2 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.World.SulfurMine, "Assets/Base/Icons/WorldMap/Sulfur.svg", (Option<UpointsStatsCategoryProto>) statsCategory));
      ProductProto orThrow5 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Sulfur);
      protosDb.Add<WorldMapMineProto>(new WorldMapMineProto(Ids.World.SulfurMine, Proto.CreateStr((Proto.ID) Ids.World.SulfurMine, "Sulfur mine", LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.World.SulfurMine.Value, locStr1.Format(orThrow5.Strings.Name))), new ProductQuantity(orThrow5, 6.Quantity()), 20.Seconds(), 0.2.Upoints(), upointsCategory2, new EntityCosts(new AssetValue(cp2.WithQuantity(200)), 8), new Func<int, EntityCosts>(sulfurMineCostFunc), 8, new Quantity?(), new WorldMapEntityProto.Gfx(orThrow5.Graphics.IconPath, "Assets/Base/Icons/WorldMap/Sulfur.svg")));
      UpointsCategoryProto upointsCategory3 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.World.CoalMine, "Assets/Base/Icons/WorldMap/Sulfur.svg", (Option<UpointsStatsCategoryProto>) statsCategory));
      ProductProto orThrow6 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Coal);
      protosDb.Add<WorldMapMineProto>(new WorldMapMineProto(Ids.World.CoalMine, Proto.CreateStr((Proto.ID) Ids.World.CoalMine, "Coal mine", LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.World.CoalMine.Value, locStr1.Format(orThrow6.Strings.Name))), new ProductQuantity(orThrow6, 9.Quantity()), 20.Seconds(), 0.5.Upoints(), upointsCategory3, new EntityCosts(new AssetValue(cp3.WithQuantity(200)), 8), new Func<int, EntityCosts>(coalMineCostFunc), 20, new Quantity?(450000.Quantity()), new WorldMapEntityProto.Gfx(orThrow6.Graphics.IconPath, "Assets/Base/Icons/WorldMap/Coal.svg")));
      UpointsCategoryProto upointsCategory4 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.World.QuartzMine, "Assets/Base/Icons/WorldMap/Quartz.svg", (Option<UpointsStatsCategoryProto>) statsCategory));
      ProductProto orThrow7 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Quartz);
      protosDb.Add<WorldMapMineProto>(new WorldMapMineProto(Ids.World.QuartzMine, Proto.CreateStr((Proto.ID) Ids.World.QuartzMine, "Quartz mine", LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.World.QuartzMine.Value, locStr1.Format(orThrow7.Strings.Name))), new ProductQuantity(orThrow7, 8.Quantity()), 20.Seconds(), 0.4.Upoints(), upointsCategory4, new EntityCosts(new AssetValue(cp3.WithQuantity(300)), 8), new Func<int, EntityCosts>(quartzMineCostFunc), 20, new Quantity?(1000000.Quantity()), new WorldMapEntityProto.Gfx(orThrow7.Graphics.IconPath, "Assets/Base/Icons/WorldMap/Quartz.svg")));
      UpointsCategoryProto upointsCategory5 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.World.UraniumMine, "Assets/Base/Icons/WorldMap/Uranium.svg", (Option<UpointsStatsCategoryProto>) statsCategory));
      ProductProto orThrow8 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.UraniumOre);
      protosDb.Add<WorldMapMineProto>(new WorldMapMineProto(Ids.World.UraniumMine, Proto.CreateStr((Proto.ID) Ids.World.UraniumMine, "Uranium mine", LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.World.UraniumMine.Value, locStr1.Format(orThrow8.Strings.Name))), new ProductQuantity(orThrow8, 3.Quantity()), 20.Seconds(), 0.4.Upoints(), upointsCategory5, new EntityCosts(new AssetValue(cp3.WithQuantity(400)), 8), new Func<int, EntityCosts>(uraniumMineCostFunc), 20, new Quantity?(400000.Quantity()), new WorldMapEntityProto.Gfx(orThrow8.Graphics.IconPath, "Assets/Base/Icons/WorldMap/Uranium.svg")));
      UpointsCategoryProto upointsCategory6 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.World.RockMine, "Assets/Base/Icons/WorldMap/Bauxite.svg", (Option<UpointsStatsCategoryProto>) statsCategory));
      ProductProto orThrow9 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Rock);
      protosDb.Add<WorldMapMineProto>(new WorldMapMineProto(Ids.World.RockMine, Proto.CreateStr((Proto.ID) Ids.World.RockMine, "Rock mine", LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.World.RockMine.Value, locStr1.Format(orThrow9.Strings.Name))), new ProductQuantity(orThrow9, 9.Quantity()), 20.Seconds(), 0.2.Upoints(), upointsCategory6, new EntityCosts(new AssetValue(cp3.WithQuantity(200)), 8), new Func<int, EntityCosts>(rockMineCostFunc), 32, new Quantity?(), new WorldMapEntityProto.Gfx(orThrow9.Graphics.IconPath, "Assets/Base/Icons/WorldMap/Rock.svg")));
      UpointsCategoryProto upointsCategory7 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.World.LimestoneMine, "Assets/Base/Icons/WorldMap/Bauxite.svg", (Option<UpointsStatsCategoryProto>) statsCategory));
      ProductProto orThrow10 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Limestone);
      protosDb.Add<WorldMapMineProto>(new WorldMapMineProto(Ids.World.LimestoneMine, Proto.CreateStr((Proto.ID) Ids.World.LimestoneMine, "Limestone quarry", LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.World.LimestoneMine.Value, locStr1.Format(orThrow10.Strings.Name))), new ProductQuantity(orThrow10, 6.Quantity()), 20.Seconds(), 0.4.Upoints(), upointsCategory7, new EntityCosts(new AssetValue(cp3.WithQuantity(200)), 8), new Func<int, EntityCosts>(limestoneMineCostFunc), 16, new Quantity?(400000.Quantity()), new WorldMapEntityProto.Gfx(orThrow10.Graphics.IconPath, "Assets/Base/Icons/WorldMap/Limestone.svg")));
      Proto.Str settlementStr = Proto.CreateStr(new Proto.ID("SettlementSmall1"), "Settlement");
      addSettlement(Ids.World.Settlement1, 100.Percent(), ImmutableArray.Create<QuickTradePairProto>(createTrade(Ids.Products.Wood, 20, Ids.Products.Bricks, 20, 1), createTrade(Ids.Products.Iron, 20, Ids.Products.Bricks, 20, 1), createTrade(Ids.Products.Wood, 40, Ids.Products.ConcreteSlab, 40, 1), createTrade(Ids.Products.Wood, 40, Ids.Products.IronScrap, 48, 1), createTrade(Ids.Products.ConstructionParts, 20, Ids.Products.Potato, 20, 1), createTrade(Ids.Products.ConstructionParts, 40, Ids.Products.Copper, 40, 1), createTrade(Ids.Products.ConstructionParts, 40, Ids.Products.Rubber, 40, 1), createTrade(Ids.Products.Diesel, 60, Ids.Products.Coal, 80, 1), createTrade(Ids.Products.Coal, 80, Ids.Products.Diesel, 40, 1)), ImmutableArray<ContractProto>.Empty, ImmutableArray.Create<WorldMapVillageProto.ProductToLend>(toLend(Ids.Products.Diesel, true), toLend(Ids.Products.Coal, true), toLend(Ids.Products.Bricks, true), toLend(Ids.Products.ConstructionParts, true), toLend(Ids.Products.Copper), toLend(Ids.Products.Iron), toLend(Ids.Products.Potato, true), toLend(Ids.Products.Rubber), toLend(Ids.Products.Wood)), 1);
      addSettlement(Ids.World.Settlement2, 100.Percent(), ImmutableArray.Create<QuickTradePairProto>(createTrade(Ids.Products.ConstructionParts2, 10, Ids.Products.Diesel, 40, 0), createTrade(Ids.Products.Rubber, 20, Ids.Products.Wood, 40, 0), createTrade(Ids.Products.ConstructionParts, 20, Ids.Products.Coal, 60, 0), createTrade(Ids.Products.ConstructionParts3, 20, Ids.Products.Glass, 40, 1), createTrade(Ids.Products.ConstructionParts2, 20, Ids.Products.Chicken, 30, 2, ignoreTradeMultipliers: true)), ImmutableArray.Create<ContractProto>(createContract(Ids.Products.Cement, 10, Ids.Products.Coal, 43, 0.11, 0.2, 2), createContract(Ids.Products.HouseholdGoods, 10, Ids.Products.Coal, 28, 0.1, 0.2, 2), createContract(Ids.Products.Rubber, 10, Ids.Products.Wood, 5, 0.11, 0.2, 2), createContract(Ids.Products.HouseholdAppliances, 10, Ids.Products.Wood, 77, 0.11, 0.2, 3), createContract(Ids.Products.LabEquipment2, 10, Ids.Products.CopperOre, 49, 0.13, 0.2, 2)), ImmutableArray.Create<WorldMapVillageProto.ProductToLend>(toLend(Ids.Products.Glass), toLend(Ids.Products.Steel), toLend(Ids.Products.ConcreteSlab), toLend(Ids.Products.ConstructionParts2), toLend(Ids.Products.ConstructionParts3), toLend(Ids.Products.Electronics)));
      addSettlement(Ids.World.Settlement3, 120.Percent(), ImmutableArray.Create<QuickTradePairProto>(createTrade(Ids.Products.Diesel, 100, Ids.Products.Bread, 30, 1), createTrade(Ids.Products.Coal, 20, Ids.Products.Sulfur, 40, 1), createTrade(Ids.Products.Electronics, 60, Ids.Products.IronOre, 40, 1), createTrade(Ids.Products.ConstructionParts3, 80, Ids.Products.Electronics2, 40, 2), createTrade(Ids.Products.ConstructionParts3, 60, Ids.Products.MedicalSupplies, 20, 2)), ImmutableArray.Create<ContractProto>(createContract(Ids.Products.ConstructionParts2, 10, Ids.Products.Limestone, 90, 0.12, 0.2, 2), createContract(Ids.Products.VehicleParts2, 10, Ids.Products.IronOre, 49, 0.11, 0.3, 2), createContract(Ids.Products.HouseholdAppliances, 10, Ids.Products.CopperOre, 54, 0.13, 0.3, 2), createContract(Ids.Products.Slag, 540, Ids.Products.SourWater, 218, 0.2, 0.4, 3)), ImmutableArray.Create<WorldMapVillageProto.ProductToLend>(toLend(Ids.Products.MedicalSupplies), toLend(Ids.Products.Electronics2), toLend(Ids.Products.PolySilicon), toLend(Ids.Products.Plastic)));
      addSettlement(Ids.World.Settlement4, 150.Percent(), ImmutableArray.Create<QuickTradePairProto>(createTrade(Ids.Products.Gold, 80, Ids.Products.Microchips, 20, 2)), ImmutableArray.Create<ContractProto>(createContract(Ids.Products.Diesel, 100, Ids.Products.Gold, 8, 0.16, 0.4, 3), createContract(Ids.Products.Coal, 120, Ids.Products.Quartz, 120, 0.14, 0.3, 2), createContract(Ids.Products.VehicleParts2, 10, Ids.Products.Quartz, 49, 0.14, 0.3, 2), createContract(Ids.Products.Sulfur, 10, Ids.Products.Sludge, 45, 0.1, 0.1, 2)), (ImmutableArray<WorldMapVillageProto.ProductToLend>) ImmutableArray.Empty);
      addSettlement(Ids.World.SettlementForFuel, 150.Percent(), (ImmutableArray<QuickTradePairProto>) ImmutableArray.Empty, ImmutableArray.Create<ContractProto>(createContract(Ids.Products.FoodPack, 48, Ids.Products.CrudeOil, 200, 0.15, 0.3, 1), createContract(Ids.Products.VehicleParts2, 10, Ids.Products.CrudeOil, 110, 0.15, 0.3, 2), createContract(Ids.Products.Gold, 10, Ids.Products.CrudeOil, 189, 0.17, 0.4, 3), createContract(Ids.Products.ConsumerElectronics, 10, Ids.Products.CrudeOil, 347, 0.17, 0.4, 3)), ImmutableArray.Create<WorldMapVillageProto.ProductToLend>(toLend(Ids.Products.CrudeOil), toLend(Ids.Products.FuelGas), toLend(Ids.Products.Hydrogen), toLend(Ids.Products.Gold)), popsAdoptionEnabled: false);
      addSettlement(Ids.World.SettlementForUranium, 150.Percent(), (ImmutableArray<QuickTradePairProto>) ImmutableArray.Empty, ImmutableArray.Create<ContractProto>(createContract(Ids.Products.FoodPack, 48, Ids.Products.UraniumOre, 16, 0.3, 0.3, 1), createContract(Ids.Products.Gold, 10, Ids.Products.UraniumOre, 16, 0.4, 0.4, 2), createContract(Ids.Products.LabEquipment4, 10, Ids.Products.UraniumOre, 58, 0.4, 0.4, 3)), (ImmutableArray<WorldMapVillageProto.ProductToLend>) ImmutableArray.Empty, popsAdoptionEnabled: false);
      addSettlement(Ids.World.Settlement5, 200.Percent(), ImmutableArray.Create<QuickTradePairProto>(createTrade(Ids.Products.Electronics, 40, Ids.Products.Quartz, 40, 1)), ImmutableArray.Create<ContractProto>(createContract(Ids.Products.Server, 10, Ids.Products.IronOre, 386, 0.12, 0.3, 1), createContract(Ids.Products.MedicalSupplies3, 10, Ids.Products.CopperOre, 36, 0.12, 0.2, 2), createContract(Ids.Products.LabEquipment3, 10, Ids.Products.Coal, 216, 0.12, 0.2, 2), createContract(Ids.Products.SolarCell, 10, Ids.Products.Quartz, 41, 0.12, 0.3, 2), createContract(Ids.Products.ConsumerElectronics, 10, Ids.Products.Quartz, 216, 0.12, 0.3, 3), createContract(Ids.Products.Server, 10, Ids.Products.GoldOre, 386, 0.12, 0.3, 3)), ImmutableArray.Create<WorldMapVillageProto.ProductToLend>(toLend(Ids.Products.IronOre), toLend(Ids.Products.CopperOre), toLend(Ids.Products.Quartz)), popsAdoptionEnabled: false);
      ProductProto orThrow11 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.CargoShip);
      ProductProto orThrow12 = protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.ConstructionParts3);
      Quantity quantity = 600.Quantity();
      addSettlement(Ids.World.SettlementForShips, 100.Percent(), ImmutableArray.Create<QuickTradePairProto>(protosDb.Add<QuickTradePairProto>(new QuickTradePairProto(new Proto.ID("Trade_" + orThrow11.Id.Value + "_For_" + orThrow12.Id.Value), orThrow11.WithQuantity(1), orThrow12.WithQuantity(quantity), 20.Upoints(), 40, 0, 1, 15.Years(), 120.Percent(), 120.Percent(), true))), (ImmutableArray<ContractProto>) ImmutableArray.Empty, (ImmutableArray<WorldMapVillageProto.ProductToLend>) ImmutableArray.Empty, popsAdoptionEnabled: false);
      Proto.Str cargoShipStr = Proto.CreateStr((Proto.ID) Ids.World.CargoShipWreckCost1, "Damaged cargo ship");
      addShipWreck(Ids.World.CargoShipWreckCost1, new AssetValue(orThrow1.WithQuantity(240)));
      addShipWreck(Ids.World.CargoShipWreckCost2, new AssetValue(orThrow2.WithQuantity(300), orThrow3.WithQuantity(200)));

      QuickTradePairProto createTrade(
        ProductProto.ID productToPayWith,
        int quantityToPayWith,
        ProductProto.ID productToBuy,
        int quantityToBuy,
        int minReputation,
        int maxSteps = 16,
        bool ignoreTradeMultipliers = false)
      {
        return protosDb.Add<QuickTradePairProto>(new QuickTradePairProto(new Proto.ID("Trade_" + productToBuy.Value + "_For_" + productToPayWith.Value), protosDb.GetOrThrow<ProductProto>((Proto.ID) productToBuy).WithQuantity(quantityToBuy), protosDb.GetOrThrow<ProductProto>((Proto.ID) productToPayWith).WithQuantity(quantityToPayWith), 0.5.Upoints(), maxSteps, minReputation, 2, 5.Minutes(), 125.Percent(), 100.Percent(), ignoreTradeMultipliers));
      }

      ContractProto createContract(
        ProductProto.ID productToPayWith,
        int quantityToPayWith,
        ProductProto.ID productToBuy,
        int quantityToBuy,
        double per100Quantity,
        double perMonth,
        int minReputation)
      {
        return protosDb.Add<ContractProto>(new ContractProto(new Proto.ID("Contract_" + productToBuy.Value + "_For_" + productToPayWith.Value), protosDb.GetOrThrow<ProductProto>((Proto.ID) productToBuy).WithQuantity(quantityToBuy), protosDb.GetOrThrow<ProductProto>((Proto.ID) productToPayWith).WithQuantity(quantityToPayWith), perMonth.Upoints(), per100Quantity.Upoints(), minReputation));
      }

      EntityCosts oilRigCostFunc(int level)
      {
        return new EntityCosts(level > 4 ? new AssetValue(cp3.WithQuantity(100 + (level - 3) * 100)) : new AssetValue(cp2.WithQuantity(200)), workers: level * 18, maintenance: new MaintenanceCosts?(new MaintenanceCosts(maintenanceT1, level * 8.Quantity())));
      }

      WorldMapMineProto addOilRig(EntityProto.ID id, AssetValue costToFix, int quantityAvailable)
      {
        return protosDb.Add<WorldMapMineProto>(new WorldMapMineProto(id, oilRigStr, new ProductQuantity(oil, 9.Quantity()), 20.Seconds(), 0.4.Upoints(), oilRigUpointsCategory, new EntityCosts(costToFix, 8), new Func<int, EntityCosts>(oilRigCostFunc), 16, new Quantity?(quantityAvailable.Quantity()), new WorldMapEntityProto.Gfx("Assets/Unity/UserInterface/WorldMap/OilRigBig.svg", "Assets/Base/Icons/WorldMap/OilRig.svg")));
      }

      EntityCosts waterWellCostFunc(int level)
      {
        return new EntityCosts(new AssetValue(cp3.WithQuantity(80 + (level - 1).Max(0) * 40)), workers: level * 16, maintenance: new MaintenanceCosts?(new MaintenanceCosts(maintenanceT1, level * 4.Quantity())));
      }

      EntityCosts sulfurMineCostFunc(int level)
      {
        return new EntityCosts(new AssetValue(cp3.WithQuantity(80 + (level - 1).Max(0) * 80)), workers: level * 12, maintenance: new MaintenanceCosts?(new MaintenanceCosts(maintenanceT1, level * 10.Quantity())));
      }

      EntityCosts coalMineCostFunc(int level)
      {
        return new EntityCosts(new AssetValue(cp3.WithQuantity(100 + (level - 1).Max(0) * 100)), workers: level * 25, maintenance: new MaintenanceCosts?(new MaintenanceCosts(maintenanceT1, level * 12.Quantity())));
      }

      EntityCosts quartzMineCostFunc(int level)
      {
        return new EntityCosts(new AssetValue(cp3.WithQuantity(120 + (level - 1).Max(0) * 60)), workers: level * 25, maintenance: new MaintenanceCosts?(new MaintenanceCosts(maintenanceT1, level * 12.Quantity())));
      }

      EntityCosts uraniumMineCostFunc(int level)
      {
        return new EntityCosts(new AssetValue(cp3.WithQuantity(200 + (level - 1).Max(0) * 80)), workers: level * 25, maintenance: new MaintenanceCosts?(new MaintenanceCosts(maintenanceT1, level * 10.Quantity())));
      }

      EntityCosts rockMineCostFunc(int level)
      {
        return new EntityCosts(new AssetValue(cp3.WithQuantity(80 + (level - 1).Max(0) * 40)), workers: level * 25, maintenance: new MaintenanceCosts?(new MaintenanceCosts(maintenanceT1, level * 8.Quantity())));
      }

      EntityCosts limestoneMineCostFunc(int level)
      {
        return new EntityCosts(new AssetValue(cp3.WithQuantity(100 + (level - 1).Max(0) * 60)), workers: level * 25, maintenance: new MaintenanceCosts?(new MaintenanceCosts(maintenanceT1, level * 10.Quantity())));
      }

      WorldMapVillageProto.ProductToLend toLend(ProductProto.ID productId, bool borrowFromStart = false)
      {
        return new WorldMapVillageProto.ProductToLend(protosDb.GetOrThrow<ProductProto>((Proto.ID) productId), borrowFromStart);
      }

      void addSettlement(
        EntityProto.ID id,
        Percent befriendCostMult,
        ImmutableArray<QuickTradePairProto> quickTrades,
        ImmutableArray<ContractProto> contracts,
        ImmutableArray<WorldMapVillageProto.ProductToLend> productsToLend,
        int startingReputation = 0,
        bool popsAdoptionEnabled = true)
      {
        ProtosDb protosDb = protosDb;
        EntityProto.ID id1 = id;
        Proto.Str strings = settlementStr;
        int minReputationNeededToAdopt = popsAdoptionEnabled ? 1 : -1;
        int startingReputation1 = startingReputation;
        Func<int, AssetValue> func = new Func<int, AssetValue>(costPerReputation);
        Upoints upointsPerPopToAdopt = 0.25.Upoints();
        Func<int, AssetValue> costPerLevel = func;
        ImmutableArray<QuickTradePairProto> quickTrades1 = quickTrades;
        ImmutableArray<ContractProto> contracts1 = contracts;
        ImmutableArray<WorldMapVillageProto.ProductToLend> productsToLend1 = productsToLend;
        WorldMapEntityProto.Gfx graphics = new WorldMapEntityProto.Gfx("Assets/Unity/UserInterface/WorldMap/VillageBig.svg", "Assets/Unity/UserInterface/WorldMap/Village.svg");
        WorldMapVillageProto proto = new WorldMapVillageProto(id1, strings, minReputationNeededToAdopt, startingReputation1, upointsPerPopToAdopt, costPerLevel, quickTrades1, contracts1, productsToLend1, graphics);
        protosDb.Add<WorldMapVillageProto>(proto);

        AssetValue costPerReputation(int level)
        {
          if (level <= 1)
            return new AssetValue(cp2.WithQuantity(80)).ScaledBy(befriendCostMult);
          if (level == 2)
            return new AssetValue(cp2.WithQuantity(120)).ScaledBy(befriendCostMult);
          return level == 3 ? new AssetValue(cp3.WithQuantity(100)).ScaledBy(befriendCostMult) : new AssetValue(cp3.WithQuantity(140)).ScaledBy(befriendCostMult);
        }
      }

      void addShipWreck(EntityProto.ID id, AssetValue costToFix)
      {
        protosDb.Add<WorldMapCargoShipWreckProto>(new WorldMapCargoShipWreckProto(id, cargoShipStr, costToFix, new WorldMapEntityProto.Gfx("Assets/Unity/UserInterface/WorldMap/CargoShipStoryIcon256.png", "Assets/Unity/UserInterface/Toolbar/CargoShip.svg")));
      }
    }

    public WorldMapEntitiesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
