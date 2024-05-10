// Decompiled with JetBrains decompiler
// Type: Mafi.Direction903d
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct Direction903d : IEquatable<Direction903d>, IComparable<Direction903d>
  {
    public const int PLUS_X_INDEX = 0;
    public const int PLUS_Y_INDEX = 1;
    public const int PLUS_Z_INDEX = 2;
    public const int MINUS_X_INDEX = 3;
    public const int MINUS_Y_INDEX = 4;
    public const int MINUS_Z_INDEX = 5;
    public static readonly Direction903d[] AllSixDirections;
    public static readonly Direction903d[] PlusMinusX;
    public static readonly Direction903d[] PlusMinusY;
    public static readonly Direction903d[] PlusMinusZ;
    private static readonly Vector3i[] DirToVec;
    public readonly int DirectionIndex;

    public static Direction903d PlusX => new Direction903d(0);

    public static Direction903d PlusY => new Direction903d(1);

    public static Direction903d PlusZ => new Direction903d(2);

    public static Direction903d MinusX => new Direction903d(3);

    public static Direction903d MinusY => new Direction903d(4);

    public static Direction903d MinusZ => new Direction903d(5);

    public Direction903d(int directionIndex)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.DirectionIndex = directionIndex;
    }

    public Direction903d(Vector3i v)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this = new Direction903d(v.X, v.Y, v.Z);
    }

    /// <summary>
    /// Creates direction from a vector. Two of the directions must be zero and other not zero, however,
    /// magnitude of the components does not matter.
    /// </summary>
    public Direction903d(int dx, int dy, int dz)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      if (dx > 0)
        this.DirectionIndex = 0;
      else if (dx < 0)
        this.DirectionIndex = 3;
      else if (dy > 0)
        this.DirectionIndex = 1;
      else if (dy < 0)
        this.DirectionIndex = 4;
      else if (dz > 0)
        this.DirectionIndex = 2;
      else if (dz < 0)
      {
        this.DirectionIndex = 5;
      }
      else
      {
        Assert.Fail("Invalid Direction903d construction from zero vector, setting to +X.");
        this.DirectionIndex = 0;
      }
    }

    public static Direction903d? FromString(string str)
    {
      if (str != null && str.Length == 2)
      {
        switch (str[1])
        {
          case 'X':
            switch (str)
            {
              case "+X":
                break;
              case "-X":
                goto label_11;
              default:
                goto label_14;
            }
            break;
          case 'Y':
            switch (str)
            {
              case "+Y":
                goto label_9;
              case "-Y":
                goto label_12;
              default:
                goto label_14;
            }
          case 'Z':
            switch (str)
            {
              case "+Z":
                goto label_10;
              case "-Z":
                goto label_13;
              default:
                goto label_14;
            }
          case 'x':
            switch (str)
            {
              case "+x":
                break;
              case "-x":
                goto label_11;
              default:
                goto label_14;
            }
            break;
          case 'y':
            switch (str)
            {
              case "+y":
                goto label_9;
              case "-y":
                goto label_12;
              default:
                goto label_14;
            }
          case 'z':
            switch (str)
            {
              case "+z":
                goto label_10;
              case "-z":
                goto label_13;
              default:
                goto label_14;
            }
          default:
            goto label_14;
        }
        return new Direction903d?(Direction903d.PlusX);
label_9:
        return new Direction903d?(Direction903d.PlusY);
label_10:
        return new Direction903d?(Direction903d.PlusZ);
label_11:
        return new Direction903d?(Direction903d.MinusX);
label_12:
        return new Direction903d?(Direction903d.MinusY);
label_13:
        return new Direction903d?(Direction903d.MinusZ);
      }
label_14:
      return new Direction903d?();
    }

    public static Direction903d? FromString(char chr)
    {
      switch (chr)
      {
        case '+':
          return new Direction903d?(Direction903d.PlusZ);
        case '-':
          return new Direction903d?(Direction903d.MinusZ);
        case '<':
          return new Direction903d?(Direction903d.MinusX);
        case '>':
          return new Direction903d?(Direction903d.PlusX);
        case '^':
          return new Direction903d?(Direction903d.PlusY);
        case 'v':
          return new Direction903d?(Direction903d.MinusY);
        default:
          return new Direction903d?();
      }
    }

    public Vector3i DirectionVector
    {
      get
      {
        return (uint) this.DirectionIndex >= (uint) Direction903d.DirToVec.Length ? Vector3i.UnitX : Direction903d.DirToVec[this.DirectionIndex];
      }
    }

    /// <summary>
    /// Whether this and given directions are parallel (+X and -X, etc).
    /// </summary>
    [Pure]
    public bool IsParallelTo(Direction903d other)
    {
      return this.DirectionIndex % 3 == other.DirectionIndex % 3;
    }

    /// <summary>Rotates by 180 degrees.</summary>
    public static Direction903d operator -(Direction903d value)
    {
      return new Direction903d((value.DirectionIndex + 3) % 6);
    }

    public Direction90 ToHorizontalOrError()
    {
      switch (this.DirectionIndex)
      {
        case 0:
          return new Direction90(1, 0);
        case 1:
          return new Direction90(0, 1);
        case 3:
          return new Direction90(-1, 0);
        case 4:
          return new Direction90(0, -1);
        default:
          Log.Error("Horizontal called on vertical or invalid Direction903d");
          return new Direction90(1, 0);
      }
    }

    public override string ToString()
    {
      switch (this.DirectionIndex)
      {
        case 0:
          return "+X";
        case 1:
          return "+Y";
        case 2:
          return "+Z";
        case 3:
          return "-X";
        case 4:
          return "-Y";
        default:
          return "-Z";
      }
    }

    public char ToChar()
    {
      switch (this.DirectionIndex)
      {
        case 0:
          return '>';
        case 1:
          return '^';
        case 2:
          return '+';
        case 3:
          return '<';
        case 4:
          return 'v';
        default:
          return '-';
      }
    }

    public bool Equals(Direction903d other) => this.DirectionIndex == other.DirectionIndex;

    public override bool Equals(object obj) => obj is Direction903d other && this.Equals(other);

    public override int GetHashCode() => this.DirectionIndex;

    public int CompareTo(Direction903d other)
    {
      return this.DirectionIndex.CompareTo(other.DirectionIndex);
    }

    public static bool operator ==(Direction903d lhs, Direction903d rhs)
    {
      return lhs.DirectionIndex == rhs.DirectionIndex;
    }

    public static bool operator !=(Direction903d lhs, Direction903d rhs)
    {
      return lhs.DirectionIndex != rhs.DirectionIndex;
    }

    public static void Serialize(Direction903d value, BlobWriter writer)
    {
      writer.WriteIntNotNegative(value.DirectionIndex);
    }

    public static Direction903d Deserialize(BlobReader reader)
    {
      return new Direction903d(reader.ReadIntNotNegative());
    }

    static Direction903d()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Direction903d.AllSixDirections = new Direction903d[6]
      {
        Direction903d.PlusX,
        Direction903d.PlusY,
        Direction903d.PlusZ,
        Direction903d.MinusX,
        Direction903d.MinusY,
        Direction903d.MinusZ
      };
      Direction903d.PlusMinusX = new Direction903d[2]
      {
        Direction903d.PlusX,
        Direction903d.MinusX
      };
      Direction903d.PlusMinusY = new Direction903d[2]
      {
        Direction903d.PlusY,
        Direction903d.MinusY
      };
      Direction903d.PlusMinusZ = new Direction903d[2]
      {
        Direction903d.PlusZ,
        Direction903d.MinusZ
      };
      Direction903d.DirToVec = new Vector3i[6]
      {
        new Vector3i(1, 0, 0),
        new Vector3i(0, 1, 0),
        new Vector3i(0, 0, 1),
        new Vector3i(-1, 0, 0),
        new Vector3i(0, -1, 0),
        new Vector3i(0, 0, -1)
      };
    }
  }
}
