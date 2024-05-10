// Decompiled with JetBrains decompiler
// Type: Mafi.Db`2
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Generic DB internally implemented using dictionary. It has protected methods for implementation of add/remove
  /// logic and is lockable.
  /// </summary>
  public abstract class Db<TKey, TValue> : IEnumerable<TValue>, IEnumerable
  {
    private readonly Dict<TKey, TValue> m_db;

    /// <summary>Whether this DB was locked and is ins readonly state.</summary>
    internal bool IsReadonly { get; private set; }

    public int Count => this.m_db.Count;

    public IEnumerable<TKey> Keys => (IEnumerable<TKey>) this.m_db.Keys;

    public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs
    {
      get => (IEnumerable<KeyValuePair<TKey, TValue>>) this.m_db;
    }

    /// <summary>
    /// Returns requested product or throws an exception. Do not use this indexer if you are not 100% sure that the
    /// ID exists. Use <see cref="M:Mafi.Db`2.TryGet(`0,`1@)" /> instead.
    /// </summary>
    /// <remarks>
    /// Do not dare to use this indexer in try-catch instead of using <see cref="M:Mafi.Db`2.TryGet(`0,`1@)" /> method!!!
    /// </remarks>
    public TValue this[TKey id] => this.m_db[id];

    public bool Contains(TKey id) => this.m_db.ContainsKey(id);

    protected bool TryGet(TKey id, out TValue productProto)
    {
      return this.m_db.TryGetValue(id, out productProto);
    }

    protected void Add(TKey id, TValue value)
    {
      if (this.IsReadonly)
        throw new InvalidOperationException("Db is not allowed to be extended with new items at this stage.");
      if (this.m_db.ContainsKey(id))
        Log.Error(string.Format("The DB already contains key '{0}'", (object) id));
      else
        this.m_db.Add(id, value);
    }

    protected bool Remove(TKey id)
    {
      if (this.IsReadonly)
        throw new InvalidOperationException("Db is not allowed to be extended with new items at this stage.");
      return this.m_db.Remove(id);
    }

    internal void SetReadonly() => this.IsReadonly = true;

    public IEnumerator<TValue> GetEnumerator()
    {
      return (IEnumerator<TValue>) this.m_db.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.m_db.Values.GetEnumerator();

    protected Db()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_db = new Dict<TKey, TValue>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
