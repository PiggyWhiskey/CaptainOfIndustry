// Decompiled with JetBrains decompiler
// Type: Mafi.GameDate
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// In-game date representation, does not have a predefined relationship with real time. Precision is one day.
  /// </summary>
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct GameDate : IEquatable<GameDate>, IComparable<GameDate>
  {
    public const int FIRST_YEAR_NUMBER = 1;
    private static string[] s_monthNames;
    public static readonly GameDate Inception;
    public readonly int Value;

    private static string[] MonthNames
    {
      get
      {
        if (GameDate.s_monthNames == null)
        {
          GameDate.s_monthNames = new string[12];
          for (int index = 0; index < 12; ++index)
            GameDate.s_monthNames[index] = new DateTime(1, index + 1, 1).ToString("MMMM", (IFormatProvider) LocalizationManager.CurrentCultureInfo);
        }
        return GameDate.s_monthNames;
      }
    }

    /// <summary>
    /// Day in the month. Value is in [1, RelGameDate.DAYS_PER_MONTH].
    /// </summary>
    public int Day => this.RelGameDate.Days + 1;

    /// <summary>
    /// Month in the year. Value is in [1, RelGameDate.MONTHS_PER_YEAR].
    /// </summary>
    public int Month => this.RelGameDate.Months + 1;

    /// <summary>
    /// Year of the date. Years start with value 1 for first year.
    /// </summary>
    public int Year => this.RelGameDate.Years + 1;

    public RelGameDate RelGameDate => new RelGameDate(this.Value);

    /// <summary>
    /// Rounds the date down to months - year and month stay and day is set to 1.
    /// </summary>
    public GameDate FloorMonths => GameDate.Create(this.Year, this.Month, 1);

    /// <summary>
    /// Rounds the date up to months - year stays the same; if day is zero, month stays, otherwise month is
    /// increased by one; day is set to 1.
    /// </summary>
    public GameDate CeilMonths
    {
      get => GameDate.Create(this.Year, this.Month + (this.RelGameDate.Days != 0 ? 1 : 0), 1);
    }

    public string MonthName => GameDate.MonthNames[this.RelGameDate.Months];

    public static GameDate Create(int year, int month, int day)
    {
      Assert.That<int>(day).IsPositive();
      Assert.That<int>(month).IsPositive();
      Assert.That<int>(year).IsPositive();
      return new GameDate(RelGameDate.Create(year - 1, month - 1, day - 1).Value);
    }

    public static GameDate operator +(GameDate lhs, RelGameDate rhs)
    {
      return new GameDate(lhs.Value + rhs.Value);
    }

    public static GameDate operator +(RelGameDate lhs, GameDate rhs)
    {
      return new GameDate(lhs.Value + rhs.Value);
    }

    public static GameDate operator -(GameDate lhs, RelGameDate rhs)
    {
      return new GameDate(lhs.Value - rhs.Value);
    }

    public static RelGameDate operator -(GameDate lhs, GameDate rhs)
    {
      return new RelGameDate(lhs.Value - rhs.Value);
    }

    public string FormatLong()
    {
      return string.Format("{0} {1} {2:0000}", (object) this.Day, (object) this.MonthName, (object) this.Year);
    }

    public static GameDate Zero => new GameDate();

    public static GameDate MinValue => new GameDate(int.MinValue);

    public static GameDate MaxValue => new GameDate(int.MaxValue);

    public GameDate(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public GameDate Abs => new GameDate(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public GameDate Min(GameDate rhs) => new GameDate(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public GameDate Max(GameDate rhs) => new GameDate(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public GameDate Clamp(GameDate min, GameDate max)
    {
      return new GameDate(this.Value.Clamp(min.Value, max.Value));
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
    public bool IsNear(GameDate other, GameDate tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public GameDate Lerp(GameDate other, int t, int scale)
    {
      return new GameDate(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public GameDate Lerp(GameDate other, Percent t)
    {
      return new GameDate(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public GameDate Average(GameDate other) => new GameDate((this.Value + other.Value) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString()
    {
      return string.Format("{0}/{1}/{2} (D/M/Y)", (object) this.Day, (object) this.Month, (object) this.Year);
    }

    public bool Equals(GameDate other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is GameDate other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(GameDate other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(GameDate lhs, GameDate rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(GameDate lhs, GameDate rhs) => lhs.Value != rhs.Value;

    public static bool operator <(GameDate lhs, GameDate rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(GameDate lhs, GameDate rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(GameDate lhs, GameDate rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(GameDate lhs, GameDate rhs) => lhs.Value >= rhs.Value;

    public static GameDate operator -(GameDate rhs) => new GameDate(-rhs.Value);

    public static void Serialize(GameDate value, BlobWriter writer) => writer.WriteInt(value.Value);

    public static GameDate Deserialize(BlobReader reader) => new GameDate(reader.ReadInt());

    static GameDate()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameDate.Inception = GameDate.Create(1, 1, 1);
    }
  }
}
