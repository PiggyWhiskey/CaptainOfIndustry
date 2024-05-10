// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.WaterDesalinationPlantData
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
  internal class WaterDesalinationPlantData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Thermal desalinator", Ids.Machines.ThermalDesalinator).Description("Heats saline water into vapor to collect it as clean water. Produces brine as a byproduct.", "short description of a machine").SetCost(Costs.Machines.ThermalDesalinator).SetElectricityConsumption(400.Kw()).SetCategories(Ids.ToolbarCategories.MachinesWater).DisableLogisticsByDefault().UseAllRecipesAtStartOrAfterUnlock().SetLayout("A@>[3][3][3][3][3][3][3][3][3][3][3]>@W", "   [3][3][3][3][3][3][3][4][3][3][3]   ", "   [4][4][4][4][4][4][4][4][4][4][3]>@X", "B@>[3][3][3][3][3][3][3][3][3][3][3]   ").SetAnimationParams((AnimationParams) AnimationParams.Loop()).SetPrefabPath("Assets/Base/Machines/Water/ThermalDesalinator.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Thermal desalination", Ids.Recipes.DesalinationFromSP, machine).AddInput<RecipeProtoBuilder.State>(18, Ids.Products.Seawater, "A").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SteamSp, "B").SetDurationSeconds(10).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Water, "W").AddOutput<RecipeProtoBuilder.State>(7, Ids.Products.Brine, "X").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Thermal desalination", Ids.Recipes.DesalinationFromHP, machine).AddInput<RecipeProtoBuilder.State>(18, Ids.Products.Seawater, "A").AddInput<RecipeProtoBuilder.State>(2, Ids.Products.SteamHi, "B").SetDurationSeconds(10).AddOutput<RecipeProtoBuilder.State>(13, Ids.Products.Water, "W").AddOutput<RecipeProtoBuilder.State>(7, Ids.Products.Brine, "X").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Thermal desalination", Ids.Recipes.DesalinationFromLP, machine).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.Seawater, "A").AddInput<RecipeProtoBuilder.State>(4, Ids.Products.SteamLo, "B").SetDurationSeconds(10).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Water, "W").AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Brine, "X").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Thermal desalination", Ids.Recipes.DesalinationFromDepleted, machine).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Seawater, "A").AddInput<RecipeProtoBuilder.State>(8, Ids.Products.SteamDepleted, "B").SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(11, Ids.Products.Water, "W").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Brine, "X").BuildAndAdd();
    }

    public WaterDesalinationPlantData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
