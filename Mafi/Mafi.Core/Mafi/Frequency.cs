// Decompiled with JetBrains decompiler
// Type: Mafi.Frequency
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
  public readonly struct Frequency : IEquatable<Frequency>, IComparable<Frequency>
  {
    public static readonly Frequency None;
    public static readonly Frequency EveryTick;
    public static readonly Frequency EverySecond;
    public readonly int Ticks;

    public static Frequency EveryNTicks(int n) => new Frequency(n);

    public static Frequency EveryNSeconds(int n) => new Frequency(n.Seconds().Ticks);

    /// <summary>Rounded number of seconds of the frequency</summary>
    public int SecondsFloored => this.Ticks / Duration.OneSecond.Ticks;

    [Pure]
    public bool IsTriggeredAt(Duration duration)
    {
      return this.Ticks != 0 && duration.Ticks % this.Ticks == 0;
    }

    public static Frequency Zero => new Frequency();

    public static Frequency MinValue => new Frequency(int.MinValue);

    public static Frequency MaxValue => new Frequency(int.MaxValue);

    public Frequency(int ticks)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Ticks = ticks;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Frequency Abs => new Frequency(this.Ticks.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Ticks.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public Frequency Min(Frequency rhs) => new Frequency(this.Ticks.Min(rhs.Ticks));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public Frequency Max(Frequency rhs) => new Frequency(this.Ticks.Max(rhs.Ticks));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public Frequency Clamp(Frequency min, Frequency max)
    {
      return new Frequency(this.Ticks.Clamp(min.Ticks, max.Ticks));
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
    public Frequency ScaledBy(Percent scale) => new Frequency(scale.Apply(this.Ticks));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Frequency other, Frequency tolerance)
    {
      return this.Ticks.IsNear(other.Ticks, tolerance.Ticks);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Frequency Lerp(Frequency other, int t, int scale)
    {
      return new Frequency(this.Ticks.Lerp(other.Ticks, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Frequency Lerp(Frequency other, Percent t)
    {
      return new Frequency(this.Ticks.Lerp(other.Ticks, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public Frequency Average(Frequency other) => new Frequency((this.Ticks + other.Ticks) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Ticks * (long) this.Ticks;

    public override string ToString() => this.Ticks.ToString();

    public bool Equals(Frequency other) => this.Ticks == other.Ticks;

    public override bool Equals(object obj) => obj is Frequency other && this.Equals(other);

    public override int GetHashCode() => this.Ticks;

    public int CompareTo(Frequency other) => this.Ticks.CompareTo(other.Ticks);

    public static bool operator ==(Frequency lhs, Frequency rhs) => lhs.Ticks == rhs.Ticks;

    public static bool operator !=(Frequency lhs, Frequency rhs) => lhs.Ticks != rhs.Ticks;

    public static bool operator <(Frequency lhs, Frequency rhs) => lhs.Ticks < rhs.Ticks;

    public static bool operator <=(Frequency lhs, Frequency rhs) => lhs.Ticks <= rhs.Ticks;

    public static bool operator >(Frequency lhs, Frequency rhs) => lhs.Ticks > rhs.Ticks;

    public static bool operator >=(Frequency lhs, Frequency rhs) => lhs.Ticks >= rhs.Ticks;

    public static Frequency operator -(Frequency rhs) => new Frequency(-rhs.Ticks);

    public static Frequency operator +(Frequency lhs, Frequency rhs)
    {
      return new Frequency(lhs.Ticks + rhs.Ticks);
    }

    public static Frequency operator -(Frequency lhs, Frequency rhs)
    {
      return new Frequency(lhs.Ticks - rhs.Ticks);
    }

    public static Frequency operator *(Frequency lhs, int rhs) => new Frequency(lhs.Ticks * rhs);

    public static Frequency operator *(int lhs, Frequency rhs) => new Frequency(lhs * rhs.Ticks);

    public static Frequency operator /(Frequency lhs, int rhs) => new Frequency(lhs.Ticks / rhs);

    public static void Serialize(Frequency value, BlobWriter writer)
    {
      writer.WriteInt(value.Ticks);
    }

    public static Frequency Deserialize(BlobReader reader) => new Frequency(reader.ReadInt());

    static Frequency()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Frequency.None = new Frequency(0);
      Frequency.EveryTick = new Frequency(1);
      Frequency.EverySecond = new Frequency(Duration.OneSecond.Ticks);
    }
  }
}
