// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Fleet.FleetData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Fleet;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Fleet
{
  public class FleetData : IModData
  {
    private static readonly ProductProto.ID SteelSlabId;
    private static readonly ProductProto.ID IronSlabId;
    private static readonly ProductProto.ID MechPartsId;
    private static readonly ProductProto.ID CopperWireId;
    private static readonly ProductProto.ID ElectronicsId;
    private static readonly ProductProto.ID Electronics2Id;
    private bool m_disableCosts;

    public void DisableCosts() => this.m_disableCosts = true;

    public void RegisterData(ProtoRegistrator registrator)
    {
      this.createEngines(registrator);
      this.createWeapons(registrator);
      this.createHullUpgrades(registrator);
      this.createBridges(registrator);
      this.createFuelTankUpgrades(registrator);
      FleetEntityHullProtoBuilder hullProtoBuilder = registrator.FleetEntityHullProtoBuilder;
      ProtosDb prototypesDb = registrator.PrototypesDb;
      hullProtoBuilder.Start("Scout ship", Ids.Fleet.Hulls.Scout).SetBattlePriority(10).SetHitChanceWeight(8).SetExtraRoundsToEscape(5).SetHullHp(800).SetHullValue(AssetValue.Empty).SetHullGraphics(new FleetEntityHullProto.Gfx("Assets/Unity/UserInterface/Ship/ScoutHull512x128.png", 47.Percent(), 53.Percent())).AddWeaponsGroup(6, true, new FleetEntityPartProto.ID[2]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun0,
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun1
      }).BuildAndAdd();
      hullProtoBuilder.Start("Patrol ship", Ids.Fleet.Hulls.Patrol).SetBattlePriority(20).SetHitChanceWeight(9).SetExtraRoundsToEscape(5).SetHullHp(1000).SetHullValue(AssetValue.Empty).SetHullGraphics(new FleetEntityHullProto.Gfx("Assets/Unity/UserInterface/Ship/PatrolHull512x128.png", 57.Percent(), 42.Percent())).AddEngineUpgradeGroup(new FleetEntityPartProto.ID[3]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Engines.EngineT1,
        (FleetEntityPartProto.ID) Ids.Fleet.Engines.EngineT2,
        (FleetEntityPartProto.ID) Ids.Fleet.Engines.EngineT3
      }).AddWeaponsGroup(1, true, new FleetEntityPartProto.ID[3]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun1,
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun2,
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun3
      }, "WpnFrontT0").AddWeaponsGroup(1, false, new FleetEntityPartProto.ID[3]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun1Rear,
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun2Rear,
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun3Rear
      }, "WpnRearT0").AddHullUpgradesGroup(2, new FleetEntityPartProto.ID[2]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT1,
        (FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT2
      }).AddRadarUpgradeGroup(new FleetEntityPartProto.ID[3]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Bridges.BridgeT1,
        (FleetEntityPartProto.ID) Ids.Fleet.Bridges.BridgeT2,
        (FleetEntityPartProto.ID) Ids.Fleet.Bridges.BridgeT3
      }).AddFuelTankUpgradeGroup(new FleetEntityPartProto.ID[1]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.FuelTanks.FuelTankT1
      }).BuildAndAdd();
      hullProtoBuilder.Start("Cruiser", Ids.Fleet.Hulls.Cruiser).SetBattlePriority(30).SetHitChanceWeight(15).SetExtraRoundsToEscape(10).SetHullHp(2000).SetHullValue(AssetValue.Empty).SetHullGraphics(new FleetEntityHullProto.Gfx("Assets/Unity/UserInterface/Ship/CruiserHull512x128.png", 82.Percent(), 23.Percent())).AddWeaponsGroup(4, true, new FleetEntityPartProto.ID[3]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun1,
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun2,
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun2
      }).AddHullUpgradesGroup(3, new FleetEntityPartProto.ID[2]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT1,
        (FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT2
      }).BuildAndAdd();
      hullProtoBuilder.Start("Battleship", Ids.Fleet.Hulls.Battleship).SetBattlePriority(40).SetHitChanceWeight(20).SetExtraRoundsToEscape(15).SetHullHp(4000).SetHullValue(AssetValue.Empty).SetHullGraphics(new FleetEntityHullProto.Gfx("Assets/Unity/UserInterface/Ship/BattleshipHull512x128.png", Percent.Hundred, 21.Percent())).AddWeaponsGroup(6, true, new FleetEntityPartProto.ID[3]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun1,
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun2,
        (FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun3
      }).AddHullUpgradesGroup(4, new FleetEntityPartProto.ID[2]
      {
        (FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT1,
        (FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT2
      }).BuildAndAdd();
    }

    private void createHullUpgrades(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      prototypesDb.Add<UpgradeHullProto>(new UpgradeHullProto((FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT1, "Armor plating", this.m_disableCosts ? AssetValue.Empty : FleetData.IronSlabId.ToAssetValue(200, prototypesDb), new UpgradableIntProto(250), new UpgradableIntProto(10), new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Armor1.png")));
      prototypesDb.Add<UpgradeHullProto>(new UpgradeHullProto((FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT2, "Armor plating II", this.m_disableCosts ? AssetValue.Empty : FleetData.SteelSlabId.ToAssetValue(200, prototypesDb), new UpgradableIntProto(750), new UpgradableIntProto(25), new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Armor2.png")));
    }

    private void createWeapons(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      registrator.PrototypesDb.Add<FleetWeaponProto>(new FleetWeaponProto(Ids.Fleet.Weapons.Gun0, "Gun (basic)", AssetValue.Empty, 120, 40, 13, 18, 90.Percent(), 75.Percent(), 0, new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Gun2.png")));
      FleetWeaponProto fleetWeaponProto1 = registrator.PrototypesDb.Add<FleetWeaponProto>(new FleetWeaponProto(Ids.Fleet.Weapons.Gun1, "Gun I (front)", this.m_disableCosts ? AssetValue.Empty : FleetData.IronSlabId.ToAssetValue(180, prototypesDb) + FleetData.MechPartsId.ToAssetValue(40, prototypesDb), 160, 50, 15, 16, 90.Percent(), 75.Percent(), 10, new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Gun2.png", (Option<string>) "WpnFrontT1")));
      registrator.PrototypesDb.Add<FleetWeaponProto>(fleetWeaponProto1.CloneWithNewId(Ids.Fleet.Weapons.Gun1Rear, "Gun I (rear)", new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Gun2.png", (Option<string>) "WpnRearT1")));
      FleetWeaponProto fleetWeaponProto2 = registrator.PrototypesDb.Add<FleetWeaponProto>(new FleetWeaponProto(Ids.Fleet.Weapons.Gun2, "Gun II (front)", this.m_disableCosts ? AssetValue.Empty : FleetData.SteelSlabId.ToAssetValue(180, prototypesDb) + FleetData.MechPartsId.ToAssetValue(80, prototypesDb) + FleetData.ElectronicsId.ToAssetValue(20, prototypesDb), 450, 150, 20, 18, 90.Percent(), 70.Percent(), 15, new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Gun3.png", (Option<string>) "WpnFrontT2")));
      registrator.PrototypesDb.Add<FleetWeaponProto>(fleetWeaponProto2.CloneWithNewId(Ids.Fleet.Weapons.Gun2Rear, "Gun II (rear)", new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Gun3.png", (Option<string>) "WpnRearT2")));
      FleetWeaponProto fleetWeaponProto3 = registrator.PrototypesDb.Add<FleetWeaponProto>(new FleetWeaponProto(Ids.Fleet.Weapons.Gun3, "Gun III (front)", this.m_disableCosts ? AssetValue.Empty : FleetData.SteelSlabId.ToAssetValue(540, prototypesDb) + FleetData.MechPartsId.ToAssetValue(240, prototypesDb) + FleetData.ElectronicsId.ToAssetValue(160, prototypesDb), 900, 300, 25, 20, 85.Percent(), 65.Percent(), 20, new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Gun4.png", (Option<string>) "WpnFrontT3")));
      registrator.PrototypesDb.Add<FleetWeaponProto>(fleetWeaponProto3.CloneWithNewId(Ids.Fleet.Weapons.Gun3Rear, "Gun III (rear)", new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Gun4.png", (Option<string>) "WpnRearT3")));
    }

    private void createEngines(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      prototypesDb.Add<FleetEnginePartProto>(new FleetEnginePartProto((FleetEntityPartProto.ID) Ids.Fleet.Engines.EngineT1, Proto.CreateStr((Proto.ID) Ids.Fleet.Engines.EngineT1, "Ship engine", ""), AssetValue.Empty, 0.35.ToFix32(), (Fix32) 26, 80.Quantity(), 15, new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Engine1.png")));
      prototypesDb.Add<FleetEnginePartProto>(new FleetEnginePartProto((FleetEntityPartProto.ID) Ids.Fleet.Engines.EngineT2, Proto.CreateStr((Proto.ID) Ids.Fleet.Engines.EngineT2, "Ship engine II", "Increases speed and range of the ship."), this.m_disableCosts ? AssetValue.Empty : FleetData.SteelSlabId.ToAssetValue(160, prototypesDb) + FleetData.MechPartsId.ToAssetValue(200, prototypesDb) + FleetData.ElectronicsId.ToAssetValue(60, prototypesDb), 1.2.ToFix32(), (Fix32) 19, 160.Quantity(), 30, new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Engine2.png", (Option<string>) "EngineT1")));
      prototypesDb.Add<FleetEnginePartProto>(new FleetEnginePartProto((FleetEntityPartProto.ID) Ids.Fleet.Engines.EngineT3, Proto.CreateStr((Proto.ID) Ids.Fleet.Engines.EngineT3, "Ship engine III", "Increases speed and range of the ship."), this.m_disableCosts ? AssetValue.Empty : FleetData.SteelSlabId.ToAssetValue(480, prototypesDb) + FleetData.MechPartsId.ToAssetValue(400, prototypesDb) + FleetData.Electronics2Id.ToAssetValue(160, prototypesDb), 2.ToFix32(), (Fix32) 15, 320.Quantity(), 60, new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Engine3.png", (Option<string>) "EngineT2")));
    }

    private void createBridges(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      prototypesDb.Add<FleetBridgePartProto>(new FleetBridgePartProto((FleetEntityPartProto.ID) Ids.Fleet.Bridges.BridgeT1, Proto.CreateStr((Proto.ID) Ids.Fleet.Bridges.BridgeT1, "Ship bridge", ""), FleetData.IronSlabId.ToAssetValue(600, prototypesDb), new UpgradableIntProto(0), new UpgradableIntProto(0), 0, new FleetBridgePartProto.Gfx("Assets/Base/Ships/BattleShip/BattleShipT1.prefab", "Assets/Base/Ships/BattleShip/Icons/Ship1.png")));
      prototypesDb.Add<FleetBridgePartProto>(new FleetBridgePartProto((FleetEntityPartProto.ID) Ids.Fleet.Bridges.BridgeT2, Proto.CreateStr((Proto.ID) Ids.Fleet.Bridges.BridgeT2, "Ship bridge II", "Provides advanced radar capabilities."), this.m_disableCosts ? AssetValue.Empty : FleetData.SteelSlabId.ToAssetValue(400, prototypesDb) + FleetData.MechPartsId.ToAssetValue(100, prototypesDb) + FleetData.ElectronicsId.ToAssetValue(100, prototypesDb), new UpgradableIntProto(500), new UpgradableIntProto(1), 10, new FleetBridgePartProto.Gfx("Assets/Base/Ships/BattleShip/BattleShipT2.prefab", "Assets/Base/Ships/BattleShip/Icons/Ship2.png")));
      prototypesDb.Add<FleetBridgePartProto>(new FleetBridgePartProto((FleetEntityPartProto.ID) Ids.Fleet.Bridges.BridgeT3, Proto.CreateStr((Proto.ID) Ids.Fleet.Bridges.BridgeT3, "Ship bridge III", "Provides advanced radar capabilities."), this.m_disableCosts ? AssetValue.Empty : FleetData.SteelSlabId.ToAssetValue(1200, prototypesDb) + FleetData.MechPartsId.ToAssetValue(200, prototypesDb) + FleetData.ElectronicsId.ToAssetValue(600, prototypesDb), new UpgradableIntProto(1500), new UpgradableIntProto(2), 30, new FleetBridgePartProto.Gfx("Assets/Base/Ships/BattleShip/BattleShipT3.prefab", "Assets/Base/Ships/BattleShip/Icons/Ship3.png")));
    }

    private void createFuelTankUpgrades(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      prototypesDb.Add<FleetFuelTankPartProto>(new FleetFuelTankPartProto((FleetEntityPartProto.ID) Ids.Fleet.FuelTanks.FuelTankT1, "Extra fuel tank", this.m_disableCosts ? AssetValue.Empty : FleetData.SteelSlabId.ToAssetValue(300, prototypesDb), 40.Quantity(), new FleetEntityGfx("Assets/Base/Ships/BattleShip/Icons/Engine1.png")));
    }

    public FleetData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FleetData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      FleetData.SteelSlabId = Ids.Products.Steel;
      FleetData.IronSlabId = Ids.Products.Iron;
      FleetData.MechPartsId = Ids.Products.MechanicalParts;
      FleetData.CopperWireId = Ids.Products.Copper;
      FleetData.ElectronicsId = Ids.Products.Electronics;
      FleetData.Electronics2Id = Ids.Products.Electronics2;
    }
  }
}
