// Decompiled with JetBrains decompiler
// Type: Mafi.Rotation90
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Numerics;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Represents absolute or relative rotation in multiplies of 90 degrees.
  /// </summary>
  [OnlyForSaveCompatibility(null)]
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct Rotation90 : IEquatable<Rotation90>, IComparable<Rotation90>
  {
    public readonly int AngleIndex;
    public const int DEG_0_INDEX = 0;
    public const int DEG_90_INDEX = 1;
    public const int DEG_180_INDEX = 2;
    public const int DEG_270_INDEX = 3;
    public static readonly Rotation90 Deg0;
    public static readonly Rotation90 Deg90;
    public static readonly Rotation90 Deg180;
    public static readonly Rotation90 Deg270;
    public static readonly Rotation90[] AllRotations;

    public override string ToString() => string.Format("{0} deg", (object) this.Angle);

    public bool Equals(Rotation90 other) => this.AngleIndex == other.AngleIndex;

    public override bool Equals(object obj) => obj is Rotation90 other && this.Equals(other);

    public override int GetHashCode() => this.AngleIndex;

    public int CompareTo(Rotation90 other) => this.AngleIndex.CompareTo(other.AngleIndex);

    public static bool operator ==(Rotation90 lhs, Rotation90 rhs)
    {
      return lhs.AngleIndex == rhs.AngleIndex;
    }

    public static bool operator !=(Rotation90 lhs, Rotation90 rhs)
    {
      return lhs.AngleIndex != rhs.AngleIndex;
    }

    public static void Serialize(Rotation90 value, BlobWriter writer)
    {
      writer.WriteInt(value.AngleIndex);
    }

    public static Rotation90 Deserialize(BlobReader reader) => new Rotation90(reader.ReadInt());

    public Rotation90(int angleIndex)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<int>(angleIndex).IsWithinIncl(0, 3);
      this.AngleIndex = angleIndex.CheckWithinIncl(0, 3);
    }

    /// <summary>
    /// Creates rotation given by X and Y directions. One of the directions must be zero, however, magnitude of the
    /// arguments does not matter.
    /// </summary>
    public Rotation90(int dx, int dy)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      if (dx > 0)
        this.AngleIndex = 0;
      else if (dx < 0)
        this.AngleIndex = 2;
      else if (dy > 0)
        this.AngleIndex = 1;
      else if (dy < 0)
      {
        this.AngleIndex = 3;
      }
      else
      {
        Assert.Fail(string.Format("Invalid rotation ({0}, {1}), setting to identity.", (object) dx, (object) dy));
        this.AngleIndex = 0;
      }
    }

    /// <summary>
    /// Creates rotation given by direction vector. One of the components of the direction must be zero, however,
    /// magnitude of the components does not matter.
    /// </summary>
    public Rotation90(Vector2i direction)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this = new Rotation90(direction.X, direction.Y);
    }

    public int Dx
    {
      get
      {
        if (this.AngleIndex == 0)
          return 1;
        return this.AngleIndex != 2 ? 0 : -1;
      }
    }

    public int Dy
    {
      get
      {
        if (this.AngleIndex == 1)
          return 1;
        return this.AngleIndex != 3 ? 0 : -1;
      }
    }

    public bool Is90Or270Deg => (this.AngleIndex & 1) != 0;

    /// <summary>Rotates by +90 degrees (counter-clock-wise).</summary>
    public Rotation90 RotatedPlus90 => new Rotation90(this.AngleIndex + 1 & 3);

    /// <summary>Rotates by -90 degrees (clock-wise).</summary>
    public Rotation90 RotatedMinus90 => new Rotation90(this.AngleIndex + 3 & 3);

    /// <summary>Rotates by 180 degrees.</summary>
    public Rotation90 Rotated180 => new Rotation90(this.AngleIndex + 2 & 3);

    /// <summary>
    /// Returns vector representing direction of this rotation. Only one coordinate is set to either 1 or -1.
    /// </summary>
    public Vector2i DirectionVector => new Vector2i(this.Dx, this.Dy);

    /// <summary>
    /// Returns angle in degrees that this rotation represents.
    /// </summary>
    public AngleDegrees1f Angle => (this.AngleIndex * 90).Degrees();

    /// <summary>
    /// Returns quaternion representation of this rotation. Identity is considered to be facing X direction and
    /// rotates in X-Y plane counter-clock wise. TODO: Test this shit!
    /// </summary>
    public UnitQuaternion4f Quaternion
    {
      get => UnitQuaternion4f.FromAxisAngle(Vector3f.UnitZ, this.Angle);
    }

    /// <summary>Rotates by 180 degrees.</summary>
    public static Rotation90 operator -(Rotation90 value)
    {
      return new Rotation90((value.AngleIndex + 2) % 4);
    }

    /// <summary>Adds two rotations.</summary>
    public static Rotation90 operator +(Rotation90 lhs, Rotation90 rhs)
    {
      return new Rotation90((lhs.AngleIndex + rhs.AngleIndex) % 4);
    }

    /// <summary>Subtracts the rhs rotation from the lhs rotation.</summary>
    public static Rotation90 operator -(Rotation90 lhs, Rotation90 rhs)
    {
      return new Rotation90((lhs.AngleIndex - rhs.AngleIndex + 4) % 4);
    }

    static Rotation90()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Rotation90.Deg0 = new Rotation90(0);
      Rotation90.Deg90 = new Rotation90(1);
      Rotation90.Deg180 = new Rotation90(2);
      Rotation90.Deg270 = new Rotation90(3);
      Rotation90.AllRotations = new Rotation90[4]
      {
        Rotation90.Deg0,
        Rotation90.Deg90,
        Rotation90.Deg180,
        Rotation90.Deg270
      };
    }
  }
}
