// Decompiled with JetBrains decompiler
// Type: Mafi.Direction90
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
  public readonly struct Direction90 : IEquatable<Direction90>, IComparable<Direction90>
  {
    public const int PLUS_X_INDEX = 0;
    public const int PLUS_Y_INDEX = 1;
    public const int MINUS_X_INDEX = 2;
    public const int MINUS_Y_INDEX = 3;
    public static readonly Direction90[] AllFourDirections;
    public static readonly Direction90[] PlusMinusX;
    public static readonly Direction90[] PlusMinusY;
    private static readonly Vector2i[] DirToVec;
    public readonly int DirectionIndex;

    public static Direction90 PlusX => new Direction90(0);

    public static Direction90 PlusY => new Direction90(1);

    public static Direction90 MinusX => new Direction90(2);

    public static Direction90 MinusY => new Direction90(3);

    public Direction90(int directionIndex)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<int>(directionIndex).IsWithinIncl(0, 3);
      this.DirectionIndex = directionIndex & 3;
    }

    /// <summary>
    /// Creates direction from a vector. One of the directions must be zero and other not zero, however,
    /// magnitude of the components does not matter.
    /// </summary>
    public Direction90(int dx, int dy)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<bool>(dx == 0).IsNotEqualTo<bool>(dy == 0, "Invalid Direction90 construction.");
      if (dx > 0)
        this.DirectionIndex = 0;
      else if (dx < 0)
        this.DirectionIndex = 2;
      else if (dy > 0)
        this.DirectionIndex = 1;
      else if (dy < 0)
      {
        this.DirectionIndex = 3;
      }
      else
      {
        Assert.Fail(string.Format("Invalid direction 90 from vector ({0}, {1}), setting to +X.", (object) dx, (object) dy));
        this.DirectionIndex = 0;
      }
    }

    public static Direction90? FromString(string str)
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
                goto label_8;
              default:
                goto label_10;
            }
            break;
          case 'Y':
            switch (str)
            {
              case "+Y":
                goto label_7;
              case "-Y":
                goto label_9;
              default:
                goto label_10;
            }
          case 'x':
            switch (str)
            {
              case "+x":
                break;
              case "-x":
                goto label_8;
              default:
                goto label_10;
            }
            break;
          case 'y':
            switch (str)
            {
              case "+y":
                goto label_7;
              case "-y":
                goto label_9;
              default:
                goto label_10;
            }
          default:
            goto label_10;
        }
        return new Direction90?(Direction90.PlusX);
label_7:
        return new Direction90?(Direction90.PlusY);
label_8:
        return new Direction90?(Direction90.MinusX);
label_9:
        return new Direction90?(Direction90.MinusY);
      }
label_10:
      return new Direction90?();
    }

    public static Direction90? FromString(char chr)
    {
      switch (chr)
      {
        case '<':
          return new Direction90?(Direction90.MinusX);
        case '>':
          return new Direction90?(Direction90.PlusX);
        case '^':
          return new Direction90?(Direction90.PlusY);
        case 'v':
          return new Direction90?(Direction90.MinusY);
        default:
          return new Direction90?();
      }
    }

    public Direction90(Vector2i v)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this = new Direction90(v.X, v.Y);
    }

    public Vector2i DirectionVector => Direction90.DirToVec[this.DirectionIndex];

    /// <summary>Rotates by +90 degrees (counter-clock-wise).</summary>
    public Direction90 RotatedPlus90 => new Direction90(this.DirectionIndex + 1 & 3);

    /// <summary>Rotates by -90 degrees (clock-wise).</summary>
    public Direction90 RotatedMinus90 => new Direction90(this.DirectionIndex + 3 & 3);

    /// <summary>
    /// Rotates by 180 degrees (the same as unary - operator).
    /// </summary>
    public Direction90 Rotated180 => new Direction90(this.DirectionIndex + 2 & 3);

    public Direction903d As3d
    {
      get
      {
        switch (this.DirectionIndex)
        {
          case 0:
            return new Direction903d(1, 0, 0);
          case 1:
            return new Direction903d(0, 1, 0);
          case 2:
            return new Direction903d(-1, 0, 0);
          default:
            return new Direction903d(0, -1, 0);
        }
      }
    }

    /// <summary>
    /// Converts current direction to a rotation assuming that +x axis is 0 rotation.
    /// </summary>
    [Pure]
    public Rotation90 ToRotation() => new Rotation90(this.DirectionIndex);

    /// <summary>
    /// Whether this and given directions are parallel (+X and -X, etc).
    /// </summary>
    [Pure]
    public bool IsParallelTo(Direction90 other)
    {
      return (this.DirectionIndex & 1) == (other.DirectionIndex & 1);
    }

    /// <summary>Rotates by 180 degrees.</summary>
    public static Direction90 operator -(Direction90 value)
    {
      return new Direction90(value.DirectionIndex + 2 & 3);
    }

    public static Direction90 operator +(Rotation90 rotation, Direction90 direction)
    {
      return new Direction90(rotation.AngleIndex + direction.DirectionIndex & 3);
    }

    public static Direction90 operator +(Direction90 direction, Rotation90 rotation)
    {
      return new Direction90(rotation.AngleIndex + direction.DirectionIndex & 3);
    }

    public static Rotation90 operator -(Direction90 lhs, Direction90 rhs)
    {
      return new Rotation90(lhs.DirectionIndex - rhs.DirectionIndex + 4 & 3);
    }

    public static Direction90 operator -(Direction90 direction, Rotation90 rotation)
    {
      return new Direction90(rotation.AngleIndex - direction.DirectionIndex & 3);
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
          return "-X";
        default:
          return "-Y";
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
          return '<';
        default:
          return 'v';
      }
    }

    public bool Equals(Direction90 other) => this.DirectionIndex == other.DirectionIndex;

    public override bool Equals(object obj) => obj is Direction90 other && this.Equals(other);

    public override int GetHashCode() => this.DirectionIndex;

    public int CompareTo(Direction90 other) => this.DirectionIndex.CompareTo(other.DirectionIndex);

    public static bool operator ==(Direction90 lhs, Direction90 rhs)
    {
      return lhs.DirectionIndex == rhs.DirectionIndex;
    }

    public static bool operator !=(Direction90 lhs, Direction90 rhs)
    {
      return lhs.DirectionIndex != rhs.DirectionIndex;
    }

    public static void Serialize(Direction90 value, BlobWriter writer)
    {
      writer.WriteInt(value.DirectionIndex);
    }

    public static Direction90 Deserialize(BlobReader reader) => new Direction90(reader.ReadInt());

    static Direction90()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Direction90.AllFourDirections = new Direction90[4]
      {
        Direction90.PlusX,
        Direction90.PlusY,
        Direction90.MinusX,
        Direction90.MinusY
      };
      Direction90.PlusMinusX = new Direction90[2]
      {
        Direction90.PlusX,
        Direction90.MinusX
      };
      Direction90.PlusMinusY = new Direction90[2]
      {
        Direction90.PlusY,
        Direction90.MinusY
      };
      Direction90.DirToVec = new Vector2i[4]
      {
        new Vector2i(1, 0),
        new Vector2i(0, 1),
        new Vector2i(-1, 0),
        new Vector2i(0, -1)
      };
    }
  }
}
