// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.SourWaterStripperData
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
  internal class SourWaterStripperData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      string description = "Makes sour water useful by extracting ammonia and sulfur. Sulfur can be turned into acid and used for instance in the production of copper";
      MachineProto machine = registrator.MachineProtoBuilder.Start("Sour water stripper", Ids.Machines.SourWaterStripper).Description(description, "short description of a machine").SetCost(Costs.Machines.SourWaterStripper).SetElectricityConsumption(160.Kw()).SetCategories(Ids.ToolbarCategories.MachinesOil).SetLayout("         [3][3][3][2][6][6][6]   ", "   [4][4][3][4][4][2][6][6][6]>@Y", "   [4][4][3][4][4][2][6][6][6]   ", "A@>[2][2][2][2][2][2][2][2]      ", "   [2][2][2][2][2]   v@Xv~Z      ", "   B@^                           ").SetPrefabPath("Assets/Base/Machines/Oil/SourWaterStripper.prefab").SetMachineSound("Assets/Base/Machines/Oil/SourWaterStripper/SourWaterStripperSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Sour water stripping (recovery)", Ids.Recipes.SourWaterStripping, machine).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.SourWater).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SteamHi).SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Sulfur, "Z").AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.Ammonia, "X").AddOutput<RecipeProtoBuilder.State>(7, Ids.Products.Water, "Y").BuildAndAdd();
    }

    public SourWaterStripperData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
