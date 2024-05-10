// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ImmutableCollections.IImmutableArray
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi.Collections.ImmutableCollections
{
  /// <summary>
  /// An internal non-generic interface implemented by <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> that allows for recognition of
  /// an <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> instance and access to its underlying array, without actually knowing the type
  /// of value stored in it.
  /// </summary>
  /// <remarks>
  /// Casting to this interface requires a boxed instance of the <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> struct, and as such
  /// should be avoided. This interface is useful, however, where the value is already boxed and we want to try to
  /// reuse immutable arrays instead of copying them.
  /// ** This interface is INTENTIONALLY INTERNAL, as it gives access to the inner array. **
  /// </remarks>
  internal interface IImmutableArray
  {
    Type ElementType { get; }

    /// <summary>Gets an untyped reference to the array.</summary>
    Array Array { get; }
  }
}
