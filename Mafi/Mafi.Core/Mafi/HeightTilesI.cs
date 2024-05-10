// Decompiled with JetBrains decompiler
// Type: Mafi.HeightTilesI
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
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct HeightTilesI : IEquatable<HeightTilesI>, IComparable<HeightTilesI>
  {
    public readonly int Value;

    public static HeightTilesI Zero => new HeightTilesI();

    public static HeightTilesI MinValue => new HeightTilesI(int.MinValue);

    public static HeightTilesI MaxValue => new HeightTilesI(int.MaxValue);

    public HeightTilesI(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public HeightTilesI Abs => new HeightTilesI(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public HeightTilesI Min(HeightTilesI rhs) => new HeightTilesI(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public HeightTilesI Max(HeightTilesI rhs) => new HeightTilesI(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public HeightTilesI Clamp(HeightTilesI min, HeightTilesI max)
    {
      return new HeightTilesI(this.Value.Clamp(min.Value, max.Value));
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

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(HeightTilesI other, HeightTilesI tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public HeightTilesI Lerp(HeightTilesI other, int t, int scale)
    {
      return new HeightTilesI(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public HeightTilesI Lerp(HeightTilesI other, Percent t)
    {
      return new HeightTilesI(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public HeightTilesI Average(HeightTilesI other)
    {
      return new HeightTilesI((this.Value + other.Value) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(HeightTilesI other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is HeightTilesI other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(HeightTilesI other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(HeightTilesI lhs, HeightTilesI rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(HeightTilesI lhs, HeightTilesI rhs) => lhs.Value != rhs.Value;

    public static bool operator <(HeightTilesI lhs, HeightTilesI rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(HeightTilesI lhs, HeightTilesI rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(HeightTilesI lhs, HeightTilesI rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(HeightTilesI lhs, HeightTilesI rhs) => lhs.Value >= rhs.Value;

    public static HeightTilesI operator -(HeightTilesI rhs) => new HeightTilesI(-rhs.Value);

    public static void Serialize(HeightTilesI value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static HeightTilesI Deserialize(BlobReader reader) => new HeightTilesI(reader.ReadInt());

    public static HeightTilesI One => new HeightTilesI(1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public HeightTilesF HeightTilesF => new HeightTilesF(this.Value);

    public HeightTilesISlim AsSlim => new HeightTilesISlim((short) this.Value);

    public static HeightTilesI operator +(HeightTilesI lhs, ThicknessTilesI rhs)
    {
      return new HeightTilesI(lhs.Value + rhs.Value);
    }

    public static HeightTilesF operator +(ThicknessTilesI lhs, HeightTilesI rhs)
    {
      return new HeightTilesF(lhs.Value + rhs.Value);
    }

    public static ThicknessTilesI operator -(HeightTilesI lhs, HeightTilesI rhs)
    {
      return new ThicknessTilesI(lhs.Value - rhs.Value);
    }

    public static HeightTilesI operator -(HeightTilesI lhs, ThicknessTilesI rhs)
    {
      return new HeightTilesI(lhs.Value - rhs.Value);
    }
  }
}
