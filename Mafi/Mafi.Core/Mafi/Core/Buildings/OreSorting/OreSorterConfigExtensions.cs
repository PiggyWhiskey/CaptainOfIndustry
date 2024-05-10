// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.OreSorterConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Buildings.OreSorting
{
  public static class OreSorterConfigExtensions
  {
    public static bool? GetDoNotAcceptSingleProduct(this EntityConfigData data)
    {
      return data.GetBool("DoNotAcceptSingleProduct");
    }

    public static void SetDoNotAcceptSingleProduct(this EntityConfigData data, bool value)
    {
      data.SetBool("DoNotAcceptSingleProduct", new bool?(value));
    }

    public static ImmutableArray<ProductProto>? GetProductsToNotify(this EntityConfigData data)
    {
      return data.GetProtoArray<ProductProto>("ProductsToNotify", true);
    }

    public static void SetProductsToNotify(
      this EntityConfigData data,
      ImmutableArray<ProductProto> products)
    {
      data.SetProtoArray<ProductProto>("ProductsToNotify", new ImmutableArray<ProductProto>?(products));
    }
  }
}
