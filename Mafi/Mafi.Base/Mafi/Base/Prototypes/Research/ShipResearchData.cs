// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Research.ShipResearchData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Fleet;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Research
{
  internal class ShipResearchData : IResearchNodesData, IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.ResearchNodeProtoBuilder.Start("Ship weapons", Ids.Research.ShipWeapons).Description("Once fitted on the ship, we can finally stop running away from every battle.", "description of a research node in the research tree").AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun1).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun1Rear).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Ship armor", Ids.Research.ShipArmor).Description("Our ship will take more hits from enemy's artillery. Our crew is looking forward to test it, huh?", "description of a research node in the research tree").AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT1).BuildAndAdd();
      LocStr desc1 = Loc.Str(Ids.Research.ShipRadar.ToString() + "__desc", "Advanced ship bridge. Also contains a better radar for area explorations.", "description of a research of a ship bridge upgrade");
      registrator.ResearchNodeProtoBuilder.Start("Ship bridge II", Ids.Research.ShipRadar).AddRequiredProto(Ids.Technology.ShipRadar).Description(desc1).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Bridges.BridgeT2).BuildAndAdd();
      LocStr desc2 = Loc.Str(Ids.Research.Engine2.ToString() + "__desc", "Once fitted, the ship will be able to travel farther.", "description of a research of a ship engine upgrade");
      registrator.ResearchNodeProtoBuilder.Start("Ship engine II", Ids.Research.Engine2).Description(desc2).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Engines.EngineT2).BuildAndAdd();
      LocStr desc3 = Loc.Str(Ids.Research.ShipWeapons2.ToString() + "__desc", "Ship guns that provide increased range and damage.", "description of ship weapons");
      registrator.ResearchNodeProtoBuilder.Start("Ship weapons II", Ids.Research.ShipWeapons2).Description(desc3).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun2).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun2Rear).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Fuel tank upgrade", Ids.Research.ShipFuelTankUpgrade).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.FuelTanks.FuelTankT1).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Ship armor II", Ids.Research.ShipArmor2).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Armor.ArmorT2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Ship bridge III", Ids.Research.ShipRadar2).Description(desc1).AddRequiredProto(Ids.Technology.ShipRadarT2).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Bridges.BridgeT3).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Ship engine III", Ids.Research.Engine3).Description(desc2).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Engines.EngineT3).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Ship weapons III", Ids.Research.ShipWeapons3).Description(desc3).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun3).AddShipPartToUnlock((FleetEntityPartProto.ID) Ids.Fleet.Weapons.Gun3Rear).BuildAndAdd();
    }

    public ShipResearchData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
