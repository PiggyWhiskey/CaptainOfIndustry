// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.FermentationTankData
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
  internal class FermentationTankData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Fermentation tank", Ids.Machines.FermentationTank).Description("Employs microorganisms to convert sugars into other useful substances such as ethanol. The microorganisms employed here are not getting any salary (they just love sugar).", "short description of a machine").SetCost(Costs.Machines.FermentationTank).SetElectricityConsumption(20.Kw()).SetCategories(Ids.ToolbarCategories.Machines).SetLayout("   [5][5][5][5][5][5][5][5]   ", "A@>[6][6][6][6][6][6][6][6]>#X", "B@>[6][6][6][6][6][6][6][6]>@Y", "   [6][6][6][6][6][6][6][6]   ", "C~>[6][6][6][6][6][6][6][6]>@Z", "   [5][5][5][5][5][5][5][5]   ").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(70.Percent()))).SetPrefabPath("Assets/Base/Machines/General/FermentationTank.prefab").SetMachineSound("Assets/Base/Machines/General/FermentationTank/FermentationTankSound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Ethanol making (sugar)", Ids.Recipes.SugarToEthanolFermentation, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Sugar).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Oxygen).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Ethanol, "Y").AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.CarbonDioxide, "Z").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Ethanol making (corn)", Ids.Recipes.CornToEthanolFermentation, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.CornMash).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Oxygen).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.Ethanol, "Y").AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.CarbonDioxide, "Z").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Antibiotics", Ids.Recipes.AntibioticsFermentation, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Sugar).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Ammonia).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Oxygen).SetDurationSeconds(80).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Antibiotics).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.CarbonDioxide, "Z").BuildAndAdd();
    }

    public FermentationTankData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
