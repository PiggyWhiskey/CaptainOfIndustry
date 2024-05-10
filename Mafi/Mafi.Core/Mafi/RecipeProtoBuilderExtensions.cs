// Decompiled with JetBrains decompiler
// Type: Mafi.RecipeProtoBuilderExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi
{
  public static class RecipeProtoBuilderExtensions
  {
    /// <summary>
    /// Selector is a string of port names (A,B,C..) or "*" for any port.
    /// </summary>
    /// <example>Selector: "*" - selects all ports, "AB" - selects ports A and B.</example>
    [MustUseReturnValue]
    public static T AddInput<T>(
      this IRecipeProtoBuilderState<T> builder,
      string portSelector,
      ProductProto.ID productId,
      Quantity quantity)
    {
      ProductProto orThrow = builder.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) productId);
      return builder.AddInput(portSelector, orThrow, quantity);
    }

    [MustUseReturnValue]
    public static T AddInput<T>(
      this IRecipeProtoBuilderState<T> builder,
      int quantity,
      ProductProto.ID productId,
      string portSelector = "*",
      bool hideInUi = false)
    {
      ProductProto orThrow = builder.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) productId);
      return builder.AddInput(portSelector, orThrow, new Quantity(quantity), hideInUi);
    }

    [MustUseReturnValue]
    public static T AddInput<T>(this IRecipeProtoBuilderState<T> builder, MechPower mechPower)
    {
      return builder.AddInput<T>("VIRTUAL", IdsCore.Products.MechanicalPower, mechPower.Quantity);
    }

    /// <summary>
    /// Selector is a string of port names (A,B,C..) or "*" for any port.
    /// </summary>
    /// <example>Selector: "*" - selects all ports, "AB" - selects ports A and B.</example>
    [MustUseReturnValue]
    public static T AddOutput<T>(
      this IRecipeProtoBuilderState<T> builder,
      string portSelector,
      ProductProto.ID productId,
      Quantity quantity,
      bool outputAtStart = false,
      bool hideInUi = false)
    {
      ProductProto orThrow = builder.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) productId);
      return builder.AddOutput(portSelector, orThrow, quantity, outputAtStart, hideInUi);
    }

    [MustUseReturnValue]
    public static T AddOutput<T>(
      this IRecipeProtoBuilderState<T> builder,
      int quantity,
      ProductProto.ID productId,
      string portSelector = "*",
      bool outputAtStart = false,
      bool hideInUi = false)
    {
      ProductProto orThrow = builder.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) productId);
      return builder.AddOutput(portSelector, orThrow, new Quantity(quantity), outputAtStart, hideInUi);
    }

    [MustUseReturnValue]
    public static T AddOutput<T>(this IRecipeProtoBuilderState<T> builder, MechPower mechPower)
    {
      return builder.AddOutput<T>("VIRTUAL", IdsCore.Products.MechanicalPower, mechPower.Quantity, true);
    }
  }
}
