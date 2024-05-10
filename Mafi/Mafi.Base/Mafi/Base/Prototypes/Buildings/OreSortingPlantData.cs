// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.OreSortingPlantData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class OreSortingPlantData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      StaticEntityProto.ID oreSortingPlantT1 = Ids.Buildings.OreSortingPlantT1;
      Proto.Str str = Proto.CreateStr((Proto.ID) Ids.Buildings.OreSortingPlantT1, "Ore sorting plant", "This facility handles sorting of mixed materials loaded onto your trucks by excavators. This is required as trucks can't directly deliver mixed loads to storage units or buildings.");
      EntityLayout layoutOrThrow = registrator.LayoutParser.ParseLayoutOrThrow("   (2)(2)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)               ", "<~V(2)(2)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "   (2)(2)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "<~W(2)(2)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "   (2)(2)(5)(5)(6)(6)(6)(6)(6)(6)(6)(6)(6)(6)(4)(4)(4)(4)", "<~X(2)(2)(5)(5)(6)(6)(6)(6)(6)(6)(6)(6)(6)(6)(4)(4)(4)(4)", "   (2)(2)(5)(5)(6)(6)(6)(6)(6)(6)(6)(6)(6)(6)(4)(4)(4)(4)", "<~Y(2)(2)(4)(4)(4)(4)(4)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)", "   (2)(2)(4)(4)(4)(4)(4)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)", "   (2)(2)(4)(4)(4)(4)(4)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)", "   (2)(2)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)");
      EntityCosts entityCosts = Costs.Buildings.OreSortingPlantT1.MapToEntityCosts(registrator);
      Quantity inputBufferCapacity = 360.Quantity();
      Quantity outputBuffersCapacity = 360.Quantity();
      Duration duration = 5.Seconds();
      Quantity quantityPerDuration = 30.Quantity();
      Percent conversionLoss = 0.Percent();
      Electricity electricityConsumed = 100.Kw();
      ImmutableArray<AnimationParams> animationParams = ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop());
      LoosePileTextureParams loosePileTextureParams = new LoosePileTextureParams(0.9f);
      Option<string> none = Option<string>.None;
      ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.BuildingsForVehicles));
      RelTile3f prefabOrigin = new RelTile3f();
      Option<string> customIconPath = none;
      ColorRgba color = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories = nullable;
      OreSortingPlantProto.Gfx graphics = new OreSortingPlantProto.Gfx("Assets/Base/Buildings/OreSorterT1.prefab", "Pile_Soft", loosePileTextureParams, prefabOrigin, customIconPath, color, visualizedLayers: visualizedLayers, categories: categories);
      OreSortingPlantProto proto = new OreSortingPlantProto(oreSortingPlantT1, str, layoutOrThrow, entityCosts, inputBufferCapacity, outputBuffersCapacity, duration, quantityPerDuration, conversionLoss, electricityConsumed, animationParams, graphics);
      prototypesDb.Add<OreSortingPlantProto>(proto).SetAvailability(false);
    }

    public OreSortingPlantData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
