// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ReadonlyCollections.ReadOnlyArray
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Collections.ReadonlyCollections
{
  /// <summary>
  /// A set of initialization methods for instances of <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1" />.
  /// </summary>
  public static class ReadOnlyArray
  {
    /// <summary>
    /// Returns read-only reference to this array. Note that this does not do defensive copy and underlying array can
    /// be mutated. Use <see cref="M:Mafi.Collections.ImmutableCollections.ImmutableArray.ToImmutableArray``1(System.Collections.Generic.IEnumerable{``0})" /> for truly
    /// immutable array.
    /// </summary>
    public static ReadOnlyArray<T> AsReadOnlyArray<T>(this T[] array)
    {
      return new ReadOnlyArray<T>(array);
    }
  }
}
