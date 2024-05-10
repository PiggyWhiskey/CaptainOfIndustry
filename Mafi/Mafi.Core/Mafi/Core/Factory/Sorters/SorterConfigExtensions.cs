// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Sorters.SorterConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Factory.Sorters
{
  public static class SorterConfigExtensions
  {
    public static ImmutableArray<ProductProto>? GetFilteredProducts(this EntityConfigData data)
    {
      return data.GetProtoArray<ProductProto>("FilteredProducts", false);
    }

    public static void SetFilteredProducts(
      this EntityConfigData data,
      ImmutableArray<ProductProto>? value)
    {
      data.SetProtoArray<ProductProto>("FilteredProducts", value);
    }
  }
}
