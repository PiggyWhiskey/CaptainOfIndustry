// Decompiled with JetBrains decompiler
// Type: Mafi.Chunk256Index
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
  public readonly struct Chunk256Index : IEquatable<Chunk256Index>, IComparable<Chunk256Index>
  {
    public readonly ushort Value;

    public Chunk256Index(ushort value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public override string ToString() => this.Value.ToString();

    public bool Equals(Chunk256Index other) => (int) this.Value == (int) other.Value;

    public override bool Equals(object obj) => obj is Chunk256Index other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(Chunk256Index other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(Chunk256Index lhs, Chunk256Index rhs)
    {
      return (int) lhs.Value == (int) rhs.Value;
    }

    public static bool operator !=(Chunk256Index lhs, Chunk256Index rhs)
    {
      return (int) lhs.Value != (int) rhs.Value;
    }

    public static void Serialize(Chunk256Index value, BlobWriter writer)
    {
      writer.WriteUShort(value.Value);
    }

    public static Chunk256Index Deserialize(BlobReader reader)
    {
      return new Chunk256Index(reader.ReadUShort());
    }
  }
}
