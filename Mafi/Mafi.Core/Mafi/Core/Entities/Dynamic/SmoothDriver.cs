// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.SmoothDriver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  /// <summary>
  /// Controls speed by smoothly accelerating and braking based on given parameters. This class provides simple API for
  /// controlling speed based on constraints such as maximal braking distance so the vehicle can always stop in time.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class SmoothDriver
  {
    private readonly Fix32 m_maxForwardsSpeedBase;
    private readonly Fix32 m_maxBackwardsSpeedBase;
    private readonly Fix32 m_maxAccelerationBase;
    private readonly Fix32 m_maxBraking;
    private readonly Fix32 m_brakingConservativeness;
    private Fix32 m_maxForwardsSpeed;
    private Fix32 m_maxBackwardsSpeed;
    private Fix32 m_maxAcceleration;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Fix32 Speed { get; private set; }

    public Percent SpeedPercentBase
    {
      get
      {
        return !this.Speed.IsNotNegative ? Percent.FromRatio(this.Speed, this.m_maxBackwardsSpeedBase) : Percent.FromRatio(this.Speed, this.m_maxForwardsSpeedBase);
      }
    }

    public Fix32 LastStepSpeed { get; private set; }

    /// <summary>
    /// Current acceleration, positive when speeding-up, negative when slowing down.
    /// </summary>
    public Fix32 Acceleration => this.Speed - this.LastStepSpeed;

    /// <summary>
    /// Acceleration in percent based on maximum possible speed. Positive when speeding-up, negative when slowing down.
    /// This takes speed factor into account.
    /// </summary>
    public Percent AccelerationPercent
    {
      get
      {
        Fix32 acceleration = this.Acceleration;
        return !acceleration.IsNotNegative ? Percent.FromRatio(acceleration, this.m_maxBraking) : Percent.FromRatio(acceleration, this.m_maxAcceleration);
      }
    }

    /// <summary>
    /// Acceleration in percent based on maximum possible speed. Positive when speeding-up, negative when slowing down.
    /// This does not take speed factor into account, so that absolute values may be larger than 100%.
    /// </summary>
    public Percent AccelerationPercentBase
    {
      get
      {
        Fix32 acceleration = this.Acceleration;
        return !acceleration.IsNotNegative ? Percent.FromRatio(acceleration, this.m_maxBraking) : Percent.FromRatio(acceleration, this.m_maxAccelerationBase);
      }
    }

    public SmoothDriver(
      Fix32 maxForwardsSpeed,
      Fix32 maxBackwardsSpeed,
      Fix32 maxAcceleration,
      Fix32 maxBraking,
      Fix32 brakingConservativeness)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_maxForwardsSpeedBase = maxForwardsSpeed.CheckPositive();
      this.m_maxBackwardsSpeedBase = maxBackwardsSpeed.CheckPositive();
      this.m_maxAccelerationBase = maxAcceleration.CheckPositive();
      this.m_maxBraking = maxBraking.CheckPositive();
      this.m_brakingConservativeness = brakingConservativeness.CheckWithinIncl((Fix32) 1, (Fix32) 10);
      this.SetSpeedFactor(Percent.Hundred);
    }

    public void SetSpeedFactor(Percent speedFactor)
    {
      this.m_maxForwardsSpeed = speedFactor.Apply(this.m_maxForwardsSpeedBase);
      this.m_maxBackwardsSpeed = speedFactor.Apply(this.m_maxBackwardsSpeedBase);
      this.m_maxAcceleration = speedFactor.Apply(this.m_maxAccelerationBase);
    }

    public void SetSpeed(Fix32 value)
    {
      this.Speed = value.Clamp(-this.m_maxBackwardsSpeed, this.m_maxForwardsSpeed);
    }

    public void StartUpdate() => this.LastStepSpeed = this.Speed;

    public void Reset()
    {
      this.Speed = (Fix32) 0;
      this.LastStepSpeed = (Fix32) 0;
    }

    /// <summary>
    /// How many steps of braking will it take to reduce <see cref="P:Mafi.Core.Entities.Dynamic.SmoothDriver.Speed" /> to zero.
    /// </summary>
    public int StepsToFullStop
    {
      get
      {
        Fix32 fix32 = this.Speed;
        fix32 = fix32.Abs() / this.m_maxBraking;
        return fix32.ToIntCeiled();
      }
    }

    /// <summary>
    /// Distance (accumulated speed per tick) that will be passed when we brake each step at 100%. Returned value is
    /// always positive.
    /// </summary>
    /// <remarks>This assumes that we perform braking before accumulation of the speed.</remarks>
    public Fix32 DistanceToFullStop
    {
      get
      {
        Fix32 fix32 = this.Speed.Abs() / this.m_maxBraking;
        int intFloored = fix32.ToIntFloored();
        return (2 * fix32 - (Fix32) 1 - (Fix32) intFloored) * intFloored / 2 * this.m_maxBraking;
      }
    }

    /// <summary>
    /// Performs braking by decreasing magnitude of <see cref="P:Mafi.Core.Entities.Dynamic.SmoothDriver.Speed" /> by given percent.
    /// </summary>
    public void BrakeBy(Percent brakePercent)
    {
      Assert.That<Percent>(brakePercent).IsWithin0To100PercIncl();
      Fix32 fix32 = brakePercent.Apply(this.m_maxBraking);
      this.Speed = this.Speed > 0 ? (this.Speed - fix32).Max((Fix32) 0) : (this.Speed + fix32).Min((Fix32) 0);
    }

    /// <summary>
    /// Tries to keep given speed that will be optionally constrained by total distance traveled.
    /// </summary>
    public void KeepSpeed(Fix32 maxSpeed)
    {
      maxSpeed = maxSpeed.Clamp(-this.m_maxBackwardsSpeed, this.m_maxForwardsSpeed);
      if (maxSpeed < 0)
        this.Speed -= (this.Speed - maxSpeed).Clamp(-this.m_maxBraking, this.m_maxAcceleration);
      else
        this.Speed += (maxSpeed - this.Speed).Clamp(-this.m_maxBraking, this.m_maxAcceleration);
    }

    /// <summary>
    /// Tries to keep given speed that will be additionally constrained by maximum brake distance and optionally by
    /// total distance traveled.
    /// </summary>
    public void KeepSpeed(Fix32 maxSpeed, Fix32 maxBrakeDistance)
    {
      Assert.That<bool>(maxSpeed < 0).IsEqualTo<bool>(maxBrakeDistance < 0);
      maxSpeed = maxSpeed >= 0 ? maxSpeed.Min(this.EstMaxSpeedTo(maxBrakeDistance)) : maxSpeed.Max(-this.EstMaxSpeedTo(-maxBrakeDistance));
      this.KeepSpeed(maxSpeed);
    }

    /// <summary>
    /// Estimates max speed in order to be able to brake in given distance. This estimation is optimistic because it
    /// uses simplified over-estimation of braking distance. The given distance has to be non-negative. Returned
    /// value is always non-negative.
    /// </summary>
    public Fix32 EstMaxSpeedTo(Fix32 brakingDistance)
    {
      Assert.That<Fix32>(brakingDistance).IsNotNegative();
      if (brakingDistance < this.m_maxBraking)
        return brakingDistance;
      Fix32 fix32 = this.m_maxBraking / this.m_brakingConservativeness;
      return (fix32 + fix32.MultAsFix64(8 * brakingDistance + fix32).SqrtToFix32()) / 2;
    }

    public static void Serialize(SmoothDriver value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SmoothDriver>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SmoothDriver.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.LastStepSpeed, writer);
      Fix32.Serialize(this.m_brakingConservativeness, writer);
      Fix32.Serialize(this.m_maxAcceleration, writer);
      Fix32.Serialize(this.m_maxAccelerationBase, writer);
      Fix32.Serialize(this.m_maxBackwardsSpeed, writer);
      Fix32.Serialize(this.m_maxBackwardsSpeedBase, writer);
      Fix32.Serialize(this.m_maxBraking, writer);
      Fix32.Serialize(this.m_maxForwardsSpeed, writer);
      Fix32.Serialize(this.m_maxForwardsSpeedBase, writer);
      Fix32.Serialize(this.Speed, writer);
    }

    public static SmoothDriver Deserialize(BlobReader reader)
    {
      SmoothDriver smoothDriver;
      if (reader.TryStartClassDeserialization<SmoothDriver>(out smoothDriver))
        reader.EnqueueDataDeserialization((object) smoothDriver, SmoothDriver.s_deserializeDataDelayedAction);
      return smoothDriver;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.LastStepSpeed = Fix32.Deserialize(reader);
      reader.SetField<SmoothDriver>(this, "m_brakingConservativeness", (object) Fix32.Deserialize(reader));
      this.m_maxAcceleration = Fix32.Deserialize(reader);
      reader.SetField<SmoothDriver>(this, "m_maxAccelerationBase", (object) Fix32.Deserialize(reader));
      this.m_maxBackwardsSpeed = Fix32.Deserialize(reader);
      reader.SetField<SmoothDriver>(this, "m_maxBackwardsSpeedBase", (object) Fix32.Deserialize(reader));
      reader.SetField<SmoothDriver>(this, "m_maxBraking", (object) Fix32.Deserialize(reader));
      this.m_maxForwardsSpeed = Fix32.Deserialize(reader);
      reader.SetField<SmoothDriver>(this, "m_maxForwardsSpeedBase", (object) Fix32.Deserialize(reader));
      this.Speed = Fix32.Deserialize(reader);
    }

    static SmoothDriver()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SmoothDriver.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SmoothDriver) obj).SerializeData(writer));
      SmoothDriver.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SmoothDriver) obj).DeserializeData(reader));
    }
  }
}
