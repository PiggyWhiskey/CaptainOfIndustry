// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.Pair`2
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
  {
    public readonly T1 First;
    public readonly T2 Second;

    public Pair(T1 first, T2 second)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.First = first;
      this.Second = second;
    }

    public bool Equals(Pair<T1, T2> other)
    {
      return EqualityComparer<T1>.Default.Equals(this.First, other.First) && EqualityComparer<T2>.Default.Equals(this.Second, other.Second);
    }

    public override bool Equals(object obj) => obj is Pair<T1, T2> other && this.Equals(other);

    public override int GetHashCode() => Hash.Combine<T1, T2>(this.First, this.Second);

    public override string ToString()
    {
      return string.Format("({0}, {1})", (object) this.First, (object) this.Second);
    }

    public static void Serialize(Pair<T1, T2> value, BlobWriter writer)
    {
      writer.WriteGeneric<T1>(value.First);
      writer.WriteGeneric<T2>(value.Second);
    }

    public static Pair<T1, T2> Deserialize(BlobReader reader)
    {
      return new Pair<T1, T2>(reader.ReadGenericAs<T1>(), reader.ReadGenericAs<T2>());
    }
  }
}
