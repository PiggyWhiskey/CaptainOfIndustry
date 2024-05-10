// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PolymerizationPlantData
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
  internal class PolymerizationPlantData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Polymerization plant", Ids.Machines.PolymerizationPlant).SetCost(Costs.Machines.PolymerizationPlant).SetElectricityConsumption(400.Kw()).SetCategories(Ids.ToolbarCategories.MachinesOil).SetLayout("            ^@Y               ", "A@>[5][5][5][2][2]            ", "   [5][6][6][6][2][2][5][5]   ", "B@>[5][5][5][2][2][2][5][5]>#X", "         [5][5][5][4][5][5]   ", "      C@>[5][5][5][4][3][2]   ", "         [5][5][5][4][3][2]   ", "         [5][5][5][4][3][2]   ").SetPrefabPath("Assets/Base/Machines/Oil/PolymerizationPlant.prefab").SetMachineSound("Assets/Base/Machines/Oil/PolymerizationPlant/PolymerizationPlantSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Plastic making", Ids.Recipes.PlasticMaking, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Naphtha).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Chlorine, "C").SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.Plastic, "X").AddHalfExhaust<RecipeProtoBuilder.State>().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Plastic making", Ids.Recipes.PlasticMakingEthanol, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Ethanol).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Chlorine, "C").SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.Plastic, "X").AddHalfExhaust<RecipeProtoBuilder.State>().BuildAndAdd();
    }

    public PolymerizationPlantData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
