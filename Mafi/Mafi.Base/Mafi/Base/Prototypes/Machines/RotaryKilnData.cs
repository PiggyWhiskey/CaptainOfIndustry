// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.RotaryKilnData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class RotaryKilnData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      LocStr desc = Loc.Str(Ids.Machines.RotaryKiln.ToString() + "__desc", "Performs calcination process under high temperatures. Used to produce cement.", "description of a machine");
      MachineProto machineProto = registrator.MachineProtoBuilder.Start("Rotary Kiln (gas)", Ids.Machines.RotaryKilnGas).Description(desc).SetCost(Costs.Machines.RotaryKilnGas).SetElectricityConsumption(100.Kw()).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout("A~>[7][7][7][6][6][2][2][2][2][2][2][2][2][2]   ", "   [7][7][7][6][6][2][2][2][2][2][2][2][2][2]>#X", "   [2][2][2][2][2][2][2][2][2][2][2][3][2][2]   ", "B@>[2][2][2][2][2][2][2][2][2][2][2][3][2][2]>@E").SetEmissionWhenWorking(2).SetPrefabPath("Assets/Base/Machines/MetalWorks/RotaryKilnGas.prefab").SetMachineSound("Assets/Base/Machines/Infrastructure/ConcreteMixer/ConcreteMixer_Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Cement production", Ids.Recipes.CementProductionGas, machineProto).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.Limestone, "A").AddInput<RecipeProtoBuilder.State>(4, Ids.Products.FuelGas, "B").SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Cement).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.CarbonDioxide).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Cement production", Ids.Recipes.CementProductionHydrogen, machineProto).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.Limestone, "A").AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Hydrogen, "B").SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Cement).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.CarbonDioxide).BuildAndAdd();
      MachineProto machine = registrator.MachineProtoBuilder.Start("Rotary Kiln", Ids.Machines.RotaryKiln).Description(desc).SetCost(Costs.Machines.RotaryKiln).SetNextTier(machineProto).SetElectricityConsumption(100.Kw()).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout("A~>[7][7][7][6][6][2][2][2][2][2][2][2][2][2]   ", "   [7][7][7][6][6][2][2][2][2][2][2][2][2][2]>#X", "   [2][2][2][2][2][2][2][2][2][2][2][3][2][2]   ", "B~>[2][2][2][2][2][2][2][2][2][2][2][3][2][2]>@E").SetEmissionWhenWorking(2).SetPrefabPath("Assets/Base/Machines/MetalWorks/RotaryKiln.prefab").SetMachineSound("Assets/Base/Machines/Infrastructure/ConcreteMixer/ConcreteMixer_Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Cement production", Ids.Recipes.CementProduction, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Limestone, "A").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Coal, "B").SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Cement).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Exhaust).BuildAndAdd();
    }

    public RotaryKilnData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
