// Decompiled with JetBrains decompiler
// Type: Mafi.AngleDegrees1f
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
  /// <summary>Angle in degrees represented as Fix32 value.</summary>
  /// <remarks>
  /// We represent this value as degrees to allow exact representation of common angles like 90 or 180 degrees.
  /// </remarks>
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct AngleDegrees1f : IEquatable<AngleDegrees1f>, IComparable<AngleDegrees1f>
  {
    public static readonly AngleDegrees1f HalfDegree;
    public static readonly AngleDegrees1f OneDegree;
    public static readonly AngleDegrees1f Deg90;
    public static readonly AngleDegrees1f Deg179;
    public static readonly AngleDegrees1f Deg180;
    public static readonly AngleDegrees1f Deg270;
    public static readonly AngleDegrees1f Deg360;
    public readonly Fix32 Degrees;

    private AngleDegrees1f(Fix32 degrees)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Degrees = degrees;
    }

    public static AngleDegrees1f FromDegrees(int degrees)
    {
      return new AngleDegrees1f(Fix32.FromInt(degrees));
    }

    public static AngleDegrees1f FromDegrees(Fix32 degrees) => new AngleDegrees1f(degrees);

    public static AngleDegrees1f FromRadians(Fix32 radians)
    {
      return new AngleDegrees1f(radians * 360 / Fix32.Tau);
    }

    /// <summary>Returns a Fix32 value representing angle in radians.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Radians => this.Degrees * Fix32.Tau / 360;

    /// <summary>
    /// Returns normalized angle that is within [0, 360) degrees.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AngleDegrees1f Normalized
    {
      get => AngleDegrees1f.FromDegrees(this.Degrees.Modulo((Fix32) 360));
    }

    /// <summary>
    /// Returns normalized direction of this angle. This is essentially unit complex number representing this angle.
    /// </summary>
    public Vector2f DirectionVector
    {
      get
      {
        Fix64 fix64 = this.Cos();
        Fix32 fix32_1 = fix64.ToFix32();
        fix64 = this.Sin();
        Fix32 fix32_2 = fix64.ToFix32();
        return new Vector2f(fix32_1, fix32_2);
      }
    }

    public AngleDegrees1f Rotated180Deg => this + AngleDegrees1f.Deg180;

    /// <summary>
    /// More precise than scaling <see cref="P:Mafi.AngleDegrees1f.DirectionVector" />.
    /// </summary>
    public Vector2f DirectionVectorOfSize(Fix32 size)
    {
      Fix64 fix64 = this.Cos() * size;
      Fix32 fix32_1 = fix64.ToFix32();
      fix64 = this.Sin() * size;
      Fix32 fix32_2 = fix64.ToFix32();
      return new Vector2f(fix32_1, fix32_2);
    }

    [Pure]
    public Fix64 Cos() => Fix64.FromDouble(Math.Cos(this.Degrees.ToDouble() * (Math.PI / 180.0)));

    [Pure]
    public Fix64 Sin() => Fix64.FromDouble(Math.Sin(this.Degrees.ToDouble() * (Math.PI / 180.0)));

    [Pure]
    public Fix64 Tan() => Fix64.FromDouble(Math.Tan(this.Degrees.ToDouble() * (Math.PI / 180.0)));

    /// <summary>
    /// Returns signed shortest-arc angle to other given angle. Unlike the <c>-</c> operator this takes into
    /// account the singularity at 0.
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleTo(AngleDegrees1f other)
    {
      AngleDegrees1f normalized = (other - this).Normalized;
      if (normalized >= AngleDegrees1f.Deg180)
        normalized -= AngleDegrees1f.Deg360;
      Assert.That<AngleDegrees1f>(normalized).IsWithinExcl<AngleDegrees1f>(-AngleDegrees1f.Deg180, AngleDegrees1f.Deg180);
      return normalized;
    }

    /// <summary>
    /// Whether this and given values are within given tolerance. This takes into account the singularity at 0.
    /// </summary>
    [Pure]
    public bool IsNear(AngleDegrees1f other)
    {
      AngleDegrees1f angleDegrees1f = (this - other).Normalized;
      if (angleDegrees1f > AngleDegrees1f.Deg180)
        angleDegrees1f = AngleDegrees1f.Deg360 - angleDegrees1f;
      Assert.That<AngleDegrees1f>(angleDegrees1f).IsGreaterOrEqual<AngleDegrees1f>(AngleDegrees1f.Zero);
      return angleDegrees1f.Degrees <= Fix32.EpsilonNear;
    }

    /// <summary>
    /// Whether this and given values are within given tolerance. This takes into account the singularity at 0.
    /// </summary>
    [Pure]
    public bool IsNear(AngleDegrees1f other, AngleDegrees1f tolerance)
    {
      Assert.That<AngleDegrees1f>(tolerance).IsWithinExcl<AngleDegrees1f>(AngleDegrees1f.Zero, AngleDegrees1f.Deg360);
      AngleDegrees1f angleDegrees1f = (this - other).Normalized;
      if (angleDegrees1f > AngleDegrees1f.Deg180)
        angleDegrees1f = AngleDegrees1f.Deg360 - angleDegrees1f;
      Assert.That<AngleDegrees1f>(angleDegrees1f).IsGreaterOrEqual<AngleDegrees1f>(AngleDegrees1f.Zero);
      return angleDegrees1f.Degrees <= tolerance.Degrees;
    }

    [Pure]
    public AngleSlim ToSlim() => AngleSlim.FromAngleDegrees(this);

    public override string ToString()
    {
      return this == this.Normalized ? string.Format("{0}°", (object) this.Degrees) : string.Format("{0}° ({1}°)", (object) this.Degrees, (object) this.Normalized.Degrees);
    }

    public static AngleDegrees1f Zero => new AngleDegrees1f();

    public static AngleDegrees1f MinValue => new AngleDegrees1f(Fix32.MinValue);

    public static AngleDegrees1f MaxValue => new AngleDegrees1f(Fix32.MaxValue);

    public static AngleDegrees1f Epsilon => new AngleDegrees1f(Fix32.Epsilon);

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AngleDegrees1f Abs => new AngleDegrees1f(this.Degrees.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Degrees.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public AngleDegrees1f Min(AngleDegrees1f rhs)
    {
      return new AngleDegrees1f(this.Degrees.Min(rhs.Degrees));
    }

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public AngleDegrees1f Max(AngleDegrees1f rhs)
    {
      return new AngleDegrees1f(this.Degrees.Max(rhs.Degrees));
    }

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public AngleDegrees1f Clamp(AngleDegrees1f min, AngleDegrees1f max)
    {
      return new AngleDegrees1f(this.Degrees.Clamp(min.Degrees, max.Degrees));
    }

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.Degrees == 0;

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.Degrees != 0;

    /// <summary>Whether this value is greater than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => this.Degrees > 0;

    /// <summary>Whether this value is less or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => this.Degrees <= 0;

    /// <summary>Whether this value is less than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => this.Degrees < 0;

    /// <summary>Whether this value is greater or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => this.Degrees >= 0;

    [Pure]
    public AngleDegrees1f ScaledBy(Percent scale) => new AngleDegrees1f(scale.Apply(this.Degrees));

    [Pure]
    public AngleDegrees1f Lerp(AngleDegrees1f other, Percent t)
    {
      return new AngleDegrees1f(this.Degrees.Lerp(other.Degrees, t));
    }

    [Pure]
    public AngleDegrees1f Lerp(AngleDegrees1f other, Fix32 t)
    {
      return new AngleDegrees1f(this.Degrees.Lerp(other.Degrees, t));
    }

    [Pure]
    public AngleDegrees1f Lerp(AngleDegrees1f other, Fix32 t, Fix32 scale)
    {
      return new AngleDegrees1f(this.Degrees.Lerp(other.Degrees, t, scale));
    }

    [Pure]
    public AngleDegrees1f SmoothStep(AngleDegrees1f other, Fix32 t)
    {
      return new AngleDegrees1f(this.Degrees.SmoothStep(other.Degrees, t));
    }

    [Pure]
    public AngleDegrees1f EaseIn(AngleDegrees1f other, Fix32 t)
    {
      return new AngleDegrees1f(this.Degrees.EaseIn(other.Degrees, t));
    }

    [Pure]
    public AngleDegrees1f EaseOut(AngleDegrees1f other, Fix32 t)
    {
      return new AngleDegrees1f(this.Degrees.EaseOut(other.Degrees, t));
    }

    [Pure]
    public AngleDegrees1f EaseInOut(AngleDegrees1f other, Fix32 t)
    {
      return new AngleDegrees1f(this.Degrees.EaseInOut(other.Degrees, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public AngleDegrees1f Average(AngleDegrees1f other)
    {
      return new AngleDegrees1f((this.Degrees + other.Degrees) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Squared => this.Degrees.MultAsFix64(this.Degrees);

    public bool Equals(AngleDegrees1f other) => this.Degrees == other.Degrees;

    public override bool Equals(object obj) => obj is AngleDegrees1f other && this.Equals(other);

    public override int GetHashCode() => this.Degrees.GetHashCode();

    public int CompareTo(AngleDegrees1f other) => this.Degrees.CompareTo(other.Degrees);

    public static bool operator ==(AngleDegrees1f lhs, AngleDegrees1f rhs)
    {
      return lhs.Degrees == rhs.Degrees;
    }

    public static bool operator !=(AngleDegrees1f lhs, AngleDegrees1f rhs)
    {
      return lhs.Degrees != rhs.Degrees;
    }

    public static bool operator <(AngleDegrees1f lhs, AngleDegrees1f rhs)
    {
      return lhs.Degrees < rhs.Degrees;
    }

    public static bool operator <=(AngleDegrees1f lhs, AngleDegrees1f rhs)
    {
      return lhs.Degrees <= rhs.Degrees;
    }

    public static bool operator >(AngleDegrees1f lhs, AngleDegrees1f rhs)
    {
      return lhs.Degrees > rhs.Degrees;
    }

    public static bool operator >=(AngleDegrees1f lhs, AngleDegrees1f rhs)
    {
      return lhs.Degrees >= rhs.Degrees;
    }

    public static AngleDegrees1f operator -(AngleDegrees1f rhs) => new AngleDegrees1f(-rhs.Degrees);

    public static AngleDegrees1f operator +(AngleDegrees1f lhs, AngleDegrees1f rhs)
    {
      return new AngleDegrees1f(lhs.Degrees + rhs.Degrees);
    }

    public static AngleDegrees1f operator -(AngleDegrees1f lhs, AngleDegrees1f rhs)
    {
      return new AngleDegrees1f(lhs.Degrees - rhs.Degrees);
    }

    public static AngleDegrees1f operator *(AngleDegrees1f lhs, Fix32 rhs)
    {
      return new AngleDegrees1f(lhs.Degrees * rhs);
    }

    public static AngleDegrees1f operator *(Fix32 lhs, AngleDegrees1f rhs)
    {
      return new AngleDegrees1f(lhs * rhs.Degrees);
    }

    public static AngleDegrees1f operator *(AngleDegrees1f lhs, int rhs)
    {
      return new AngleDegrees1f(lhs.Degrees * rhs);
    }

    public static AngleDegrees1f operator *(int lhs, AngleDegrees1f rhs)
    {
      return new AngleDegrees1f(lhs * rhs.Degrees);
    }

    public static AngleDegrees1f operator /(AngleDegrees1f lhs, Fix32 rhs)
    {
      return new AngleDegrees1f(lhs.Degrees / rhs);
    }

    public static AngleDegrees1f operator /(AngleDegrees1f lhs, int rhs)
    {
      return new AngleDegrees1f(lhs.Degrees / rhs);
    }

    public static void Serialize(AngleDegrees1f value, BlobWriter writer)
    {
      Fix32.Serialize(value.Degrees, writer);
    }

    public static AngleDegrees1f Deserialize(BlobReader reader)
    {
      return new AngleDegrees1f(Fix32.Deserialize(reader));
    }

    static AngleDegrees1f()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      AngleDegrees1f.HalfDegree = AngleDegrees1f.FromDegrees(Fix32.Half);
      AngleDegrees1f.OneDegree = AngleDegrees1f.FromDegrees(Fix32.One);
      AngleDegrees1f.Deg90 = AngleDegrees1f.FromDegrees(90);
      AngleDegrees1f.Deg179 = AngleDegrees1f.FromDegrees(179);
      AngleDegrees1f.Deg180 = AngleDegrees1f.FromDegrees(180);
      AngleDegrees1f.Deg270 = AngleDegrees1f.FromDegrees(270);
      AngleDegrees1f.Deg360 = AngleDegrees1f.FromDegrees(360);
    }
  }
}
