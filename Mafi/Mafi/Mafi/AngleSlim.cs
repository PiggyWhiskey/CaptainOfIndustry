// Decompiled with JetBrains decompiler
// Type: Mafi.AngleSlim
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
  /// <summary>
  /// Angle represented as ushort where the entire range represents one rotation.
  /// 0 is zero degrees, (ushort.MaxValue + 1) / 2 is 180 degrees, and (ushort.MaxValue + 1) is 360 or 0.
  /// Thanks to this representation the angle is always normalized.
  /// </summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct AngleSlim : IEquatable<AngleSlim>, IComparable<AngleSlim>
  {
    public readonly ushort Raw;

    public Fix32 Degrees => Fix32.FromFraction((long) ((int) this.Raw * 360), 65536L);

    public float RadiansAsFloat => (float) ((double) this.Raw * 6.2831854820251465 / 65536.0);

    private AngleSlim(ushort raw)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Raw = raw;
    }

    public static AngleSlim FromRaw(ushort raw) => new AngleSlim(raw);

    public static AngleSlim FromAngleDegrees(AngleDegrees1f angle)
    {
      return new AngleSlim((ushort) ((ulong) angle.Degrees.MultAsFix64((Fix32) 65536).IntegerPart / 360UL));
    }

    public AngleDegrees1f ToAngleDegrees1f() => AngleDegrees1f.FromDegrees(this.Degrees);

    public float CosAsFloat() => MafiMath.Cos(this.RadiansAsFloat);

    public float SinAsFloat() => MafiMath.Sin(this.RadiansAsFloat);

    public static AngleSlim operator -(AngleSlim rhs)
    {
      return new AngleSlim((ushort) (65536U - (uint) rhs.Raw));
    }

    public static AngleSlim Zero => new AngleSlim();

    public static AngleSlim MinValue => new AngleSlim((ushort) 0);

    public static AngleSlim MaxValue => new AngleSlim(ushort.MaxValue);

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public AngleSlim Min(AngleSlim rhs) => new AngleSlim(this.Raw.Min(rhs.Raw));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public AngleSlim Max(AngleSlim rhs) => new AngleSlim(this.Raw.Max(rhs.Raw));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public AngleSlim Clamp(AngleSlim min, AngleSlim max)
    {
      return new AngleSlim(this.Raw.Clamp(min.Raw, max.Raw));
    }

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.Raw == (ushort) 0;

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.Raw > (ushort) 0;

    /// <summary>Whether this value is greater than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => this.Raw > (ushort) 0;

    /// <summary>Whether this value is less or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => this.Raw <= (ushort) 0;

    /// <summary>Whether this value is less than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => this.Raw < (ushort) 0;

    /// <summary>Whether this value is greater or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => this.Raw >= (ushort) 0;

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public AngleSlim Average(AngleSlim other)
    {
      return new AngleSlim((ushort) (((int) this.Raw + (int) other.Raw) / 2));
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Squared => (int) this.Raw * (int) this.Raw;

    public override string ToString() => string.Format("{0}°", (object) this.Degrees);

    public bool Equals(AngleSlim other) => (int) this.Raw == (int) other.Raw;

    public override bool Equals(object obj) => obj is AngleSlim other && this.Equals(other);

    public override int GetHashCode() => this.Raw.GetHashCode();

    public int CompareTo(AngleSlim other) => this.Raw.CompareTo(other.Raw);

    public static bool operator ==(AngleSlim lhs, AngleSlim rhs) => (int) lhs.Raw == (int) rhs.Raw;

    public static bool operator !=(AngleSlim lhs, AngleSlim rhs) => (int) lhs.Raw != (int) rhs.Raw;

    public static bool operator <(AngleSlim lhs, AngleSlim rhs) => (int) lhs.Raw < (int) rhs.Raw;

    public static bool operator <=(AngleSlim lhs, AngleSlim rhs) => (int) lhs.Raw <= (int) rhs.Raw;

    public static bool operator >(AngleSlim lhs, AngleSlim rhs) => (int) lhs.Raw > (int) rhs.Raw;

    public static bool operator >=(AngleSlim lhs, AngleSlim rhs) => (int) lhs.Raw >= (int) rhs.Raw;

    public static void Serialize(AngleSlim value, BlobWriter writer)
    {
      writer.WriteUShort(value.Raw);
    }

    public static AngleSlim Deserialize(BlobReader reader) => new AngleSlim(reader.ReadUShort());
  }
}
