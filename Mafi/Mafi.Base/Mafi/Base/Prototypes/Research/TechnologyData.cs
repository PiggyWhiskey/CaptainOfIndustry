// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Research.TechnologyData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Research
{
  internal class TechnologyData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      string descShort1 = "We need to discover an oil rig in the world map to obtain this technology.";
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.OilDrilling, Proto.CreateStr(Ids.Technology.OilDrilling, "Ocean drilling technology", descShort1), new TechnologyProto.Gfx("Assets/Base/Products/Icons/CrudeOil.svg")));
      string descShort2 = "We need to discover a cargo ship in the world map to understand how to design this building.";
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.CargoShip, Proto.CreateStr(Ids.Technology.CargoShip, "Cargo ship technology", descShort2), new TechnologyProto.Gfx("Assets/Base/Icons/Technologies/CargoShipTechnology.png")));
      LocStr descShort3 = Loc.Str("DiscoverOnWorldMap__desc", "Has to be discovered by exploring the world map", "explains the player what they need to do in order to get access to some technology");
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.ShipRadar, Proto.CreateStr(Ids.Technology.ShipRadar, "Radar system", descShort3), new TechnologyProto.Gfx("Assets/Base/Ships/BattleShip/Icons/Ship2.png")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.ShipRadarT2, Proto.CreateStr(Ids.Technology.ShipRadarT2, "Radar system II", descShort3), new TechnologyProto.Gfx("Assets/Base/Ships/BattleShip/Icons/Ship3.png")));
      LocStr descShort4 = Loc.Str("SeedsTech_Description", "To discover new seeds, explore the world map with your ship.", "explains the player what they need to do in order to get access to seeds - wheat seeds, canola seeds etc.");
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.WheatSeeds, Proto.CreateStr(Ids.Technology.WheatSeeds, "Wheat seeds", descShort4), new TechnologyProto.Gfx("Assets/Base/Products/Icons/Wheat.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.CanolaSeeds, Proto.CreateStr(Ids.Technology.CanolaSeeds, "Canola seeds", descShort4), new TechnologyProto.Gfx("Assets/Base/Products/Icons/Canola.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.SugarCaneSeeds, Proto.CreateStr(Ids.Technology.SugarCaneSeeds, "Sugar cane seeds", descShort4), new TechnologyProto.Gfx("Assets/Base/Products/Icons/SugarCane.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.SoybeansSeeds, Proto.CreateStr(Ids.Technology.SoybeansSeeds, "Soybean seeds", descShort4), new TechnologyProto.Gfx("Assets/Base/Products/Icons/Soybean.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.FruitSeeds, Proto.CreateStr(Ids.Technology.FruitSeeds, "Fruit seeds", descShort4), new TechnologyProto.Gfx("Assets/Base/Products/Icons/Fruits.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.CornSeeds, Proto.CreateStr(Ids.Technology.CornSeeds, "Corn seeds", descShort4), new TechnologyProto.Gfx("Assets/Base/Products/Icons/Corn.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.PoppySeeds, Proto.CreateStr(Ids.Technology.PoppySeeds, "Poppy seeds", descShort4), new TechnologyProto.Gfx("Assets/Base/Products/Icons/Poppy.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.Electronics2, Proto.CreateStr(Ids.Technology.Electronics2, "Electronics II technology", descShort3), new TechnologyProto.Gfx("Assets/Base/Products/Icons/Electronics2.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.HydrogenCargoShip, Proto.CreateStr(Ids.Technology.HydrogenCargoShip, "Hydrogen cargo ship"), new TechnologyProto.Gfx("Assets/Base/Icons/Technologies/HydrogenCargoShipTechnology.png")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.Microchip, Proto.CreateStr(Ids.Technology.Microchip, "Microchip technology", descShort3), new TechnologyProto.Gfx("Assets/Base/Products/Icons/Microchip.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(Ids.Technology.NuclearEnergy, Proto.CreateStr(Ids.Technology.NuclearEnergy, "Nuclear energy", descShort3), new TechnologyProto.Gfx("Assets/Base/Icons/Technologies/NuclearEnergy.svg")));
      LocStr1 locStr1 = Loc.Str1(IdsCore.Technology.CustomRoutes.ToString() + "__desc", "Enables to manually assign buildings to each other to set up routes that trucks will follow. Supported by storage, {0}. Also enables to assign trucks to individual storages.", "{0}=list of buildings");
      string str = prototypesDb.Get<Proto>((Proto.ID) Ids.Buildings.MineTower).ValueOrNull?.Strings.Name.TranslatedString ?? "Mine tower";
      LocStr alreadyLocalizedStr = LocalizationManager.CreateAlreadyLocalizedStr(IdsCore.Technology.CustomRoutes.ToString() + "_formatted", locStr1.Format(str).Value);
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.CustomRoutes, Proto.CreateStr(IdsCore.Technology.CustomRoutes, "Custom routes", alreadyLocalizedStr), new TechnologyProto.Gfx("Assets/Base/Icons/Technologies/CustomRoutes.svg")));
      prototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.MechPowerAutoBalance, Proto.CreateStr(IdsCore.Technology.MechPowerAutoBalance, "Turbine control", "Once manually enabled for a steam turbine, it prevents it from wasting steam by automatically shutting it off in case there is a large excess of mechanical power on the shaft. Once the power on the shaft gets low, the turbine gets restarted. However restarts are not instant so shaft should be accompanied with a mechanical power storage to achieve stable supply of energy."), new TechnologyProto.Gfx("Assets/Base/Icons/Technologies/AutoBalance.svg")));
    }

    public TechnologyData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
