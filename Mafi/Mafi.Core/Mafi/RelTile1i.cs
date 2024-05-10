// Decompiled with JetBrains decompiler
// Type: Mafi.RelTile1i
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>Immutable 1D relative tile coordinate or distance.</summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct RelTile1i : IEquatable<RelTile1i>, IComparable<RelTile1i>
  {
    public readonly int Value;
    public static readonly RelTile1i One;

    public static RelTile1i Zero => new RelTile1i();

    public static RelTile1i MinValue => new RelTile1i(int.MinValue);

    public static RelTile1i MaxValue => new RelTile1i(int.MaxValue);

    public RelTile1i(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile1i Abs => new RelTile1i(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public RelTile1i Min(RelTile1i rhs) => new RelTile1i(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public RelTile1i Max(RelTile1i rhs) => new RelTile1i(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public RelTile1i Clamp(RelTile1i min, RelTile1i max)
    {
      return new RelTile1i(this.Value.Clamp(min.Value, max.Value));
    }

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.Value == 0;

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.Value != 0;

    /// <summary>Whether this value is greater than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => this.Value > 0;

    /// <summary>Whether this value is less or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => this.Value <= 0;

    /// <summary>Whether this value is less than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => this.Value < 0;

    /// <summary>Whether this value is greater or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => this.Value >= 0;

    [Pure]
    public RelTile1i ScaledBy(Percent scale) => new RelTile1i(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(RelTile1i other, RelTile1i tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public RelTile1i Lerp(RelTile1i other, int t, int scale)
    {
      return new RelTile1i(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public RelTile1i Lerp(RelTile1i other, Percent t)
    {
      return new RelTile1i(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public RelTile1i Average(RelTile1i other) => new RelTile1i((this.Value + other.Value) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(RelTile1i other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is RelTile1i other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(RelTile1i other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(RelTile1i lhs, RelTile1i rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(RelTile1i lhs, RelTile1i rhs) => lhs.Value != rhs.Value;

    public static bool operator <(RelTile1i lhs, RelTile1i rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(RelTile1i lhs, RelTile1i rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(RelTile1i lhs, RelTile1i rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(RelTile1i lhs, RelTile1i rhs) => lhs.Value >= rhs.Value;

    public static RelTile1i operator -(RelTile1i rhs) => new RelTile1i(-rhs.Value);

    public static RelTile1i operator +(RelTile1i lhs, RelTile1i rhs)
    {
      return new RelTile1i(lhs.Value + rhs.Value);
    }

    public static RelTile1i operator -(RelTile1i lhs, RelTile1i rhs)
    {
      return new RelTile1i(lhs.Value - rhs.Value);
    }

    public static RelTile1i operator *(RelTile1i lhs, int rhs) => new RelTile1i(lhs.Value * rhs);

    public static RelTile1i operator *(int lhs, RelTile1i rhs) => new RelTile1i(lhs * rhs.Value);

    public static RelTile1i operator /(RelTile1i lhs, int rhs) => new RelTile1i(lhs.Value / rhs);

    public static void Serialize(RelTile1i value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static RelTile1i Deserialize(BlobReader reader) => new RelTile1i(reader.ReadInt());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile1f RelTile1f => new RelTile1f(this.Value);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Meters => this.Value * 2;

    static RelTile1i()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RelTile1i.One = new RelTile1i(1);
    }
  }
}
