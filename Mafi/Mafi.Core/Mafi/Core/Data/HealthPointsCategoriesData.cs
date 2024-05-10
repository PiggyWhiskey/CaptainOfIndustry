// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Data.HealthPointsCategoriesData
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
  internal class HealthPointsCategoriesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.Base, Proto.CreateStr(IdsCore.HealthPointsCategories.Base, "Base"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.Edicts, Proto.CreateStr(IdsCore.HealthPointsCategories.Edicts, "Edicts"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.LandfillPollution, Proto.CreateStr(IdsCore.HealthPointsCategories.LandfillPollution, "Landfill pollution"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.WaterPollution, Proto.CreateStr(IdsCore.HealthPointsCategories.WaterPollution, "Water pollution"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.AirPollution, Proto.CreateStr(IdsCore.HealthPointsCategories.AirPollution, "Air pollution"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.AirPollutionVehicles, Proto.CreateStr(IdsCore.HealthPointsCategories.AirPollutionVehicles, "Vehicles pollution"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.AirPollutionShips, Proto.CreateStr(IdsCore.HealthPointsCategories.AirPollutionShips, "Ships pollution"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.Food, Proto.CreateStr(IdsCore.HealthPointsCategories.Food, "Food"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.Healthcare, Proto.CreateStr(IdsCore.HealthPointsCategories.Healthcare, "Healthcare"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.WasteInSettlement, Proto.CreateStr(IdsCore.HealthPointsCategories.WasteInSettlement, "Settlement waste"), HealthPointsCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.HealthPointsCategories.Disease, Proto.CreateStr(IdsCore.HealthPointsCategories.Disease, "Disease"), HealthPointsCategoryProto.Gfx.Empty));
    }

    public HealthPointsCategoriesData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
