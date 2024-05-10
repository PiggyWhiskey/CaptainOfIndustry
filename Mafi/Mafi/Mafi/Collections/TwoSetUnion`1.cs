// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.TwoSetUnion`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  [GenerateSerializer(false, null, 0)]
  public class TwoSetUnion<T> : IReadOnlySet<T>, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
  {
    private IReadOnlySet<T> m_set1;
    private IReadOnlySet<T> m_set2;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public TwoSetUnion(IReadOnlySet<T> set1, IReadOnlySet<T> set2)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ResetInternalSets(set1, set2);
    }

    public int Count => this.m_set1.Count + this.m_set2.Count;

    public IEqualityComparer<T> Comparer => this.m_set1.Comparer;

    public void ResetInternalSets(IReadOnlySet<T> set1, IReadOnlySet<T> set2)
    {
      Assert.That<IEqualityComparer<T>>(set1.Comparer).IsEqualTo<IEqualityComparer<T>>(set2.Comparer);
      this.m_set1 = set1.CheckNotNull<IReadOnlySet<T>>();
      this.m_set2 = set2.CheckNotNull<IReadOnlySet<T>>();
    }

    public bool Contains(T item) => this.m_set1.Contains(item) || this.m_set2.Contains(item);

    public bool TryGetValue(T equalValue, out T actualValue)
    {
      return this.m_set1.TryGetValue(equalValue, out actualValue) || this.m_set2.TryGetValue(equalValue, out actualValue);
    }

    public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public static void Serialize(TwoSetUnion<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TwoSetUnion<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TwoSetUnion<T>.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IReadOnlySet<T>>(this.m_set1);
      writer.WriteGeneric<IReadOnlySet<T>>(this.m_set2);
    }

    public static TwoSetUnion<T> Deserialize(BlobReader reader)
    {
      TwoSetUnion<T> twoSetUnion;
      if (reader.TryStartClassDeserialization<TwoSetUnion<T>>(out twoSetUnion))
        reader.EnqueueDataDeserialization((object) twoSetUnion, TwoSetUnion<T>.s_deserializeDataDelayedAction);
      return twoSetUnion;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_set1 = reader.ReadGenericAs<IReadOnlySet<T>>();
      this.m_set2 = reader.ReadGenericAs<IReadOnlySet<T>>();
    }

    static TwoSetUnion()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      TwoSetUnion<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TwoSetUnion<T>) obj).SerializeData(writer));
      TwoSetUnion<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TwoSetUnion<T>) obj).DeserializeData(reader));
    }
  }
}
