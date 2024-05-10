// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.BasicDieselDistillerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class BasicDieselDistillerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Basic distiller", Ids.Machines.BasicDieselDistiller).Description("Allows distillation of low-grade diesel but it is quite inefficient and produces a lot of waste.", "short description of a machine").SetCost(Costs.Machines.BioDieselDistiller).SetCategories(Ids.ToolbarCategories.MachinesOil).SetLayout("C~>[3][3][3][3][3]>@S", "   [3][3][3][3][3]   ", "   [3][3][3][3][3]   ", "A@>[3][5][5][5][3]>@X", "   [3][5][5][5][3]   ", "   [3][5][5][5][3]>@Z").SetPrefabPath("Assets/Base/Machines/Oil/BasicDistiller.prefab").SetMachineSound("Assets/Base/Machines/Oil/BasicDistiller/BasicDistillerSound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Diesel distillation (basic)", Ids.Recipes.DieselDistillationBasic, machine).AddInput<RecipeProtoBuilder.State>(18, Ids.Products.CrudeOil).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Coal).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Diesel, "X").AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.WasteWater, "Z").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Exhaust, "S").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Water desalination (basic)", Ids.Recipes.WaterDesalinationBasic, machine).AddInput<RecipeProtoBuilder.State>(10, Ids.Products.Seawater).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Coal).SetDurationSeconds(10).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Water, "X").AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Brine, "Z").AddQuarterExhaust<RecipeProtoBuilder.State>("S").BuildAndAdd();
    }

    public BasicDieselDistillerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
