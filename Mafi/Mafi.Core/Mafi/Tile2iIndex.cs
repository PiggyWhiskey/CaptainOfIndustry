// Decompiled with JetBrains decompiler
// Type: Mafi.Tile2iIndex
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
  public readonly struct Tile2iIndex : IEquatable<Tile2iIndex>, IComparable<Tile2iIndex>
  {
    public readonly int Value;

    public Tile2iIndex(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public Tile2iIndex Min(Tile2iIndex rhs) => new Tile2iIndex(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public Tile2iIndex Max(Tile2iIndex rhs) => new Tile2iIndex(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public Tile2iIndex Clamp(Tile2iIndex min, Tile2iIndex max)
    {
      return new Tile2iIndex(this.Value.Clamp(min.Value, max.Value));
    }

    public override string ToString() => this.Value.ToString();

    public bool Equals(Tile2iIndex other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is Tile2iIndex other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(Tile2iIndex other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(Tile2iIndex lhs, Tile2iIndex rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(Tile2iIndex lhs, Tile2iIndex rhs) => lhs.Value != rhs.Value;

    public static Tile2iIndex operator +(Tile2iIndex lhs, Tile2iIndex rhs)
    {
      return new Tile2iIndex(lhs.Value + rhs.Value);
    }

    public static Tile2iIndex operator -(Tile2iIndex lhs, Tile2iIndex rhs)
    {
      return new Tile2iIndex(lhs.Value - rhs.Value);
    }

    public static Tile2iIndex operator *(Tile2iIndex lhs, int rhs)
    {
      return new Tile2iIndex(lhs.Value * rhs);
    }

    public static Tile2iIndex operator *(int lhs, Tile2iIndex rhs)
    {
      return new Tile2iIndex(lhs * rhs.Value);
    }

    public static Tile2iIndex operator /(Tile2iIndex lhs, int rhs)
    {
      return new Tile2iIndex(lhs.Value / rhs);
    }

    public static void Serialize(Tile2iIndex value, BlobWriter writer)
    {
      writer.WriteIntNotNegative(value.Value);
    }

    public static Tile2iIndex Deserialize(BlobReader reader)
    {
      return new Tile2iIndex(reader.ReadIntNotNegative());
    }

    /// <summary>
    /// Returns coordinate for the +X neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iIndex PlusXNeighborUnchecked => new Tile2iIndex(this.Value + 1);

    /// <summary>
    /// Returns coordinate for the +Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iIndex PlusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iIndex(this.Value + terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the -X neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iIndex MinusXNeighborUnchecked => new Tile2iIndex(this.Value - 1);

    /// <summary>
    /// Returns coordinate for the -Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iIndex MinusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iIndex(this.Value - terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the -X -Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iIndex MinusXMinusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iIndex(this.Value - 1 - terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the -X +Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iIndex MinusXPlusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iIndex(this.Value - 1 + terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the +X -Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iIndex PlusXMinusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iIndex(this.Value + 1 - terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the +X +Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iIndex PlusXPlusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iIndex(this.Value + 1 + terrainWidth);
    }

    public static Tile2iIndex operator +(Tile2iIndex lhs, int rhs)
    {
      return new Tile2iIndex(lhs.Value + rhs);
    }

    public static Tile2iIndex operator +(int lhs, Tile2iIndex rhs)
    {
      return new Tile2iIndex(lhs + rhs.Value);
    }

    public static Tile2iIndex operator -(Tile2iIndex lhs, int rhs)
    {
      return new Tile2iIndex(lhs.Value - rhs);
    }
  }
}
