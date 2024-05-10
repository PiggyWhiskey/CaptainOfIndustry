// Decompiled with JetBrains decompiler
// Type: Mafi.SimStep
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
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct SimStep : IEquatable<SimStep>, IComparable<SimStep>
  {
    public readonly int Value;
    public static readonly SimStep One;
    public const int STEPS_PER_SECOND = 10;
    public const float SECONDS_PER_STEP = 0.1f;

    public static SimStep Zero => new SimStep();

    public static SimStep MinValue => new SimStep(int.MinValue);

    public static SimStep MaxValue => new SimStep(int.MaxValue);

    public SimStep(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public SimStep Abs => new SimStep(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public SimStep Min(SimStep rhs) => new SimStep(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public SimStep Max(SimStep rhs) => new SimStep(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public SimStep Clamp(SimStep min, SimStep max)
    {
      return new SimStep(this.Value.Clamp(min.Value, max.Value));
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
    public bool IsNear(SimStep other, SimStep tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public SimStep Lerp(SimStep other, int t, int scale)
    {
      return new SimStep(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public SimStep Lerp(SimStep other, Percent t) => new SimStep(this.Value.Lerp(other.Value, t));

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public SimStep Average(SimStep other) => new SimStep((this.Value + other.Value) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(SimStep other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is SimStep other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(SimStep other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(SimStep lhs, SimStep rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(SimStep lhs, SimStep rhs) => lhs.Value != rhs.Value;

    public static bool operator <(SimStep lhs, SimStep rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(SimStep lhs, SimStep rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(SimStep lhs, SimStep rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(SimStep lhs, SimStep rhs) => lhs.Value >= rhs.Value;

    public static SimStep operator -(SimStep rhs) => new SimStep(-rhs.Value);

    public static void Serialize(SimStep value, BlobWriter writer) => writer.WriteInt(value.Value);

    public static SimStep Deserialize(BlobReader reader) => new SimStep(reader.ReadInt());

    public static SimStep operator +(SimStep lhs, SimStep rhs)
    {
      return new SimStep(lhs.Value + rhs.Value);
    }

    public static SimStep operator +(SimStep lhs, Duration rhs)
    {
      return new SimStep(lhs.Value + rhs.Ticks);
    }

    public static Duration operator -(SimStep lhs, SimStep rhs)
    {
      return new Duration(lhs.Value - rhs.Value);
    }

    public static SimStep operator -(SimStep lhs, Duration rhs)
    {
      return new SimStep(lhs.Value - rhs.Ticks);
    }

    public static SimStep operator *(SimStep lhs, int rhs) => new SimStep(lhs.Value * rhs);

    public static SimStep operator *(int lhs, SimStep rhs) => new SimStep(lhs * rhs.Value);

    public static SimStep operator /(SimStep lhs, int rhs) => new SimStep(lhs.Value / rhs);

    public static Duration operator %(SimStep lhs, Duration rhs)
    {
      return new Duration(lhs.Value % rhs.Ticks);
    }

    static SimStep()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SimStep.One = new SimStep(1);
    }
  }
}
