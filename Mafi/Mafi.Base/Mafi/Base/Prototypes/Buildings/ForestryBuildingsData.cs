// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ForestryBuildingsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class ForestryBuildingsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      LocStr descShort = Loc.Str(Ids.Buildings.ForestryTower.Value + "__desc", "Enables assignment of tree planters and tree harvesters to designated forestry areas. Only designated forestry areas within the influence of the tower can be used.", "Description of forestry tower.");
      ImmutableArray<ToolbarCategoryProto> categoriesProtos = registrator.GetCategoriesProtos(Ids.ToolbarCategories.BuildingsForVehicles);
      registrator.PrototypesDb.Add<ForestryTowerProto>(new ForestryTowerProto(Ids.Buildings.ForestryTower, Proto.CreateStr((Proto.ID) Ids.Buildings.ForestryTower, "Forestry control tower", descShort), registrator.LayoutParser.ParseLayoutOrThrow("(9)(9)(9)(9)", "(9)(9)(9)(9)", "(9)(9)(9)(9)", "(9)(9)(9)(9)"), Costs.Buildings.ForestryTower.MapToEntityCosts(registrator), new ForestryTowerProto.ForestryArea(new RelTile2i(5, 2), new RelTile2i(60, 60), new RelTile1i(192)), new LayoutEntityProto.Gfx("Assets/Base/Buildings/ForestryTower.prefab", categories: new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos), useInstancedRendering: true))).AddParam((IProtoParam) new DrawArrowWileBuildingProtoParam(4f));
    }

    public ForestryBuildingsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
