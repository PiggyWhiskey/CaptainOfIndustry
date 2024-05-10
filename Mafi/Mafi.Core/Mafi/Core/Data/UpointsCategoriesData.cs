// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Data.UpointsCategoriesData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Data
{
  internal class UpointsCategoriesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      UpointsStatsCategoryProto statsCategory1 = registrator.PrototypesDb.Add<UpointsStatsCategoryProto>(new UpointsStatsCategoryProto(IdsCore.UpointsStatsCategories.OneTimeAction, Proto.CreateStr(IdsCore.UpointsStatsCategories.OneTimeAction, "One time actions"), UpointsStatsCategoryProto.Gfx.Empty));
      UpointsStatsCategoryProto statsCategory2 = registrator.PrototypesDb.Add<UpointsStatsCategoryProto>(new UpointsStatsCategoryProto(IdsCore.UpointsStatsCategories.Ignore, Proto.CreateStr(IdsCore.UpointsStatsCategories.Ignore, "Ignore", "", "HIDE"), UpointsStatsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<UpointsStatsCategoryProto>(new UpointsStatsCategoryProto(IdsCore.UpointsStatsCategories.IslandBuilding, Proto.CreateStr(IdsCore.UpointsStatsCategories.IslandBuilding, "Island buildings"), UpointsStatsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.Edict, Proto.CreateStr(IdsCore.UpointsCategories.Edict, "Edicts"), UpointsStatsCategoryProto.Gfx.Empty, Option<UpointsStatsCategoryProto>.None));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.Boost, Proto.CreateStr(IdsCore.UpointsCategories.Boost, "Boost"), UpointsStatsCategoryProto.Gfx.Empty, Option<UpointsStatsCategoryProto>.None));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.Health, Proto.CreateStr(IdsCore.UpointsCategories.Health, "Health"), UpointsStatsCategoryProto.Gfx.Empty, Option<UpointsStatsCategoryProto>.None));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.Starvation, Proto.CreateStr(IdsCore.UpointsCategories.Starvation, "Starvation"), UpointsStatsCategoryProto.Gfx.Empty, Option<UpointsStatsCategoryProto>.None));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.Contract, Proto.CreateStr(IdsCore.UpointsCategories.Contract, "Contracts"), UpointsStatsCategoryProto.Gfx.Empty, Option<UpointsStatsCategoryProto>.None));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.SettlementQuality, Proto.CreateStr(IdsCore.UpointsCategories.SettlementQuality, "Settlements quality"), UpointsStatsCategoryProto.Gfx.Empty, Option<UpointsStatsCategoryProto>.None));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.Homeless, Proto.CreateStr(IdsCore.UpointsCategories.Homeless, "Homeless"), UpointsStatsCategoryProto.Gfx.Empty, (Option<UpointsStatsCategoryProto>) statsCategory2));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.Rockets, Proto.CreateStr(IdsCore.UpointsCategories.Rockets, "Rocket launch"), UpointsStatsCategoryProto.Gfx.Empty, Option<UpointsStatsCategoryProto>.None));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.FreeUnity, Proto.CreateStr(IdsCore.UpointsCategories.FreeUnity, "Free"), UpointsStatsCategoryProto.Gfx.Empty, (Option<UpointsStatsCategoryProto>) statsCategory2));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.PopsAdoption, Proto.CreateStr(IdsCore.UpointsCategories.PopsAdoption, "Pops adoption"), UpointsStatsCategoryProto.Gfx.Empty, (Option<UpointsStatsCategoryProto>) statsCategory1));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.QuickTrade, Proto.CreateStr(IdsCore.UpointsCategories.QuickTrade, "Quick trade"), UpointsStatsCategoryProto.Gfx.Empty, (Option<UpointsStatsCategoryProto>) statsCategory1));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.QuickBuild, Proto.CreateStr(IdsCore.UpointsCategories.QuickBuild, "Quick build"), UpointsStatsCategoryProto.Gfx.Empty, (Option<UpointsStatsCategoryProto>) statsCategory1));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.ContractEstablish, Proto.CreateStr(IdsCore.UpointsCategories.ContractEstablish, "Contract establish"), UpointsStatsCategoryProto.Gfx.Empty, (Option<UpointsStatsCategoryProto>) statsCategory1));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.QuickRemove, Proto.CreateStr(IdsCore.UpointsCategories.QuickRemove, "Quick remove"), UpointsStatsCategoryProto.Gfx.Empty, (Option<UpointsStatsCategoryProto>) statsCategory1));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.QuickRepair, Proto.CreateStr(IdsCore.UpointsCategories.QuickRepair, "Quick repair"), UpointsStatsCategoryProto.Gfx.Empty, (Option<UpointsStatsCategoryProto>) statsCategory1));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.VehicleRecovery, Proto.CreateStr(IdsCore.UpointsCategories.VehicleRecovery, "Vehicle recovery"), UpointsStatsCategoryProto.Gfx.Empty, (Option<UpointsStatsCategoryProto>) statsCategory1));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.OtherDecorations, Proto.CreateStr(IdsCore.UpointsCategories.OtherDecorations, "Other decorations"), UpointsStatsCategoryProto.Gfx.Empty, Option<UpointsStatsCategoryProto>.None));
      registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.UpointsCategories.ShipFuel, Proto.CreateStr(IdsCore.UpointsCategories.ShipFuel, "Fuel for ships"), UpointsStatsCategoryProto.Gfx.Empty, Option<UpointsStatsCategoryProto>.None));
    }

    public UpointsCategoriesData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
