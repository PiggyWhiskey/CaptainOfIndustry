// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.RecipeUnlock
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  /// <summary>
  /// Unlocks <see cref="T:Mafi.Core.Factory.Recipes.RecipeProto" />.
  /// </summary>
  public class RecipeUnlock : ProtoUnlock
  {
    /// <summary>Proto to be unlocked.</summary>
    public readonly RecipeProto Proto;
    /// <summary>
    /// Machine that can execute the unlocked recipe. Used only to inform the player in UI.
    /// </summary>
    public readonly MachineProto MachineProto;

    public RecipeUnlock(RecipeProto recipeProto, MachineProto machineProto, bool hideInUi)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(((IEnumerable<IProto>) recipeProto.AllInputs.Select<ProductProto>((Func<RecipeInput, ProductProto>) (x => x.Product)).Concat<ProductProto>(recipeProto.AllOutputs.Select<ProductProto>((Func<RecipeOutput, ProductProto>) (x => x.Product)))).Concat<IProto>((IEnumerable<IProto>) new IProto[1]
      {
        (IProto) recipeProto
      }).ToImmutableArray<IProto>(), hideInUi);
      this.Proto = recipeProto.CheckNotNull<RecipeProto>();
      this.MachineProto = machineProto.CheckNotNull<MachineProto>();
    }
  }
}
