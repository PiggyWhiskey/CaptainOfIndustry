// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.CharcoalMakerData
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
  internal class CharcoalMakerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Coal maker", Ids.Machines.CharcoalMaker).Description("Uses wood to create coal but it is quite inefficient.", "short description of a machine").SetCost(Costs.Machines.CharcoalMaker).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(Duration.FromKeyframes(80), AnimationWithPauseParams.Mode.ExtendPauseToFit, Duration.FromKeyframes(42))).UseAllRecipesAtStartOrAfterUnlock().SetLayout("   [3][3][3][3][3]>@S", "A#>[3][3][3][3][3]>~X", "   [3][3][3][3][3]   ").SetPrefabPath("Assets/Base/Machines/MetalWorks/CharcoalMaker.prefab").SetMachineSound("Assets/Base/Machines/MetalWorks/CharcoalMaker/CharcoalMakerSound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Charcoal making", Ids.Recipes.CharcoalBurning, machine).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.Wood).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(5, Ids.Products.Coal).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Exhaust).BuildAndAdd();
    }

    public CharcoalMakerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
