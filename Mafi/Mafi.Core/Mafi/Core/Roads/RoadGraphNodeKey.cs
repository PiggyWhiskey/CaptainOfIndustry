// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadGraphNodeKey
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Utils;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Mafi.Core.Roads
{
  [ExpectedStructSize(8)]
  [StructLayout(LayoutKind.Explicit)]
  public readonly struct RoadGraphNodeKey : IEquatable<RoadGraphNodeKey>
  {
    /// <summary>
    /// We store X and Y coordinates as doubled values to allow representation of points with 0.5 precision.
    /// </summary>
    [FieldOffset(0)]
    private readonly ushort DoubleX;
    [FieldOffset(2)]
    private readonly ushort DoubleY;
    [FieldOffset(4)]
    private readonly short Z;
    [FieldOffset(6)]
    public readonly RoadGraphNodeDirection Direction;
    [FieldOffset(7)]
    public readonly byte m_unused;
    [FieldOffset(0)]
    private readonly ulong RawData;

    public Tile3f Position
    {
      get
      {
        Fix32 fix32 = Fix32.FromInt(this.DoubleX);
        Fix32 halfFast1 = fix32.HalfFast;
        fix32 = Fix32.FromInt(this.DoubleY);
        Fix32 halfFast2 = fix32.HalfFast;
        Fix32 z = Fix32.FromInt(this.Z);
        return new Tile3f(halfFast1, halfFast2, z);
      }
    }

    public Tile2f Position2f
    {
      get
      {
        Fix32 fix32 = Fix32.FromInt(this.DoubleX);
        Fix32 halfFast1 = fix32.HalfFast;
        fix32 = Fix32.FromInt(this.DoubleY);
        Fix32 halfFast2 = fix32.HalfFast;
        return new Tile2f(halfFast1, halfFast2);
      }
    }

    public RoadGraphNodeKey(
      ushort doubleX,
      ushort doubleY,
      short z,
      RoadGraphNodeDirection direction)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_unused = (byte) 0;
      this.RawData = 0UL;
      this.DoubleX = doubleX;
      this.DoubleY = doubleY;
      this.Z = z;
      this.Direction = direction;
    }

    public static RoadGraphNodeKey FromPosition(Tile3f position, RoadGraphNodeDirection direction)
    {
      return new RoadGraphNodeKey((ushort) position.X.Times2Fast.ToIntRounded(), (ushort) position.Y.Times2Fast.ToIntRounded(), (short) position.Z.ToIntRounded(), direction);
    }

    public static bool operator ==(RoadGraphNodeKey lhs, RoadGraphNodeKey rhs)
    {
      return (long) lhs.RawData == (long) rhs.RawData;
    }

    public static bool operator !=(RoadGraphNodeKey lhs, RoadGraphNodeKey rhs)
    {
      return (long) lhs.RawData != (long) rhs.RawData;
    }

    public bool Equals(RoadGraphNodeKey other) => (long) this.RawData == (long) other.RawData;

    public override bool Equals(object obj) => obj is RoadGraphNodeKey other && this.Equals(other);

    public override int GetHashCode() => this.RawData.GetHashCode();

    public override string ToString()
    {
      return string.Format("{0} dir {1}", (object) this.Position, (object) this.Direction);
    }
  }
}
