// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Recipes.IRecipeForUi
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Recipes
{
  /// <summary>Lightweight interface for UI.</summary>
  public interface IRecipeForUi
  {
    /// <summary>
    /// Recipe ID that uniquely identifies a recipe. This may be implemented in any way suitable,
    /// undefined proto ID or empty ID can be fine in some situations.
    /// </summary>
    Proto.ID Id { get; }

    /// <summary>All inputs visible to the user.</summary>
    ImmutableArray<RecipeInput> AllUserVisibleInputs { get; }

    /// <summary>All outputs visible to the user.</summary>
    ImmutableArray<RecipeOutput> AllUserVisibleOutputs { get; }

    /// <summary>Number of updates it takes to transform the products</summary>
    Duration Duration { get; }
  }
}
