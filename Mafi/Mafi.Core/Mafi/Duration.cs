// Decompiled with JetBrains decompiler
// Type: Mafi.Duration
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
  /// <summary>Immutable game duration in sim ticks.</summary>
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct Duration : IEquatable<Duration>, IComparable<Duration>
  {
    public const int TICKS_PER_YEAR = 7200;
    public static readonly Duration OneTick;
    public static readonly Duration OneSecond;
    public static readonly Duration OneMinute;
    public static readonly Duration OneDay;
    public static readonly Duration OneMonth;
    public static readonly Duration OneYear;
    public readonly int Ticks;

    public Fix64 Millis => this.Ticks.ToFix64() * 100;

    public Fix64 Seconds => this.Millis / 1000;

    public int SecondsFloored => (int) ((long) this.Ticks * 100L / 1000L);

    public Fix64 Minutes => this.Millis / 60000;

    public Fix32 Days => this.Ticks.ToFix32() / 20;

    public Fix32 Months => this.Days / 30;

    public Fix32 Years => this.Months / 12;

    public static Duration FromTicks(int ticks) => new Duration(ticks);

    public static Duration FromDays(int days) => new Duration(20 * days);

    public static Duration FromMonths(int months) => new Duration(600 * months);

    public static Duration FromYears(int years) => new Duration(7200 * years);

    public static Duration FromYears(double years) => new Duration((7200.0 * years).RoundToInt());

    public static Duration FromMin(double minutes) => Duration.FromSec(minutes * 60.0);

    public static Duration FromSec(int seconds)
    {
      Assert.That<int>(seconds).IsNotNegative();
      int ticks = seconds * 1000 / 100;
      return ticks >= 0 ? new Duration(ticks) : Duration.Zero;
    }

    public static Duration FromSec(double seconds)
    {
      Assert.That<double>(seconds).IsNotNegative();
      int ticks = (seconds * 1000.0 / 100.0).RoundToInt();
      return ticks >= 0 ? new Duration(ticks) : Duration.Zero;
    }

    public static Duration FromMs(Fix32 milliseconds)
    {
      Assert.That<Fix32>(milliseconds).IsNotNegative();
      int intRounded = (milliseconds / 100).ToIntRounded();
      return intRounded >= 0 ? new Duration(intRounded) : Duration.Zero;
    }

    /// <summary>
    /// Creates duration from unity key frames count. This assumes that Unity animations runs at <see cref="F:Mafi.Duration.UNITY_KEYFRAMES_PER_SEC" /> fps (usually 30). The duration is always rounded up so that animations are
    /// never cut short. Note that animation FPS and game FPS are independent.
    /// </summary>
    public static Duration FromKeyframes(int keyframes)
    {
      Assert.That<int>(keyframes).IsNotNegative();
      int ticks = ((float) ((double) keyframes * 1000.0 / 3000.0)).CeilToInt();
      return ticks >= 0 ? new Duration(ticks) : Duration.Zero;
    }

    /// <summary>
    /// Same as <see cref="M:Mafi.Duration.FromKeyframes(System.Int32)" /> but also applies the given speed.
    /// </summary>
    public static Duration FromKeyframes(int keyframes, float speed)
    {
      return Duration.FromKeyframes(((float) keyframes / speed).CeilToInt());
    }

    /// <summary>
    /// Calculates the speed needed to be applied on the current duration to achieve the given one.
    /// </summary>
    public float SpeedToMatch(Duration duration) => (float) this.Ticks / (float) duration.Ticks;

    [Pure]
    public string Format() => this.Seconds.ToStringRounded() + " sec";

    [Pure]
    public string FormatShort()
    {
      return this.Seconds == 1 ? "sec" : this.Seconds.ToStringRounded() + " sec";
    }

    [Pure]
    public string ToCalendarDurationStr()
    {
      int num1 = this.Ticks / 20;
      if (num1 == 0)
        return "<1 day";
      int num2 = num1 / 30;
      if (num2 == 0)
        return string.Format("{0} day{1}", (object) num1, num1 == 1 ? (object) "" : (object) "s");
      int num3 = num1 - num2 * 30;
      int num4 = num2 / 12;
      if (num4 == 0)
        return string.Format("{0} month{1}, {2} day{3}", (object) num2, num2 == 1 ? (object) "" : (object) "s", (object) num3, num3 == 1 ? (object) "" : (object) "s");
      int num5 = num2 - num4 * 12;
      return string.Format("{0} year{1}, {2} month{3}, {4} day{5}", (object) num4, num4 == 1 ? (object) "" : (object) "s", (object) num5, num5 == 1 ? (object) "" : (object) "s", (object) num3, num3 == 1 ? (object) "" : (object) "s");
    }

    public static Duration Zero => new Duration();

    public static Duration MinValue => new Duration(int.MinValue);

    public static Duration MaxValue => new Duration(int.MaxValue);

    public Duration(int ticks)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Ticks = ticks;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Duration Abs => new Duration(this.Ticks.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Ticks.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public Duration Min(Duration rhs) => new Duration(this.Ticks.Min(rhs.Ticks));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public Duration Max(Duration rhs) => new Duration(this.Ticks.Max(rhs.Ticks));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public Duration Clamp(Duration min, Duration max)
    {
      return new Duration(this.Ticks.Clamp(min.Ticks, max.Ticks));
    }

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.Ticks == 0;

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.Ticks != 0;

    /// <summary>Whether this value is greater than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => this.Ticks > 0;

    /// <summary>Whether this value is less or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => this.Ticks <= 0;

    /// <summary>Whether this value is less than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => this.Ticks < 0;

    /// <summary>Whether this value is greater or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => this.Ticks >= 0;

    [Pure]
    public Duration ScaledBy(Percent scale) => new Duration(scale.Apply(this.Ticks));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Duration other, Duration tolerance)
    {
      return this.Ticks.IsNear(other.Ticks, tolerance.Ticks);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Duration Lerp(Duration other, int t, int scale)
    {
      return new Duration(this.Ticks.Lerp(other.Ticks, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Duration Lerp(Duration other, Percent t)
    {
      return new Duration(this.Ticks.Lerp(other.Ticks, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public Duration Average(Duration other) => new Duration((this.Ticks + other.Ticks) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Ticks * (long) this.Ticks;

    public override string ToString() => this.Ticks.ToString();

    public bool Equals(Duration other) => this.Ticks == other.Ticks;

    public override bool Equals(object obj) => obj is Duration other && this.Equals(other);

    public override int GetHashCode() => this.Ticks;

    public int CompareTo(Duration other) => this.Ticks.CompareTo(other.Ticks);

    public static bool operator ==(Duration lhs, Duration rhs) => lhs.Ticks == rhs.Ticks;

    public static bool operator !=(Duration lhs, Duration rhs) => lhs.Ticks != rhs.Ticks;

    public static bool operator <(Duration lhs, Duration rhs) => lhs.Ticks < rhs.Ticks;

    public static bool operator <=(Duration lhs, Duration rhs) => lhs.Ticks <= rhs.Ticks;

    public static bool operator >(Duration lhs, Duration rhs) => lhs.Ticks > rhs.Ticks;

    public static bool operator >=(Duration lhs, Duration rhs) => lhs.Ticks >= rhs.Ticks;

    public static Duration operator -(Duration rhs) => new Duration(-rhs.Ticks);

    public static Duration operator +(Duration lhs, Duration rhs)
    {
      return new Duration(lhs.Ticks + rhs.Ticks);
    }

    public static Duration operator -(Duration lhs, Duration rhs)
    {
      return new Duration(lhs.Ticks - rhs.Ticks);
    }

    public static Duration operator *(Duration lhs, int rhs) => new Duration(lhs.Ticks * rhs);

    public static Duration operator *(int lhs, Duration rhs) => new Duration(lhs * rhs.Ticks);

    public static Duration operator /(Duration lhs, int rhs) => new Duration(lhs.Ticks / rhs);

    public static void Serialize(Duration value, BlobWriter writer) => writer.WriteInt(value.Ticks);

    public static Duration Deserialize(BlobReader reader) => new Duration(reader.ReadInt());

    static Duration()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Duration.OneTick = new Duration(1);
      Duration.OneSecond = Duration.FromSec(1);
      Duration.OneMinute = Duration.FromMin(1.0);
      Duration.OneDay = Duration.FromDays(1);
      Duration.OneMonth = Duration.FromMonths(1);
      Duration.OneYear = Duration.FromYears(1);
    }
  }
}
