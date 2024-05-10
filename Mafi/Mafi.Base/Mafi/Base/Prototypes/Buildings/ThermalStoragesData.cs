// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ThermalStoragesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes.Buildings.ThermalStorages;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class ThermalStoragesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto orThrow1 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamLo);
      ProductProto orThrow2 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamHi);
      ProductProto orThrow3 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamSp);
      string[] strArray = new string[18]
      {
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "B@>[6][6][6][6][6][6][6]>@Y",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "A@>[6][6][6][6][6][6][6]>@X",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   ",
        "   [6][6][6][6][6][6][6]   "
      };
      LocStr descShort = Loc.Str(Ids.Buildings.ThermalStorage.ToString() + "__desc", "Thermal storage uses steam to heat its tank of molten salt to store thermal energy. The accumulated energy can be then used to boil incoming water back to steam. The conversion process has losses but maintaining the accumulated heat does not decay while the storage is operational.", "description of a thermal storage");
      prototypesDb.Add<ThermalStorageProto>(new ThermalStorageProto(Ids.Buildings.ThermalStorage, Proto.CreateStr((Proto.ID) Ids.Buildings.ThermalStorage, "Thermal storage", descShort), registrator.LayoutParser.ParseLayoutOrThrow(strArray), Costs.Buildings.ThermalStorage.MapToEntityCosts(registrator), 200.Kw(), 3000.Quantity(), ImmutableArray.Create<ThermalStorageProto.ProductData>(new ThermalStorageProto.ProductData(orThrow1, 5, 4), new ThermalStorageProto.ProductData(orThrow2, 10, 8), new ThermalStorageProto.ProductData(orThrow3, 20, 16)), prototypesDb.GetOrThrow<VirtualProductProto>((Proto.ID) Ids.Products.Heat), prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Water), 'B', prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamDepleted), 'Y', 4.Percent(), new ThermalStorageProto.Gfx("Assets/Base/Buildings/ThermalStorage.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesWater))));
    }

    public ThermalStoragesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
