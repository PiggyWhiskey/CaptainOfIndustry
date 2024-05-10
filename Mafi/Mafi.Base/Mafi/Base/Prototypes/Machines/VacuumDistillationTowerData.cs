// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.VacuumDistillationTowerData
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
  internal class VacuumDistillationTowerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Rubber maker", Ids.Machines.VacuumDistillationTower).Description("Produces synthetic rubber.", "short description of a machine").SetCost(Costs.Machines.VacuumDistillationTower).SetCategories(Ids.ToolbarCategories.MachinesOil).SetElectricityConsumption(300.Kw()).SetLayout("A~>[3][3][3][4][4][4]   ", "   [3][5][5][5][5][4]   ", "B@>[3][6][6][6][6][5]   ", "[1][3][6][6][7][6][5]>#X", "[1][3][6][6][7][6][5]   ", "[1][3][6][6][7][6][2]   ", "[1][3][3][3][7][2][2]>@Y", "      [3][3][3][2][2]   ").SetPrefabPath("Assets/Base/Machines/Oil/VacuumDistillation.prefab").SetMachineSound("Assets/Base/Machines/Oil/VacuumDistillation/VacuumDistillationSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).EnableSemiInstancedRendering().BuildAndAdd();
      int quantity1 = 4;
      int quantity2 = 1;
      int quantity3 = 1;
      int quantity4 = 6;
      registrator.RecipeProtoBuilder.Start("Rubber production", Ids.Recipes.RubberProductionNaphtha, machine).SetDuration(20.Seconds()).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Naphtha).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.Sulfur).AddOutput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Rubber, "X").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Rubber production (coal)", Ids.Recipes.RubberProductionNaphthaAlt, machine).SetDuration(30.Seconds()).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Naphtha).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.Coal).AddOutput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Rubber, "X").AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.WasteWater, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Rubber production (ethanol)", Ids.Recipes.RubberProductionEthanol, machine).SetDuration(20.Seconds()).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Ethanol).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.Sulfur).AddOutput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Rubber, "X").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Rubber production (alt)", Ids.Recipes.RubberProductionDiesel, machine).SetDuration(20.Seconds()).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Diesel).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.Sulfur).AddOutput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Rubber, "X").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Rubber production (coal) (alt)", Ids.Recipes.RubberProductionDieselWithCoal, machine).SetDuration(30.Seconds()).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Diesel).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.Coal).AddOutput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Rubber, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.WasteWater, "Y").BuildAndAdd();
    }

    public VacuumDistillationTowerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
