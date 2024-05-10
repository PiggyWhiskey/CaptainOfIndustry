// Decompiled with JetBrains decompiler
// Type: Mafi.Quantity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>Immutable discrete quantity.</summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct Quantity : IEquatable<Quantity>, IComparable<Quantity>
  {
    public readonly int Value;
    public static readonly Quantity One;

    public static Quantity Zero => new Quantity();

    public static Quantity MinValue => new Quantity(int.MinValue);

    public static Quantity MaxValue => new Quantity(int.MaxValue);

    public Quantity(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Quantity Abs => new Quantity(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public Quantity Min(Quantity rhs) => new Quantity(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public Quantity Max(Quantity rhs) => new Quantity(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public Quantity Clamp(Quantity min, Quantity max)
    {
      return new Quantity(this.Value.Clamp(min.Value, max.Value));
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
    public Quantity ScaledBy(Percent scale) => new Quantity(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Quantity other, Quantity tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Quantity Lerp(Quantity other, int t, int scale)
    {
      return new Quantity(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Quantity Lerp(Quantity other, Percent t)
    {
      return new Quantity(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public Quantity Average(Quantity other) => new Quantity((this.Value + other.Value) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString() => this.Value.ToStringCached();

    public bool Equals(Quantity other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is Quantity other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(Quantity other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(Quantity lhs, Quantity rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(Quantity lhs, Quantity rhs) => lhs.Value != rhs.Value;

    public static bool operator <(Quantity lhs, Quantity rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(Quantity lhs, Quantity rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(Quantity lhs, Quantity rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(Quantity lhs, Quantity rhs) => lhs.Value >= rhs.Value;

    public static Quantity operator -(Quantity rhs) => new Quantity(-rhs.Value);

    public static Quantity operator +(Quantity lhs, Quantity rhs)
    {
      return new Quantity(lhs.Value + rhs.Value);
    }

    public static Quantity operator -(Quantity lhs, Quantity rhs)
    {
      return new Quantity(lhs.Value - rhs.Value);
    }

    public static Quantity operator *(Quantity lhs, int rhs) => new Quantity(lhs.Value * rhs);

    public static Quantity operator *(int lhs, Quantity rhs) => new Quantity(lhs * rhs.Value);

    public static Quantity operator /(Quantity lhs, int rhs) => new Quantity(lhs.Value / rhs);

    public static void Serialize(Quantity value, BlobWriter writer) => writer.WriteInt(value.Value);

    public static Quantity Deserialize(BlobReader reader) => new Quantity(reader.ReadInt());

    public QuantityLarge AsLarge => new QuantityLarge((long) this.Value);

    public PartialQuantity AsPartial => new PartialQuantity(this.Value);

    [Pure]
    public Quantity ScaledByCeiled(Percent percent)
    {
      return new Quantity(percent.ApplyCeiled(this.Value));
    }

    [Pure]
    public Quantity InverslyScaledBy(Percent percent)
    {
      return new Quantity(percent.ApplyInverse((Fix32) this.Value).ToIntRounded());
    }

    [Pure]
    public Quantity CeilDiv(int divider) => new Quantity(this.Value.CeilDiv(divider));

    [Pure]
    public Quantity FloorDiv(int divider) => new Quantity(this.Value.FloorDiv(divider));

    [Pure]
    public Quantity RoundDiv(int divider) => new Quantity(this.Value.RoundDiv(divider));

    [Pure]
    public ProductQuantity Of(ProductProto product) => new ProductQuantity(product, this);

    public static Quantity operator %(Quantity lhs, Quantity rhs)
    {
      return new Quantity(lhs.Value % rhs.Value);
    }

    static Quantity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Quantity.One = new Quantity(1);
    }
  }
}
