// Decompiled with JetBrains decompiler
// Type: Mafi.Tile2iAndIndex
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using Mafi.Utils;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Very efficient way to represent a tile and its index (8 bytes). Only valid coordinates are supported.
  /// </summary>
  [ManuallyWrittenSerialization]
  [ExpectedStructSize(8)]
  [StructLayout(LayoutKind.Explicit)]
  public readonly struct Tile2iAndIndex : IEquatable<Tile2iAndIndex>
  {
    [FieldOffset(0)]
    public readonly ushort X;
    [FieldOffset(2)]
    public readonly ushort Y;
    /// <summary>
    /// Represents a raw value of <see cref="T:Mafi.Tile2iIndex" />, an index to a contiguous array of all tiles on the map.
    /// This is computed as <c>X + Y * TerrainWidth</c>.
    /// </summary>
    [FieldOffset(4)]
    public readonly int IndexRaw;
    [FieldOffset(0)]
    public readonly uint XyPacked;
    [FieldOffset(0)]
    public readonly ulong XyIndexPacked;

    /// <summary>
    /// WARNING: It is the callers responsibility to make sure that the (X, Y) coordinates are matching the index.
    /// </summary>
    public Tile2iAndIndex(ushort x, ushort y, int indexRaw)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.XyPacked = 0U;
      this.XyIndexPacked = 0UL;
      this.X = x;
      this.Y = y;
      this.IndexRaw = indexRaw;
    }

    private Tile2iAndIndex(ulong xyIndexPacked)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = (ushort) 0;
      this.Y = (ushort) 0;
      this.IndexRaw = 0;
      this.XyPacked = 0U;
      this.XyIndexPacked = xyIndexPacked;
    }

    public Tile2iIndex Index => new Tile2iIndex(this.IndexRaw);

    public Tile2i TileCoord => new Tile2i((int) this.X, (int) this.Y);

    public Tile2iSlim TileCoordSlim => new Tile2iSlim(this.X, this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Chunk2i ChunkCoord2i => new Chunk2i((int) this.X >> 6, (int) this.Y >> 6);

    /// <summary>
    /// Returns coordinate for the +X neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iAndIndex PlusXNeighborUnchecked
    {
      get => new Tile2iAndIndex((ushort) ((uint) this.X + 1U), this.Y, this.IndexRaw + 1);
    }

    /// <summary>
    /// Returns coordinate for the +Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iAndIndex PlusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iAndIndex(this.X, (ushort) ((uint) this.Y + 1U), this.IndexRaw + terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the -X neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iAndIndex MinusXNeighborUnchecked
    {
      get => new Tile2iAndIndex((ushort) ((uint) this.X - 1U), this.Y, this.IndexRaw - 1);
    }

    /// <summary>
    /// Returns coordinate for the -Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iAndIndex MinusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iAndIndex(this.X, (ushort) ((uint) this.Y - 1U), this.IndexRaw - terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the -X -Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iAndIndex MinusXMinusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iAndIndex((ushort) ((uint) this.X - 1U), (ushort) ((uint) this.Y - 1U), this.IndexRaw - 1 - terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the -X +Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iAndIndex MinusXPlusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iAndIndex((ushort) ((uint) this.X - 1U), (ushort) ((uint) this.Y + 1U), this.IndexRaw - 1 + terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the +X -Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iAndIndex PlusXMinusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iAndIndex((ushort) ((uint) this.X + 1U), (ushort) ((uint) this.Y - 1U), this.IndexRaw + 1 - terrainWidth);
    }

    /// <summary>
    /// Returns coordinate for the +X +Y neighbor. This is only safe for non-boundary tiles.
    /// </summary>
    public Tile2iAndIndex PlusXPlusYNeighborUnchecked(int terrainWidth)
    {
      return new Tile2iAndIndex((ushort) ((uint) this.X + 1U), (ushort) ((uint) this.Y + 1U), this.IndexRaw + 1 + terrainWidth);
    }

    public static bool operator ==(Tile2iAndIndex lhs, Tile2iAndIndex rhs)
    {
      return (long) lhs.XyIndexPacked == (long) rhs.XyIndexPacked;
    }

    public static bool operator !=(Tile2iAndIndex lhs, Tile2iAndIndex rhs)
    {
      return (long) lhs.XyIndexPacked != (long) rhs.XyIndexPacked;
    }

    public static Tile2iAndIndexRel operator -(Tile2iAndIndex lhs, Tile2iAndIndex rhs)
    {
      return new Tile2iAndIndexRel((short) ((int) lhs.X - (int) rhs.X), (short) ((int) lhs.Y - (int) rhs.Y), lhs.IndexRaw - rhs.IndexRaw);
    }

    [Pure]
    public bool Equals(Tile2iAndIndex other) => other == this;

    [Pure]
    public override bool Equals(object other)
    {
      return other is Tile2iAndIndex tile2iAndIndex && tile2iAndIndex == this;
    }

    [Pure]
    public override int GetHashCode() => this.IndexRaw.GetHashCode();

    public override string ToString()
    {
      return string.Format("{0} ({1}, {2})", (object) this.IndexRaw, (object) this.X, (object) this.Y);
    }

    public static void Serialize(Tile2iAndIndex value, BlobWriter writer)
    {
      writer.WriteULongNonVariable(value.XyIndexPacked);
    }

    public static Tile2iAndIndex Deserialize(BlobReader reader)
    {
      return new Tile2iAndIndex(reader.ReadULongNonVariable());
    }
  }
}
