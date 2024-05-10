// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.ShredderData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class ShredderData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Shredder", Ids.Machines.Shredder).Description("Breaks down products into loose form.", "short description of a machine").SetCost(Costs.Machines.Shredder).SetElectricityConsumption(100.Kw()).SetCategories(Ids.ToolbarCategories.Waste).SetLayout("   [3][3][3]   ", "   [3][3][3]   ", "A#>[4][4][4]>~X", "   [3][3][3]   ", "   [3][3][3]   ", "   [3][3][3]   ").SetPrefabPath("Assets/Base/Machines/Waste/Shredder.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).EnableSemiInstancedRendering().BuildAndAdd();
      Duration durationTicks = 10.Seconds();
      registrator.RecipeProtoBuilder.Start("Shredding wood", Ids.Recipes.ShreddingWood, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Wood).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Woodchips).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Shredding saplings", Ids.Recipes.ShreddingSaplings, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.TreeSapling).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Biomass).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Shredding iron scrap", Ids.Recipes.ShreddingIronScrap, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.IronScrapPressed).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.IronScrap).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Shredding copper scrap", Ids.Recipes.ShreddingCopperScrap, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.CopperScrapPressed).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.CopperScrap).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Shredding gold scrap", Ids.Recipes.ShreddingGoldScrap, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.GoldScrapPressed).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.GoldScrap).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Shredding waste", Ids.Recipes.ShreddingWaste, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.WastePressed).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Waste).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Shredding waste", Ids.Recipes.ShreddingRetiredWaste, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.RetiredWaste).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.Recyclables).DisableSourceProductsConversionLoss().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Shredding poly cells", Ids.Recipes.ShreddingPolyCells, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SolarCell).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.Recyclables).DisableSourceProductsConversionLoss().BuildAndAdd();
    }

    public ShredderData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
