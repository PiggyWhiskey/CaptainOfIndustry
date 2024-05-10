// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Cargo.CargoDepotsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes.Buildings;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Cargo
{
  internal class CargoDepotsData : IModData
  {
    private static readonly RelTile2i MODULE_ORIGIN;

    public void RegisterData(ProtoRegistrator registrator)
    {
      this.registerModules(registrator);
      EntityLayoutParams layoutParams = new EntityLayoutParams((Predicate<LayoutTile>) (x => x.Constraint == LayoutTileConstraint.None || x.Constraint.HasAnyConstraints(LayoutTileConstraint.Ocean)), (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[3]
      {
        new CustomLayoutToken("~0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-10, h, LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("{0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? nullable = new int?(0);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = nullable;
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(-10, heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("~~~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-12, -10, LayoutTileConstraint.Ocean)))
      });
      LocStr locStr = Loc.Str("CargoDepotBase__desc", "Once built, a repaired cargo ship can dock here and transfer its cargo via attached cargo depot modules.", "cargo depot description");
      string[] array = Enumerable.Repeat<string>("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ", 2).Concat<string>((IEnumerable<string>) new string[1]
      {
        "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)<@A"
      }).Concat<string>(Enumerable.Repeat<string>("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ", 2)).ToArray<string>();
      CargoDepotProto nextTier1 = registrator.CargoDepotProtoBuilder.Start("Cargo depot (8)", Ids.Buildings.CargoDepotT4).SetCost(Costs.Buildings.CargoDepotT4).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotT4.ToString() + "__desc", locStr.TranslatedString)).SetSlots(CargoDepotsData.MODULE_ORIGIN, 5, 9, 8).SetReservedOceanAreasSets(ShipyardData.AllApproachesAreas, ShipyardData.AllApproachesAnimationPrefabs).SetCargoShipProtoId(Ids.Ships.CargoShipT4).SetCategories(Ids.ToolbarCategories.Docks).SetLayout(layoutParams, ((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                   ", false)).Concat<string>(Enumerable.Repeat<string>("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ", 40)).Concat<string>((IEnumerable<string>) array).Concat<string>((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                   ", true)).ToArray<string>()).SetPrefabPath("Assets/Base/Buildings/CargoDepot/CargoDepot_BaseT3.prefab").BuildAndAdd();
      nextTier1.AddParam((IProtoParam) new DrawArrowWileBuildingProtoParam(6f));
      registrator.CargoDepotProtoBuilder.Start("Cargo depot (6)", Ids.Buildings.CargoDepotT3).SetAsLockedOnInit().SetCost(Costs.Buildings.CargoDepotT3).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotT3.ToString() + "__desc", locStr.TranslatedString)).SetSlots(CargoDepotsData.MODULE_ORIGIN, 5, 9, 6).SetReservedOceanAreasSets(ShipyardData.AllApproachesAreas, ShipyardData.AllApproachesAnimationPrefabs).SetPrefabOffset(new RelTile3f((Fix32) 0, (Fix32) 5, (Fix32) 0)).SetNextTier(nextTier1).SetCargoShipProtoId(Ids.Ships.CargoShipT3).SetCategories(Ids.ToolbarCategories.Docks).SetLayout(layoutParams, ((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                   ", false)).Concat<string>(Enumerable.Repeat<string>("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ", 40)).Concat<string>((IEnumerable<string>) array).Concat<string>((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                   ", true)).ToArray<string>()).SetPrefabPath("Assets/Base/Buildings/CargoDepot/CargoDepot_BaseT2.prefab").BuildAndAdd().AddParam((IProtoParam) new DrawArrowWileBuildingProtoParam(6f));
      CargoDepotProto nextTier2 = registrator.CargoDepotProtoBuilder.Start("Cargo depot (4)", Ids.Buildings.CargoDepotT2).SetCost(Costs.Buildings.CargoDepotT2).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotT2.ToString() + "__desc", locStr.TranslatedString)).SetSlots(CargoDepotsData.MODULE_ORIGIN, 5, 9, 4).SetReservedOceanAreasSets(ShipyardData.AllApproachesAreas, ShipyardData.AllApproachesAnimationPrefabs).SetCargoShipProtoId(Ids.Ships.CargoShipT2).SetCategories(Ids.ToolbarCategories.Docks).SetLayout(layoutParams, ((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                   ", false)).Concat<string>(Enumerable.Repeat<string>("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ", 20)).Concat<string>((IEnumerable<string>) array).Concat<string>((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                   ", true)).ToArray<string>()).SetPrefabPath("Assets/Base/Buildings/CargoDepot/CargoDepot_BaseT1.prefab").BuildAndAdd();
      nextTier2.AddParam((IProtoParam) new DrawArrowWileBuildingProtoParam(6f));
      registrator.CargoDepotProtoBuilder.Start("Cargo depot (2)", Ids.Buildings.CargoDepotT1).SetAsLockedOnInit().SetCost(Costs.Buildings.CargoDepotT1).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotT1.ToString() + "__desc", locStr.TranslatedString)).SetSlots(CargoDepotsData.MODULE_ORIGIN, 5, 9, 2).SetReservedOceanAreasSets(ShipyardData.AllApproachesAreas, ShipyardData.AllApproachesAnimationPrefabs).SetPrefabOffset(new RelTile3f((Fix32) 0, (Fix32) 5, (Fix32) 0)).SetNextTier(nextTier2).SetCargoShipProtoId(Ids.Ships.CargoShipT1).SetCategories(Ids.ToolbarCategories.Docks).SetLayout(layoutParams, ((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                   ", false)).Concat<string>(Enumerable.Repeat<string>("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ", 20)).Concat<string>((IEnumerable<string>) array).Concat<string>((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                   ", true)).ToArray<string>()).SetPrefabPath("Assets/Base/Buildings/CargoDepot/CargoDepot_BaseT0.prefab").BuildAndAdd().AddParam((IProtoParam) new DrawArrowWileBuildingProtoParam(6f));
    }

    private void registerModules(ProtoRegistrator registrator)
    {
      CargoDepotModuleProtoBuilder moduleProtoBuilder = registrator.CargoDepotModuleProtoBuilder;
      LocStr locStr1 = Loc.Str("CargoModuleUnitCommon__desc", "Cargo depot module for transferring units of products (such as construction parts). It can be built in any empty slot of a cargo depot.", "cargo depot module description");
      LocStr1 locStr1_1 = Loc.Str1("CargoModuleUnit__descUpgraded", "Cargo depot module for transferring units of products (such as construction parts). It can be built in any empty slot of a cargo depot. Unloads cargo {0} faster comparing to the basic module.", "cargo depot module description, {0} is e.g. '50%'");
      Percent percentOfAnimationToDropCargoToShip1 = 85.Percent();
      moduleProtoBuilder.Start("Unit module (L)", Ids.Buildings.CargoDepotModuleUnitT3).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotModuleUnitT3.ToString() + "__desc", locStr1_1.Format("300%").Value)).SetLayout("[3][3][3][3][3][3][3][3][3][3]   ", "[4][4][4][4][4][4][4][4][4][4]+#X", "[4][4][4][4][4][4][4][4][4][4]+#Y", "[4][4][4][4][4][4][4][4][4][4]+#Z", "[3][3][3][3][3][3][3][3][3][3]   ").SetCapacity(720).SetExchangeParams(60, 10.Seconds()).SetProductType(CountableProductProto.ProductType).SetPowerConsumedByCrane(250.Kw()).SetCost(Costs.Buildings.CargoModuleUnitT3).SetPrefabPath("Assets/Base/Buildings/CargoDepot/Module_Countable_T3.prefab").SetCraneGraphics("Assets/Base/Buildings/CargoDepot/Crane_Countable.prefab", percentOfAnimationToDropCargoToShip1).SetCategories(Ids.ToolbarCategories.Docks).BuildAndAdd();
      moduleProtoBuilder.Start("Unit module (M)", Ids.Buildings.CargoDepotModuleUnitT2).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotModuleUnitT2.ToString() + "__desc", locStr1_1.Format("100%").Value)).SetLayout("[3][3][3][3][3][3][3][3][3][3]   ", "[4][4][4][4][4][4][4][4][4][4]+#X", "[4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4]+#Y", "[3][3][3][3][3][3][3][3][3][3]   ").SetCapacity(360).SetExchangeParams(36, 12.Seconds()).SetProductType(CountableProductProto.ProductType).SetPowerConsumedByCrane(200.Kw()).SetCost(Costs.Buildings.CargoModuleUnitT2).SetNextTier(Ids.Buildings.CargoDepotModuleUnitT3).SetPrefabPath("Assets/Base/Buildings/CargoDepot/Module_Countable_T2.prefab").SetCraneGraphics("Assets/Base/Buildings/CargoDepot/Crane_Countable.prefab", percentOfAnimationToDropCargoToShip1).SetCategories(Ids.ToolbarCategories.Docks).BuildAndAdd();
      moduleProtoBuilder.Start("Unit module (S)", Ids.Buildings.CargoDepotModuleUnitT1).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotModuleUnitT1.ToString() + "__desc", locStr1.TranslatedString)).SetLayout("[3][3][3][3][3][3][3][3][3][3]   ", "[4][4][4][4][4][4][4][4][4][4]+#X", "[4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4]+#Y", "[3][3][3][3][3][3][3][3][3][3]   ").SetCapacity(180).SetExchangeParams(24, 16.Seconds()).SetProductType(CountableProductProto.ProductType).SetPowerConsumedByCrane(150.Kw()).SetCost(Costs.Buildings.CargoModuleUnitT1).SetNextTier(Ids.Buildings.CargoDepotModuleUnitT2).SetPrefabPath("Assets/Base/Buildings/CargoDepot/Module_Countable_T1.prefab").SetCraneGraphics("Assets/Base/Buildings/CargoDepot/Crane_Countable.prefab", percentOfAnimationToDropCargoToShip1).SetCategories(Ids.ToolbarCategories.Docks).BuildAndAdd();
      LocStr locStr2 = Loc.Str("CargoModuleLooseCommon__desc", "Cargo depot module for transferring loose materials (such as coal). It can be built in any empty slot of a cargo depot.", "cargo depot module description");
      LocStr1 locStr1_2 = Loc.Str1("CargoModuleLoose__descUpgraded", "Cargo depot module for transferring loose materials (such as coal). It can be built in any empty slot of a cargo depot. Unloads cargo {0} faster comparing to the basic module.", "cargo depot module description, {0} is e.g. '50%'");
      Percent percentOfAnimationToDropCargoToShip2 = 81.Percent();
      moduleProtoBuilder.Start("Loose module (L)", Ids.Buildings.CargoDepotModuleLooseT3).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotModuleLooseT3.ToString() + "__desc", locStr1_2.Format("300%").Value)).SetLayout("[4][4][4][4][4][4][4][4][5][4]   ", "[4][4][4][4][4][4][4][4][6][5]+~X", "[4][4][4][4][4][4][4][4][6][5]+~Y", "[4][4][4][4][4][4][4][4][6][5]+~Z", "[4][4][4][4][4][4][4][4][5][4]   ").SetCapacity(720).SetExchangeParams(60, 10.Seconds()).SetProductType(LooseProductProto.ProductType).SetPowerConsumedByCrane(250.Kw()).SetCost(Costs.Buildings.CargoModuleLooseT3).SetPrefabPath("Assets/Base/Buildings/CargoDepot/Module_Loose_T3.prefab").SetCraneGraphics("Assets/Base/Buildings/CargoDepot/Crane_Loose.prefab", percentOfAnimationToDropCargoToShip2).SetCategories(Ids.ToolbarCategories.Docks).BuildAndAdd();
      moduleProtoBuilder.Start("Loose module (M)", Ids.Buildings.CargoDepotModuleLooseT2).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotModuleLooseT2.ToString() + "__desc", locStr1_2.Format("100%").Value)).SetLayout("[4][4][4][4][4][4][4][4][5][4]   ", "[4][4][4][4][4][4][4][4][6][5]+~X", "[4][4][4][4][4][4][4][4][6][6]   ", "[4][4][4][4][4][4][4][4][6][5]+~Y", "[4][4][4][4][4][4][4][4][5][4]   ").SetCapacity(360).SetExchangeParams(36, 12.Seconds()).SetProductType(LooseProductProto.ProductType).SetPowerConsumedByCrane(200.Kw()).SetCost(Costs.Buildings.CargoModuleLooseT2).SetNextTier(Ids.Buildings.CargoDepotModuleLooseT3).SetPrefabPath("Assets/Base/Buildings/CargoDepot/Module_Loose_T2.prefab").SetCraneGraphics("Assets/Base/Buildings/CargoDepot/Crane_Loose.prefab", percentOfAnimationToDropCargoToShip2).SetCategories(Ids.ToolbarCategories.Docks).BuildAndAdd();
      moduleProtoBuilder.Start("Loose module (S)", Ids.Buildings.CargoDepotModuleLooseT1).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotModuleLooseT1.ToString() + "__desc", locStr2.TranslatedString)).SetLayout("[4][4][4][4][4][4][4][4][5][4]   ", "[4][4][4][4][4][4][4][4][6][5]+~X", "[4][4][4][4][4][4][4][4][6][6]   ", "[4][4][4][4][4][4][4][4][6][5]+~Y", "[4][4][4][4][4][4][4][4][5][4]   ").SetCapacity(180).SetExchangeParams(24, 16.Seconds()).SetProductType(LooseProductProto.ProductType).SetPowerConsumedByCrane(150.Kw()).SetCost(Costs.Buildings.CargoModuleLooseT1).SetNextTier(Ids.Buildings.CargoDepotModuleLooseT2).SetPrefabPath("Assets/Base/Buildings/CargoDepot/Module_Loose_T1.prefab").SetCraneGraphics("Assets/Base/Buildings/CargoDepot/Crane_Loose.prefab", percentOfAnimationToDropCargoToShip2).SetCategories(Ids.ToolbarCategories.Docks).BuildAndAdd();
      LocStr locStr3 = Loc.Str("CargoModuleFluidCommon__desc", "Cargo depot module for transferring fluid materials (such as oil). It can be built in any empty slot of a cargo depot.", "cargo depot module description");
      LocStr1 locStr1_3 = Loc.Str1("CargoModuleFluid__descUpgraded", "Cargo depot module for transferring fluid materials (such as oil). It can be built in any empty slot of a cargo depot. Unloads cargo {0} faster comparing to the basic module.", "cargo depot module description, {0} is e.g. '50%'");
      moduleProtoBuilder.Start("Fluid module (L)", Ids.Buildings.CargoDepotModuleFluidT3).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotModuleFluidT3.ToString() + "__desc", locStr1_3.Format("300%").Value)).SetLayout("[5][5][5][5][5][5][5][5][5][4]   ", "[5][5][5][5][5][5][5][5][6][4]+@X", "[5][5][5][5][5][5][5][5][6][4]+@Y", "[5][5][5][5][5][5][5][5][6][4]+@Z", "[5][5][5][5][5][5][5][5][5][4]   ").SetCapacity(880).SetExchangeParams(22, 3.Seconds()).SetProductType(FluidProductProto.ProductType).SetPowerConsumedByCrane(250.Kw()).SetCost(Costs.Buildings.CargoModuleFluidT3).SetPrefabPath("Assets/Base/Buildings/CargoDepot/Module_Gas_T3.prefab").SetPumpCraneGraphics("Assets/Base/Buildings/CargoDepot/Crane_Liquid.prefab").SetCategories(Ids.ToolbarCategories.Docks).BuildAndAdd();
      moduleProtoBuilder.Start("Fluid module (M)", Ids.Buildings.CargoDepotModuleFluidT2).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotModuleFluidT2.ToString() + "__desc", locStr1_3.Format("100%").Value)).SetLayout("[5][5][5][5][5][5][5][5][5][4]   ", "[5][5][5][5][5][5][5][5][6][4]+@X", "[5][5][5][5][5][5][5][5][6][6]   ", "[5][5][5][5][5][5][5][5][6][4]+@Y", "[5][5][5][5][5][5][5][5][5][4]   ").SetCapacity(440).SetExchangeParams(11, 3.Seconds()).SetProductType(FluidProductProto.ProductType).SetPowerConsumedByCrane(200.Kw()).SetCost(Costs.Buildings.CargoModuleFluidT2).SetNextTier(Ids.Buildings.CargoDepotModuleFluidT3).SetPrefabPath("Assets/Base/Buildings/CargoDepot/Module_Gas_T2.prefab").SetPumpCraneGraphics("Assets/Base/Buildings/CargoDepot/Crane_Liquid.prefab").SetCategories(Ids.ToolbarCategories.Docks).BuildAndAdd();
      moduleProtoBuilder.Start("Fluid module (S)", Ids.Buildings.CargoDepotModuleFluidT1).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CargoDepotModuleFluidT1.ToString() + "__desc", locStr3.TranslatedString)).SetAsLockedOnInit().SetLayout("[5][5][5][5][5][5][5][5][5][4]   ", "[5][5][5][5][5][5][5][5][6][4]+@X", "[5][5][5][5][5][5][5][5][6][6]   ", "[5][5][5][5][5][5][5][5][6][4]+@Y", "[5][5][5][5][5][5][5][5][5][4]   ").SetCapacity(220).SetExchangeParams(11, 6.Seconds()).SetProductType(FluidProductProto.ProductType).SetPowerConsumedByCrane(150.Kw()).SetCost(Costs.Buildings.CargoModuleFluidT1).SetNextTier(Ids.Buildings.CargoDepotModuleFluidT2).SetPrefabPath("Assets/Base/Buildings/CargoDepot/Module_Gas_T1.prefab").SetPumpCraneGraphics("Assets/Base/Buildings/CargoDepot/Crane_Gas.prefab").SetCategories(Ids.ToolbarCategories.Docks).BuildAndAdd();
    }

    public CargoDepotsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static CargoDepotsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      CargoDepotsData.MODULE_ORIGIN = new RelTile2i("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ".Length / 3 - 1, 13);
    }
  }
}
