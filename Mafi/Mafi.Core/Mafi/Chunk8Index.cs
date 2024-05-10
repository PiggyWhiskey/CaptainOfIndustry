// Decompiled with JetBrains decompiler
// Type: Mafi.Chunk8Index
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct Chunk8Index : IEquatable<Chunk8Index>, IComparable<Chunk8Index>
  {
    public readonly int Value;

    public Chunk8Index(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public override string ToString() => this.Value.ToString();

    public bool Equals(Chunk8Index other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is Chunk8Index other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(Chunk8Index other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(Chunk8Index lhs, Chunk8Index rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(Chunk8Index lhs, Chunk8Index rhs) => lhs.Value != rhs.Value;

    public static Chunk8Index operator -(Chunk8Index rhs) => new Chunk8Index(-rhs.Value);

    public static void Serialize(Chunk8Index value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static Chunk8Index Deserialize(BlobReader reader) => new Chunk8Index(reader.ReadInt());
  }
}
