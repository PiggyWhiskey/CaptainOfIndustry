// Decompiled with JetBrains decompiler
// Type: Mafi.RelGameDate
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Represents in-game date interval length. Precision is one day.
  /// TODO: Consolidate with Duration.
  /// </summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct RelGameDate : IEquatable<RelGameDate>, IComparable<RelGameDate>
  {
    public readonly int Value;
    public const int DAYS_PER_MONTH = 30;
    public const int MONTHS_PER_YEAR = 12;
    public const int DAYS_PER_YEAR = 360;
    public static readonly RelGameDate OneDay;
    public static readonly RelGameDate OneMonth;
    public static readonly RelGameDate OneYear;

    public static RelGameDate Zero => new RelGameDate();

    public static RelGameDate MinValue => new RelGameDate(int.MinValue);

    public static RelGameDate MaxValue => new RelGameDate(int.MaxValue);

    public RelGameDate(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelGameDate Abs => new RelGameDate(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public RelGameDate Min(RelGameDate rhs) => new RelGameDate(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public RelGameDate Max(RelGameDate rhs) => new RelGameDate(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public RelGameDate Clamp(RelGameDate min, RelGameDate max)
    {
      return new RelGameDate(this.Value.Clamp(min.Value, max.Value));
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
    public RelGameDate ScaledBy(Percent scale) => new RelGameDate(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(RelGameDate other, RelGameDate tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public RelGameDate Lerp(RelGameDate other, int t, int scale)
    {
      return new RelGameDate(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public RelGameDate Lerp(RelGameDate other, Percent t)
    {
      return new RelGameDate(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public RelGameDate Average(RelGameDate other)
    {
      return new RelGameDate((this.Value + other.Value) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString()
    {
      return string.Format("{0}/{1}/{2} (D/M/Y)", (object) this.Days, (object) this.Months, (object) this.Years);
    }

    public bool Equals(RelGameDate other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is RelGameDate other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(RelGameDate other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(RelGameDate lhs, RelGameDate rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(RelGameDate lhs, RelGameDate rhs) => lhs.Value != rhs.Value;

    public static bool operator <(RelGameDate lhs, RelGameDate rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(RelGameDate lhs, RelGameDate rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(RelGameDate lhs, RelGameDate rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(RelGameDate lhs, RelGameDate rhs) => lhs.Value >= rhs.Value;

    public static RelGameDate operator -(RelGameDate rhs) => new RelGameDate(-rhs.Value);

    public static RelGameDate operator +(RelGameDate lhs, RelGameDate rhs)
    {
      return new RelGameDate(lhs.Value + rhs.Value);
    }

    public static RelGameDate operator -(RelGameDate lhs, RelGameDate rhs)
    {
      return new RelGameDate(lhs.Value - rhs.Value);
    }

    public static RelGameDate operator *(RelGameDate lhs, int rhs)
    {
      return new RelGameDate(lhs.Value * rhs);
    }

    public static RelGameDate operator *(int lhs, RelGameDate rhs)
    {
      return new RelGameDate(lhs * rhs.Value);
    }

    public static RelGameDate operator /(RelGameDate lhs, int rhs)
    {
      return new RelGameDate(lhs.Value / rhs);
    }

    public static void Serialize(RelGameDate value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static RelGameDate Deserialize(BlobReader reader) => new RelGameDate(reader.ReadInt());

    /// <summary>Interval length in days.</summary>
    public int TotalDays => this.Value;

    /// <summary>Interval length in months floored.</summary>
    public int TotalMonthsFloored => this.Value / 30;

    public int TotalMonthsRounded => (this.Value.ToFix32() / 30).ToIntRounded();

    /// <summary>
    /// Remaining days of the interval length after months and years are subtracted. Value is in [0, DAYS_PER_MONTH).
    /// </summary>
    public int Days => this.TotalDays % 30;

    /// <summary>
    /// Remaining months of the interval length after years are subtracted. Value is in [0, MONTHS_PER_YEAR).
    /// </summary>
    public int Months => this.TotalMonthsFloored % 12;

    /// <summary>Number of years of the interval floored.</summary>
    public int Years => this.Value / 360;

    public static RelGameDate Create(int years, int months, int days)
    {
      Assert.That<int>(days).IsNotNegative();
      Assert.That<int>(months).IsNotNegative();
      Assert.That<int>(years).IsNotNegative();
      return new RelGameDate(days + 30 * (months + 12 * years));
    }

    public static RelGameDate FromDays(int days) => RelGameDate.Create(0, 0, days);

    public static RelGameDate FromMonths(int months) => RelGameDate.Create(0, months, 0);

    public static RelGameDate FromYears(int years) => RelGameDate.Create(years, 0, 0);

    [Pure]
    public RelGameDate FloorToMonths() => RelGameDate.Create(this.Years, this.Months, 0);

    [Pure]
    public LocStrFormatted FormatYearsOrMonthsLong()
    {
      return this.Years >= 2 ? TrCore.NumberOfYears.Format(this.Years) : TrCore.NumberOfMonths.Format(this.TotalMonthsFloored);
    }

    static RelGameDate()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RelGameDate.OneDay = RelGameDate.Create(0, 0, 1);
      RelGameDate.OneMonth = RelGameDate.Create(0, 1, 0);
      RelGameDate.OneYear = RelGameDate.Create(1, 0, 0);
    }
  }
}
