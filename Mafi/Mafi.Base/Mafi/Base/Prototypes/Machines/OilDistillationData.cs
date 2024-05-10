// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.OilDistillationData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class OilDistillationData : IModData
  {
    public static readonly Duration DURATION;

    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine1 = registrator.MachineProtoBuilder.Start("Distillation (stage I)", Ids.Machines.DistillationTowerT1).Description("The entry point for advanced crude oil processing. Separates oil into two components for additional processing into useful resources.", "short description of a machine").SetCost(Costs.Machines.DistillationTowerT1).SetCategories(Ids.ToolbarCategories.MachinesOil).SetLayout("   A@v         ^@Y   ", "   [1][2][4][4][1]   ", "   [1][6][6][6][1]   ", "B@>[2][6][6][6][6]>@X", "   [2][6][6][6][1]   ", "   [2][2][2][4][1]   ", "               v@Z   ").SetPrefabPath("Assets/Base/Machines/Oil/DistillationT1.prefab").SetMachineSound("Assets/Base/Machines/Oil/DistillationT1/DistillationT1Sound.prefab").AddParticleParams(ParticlesParams.Loop("Steam", true)).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Crude oil refining", Ids.Recipes.CrudeOilRefiningT1, machine1).AddInput<RecipeProtoBuilder.State>(18, Ids.Products.CrudeOil, "B").AddInput<RecipeProtoBuilder.State>(2, Ids.Products.SteamHi, "A").SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MediumOil, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.HeavyOil, "Z").AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.SourWater, "Y").BuildAndAdd();
      LocStr desc = Loc.Str(Ids.Machines.DistillationTowerT2.ToString() + "__desc", "Introduces extra distillation step to expand oil processing capabilities.", "description of a distillation tower");
      MachineProto machine2 = registrator.MachineProtoBuilder.Start("Distillation (stage II)", Ids.Machines.DistillationTowerT2).Description(desc).SetCost(Costs.Machines.DistillationTowerT2).SetCategories(Ids.ToolbarCategories.MachinesOil).SetLayout("   A@v               ", "   [1][2][4][4]      ", "   [1][6][6][6]      ", "B@>[2][6][8][8][8]>@X", "   [2][6][8][8][7]   ", "   [2][4][6][6][6]   ", "               v@Z   ").SetPrefabPath("Assets/Base/Machines/Oil/DistillationT2.prefab").SetMachineSound("Assets/Base/Machines/Oil/DistillationT1/DistillationT1Sound.prefab").AddParticleParams(ParticlesParams.Loop("Steam", true)).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Crude oil refining", Ids.Recipes.CrudeOilRefiningT2, machine2).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.MediumOil, "B").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SteamHi, "A").SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Diesel, "Z").AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.LightOil, "X").BuildAndAdd();
      MachineProto machine3 = registrator.MachineProtoBuilder.Start("Distillation (stage III)", Ids.Machines.DistillationTowerT3).Description(desc).SetCost(Costs.Machines.DistillationTowerT3).SetCategories(Ids.ToolbarCategories.MachinesOil).SetLayout("   A@v               ", "   [1][7][4][4]      ", "   [1][8][8][8][3]   ", "B@>[2][8][9][9][9]>@X", "   [2][8][9][9][9]   ", "   [2][4][6][8][8]   ", "               v@Z   ").SetPrefabPath("Assets/Base/Machines/Oil/DistillationT3.prefab").SetMachineSound("Assets/Base/Machines/Oil/DistillationT1/DistillationT1Sound.prefab").AddParticleParams(ParticlesParams.Loop("Steam", true)).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Heavy distillate refining", Ids.Recipes.HeavyDistillateRefining, machine3).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.LightOil, "B").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SteamHi, "A").SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Naphtha, "X").AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.FuelGas, "Z").BuildAndAdd();
    }

    public OilDistillationData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static OilDistillationData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      OilDistillationData.DURATION = 20.Seconds();
    }
  }
}
