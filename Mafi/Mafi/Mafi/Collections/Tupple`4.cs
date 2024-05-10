// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.Tupple`4
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
  public readonly struct Tupple<T1, T2, T3, T4> : IEquatable<Tupple<T1, T2, T3, T4>>
  {
    public readonly T1 First;
    public readonly T2 Second;
    public readonly T3 Third;
    public readonly T4 Fourth;

    public Tupple(T1 first, T2 second, T3 third, T4 fourth)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.First = first;
      this.Second = second;
      this.Third = third;
      this.Fourth = fourth;
    }

    public bool Equals(Tupple<T1, T2, T3, T4> other)
    {
      return EqualityComparer<T1>.Default.Equals(this.First, other.First) && EqualityComparer<T2>.Default.Equals(this.Second, other.Second) && EqualityComparer<T3>.Default.Equals(this.Third, other.Third) && EqualityComparer<T4>.Default.Equals(this.Fourth, other.Fourth);
    }

    public override bool Equals(object obj)
    {
      return obj is Tupple<T1, T2, T3, T4> other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return Hash.Combine<T1, T2, T3, T4>(this.First, this.Second, this.Third, this.Fourth);
    }

    public static void Serialize(Tupple<T1, T2, T3, T4> value, BlobWriter writer)
    {
      writer.WriteGeneric<T1>(value.First);
      writer.WriteGeneric<T2>(value.Second);
      writer.WriteGeneric<T3>(value.Third);
      writer.WriteGeneric<T4>(value.Fourth);
    }

    public static Tupple<T1, T2, T3, T4> Deserialize(BlobReader reader)
    {
      return new Tupple<T1, T2, T3, T4>(reader.ReadGenericAs<T1>(), reader.ReadGenericAs<T2>(), reader.ReadGenericAs<T3>(), reader.ReadGenericAs<T4>());
    }
  }
}
