// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.MachineUtils
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Factory.Recipes;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Factory.Machines
{
  internal static class MachineUtils
  {
    internal static bool AreRecipesSame(RecipeProto first, RecipeProto second)
    {
      foreach (RecipeInput allInput in first.AllInputs)
      {
        RecipeInput input = allInput;
        if (!second.AllInputs.Any((Func<RecipeInput, bool>) (x => (Proto) x.Product == (Proto) input.Product)))
          return false;
      }
      foreach (RecipeInput allInput in second.AllInputs)
      {
        RecipeInput input = allInput;
        if (!first.AllInputs.Any((Func<RecipeInput, bool>) (x => (Proto) x.Product == (Proto) input.Product)))
          return false;
      }
      foreach (RecipeOutput allOutput in first.AllOutputs)
      {
        RecipeOutput output = allOutput;
        if (!second.AllOutputs.Any((Func<RecipeOutput, bool>) (x => (Proto) x.Product == (Proto) output.Product)))
          return false;
      }
      foreach (RecipeOutput allOutput in second.AllOutputs)
      {
        RecipeOutput output = allOutput;
        if (!first.AllOutputs.Any((Func<RecipeOutput, bool>) (x => (Proto) x.Product == (Proto) output.Product)))
          return false;
      }
      return true;
    }
  }
}
