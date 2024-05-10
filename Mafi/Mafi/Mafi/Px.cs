// Decompiled with JetBrains decompiler
// Type: Mafi.Px
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
  public readonly struct Px : IEquatable<Px>, IComparable<Px>
  {
    public readonly float Pixels;
    public static readonly Px Auto;
    public const int POINTS_MULTIPLIER = 4;

    public static Px Zero => new Px();

    public static Px MinValue => new Px(float.MinValue);

    public static Px MaxValue => new Px(float.MaxValue);

    public Px(float pixels)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Pixels = pixels;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Px Abs => new Px(this.Pixels.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Pixels.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public Px Min(Px rhs) => new Px(this.Pixels.Min(rhs.Pixels));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public Px Max(Px rhs) => new Px(this.Pixels.Max(rhs.Pixels));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public Px Clamp(Px min, Px max) => new Px(this.Pixels.Clamp(min.Pixels, max.Pixels));

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => (double) this.Pixels == 0.0;

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => (double) this.Pixels != 0.0;

    /// <summary>Whether this value is greater than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => (double) this.Pixels > 0.0;

    /// <summary>Whether this value is less or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => (double) this.Pixels <= 0.0;

    /// <summary>Whether this value is less than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => (double) this.Pixels < 0.0;

    /// <summary>Whether this value is greater or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => (double) this.Pixels >= 0.0;

    [Pure]
    public Px ScaledBy(Percent scale) => new Px(scale.Apply(this.Pixels));

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public Px Average(Px other)
    {
      return new Px((float) (((double) this.Pixels + (double) other.Pixels) / 2.0));
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public float Squared => this.Pixels * this.Pixels;

    public override string ToString() => this.Pixels.ToString();

    public bool Equals(Px other) => (double) this.Pixels == (double) other.Pixels;

    public override bool Equals(object obj) => obj is Px other && this.Equals(other);

    public override int GetHashCode() => this.Pixels.GetHashCode();

    public int CompareTo(Px other) => this.Pixels.CompareTo(other.Pixels);

    public static bool operator ==(Px lhs, Px rhs) => (double) lhs.Pixels == (double) rhs.Pixels;

    public static bool operator !=(Px lhs, Px rhs) => (double) lhs.Pixels != (double) rhs.Pixels;

    public static bool operator <(Px lhs, Px rhs) => (double) lhs.Pixels < (double) rhs.Pixels;

    public static bool operator <=(Px lhs, Px rhs) => (double) lhs.Pixels <= (double) rhs.Pixels;

    public static bool operator >(Px lhs, Px rhs) => (double) lhs.Pixels > (double) rhs.Pixels;

    public static bool operator >=(Px lhs, Px rhs) => (double) lhs.Pixels >= (double) rhs.Pixels;

    public static Px operator -(Px rhs) => new Px(-rhs.Pixels);

    public static Px operator +(Px lhs, Px rhs) => new Px(lhs.Pixels + rhs.Pixels);

    public static Px operator -(Px lhs, Px rhs) => new Px(lhs.Pixels - rhs.Pixels);

    public static Px operator *(Px lhs, float rhs) => new Px(lhs.Pixels * rhs);

    public static Px operator *(float lhs, Px rhs) => new Px(lhs * rhs.Pixels);

    public static Px operator *(Px lhs, int rhs) => new Px(lhs.Pixels * (float) rhs);

    public static Px operator *(int lhs, Px rhs) => new Px((float) lhs * rhs.Pixels);

    public static Px operator /(Px lhs, float rhs) => new Px(lhs.Pixels / rhs);

    public static Px operator /(Px lhs, int rhs) => new Px(lhs.Pixels / (float) rhs);

    public static void Serialize(Px value, BlobWriter writer) => writer.WriteFloat(value.Pixels);

    public static Px Deserialize(BlobReader reader) => new Px(reader.ReadFloat());

    public static implicit operator float(Px v) => v.Pixels;

    static Px()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Px.Auto = Px.MaxValue;
    }
  }
}
