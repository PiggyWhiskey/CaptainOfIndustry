// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.TombOfCaptainsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class TombOfCaptainsData : IModData
  {
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> BASE_0_AREA;
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> BASE_1_AREA;
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> BASE_2_AREA;
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> TEMPLE_1_AREA;
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> TEMPLE_2_AREA;
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> CUPOLA_1_AREA;
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> CUPOLA_2_AREA;
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> ANCHOR_AREA;
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> CAT_1_AREA;
    private static readonly KeyValuePair<int, RectangleTerrainArea2i> CAT_2_AREA;
    private static readonly CustomLayoutToken[] EXTRA_TOKENS;

    private static EntityLayout createLayout(
      ProtoRegistrator registrator,
      bool includePorts,
      KeyValuePair<int, RectangleTerrainArea2i>[] heights)
    {
      int[][] array = Enumerable.Range(0, 46).Select<int, int[]>((Func<int, int[]>) (_ => new int[75])).ToArray<int[]>();
      foreach (KeyValuePair<int, RectangleTerrainArea2i> height in heights)
      {
        Tile2i origin = height.Value.Origin;
        RelTile2i size = height.Value.Size;
        Assert.That<int>(origin.X + size.X).IsLessOrEqual(75, string.Format("Area {0}+{1} is outside of {2}x{3} (x is bad).", (object) origin, (object) size, (object) 75, (object) 46));
        Assert.That<int>(origin.Y + size.Y).IsLessOrEqual(46, string.Format("Area {0}+{1} is outside of {2}x{3} (y is bad).", (object) origin, (object) size, (object) 75, (object) 46));
        for (int index1 = 0; index1 < size.Y; ++index1)
        {
          for (int index2 = 0; index2 < size.X; ++index2)
            array[origin.Y + index1][origin.X + index2] = height.Key.Max(array[origin.Y + index1][origin.X + index2]);
        }
      }
      string[] strArray = array.MapArray<int[], string>((Func<int[], int, string>) ((x, y) =>
      {
        string str;
        if (!includePorts)
        {
          str = "   ";
        }
        else
        {
          switch (y)
          {
            case 8:
              str = "A>@";
              break;
            case 10:
              str = "B>#";
              break;
            default:
              str = "   ";
              break;
          }
        }
        return str + ((IEnumerable<int>) x).Select<int, string>((Func<int, string>) (i => i >= 10 ? string.Format("{0}]", (object) i) : string.Format("[{0}]", (object) i))).JoinStrings();
      }));
      return registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) TombOfCaptainsData.EXTRA_TOKENS), strArray);
    }

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      LocStr name = Loc.Str(Ids.Buildings.TombOfCaptainsStageFinal.ToString() + "__name", "Tomb of Captains", "name of a building");
      LocStr descShort = Loc.Str(Ids.Buildings.TombOfCaptainsStage1.ToString() + "__desc", "Tomb of Captains provides final resting place for generations of captains who lead the people of this island. When completed, fires should be always lit and tomb should be decorated with flowers to pay tribute to resting Captains.", "a description of tomb of captains");
      ProductProto orThrow1 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.FuelGas);
      ProductProto orThrow2 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Flowers);
      Duration constrDurPerProduct = LayoutEntityProto.DEFAULT_CONSTR_DUR_PER_PRODUCT;
      ProtosDb protosDb1 = prototypesDb;
      StaticEntityProto.ID ofCaptainsStage1 = Ids.Buildings.TombOfCaptainsStage1;
      Proto.Str strings1 = new Proto.Str(LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.Buildings.TombOfCaptainsStage1.Value, TrCore.StageStr.Format(name.TranslatedString, "1")), descShort);
      EntityLayout layout1 = TombOfCaptainsData.createLayout(registrator, false, new KeyValuePair<int, RectangleTerrainArea2i>[2]
      {
        TombOfCaptainsData.BASE_0_AREA,
        TombOfCaptainsData.BASE_1_AREA
      });
      EntityCosts entityCosts1 = Costs.Buildings.TombOfCaptains1.MapToEntityCosts(registrator);
      Duration duration1 = constrDurPerProduct;
      PartialProductQuantity none1 = PartialProductQuantity.None;
      PartialProductQuantity none2 = PartialProductQuantity.None;
      Upoints upoints1 = 0.Upoints();
      Upoints upoints2 = 0.Upoints();
      Upoints upoints3 = 0.Upoints();
      Upoints upoints4 = 0.Upoints();
      LayoutEntityProto.Gfx graphics1 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/Landmarks/TombOfCaptains01.prefab", customIconPath: (Option<string>) "Assets/Unity/Generated/Icons/LayoutEntity/TombOfCaptains.png", categories: new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Landmarks)));
      Duration constructionDurationPerProduct1 = duration1;
      PartialProductQuantity fuelConsumptionPerDay1 = none1;
      PartialProductQuantity flowersConsumptionPerDay1 = none2;
      Upoints minUnityForFirePerMonth1 = upoints1;
      Upoints maxUnityForFirePerMonth1 = upoints2;
      Upoints minUnityForFlowersPerMonth1 = upoints3;
      Upoints maxUnityForFlowersPerMonth1 = upoints4;
      TombOfCaptainsProto proto1 = new TombOfCaptainsProto(ofCaptainsStage1, strings1, layout1, entityCosts1, graphics1, constructionDurationPerProduct1, fuelConsumptionPerDay1, flowersConsumptionPerDay1, minUnityForFirePerMonth1, maxUnityForFirePerMonth1, minUnityForFlowersPerMonth1, maxUnityForFlowersPerMonth1);
      TombOfCaptainsProto tombOfCaptainsProto = protosDb1.Add<TombOfCaptainsProto>(proto1);
      tombOfCaptainsProto.SetAvailability(false);
      ProtosDb protosDb2 = prototypesDb;
      StaticEntityProto.ID ofCaptainsStage2 = Ids.Buildings.TombOfCaptainsStage2;
      Proto.Str strings2 = new Proto.Str(LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.Buildings.TombOfCaptainsStage2.Value, TrCore.StageStr.Format(name.TranslatedString, "2")), descShort);
      EntityLayout layout2 = TombOfCaptainsData.createLayout(registrator, false, new KeyValuePair<int, RectangleTerrainArea2i>[3]
      {
        TombOfCaptainsData.BASE_0_AREA,
        TombOfCaptainsData.BASE_1_AREA,
        TombOfCaptainsData.BASE_2_AREA
      });
      EntityCosts entityCosts2 = Costs.Buildings.TombOfCaptains2.MapToEntityCosts(registrator);
      Duration duration2 = constrDurPerProduct;
      PartialProductQuantity none3 = PartialProductQuantity.None;
      PartialProductQuantity none4 = PartialProductQuantity.None;
      Upoints upoints5 = 0.Upoints();
      Upoints upoints6 = 0.Upoints();
      Upoints upoints7 = 0.Upoints();
      Upoints upoints8 = 0.Upoints();
      string prefabPath1 = tombOfCaptainsProto.Graphics.PrefabPath;
      Option<string> iconPath1 = (Option<string>) tombOfCaptainsProto.Graphics.IconPath;
      RelTile3f prefabOrigin1 = new RelTile3f();
      Option<string> customIconPath1 = iconPath1;
      ColorRgba color1 = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers1 = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories1 = new ImmutableArray<ToolbarCategoryProto>?();
      ImmutableArray<string> instancedRenderingExcludedObjects1 = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics2 = new LayoutEntityProto.Gfx(prefabPath1, prefabOrigin1, customIconPath1, color1, visualizedLayers: visualizedLayers1, categories: categories1, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects1);
      Duration constructionDurationPerProduct2 = duration2;
      PartialProductQuantity fuelConsumptionPerDay2 = none3;
      PartialProductQuantity flowersConsumptionPerDay2 = none4;
      Upoints minUnityForFirePerMonth2 = upoints5;
      Upoints maxUnityForFirePerMonth2 = upoints6;
      Upoints minUnityForFlowersPerMonth2 = upoints7;
      Upoints maxUnityForFlowersPerMonth2 = upoints8;
      TombOfCaptainsProto proto2 = new TombOfCaptainsProto(ofCaptainsStage2, strings2, layout2, entityCosts2, graphics2, constructionDurationPerProduct2, fuelConsumptionPerDay2, flowersConsumptionPerDay2, minUnityForFirePerMonth2, maxUnityForFirePerMonth2, minUnityForFlowersPerMonth2, maxUnityForFlowersPerMonth2);
      protosDb2.Add<TombOfCaptainsProto>(proto2).SetAvailability(false);
      ProtosDb protosDb3 = prototypesDb;
      StaticEntityProto.ID ofCaptainsStage3 = Ids.Buildings.TombOfCaptainsStage3;
      Proto.Str strings3 = new Proto.Str(LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.Buildings.TombOfCaptainsStage3.Value, TrCore.StageStr.Format(name.TranslatedString, "3")), descShort);
      EntityLayout layout3 = TombOfCaptainsData.createLayout(registrator, false, new KeyValuePair<int, RectangleTerrainArea2i>[4]
      {
        TombOfCaptainsData.BASE_0_AREA,
        TombOfCaptainsData.BASE_1_AREA,
        TombOfCaptainsData.BASE_2_AREA,
        TombOfCaptainsData.TEMPLE_1_AREA
      });
      EntityCosts entityCosts3 = Costs.Buildings.TombOfCaptains3.MapToEntityCosts(registrator);
      Duration duration3 = constrDurPerProduct;
      PartialProductQuantity none5 = PartialProductQuantity.None;
      PartialProductQuantity none6 = PartialProductQuantity.None;
      Upoints upoints9 = 0.Upoints();
      Upoints upoints10 = 0.Upoints();
      Upoints upoints11 = 0.Upoints();
      Upoints upoints12 = 0.Upoints();
      string prefabPath2 = tombOfCaptainsProto.Graphics.PrefabPath;
      Option<string> iconPath2 = (Option<string>) tombOfCaptainsProto.Graphics.IconPath;
      RelTile3f prefabOrigin2 = new RelTile3f();
      Option<string> customIconPath2 = iconPath2;
      ColorRgba color2 = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers2 = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories2 = new ImmutableArray<ToolbarCategoryProto>?();
      ImmutableArray<string> instancedRenderingExcludedObjects2 = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics3 = new LayoutEntityProto.Gfx(prefabPath2, prefabOrigin2, customIconPath2, color2, visualizedLayers: visualizedLayers2, categories: categories2, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects2);
      Duration constructionDurationPerProduct3 = duration3;
      PartialProductQuantity fuelConsumptionPerDay3 = none5;
      PartialProductQuantity flowersConsumptionPerDay3 = none6;
      Upoints minUnityForFirePerMonth3 = upoints9;
      Upoints maxUnityForFirePerMonth3 = upoints10;
      Upoints minUnityForFlowersPerMonth3 = upoints11;
      Upoints maxUnityForFlowersPerMonth3 = upoints12;
      TombOfCaptainsProto proto3 = new TombOfCaptainsProto(ofCaptainsStage3, strings3, layout3, entityCosts3, graphics3, constructionDurationPerProduct3, fuelConsumptionPerDay3, flowersConsumptionPerDay3, minUnityForFirePerMonth3, maxUnityForFirePerMonth3, minUnityForFlowersPerMonth3, maxUnityForFlowersPerMonth3);
      protosDb3.Add<TombOfCaptainsProto>(proto3).SetAvailability(false);
      ProtosDb protosDb4 = prototypesDb;
      StaticEntityProto.ID ofCaptainsStage4 = Ids.Buildings.TombOfCaptainsStage4;
      Proto.Str strings4 = new Proto.Str(LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.Buildings.TombOfCaptainsStage4.Value, TrCore.StageStr.Format(name.TranslatedString, "4")), descShort);
      EntityLayout layout4 = TombOfCaptainsData.createLayout(registrator, false, new KeyValuePair<int, RectangleTerrainArea2i>[4]
      {
        TombOfCaptainsData.BASE_0_AREA,
        TombOfCaptainsData.BASE_1_AREA,
        TombOfCaptainsData.BASE_2_AREA,
        TombOfCaptainsData.TEMPLE_1_AREA
      });
      EntityCosts entityCosts4 = Costs.Buildings.TombOfCaptains4.MapToEntityCosts(registrator);
      Duration duration4 = constrDurPerProduct;
      PartialProductQuantity none7 = PartialProductQuantity.None;
      PartialProductQuantity none8 = PartialProductQuantity.None;
      Upoints upoints13 = 0.Upoints();
      Upoints upoints14 = 0.Upoints();
      Upoints upoints15 = 0.Upoints();
      Upoints upoints16 = 0.Upoints();
      string prefabPath3 = tombOfCaptainsProto.Graphics.PrefabPath;
      Option<string> iconPath3 = (Option<string>) tombOfCaptainsProto.Graphics.IconPath;
      RelTile3f prefabOrigin3 = new RelTile3f();
      Option<string> customIconPath3 = iconPath3;
      ColorRgba color3 = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers3 = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories3 = new ImmutableArray<ToolbarCategoryProto>?();
      ImmutableArray<string> instancedRenderingExcludedObjects3 = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics4 = new LayoutEntityProto.Gfx(prefabPath3, prefabOrigin3, customIconPath3, color3, visualizedLayers: visualizedLayers3, categories: categories3, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects3);
      Duration constructionDurationPerProduct4 = duration4;
      PartialProductQuantity fuelConsumptionPerDay4 = none7;
      PartialProductQuantity flowersConsumptionPerDay4 = none8;
      Upoints minUnityForFirePerMonth4 = upoints13;
      Upoints maxUnityForFirePerMonth4 = upoints14;
      Upoints minUnityForFlowersPerMonth4 = upoints15;
      Upoints maxUnityForFlowersPerMonth4 = upoints16;
      TombOfCaptainsProto proto4 = new TombOfCaptainsProto(ofCaptainsStage4, strings4, layout4, entityCosts4, graphics4, constructionDurationPerProduct4, fuelConsumptionPerDay4, flowersConsumptionPerDay4, minUnityForFirePerMonth4, maxUnityForFirePerMonth4, minUnityForFlowersPerMonth4, maxUnityForFlowersPerMonth4);
      protosDb4.Add<TombOfCaptainsProto>(proto4).SetAvailability(false);
      ProtosDb protosDb5 = prototypesDb;
      StaticEntityProto.ID ofCaptainsStage5 = Ids.Buildings.TombOfCaptainsStage5;
      Proto.Str strings5 = new Proto.Str(LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.Buildings.TombOfCaptainsStage5.Value, TrCore.StageStr.Format(name.TranslatedString, "5")), descShort);
      EntityLayout layout5 = TombOfCaptainsData.createLayout(registrator, true, new KeyValuePair<int, RectangleTerrainArea2i>[7]
      {
        TombOfCaptainsData.BASE_0_AREA,
        TombOfCaptainsData.BASE_1_AREA,
        TombOfCaptainsData.BASE_2_AREA,
        TombOfCaptainsData.TEMPLE_2_AREA,
        TombOfCaptainsData.CUPOLA_1_AREA,
        TombOfCaptainsData.CAT_1_AREA,
        TombOfCaptainsData.CAT_2_AREA
      });
      EntityCosts entityCosts5 = Costs.Buildings.TombOfCaptains5.MapToEntityCosts(registrator);
      Duration duration5 = constrDurPerProduct;
      PartialProductQuantity partialProductQuantity1 = 0.05.Of(orThrow1);
      PartialProductQuantity partialProductQuantity2 = 0.075.Of(orThrow2);
      Upoints upoints17 = 0.0.Upoints();
      Upoints upoints18 = 0.25.Upoints();
      Upoints upoints19 = 0.0.Upoints();
      Upoints upoints20 = 0.25.Upoints();
      string prefabPath4 = tombOfCaptainsProto.Graphics.PrefabPath;
      Option<string> iconPath4 = (Option<string>) tombOfCaptainsProto.Graphics.IconPath;
      RelTile3f prefabOrigin4 = new RelTile3f();
      Option<string> customIconPath4 = iconPath4;
      ColorRgba color4 = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers4 = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories4 = new ImmutableArray<ToolbarCategoryProto>?();
      ImmutableArray<string> instancedRenderingExcludedObjects4 = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics5 = new LayoutEntityProto.Gfx(prefabPath4, prefabOrigin4, customIconPath4, color4, visualizedLayers: visualizedLayers4, categories: categories4, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects4);
      Duration constructionDurationPerProduct5 = duration5;
      PartialProductQuantity fuelConsumptionPerDay5 = partialProductQuantity1;
      PartialProductQuantity flowersConsumptionPerDay5 = partialProductQuantity2;
      Upoints minUnityForFirePerMonth5 = upoints17;
      Upoints maxUnityForFirePerMonth5 = upoints18;
      Upoints minUnityForFlowersPerMonth5 = upoints19;
      Upoints maxUnityForFlowersPerMonth5 = upoints20;
      TombOfCaptainsProto proto5 = new TombOfCaptainsProto(ofCaptainsStage5, strings5, layout5, entityCosts5, graphics5, constructionDurationPerProduct5, fuelConsumptionPerDay5, flowersConsumptionPerDay5, minUnityForFirePerMonth5, maxUnityForFirePerMonth5, minUnityForFlowersPerMonth5, maxUnityForFlowersPerMonth5);
      protosDb5.Add<TombOfCaptainsProto>(proto5).SetAvailability(false);
      ProtosDb protosDb6 = prototypesDb;
      StaticEntityProto.ID captainsStageFinal = Ids.Buildings.TombOfCaptainsStageFinal;
      Proto.Str strings6 = new Proto.Str(name, descShort);
      EntityLayout layout6 = TombOfCaptainsData.createLayout(registrator, true, new KeyValuePair<int, RectangleTerrainArea2i>[9]
      {
        TombOfCaptainsData.BASE_0_AREA,
        TombOfCaptainsData.BASE_1_AREA,
        TombOfCaptainsData.BASE_2_AREA,
        TombOfCaptainsData.TEMPLE_2_AREA,
        TombOfCaptainsData.CUPOLA_1_AREA,
        TombOfCaptainsData.CUPOLA_2_AREA,
        TombOfCaptainsData.ANCHOR_AREA,
        TombOfCaptainsData.CAT_1_AREA,
        TombOfCaptainsData.CAT_2_AREA
      });
      EntityCosts entityCosts6 = Costs.Buildings.TombOfCaptainsFinal.MapToEntityCosts(registrator);
      Duration duration6 = constrDurPerProduct;
      PartialProductQuantity partialProductQuantity3 = 0.125.Of(orThrow1);
      PartialProductQuantity partialProductQuantity4 = 0.15.Of(orThrow2);
      Upoints upoints21 = 0.0.Upoints();
      Upoints upoints22 = 0.5.Upoints();
      Upoints upoints23 = 0.0.Upoints();
      Upoints upoints24 = 0.5.Upoints();
      string prefabPath5 = tombOfCaptainsProto.Graphics.PrefabPath;
      Option<string> iconPath5 = (Option<string>) tombOfCaptainsProto.Graphics.IconPath;
      RelTile3f prefabOrigin5 = new RelTile3f();
      Option<string> customIconPath5 = iconPath5;
      ColorRgba color5 = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers5 = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories5 = new ImmutableArray<ToolbarCategoryProto>?();
      ImmutableArray<string> instancedRenderingExcludedObjects5 = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics6 = new LayoutEntityProto.Gfx(prefabPath5, prefabOrigin5, customIconPath5, color5, visualizedLayers: visualizedLayers5, categories: categories5, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects5);
      Duration constructionDurationPerProduct6 = duration6;
      PartialProductQuantity fuelConsumptionPerDay6 = partialProductQuantity3;
      PartialProductQuantity flowersConsumptionPerDay6 = partialProductQuantity4;
      Upoints minUnityForFirePerMonth6 = upoints21;
      Upoints maxUnityForFirePerMonth6 = upoints22;
      Upoints minUnityForFlowersPerMonth6 = upoints23;
      Upoints maxUnityForFlowersPerMonth6 = upoints24;
      TombOfCaptainsProto proto6 = new TombOfCaptainsProto(captainsStageFinal, strings6, layout6, entityCosts6, graphics6, constructionDurationPerProduct6, fuelConsumptionPerDay6, flowersConsumptionPerDay6, minUnityForFirePerMonth6, maxUnityForFirePerMonth6, minUnityForFlowersPerMonth6, maxUnityForFlowersPerMonth6);
      protosDb6.Add<TombOfCaptainsProto>(proto6).SetAvailability(false);
    }

    public TombOfCaptainsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TombOfCaptainsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TombOfCaptainsData.BASE_0_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(4, new RectangleTerrainArea2i(new Tile2i(0, 0), new RelTile2i(75, 46)));
      TombOfCaptainsData.BASE_1_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(8, new RectangleTerrainArea2i(new Tile2i(2, 4), new RelTile2i(64, 38)));
      TombOfCaptainsData.BASE_2_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(12, new RectangleTerrainArea2i(new Tile2i(7, 9), new RelTile2i(47, 28)));
      TombOfCaptainsData.TEMPLE_1_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(16, new RectangleTerrainArea2i(new Tile2i(12, 12), new RelTile2i(36, 22)));
      TombOfCaptainsData.TEMPLE_2_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(23, new RectangleTerrainArea2i(new Tile2i(11, 12), new RelTile2i(39, 22)));
      TombOfCaptainsData.CUPOLA_1_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(31, new RectangleTerrainArea2i(new Tile2i(12, 14), new RelTile2i(17, 18)));
      TombOfCaptainsData.CUPOLA_2_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(40, new RectangleTerrainArea2i(new Tile2i(19, 20), new RelTile2i(6, 6)));
      TombOfCaptainsData.ANCHOR_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(27, new RectangleTerrainArea2i(new Tile2i(48, 20), new RelTile2i(4, 6)));
      TombOfCaptainsData.CAT_1_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(11, new RectangleTerrainArea2i(new Tile2i(56, 11), new RelTile2i(11, 10)));
      TombOfCaptainsData.CAT_2_AREA = new KeyValuePair<int, RectangleTerrainArea2i>(11, new RectangleTerrainArea2i(new Tile2i(56, 26), new RelTile2i(11, 10)));
      TombOfCaptainsData.EXTRA_TOKENS = new CustomLayoutToken[3]
      {
        new CustomLayoutToken("20]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 20 + h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("30]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 30 + h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("40]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 40 + h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      };
    }
  }
}
