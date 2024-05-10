// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.WaterChillerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class WaterChillerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Water chiller", Ids.Machines.WaterChiller).SetCost(Costs.Machines.WaterChiller).SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout("   [4][4][4][4]   ", "A@>[4][6][6][4]>@X", "B@>[4][6][6][4]>@Y", "   [4][4][4][4]   ").AddParticleParams(ParticlesParams.Loop("Steam", true)).UseAllRecipesAtStartOrAfterUnlock().SetElectricityConsumption(800.Kw()).SetPrefabPath("Assets/Base/Machines/PowerPlant/WaterChiller.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Water cooling", Ids.Recipes.WaterChilling, machine).AddInput<RecipeProtoBuilder.State>(10, Ids.Products.Water).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.ChilledWater).BuildAndAdd();
    }

    public WaterChillerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
