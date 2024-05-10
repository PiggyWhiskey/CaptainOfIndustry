// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ImmutableCollections.SmallImmutableArray`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Mafi.Collections.ImmutableCollections
{
  /// <summary>
  /// Immutable container optimized for having a single element without need for any allocations.
  /// </summary>
  [ManuallyWrittenSerialization]
  public readonly struct SmallImmutableArray<T>
  {
    public static readonly SmallImmutableArray<T> Empty;
    private readonly T m_singleElement;
    private readonly ImmutableArray<T> m_array;

    private bool IsSingleElement => this.m_array.IsNotValid;

    public int Length => !this.IsSingleElement ? this.m_array.Length : 1;

    public bool IsEmpty => this.m_array.IsValid && this.m_array.IsEmpty;

    public bool IsNotEmpty => this.m_array.IsNotValid || this.m_array.IsNotEmpty;

    public T this[int index]
    {
      get
      {
        if (!this.IsSingleElement)
          return this.m_array[index];
        Mafi.Assert.That<int>(index).IsZero();
        return this.m_singleElement;
      }
    }

    public SmallImmutableArray(T singleElement)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_singleElement = singleElement;
      this.m_array = new ImmutableArray<T>();
    }

    public SmallImmutableArray(ImmutableArray<T> array)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_singleElement = default (T);
      this.m_array = array.CheckNotDefaultStruct<ImmutableArray<T>>();
    }

    [Pure]
    public SmallImmutableArray<T>.Enumerator GetEnumerator()
    {
      return new SmallImmutableArray<T>.Enumerator(this);
    }

    /// <summary>
    /// Copies the contents of this array to the specified array.
    /// </summary>
    [Pure]
    public void CopyTo(T[] destination, int destinationIndex = 0)
    {
      if (this.IsSingleElement)
        destination[destinationIndex] = this.m_singleElement;
      else
        this.m_array.CopyTo(destination, destinationIndex);
    }

    /// <summary>
    /// Maps this immutable array using given function. This operation is not lazy like LINQ methods but is very
    /// efficient.
    /// </summary>
    [Pure]
    public SmallImmutableArray<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
      return this.IsSingleElement ? new SmallImmutableArray<TResult>(mapper(this.m_singleElement)) : new SmallImmutableArray<TResult>(this.m_array.Map<TResult>(mapper));
    }

    [Pure]
    public SmallImmutableArray<T> Filter(Predicate<T> predicate)
    {
      if (this.IsSingleElement)
        return !predicate(this.m_singleElement) ? SmallImmutableArray<T>.Empty : this;
      ImmutableArray<T> array = this.m_array.Filter(predicate);
      if (array.Length > 1)
        return new SmallImmutableArray<T>(array);
      return array.IsEmpty ? SmallImmutableArray<T>.Empty : new SmallImmutableArray<T>(array.First);
    }

    [Pure]
    public string Join(string delimiter, Func<T, string> mapper)
    {
      if (this.IsEmpty)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(mapper(this[0]));
      for (int index = 1; index < this.Length; ++index)
      {
        stringBuilder.Append(delimiter);
        stringBuilder.Append(mapper(this[index]));
      }
      return stringBuilder.ToString();
    }

    /// <summary>Searches the array for the specified item.</summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item)
    {
      if (!this.IsSingleElement)
        return this.m_array.IndexOf(item);
      return EqualityComparer<T>.Default.Equals(this.m_singleElement, item) ? 0 : -1;
    }

    [Pure]
    public int IndexOf(Predicate<T> predicate)
    {
      if (!this.IsSingleElement)
        return this.m_array.IndexOf(predicate);
      return predicate(this.m_singleElement) ? 0 : -1;
    }

    [Pure]
    public int Sum(Func<T, int> selector)
    {
      return !this.IsSingleElement ? this.m_array.Sum(selector) : selector(this.m_singleElement);
    }

    [Pure]
    public ImmutableArray<T> ToImmutableArray()
    {
      return this.IsSingleElement ? ImmutableArray.Create<T>(this.m_singleElement) : this.m_array;
    }

    [Pure]
    public Lyst<T> ToLyst()
    {
      if (!this.IsSingleElement)
        return this.m_array.ToLyst();
      return new Lyst<T>(1) { this.m_singleElement };
    }

    public override string ToString()
    {
      return !this.IsSingleElement ? "{ " + ((IEnumerable<string>) this.m_array.MapArray<string>((Func<T, string>) (x => x.ToString()))).JoinStrings(", ") + " }" : string.Format("{{ {0} }}", (object) this.m_singleElement);
    }

    public static void Serialize(SmallImmutableArray<T> value, BlobWriter writer)
    {
      writer.WriteBool(value.IsSingleElement);
      if (value.IsSingleElement)
        writer.GetSerializerFor<T>()(value.m_singleElement, writer);
      else
        ImmutableArray<T>.Serialize(value.m_array, writer);
    }

    public static SmallImmutableArray<T> Deserialize(BlobReader reader)
    {
      return !reader.ReadBool() ? new SmallImmutableArray<T>(ImmutableArray<T>.Deserialize(reader)) : new SmallImmutableArray<T>(reader.GetDeserializerFor<T>()(reader));
    }

    static SmallImmutableArray()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      SmallImmutableArray<T>.Empty = new SmallImmutableArray<T>(ImmutableArray<T>.Empty);
    }

    /// <summary>Enumerator specific for this container.</summary>
    /// <remarks>
    /// It is important that this enumerator does NOT implement <see cref="T:System.IDisposable" />. We want the iterator to
    /// inline when we do foreach and to not result in a try/finally frame in the client.
    /// </remarks>
    [DebuggerStepThrough]
    public struct Enumerator
    {
      private readonly SmallImmutableArray<T> m_container;
      private int m_index;

      internal Enumerator(SmallImmutableArray<T> container)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_container = container;
        this.m_index = -1;
      }

      public T Current => this.m_container[this.m_index];

      public bool MoveNext() => ++this.m_index < this.m_container.Length;
    }
  }
}
