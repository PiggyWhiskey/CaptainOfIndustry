// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.WasteSortingPlantData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Waste;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class WasteSortingPlantData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto orThrow1 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Recyclables);
      orThrow1.AddParam((IProtoParam) new ApplyRecyclingRatioOnSourcesParam());
      ProductProto orThrow2 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.RecyclablesPressed);
      LocStr2 locStr2 = Loc.Str2(Ids.Buildings.WasteSortingPlant.ToString() + "__desc", "Recycling can transform your island's economy in a significant way as it allows to recover a portion of raw materials from various sources like settlement, maintenance, or research. Recycling reduces the need for raw minerals extraction and thus also extends longevity of deposits. Places that support recycling will return '{0}'. This product can be separated via waste sorting plant into scraps (such as iron or copper scraps) which can be sent to any furnace for smelting. The ratio of materials recycled is based on '{1}' and more about that is explained in the waste sorting plant.", "{0} stands for recyclables product, {1} stands for recycling efficiency (RecyclingEfficiency__Title)");
      LocStr alreadyLocalizedStr = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.WasteSortingPlant.ToString() + "_formatted", locStr2.Format(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Recyclables).Strings.Name.TranslatedString, TrCore.RecyclingEfficiency__Title.TranslatedString).Value);
      prototypesDb.Add<WasteSortingPlantProto>(new WasteSortingPlantProto(Ids.Buildings.WasteSortingPlant, Proto.CreateStr((Proto.ID) Ids.Buildings.WasteSortingPlant, "Waste sorting plant", alreadyLocalizedStr), registrator.LayoutParser.ParseLayoutOrThrow("                        ^~X      ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][4][4][4][4][4][4]   ", "   [5][5][5][5][4][6][6][6][6]   ", "   [5][5][5][5][5][6][6][6][6]   ", "   [5][5][5][5][5][6][6][6][6]   ", "   [5][5][5][5][5][6][6][6][6]   ", "A#>[5][5][5][5][5][6][6][6][6]   ", "B#>[5][5][5][5][5][6][6][6][6]   ", "   [5][5][5][5][5][6][6][6][6]   ", "C~>[5][5][5][5][5][6][6][6][6]   ", "D~>[5][5][5][5][5][6][6][6][6]   ", "   [5][5][5][5][5][6][6][6][6]   "), Costs.Buildings.WasteSortingPlant.MapToEntityCosts(registrator), ImmutableArray.Create<ProductQuantity>(orThrow1.WithQuantity(48), orThrow2.WithQuantity(16)), 360.Quantity(), 180.Quantity(), 20.Seconds(), 300.Kw(), Option<WasteSortingPlantProto>.None, ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.PlayOnce(Duration.FromKeyframes(390))), new WasteSortingPlantProto.Gfx("Assets/Base/Buildings/WasteSortingPlant.prefab", customIconPath: Option<string>.None, categories: new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Waste)))));
    }

    public WasteSortingPlantData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
