// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Numerics.Chunk256AndIndex
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Utils;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Mafi.Core.Numerics
{
  [ExpectedStructSize(4)]
  [StructLayout(LayoutKind.Explicit)]
  public readonly struct Chunk256AndIndex : IEquatable<Chunk256AndIndex>
  {
    [FieldOffset(0)]
    public readonly Chunk256 ChunkCoord;
    /// <summary>
    /// Index to a contiguous array of all chunks on the map. This is computed as <c>ChunkCoord.X +
    /// ChunkCoord.Y * (TerrainWidth / 256)</c> (note that terrain size is always divisible by 256).
    /// </summary>
    [FieldOffset(2)]
    public readonly Chunk256Index ChunkIndex;
    [FieldOffset(0)]
    public readonly uint CoordAndIndexPacked;

    public Chunk256AndIndex(Chunk256 coord, Chunk256Index index)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.CoordAndIndexPacked = 0U;
      this.ChunkCoord = coord;
      this.ChunkIndex = index;
    }

    public Tile2i OriginTile2i => this.ChunkCoord.OriginTile2i;

    public static bool operator ==(Chunk256AndIndex lhs, Chunk256AndIndex rhs)
    {
      return (int) lhs.CoordAndIndexPacked == (int) rhs.CoordAndIndexPacked;
    }

    public static bool operator !=(Chunk256AndIndex lhs, Chunk256AndIndex rhs)
    {
      return (int) lhs.CoordAndIndexPacked != (int) rhs.CoordAndIndexPacked;
    }

    public bool Equals(Chunk256AndIndex other)
    {
      return (int) this.CoordAndIndexPacked == (int) other.CoordAndIndexPacked;
    }

    public override bool Equals(object obj)
    {
      return obj is Chunk256AndIndex chunk256AndIndex && this == chunk256AndIndex;
    }

    public override int GetHashCode() => (int) this.CoordAndIndexPacked;

    public override string ToString()
    {
      return string.Format("{0} ({1}, {2})", (object) this.ChunkIndex, (object) this.ChunkCoord.X, (object) this.ChunkCoord.Y);
    }
  }
}
