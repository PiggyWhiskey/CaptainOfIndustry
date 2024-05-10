// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Recipes.IRecipeProtoBuilderState`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Recipes
{
  public interface IRecipeProtoBuilderState<TState>
  {
    ProtosDb ProtosDb { get; }

    Duration? RecipeDuration { get; }

    /// <summary>
    /// Selector is a string of port names (A,B,C..) or "*" for any port.
    /// </summary>
    /// <example>Selector: "*" - selects all ports, "AB" - selects ports A and B.</example>
    [MustUseReturnValue]
    TState AddInput(string portSelector, ProductProto product, Quantity quantity, bool hideInUi = false);

    /// <summary>
    /// Selector is a string of port names (A,B,C..) or "*" for any port.
    /// </summary>
    /// <example>Selector: "*" - selects all ports, "AB" - selects ports A and B.</example>
    [MustUseReturnValue]
    TState AddOutput(
      string portSelector,
      ProductProto product,
      Quantity quantity,
      bool outputAtStart = false,
      bool hideInUi = false);
  }
}
