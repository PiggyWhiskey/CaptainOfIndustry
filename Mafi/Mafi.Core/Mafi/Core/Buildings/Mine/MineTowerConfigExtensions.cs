// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Mine.MineTowerConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Buildings.Mine
{
  public static class MineTowerConfigExtensions
  {
    public static ImmutableArray<ProductProto>? GetDumpableProducts(this EntityConfigData data)
    {
      return data.GetProtoArray<ProductProto>("DumpableProducts", false);
    }

    public static void SetDumpableProducts(
      this EntityConfigData data,
      ImmutableArray<ProductProto>? value)
    {
      data.SetProtoArray<ProductProto>("DumpableProducts", value);
    }

    public static ImmutableArray<ProductProto>? GetNotifyOnProducts(this EntityConfigData data)
    {
      return data.GetProtoArray<ProductProto>("NotifyOnProducts", false);
    }

    public static void SetNotifyOnProducts(
      this EntityConfigData data,
      ImmutableArray<ProductProto>? value)
    {
      data.SetProtoArray<ProductProto>("NotifyOnProducts", value);
    }
  }
}
