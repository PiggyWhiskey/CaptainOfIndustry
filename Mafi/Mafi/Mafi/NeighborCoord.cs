// Decompiled with JetBrains decompiler
// Type: Mafi.NeighborCoord
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Represents von Neumann neighbor coord (set of points at a Manhattan distance of 1). Index maps points (1, 0),
  /// (-1, 0), (0, 1), and (0, -1) to indices 0, 1, 2, and 3 respectively. That's: 0b00 = +x, 0b01 = -x, 0b10 = +y, and
  /// 0b11 = -y.
  /// </summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct NeighborCoord : IEquatable<NeighborCoord>, IComparable<NeighborCoord>
  {
    public readonly int Index;
    public const int PLUS_X_INDEX = 0;
    public const int MINUS_X_INDEX = 1;
    public const int PLUS_Y_INDEX = 2;
    public const int MINUS_Y_INDEX = 3;
    public static readonly NeighborCoord PlusX;
    public static readonly NeighborCoord MinusX;
    public static readonly NeighborCoord PlusY;
    public static readonly NeighborCoord MinusY;
    /// <summary>
    /// All four neighbors array. We can not wrap this immediately because of type initialization loop.
    /// </summary>
    private static readonly NeighborCoord[] s_all4Neighbors;

    public override string ToString()
    {
      return string.Format("Index = {0}, Direction = ({1}, {2})", (object) this.Index, (object) this.Dx, (object) this.Dy);
    }

    public bool Equals(NeighborCoord other) => this.Index == other.Index;

    public override bool Equals(object obj) => obj is NeighborCoord other && this.Equals(other);

    public override int GetHashCode() => this.Index;

    public int CompareTo(NeighborCoord other) => this.Index.CompareTo(other.Index);

    public static bool operator ==(NeighborCoord lhs, NeighborCoord rhs) => lhs.Index == rhs.Index;

    public static bool operator !=(NeighborCoord lhs, NeighborCoord rhs) => lhs.Index != rhs.Index;

    public static void Serialize(NeighborCoord value, BlobWriter writer)
    {
      writer.WriteInt(value.Index);
    }

    public static NeighborCoord Deserialize(BlobReader reader)
    {
      return new NeighborCoord(reader.ReadInt());
    }

    /// <summary>Helper array for looping over all four neighbors.</summary>
    public static ReadOnlyArray<NeighborCoord> All4Neighbors
    {
      get => NeighborCoord.s_all4Neighbors.AsReadOnlyArray<NeighborCoord>();
    }

    public NeighborCoord(int index)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Index = index & 3;
    }

    public NeighborCoord(int dx, int dy)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<bool>(dx == 0 && Math.Abs(dy) == 1 || dy == 0 && Math.Abs(dx) == 1).IsTrue("One coordinate of NeighborCoord must be 0 and the other one must be either +- 1.");
      this.Index = dy != 0 ? 2 | (1 - dy) / 2 & 1 : (1 - dx) / 2 & 1;
      Assert.That<int>(this.Index).IsWithinIncl(0, 3);
    }

    public int Dx => ((~this.Index & 2) >> 1) * (1 - ((this.Index & 1) << 1));

    public int Dy => ((this.Index & 2) >> 1) * (1 - ((this.Index & 1) << 1));

    public NeighborCoord Opposite => new NeighborCoord(this.Index ^ 1);

    public NeighborCoord Left
    {
      get
      {
        int num = ~this.Index & 3;
        return new NeighborCoord(num ^ num >> 1);
      }
    }

    public NeighborCoord Right => new NeighborCoord(~this.Index & 3 ^ this.Index >> 1);

    static NeighborCoord()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      NeighborCoord.PlusX = new NeighborCoord(0);
      NeighborCoord.MinusX = new NeighborCoord(1);
      NeighborCoord.PlusY = new NeighborCoord(2);
      NeighborCoord.MinusY = new NeighborCoord(3);
      NeighborCoord.s_all4Neighbors = new NeighborCoord[4]
      {
        new NeighborCoord(0),
        new NeighborCoord(1),
        new NeighborCoord(2),
        new NeighborCoord(3)
      };
    }
  }
}
