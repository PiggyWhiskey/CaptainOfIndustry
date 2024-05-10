// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ImmutableCollections.ImmutableArrayBuilder`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Collections.ImmutableCollections
{
  /// <summary>
  /// Provides efficient copy-free construction of <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> when we know its size beforehand.
  /// </summary>
  /// <remarks>This is a struct to prevent unnecessary allocations.</remarks>
  /// <example>
  /// Intended use:
  /// <code>
  /// var builder = new ImmutableArrayBuilder&lt;int&gt;(count);
  /// 
  /// for (...) {
  /// 	builder[i] = ...;
  /// }
  /// 
  /// return builder.GetImmutableArrayAndClear();
  /// </code>
  /// </example>
  public struct ImmutableArrayBuilder<T>
  {
    private T[] m_array;

    /// <summary>Creates new empty array of given size</summary>
    public ImmutableArrayBuilder(int length)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Mafi.Assert.That<int>(length).IsNotNegative();
      this.m_array = length <= 0 ? Array.Empty<T>() : new T[length];
    }

    /// <summary>Gets or sets element on given index.</summary>
    public readonly T this[int i]
    {
      get => this.m_array[i];
      set => this.m_array[i] = value;
    }

    public readonly int Length => this.m_array.Length;

    public readonly T First
    {
      get => this.m_array[0];
      set => this.m_array[0] = value;
    }

    public readonly T Last
    {
      get => this.m_array[this.m_array.Length - 1];
      set => this.m_array[this.m_array.Length - 1] = value;
    }

    /// <summary>
    /// Whether this struct has <c>null</c> as an array instance. We should never need to explicitly check this
    /// property in normal program flow. It is exposed only for asserts and tests. While ctor does not allow
    /// construction with null array it may happen when <c>default( <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArrayBuilder`1" />)</c> is
    /// used.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly bool IsNotValid => this.m_array == null;

    public readonly void Reverse() => Array.Reverse((Array) this.m_array);

    public readonly void Sort(IComparer<T> comparer) => Array.Sort<T>(this.m_array, comparer);

    public readonly void Sort(Comparison<T> comparison)
    {
      if (comparison == null)
        throw new ArgumentNullException(nameof (comparison));
      if (this.m_array.Length == 0)
        return;
      Array.Sort<T>(this.m_array, (IComparer<T>) new Mafi.Collections.ComparisonComparer<T>(comparison));
    }

    /// <summary>
    /// Creates immutable array from the internal array and clear the builder. No further methods can be called on
    /// this builder.
    /// </summary>
    public ImmutableArray<T> GetImmutableArrayAndClear()
    {
      if (this.m_array == null)
      {
        Mafi.Log.Warning("Calling `GetImmutableArrayAndClear` on cleared builder.");
        return ImmutableArray<T>.Empty;
      }
      if (this.m_array.Length == 0)
        return ImmutableArray<T>.Empty;
      ImmutableArray<T> immutableArrayAndClear = new ImmutableArray<T>(this.m_array);
      this.m_array = (T[]) null;
      return immutableArrayAndClear;
    }
  }
}
