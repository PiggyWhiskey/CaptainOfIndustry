// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Cargo.CargoShipsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes.Vehicles;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Economy;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Localization;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Cargo
{
  internal class CargoShipsData : IModData
  {
    private static readonly Quantity FUEL_PER_JOURNEY_BASE;
    private static readonly Quantity FUEL_PER_JOURNEY_PER_MODULE;
    private static readonly RelTile2f SHIP_OFFSET;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto orThrow1 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Diesel);
      ProductProto orThrow2 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.HeavyOil);
      ProductProto orThrow3 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Hydrogen);
      ProductProto orThrow4 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Electronics);
      ProductProto orThrow5 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.MechanicalParts);
      ProductProto orThrow6 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Steel);
      TechnologyProto orThrow7 = prototypesDb.GetOrThrow<TechnologyProto>(Ids.Technology.HydrogenCargoShip);
      AssetValue assetValue1 = new AssetValue(orThrow5.WithQuantity(100), orThrow6.WithQuantity(100));
      AssetValue assetValue2 = new AssetValue(orThrow4.WithQuantity(180), orThrow6.WithQuantity(180));
      CargoShipProto.Gfx gfx1 = new CargoShipProto.Gfx("Assets/Base/Ships/CargoShip/CargoShip_Front.prefab", "Assets/Base/Ships/CargoShip/CargoShip_Rear.prefab", "Assets/Base/Ships/CargoShip/CargoShipModule_Empty.prefab", "Assets/Base/Ships/CargoShip/Audio/Engine.prefab", "Assets/Base/Ships/CargoShip/Audio/Arrival.prefab", "Assets/Base/Ships/CargoShip/Audio/Departure.prefab", new RelTile1i(5), RelTile3f.FromDimensionsInMeters(40.0, 22.0, 25.0), "Assets/Base/Icons/Technologies/CargoShipTechnology.png");
      string modulePrefabPath = gfx1.EmptyModulePrefabPath;
      RelTile1i moduleSlotLength1 = gfx1.ModuleSlotLength;
      string engineSoundPath = gfx1.EngineSoundPath;
      string arrivalSoundPath = gfx1.ArrivalSoundPath;
      string departureSoundPath = gfx1.DepartureSoundPath;
      RelTile1i moduleSlotLength2 = moduleSlotLength1;
      RelTile3f basicBoxColliderSize = gfx1.BasicBoxColliderSize;
      string iconPath = gfx1.IconPath;
      CargoShipProto.Gfx gfx2 = new CargoShipProto.Gfx("Assets/Base/Ships/CargoShip/HydrogenFront.prefab", "Assets/Base/Ships/CargoShip/HydrogenRear.prefab", modulePrefabPath, engineSoundPath, arrivalSoundPath, departureSoundPath, moduleSlotLength2, basicBoxColliderSize, iconPath);
      ImmutableArray<CargoShipModuleProto> availableModules = this.registerShipModules(registrator);
      ProductProto fuelProto1 = orThrow1;
      Quantity fuelPerJourneyBase1 = CargoShipsData.FUEL_PER_JOURNEY_BASE;
      Quantity journeyPerModule = CargoShipsData.FUEL_PER_JOURNEY_PER_MODULE;
      Option<Proto> none1 = (Option<Proto>) Option.None;
      ImmutableArray<ProductProto> compatibleFuels1 = ImmutableArray.Create<ProductProto>(orThrow2);
      Percent percent1 = 100.Percent();
      AssetValue cost1 = assetValue1;
      Percent pollutionPercent1 = percent1;
      Option<CargoShipProto.Gfx> none2 = Option<CargoShipProto.Gfx>.None;
      CargoShipProto.FuelData fuelData1 = new CargoShipProto.FuelData(fuelProto1, fuelPerJourneyBase1, journeyPerModule, none1, compatibleFuels1, cost1, pollutionPercent1, none2);
      ProductProto fuelProto2 = orThrow2;
      Quantity fuelPerJourneyBase2 = CargoShipsData.FUEL_PER_JOURNEY_BASE * 4 / 6;
      Quantity fuelPerJourneyPerModule1 = CargoShipsData.FUEL_PER_JOURNEY_PER_MODULE * 4 / 6;
      Option<Proto> lockingProto1 = (Option<Proto>) (Proto) orThrow2;
      ImmutableArray<ProductProto> compatibleFuels2 = ImmutableArray.Create<ProductProto>(orThrow1);
      Percent percent2 = 120.Percent() * 6 / 4;
      AssetValue cost2 = assetValue1;
      Percent pollutionPercent2 = percent2;
      Option<CargoShipProto.Gfx> none3 = Option<CargoShipProto.Gfx>.None;
      CargoShipProto.FuelData fuelData2 = new CargoShipProto.FuelData(fuelProto2, fuelPerJourneyBase2, fuelPerJourneyPerModule1, lockingProto1, compatibleFuels2, cost2, pollutionPercent2, none3);
      ProductProto fuelProto3 = orThrow3;
      Quantity fuelPerJourneyBase3 = CargoShipsData.FUEL_PER_JOURNEY_BASE.ScaledBy(TrucksData.HYDROGEN_DIESEL_ENERGY_RATIO);
      Quantity fuelPerJourneyPerModule2 = CargoShipsData.FUEL_PER_JOURNEY_PER_MODULE.ScaledBy(TrucksData.HYDROGEN_DIESEL_ENERGY_RATIO);
      Option<Proto> lockingProto2 = (Option<Proto>) (Proto) orThrow7;
      ImmutableArray<ProductProto> empty = (ImmutableArray<ProductProto>) ImmutableArray.Empty;
      Percent zero = Percent.Zero;
      AssetValue cost3 = assetValue2;
      Percent pollutionPercent3 = zero;
      Option<CargoShipProto.Gfx> graphics = (Option<CargoShipProto.Gfx>) gfx2;
      CargoShipProto.FuelData fuelData3 = new CargoShipProto.FuelData(fuelProto3, fuelPerJourneyBase3, fuelPerJourneyPerModule2, lockingProto2, empty, cost3, pollutionPercent3, graphics);
      ImmutableArray<CargoShipProto.FuelData> availableFuels = ImmutableArray.Create<CargoShipProto.FuelData>(fuelData1, fuelData2, fuelData3);
      LocStr desc = Loc.Str(Ids.Ships.CargoShipT1.ToString() + "__desc", "The cargo ship departs automatically when there is enough cargo to collect on the world map. Each journey of the ship has a fixed cost of fuel that is based on the size of the ship. Larger ships are more efficient in fuel consumption (they get upgraded automatically with cargo depot upgrade). The ship doesn't need to be assigned to individual mines / oil rigs, it collects cargo automatically. It can transport multiple types of products at the same time and will make sure all of them are supplied (it will depart anytime any of its products need to be transported).", "description of a cargo ship");
      registrator.CargoShipProtoBuilder.Start("Cargo Ship", Ids.Ships.CargoShipT1).Description(desc).SetCost(Costs.CargoShip.CargoShipT1).SetMaximumModulesCount(2).SetDockOffset(CargoShipsData.SHIP_OFFSET).SetAvailableModules(availableModules).SetAvailableFuels(availableFuels).SetGraphics(gfx1).BuildAndAdd();
      registrator.CargoShipProtoBuilder.Start("Cargo Ship", Ids.Ships.CargoShipT2).Description(desc).SetCost(Costs.CargoShip.CargoShipT2).SetMaximumModulesCount(4).SetDockOffset(CargoShipsData.SHIP_OFFSET).SetAvailableModules(availableModules).SetAvailableFuels(availableFuels).SetGraphics(gfx1).BuildAndAdd();
      registrator.CargoShipProtoBuilder.Start("Cargo Ship", Ids.Ships.CargoShipT3).Description(desc).SetCost(Costs.CargoShip.CargoShipT3).SetMaximumModulesCount(6).SetDockOffset(CargoShipsData.SHIP_OFFSET).SetAvailableModules(availableModules).SetAvailableFuels(availableFuels).SetGraphics(gfx1).BuildAndAdd();
      registrator.CargoShipProtoBuilder.Start("Cargo Ship", Ids.Ships.CargoShipT4).Description(desc).SetCost(Costs.CargoShip.CargoShipT4).SetMaximumModulesCount(8).SetDockOffset(CargoShipsData.SHIP_OFFSET).SetAvailableModules(availableModules).SetAvailableFuels(availableFuels).SetGraphics(gfx1).BuildAndAdd();
    }

    private ImmutableArray<CargoShipModuleProto> registerShipModules(ProtoRegistrator registrator)
    {
      CargoShipModuleProto cargoShipModuleProto = registrator.PrototypesDb.Add<CargoShipModuleProto>(new CargoShipModuleProto((Proto.ID) Ids.Ships.CargoShipFluidModule, Proto.Str.Empty, FluidProductProto.ProductType, 440.Quantity(), new CargoShipModuleProto.Gfx("Assets/Base/Ships/CargoShip/CargoShipModule_Gas.prefab")));
      CargoShipLooseModuleProto looseModuleProto = registrator.PrototypesDb.Add<CargoShipLooseModuleProto>(new CargoShipLooseModuleProto((Proto.ID) Ids.Ships.CargoShipLooseModule, Proto.Str.Empty, 360.Quantity(), new CargoShipLooseModuleProto.Gfx("Assets/Base/Ships/CargoShip/CargoShipModule_Loose.prefab", "PileSmooth", "PileRough", new Vector3f((Fix32) 0, (Fix32) 0, -0.1.ToFix32()), new Vector3f((Fix32) 0, (Fix32) 0, 4.5.ToFix32()))));
      int[] source = new int[2]{ 6, 5 };
      ImmutableArrayBuilder<Vector3f> immutableArrayBuilder = new ImmutableArrayBuilder<Vector3f>(((IEnumerable<int>) source).Sum());
      Fix32 fix32_1 = (Fix32) 19;
      Fix32 fix32_2 = 2.5.ToFix32();
      Fix32 fix32_3 = (fix32_1 - 6 * fix32_2) / 8;
      Vector3f vector3f1 = new Vector3f(fix32_2 + fix32_3, (Fix32) 0, (Fix32) 0);
      Fix32 fix32_4 = 2.8.ToFix32();
      Fix32 fix32_5 = 2.5.ToFix32();
      int i = 0;
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        int num = source[index1];
        Fix32 fix32_6 = (6 - num) * (fix32_2 + fix32_3) / 2;
        Vector3f vector3f2 = new Vector3f(-fix32_1 / 2 + fix32_2 / 2 + fix32_6 + fix32_3, (Fix32) 0, fix32_4 + index1 * fix32_5);
        for (int index2 = 0; index2 < num; ++index2)
        {
          immutableArrayBuilder[i] = vector3f2 + index2 * vector3f1;
          ++i;
        }
      }
      CargoShipCountableModuleProto countableModuleProto = registrator.PrototypesDb.Add<CargoShipCountableModuleProto>(new CargoShipCountableModuleProto((Proto.ID) Ids.Ships.CargoShipUnitModule, Proto.Str.Empty, 360.Quantity(), new CargoShipCountableModuleProto.Gfx("Assets/Base/Ships/CargoShip/CargoShipModule_Countable.prefab", "Assets/Base/Ships/CargoShip/CargoShip_Container.prefab", "", immutableArrayBuilder.GetImmutableArrayAndClear())));
      return ImmutableArray.Create<CargoShipModuleProto>(cargoShipModuleProto, (CargoShipModuleProto) looseModuleProto, (CargoShipModuleProto) countableModuleProto);
    }

    public CargoShipsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static CargoShipsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      CargoShipsData.FUEL_PER_JOURNEY_BASE = 60.Quantity();
      CargoShipsData.FUEL_PER_JOURNEY_PER_MODULE = 30.Quantity();
      CargoShipsData.SHIP_OFFSET = new RelTile2f((Fix32) 0, 23.5.ToFix32());
    }
  }
}
