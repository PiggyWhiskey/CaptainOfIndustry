// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.BricksMakerData
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
  internal class BricksMakerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Brickworks", Ids.Machines.BricksMaker).Description("Extracts clay out of dirt and produces bricks.", "short description of a machine").SetCost(Costs.Machines.BricksMaker).SetCategories(Ids.ToolbarCategories.Machines).SetLayout("   [3][3][3][3][3][3]>@Y", "A~>[4][4][4][4][4][4]   ", "   [4][4][4][4][4][4]>#X", "B~>[4][4][4][4][4][4]   ", "   [3][3][3][3][3][3]   ", "C@>[3][3][3][3][3][3]   ").SetPrefabPath("Assets/Base/Machines/Infrastructure/BricksMaker.prefab").SetEmissionWhenWorking(3).SetMachineSound("Assets/Base/Machines/MetalWorks/CharcoalMaker/CharcoalMakerSound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Bricks making", Ids.Recipes.BricksMaking, machine).AddInput<RecipeProtoBuilder.State>(14, Ids.Products.Dirt).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Coal).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Water).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Bricks).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Exhaust).BuildAndAdd();
    }

    public BricksMakerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
