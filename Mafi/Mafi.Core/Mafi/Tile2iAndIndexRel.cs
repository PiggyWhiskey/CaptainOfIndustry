// Decompiled with JetBrains decompiler
// Type: Mafi.Tile2iAndIndexRel
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Mafi
{
  [StructLayout(LayoutKind.Explicit)]
  public readonly struct Tile2iAndIndexRel : IEquatable<Tile2iAndIndexRel>
  {
    [FieldOffset(0)]
    public readonly short X;
    [FieldOffset(2)]
    public readonly short Y;
    [FieldOffset(4)]
    public readonly int IndexDelta;
    [FieldOffset(0)]
    public readonly uint XyPacked;
    [FieldOffset(0)]
    public readonly ulong XyIndexPacked;

    public Tile2iAndIndexRel(short x, short y, int indexDelta)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.XyPacked = 0U;
      this.XyIndexPacked = 0UL;
      this.X = x;
      this.Y = y;
      this.IndexDelta = indexDelta;
    }

    public static Tile2iAndIndexRel Create(RelTile2i rel, int terrainWidth)
    {
      return new Tile2iAndIndexRel((short) rel.X, (short) rel.Y, rel.X + rel.Y * terrainWidth);
    }

    public static bool operator ==(Tile2iAndIndexRel lhs, Tile2iAndIndexRel rhs)
    {
      return lhs.IndexDelta == rhs.IndexDelta;
    }

    public static bool operator !=(Tile2iAndIndexRel lhs, Tile2iAndIndexRel rhs)
    {
      return lhs.IndexDelta != rhs.IndexDelta;
    }

    public static Tile2iAndIndexRel operator -(Tile2iAndIndexRel value)
    {
      return new Tile2iAndIndexRel(-value.X, -value.Y, -value.IndexDelta);
    }

    public static Tile2iAndIndexRel operator +(Tile2iAndIndexRel lhs, Tile2iAndIndexRel rhs)
    {
      return new Tile2iAndIndexRel((short) ((int) lhs.X + (int) rhs.X), (short) ((int) lhs.Y + (int) rhs.Y), lhs.IndexDelta + rhs.IndexDelta);
    }

    public static Tile2iAndIndex operator +(Tile2iAndIndex lhs, Tile2iAndIndexRel rhs)
    {
      return new Tile2iAndIndex((ushort) ((uint) lhs.X + (uint) rhs.X), (ushort) ((uint) lhs.Y + (uint) rhs.Y), lhs.IndexRaw + rhs.IndexDelta);
    }

    public static Tile2iAndIndex operator +(Tile2iAndIndexRel lhs, Tile2iAndIndex rhs)
    {
      return new Tile2iAndIndex((ushort) ((uint) lhs.X + (uint) rhs.X), (ushort) ((uint) lhs.Y + (uint) rhs.Y), lhs.IndexDelta + rhs.IndexRaw);
    }

    [Pure]
    public bool Equals(Tile2iAndIndexRel other) => other == this;

    [Pure]
    public override bool Equals(object other)
    {
      return other is Tile2iAndIndexRel tile2iAndIndexRel && tile2iAndIndexRel == this;
    }

    [Pure]
    public override int GetHashCode() => this.XyIndexPacked.GetHashCode();

    public override string ToString()
    {
      return string.Format("{0} ({1}, {2})", (object) this.IndexDelta, (object) this.X, (object) this.Y);
    }
  }
}
