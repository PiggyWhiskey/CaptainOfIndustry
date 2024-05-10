// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Recipes.RecipeForUiData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Recipes
{
  /// <summary>
  /// Simple non-saveable implementation of IRecipeForUi for protos.
  /// </summary>
  public class RecipeForUiData : IRecipeForUi
  {
    public Proto.ID Id { get; }

    public ImmutableArray<RecipeInput> AllUserVisibleInputs { get; }

    public ImmutableArray<RecipeOutput> AllUserVisibleOutputs { get; }

    public Duration Duration { get; }

    public RecipeForUiData(
      Duration duration,
      ImmutableArray<RecipeInput> allInputs,
      ImmutableArray<RecipeOutput> allOutputs)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Id = new Proto.ID(nameof (RecipeForUiData));
      this.Duration = duration.CheckPositive();
      this.AllUserVisibleInputs = allInputs;
      this.AllUserVisibleOutputs = allOutputs;
    }
  }
}
