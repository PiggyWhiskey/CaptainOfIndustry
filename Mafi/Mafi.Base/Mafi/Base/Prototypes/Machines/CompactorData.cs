// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.CompactorData
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
  public class CompactorData : IModData
  {
    public const int COMPRESS_MULT = 3;

    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Compactor", Ids.Machines.Compactor).Description("Presses loose items into compact units for easier transportation. A shredder can be used to restore compacted products back to their uncompacted state.", "short description of a machine").SetCost(Costs.Machines.Compactor).SetElectricityConsumption(100.Kw()).SetCategories(Ids.ToolbarCategories.Waste).SetLayout("   [3][3][3][3][3]   ", "A~>[3][3][3][3][3]>#X", "   [3][3][3][3][3]   ").SetPrefabPath("Assets/Base/Machines/Waste/Compactor.prefab").SetAnimationParams((AnimationParams) AnimationParams.PlayOnce(Duration.FromKeyframes(90), true)).EnableSemiInstancedRendering().BuildAndAdd();
      Duration durationTicks = 5.Seconds();
      registrator.RecipeProtoBuilder.Start("Recyclables pressing", Ids.Recipes.PressingOfRecyclables, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Recyclables).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.RecyclablesPressed).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Copper scrap pressing", Ids.Recipes.PressingOfCopperScrap, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.CopperScrap).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.CopperScrapPressed).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Iron scrap pressing", Ids.Recipes.PressingOfIronScrap, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.IronScrap).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.IronScrapPressed).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Gold scrap pressing", Ids.Recipes.PressingOfGoldScrap, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.GoldScrap).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.GoldScrapPressed).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Waste pressing", Ids.Recipes.PressingOfWaste, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Waste).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.WastePressed).BuildAndAdd();
    }

    public CompactorData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
