// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Recipes.IRecipeExecutorForUi
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Factory.Recipes
{
  public interface IRecipeExecutorForUi
  {
    bool IsBoosted { get; }

    bool WorkedThisTick { get; }

    Percent DurationMultiplier { get; }

    bool HasClearProductsActionFor(IRecipeForUi recipe);

    Percent ProgressOnRecipe(IRecipeForUi recipe);

    Duration GetTargetDurationFor(IRecipeForUi recipe);

    Quantity GetInputQuantityFor(ProductProto product);

    Quantity GetInputCapacityFor(ProductProto product);

    Quantity GetOutputQuantityFor(ProductProto product);

    Quantity GetOutputCapacityFor(ProductProto product);
  }
}
