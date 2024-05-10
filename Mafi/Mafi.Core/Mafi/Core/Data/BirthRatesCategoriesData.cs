// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Data.BirthRatesCategoriesData
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
  internal class BirthRatesCategoriesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<BirthRateCategoryProto>(new BirthRateCategoryProto(IdsCore.BirthRateCategories.Base, Proto.CreateStr(IdsCore.BirthRateCategories.Base, "Base"), BirthRateCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<BirthRateCategoryProto>(new BirthRateCategoryProto(IdsCore.BirthRateCategories.Starvation, Proto.CreateStr(IdsCore.BirthRateCategories.Starvation, "Starvation"), BirthRateCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<BirthRateCategoryProto>(new BirthRateCategoryProto(IdsCore.BirthRateCategories.Radiation, Proto.CreateStr(IdsCore.BirthRateCategories.Radiation, "Radiation"), BirthRateCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<BirthRateCategoryProto>(new BirthRateCategoryProto(IdsCore.BirthRateCategories.Edicts, Proto.CreateStr(IdsCore.BirthRateCategories.Edicts, "Edicts"), BirthRateCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<BirthRateCategoryProto>(new BirthRateCategoryProto(IdsCore.BirthRateCategories.Health, Proto.CreateStr(IdsCore.BirthRateCategories.Health, "Health"), BirthRateCategoryProto.Gfx.Empty));
      registrator.PrototypesDb.Add<BirthRateCategoryProto>(new BirthRateCategoryProto(IdsCore.BirthRateCategories.Disease, Proto.CreateStr(IdsCore.BirthRateCategories.Disease, "Disease"), BirthRateCategoryProto.Gfx.Empty));
    }

    public BirthRatesCategoriesData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
