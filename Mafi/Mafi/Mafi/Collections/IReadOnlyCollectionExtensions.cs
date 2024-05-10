// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.IReadOnlyCollectionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  public static class IReadOnlyCollectionExtensions
  {
    public static bool IsNotEmpty<T>(this IReadOnlyCollection<T> collection)
    {
      return collection.Count > 0;
    }

    public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection) => collection.Count <= 0;
  }
}
