// Decompiled with JetBrains decompiler
// Type: <>z__ReadOnlyArray`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ut12pZTdSUNA6wM24P;

#nullable disable
internal sealed class \u003C\u003Ez__ReadOnlyArray<T> : 
  IEnumerable,
  ICollection,
  IList,
  IEnumerable<T>,
  IReadOnlyCollection<T>,
  IReadOnlyList<T>,
  ICollection<T>,
  IList<T>
{
  public \u003C\u003Ez__ReadOnlyArray(T[] items)
  {
    xxhJUtQyC9HnIshc6H.OukgcisAbr();
    // ISSUE: explicit constructor call
    base.\u002Ector();
    // ISSUE: reference to a compiler-generated field
    this._items = items;
  }

  IEnumerator IEnumerable.GetEnumerator() => this._items.GetEnumerator();

  int ICollection.Count => this._items.Length;

  bool ICollection.IsSynchronized => false;

  object ICollection.SyncRoot => (object) this;

  void ICollection.CopyTo(Array array, int index) => this._items.CopyTo(array, index);

  [IndexerName("System.Collections.IList.this[]")]
  object IList.this[int index]
  {
    get => (object) this._items[index];
    set => throw new NotSupportedException();
  }

  bool IList.IsFixedSize => true;

  bool IList.IsReadOnly => true;

  int IList.Add(object value) => throw new NotSupportedException();

  void IList.Clear() => throw new NotSupportedException();

  bool IList.Contains(object value) => this._items.Contains(value);

  int IList.IndexOf(object value) => this._items.IndexOf(value);

  void IList.Insert(int index, object value) => throw new NotSupportedException();

  void IList.Remove(object value) => throw new NotSupportedException();

  void IList.RemoveAt(int index) => throw new NotSupportedException();

  IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>) this._items).GetEnumerator();

  int IReadOnlyCollection<T>.Count => this._items.Length;

  [IndexerName("System.Collections.Generic.IReadOnlyList<T>.this[]")]
  T IReadOnlyList<T>.this[int index] => this._items[index];

  int ICollection<T>.Count => this._items.Length;

  bool ICollection<T>.IsReadOnly => true;

  void ICollection<T>.Add(T item) => throw new NotSupportedException();

  void ICollection<T>.Clear() => throw new NotSupportedException();

  bool ICollection<T>.Contains(T item) => ((ICollection<T>) this._items).Contains(item);

  void ICollection<T>.CopyTo(T[] array, int arrayIndex)
  {
    // ISSUE: reference to a compiler-generated field
    ((ICollection<T>) this._items).CopyTo(array, arrayIndex);
  }

  bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

  [IndexerName("System.Collections.Generic.IList<T>.this[]")]
  T IList<T>.this[int index]
  {
    get => this._items[index];
    set => throw new NotSupportedException();
  }

  int IList<T>.IndexOf(T item) => ((IList<T>) this._items).IndexOf(item);

  void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

  void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
}
