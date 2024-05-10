// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Recipes.RecipeProduct
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Factory.Recipes
{
  /// <summary>
  /// Definition of input / output of <see cref="T:Mafi.Core.Factory.Recipes.RecipeProto" />.
  /// </summary>
  public abstract class RecipeProduct
  {
    public readonly ProductProto Product;
    public readonly Quantity Quantity;
    /// <summary>
    /// Ports of a machine / building that should be used for the input / output.
    /// </summary>
    public readonly ImmutableArray<IoPortTemplate> Ports;
    /// <summary>
    /// Whether the product should be hidden in the UI (when displaying recipe to the player).
    /// </summary>
    public readonly bool HideInUi;
    public readonly bool IsPollution;

    public RecipeProduct(
      ImmutableArray<IoPortTemplate> ports,
      ProductProto product,
      Quantity quantity,
      bool hideInUi = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.HideInUi = hideInUi;
      this.Ports = ports.CheckNotDefaultStruct<ImmutableArray<IoPortTemplate>>();
      this.Product = product.CheckNotNull<ProductProto>();
      this.Quantity = quantity.CheckNotNegative();
      this.IsPollution = product.Id.Value == "Product_Virtual_PollutedAir" || product.Id.Value == "Product_Virtual_PollutedWater";
    }
  }
}
