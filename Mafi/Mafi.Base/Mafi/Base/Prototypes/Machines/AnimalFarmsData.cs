// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.AnimalFarmsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class AnimalFarmsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto orThrow1 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.AnimalFeed);
      ProductProto orThrow2 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.CleanWater);
      ProductProto orThrow3 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Eggs);
      ProtosDb protosDb = prototypesDb;
      StaticEntityProto.ID chickenFarm = Ids.Buildings.ChickenFarm;
      Proto.Str str = Proto.CreateStr((Proto.ID) Ids.Buildings.ChickenFarm, "Chicken farm", "Enables to raise chickens for eggs and meat. Chickens need to be provided with water and animal feed. To obtain some chickens you can trade with a village on the world map.");
      EntityLayout layoutOrThrow = registrator.LayoutParser.ParseLayoutOrThrow("   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]>X#", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]>Y#", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ", "~A>[4][4][4][6][6][6][6][6][3][3][3][3][3][3][3][3]   ", "@B>[4][4][4][6][6][6][6][6]                           ", "   [4][4][4][6][6][6][6][6]                           ");
      EntityCosts entityCosts = Costs.Buildings.ChickenFarm.MapToEntityCosts(registrator);
      Option<AnimalFarmProto> none = Option<AnimalFarmProto>.None;
      VirtualProductProto orThrow4 = prototypesDb.GetOrThrow<VirtualProductProto>((Proto.ID) Ids.Products.Chicken);
      ProductProto orThrow5 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.ChickenCarcass);
      Fix32 animalsBornPer100AnimalsPerMonth = (Fix32) 4;
      Fix32 fix32 = 0.5.ToFix32();
      PartialProductQuantity foodPerAnimalPerMonth = new PartialProductQuantity(orThrow1, new PartialQuantity(0.03.ToFix32()));
      PartialProductQuantity waterPerAnimalPerMonth = new PartialProductQuantity(orThrow2, new PartialQuantity(0.036.ToFix32()));
      PartialProductQuantity? producedPerAnimalPerMonth = new PartialProductQuantity?(new PartialProductQuantity(orThrow3, new PartialQuantity(0.015.ToFix32())));
      Quantity foodBufferCapacity = 120.Quantity();
      Quantity waterBufferCapacity = 120.Quantity();
      Quantity carcassBufferCapacity = 60.Quantity();
      Quantity producedBufferCapacity = 60.Quantity();
      ImmutableArray<AnimationParams> animationParams = ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop());
      RelTile3f prefabOrigin = new RelTile3f((Fix32) 0, (Fix32) -1, (Fix32) 0);
      ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesFood));
      Option<string> customIconPath = new Option<string>();
      ColorRgba color = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories = nullable;
      ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics = new LayoutEntityProto.Gfx("Assets/Base/Buildings/ChickenFarm.prefab", prefabOrigin, customIconPath, color, visualizedLayers: visualizedLayers, categories: categories, useSemiInstancedRendering: true, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
      AnimalFarmProto proto = new AnimalFarmProto(chickenFarm, str, layoutOrThrow, entityCosts, none, orThrow4, 500, orThrow5, 'X', animalsBornPer100AnimalsPerMonth, fix32, foodPerAnimalPerMonth, waterPerAnimalPerMonth, producedPerAnimalPerMonth, foodBufferCapacity, waterBufferCapacity, carcassBufferCapacity, producedBufferCapacity, animationParams, graphics);
      protosDb.Add<AnimalFarmProto>(proto);
    }

    public AnimalFarmsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
