// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Recipes.RecipeInput
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
  public class RecipeInput : RecipeProduct
  {
    public RecipeInput(ProductProto product, Quantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((ImmutableArray<IoPortTemplate>) ImmutableArray.Empty, product, quantity);
    }

    public RecipeInput(
      ImmutableArray<IoPortTemplate> ports,
      ProductProto product,
      Quantity quantity,
      bool hideInUi = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(ports, product, quantity, hideInUi);
    }
  }
}
